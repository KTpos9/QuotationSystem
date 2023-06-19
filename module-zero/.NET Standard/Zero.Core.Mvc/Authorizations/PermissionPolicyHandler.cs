using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using System.Linq;
using System.Threading.Tasks;
using Zero.Core.Mvc.Authorizations.Contexts;
using Zero.Core.Mvc.Authorizations.Requirements;
using Zero.Core.Mvc.Extensions;

namespace Zero.Core.Mvc.Authorizations
{
    public class PermissionPolicyHandler : AuthorizationHandler<PermissionRequirement>
    {
        private readonly IHttpContextAccessor httpContextAccessor;

        public PermissionPolicyHandler(IHttpContextAccessor httpContextAccessor)
        {
            this.httpContextAccessor = httpContextAccessor;
        }

        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, PermissionRequirement requirement)
        {
            if (context.Resource is Endpoint endpoint)
            {
                var policy = $"{requirement.RequiredModule}/{requirement.RequiredResource}/{requirement.RequiredOperation}";
                if (endpoint.Metadata.OfType<AuthorizeAttribute>().Any(filter => filter.Policy == policy))
                {
                    var policyContext = httpContextAccessor.HttpContext.Resolving<IPermissionPolicyContext>();
                    if (policyContext.Permissions.IsValidPermission(requirement))
                    {
                        context.Succeed(requirement);
                    }
                }
            }

            return Task.CompletedTask;
        }
    }
}