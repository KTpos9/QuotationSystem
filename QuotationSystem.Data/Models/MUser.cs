using System;
using System.Collections.Generic;

#nullable disable

namespace QuotationSystem.Data.Models
{
    public partial class MUser
    {
        public MUser()
        {
            MUserPermissions = new HashSet<MUserPermission>();
        }

        public string UserId { get; set; }
        public string Password { get; set; }
        public string ChangePassword { get; set; }
        public string UserName { get; set; }
        public string DepartmentId { get; set; }
        public string ActiveStatus { get; set; }
        public DateTime CreateDate { get; set; }
        public string CreateBy { get; set; }
        public DateTime? UpdateDate { get; set; }
        public string UpdateBy { get; set; }

        public virtual MDepartment Department { get; set; }
        public virtual ICollection<MUserPermission> MUserPermissions { get; set; }
    }
}
