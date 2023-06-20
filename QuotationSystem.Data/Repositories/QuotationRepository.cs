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
        public DataTableResultModel<TQuotationHeader> GetQuotationList(DataTableOptionModel dtOption, string customer = "", string qutoationNo = "")
        {
            using (var db = new QuotationContext(option))
            {
                return db.TQuotationHeaders
                    .WhereIf(string.IsNullOrEmpty(qutoationNo) == false, x => x.QuotationNo.Contains(qutoationNo))
                    .WhereIf(string.IsNullOrEmpty(customer) == false, x => x.CustomerName.Contains(customer))
                    .Select(q => new TQuotationHeader
                    {
                        QuotationNo = q.QuotationNo,
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
                    .FirstOrDefault(q => q.QuotationNo == quotationNo);
                //return db.TQuotationDetails
                //    .Include(q => q.ItemCodeNavigation)
                //    .Include(q => q.QuotationNoNavigation)
                //    .ToList();
            }
        }
        public void AddQuotation(TQuotationDetail quotation)
        {
            using (var db = new QuotationContext(option))
            {
                //db.CurrentUser = currentUser;

                //quotation.MUserPermissions = db.MMenus.Select(m => new MUserPermission
                //{
                //    MenuId = m.MenuId
                //}).ToList();
                //if (quotation.ActiveStatus == "false")
                //{
                //    quotation.ActiveStatus = "N";
                //}
                //foreach (var permission in permissions)
                //{
                //    quotation.MUserPermissions.Where(x => x.MenuId == permission).FirstOrDefault().ActiveStatus = "Y";
                //}
                db.Add(quotation);
                db.SaveChanges();
            }
        }
        public void EditQuotation(TQuotationDetail quotation)
        {

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
    }
}
