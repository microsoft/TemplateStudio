// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.Templates.Core;
using Microsoft.Templates.Core.Gen;
using Microsoft.Templates.Core.Locations;
using Microsoft.Templates.Fakes;
using Microsoft.Templates.UI;

namespace Microsoft.Templates.Test
{
    public sealed class BuildRightClickWithLegacyFixture : BaseGenAndBuildFixture, IDisposable
    {
        private string testExecutionTimeStamp = DateTime.Now.FormatAsDateHoursMinutes();
        public override string GetTestRunPath() => $"{Path.GetPathRoot(Environment.CurrentDirectory)}\\UIT\\LEG\\{testExecutionTimeStamp}\\";

        public TemplatesSource Source => new LegacyTemplatesSourceV2();
        public TemplatesSource LocalSource => new LocalTemplatesSource("BldRClickLegacy");

        private static bool syncExecuted;

        public static IEnumerable<object[]> GetProjectTemplates()
        {
            List<object[]> result = new List<object[]>();

            foreach (var language in ProgrammingLanguages.GetAllLanguages())
            {
                Configuration.Current.CdnUrl = "https://wtsrepository.blob.core.windows.net/pro/";

                InitializeTemplates(new LegacyTemplatesSourceV2(), language);

                // TODO: Re-enable for all platforms
                ////foreach (var language in Platforms.GetAllPlarforms())
                var projectTypes = GenContext.ToolBox.Repo.GetProjectTypes()
                            .Where(m => !string.IsNullOrEmpty(m.Description))
                            .Select(m => m.Name);

                foreach (var projectType in projectTypes)
                {
                    // TODO: Re-enable for all platforms
                    // var projectFrameworks = GenComposer.GetSupportedFx(projectType, string.Empty);
                    var targetFrameworks = GenContext.ToolBox.Repo.GetFrameworks()
                                                .Select(m => m.Name).ToList();

                    foreach (var framework in targetFrameworks)
                    {
                        result.Add(new object[] { projectType, framework, Platforms.Uwp, language });
                    }
                }
            }

            return result;
        }

        [SuppressMessage(
         "Usage",
         "VSTHRD002:Synchronously waiting on tasks or awaiters may cause deadlocks",
         Justification = "Required for unit testing.")]
        private static void InitializeTemplates(TemplatesSource source, string language)
        {
            Configuration.Current.CdnUrl = "https://wtsrepository.blob.core.windows.net/pro/";

            source.LoadConfigAsync(default(CancellationToken)).Wait();
            var version = new Version(source.Config.Latest.Version.Major, source.Config.Latest.Version.Minor);

            GenContext.Bootstrap(source, new FakeGenShell(Platforms.Uwp, language), version, language);
            if (!syncExecuted)
            {
                GenContext.ToolBox.Repo.SynchronizeAsync(true, true).Wait();

                syncExecuted = true;
            }
        }

        [SuppressMessage(
         "Usage",
         "VSTHRD002:Synchronously waiting on tasks or awaiters may cause deadlocks",
         Justification = "Required for unit testing.")]
        public void ChangeTemplatesSource(TemplatesSource source, string language, string platform)
        {
            GenContext.Bootstrap(source, new FakeGenShell(platform, language), language);
            GenContext.ToolBox.Repo.SynchronizeAsync(true, true).Wait();
        }

        // Renamed second parameter as this fixture needs the language while others don't
        public override void InitializeFixture(IContextProvider contextProvider, string language = "")
        {
            GenContext.Current = contextProvider;
            Configuration.Current.Environment = "Pro";
            Configuration.Current.CdnUrl = "https://wtsrepository.blob.core.windows.net/pro/";
            InitializeTemplates(Source, language);
        }
    }
}
