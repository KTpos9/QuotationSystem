using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;

namespace Zero.Integration.Email
{
    public class EmailMessage
    {
        public MailAddress From { get; set; }
        public List<MailAddress> To { get; }
        public List<MailAddress> Cc { get; }
        public List<MailAddress> Bcc { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public bool IsBodyHtml { get; set; }
        public List<Attachment> Attachments { get; }
        public List<AlternateView> AlternateViews { get; }

        public EmailMessage()
        {
            To = new List<MailAddress>();
            Cc = new List<MailAddress>();
            Bcc = new List<MailAddress>();
            IsBodyHtml = true;
            Attachments = new List<Attachment>();
            AlternateViews = new List<AlternateView>();
        }

        public EmailMessage(string from, string to, string subject, string body) : this()
        {
            Subject = subject;
            Body = body;
            From = new MailAddress(from);
            To.Add(new MailAddress(to));
        }

        public EmailMessage(MailAddress from, MailAddress to, string subject, string body) : this()
        {
            Subject = subject;
            Body = body;
            From = from;
            To.Add(to);
        }

        public EmailMessage(string from, List<string> tos, string subject, string body) : this()
        {
            Subject = subject;
            Body = body;
            From = new MailAddress(from);
            To.AddRange(tos.Select(x => new MailAddress(x)));
        }

        public EmailMessage(MailAddress from, List<MailAddress> tos, string subject, string body) : this()
        {
            Subject = subject;
            Body = body;
            From = from;
            To.AddRange(tos);
        }

        public MailMessage CreateMailMessage()
        {
            var message = new MailMessage();
            message.From = From;
            message.Subject = Subject;
            message.SubjectEncoding = Encoding.UTF8;
            message.Body = Body;
            message.BodyEncoding = Encoding.UTF8;
            message.IsBodyHtml = IsBodyHtml;

            foreach (var to in To)
            {
                message.To.Add(to);
            }
            foreach (var cc in Cc)
            {
                message.CC.Add(cc);
            }
            foreach (var bcc in Bcc)
            {
                message.Bcc.Add(bcc);
            }
            foreach (var attachment in Attachments)
            {
                message.Attachments.Add(attachment);
            }
            foreach (var alternativeView in AlternateViews)
            {
                message.AlternateViews.Add(alternativeView);
            }

            return message;
        }
    }
}