using Microsoft.AspNetCore.Mvc;
using MimeTypes.Core;
using OfficeOpenXml;
using OfficeOpenXml.Table;
using System.Collections.Generic;

namespace Zero.Core.Mvc.Extensions
{
    public static class ExcelExtension
    {
        public static FileContentResult Download(this ExcelPackage excel, string fileName)
        {
            var contentType = MimeTypeMap.GetMimeType("xlsx");
            var result = new FileContentResult(excel.GetAsByteArray(), contentType);
            result.FileDownloadName = fileName;

            return result;
        }

        public static ExcelWorksheet WriteSheet<T>(this ExcelPackage excel, string sheetName, IEnumerable<T> rows)
        {
            return excel.WriteSheet(sheetName, rows, TableStyles.None);
        }

        public static ExcelWorksheet WriteSheet<T>(this ExcelPackage excel, string sheetName, IEnumerable<T> rows, TableStyles tableStyles)
        {
            var worksheet = excel.Workbook.Worksheets.Add(sheetName);
            worksheet.WriteRows(rows, tableStyles);
            return worksheet;
        }

        public static void WriteRows<T>(this ExcelWorksheet worksheet, IEnumerable<T> rows)
        {
            worksheet.WriteRows(rows, TableStyles.None);
        }

        public static void WriteRows<T>(this ExcelWorksheet worksheet, IEnumerable<T> rows, TableStyles tableStyles)
        {
            worksheet.Cells["A1"].LoadFromCollection(rows, true, tableStyles);
            worksheet.Cells[worksheet.Dimension.Address].AutoFitColumns();
        }
    }
}