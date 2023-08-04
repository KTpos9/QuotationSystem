using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System;
using System.Text;
using System.Threading.Tasks;
using QuotationSystem.Data.Models;
using QuotationSystem.Data.Helpers;
using QuotationSystem.Data.Repositories.Interfaces;

namespace QuotationSystem.Data.Repositories
{
    public class RunningNoRepository : IRunningNoRepository
    {
        private DbContextOptionBuilder option;

        public RunningNoRepository(DbContextOptionBuilder option)
        {
            this.option = option;
        }
        public string GetLastRunningDate()
        {
            using (var db = new QuotationContext(option))
            {
                var t = db.TRunningNos.OrderByDescending(r => r.RunningDate).FirstOrDefault()?.RunningDate;
                return t;
            }
        }

        public string GetLastRunningNo()
        {
            using (var db = new QuotationContext(option))
            {
                return db.TRunningNos.OrderByDescending(r => r.RunningDate).FirstOrDefault()?.RunningNo;
            }
        }

        public void insert(TRunningNo runningNo)
        {
            using (var db = new QuotationContext(option))
            {
                db.TRunningNos.Add(runningNo);
                db.SaveChanges();
            }
        }

        public void Update(string runningDate, int labelQty)
        {
            using (var db = new QuotationContext(option))
            {
                var label = db.TRunningNos.FirstOrDefault(e => e.RunningDate == runningDate);

                if (label is not null)
                {
                    label.RunningNo = (int.Parse(label.RunningNo) + labelQty).ToString();
                    db.SaveChanges();
                }
            }
        }


    }
}
