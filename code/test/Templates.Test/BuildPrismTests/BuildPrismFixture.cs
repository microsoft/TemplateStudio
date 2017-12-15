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
    public sealed class BuildPrismFixture : BaseGenAndBuildFixture, IDisposable
    {
        private string testExecutionTimeStamp = DateTime.Now.FormatAsDateHoursMinutes();
        public override string GetTestRunPath() => $"{Path.GetPathRoot(Environment.CurrentDirectory)}\\UIT\\P\\{testExecutionTimeStamp}\\";

        public TemplatesSource Source => new LocalTemplatesSource("TstBldPrism");

        private static bool syncExecuted;

        public static async Task<IEnumerable<object[]>> GetProjectTemplatesAsync(string frameworkFilter)
        {
            List<object[]> result = new List<object[]>();

            foreach (var platform in Platforms.GetAllPlatforms())
            {
                await InitializeTemplatesForLanguageAsync(new LocalTemplatesSource("TstBldPrism"), platform, ProgrammingLanguages.CSharp);

                var templateProjectTypes = GenComposer.GetSupportedProjectTypes(platform);

                var projectTypes = GenContext.ToolBox.Repo.GetProjectTypes(platform)
                            .Where(m => templateProjectTypes.Contains(m.Name) && !string.IsNullOrEmpty(m.Description))
                            .Select(m => m.Name);

                foreach (var projectType in projectTypes)
                {
                    var projectFrameworks = GenComposer.GetSupportedFx(projectType, platform);

                    var targetFrameworks = GenContext.ToolBox.Repo.GetFrameworks(platform)
                                                .Where(m => projectFrameworks.Contains(m.Name) && m.Name == frameworkFilter)
                                                .Select(m => m.Name)
                                                .ToList();

                    foreach (var framework in targetFrameworks)
                    {
                        result.Add(new object[] { projectType, framework, platform, ProgrammingLanguages.CSharp });
                    }
                }
            }

            return result;
        }

        public static async Task<IEnumerable<object[]>> GetPageAndFeatureTemplatesAsync(string frameworkFilter)
        {
            List<object[]> result = new List<object[]>();

            foreach (var platform in Platforms.GetAllPlatforms())
            {
                await InitializeTemplatesForLanguageAsync(new LocalTemplatesSource("TstBldPrism"), platform, ProgrammingLanguages.CSharp);

                var templateProjectTypes = GenComposer.GetSupportedProjectTypes(platform);

                var projectTypes = GenContext.ToolBox.Repo.GetProjectTypes(platform)
                            .Where(m => templateProjectTypes.Contains(m.Name) && !string.IsNullOrEmpty(m.Description))
                            .Select(m => m.Name);

                foreach (var projectType in projectTypes)
                {
                    var projectFrameworks = GenComposer.GetSupportedFx(projectType, platform);

                    var targetFrameworks = GenContext.ToolBox.Repo.GetFrameworks(platform)
                                                .Where(m => projectFrameworks.Contains(m.Name) && m.Name == frameworkFilter)
                                                .Select(m => m.Name).ToList();

                    foreach (var framework in targetFrameworks)
                    {
                        var itemTemplates = GenContext.ToolBox.Repo.GetAll().Where(t => t.GetFrameworkList().Contains(framework)
                                                             && (t.GetTemplateType() == TemplateType.Page || t.GetTemplateType() == TemplateType.Feature)
                                                             && t.GetPlatform() == platform
                                                             && !t.GetIsHidden());

                        foreach (var itemTemplate in itemTemplates)
                        {
                            result.Add(new object[]
                                { itemTemplate.Name, projectType, framework, platform, itemTemplate.Identity, ProgrammingLanguages.CSharp });
                        }
                    }
                }
            }

            return result;
        }

        private static async Task InitializeTemplatesForLanguageAsync(TemplatesSource source, string platform, string language)
        {
            GenContext.Bootstrap(source, new FakeGenShell(platform, language), language);

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

        public override async Task InitializeFixtureAsync(string platform, string language, IContextProvider contextProvider)
        {
            GenContext.Current = contextProvider;

            await InitializeTemplatesForLanguageAsync(Source, platform, language);
        }
    }
}
