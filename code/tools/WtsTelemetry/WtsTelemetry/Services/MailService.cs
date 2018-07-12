using SendGrid.Helpers.Mail;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace WtsTelemetry.Services
{
    public static class MailService
    {
        private static readonly Regex emailRegex = new Regex(@"^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$");

        public static Mail CreateMail(string content, string date)
        {
            var personalization = new Personalization();
            personalization.Tos = GetEmailReceivers().ToList();

            var mail = new Mail();
            mail.AddPersonalization(personalization);
            mail.From = new Email(Environment.GetEnvironmentVariable("SendGrid:From"));
            mail.Subject = string.Format(Environment.GetEnvironmentVariable("SendGrid:Subject"), date);
            mail.AddContent(new Content("text", content));
            //mail.AddAttachment("telemetry.md", content);

            return mail;
        }

        private static IEnumerable<Email> GetEmailReceivers()
        {
            var receivers = Environment.GetEnvironmentVariable("SendGrid:To").Split(';');
            foreach (var receiver in receivers)
            {
                var email = receiver.Trim();
                if(email.IsValidEmail())
                {
                    yield return new Email(email);
                }
            }
        }

        private static void AddAttachment(this Mail mail, string filename, string content)
        {
            var bytes = Encoding.ASCII.GetBytes(content);
            var base64Content = Convert.ToBase64String(bytes);

            var attachment = new Attachment
            {
                Filename = filename,
                Content = base64Content
            };

            mail.AddAttachment(attachment);
        }

        private static bool IsValidEmail(this string emailAdress) => emailRegex.IsMatch(emailAdress.Trim());
    }
}
