// ******************************************************************
// Copyright (c) Microsoft. All rights reserved.
// This code is licensed under the MIT License (MIT).
// THE CODE IS PROVIDED “AS IS”, WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED,
// INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.
// IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
// DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT,
// TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH
// THE CODE OR THE USE OR OTHER DEALINGS IN THE CODE.
// ******************************************************************

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.TemplateEngine.Abstractions;
using Microsoft.TemplateEngine.Edge.Template;

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


        public async Task TrackWizardCompletedAsync(WizardTypeEnum wizardType)
        {
            //TODO: Now only record cancellation for projects
            Dictionary<string, string> properties = new Dictionary<string, string>()
            {
                { TelemetryProperties.WizardStatus, WizardStatusEnum.Completed.ToString() },
                { TelemetryProperties.WizardType, wizardType.ToString() },
            };
            await TelemetryService.Current.TrackEventAsync(TelemetryEvents.Wizard, properties).ConfigureAwait(false);
        }

        public async Task TrackWizardCompletedAsync()
        {
            //TODO: Now only record cancellation for projects
            Dictionary<string, string> properties = new Dictionary<string, string>()
            {
                { TelemetryProperties.WizardStatus, WizardStatusEnum.Completed.ToString() },
                { TelemetryProperties.WizardType, WizardTypeEnum.NewProject.ToString() },
            };
            await TelemetryService.Current.TrackEventAsync(TelemetryEvents.Wizard, properties).ConfigureAwait(false);
        }
        public async Task TrackWizardCancelledAsync(WizardTypeEnum wizardType)
        {
            //TODO: Now only record cancellation for projects
            Dictionary<string, string> properties = new Dictionary<string, string>()
            {
                { TelemetryProperties.WizardStatus, WizardStatusEnum.Cancelled.ToString() },
                { TelemetryProperties.WizardType, wizardType.ToString() },
            };
            await TelemetryService.Current.TrackEventAsync(TelemetryEvents.Wizard, properties).ConfigureAwait(false);
        }



        public async Task TrackProjectGenAsync(ITemplateInfo template, string appFx, TemplateCreationResult result, int? pagesCount = null, int? featuresCount = null, double? timeSpent = null)
        {
            if (template == null) throw new ArgumentNullException("template");
            if (result == null) throw new ArgumentNullException("result");

            if (template.GetTemplateType() != TemplateType.Project)
            {
                return;
            }

            GenStatusEnum telemetryStatus = result.Status == CreationResultStatus.Success ? GenStatusEnum.Completed : GenStatusEnum.Error;            
            await TrackProjectAsync(telemetryStatus, template.Name, template.GetProjectType(), appFx, pagesCount, featuresCount, timeSpent, result.Status, result.Message);
        }

        public async Task TrackPageOrFeatureTemplateGenAsync(ITemplateInfo template, string appFx, TemplateCreationResult result)
        {
            if (template == null) throw new ArgumentNullException("template");
            if (result == null) throw new ArgumentNullException("result");

            if (template != null && result != null)
            {
                switch (template.GetTemplateType())
                {
                    case TemplateType.Page:
                        await TrackPageGenAsync(template, appFx, result);
                        break;
                    case TemplateType.Unspecified:
                        break;
                }
            }
        }

        private async Task TrackProjectAsync(GenStatusEnum status, string templateName, string appType, string appFx, int? pagesCount = null, int? featuresCount = null, double? timeSpent = null, CreationResultStatus genStatus = CreationResultStatus.Success, string message = "")
        {
            Dictionary<string, string> properties = new Dictionary<string, string>()
            {
                { TelemetryProperties.Status, status.ToString() },
                { TelemetryProperties.AppType, appType },
                { TelemetryProperties.AppFx, appFx },
                { TelemetryProperties.TemplateName, templateName },
                { TelemetryProperties.GenEngineStatus, genStatus.ToString() },
                { TelemetryProperties.GenEngineMessage, message }
            };

            Dictionary<string, double> metrics = new Dictionary<string, double>();
            if (pagesCount.HasValue)
            {
                metrics.Add(TelemetryMetrics.PagesCount, pagesCount.Value);
            }
            if (timeSpent.HasValue)
            {
                metrics.Add(TelemetryMetrics.TimeSpent, timeSpent.Value);
            }
            if (featuresCount.HasValue)
            {
                metrics.Add(TelemetryMetrics.FeaturesCount, featuresCount.Value);
            }

            await TelemetryService.Current.TrackEventAsync(TelemetryEvents.ProjectGen, properties, metrics).ConfigureAwait(false);
        }

        private async Task TrackPageGenAsync(ITemplateInfo template, string appFx, TemplateCreationResult result)
        {
            if(template.GetTemplateType() != TemplateType.Page)
            {
                return;
            }

            GenStatusEnum telemetryStatus = result.Status == CreationResultStatus.Success ? GenStatusEnum.Completed : GenStatusEnum.Error;

            await TrackPageAsync(telemetryStatus, template.GetProjectType(), appFx, template.Name, result.Status, result.Message);
        }

        private async Task TrackFeatureGenAsync(ITemplateInfo template, TemplateCreationResult result)
        {
            GenStatusEnum telemetryStatus = result.Status == CreationResultStatus.Success ? GenStatusEnum.Completed : GenStatusEnum.Error;

            await TrackFeatureAsync();
        }

        private async Task TrackPageAsync(GenStatusEnum status, string appType, string pageFx, string templateName, CreationResultStatus genStatus= CreationResultStatus.Success, string message="")
        {
            Dictionary<string, string> properties = new Dictionary<string, string>()
            {
                { TelemetryProperties.Status, status.ToString() },
                { TelemetryProperties.AppFx, pageFx },
                { TelemetryProperties.TemplateName, templateName },
                { TelemetryProperties.GenEngineStatus, genStatus.ToString() },
                { TelemetryProperties.GenEngineMessage, message }
            };

            await TelemetryService.Current.TrackEventAsync(TelemetryEvents.PageGen, properties).ConfigureAwait(false);
        }

        private Task TrackFeatureAsync()
        {
            throw new NotImplementedException();
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
