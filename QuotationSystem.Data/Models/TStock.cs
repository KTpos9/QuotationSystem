using System;
using System.Collections.Generic;

#nullable disable

namespace QuotationSystem.Data.Models
{
    public partial class TStock
    {
        public string LabelId { get; set; }
        public string ItemCode { get; set; }
        public int Qty { get; set; }
        public string ActiveStatus { get; set; }
        public DateTime StockInDate { get; set; }
        public string LotNo { get; set; }
        public string LocationId { get; set; }
        public string WhId { get; set; }
        public string CargoStatus { get; set; }
        public string CreateBy { get; set; }
        public DateTime CreateDate { get; set; }
        public string UpdateBy { get; set; }
        public DateTime? UpdateDate { get; set; }

        public virtual MItem ItemCodeNavigation { get; set; }
    }
}
