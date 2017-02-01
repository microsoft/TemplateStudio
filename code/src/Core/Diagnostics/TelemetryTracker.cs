using Microsoft.TemplateEngine.Abstractions;
using Microsoft.TemplateEngine.Edge.Template;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Templates.Core.Diagnostics
{
    public class TelemetryTracker : IDisposable
    {
        public const string PropertiesPrefix = "Uct";
        public TelemetryTracker()
        {
        }
        public TelemetryTracker(Configuration config)
        { 
            TelemetryService.SetConfiguration(config);
        }

        public async Task TrackCancellationAsync()
        {
            //TODO: Now only record cancellation for projects
            Dictionary<string, string> properties = new Dictionary<string, string>()
            {
                { TelemetryProperties.Action, ActionEnum.Add.ToString() },
                { TelemetryProperties.ActionStatus, ActionStatusEnum.Cancelled.ToString() },
            };
            await TelemetryService.Current.TrackEventAsync(TelemetryEvents.Project, properties).ConfigureAwait(false);
        }

        public async Task TrackTemplateGeneratedOkAsync(ITemplateInfo template, int pagesAdded, int featuresAdded)
        {
            if (template.GetTemplateType() == TemplateType.Project)
            {
                await TrackProjectCompletedAsync(template.GetProjectType(), template.GetFramework(), template.Name, pagesAdded, 0, featuresAdded);
            }
        }

        public async Task TrackTemplateGeneratedErrorAsync(ITemplateInfo template, int pagesAdded, int featuresAdded, CreationResultStatus genStatus, string message)
        {
            if (template.GetTemplateType() == TemplateType.Project)
            {
                await TrackProjectErrorAsync(template.GetProjectType(), template.GetFramework(), template.Name, pagesAdded, featuresAdded, 0, genStatus, message);
            }

            if (template.GetTemplateType() == TemplateType.Page)
            {
                await TrackPageErrorAsync(template.GetProjectType(), template.GetFramework(), template.Name, genStatus, message);
            }
        }

        private async Task TrackProjectCompletedAsync(string appType, string appFx, string templateName, int pagesCount = 0, int featuresCount = 0, double timeSpent = 0)
        {
            Dictionary<string, string> properties = new Dictionary<string, string>()
            {
                { TelemetryProperties.Action, ActionEnum.Add.ToString() },
                { TelemetryProperties.ActionStatus, ActionStatusEnum.Completed.ToString() },
                { TelemetryProperties.AppType, appType },
                { TelemetryProperties.AppFx, appFx },
                { TelemetryProperties.TemplateName, templateName },
                { TelemetryProperties.GenStatus, CreationResultStatus.CreateSucceeded.ToString()}
            };

            Dictionary<string, double> metrics = new Dictionary<string, double>()
            {
                { TelemetryMetrics.PagesCount, pagesCount },
                { TelemetryMetrics.TimeSpent, timeSpent},
                { TelemetryMetrics.FeaturesCount, featuresCount }
            };
            await TelemetryService.Current.TrackEventAsync(TelemetryEvents.Project, properties, metrics).ConfigureAwait(false);
        }

        private async Task TrackProjectErrorAsync(string appType, string appFx, string templateName, int pagesCount, int featuresCount, double timeSpent, CreationResultStatus genStatus, string message)
        {
            Dictionary<string, string> properties = new Dictionary<string, string>()
            {
                { TelemetryProperties.Action, ActionEnum.Add.ToString() },
                { TelemetryProperties.ActionStatus, ActionStatusEnum.Error.ToString() },
                { TelemetryProperties.AppType, appType },
                { TelemetryProperties.AppFx, appFx },
                { TelemetryProperties.TemplateName, templateName },
                { TelemetryProperties.GenStatus, genStatus.ToString() },
                { TelemetryProperties.GenMessage, message }
            };
            Dictionary<string, double> metrics = new Dictionary<string, double>()
            {
                { TelemetryMetrics.PagesCount, pagesCount },
                { TelemetryMetrics.TimeSpent, timeSpent},
                { TelemetryMetrics.FeaturesCount, featuresCount }
            };

            await TelemetryService.Current.TrackEventAsync(TelemetryEvents.Project, properties, metrics).ConfigureAwait(false);
        }


        private async Task TrackPageCompletedAsync(ActionStatusEnum status, string appType, string appFx, string templateName)
        {
            Dictionary<string, string> properties = new Dictionary<string, string>()
            {
                { TelemetryProperties.Action, ActionEnum.Add.ToString() },
                { TelemetryProperties.ActionStatus, status.ToString() },
                { TelemetryProperties.AppType, appType },
                { TelemetryProperties.AppFx, appFx },
                { TelemetryProperties.TemplateName, templateName },
                { TelemetryProperties.GenStatus, CreationResultStatus.CreateSucceeded.ToString()}
            };
            await TelemetryService.Current.TrackEventAsync(TelemetryEvents.Page, properties).ConfigureAwait(false);
        }
        private async Task TrackPageErrorAsync(string appType, string appFx, string templateName, CreationResultStatus genStatus, string message)
        {
            Dictionary<string, string> properties = new Dictionary<string, string>()
            {
                { TelemetryProperties.Action, ActionEnum.Add.ToString() },
                { TelemetryProperties.ActionStatus, ActionStatusEnum.Error.ToString() },
                { TelemetryProperties.AppType, appType },
                { TelemetryProperties.AppFx, appFx },
                { TelemetryProperties.TemplateName, templateName },
                { TelemetryProperties.GenStatus, genStatus.ToString() },
                { TelemetryProperties.GenMessage, message }
            };
            await TelemetryService.Current.TrackEventAsync(TelemetryEvents.Page, properties).ConfigureAwait(false);
        }

        ~TelemetryTracker()
        {
            Dispose(false);
        }
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                // free managed resources 
                TelemetryService.Current.Dispose();
            }
            //free native resources if any.
        }
    }
}
