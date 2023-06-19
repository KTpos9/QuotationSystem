using Microsoft.AspNetCore.Authorization;

namespace Zero.Core.Mvc.Authorizations.Requirements
{
    public class LoginRequirement : IAuthorizationRequirement
    {
        public LoginRequirement(string policy)
        {
            LoginPolicy = policy;
        }

        public string LoginPolicy { get; set; }
    }
}