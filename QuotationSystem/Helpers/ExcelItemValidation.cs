using OfficeOpenXml;
using QuotationSystem.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuotationSystem.Helpers
{
    public class ExcelItemValidation
    {
        private ExcelWorksheet _worksheet;

        public StringBuilder ErrorLog { get; private set; }
        public bool IsValidFormat { get; private set; }

        public ExcelItemValidation(ExcelWorksheet worksheet, StringBuilder errorLog)
        {
            _worksheet = worksheet;
            ErrorLog = errorLog;
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
                        ItemCode = _worksheet.Cells[row, 1].Value.ToString(),
                        ItemName = _worksheet.Cells[row, 2].Value.ToString(),
                        ItemDesc = _worksheet.Cells[row, 3].Value.ToString(),
                        UnitPrice = Convert.ToDouble(_worksheet.Cells[row, 4].Value),
                        Unit = _worksheet.Cells[row, 5].Value.ToString(),
                        Remark = _worksheet.Cells[row, 6].Value is null ? "" : _worksheet.Cells[row, 6].Value.ToString(),
                        ActiveStatus = _worksheet.Cells[row, 7].Value.ToString(),
                    };

                    list.Add(item);
                }
                return list;
            }
            return Enumerable.Empty<MItem>();
        }

        private bool IsValidExcel()
        {
            IsValidFormat = IsValidExcelHeader() && IsValidExcelData();
            return IsValidFormat;
        }
        private bool IsValidExcelHeader()
        {
            ErrorLog.Append(_worksheet.Cells[1, 1].Value.ToString() switch
            {
                "ItemCode" => "",
                _ => "The table header of column 1 should be ItemCode\n"
            }).Append(_worksheet.Cells[1, 2].Value.ToString() switch
            {
                "ItemName" => "",
                _ => "The table header of column 2 should be ItemName\n"
            }).Append(_worksheet.Cells[1, 3].Value.ToString() switch
            {
                "ItemDesc" => "",
                _ => "The table header of column 3 should be ItemDesc\n"
            }).Append(_worksheet.Cells[1, 4].Value.ToString() switch
            {
                "UnitPrice" => "",
                _ => "The table header of column 4 should be UnitPrice\n"
            }).Append(_worksheet.Cells[1, 5].Value.ToString() switch
            {
                "Unit" => "",
                _ => "The table header of column 5 should be Unit\n"
            }).Append(_worksheet.Cells[1, 6].Value.ToString() switch
            {
                "Remark" => "",
                _ => "The table header of column 6 should be Remark\n"
            }).Append(_worksheet.Cells[1, 7].Value.ToString() switch
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
            for(int row = 2; row <= _worksheet.Dimension.Rows; row++)
            {
                ErrorLog.Append(_worksheet.Cells[row, 1].Value.ToString() switch
                {
                    null => $"ItemCode at row {row} column 1 is required and cannot be null.\n",
                    "" => $"ItemCode at row {row} column 1 is required and cannot be empty.\n",
                    _ => ""
                }).Append(_worksheet.Cells[row, 2].Value.ToString() switch
                {
                    null => $"ItemName at row {row} column 2 is required and cannot be null.\n",
                    "" => $"ItemName at row {row} column 2 is required and cannot be empty.\n",
                    _ => ""
                }).Append(_worksheet.Cells[row, 3].Value.ToString() switch
                {
                    null => $"ItemDesc at row {row} column 3 is required and cannot be null.\n",
                    "" => $"ItemDesc at row {row} column 3 is required and cannot be empty.\n",
                    _ => ""
                }).Append(_worksheet.Cells[row, 4].Value.ToString() switch
                {
                    null => $"UnitPrice at row {row} column 4 is required and cannot be null.\n",
                    "" => $"UnitPrice at row {row} column 4 is required and cannot be empty.\n",
                    string s => double.TryParse(s, out _) ? "" : $"ItemDesc at row {row} column 4 must be a number.\n"
                }).Append(_worksheet.Cells[row, 5].Value.ToString() switch
                {
                    null => $"Unit at row {row} column 5 is required and cannot be null.\n",
                    "" => $"Unit at row {row} column 5 is required and cannot be empty.\n",
                    _ => ""
                }).Append(_worksheet.Cells[row, 7].Value.ToString() switch
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
