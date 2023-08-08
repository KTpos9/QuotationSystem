using System.Collections.Generic;

namespace QuotationSystem.Models.StockAsOn
{
    public class StockAsOnViewModel
    {
        public string ItemCode { get; set; }
        public string WhId { get; set; }
        public List<string> WhIds { get; set; }
    }
}
