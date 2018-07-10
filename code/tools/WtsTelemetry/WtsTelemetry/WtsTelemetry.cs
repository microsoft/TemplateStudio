using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using SendGrid.Helpers.Mail;
using System;
using WtsTelemetry.Services;

namespace WtsTelemetry
{
    public static class WtsTelemetry
    {
        // Every minute: 0 * * * * *
        // Every 5 minutes: 0 */5 * * * *
        // every 1st of month (monthly): 0 0 0 1 * *
        // Every 15 seconds:  */15 * * * * *

        [FunctionName("WtsTelemetry")]
        public static void Run(
            [TimerTrigger("0 0 0 1 * *")]TimerInfo myTimer,
            TraceWriter log, 
            [SendGrid] out Mail message)
        {
            var year = DateTime.Today.AddMonths(-1).Year;
            var month = DateTime.Today.AddMonths(-1).Month;

            log.Info($"------ WTS: Get data from {month}.{year} -----------");

            var queries = new QueryService(year, month);

            var projectsData = DataService.GetData(queries.Projects);
            var frameworksData = DataService.GetData(queries.Frameworks);
            var pagesData = DataService.GetData(queries.Pages);
            var featuresData = DataService.GetData(queries.Features);
            var entryPointData = DataService.GetData(queries.EntryPoints);

            log.Info($"------ WTS: send data mail -----------");
            message = MailService.CreateMail();

            log.Info($"------ WTS: Finish -----------");
        }
    }
}
