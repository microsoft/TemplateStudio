using SendGrid.Helpers.Mail;
using System;
using System.Collections.Generic;
using System.Linq;

namespace WtsTelemetry.Services
{
    public static class MailService
    {
        public static Mail CreateMail(string content)
        {
            var personalization = new Personalization();
            personalization.Tos = GetEmailReceivers().ToList();

            var mail = new Mail();
            mail.AddPersonalization(personalization);
            mail.From = new Email(Environment.GetEnvironmentVariable("SendGrid:From"));
            mail.Subject = Environment.GetEnvironmentVariable("SendGrid:Subject");
            mail.AddContent(new Content("text/html", content));

            return mail;
        }

        private static IEnumerable<Email> GetEmailReceivers()
        {
            var receivers = Environment.GetEnvironmentVariable("SendGrid:To").Split(';');
            foreach (var receiver in receivers)
            {
                yield return new Email(receiver.Trim());
            }
        }
    }
}
