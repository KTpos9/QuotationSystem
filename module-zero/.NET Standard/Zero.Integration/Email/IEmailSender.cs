using System.Net.Mail;

namespace Zero.Integration.Email
{
    public interface IEmailSender
    {
        EmailServerConfig ServerConfig { get; set; }

        void SendMessage(EmailMessage message);

        void SendMessage(MailMessage message);
    }
}