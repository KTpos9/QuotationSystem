using System;
using System.Collections.Generic;

#nullable disable

namespace QuotationSystem.Data.Models
{
    public partial class TQuotationHeader
    {
        public TQuotationHeader()
        {
            TQuotationDetails = new HashSet<TQuotationDetail>();
        }

        public string QuotationNo { get; set; }
        public DateTime QuotationDate { get; set; }
        public string Seller { get; set; }
        public string CustomerName { get; set; }
        public string CustomerAddress { get; set; }
        public string CustomerContact { get; set; }
        public string TaxId { get; set; }
        public string ActiveStatus { get; set; }
        public double? Vat { get; set; }
        public double Total { get; set; }
        public double? GrandTotal { get; set; }
        public string Remark { get; set; }
        public DateTime CreateDate { get; set; }
        public string CreateBy { get; set; }
        public DateTime? UpdateDate { get; set; }
        public string UpdateBy { get; set; }

        public virtual ICollection<TQuotationDetail> TQuotationDetails { get; set; }
    }
}
