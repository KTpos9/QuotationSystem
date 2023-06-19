namespace Zero.Web.UserAgentParsers
{
    public class BrowserVersion
    {
        public BrowserVersion(string family, string major, string minor, string patch)
        {
            Family = family;
            Major = major;
            Minor = minor;
            Patch = patch;
        }

        public string Family { get; }

        public string Major { get; }

        public string Minor { get; }

        public string Patch { get; }

        public override string ToString()
        {
            var version = VersionString.Format(Major, Minor, Patch);
            return Family + (!string.IsNullOrEmpty(version) ? " " + version : null);
        }
    }
}