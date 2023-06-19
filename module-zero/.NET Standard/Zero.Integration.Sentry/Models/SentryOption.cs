namespace Zero.Integration.Sentry.Models
{
    public class SentryOption
    {
        public bool Enabled { get; set; }
        public string Dsn { get; set; }
        public string Environment { get; set; }
        public string Release { get; set; }

        public SentryOption ReleaseVersion(string version)
        {
            Release = version;
            return this;
        }
    }
}