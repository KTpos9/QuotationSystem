using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuotationSystem.Models.Quotation
{
    public class QuotationItemViewModel
    {
        public string itemCode { get; set; }
        public string itemName { get; set; }
        public string itemDesc { get; set; }
        public double Qty { get; set; }
        public string unitId { get; set; }
        public string unitPrice { get; set; }
        public double discount { get; set; }
        public string total { get; set; }
    }
}
