// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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

        public static async Task<IEnumerable<object[]>> GetProjectTemplatesAsync()
        {
            List<object[]> result = new List<object[]>();

            foreach (var language in ProgrammingLanguages.GetAllLanguages())
            {
                Configuration.Current.CdnUrl = "https://wtsrepository.blob.core.windows.net/pro/";

                await InitializeTemplatesAsync(new LegacyTemplatesSourceV2(), language);

                var projectTypes = GenContext.ToolBox.Repo.GetAll().Where(t => t.GetTemplateType() == TemplateType.Project
                                                          && t.GetLanguage() == language).SelectMany(p => p.GetProjectTypeList()).Distinct();

                foreach (var projectType in projectTypes)
                {
                    var frameworks = GenComposer.GetSupportedFx(projectType);

                    foreach (var framework in frameworks)
                    {
                        // See https://github.com/Microsoft/WindowsTemplateStudio/issues/1985
                        if (framework == "MVVMLight")
                        {
                            continue;
                        }

                        result.Add(new object[] { projectType, framework, language });
                    }
                }
            }

            return result;
        }

        private static async Task InitializeTemplatesAsync(TemplatesSource source, string language)
        {
            Configuration.Current.CdnUrl = "https://wtsrepository.blob.core.windows.net/pro/";

            GenContext.Bootstrap(source, new FakeGenShell(language), new Version("1.7"), language);
            if (!syncExecuted)
            {
                await GenContext.ToolBox.Repo.SynchronizeAsync(true);

                syncExecuted = true;
            }
        }

        public async Task ChangeTemplatesSourceAsync(TemplatesSource source, string language)
        {
            GenContext.Bootstrap(source, new FakeGenShell(language), language);
            await GenContext.ToolBox.Repo.SynchronizeAsync(true);
        }

        // Renamed second parameter as this fixture needs the language while others don't
        public override async Task InitializeFixtureAsync(IContextProvider contextProvider, string language = "")
        {
            GenContext.Current = contextProvider;
            Configuration.Current.Environment = "Pro";
            Configuration.Current.CdnUrl = "https://wtsrepository.blob.core.windows.net/pro/";
            await InitializeTemplatesAsync(Source, language);
        }
    }
}
