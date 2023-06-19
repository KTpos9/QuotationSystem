using System.Collections.Generic;
using Google.Apis.Sheets.v4;
using Google.Apis.Sheets.v4.Data;

namespace Zero.Integration.Google.GoogleSheet
{
    public interface IGoogleSheetService
    {
        string CreateGoogleSheet(string refreshToken, string title, string sheetName, List<RowData> rows);

        string CreateGoogleSheet(string refreshToken, string title, SheetProperties sheetProperties, List<RowData> rows);

        SheetsService CreateSheetService(string refreshToken);

        string CreateSpreadsheet(SheetsService sheetsService, Spreadsheet body);

        int GetSheetId(SheetsService service, string spreadSheetId, string sheetName);

        void UpdateCells(SheetsService sheetService, string spreadSheetId, List<RowData> rows, GridCoordinate coordinate);

        void UpdateSheet(SheetsService sheetService, string spreadSheetId, string sheetName, List<RowData> rows);
    }
}