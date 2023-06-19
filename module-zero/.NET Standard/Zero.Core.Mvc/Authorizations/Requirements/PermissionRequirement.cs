using Microsoft.AspNetCore.Authorization;

namespace Zero.Core.Mvc.Authorizations.Requirements
{
    public class PermissionRequirement : IAuthorizationRequirement
    {
        public PermissionRequirement(string policy)
        {
            var req = policy.Split('/');
            RequiredModule = req[0];
            RequiredResource = req[1];
            RequiredOperation = req[2];
        }

        public PermissionRequirement(string requiredModule, string requiredResource, string requiredOperation)
        {
            RequiredModule = requiredModule;
            RequiredResource = requiredResource;
            RequiredOperation = requiredOperation;
        }

        public string RequiredModule { get; set; }
        public string RequiredResource { get; set; }
        public string RequiredOperation { get; set; }
    }
}