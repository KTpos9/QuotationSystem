using Microsoft.EntityFrameworkCore;
using QuotationSystem.Data.Helpers;
using QuotationSystem.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using Zero.Core.Mvc.Extensions;
using Zero.Core.Mvc.Models.DataTables;
using Zero.Extension;

namespace QuotationSystem.Data.Repositories
{
    public class QuotationRepository : IQuotationRepository
    {
        private DbContextOptionBuilder option;
        public QuotationRepository(DbContextOptionBuilder option)
        {
            this.option = option;
        }
        public DataTableResultModel<TQuotationHeader> GetQuotationList(DataTableOptionModel dtOption, string customer = "", string qutoationNo = "", string startDate = "", string endDate ="")
        {
            using (var db = new QuotationContext(option))
            {
                return db.TQuotationHeaders
                    .WhereIf(string.IsNullOrEmpty(qutoationNo) == false, x => x.QuotationNo.Contains(qutoationNo))
                    .WhereIf(string.IsNullOrEmpty(customer) == false, x => x.CustomerName.Contains(customer))
                    .WhereIf(string.IsNullOrEmpty(startDate) == false, x => x.QuotationDate >= DateTime.Parse(startDate))
                    .WhereIf(string.IsNullOrEmpty(endDate) == false, x => x.QuotationDate <= DateTime.Parse(endDate))
                    .Select(q => new TQuotationHeader
                    {
                        QuotationNo = q.QuotationNo,
                        QuotationDate = q.QuotationDate,
                        CustomerName = q.CustomerName,
                        Seller = q.Seller,
                        ActiveStatus = q.ActiveStatus,
                        Total = q.Total,
                        GrandTotal = q.GrandTotal
                    })
                    .ToDataTableResult(dtOption);
            }
        }
        public TQuotationHeader GetQuotationById(string quotationNo)
        {
            using (var db = new QuotationContext(option))
            {
                return db.TQuotationHeaders
                    .Include(q => q.TQuotationDetails)
                        .ThenInclude(qd => qd.ItemCodeNavigation) // Include MItem data for each TQuotationDetail
                    .FirstOrDefault(q => q.QuotationNo == quotationNo);
            }
        }
        public void AddQuotation(TQuotationHeader quotation)
        {
            using (var db = new QuotationContext(option))
            {
                db.Add(quotation);
                db.SaveChanges();
            }
        }
        public void EditQuotation(TQuotationHeader quotation)
        {
            using (var db = new QuotationContext(option))
            {
                // Attach the main entity and mark it as modified
                var entry = db.Entry(quotation);
                db.TQuotationHeaders.Attach(quotation);
                entry.State = EntityState.Modified;

                // Load the related TQuotationDetails from the database
                db.Entry(quotation).Collection(q => q.TQuotationDetails).Load();

                // Update scalar properties of the existing entity
                db.Entry(quotation).CurrentValues.SetValues(quotation);

                // Exclude CreateDate from the update
                db.Entry(quotation).Property(x => x.CreateDate).IsModified = false;

                // Mark each related TQuotationDetail as modified & exclude some property
                foreach (var detail in quotation.TQuotationDetails)
                {
                    db.Entry(detail).State = EntityState.Modified;
                    db.Entry(detail).Property(x => x.CreateDate).IsModified = false;
                    db.Entry(detail).Property(x => x.ActiveStatus).IsModified = false;
                }

                db.SaveChanges();

                //var existingQuotation = db.TQuotationHeaders.Find(quotation.QuotationNo);

                //if (existingQuotation is not null)
                //{
                //    // Update the properties of the existing entity, excluding CreateDate
                //    db.Entry(existingQuotation).CurrentValues.SetValues(quotation);

                //    // Manually reset the CreateDate property to its original value
                //    db.Entry(existingQuotation).Property(x => x.CreateDate).IsModified = false;
                //    db.Entry(existingQuotation).State = EntityState.Modified;

                //    db.SaveChanges();
                //}
            }
        }
        public void DeleteQuotation(string quotationNo)
        {
            using (var db = new QuotationContext(option))
            {
                var quotation = db.TQuotationHeaders
                                    .Include(q => q.TQuotationDetails)
                                    .FirstOrDefault(q => q.QuotationNo == quotationNo);
                db.Remove(quotation);
                db.SaveChanges();
            }
        }

        public List<TQuotationHeader> GetTodayQuotationHeader()
        {
            using (var db = new QuotationContext(option))
            {
                return db.TQuotationHeaders.Where(x => x.UpdateDate == DateTime.Today).ToList();
            }
        }
        public int GetWeeklyCount()
        {
            using (var db = new QuotationContext(option))
            {
                DateTime today = DateTime.Today;
                int daysUntilMonday = ((int)today.DayOfWeek - (int)DayOfWeek.Monday + 7) % 7;
                DateTime startOfWeek = today.AddDays(-daysUntilMonday).Date;
                DateTime endOfWeek = startOfWeek.AddDays(6).Date;

                return db.TQuotationHeaders
                        .Where(x => x.UpdateDate >= startOfWeek && x.UpdateDate <= endOfWeek)
                        .Count();
            }
        }
        public int GetMonthlyCount()
        {
            using (var db = new QuotationContext(option))
            {
                DateTime today = DateTime.Today;
                DateTime startOfMonth = new DateTime(today.Year, today.Month, 1);
                DateTime endOfMonth = startOfMonth.AddMonths(1).AddDays(-1);

                int daysInMonth = DateTime.DaysInMonth(today.Year, today.Month);
                endOfMonth = endOfMonth.AddDays(daysInMonth - 1);

                return db.TQuotationHeaders
                    .Where(x => x.UpdateDate >= startOfMonth && x.UpdateDate <= endOfMonth)
                    .Count();
            }
        }
        public ReadOnlySpan<char> GetLastRecordId()
        {
            using (var db = new QuotationContext(option))
            {
                var result = db.TQuotationHeaders.OrderByDescending(x => x.QuotationNo).FirstOrDefault();
                if (result is null)
                {
                    return ReadOnlySpan<char>.Empty;
                }
                return result.QuotationNo;
            }
        }
    }
}
