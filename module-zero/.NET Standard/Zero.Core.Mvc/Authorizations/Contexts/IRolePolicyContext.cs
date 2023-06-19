using System.Collections.Generic;

namespace Zero.Core.Mvc.Authorizations.Contexts
{
    public interface IRolePolicyContext : ILoginPolicyContext
    {
        List<int> Roles { get; }
    }
}