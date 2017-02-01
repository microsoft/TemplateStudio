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

        public async Task TrackNewProject(ActionStatus status, string appType, string appFx, string templateName, int pagesCount = 0, double timeSpent=0, int featuresDefaultCount=0, int featuresAddedCount = 0, int featuresRemovedCount = 0)
        {
            Dictionary<string, string> properties = new Dictionary<string, string>()
            {
                { TelemetryProperties.ActionStatus, status.ToString() },
                { TelemetryProperties.AppType, appType },
                { TelemetryProperties.FxType, appFx },
                { TelemetryProperties.TemplateName, templateName }
            };

            Dictionary<string, double> metrics = new Dictionary<string, double>()
            {
                { TelemetryMetrics.PagesCount, pagesCount },
                { TelemetryMetrics.TimeSpent, timeSpent},
                { TelemetryMetrics.FeaturesDefaultCount, featuresDefaultCount },
                { TelemetryMetrics.FeaturesAddedCount, featuresAddedCount },
                { TelemetryMetrics.FeaturesRemovedCount, featuresRemovedCount }

            };
            await TelemetryService.Current.TrackEventAsync(TelemetryEvents.NewProject, properties, metrics).ConfigureAwait(false);
        }

        public async Task TrackNewPage(ActionStatus status, string appType, string appFx, string templateName)
        {
            Dictionary<string, string> properties = new Dictionary<string, string>()
            {
                { TelemetryProperties.ActionStatus, status.ToString() },
                { TelemetryProperties.AppType, appType },
                { TelemetryProperties.FxType, appFx },
                { TelemetryProperties.TemplateName, templateName }
            };
            await TelemetryService.Current.TrackEventAsync(TelemetryEvents.NewPage, properties).ConfigureAwait(false);
        }

        public async Task TrackWizardAction(int lastStep)
        {
            Dictionary<string, string> properties = new Dictionary<string, string>()
            {
                { TelemetryProperties.LastStep, lastStep.ToString() }
            };
            await TelemetryService.Current.TrackEventAsync(TelemetryEvents.Wizard, properties).ConfigureAwait(false);
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
