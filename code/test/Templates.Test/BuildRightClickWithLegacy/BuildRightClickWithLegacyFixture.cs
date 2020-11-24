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
using Microsoft.CodeAnalysis;
using Microsoft.Templates.Core;
using Microsoft.Templates.Core.Gen;
using Microsoft.Templates.Core.Locations;
using Microsoft.Templates.Fakes;

namespace Microsoft.Templates.Test.BuildWithLegacy
{
    public abstract class BuildRightClickWithLegacyFixture : BaseGenAndBuildFixture, IDisposable
    {
        private readonly string testExecutionTimeStamp = DateTime.Now.FormatAsDateHoursMinutes();
        public override string GetTestRunPath() => $"{Path.GetPathRoot(Environment.CurrentDirectory)}\\UIT\\LEG\\{testExecutionTimeStamp}\\";

        public TemplatesSource Source => new LegacyTemplatesSourceV2(Platform, Language);

        public TemplatesSource LocalSource => new LocalTemplatesSource(null, "BldRClickLegacy");

        public abstract  string Platform { get; }
        public abstract string Language { get; }

        private static Dictionary<string, bool> syncExecuted = new Dictionary<string, bool>();

        public static IEnumerable<object[]> GetProjectTemplates(string platform, string language)
        {
            List<object[]> result = new List<object[]>();

            Configuration.Current.CdnUrl = "https://wtsrepository.blob.core.windows.net/pro/";

            InitializeTemplates(new LegacyTemplatesSourceV2(platform, language), platform, language);

            var projectTypes = GenContext.ToolBox.Repo.GetProjectTypes(platform)
                        .Where(m => !string.IsNullOrEmpty(m.Description))
                        .Select(m => m.Name);

            foreach (var projectType in projectTypes)
            {
                var targetFrameworks = GenContext.ToolBox.Repo.GetFrontEndFrameworks(platform, projectType)
                                            .Select(m => m.Name).ToList();

                foreach (var framework in targetFrameworks)
                {
                    result.Add(new object[] { projectType, framework, platform, language });
                }
            }  

            return result;
        }

        [SuppressMessage(
 "Usage",
 "VSTHRD002:Synchronously waiting on tasks or awaiters may cause deadlocks",
 Justification = "Required for unit testing.")]
        private static void InitializeTemplates(TemplatesSource source, string platform, string language)
        {
            Configuration.Current.CdnUrl = "https://wtsrepository.blob.core.windows.net/pro/";

            source.LoadConfigAsync(default).Wait();
            var version = new Version(source.Config.Latest.Version.Major, source.Config.Latest.Version.Minor);

            if (syncExecuted.ContainsKey(platform + language) && syncExecuted[platform + language] == true)
            {
                return;
            }

            GenContext.Bootstrap(source, new FakeGenShell(platform, language), version, platform, language);

            GenContext.ToolBox.Repo.SynchronizeAsync(true, true).Wait();

            syncExecuted.Add(platform + language, true);

        }


        public async Task ChangeToLocalTemplatesSourceAsync()
        {
            GenContext.Bootstrap(LocalSource, new FakeGenShell(Platform, Language), Platform, Language);
            await GenContext.ToolBox.Repo.SynchronizeAsync(true, true);
        }

        // Renamed second parameter as this fixture needs the language while others don't
        public override void InitializeFixture(IContextProvider contextProvider, string language = "")
        {
            GenContext.Current = contextProvider;
            Configuration.Current.Environment = "Pro";
            Configuration.Current.CdnUrl = "https://wtsrepository.blob.core.windows.net/pro/";
            InitializeTemplates(Source, Platform, Language);
        }
    }
}
