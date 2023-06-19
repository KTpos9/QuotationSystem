using System.Net;
using System.Net.Mail;

namespace Zero.Integration.Email
{
    /// <summary>
    /// ใช้ส่ง Email แบบ Smtp, มีการ mapping server config, email message เป็น class เพื่อให้ง่ายมากขึ้น
    /// </summary>
    public class EmailSender : IEmailSender
    {
        public EmailServerConfig ServerConfig { get; set; }

        public EmailSender(EmailServerConfig config)
        {
            ServerConfig = config;
        }

        /// <summary>
        /// ส่ง Class EmailMessage ซึ่ง mapping properties มาให้ง่ายต่อการใช้งานมากขึ้น
        /// </summary>
        /// <param name="message"></param>
        public void SendMessage(EmailMessage message)
        {
            SendMessage(message.CreateMailMessage());
        }

        /// <summary>
        /// ส่งด้วย Class MailMessage ของ .NET เอง ทำให้สามารถใช้งานได้เต็มรูปแบบ
        /// </summary>
        /// <param name="message"></param>
        public void SendMessage(MailMessage message)
        {
            FilterDomain(message);

            using (SmtpClient smtp = new SmtpClient(ServerConfig.HostName, ServerConfig.SmtpPort)
            {
                EnableSsl = ServerConfig.EnableSsl
            })
            {
                if (ServerConfig.UseDefaultCredentials == false)
                {
                    smtp.UseDefaultCredentials = ServerConfig.UseDefaultCredentials;
                    smtp.Credentials = new NetworkCredential(ServerConfig.NetworkUsername, ServerConfig.NetworkPassword);
                }

                smtp.Send(message);
            }
        }

        private void FilterDomain(MailMessage message)
        {
            if (string.IsNullOrWhiteSpace(ServerConfig.DomainFilter) == false)
            {
                FilterDomain(message.To);
                FilterDomain(message.CC);
                FilterDomain(message.Bcc);
            }
        }

        private void FilterDomain(MailAddressCollection collection)
        {
            for (int i = 0; i < collection.Count; i++)
            {
                var mail = collection[i];
                if (mail.Address.Contains(ServerConfig.DomainFilter) == false)
                {
                    collection.RemoveAt(i);
                    i--;
                }
            }
        }
    }
}