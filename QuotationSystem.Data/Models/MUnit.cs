using System;
using System.Collections.Generic;

#nullable disable

namespace QuotationSystem.Data.Models
{
    public partial class MUnit
    {
        public MUnit()
        {
            MItems = new HashSet<MItem>();
        }

        public string UnitId { get; set; }
        public string UnitName { get; set; }
        public string UnitDesc { get; set; }
        public string Remark { get; set; }
        public string ActiveStatus { get; set; }
        public DateTime CreateDate { get; set; }
        public string CreateBy { get; set; }
        public DateTime? UpdateDate { get; set; }
        public string UpdateBy { get; set; }

        public virtual ICollection<MItem> MItems { get; set; }
    }
}
