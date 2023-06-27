using System;
using System.Collections.Generic;

#nullable disable

namespace QuotationSystem.Data.Models
{
    public partial class MItem : IUpdateable
    {
        public MItem()
        {
            TQuotationDetails = new HashSet<TQuotationDetail>();
        }

        public string ItemCode { get; set; }
        public string ItemName { get; set; }
        public string ItemDesc { get; set; }
        public double UnitPrice { get; set; }
        public string UnitId { get; set; }
        public string Remark { get; set; }
        public string ActiveStatus { get; set; }
        public DateTime CreateDate { get; set; }
        public string CreateBy { get; set; }
        public DateTime? UpdateDate { get; set; }
        public string UpdateBy { get; set; }

        public virtual MUnit Unit { get; set; }
        public virtual ICollection<TQuotationDetail> TQuotationDetails { get; set; }
    }
}
