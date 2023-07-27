using System;
using System.Collections.Generic;

#nullable disable

namespace QuotationSystem.Data.Models
{
    public partial class MMenu
    {
        public MMenu()
        {
            MUserPermissions = new HashSet<MUserPermission>();
        }

        public string MenuId { get; set; }
        public string MenuName { get; set; }
        public string ParentMenu { get; set; }
        public string ActiveStatus { get; set; }
        public string Remark { get; set; }
        public DateTime CreateDate { get; set; }
        public string CreateBy { get; set; }
        public DateTime? UpdateDate { get; set; }
        public string UpdateBy { get; set; }

        public virtual ICollection<MUserPermission> MUserPermissions { get; set; }
    }
}
