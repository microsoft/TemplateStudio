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
        private string _testExecutionTimeStamp = DateTime.Now.FormatAsDateHoursMinutes();

        public override string GetTestRunPath() => $"{Path.GetPathRoot(Environment.CurrentDirectory)}\\UIT\\Build\\{_testExecutionTimeStamp}\\";

        public TemplatesSource Source => new LocalTemplatesSource("TstBld");

        private static bool syncExecuted;

        public static async Task<IEnumerable<object[]>> GetProjectTemplatesAsync()
        {
            await InitializeTemplatesAsync(new LocalTemplatesSource("TstBld"));

            List<object[]> result = new List<object[]>();
            foreach (var language in ProgrammingLanguages.GetAllLanguages())
            {
                SetCurrentLanguage(language);
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

        public static async Task<IEnumerable<object[]>> GetPageAndFeatureTemplatesAsync()
        {
            await InitializeTemplatesAsync(new LocalTemplatesSource("TstBld"));

            List<object[]> result = new List<object[]>();
            foreach (var language in ProgrammingLanguages.GetAllLanguages())
            {
                var projectTypes = GenContext.ToolBox.Repo.GetAll().Where(t => t.GetTemplateType() == TemplateType.Project
                                                          && t.GetLanguage() == language).SelectMany(p => p.GetProjectTypeList()).Distinct();

                foreach (var projectType in projectTypes)
                {
                    var frameworks = GenComposer.GetSupportedFx(projectType);

                    foreach (var framework in frameworks)
                    {
                        var itemTemplates = GenContext.ToolBox.Repo.GetAll().Where(t => t.GetFrameworkList().Contains(framework)
                                                                && (t.GetTemplateType() == TemplateType.Page || t.GetTemplateType() == TemplateType.Feature)
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

            return result;
        }

        private static async Task InitializeTemplatesAsync(TemplatesSource source)
        {
            GenContext.Bootstrap(source, new FakeGenShell(ProgrammingLanguages.CSharp), ProgrammingLanguages.CSharp);

            if (!syncExecuted)
            {
                await GenContext.ToolBox.Repo.SynchronizeAsync(true);
                syncExecuted = true;
            }
        }

        public override async Task InitializeFixtureAsync(IContextProvider contextProvider, string framework = "")
        {
            GenContext.Current = contextProvider;

            await InitializeTemplatesAsync(Source);
        }
    }
}
