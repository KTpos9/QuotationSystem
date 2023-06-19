using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using System.Linq;
using System.Threading.Tasks;
using Zero.Core.Mvc.Authorizations.Contexts;
using Zero.Core.Mvc.Authorizations.Requirements;
using Zero.Core.Mvc.Extensions;

namespace Zero.Core.Mvc.Authorizations
{
    public class RolePolicyHandler : AuthorizationHandler<RoleRequirement>
    {
        private readonly IHttpContextAccessor httpContextAccessor;

        public RolePolicyHandler(IHttpContextAccessor httpContextAccessor)
        {
            this.httpContextAccessor = httpContextAccessor;
        }

        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, RoleRequirement requirement)
        {
            if (context.Resource is Endpoint endpoint)
            {
                var policyContext = httpContextAccessor.HttpContext.Resolving<IRolePolicyContext>();
                if (policyContext.Roles != null
                    && policyContext.Roles.Exists(x => requirement.RequiredRoleId.Any(r => r == x)))
                {
                    context.Succeed(requirement);
                }
            }
            return Task.CompletedTask;
        }
    }
}