// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

using Microsoft.TemplateEngine.Abstractions;
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

            // TODO: X-Ref https://github.com/Microsoft/WindowsTemplateStudio/issues/1325
            // Re-enable for all languages
            ////foreach (var language in ProgrammingLanguages.GetAllLanguages())
            {
                const string language = ProgrammingLanguages.CSharp;
                Configuration.Current.CdnUrl = "https://wtsrepository.blob.core.windows.net/pro/";

                await InitializeTemplatesAsync(new LegacyTemplatesSourceV2());

                var projectTypes = GenContext.ToolBox.Repo.GetAll().Where(t => t.GetTemplateType() == TemplateType.Project
                                                          && t.GetLanguage() == language).SelectMany(p => p.GetProjectTypeList()).Distinct();

                foreach (var projectType in projectTypes)
                {
                    var frameworks = GenComposer.GetSupportedFx(projectType);

                    foreach (var framework in frameworks)
                    {
                        result.Add(new object[] { projectType, framework, language });
                    }
                }
            }

            return result;
        }

        private static async Task InitializeTemplatesAsync(TemplatesSource source)
        {
            Configuration.Current.CdnUrl = "https://wtsrepository.blob.core.windows.net/pro/";

            GenContext.Bootstrap(source, new FakeGenShell(ProgrammingLanguages.CSharp), new Version("1.6"), ProgrammingLanguages.CSharp);
            if (!syncExecuted)
            {
                await GenContext.ToolBox.Repo.SynchronizeAsync(true);

                syncExecuted = true;
            }
        }

        public async Task ChangeTemplatesSourceAsync(TemplatesSource source)
        {
            GenContext.Bootstrap(source, new FakeGenShell(ProgrammingLanguages.CSharp), ProgrammingLanguages.CSharp);
            await GenContext.ToolBox.Repo.SynchronizeAsync(true);
        }

        public override async Task InitializeFixtureAsync(IContextProvider contextProvider, string framework = "")
        {
            GenContext.Current = contextProvider;
            Configuration.Current.Environment = "Pro";
            Configuration.Current.CdnUrl = "https://wtsrepository.blob.core.windows.net/pro/";
            await InitializeTemplatesAsync(Source);
        }
    }
}
