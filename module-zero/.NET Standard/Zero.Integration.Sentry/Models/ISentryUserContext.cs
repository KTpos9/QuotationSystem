namespace Zero.Integration.Sentry.Models
{
    public interface ISentryUserContext
    {
        string Id { get; }
        string Username { get; }
        string Email { get; }
    }
}