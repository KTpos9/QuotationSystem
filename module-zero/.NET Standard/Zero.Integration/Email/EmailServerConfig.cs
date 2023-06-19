namespace Zero.Integration.Email
{
    /// <summary>
    /// ใช้กำหนดค่า Smtp Server ที่จะใช้ส่ง Email
    /// </summary>
    public class EmailServerConfig
    {
        public string HostName { get; set; }
        public int SmtpPort { get; set; }
        public bool EnableSsl { get; set; }
        public bool UseDefaultCredentials { get; set; }
        public string NetworkUsername { get; set; }
        public string NetworkPassword { get; set; }
        public string DomainFilter { get; set; }
        public string Sender { get; set; }

        public EmailServerConfig(string hostName, int smtpPort)
        {
            HostName = hostName;
            SmtpPort = smtpPort;
            EnableSsl = false;
            UseDefaultCredentials = true;
        }

        public EmailServerConfig(string hostName, int smtpPort, string networkUsername, string networkPassword)
        {
            HostName = hostName;
            SmtpPort = smtpPort;
            EnableSsl = false;
            UseDefaultCredentials = false;
            NetworkUsername = networkUsername;
            NetworkPassword = networkPassword;
        }

        public EmailServerConfig(string hostName, int smtpPort, string networkUsername, string networkPassword, string sender)
        {
            HostName = hostName;
            SmtpPort = smtpPort;
            EnableSsl = false;
            UseDefaultCredentials = false;
            NetworkUsername = networkUsername;
            NetworkPassword = networkPassword;
            Sender = sender;
        }
    }
}