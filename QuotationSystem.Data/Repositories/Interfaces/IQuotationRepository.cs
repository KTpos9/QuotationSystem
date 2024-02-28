using QuotationSystem.Data.Models;
using System;
using System.Collections.Generic;
using Zero.Core.Mvc.Models.DataTables;

namespace QuotationSystem.Data.Repositories
{
    public interface IQuotationRepository
    {
        DataTableResultModel<TQuotationHeader> GetTodayQuotationHeader(DataTableOptionModel option);
        DataTableResultModel<TQuotationHeader> GetQuotationList(DataTableOptionModel dtOption, string customer = "", string qutoationNo = "", string startDate = "", string endDate = "");
        TQuotationHeader GetQuotationById(string quotationNo);
        void AddQuotation(TQuotationHeader quotation);
        void EditQuotation(TQuotationHeader quotation);
        void DeleteQuotation(string quotationNo);
        (int todayCount, int weeklyCount, int monthlyCount) GetQuotationCounts();
        ReadOnlySpan<char> GetLastRecordId();
    }
}