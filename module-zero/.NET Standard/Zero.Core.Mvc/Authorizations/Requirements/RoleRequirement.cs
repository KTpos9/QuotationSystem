using Microsoft.AspNetCore.Authorization;

namespace Zero.Core.Mvc.Authorizations.Requirements
{
    public class RoleRequirement : IAuthorizationRequirement
    {
        public RoleRequirement(params int[] roleIds)
        {
            RequiredRoleId = roleIds;
        }

        public int[] RequiredRoleId { get; set; }
    }
}