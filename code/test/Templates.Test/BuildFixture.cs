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
    public sealed class BuildFixture : BaseGenAndBuildFixture, IDisposable
    {
        private string testExecutionTimeStamp = DateTime.Now.FormatAsDateHoursMinutes();
        public override string GetTestRunPath() => $"{Path.GetPathRoot(Environment.CurrentDirectory)}\\UIT\\Build\\{testExecutionTimeStamp}\\";

        public TemplatesSource Source => new LocalTemplatesSource("TstBld");

        private static bool syncExecuted;

        public static async Task<IEnumerable<object[]>> GetProjectTemplatesAsync()
        {
            List<object[]> result = new List<object[]>();
            foreach (var language in ProgrammingLanguages.GetAllLanguages())
            {
                await InitializeTemplatesForLanguageAsync(new LocalTemplatesSource("TstBld"), language);

                foreach (var platform in Platforms.GetAllPlatforms())
                {
                    var templateProjectTypes = GenComposer.GetSupportedProjectTypes(platform);

                    var projectTypes = GenContext.ToolBox.Repo.GetProjectTypes(platform)
                                .Where(m => templateProjectTypes.Contains(m.Name) && !string.IsNullOrEmpty(m.Description))
                                .Select(m => m.Name);

                    foreach (var projectType in projectTypes)
                    {
                        var projectFrameworks = GenComposer.GetSupportedFx(projectType, platform);

                        var targetFrameworks = GenContext.ToolBox.Repo.GetFrameworks(platform)
                                                    .Where(m => projectFrameworks.Contains(m.Name))
                                                    .Select(m => m.Name).ToList();

                        foreach (var framework in targetFrameworks)
                        {
                            result.Add(new object[] { projectType, framework, platform, language });
                        }
                    }
                }
            }

            return result;
        }

        public static async Task<IEnumerable<object[]>> GetPageAndFeatureTemplatesAsync()
        {
            List<object[]> result = new List<object[]>();
            foreach (var language in ProgrammingLanguages.GetAllLanguages())
            {
                await InitializeTemplatesForLanguageAsync(new LocalTemplatesSource("TstBld"), language);

                foreach (var platform in Platforms.GetAllPlatforms())
                {
                    var templateProjectTypes = GenComposer.GetSupportedProjectTypes(platform);

                    var projectTypes = GenContext.ToolBox.Repo.GetProjectTypes(platform)
                                .Where(m => templateProjectTypes.Contains(m.Name) && !string.IsNullOrEmpty(m.Description))
                                .Select(m => m.Name);

                    foreach (var projectType in projectTypes)
                    {
                        var projectFrameworks = GenComposer.GetSupportedFx(projectType, platform);

                        var targetFrameworks = GenContext.ToolBox.Repo.GetFrameworks(platform)
                                                    .Where(m => projectFrameworks.Contains(m.Name))
                                                    .Select(m => m.Name).ToList();

                        foreach (var framework in targetFrameworks)
                        {
                            var itemTemplates = GenContext.ToolBox.Repo.GetAll().Where(t => t.GetFrameworkList().Contains(framework)
                                                                 && (t.GetTemplateType() == TemplateType.Page || t.GetTemplateType() == TemplateType.Feature)
                                                                 && t.GetPlatform() == platform
                                                                 && t.GetLanguage() == language
                                                                 && !t.GetIsHidden());

                            foreach (var itemTemplate in itemTemplates)
                            {
                                result.Add(new object[]
                                    { itemTemplate.Name, projectType, framework, itemTemplate.Identity, language });
                            }
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

        public override async Task InitializeFixtureAsync(string language, IContextProvider contextProvider)
        {
            GenContext.Current = contextProvider;

            await InitializeTemplatesForLanguageAsync(Source, language);
        }
    }
}
