using OfficeOpenXml;
using QuotationSystem.ApplicationCore.Models;
using QuotationSystem.Data.Models;
using System.Collections.Generic;

namespace QuotationSystem.Services
{
    public interface IExcelService
    {
        Result<List<MItem>, string> GetItemList(ExcelWorksheet worksheet, string currentUser);
    }
}