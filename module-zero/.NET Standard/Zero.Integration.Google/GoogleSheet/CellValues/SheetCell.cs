using System;
using Google.Apis.Sheets.v4.Data;

namespace Zero.Integration.Google.GoogleSheet.CellValues
{
    public static class SheetCell
    {
        public static CellData Create(string value)
        {
            var cellData = new CellData();
            cellData.UserEnteredValue = new ExtendedValue { StringValue = value };

            var format = new CellFormat { TextFormat = new TextFormat() };
            return cellData.Format(format);
        }

        public static CellData Create(int value)
        {
            var cellData = new CellData();
            cellData.UserEnteredValue = new ExtendedValue { NumberValue = value };

            var format = new CellFormat { NumberFormat = new NumberFormat { Type = "NUMBER", Pattern = "#,##0" } };
            return cellData.Format(format);
        }

        public static CellData Create(decimal value)
        {
            var cellData = new CellData();
            cellData.UserEnteredValue = new ExtendedValue { NumberValue = decimal.ToDouble(value) };

            var format = new CellFormat { NumberFormat = new NumberFormat { Type = "NUMBER", Pattern = "#,##0.00" } };
            return cellData.Format(format);
        }

        public static CellData Create(DateTime value)
        {
            var stringValue = value.TimeOfDay == TimeSpan.Zero
                ? value.ToString("MMM d, yyyy") : value.ToString("MMM d, yyyy HH:mm");

            var cellData = new CellData();
            cellData.UserEnteredValue = new ExtendedValue { StringValue = stringValue };

            return cellData;
        }

        public static CellData Create(DateTime? value, string defaultValue = null)
        {
            if (value.HasValue)
            {
                return Create(value.Value);
            }

            return Create(defaultValue);
        }

        public static CellData Format(this CellData cell, CellFormat format)
        {
            cell.UserEnteredFormat = format;
            return cell;
        }

        public static CellData Bold(this CellData cell)
        {
            cell.UserEnteredFormat.TextFormat.Bold = true;
            return cell;
        }

        public static CellData Italic(this CellData cell)
        {
            cell.UserEnteredFormat.TextFormat.Italic = true;
            return cell;
        }

        public static CellData Underline(this CellData cell)
        {
            cell.UserEnteredFormat.TextFormat.Underline = true;
            return cell;
        }

        public static CellData BackgroundColor(this CellData cell, Color color)
        {
            cell.UserEnteredFormat.BackgroundColor = color;
            return cell;
        }

        public static CellData ForegroundColor(this CellData cell, Color color)
        {
            cell.UserEnteredFormat.TextFormat.ForegroundColor = color;
            return cell;
        }
    }
}