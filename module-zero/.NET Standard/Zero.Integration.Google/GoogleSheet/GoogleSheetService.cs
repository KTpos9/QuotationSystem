using System.Collections.Generic;
using System.Linq;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Auth.OAuth2.Flows;
using Google.Apis.Auth.OAuth2.Responses;
using Google.Apis.Services;
using Google.Apis.Sheets.v4;
using Google.Apis.Sheets.v4.Data;

namespace Zero.Integration.Google.GoogleSheet
{
    public class GoogleSheetService : IGoogleSheetService
    {
        private readonly ClientSecrets secrets;
        private readonly string applicationName;
        private const string urlFormat = "https://docs.google.com/spreadsheets/d/{0}/edit#gid=0";

        public GoogleSheetService(string clientId, string clientSecret, string applicationName)
        {
            secrets = new ClientSecrets();
            secrets.ClientId = clientId;
            secrets.ClientSecret = clientSecret;

            this.applicationName = applicationName;
        }

        public string CreateGoogleSheet(string refreshToken, string title, string sheetName, List<RowData> rows)
        {
            var service = CreateSheetService(refreshToken);
            var sheet = CreateEmptySpreadsheetBody(title, sheetName);
            var spreadSheetId = CreateSpreadsheet(service, sheet);
            UpdateSheet(service, spreadSheetId, sheetName, rows);

            return string.Format(urlFormat, spreadSheetId);
        }

        public string CreateGoogleSheet(string refreshToken, string title, SheetProperties sheetProperties, List<RowData> rows)
        {
            var service = CreateSheetService(refreshToken);
            var sheet = CreateEmptySpreadsheetBody(title, sheetProperties);
            var spreadSheetId = CreateSpreadsheet(service, sheet);
            UpdateSheet(service, spreadSheetId, sheetProperties.Title, rows);

            return string.Format(urlFormat, spreadSheetId);
        }

        public SheetsService CreateSheetService(string refreshToken)
        {
            var token = new TokenResponse { RefreshToken = refreshToken };
            var credential = new UserCredential(new GoogleAuthorizationCodeFlow(
                new GoogleAuthorizationCodeFlow.Initializer
                {
                    ClientSecrets = secrets
                }),
                "user",
                token);

            var sheetsService = new SheetsService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = applicationName
            });
            return sheetsService;
        }

        public string CreateSpreadsheet(SheetsService sheetsService, Spreadsheet body)
        {
            var spreadSheet = sheetsService.Spreadsheets.Create(body).Execute();
            return spreadSheet.SpreadsheetId;
        }

        private Spreadsheet CreateEmptySpreadsheetBody(string title, string sheetName)
        {
            var properties = new SheetProperties();
            properties.Title = sheetName;

            return CreateEmptySpreadsheetBody(title, properties);
        }

        private Spreadsheet CreateEmptySpreadsheetBody(string title, SheetProperties sheetProperties)
        {
            Spreadsheet spreadsheet = new Spreadsheet();
            spreadsheet.Properties = new SpreadsheetProperties();
            spreadsheet.Properties.Title = title;

            Sheet sheet = new Sheet();
            sheet.Properties = sheetProperties;

            var SheetSet = new List<Sheet>();
            SheetSet.Add(sheet);

            spreadsheet.Sheets = SheetSet;
            return spreadsheet;
        }

        public void UpdateSheet(SheetsService sheetService, string spreadSheetId, string sheetName, List<RowData> rows)
        {
            var sheetId = GetSheetId(sheetService, spreadSheetId, sheetName);
            GridCoordinate coordinate = new GridCoordinate
            {
                ColumnIndex = 0,
                RowIndex = 0,
                SheetId = sheetId
            };

            UpdateCells(sheetService, spreadSheetId, rows, coordinate);
        }

        public void UpdateCells(SheetsService sheetService, string spreadSheetId, List<RowData> rows, GridCoordinate coordinate)
        {
            var request = new Request { UpdateCells = new UpdateCellsRequest { Start = coordinate, Fields = "*" } };
            request.UpdateCells.Rows = rows;

            var requests = new BatchUpdateSpreadsheetRequest { Requests = new List<Request>() };
            requests.Requests.Add(request);

            sheetService.Spreadsheets.BatchUpdate(requests, spreadSheetId).Execute();
        }

        public int GetSheetId(SheetsService service, string spreadSheetId, string sheetName)
        {
            var spreadSheet = service.Spreadsheets.Get(spreadSheetId).Execute();
            var sheet = spreadSheet.Sheets.FirstOrDefault(s => s.Properties.Title == sheetName);
            int sheetId = (int)sheet.Properties.SheetId;
            return sheetId;
        }
    }
}