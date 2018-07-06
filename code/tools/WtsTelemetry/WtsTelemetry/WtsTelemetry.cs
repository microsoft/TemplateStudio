using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using SendGrid.Helpers.Mail;

namespace WtsTelemetry
{
    public static class WtsTelemetry
    {
        [FunctionName("WtsTelemetry")]
        public static void Run(
            [TimerTrigger("0 0 0 1 * *")]TimerInfo myTimer, 
            TraceWriter log, 
            [SendGrid] out Mail message)
        {
            message = CreateMail();
            log.Info($"------ WTS: send mail -----------");

        }

        private static Mail CreateMail()
        {
            var personalization = new Personalization();
            personalization.Tos = GetEmailReceivers().ToList();

            var mail = new Mail();
            mail.AddPersonalization(personalization);
            mail.From = new Email(Environment.GetEnvironmentVariable("SendGrid:From"));
            mail.Subject = Environment.GetEnvironmentVariable("SendGrid:Subject");
            mail.AddContent(new Content("text/html", Environment.GetEnvironmentVariable("SendGrid:Content")));

            return mail;
        }

        private static IEnumerable<Email>GetEmailReceivers()
        {
            var receivers = Environment.GetEnvironmentVariable("SendGrid:To").Split(';');
            foreach(var receiver in receivers)
            {
                yield return new Email(receiver.Trim());
            }
        }
    }
}
