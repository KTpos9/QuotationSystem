using Microsoft.AspNetCore.Mvc.Rendering;
using QuotationSystem.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuotationSystem.Models.User
{
    public class UserViewModel
    {
        public List<MUser> Users { get; set; }
        public MUser User { get; set; }
        public string UserId { get; set; }
        public List<SelectListItem> DepartmentIds { get; set; }
        //public string Username { get; set; }
        //public string DepartmentId { get; set; }
    }
}
