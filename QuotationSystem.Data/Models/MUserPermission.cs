using System;
using System.Collections.Generic;

#nullable disable

namespace QuotationSystem.Data.Models
{
    public partial class MUserPermission : IUpdateable
    {
        public string UserId { get; set; }
        public string MenuId { get; set; }
        public string ActiveStatus { get; set; }
        public string Remark { get; set; }
        public DateTime CreateDate { get; set; }
        public string CreateBy { get; set; }
        public DateTime? UpdateDate { get; set; }
        public string UpdateBy { get; set; }

        public virtual MMenu Menu { get; set; }
        public virtual MUser User { get; set; }
    }
}
