using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using SendGrid.Helpers.Mail;
using WtsTelemetry.Services;

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
            var projectData = DataService.GetProjectData();
            message = MailService.CreateMail();
            log.Info($"------ WTS: send mail -----------");
        }        
    }
}
