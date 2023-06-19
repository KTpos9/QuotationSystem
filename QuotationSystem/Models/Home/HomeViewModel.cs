using QuotationSystem.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuotationSystem.Models.Home
{
    public class HomeViewModel
    {
        public List<TQuotationHeader> QuotationHeader { get; set; }
        public int WeeklyCount { get; set; }
        public int MonthlyCount { get; set; }
    }
}
