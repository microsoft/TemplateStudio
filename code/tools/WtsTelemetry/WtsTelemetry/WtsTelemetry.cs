using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SendGrid.Helpers.Mail;
using System;
using System.Collections.Generic;
using System.Linq;
using WtsTelemetry.Services;

namespace WtsTelemetry
{
    public static class WtsTelemetry
    {

        private const string Uwp = "Uwp";
        private const string Wpf = "Wpf";

        // Every minute: 0 * * * * *
        // Every 5 minutes: 0 */5 * * * *
        // Every day: 0 0 0 * * *
        // every monday at 09:00:00: 0 0 9 * * MON
        // every 1st of month (monthly): 0 0 0 1 * *

        [FunctionName("WtsTelemetry")]
        public static void Run(
            [TimerTrigger("0 0 0 1 * *")]TimerInfo myTimer,
            TraceWriter log, 
            [SendGrid] out Mail message)
        {
            var year = DateTime.Today.AddMonths(-1).Year;
            var month = DateTime.Today.AddMonths(-1).Month;
            var queries = new QueryService(year, month);

            var stringDate = $"{year}.{month.ToString("D2")}";

            log.Info($"WTS: Get Application Insight data from {stringDate}");
            var projectDataUwp = DataService.GetData(queries.Projects(Uwp));
            var projectDataWpf = DataService.GetData(queries.Projects(Wpf));
            var frameworksDataUwp = DataService.GetData(queries.Frameworks(Uwp));
            var frameworksDataWpf = DataService.GetData(queries.Frameworks(Wpf));
            var pagesDataUwp = DataService.GetData(queries.Pages(Uwp));
            var pagesDataWpf = DataService.GetData(queries.Pages(Wpf));
            var featuresDataUwp = DataService.GetData(queries.Features(Uwp));
            var featuresDataWpf = DataService.GetData(queries.Features(Wpf));
            var servicesDataUwp = DataService.GetData(queries.Services(Uwp));
            var servicesDataWpf = DataService.GetData(queries.Services(Wpf));
            var testingDataUwp = DataService.GetData(queries.Testing(Uwp));
            var testingDataWpf = DataService.GetData(queries.Testing(Wpf));
            var entryPointData = DataService.GetData(queries.EntryPoints);
            var languageData = DataService.GetData(queries.Languages);

            log.Info($"WTS: Create Md File");
            var mdText = new MarkdownBuilder()
                        .AddHeader(year, month)
                        .AddTable("Project Type (Uwp)", "Project", projectDataUwp)
                        .AddTable("Project Type (Wpf)", "Project", projectDataWpf)
                        .AddTable("Framework (Uwp)", "Framework Type", frameworksDataUwp)
                        .AddTable("Framework (Wpf)", "Framework Type", frameworksDataWpf)
                        .AddTable("Pages (Uwp)", "Pages", pagesDataUwp)
                        .AddTable("Pages (Wpf)", "Pages", pagesDataWpf)
                        .AddTable("Features (Uwp)", "Features", featuresDataUwp)
                        .AddTable("Features (Wpf)", "Features", featuresDataWpf)
                        .AddTable("Services (Uwp)", "Services", servicesDataUwp)
                        .AddTable("Services (Wpf)", "Services", servicesDataWpf)
                        .AddTable("Testing (Uwp)", "Testing", testingDataUwp)
                        .AddTable("Testing (Wpf)", "Services", testingDataWpf)
                        .AddTable("Windows Template Studio entry point (Common)", "Entry point", entryPointData)
                        .AddTable("Programming languages (Common)", "Languages", languageData)
                        .GetText();


            log.Info($"WTS: send data mail");
            message = MailService.CreateMail(mdText, stringDate);

            log.Info($"WTS: Finish");
        }
    }
}
