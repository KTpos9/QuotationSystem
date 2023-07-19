using QuotationSystem.Data.Models;
using System;
using System.Collections.Generic;

namespace QuotationSystem.Models.Quotation
{
    public class QuotationViewModel
    {
        public TQuotationHeader QuotationHeader { get; set; }
        public string QuotationNo { get; set; }
        public DateTime Date { get; set; }
        public string Customer { get; set; }
        public string CustomerAddress { get; set; }
        public string CustomerContact { get; set; }
        public string TaxId { get; set; }
        public string SalesName { get; set; }
        public double Vat { get; set; }
        public string ActiveStatus { get; set; }
        //public List<MItem> ItemList { get; set; }
    }
}
