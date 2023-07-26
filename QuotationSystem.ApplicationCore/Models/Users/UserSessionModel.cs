using System.Collections.Generic;

namespace QuotationSystem.ApplicationCore.Models.Users
{
    public class UserSessionModel
    {
        public string Id { get; set; }
        public List<int> RoleIds { get; set; }
    }
}