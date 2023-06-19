using Microsoft.AspNetCore.Mvc.Rendering;
using QuotationSystem.Data.Models;
using System.Collections.Generic;

namespace QuotationSystem.Models.User
{
    public class UserModalViewModel
    {
        public string UserId { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Department { get; set; }
        public string ActiveStatus { get; set; }
        public ICollection<MUserPermission> MUserPermissions { get; set; }
        public List<SelectListItem> DepartmentList { get; set; }
    }
}
