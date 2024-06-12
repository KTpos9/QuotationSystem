using System;
using System.Collections.Generic;

#nullable disable

namespace QuotationSystem.Data.Models
{
    public partial class MWh
    {
        public MWh()
        {
            TStocks = new HashSet<TStock>();
        }

        public string WhId { get; set; }
        public string WhName { get; set; }
        public string Remark { get; set; }
        public string ActiveStatus { get; set; }
        public string CreateBy { get; set; }
        public DateTime CreateDate { get; set; }
        public string UpdateBy { get; set; }
        public DateTime? UpdateDate { get; set; }

        public virtual ICollection<TStock> TStocks { get; set; }
    }
}
