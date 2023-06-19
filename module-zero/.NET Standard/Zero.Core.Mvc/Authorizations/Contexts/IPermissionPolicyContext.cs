using System.Collections.Generic;
using System.Linq;
using Zero.Core.Mvc.Authorizations.Requirements;

namespace Zero.Core.Mvc.Authorizations.Contexts
{
    public interface IPermissionPolicyContext : ILoginPolicyContext
    {
        List<UserPermissionModel> Permissions { get; }
    }

    public class UserPermissionModel
    {
        public string Module { get; set; }
        public string Resource { get; set; }
        public string[] Operations { get; set; }
    }

    public static class UserPermissionModelExtension
    {
        public static bool IsValidPermission(this List<UserPermissionModel> permissions, string policy)
        {
            var requirement = new PermissionRequirement(policy);
            return permissions.IsValidPermission(requirement);
        }

        public static bool IsValidPermission(this List<UserPermissionModel> permissions, PermissionRequirement requirement)
        {
            var authorized = permissions?.FirstOrDefault(x =>
                x.Module == requirement.RequiredModule && x.Resource == requirement.RequiredResource);

            return authorized != null
                   && authorized.Operations.Any(p => requirement.RequiredOperation == p);
        }
    }
}