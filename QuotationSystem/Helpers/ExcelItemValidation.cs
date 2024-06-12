using OfficeOpenXml;
using QuotationSystem.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QuotationSystem.Helpers
{
    public class ExcelItemValidation
    {
        private ExcelWorksheet _worksheet;
        private string currentUser;

        public StringBuilder ErrorLog { get; private set; }
        public bool IsValidFormat { get; private set; }

        public ExcelItemValidation(ExcelWorksheet worksheet, StringBuilder errorLog, string currentUser)
        {
            _worksheet = worksheet;
            ErrorLog = errorLog;
            this.currentUser = currentUser;
        }
        public IEnumerable<MItem> ExcelToItemList()
        {
            if (IsValidExcel())
            {
                List<MItem> list = new();

                for (int row = 2; row <= _worksheet.Dimension.Rows; row++)
                {
                    MItem item = new MItem
                    {
                        ItemCode = (string)_worksheet.Cells[row, 1].Value,
                        ItemName = (string)_worksheet.Cells[row, 2].Value,
                        ItemDesc = (string)_worksheet.Cells[row, 3].Value,
                        UnitPrice = Convert.ToDouble(_worksheet.Cells[row, 4].Value),
                        UnitId = (string)_worksheet.Cells[row, 5].Value,
                        Remark = _worksheet.Cells[row, 6].Value is null ? "" : (string)_worksheet.Cells[row, 6].Value,
                        ActiveStatus = (string)_worksheet.Cells[row, 7].Value,
                        CreateDate = DateTime.Now,
                        CreateBy = currentUser,
                        UpdateDate = DateTime.Now,
                        UpdateBy = currentUser
                    };

                    list.Add(item);
                }
                return list;
            }
            return Enumerable.Empty<MItem>();
        }

        private bool IsValidExcel()
        {
            int rowCount = _worksheet.Dimension?.End.Row ?? 0;
            if (rowCount == 0)
            {
                ErrorLog.Append("The file is empty.\n");
                return false;
            }
            IsValidFormat = IsValidExcelHeader() && IsValidExcelData();
            return IsValidFormat;
        }
        private bool IsValidExcelHeader()
        {
            ErrorLog.Append(_worksheet.Cells[1, 1].Text switch
            {
                "ItemCode" => "",
                _ => "The table header of column 1 should be ItemCode\n"
            }).Append(_worksheet.Cells[1, 2].Text switch
            {
                "ItemName" => "",
                _ => "The table header of column 2 should be ItemName\n"
            }).Append(_worksheet.Cells[1, 3].Text switch
            {
                "ItemDesc" => "",
                _ => "The table header of column 3 should be ItemDesc\n"
            }).Append(_worksheet.Cells[1, 4].Text switch
            {
                "UnitPrice" => "",
                _ => "The table header of column 4 should be UnitPrice\n"
            }).Append(_worksheet.Cells[1, 5].Text switch
            {
                "Unit" => "",
                _ => "The table header of column 5 should be Unit\n"
            }).Append(_worksheet.Cells[1, 6].Text switch
            {
                "Remark" => "",
                _ => "The table header of column 6 should be Remark\n"
            }).Append(_worksheet.Cells[1, 7].Text switch
            {
                "ActiveStatus" => "",
                _ => "The table header of column 7 should be ActiveStatus\n"
            }).Append(_worksheet.Dimension.Columns switch
            {
                7 => "",
                _ => $"expected 7 columns, but the file have {_worksheet.Dimension.Columns} column(s), Please check for white space in the spreadsheet and try again."
            });
            return ErrorLog.Length == 0;
        }
        private bool IsValidExcelData()
        {
            HashSet<string> itemCode = new();
            for(int row = 2; row <= _worksheet.Dimension.Rows; row++)
            {
                ErrorLog.Append(_worksheet.Cells[row, 1].Value switch
                {
                    null => $"ItemCode at row {row} column 1 is required and cannot be null.\n",
                    string s when s.Length > 30 => $"ItemCode at row {row} column 1 must be less than or equal to 30 characters.\n",
                    string s when !itemCode.Add(s) => $"ItemCode {s} at row {row} column 1 is duplicated.\n",
                    "" => $"ItemCode at row {row} column 1 is required and cannot be empty.\n",
                    _ => ""
                }).Append(_worksheet.Cells[row, 2].Value switch
                {
                    null => $"ItemName at row {row} column 2 is required and cannot be null.\n",
                    "" => $"ItemName at row {row} column 2 is required and cannot be empty.\n",
                    string s when s.Length > 30 => $"ItemName at row {row} column 2 must be less than or equal to 30 characters.\n",
                    _ => ""
                }).Append(_worksheet.Cells[row, 3].Value switch
                {
                    null => $"ItemDesc at row {row} column 3 is required and cannot be null.\n",
                    "" => $"ItemDesc at row {row} column 3 is required and cannot be empty.\n",
                    string s when s.Length > 30 => $"ItemDesc at row {row} column 3 must be less than or equal to 30 characters.\n",
                    _ => ""
                }).Append(_worksheet.Cells[row, 4].Value switch
                {
                    null => $"UnitPrice at row {row} column 4 is required and cannot be null.\n",
                    "" => $"UnitPrice at row {row} column 4 is required and cannot be empty.\n",
                    string s when !double.TryParse(s, out var _) => $"ItemDesc at row {row} column 4 must be a number.\n",
                    _ => ""
                }).Append(_worksheet.Cells[row, 5].Value switch
                {
                    null => $"Unit at row {row} column 5 is required and cannot be null.\n",
                    "" => $"Unit at row {row} column 5 is required and cannot be empty.\n",
                    string s when s.Length > 30 => $"Unit at row {row} column 5 must be less than or equal to 30 characters.\n",
                    _ => ""
                }).Append(_worksheet.Cells[row, 6].Value is null ? "" : _worksheet.Cells[row, 6].Value.ToString() switch
                {
                    string s when s.Length > 150 => $"Unit at row {row} column 6 must be less than or equal to 150 characters.\n",
                    _ => string.Empty
                }).Append(_worksheet.Cells[row, 7].Value switch
                {
                    null => $"ActiveStatus at row {row} column 7 is required and cannot be null.\n",
                    "" => $"ActiveStatus at row {row} column 7 is required and cannot be empty.\n",
                    "Y" => "",
                    "N" => "",
                    _ => $"ActiveStatus at row {row} column 7 must be Y or N.\n"
                });
            }
            return ErrorLog.Length == 0;
        }
    }
}
