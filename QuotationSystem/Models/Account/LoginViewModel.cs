using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuotationSystem.Models.Account
{
    public class LoginViewModel
    {
        public string UserId { get; set; }
        public string Password { get; set; }
        public string ReturnUrl { get; set; }
    }
}
