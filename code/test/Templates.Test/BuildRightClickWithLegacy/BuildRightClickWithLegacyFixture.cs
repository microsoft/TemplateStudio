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
        public TemplatesSource LocalSource => new LocalTemplatesSource("BuildRightClickWithLegacy");

        private static bool syncExecuted;

        public static async Task<IEnumerable<object[]>> GetProjectTemplatesAsync()
        {
            List<object[]> result = new List<object[]>();
            foreach (var language in ProgrammingLanguages.GetAllLanguages())
            {
                await InitializeTemplatesForLanguageAsync(new LegacyTemplatesSource(), language);

                var projectTemplates = GenContext.ToolBox.Repo.GetAll().Where(t => t.GetTemplateType() == TemplateType.Project
                                                         && t.GetLanguage() == language);

                foreach (var projectTemplate in projectTemplates)
                {
                    var projectTypeList = projectTemplate.GetProjectTypeList();

                    foreach (var projectType in projectTypeList)
                    {
                        var frameworks = GenComposer.GetSupportedFx(projectType);

                        foreach (var framework in frameworks)
                        {
                            result.Add(new object[] { projectType, framework, language });
                        }
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
