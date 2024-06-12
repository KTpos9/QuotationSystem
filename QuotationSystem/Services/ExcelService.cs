using OfficeOpenXml;
using OfficeOpenXml.Style;
using QuotationSystem.ApplicationCore.Models;
using QuotationSystem.Data.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace QuotationSystem.Services
{
    public class ExcelService : IExcelService
    {
        private const int ErrorCol = 8;

        public Result<List<MItem>, string> GetItemList(ExcelWorksheet worksheet, string currentUser)
        {
            int rowCount = worksheet.Dimension?.End.Row ?? 0;
            if (rowCount == 0)
            {
                return "No data, Please check the upload file.";
            }
            if (!IsValidHeader(worksheet))
            {
                return "Columns mismatch the template.";
            }
            if (!IsValidBody(worksheet))
            {
                return "Invalid data in the file.";
            }
            List<MItem> list = new();

            for (int row = 2; row <= worksheet.Dimension.Rows; row++)
            {
                MItem item = new MItem
                {
                    ItemCode = (string)worksheet.Cells[row, 1].Value,
                    ItemName = (string)worksheet.Cells[row, 2].Value,
                    ItemDesc = (string)worksheet.Cells[row, 3].Value,
                    UnitPrice = Convert.ToDouble(worksheet.Cells[row, 4].Value),
                    UnitId = (string)worksheet.Cells[row, 5].Value,
                    Remark = worksheet.Cells[row, 6].Value is null ? "" : (string)worksheet.Cells[row, 6].Value,
                    ActiveStatus = (string)worksheet.Cells[row, 7].Value,
                    CreateDate = DateTime.Now,
                    CreateBy = currentUser,
                    UpdateDate = DateTime.Now,
                    UpdateBy = currentUser
                };

                list.Add(item);
            }
            return list;
        }
        private bool IsValidHeader(ExcelWorksheet worksheet)
        {
            string[] headers = { "Item Code", "Item Name", "Item Description", "Unit Price", "Unit ID", "Remark", "Active Status" };
            var mismatchedHeaders = headers.Where((header, index) => !string.Equals(worksheet.Cells[1, index + 1].Text.Trim(), header, StringComparison.OrdinalIgnoreCase)).ToList();
            return !mismatchedHeaders.Any();
        }
        private bool IsValidBody(ExcelWorksheet worksheet)
        {
            worksheet.Cells[1, ErrorCol].Value = "ERROR";
            List<bool> results = new();
            HashSet<string> itemCodes = new();
            for (int row = 2; row <= worksheet.Dimension.Rows; row++)
            {
                var itemCode = worksheet.Cells[row, 1];
                var itemName = worksheet.Cells[row, 2];
                var itemDesc = worksheet.Cells[row, 3];
                var unitPrice = worksheet.Cells[row, 4];
                var unitId = worksheet.Cells[row, 5];
                var remark = worksheet.Cells[row, 6];
                var activeStatus = worksheet.Cells[row, 7];

                results.Add(WriteCell(itemCode, (string)itemCode.Value switch
                {
                    string s when string.IsNullOrWhiteSpace(s) => "Item Code is required\n",
                    { Length: > 30 } => "Item Code must be less than or equal to 30 characters.\n",
                    string s when !itemCodes.Add(s) => $"ItemCode is duplicated.\n",
                    _ => ""
                }));
                results.Add(WriteCell(itemName, (string)itemName.Value switch
                {
                    string s when string.IsNullOrWhiteSpace(s) => "Item Name is required\n",
                    { Length: > 30 } => "Item Name must be less than or equal to 30 characters.\n",
                    _ => ""
                }));
                results.Add(WriteCell(itemDesc, (string)itemDesc.Value switch
                {
                    string s when string.IsNullOrWhiteSpace(s) => "Item Description is required\n",
                    { Length: > 30 } => "Item Description must be less than or equal to 30 characters.\n",
                    _ => ""
                }));
                results.Add(WriteCell(unitPrice, (string)unitPrice.Value switch
                {
                    string s when string.IsNullOrWhiteSpace(s) => "Unit Price is required\n",
                    string s when !double.TryParse(s, out var _) => $"UnitPrice must be a number.\n",
                    _ => ""
                }));
                results.Add(WriteCell(unitId, (string)unitId.Value switch
                {
                    string s when string.IsNullOrWhiteSpace(s) => "Unit ID is required\n",
                    { Length: > 10 } => "Unit must be less than or equal to 10 characters.\n",
                    _ => ""
                }));
                results.Add(WriteCell(remark, (string)remark.Value switch
                {
                    { Length: > 150 } => "Remark must be less than or equal to 150 characters.\n",
                    _ => ""
                }));
                results.Add(WriteCell(activeStatus, (string)activeStatus.Value switch
                {
                    string s when string.IsNullOrWhiteSpace(s) => "Active Status is required\n",
                    "Y" or "N" => "",
                    _ => $"ActiveStatus must be Y or N.\n"
                }));
            }
            return results.TrueForAll(x => x);
        }
        private bool WriteCell(ExcelRange cell, string value)
        {
            if (string.IsNullOrEmpty(value)) return true;
            cell.Style.Font.Color.SetColor(System.Drawing.Color.Red);
            cell.Style.Border.Top.Style = ExcelBorderStyle.Thin;
            cell.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
            cell.Style.Border.Left.Style = ExcelBorderStyle.Thin;
            cell.Style.Border.Right.Style = ExcelBorderStyle.Thin;
            cell.Style.Border.Top.Color.SetColor(Color.Red);
            cell.Style.Border.Bottom.Color.SetColor(Color.Red);
            cell.Style.Border.Left.Color.SetColor(Color.Red);
            cell.Style.Border.Right.Color.SetColor(Color.Red);
            cell[cell.Start.Row, ErrorCol].Value += value;
            return false;
        }
    }
}
