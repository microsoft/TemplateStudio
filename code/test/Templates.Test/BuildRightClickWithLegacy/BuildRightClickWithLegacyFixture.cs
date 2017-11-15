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

        public TemplatesSource Source => new LegacyTemplatesSource();
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

                await InitializeTemplatesForLanguageAsync(new LegacyTemplatesSource(), language);

                // TODO: Re-enable for all platforms
                ////foreach (var language in Platforms.GetAllPlarforms())
                var projectTypes = GenContext.ToolBox.Repo.GetProjectTypes()
                            .Where(m => !string.IsNullOrEmpty(m.Description))
                            .Select(m => m.Name);

                foreach (var projectType in projectTypes)
                {
                    var projectFrameworks = GenComposer.GetSupportedFx(projectType, string.Empty);

                    var targetFrameworks = GenContext.ToolBox.Repo.GetFrameworks()
                                                .Where(m => projectFrameworks.Contains(m.Name))
                                                .Select(m => m.Name).ToList();

                    foreach (var framework in targetFrameworks)
                    {
                        result.Add(new object[] { projectType, framework, Platforms.Uwp, language });
                    }
                }
            }

            return result;
        }

        private static async Task InitializeTemplatesForLanguageAsync(TemplatesSource source, string language)
        {
            GenContext.Bootstrap(source, new FakeGenShell(language), language);

            if (!syncExecuted)
            {
                await GenContext.ToolBox.Repo.SynchronizeAsync();
                syncExecuted = true;
            }
            else
            {
                await GenContext.ToolBox.Repo.RefreshAsync();
            }
        }

        public async Task ChangeTemplatesSourceAsync(TemplatesSource source, string language)
        {
            GenContext.Bootstrap(source, new FakeGenShell(language), language);
            await GenContext.ToolBox.Repo.SynchronizeAsync();
        }

        public override async Task InitializeFixtureAsync(string language, IContextProvider contextProvider)
        {
            GenContext.Current = contextProvider;

            await InitializeTemplatesForLanguageAsync(Source, language);
        }
    }
}
