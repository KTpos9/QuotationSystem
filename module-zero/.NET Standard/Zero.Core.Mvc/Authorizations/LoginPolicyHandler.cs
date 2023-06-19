using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using Zero.Core.Mvc.Authorizations.Contexts;
using Zero.Core.Mvc.Authorizations.Requirements;
using Zero.Core.Mvc.Extensions;

namespace Zero.Core.Mvc.Authorizations
{
    public class LoginPolicyHandler : AuthorizationHandler<LoginRequirement>
    {
        private readonly IHttpContextAccessor httpContextAccessor;

        public LoginPolicyHandler(IHttpContextAccessor httpContextAccessor)
        {
            this.httpContextAccessor = httpContextAccessor;
        }

        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, LoginRequirement requirement)
        {
            var sessionContext = httpContextAccessor.HttpContext.Resolving<ILoginPolicyContext>();
            if (sessionContext.IsLoggedIn)
            {
                context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }
    }
}