using QuotationSystem.Data.Models;
using System.Collections.Generic;
using Zero.Core.Mvc.Models.DataTables;

namespace QuotationSystem.Data.Repositories
{
    public interface IQuotationRepository
    {
        List<TQuotationHeader> GetTodayQuotationHeader();
        DataTableResultModel<TQuotationHeader> GetQuotationList(DataTableOptionModel dtOption, string customer = "", string qutoationNo = "");
        TQuotationHeader GetQuotationById(string quotationNo);
        void AddQuotation(TQuotationDetail quotation);
        void EditQuotation(TQuotationDetail quotation);
        void DeleteQuotation(string quotationNo);
        int GetWeeklyCount();
        int GetMonthlyCount();
    }
}