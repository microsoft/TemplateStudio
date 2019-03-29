// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Reflection;
using Microsoft.Templates.Core;
using Microsoft.Templates.Core.Diagnostics;
using VsTelem = Microsoft.VisualStudio.Telemetry;

namespace Microsoft.Templates.UI.VisualStudio
{
    public class VSTelemetryService
    {
        private bool vsTelemAvailable = false;

        public VSTelemetryInfo VsTelemetryIsOptedIn()
        {
            try
            {
                Assembly.Load(new AssemblyName("Microsoft.VisualStudio.Telemetry"));
                vsTelemAvailable = true;
                return GetVSTelemetryInfo();
            }
            catch (Exception ex)
            {
                AppHealth.Current.Info.TrackAsync($"Unable to load the assembly 'Microsoft.VisualStudio.Telemetry'. Visual Studio Telemetry OptIn/OptOut setting will not be considered. Details:\r\n{ex.ToString()}").FireAndForget();
                return new VSTelemetryInfo()
                {
                    OptedIn = false,
                    VisualStudioEdition = string.Empty,
                    VisualStudioExeVersion = string.Empty,
                    VisualStudioCulture = string.Empty,
                    VisualStudioManifestId = string.Empty,
                };
            }
        }

        private VSTelemetryInfo GetVSTelemetryInfo()
        {
            bool result = false;
            string vsEdition = string.Empty;
            string vsVersion = string.Empty;
            string vsCulture = string.Empty;
            string vsManifestId = string.Empty;
            try
            {
                if (VsTelem.TelemetryService.DefaultSession != null)
                {
                    var isOptedIn = VsTelem.TelemetryService.DefaultSession.IsOptedIn;
                    AppHealth.Current.Info.TrackAsync($"Vs Telemetry IsOptedIn: {isOptedIn}").FireAndForget();
                    vsEdition = VsTelem.TelemetryService.DefaultSession?.GetSharedProperty("VS.Core.SkuName");
                    vsVersion = VsTelem.TelemetryService.DefaultSession?.GetSharedProperty("VS.Core.ExeVersion");
                    vsCulture = VsTelem.TelemetryService.DefaultSession?.GetSharedProperty("VS.Core.Locale.ProductLocaleName");
                    vsManifestId = VsTelem.TelemetryService.DefaultSession?.GetSharedProperty("VS.Core.ManifestId");

                    result = isOptedIn;
                }
                else
                {
                    result = false;
                }
            }
            catch (Exception ex)
            {
                AppHealth.Current.Info.TrackAsync($"Exception checking VsTelemetry IsOptedIn:\r\n" + ex.ToString()).FireAndForget();
                result = false;
            }

            return new VSTelemetryInfo()
            {
                OptedIn = result,
                VisualStudioEdition = vsEdition,
                VisualStudioExeVersion = vsVersion,
                VisualStudioCulture = vsCulture,
                VisualStudioManifestId = vsManifestId,
            };
        }

        public void SafeTrackProjectVsTelemetry(Dictionary<string, string> properties, string pageIdentities, string featureIdentities, Dictionary<string, double> metrics, bool success = true)
        {
            try
            {
                if (vsTelemAvailable)
                {
                    TrackVsTelemetry("Project generated", properties, pageIdentities, featureIdentities, metrics, success);
                }
            }
            catch (Exception ex)
            {
                AppHealth.Current.Info.TrackAsync($"Exception tracking Project Creation in VsTelemetry:\r\n" + ex.ToString()).FireAndForget();
            }
        }

        public void SafeTrackNewItemVsTelemetry(Dictionary<string, string> properties, string pageIdentities, string featureIdentities, Dictionary<string, double> metrics, bool success = true)
        {
            try
            {
                if (vsTelemAvailable)
                {
                    TrackVsTelemetry("New Item generated", properties, pageIdentities, featureIdentities, metrics, success);
                }
            }
            catch (Exception ex)
            {
                AppHealth.Current.Info.TrackAsync($"Exception tracking New Item Creation in VsTelemetry:\r\n" + ex.ToString()).FireAndForget();
            }
        }

        public void SafeTrackWizardCancelledVsTelemetry(Dictionary<string, string> properties, bool success = true)
        {
            try
            {
                if (vsTelemAvailable)
                {
                    TrackWizardCancelledVsTelemetry(properties, success);
                }
            }
            catch (Exception ex)
            {
                AppHealth.Current.Info.TrackAsync($"Exception tracking Wizard Cancelled in VsTelemetry:\r\n" + ex.ToString()).FireAndForget();
            }
        }

        private static void TrackVsTelemetry(string resultSummary, Dictionary<string, string> properties, string pageIdentities, string featureIdentities, Dictionary<string, double> metrics, bool success)
        {
            VsTelem.TelemetryResult result = success ? VsTelem.TelemetryResult.Success : VsTelem.TelemetryResult.Failure;

            VsTelem.UserTaskEvent e = new VsTelem.UserTaskEvent(VsTelemetryEvents.WtsGen, result, resultSummary);

            foreach (var key in properties.Keys)
            {
                string renamedKey = key.Replace(TelemetryEvents.Prefix, VsTelemetryEvents.Prefix);
                if (!string.IsNullOrEmpty(properties[key]))
                {
                    e.Properties[renamedKey] = properties[key];
                }
            }

            e.Properties.Add(VsTelemetryProperties.Pages, pageIdentities);
            e.Properties.Add(VsTelemetryProperties.Features, featureIdentities);

            foreach (var key in metrics.Keys)
            {
                string renamedKey = key.Replace(TelemetryEvents.Prefix, TelemetryEvents.Prefix.ToUpperInvariant() + ".");
                e.Properties[renamedKey] = new VsTelem.TelemetryMetricProperty(metrics[key]);
            }

            VsTelem.TelemetryService.DefaultSession.PostEvent(e);
        }

        private void TrackWizardCancelledVsTelemetry(Dictionary<string, string> properties, bool success)
        {
            VsTelem.TelemetryResult result = success ? VsTelem.TelemetryResult.Success : VsTelem.TelemetryResult.Failure;

            VsTelem.UserTaskEvent e = new VsTelem.UserTaskEvent(VsTelemetryEvents.WtsWizard, result, "Wizard cancelled");

            foreach (var key in properties.Keys)
            {
                string renamedKey = key.Replace(TelemetryEvents.Prefix, VsTelemetryEvents.Prefix);
                if (!string.IsNullOrEmpty(properties[key]))
                {
                    e.Properties[renamedKey] = properties[key];
                }
            }

            VsTelem.TelemetryService.DefaultSession.PostEvent(e);
        }
    }
}
