using OfficeOpenXml;
using QuotationSystem.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuotationSystem.Data.Helpers
{
    public class ExcelParser
    {
        /// <summary>
        /// Converts an Excel worksheet to a tuple of MItem objects, performing validation.
        /// </summary>
        /// <param name="worksheet">The ExcelWorksheet object representing the worksheet to convert.</param>
        /// <returns>A tuple containing a list of MItem objects, a boolean value indicating whether the conversion is valid, and a validation log.</returns>
        public static (IEnumerable<MItem> data, bool isValid, string log) ExceltoTuple(ExcelWorksheet worksheet)
        {
            return TableHeaderValidator(worksheet, new StringBuilder()).isValid ?
                ExceltoTupleHelper(worksheet, 2, worksheet.Dimension.Rows, new List<MItem>(), new StringBuilder(), new HashSet<string>()) :
                (Enumerable.Empty<MItem>(), false, TableHeaderValidator(worksheet, new StringBuilder()).log);
        }

        /// <summary>
        /// Recursive helper function for converting an Excel worksheet to a tuple of MItem objects, accumulating data and validation log.
        /// </summary>
        /// <param name="worksheet">The ExcelWorksheet object representing the worksheet to convert.</param>
        /// <param name="row">The current row index being processed.</param>
        /// <param name="maxRows">The maximum number of rows in the worksheet.</param>
        /// <param name="accData">The accumulated list of MItem objects.</param>
        /// <param name="accLog">The accumulated validation log.</param>
        /// <returns>A tuple containing the final list of MItem objects, a boolean value indicating whether the conversion is valid, and the validation log.</returns>
        private static (List<MItem> data, bool isValid, string log) ExceltoTupleHelper(ExcelWorksheet worksheet, int row, int maxRows, List<MItem> accData, StringBuilder accLog, HashSet<string> itemcodeaccu)
        {
            return row < maxRows ?
                ExceltoTupleHelper(worksheet, row + 1, maxRows, accData.Append(WorksheetRowToMItem(worksheet, row)).ToList(), accLog.Append(WorksheetRowToLog(worksheet, row, new StringBuilder(), itemcodeaccu)), itemcodeaccu) :
                (accData, accLog.Length == 0, accLog.ToString());
        }

        /// <summary>
        /// Generates a validation log for a specific row in an Excel worksheet.
        /// </summary>
        /// <param name="worksheet">The ExcelWorksheet object representing the worksheet containing the row.</param>
        /// <param name="row">The row index to generate the validation log for.</param>
        /// <param name="str">A StringBuilder object for storing the validation log.</param>
        /// <returns>The validation log for the specified row as a string.</returns>
        private static string WorksheetRowToLog(ExcelWorksheet worksheet, int row, StringBuilder str, HashSet<string> itemcode)
        {
            return str.Append(GetSheetStr(worksheet, row, 1) switch
            {
                null => $"ItemCode at row {row} column 1 is required and cannot be null.\n",
                "" => $"ItemCode at row {row} column 1 is required and cannot be empty.\n",
                _ => ""
            }).Append(itemcode.Add(GetSheetStr(worksheet, row, 1)) switch
            {
                false => $"ItemCode {GetSheetStr(worksheet, row, 1)} at row {row} column 1 is dulplicated.\n",
                _ => "",
            }).Append(GetSheetStr(worksheet, row, 2) switch
            {
                null => $"ItemName at row {row} column 2 is required and cannot be null.\n",
                "" => $"ItemName at row {row} column 2 is required and cannot be empty.\n",
                _ => ""
            }).Append(GetSheetStr(worksheet, row, 3) switch
            {
                null => $"ItemDesc at row {row} column 3 is required and cannot be null.\n",
                "" => $"ItemDesc at row {row} column 3 is required and cannot be empty.\n",
                _ => ""
            }).Append(GetSheetStr(worksheet, row, 4) switch
            {
                null => $"UnitPrice at row {row} column 4 is required and cannot be null.\n",
                "" => $"UnitPrice at row {row} column 4 is required and cannot be empty.\n",
                string s => double.TryParse(s, out var parsedValue) ? "" : $"ItemDesc at row {row} column 4 must be a number.\n"
            }).Append(GetSheetStr(worksheet, row, 5) switch
            {
                null => $"Unit at row {row} column 5 is required and cannot be null.\n",
                "" => $"Unit at row {row} column 5 is required and cannot be empty.\n",
                _ => ""
            }).Append(GetSheetStr(worksheet, row, 7) switch
            {
                null => $"ActiveStatus at row {row} column 7 is required and cannot be null.\n",
                "" => $"ActiveStatus at row {row} column 7 is required and cannot be empty.\n",
                "Y" => "",
                "N" => "",
                _ => $"ActiveStatus at row {row} column 7 must be Y or N.\n"
            }).ToString();
        }

        /// <summary>
        /// Generates a validation log for a specific row in an Excel worksheet.
        /// </summary>
        /// <param name="worksheet">The ExcelWorksheet object representing the worksheet containing the row.</param>
        /// <param name="row">The row index to generate the validation log for.</param>
        /// <param name="str">A StringBuilder object for storing the validation log.</param>
        /// <returns>The validation log for the specified row as a string.</returns>
        private static MItem WorksheetRowToMItem(ExcelWorksheet worksheet, int row)
        {
            return new MItem
            {
                ItemCode = GetSheetStr(worksheet, row, 1),
                ItemName = GetSheetStr(worksheet, row, 2),
                ItemDesc = GetSheetStr(worksheet, row, 3),
                UnitPrice = GetSheetStr(worksheet, row, 4) switch
                {
                    null => 0,
                    "" => 0,
                    string str => double.TryParse(str, out var parsedValue) ? parsedValue : 0
                },
                UnitId = GetSheetStr(worksheet, row, 5),
                Remark = GetSheetStr(worksheet, row, 6),
                ActiveStatus = GetSheetStr(worksheet, row, 7) switch
                {
                    "Y" => "Y",
                    "N" => "N",
                    _ => "N",
                },
            };
        }

        /// <summary>
        /// Validates the table header of an Excel worksheet.
        /// </summary>
        /// <param name="worksheet">The ExcelWorksheet object representing the worksheet to validate.</param>
        /// <param name="str">A StringBuilder object for storing the validation log.</param>
        /// <returns>A tuple containing a validation log and a boolean value indicating whether the header is valid.</returns>
        private static (string log, bool isValid) TableHeaderValidator(ExcelWorksheet worksheet, StringBuilder str)
        {

            return (str.Append(GetSheetStr(worksheet, 1, 1) switch
            {
                "ItemCode" => "",
                _ => "The table header of column 1 should be ItemCode\n"
            }).Append(GetSheetStr(worksheet, 1, 2) switch
            {
                "ItemName" => "",
                _ => "The table header of column 2 should be ItemName\n"
            }).Append(GetSheetStr(worksheet, 1, 3) switch
            {
                "ItemDesc" => "",
                _ => "The table header of column 3 should be ItemDesc\n"
            }).Append(GetSheetStr(worksheet, 1, 4) switch
            {
                "UnitPrice" => "",
                _ => "The table header of column 4 should be UnitPrice\n"
            }).Append(GetSheetStr(worksheet, 1, 5) switch
            {
                "Unit" => "",
                _ => "The table header of column 5 should be Unit\n"
            }).Append(GetSheetStr(worksheet, 1, 6) switch
            {
                "Remark" => "",
                _ => "The table header of column 6 should be Remark\n"
            }).Append(GetSheetStr(worksheet, 1, 7) switch
            {
                "ActiveStatus" => "",
                _ => "The table header of column 7 should be ActiveStatus\n"
            }).Append(worksheet.Dimension.Columns switch
            {
                7 => "",
                _ => $"expected 7 columns, but the file have {worksheet.Dimension.Columns} column(s), Please check for white space in the worksheet and try again."
            }).ToString(), str.Length == 0);
        }

        /// <summary>
        /// Retrieves the value of a cell in an Excel worksheet as a string.
        /// </summary>
        /// <param name="worksheet">The ExcelWorksheet object representing the worksheet containing the cell.</param>
        /// <param name="row">The row index of the cell.</param>
        /// <param name="col">The column index of the cell.</param>
        /// <returns>The cell value as a string, or an empty string if the cell is null.</returns>
        private static string GetSheetStr(ExcelWorksheet worksheet, int row, int col)
        {
            return worksheet.Cells[row, col].Value is null ? "" : worksheet.Cells[row, col].Value.ToString();
        }
    }
}
