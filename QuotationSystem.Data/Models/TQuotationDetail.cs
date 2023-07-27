using System;
using System.Collections.Generic;

#nullable disable

namespace QuotationSystem.Data.Models
{
    public partial class TQuotationDetail
    {
        public string QuotationNo { get; set; }
        public string ItemCode { get; set; }
        public double? ItemQty { get; set; }
        public double? DiscountPercent { get; set; }
        public string Remark { get; set; }
        public string ActiveStatus { get; set; }
        public DateTime CreateDate { get; set; }
        public string CreateBy { get; set; }
        public DateTime? UpdateDate { get; set; }
        public string UpdateBy { get; set; }

        public virtual MItem ItemCodeNavigation { get; set; }
        public virtual TQuotationHeader QuotationNoNavigation { get; set; }
    }
}
