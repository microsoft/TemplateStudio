// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.TemplateEngine.Edge.Template;
using Microsoft.Templates.Core.Diagnostics;
using Microsoft.Templates.Core.PostActions;
using Microsoft.Templates.Core.Resources;

namespace Microsoft.Templates.Core.Gen
{
    public class NewProjectGenController : GenController
    {
        private static Lazy<NewProjectGenController> _instance = new Lazy<NewProjectGenController>(() => new NewProjectGenController());

        public static NewProjectGenController Instance => _instance.Value;

        private NewProjectGenController()
        {
            PostactionFactory = new NewProjectPostActionFactory();
        }

        public async Task UnsafeGenerateProjectAsync(UserSelection userSelection)
        {
            VerifyGenContextPaths();

            var genItems = GenComposer.Compose(userSelection).ToList();

            var chrono = Stopwatch.StartNew();
            var genResults = await GenerateItemsAsync(genItems, false);
            chrono.Stop();

            TrackTelemetry(genItems, genResults, chrono.Elapsed.TotalSeconds, userSelection.ProjectType, userSelection.Framework, userSelection.Platform, userSelection.Language);
        }

        private static void TrackTelemetry(IEnumerable<GenInfo> genItems, Dictionary<string, TemplateCreationResult> genResults, double timeSpent, string appProjectType, string appFx, string appPlatform, string language)
        {
            try
            {
                var genItemsTelemetryData = new GenItemsTelemetryData(genItems);

                foreach (var genInfo in genItems.Where(g => g.Template != null))
                {
                    string resultsKey = $"{genInfo.Template.Identity}_{genInfo.Name}";

                    if (genInfo.Template.GetTemplateType() == TemplateType.Project)
                    {
                        AppHealth.Current.Telemetry.TrackProjectGenAsync(genInfo.Template, appProjectType, appFx, appPlatform, genResults[resultsKey], GenContext.ToolBox.Shell.GetVsProjectId(), language, genItemsTelemetryData, timeSpent, GenContext.Current.ProjectMetrics).FireAndForget();
                    }
                    else
                    {
                        AppHealth.Current.Telemetry.TrackItemGenAsync(genInfo.Template, GenSourceEnum.NewProject, appProjectType, appFx, genResults[resultsKey]).FireAndForget();
                    }
                }
            }
            catch (Exception ex)
            {
                AppHealth.Current.Exception.TrackAsync(ex, StringRes.ErrorTrackTelemetryException).FireAndForget();
            }
        }
    }
}
