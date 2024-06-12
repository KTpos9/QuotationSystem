using System;
using System.Collections.Generic;

#nullable disable

namespace QuotationSystem.Data.Models
{
    public partial class MDepartment
    {
        public MDepartment()
        {
            MUsers = new HashSet<MUser>();
        }

        public string DepartmentId { get; set; }
        public string DepartmentName { get; set; }
        public string DepartmentDesc { get; set; }
        public string Remark { get; set; }
        public string ActiveStatus { get; set; }
        public DateTime CreateDate { get; set; }
        public string CreateBy { get; set; }
        public DateTime? UpdateDate { get; set; }
        public string UpdateBy { get; set; }

        public virtual ICollection<MUser> MUsers { get; set; }
    }
}
