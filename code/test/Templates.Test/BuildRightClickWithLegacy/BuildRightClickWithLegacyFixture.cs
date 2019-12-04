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

namespace Microsoft.Templates.Test
{
    public sealed class BuildRightClickWithLegacyFixture : BaseGenAndBuildFixture, IDisposable
    {
        private string testExecutionTimeStamp = DateTime.Now.FormatAsDateHoursMinutes();
        public override string GetTestRunPath() => $"{Path.GetPathRoot(Environment.CurrentDirectory)}\\UIT\\LEG\\{testExecutionTimeStamp}\\";

        public TemplatesSource Source => new LegacyTemplatesSourceV2(ProgrammingLanguages.CSharp);

        public TemplatesSource VBSource => new LegacyTemplatesSourceV2(ProgrammingLanguages.VisualBasic);

        public TemplatesSource LocalSource => new LocalTemplatesSource(null, "BldRClickLegacy");

        private static Dictionary<string, bool> syncExecuted = new Dictionary<string, bool>();

        public static IEnumerable<object[]> GetProjectTemplates()
        {
            List<object[]> result = new List<object[]>();

            foreach (var language in ProgrammingLanguages.GetAllLanguages())
            {
                if (language == ProgrammingLanguages.Any)
                {
                    continue;
                }

                Configuration.Current.CdnUrl = "https://wtsrepository.blob.core.windows.net/pro/";

                InitializeTemplates(new LegacyTemplatesSourceV2(language), language);

                var projectTypes = GenContext.ToolBox.Repo.GetProjectTypes(Platforms.Uwp)
                            .Where(m => !string.IsNullOrEmpty(m.Description))
                            .Select(m => m.Name);

                foreach (var projectType in projectTypes)
                {
                    var targetFrameworks = GenContext.ToolBox.Repo.GetFrontEndFrameworks(Platforms.Uwp, projectType)
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

            if (syncExecuted.ContainsKey(language) && syncExecuted[language] == true)
            {
                return;
            }

            GenContext.Bootstrap(source, new FakeGenShell(Platforms.Uwp, language), version, Platforms.Uwp, language);

            GenContext.ToolBox.Repo.SynchronizeAsync(true, true).Wait();

            syncExecuted.Add(language,true);
            
        }


        public async Task ChangeToLocalTemplatesSource(TemplatesSource source, string language, string platform)
        {
            GenContext.Bootstrap(source, new FakeGenShell(platform, language), platform, language);
            await GenContext.ToolBox.Repo.SynchronizeAsync(true, true);
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
