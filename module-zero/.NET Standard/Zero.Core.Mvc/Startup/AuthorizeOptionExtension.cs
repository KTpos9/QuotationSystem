using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;
using Zero.Core.Mvc.Authorizations.Requirements;

namespace Zero.Core.Mvc.Startup
{
    public static class AuthorizeOptionExtension
    {
        public static void AddPolicy(this AuthorizationOptions option, string name, string permission)
        {
            option.AddPolicy(name,
                policy => policy.Requirements.Add(new PermissionRequirement(permission)));
        }

        public static void AddPolicy(this AuthorizationOptions option, string[] policies)
        {
            foreach (var policy in policies)
            {
                option.AddPolicy(policy, policy);
            }
        }

        public static void AddPolicy(this AuthorizationOptions option, Dictionary<string, string> policies)
        {
            foreach (var policy in policies)
            {
                option.AddPolicy(policy.Key, policy.Value);
            }
        }
    }
}