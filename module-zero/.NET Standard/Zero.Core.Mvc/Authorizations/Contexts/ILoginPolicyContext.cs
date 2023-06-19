namespace Zero.Core.Mvc.Authorizations.Contexts
{
    public interface ILoginPolicyContext
    {
        bool IsLoggedIn { get; }
    }
}