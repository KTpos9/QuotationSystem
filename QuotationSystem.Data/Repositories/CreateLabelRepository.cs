using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System;
using System.Text;
using System.Threading.Tasks;
using QuotationSystem.Data.Models;
using QuotationSystem.Data.Helpers;

namespace QuotationSystem.Data.Repositories
{
    public class CreateLabelRepository
    {
        private DbContextOptionBuilder option;

        public CreateLabelRepository(DbContextOptionBuilder option)
        {
            this.option = option;
        }
        public string GetLastRunningDate()
        {
            return "";
        }
        public string GetLabel(int labelQty) {
            
            return "";
        }
        public void insert(TRunningNo runningNo)
        {
            using (var db = new QuotationContext(option))
            {
                db.TRunningNos.Add(runningNo);
            }
        }
        public void update(string runningDate, int labelQty)
        {
            using (var db = new QuotationContext(option))
            {
                var label = db.TRunningNos.FirstOrDefault(e => e.RunningDate == runningDate);
                
            }
        }
    }
}
