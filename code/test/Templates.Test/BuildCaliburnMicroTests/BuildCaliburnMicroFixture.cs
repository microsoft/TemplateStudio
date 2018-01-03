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
    public sealed class BuildCaliburnMicroFixture : BaseGenAndBuildFixture, IDisposable
    {
        private string testExecutionTimeStamp = DateTime.Now.FormatAsDateHoursMinutes();
        public override string GetTestRunPath() => $"{Path.GetPathRoot(Environment.CurrentDirectory)}\\UIT\\CM\\{testExecutionTimeStamp}\\";

        public TemplatesSource Source => new LocalTemplatesSource("TstBldCaliburn");

        private static bool syncExecuted;

        public static async Task<IEnumerable<object[]>> GetProjectTemplatesAsync(string frameworkFilter)
        {
            List<object[]> result = new List<object[]>();

            await InitializeTemplatesForLanguageAsync(new LocalTemplatesSource("TstBldCaliburn"), ProgrammingLanguages.CSharp);

            var projectTemplates = GenContext.ToolBox.Repo.GetAll().Where(t => t.GetTemplateType() == TemplateType.Project
                                                                            && t.GetFrameworkList().Contains(frameworkFilter)
                                                                            && t.GetLanguage() == ProgrammingLanguages.CSharp);

            foreach (var projectTemplate in projectTemplates)
            {
                var projectTypeList = projectTemplate.GetProjectTypeList();

                foreach (var projectType in projectTypeList)
                {
                    var frameworks = GenComposer.GetSupportedFx(projectType).Where(f => f == frameworkFilter);

                    foreach (var framework in frameworks)
                    {
                        result.Add(new object[] { projectType, framework, ProgrammingLanguages.CSharp });
                    }
                }
            }

            return result;
        }

        public static async Task<IEnumerable<object[]>> GetPageAndFeatureTemplatesAsync(string frameworkFilter)
        {
            List<object[]> result = new List<object[]>();

            await InitializeTemplatesForLanguageAsync(new LocalTemplatesSource("TstBldCaliburn"), ProgrammingLanguages.CSharp);

            var projectTemplates = GenContext.ToolBox.Repo.GetAll().Where(t => t.GetTemplateType() == TemplateType.Project
                                                                            && t.GetFrameworkList().Contains(frameworkFilter)
                                                                            && t.GetLanguage() == ProgrammingLanguages.CSharp);

            foreach (var projectTemplate in projectTemplates)
            {
                var projectTypeList = projectTemplate.GetProjectTypeList();

                foreach (var projectType in projectTypeList)
                {
                    var frameworks = GenComposer.GetSupportedFx(projectType).Where(f => f == frameworkFilter);

                    foreach (var framework in frameworks)
                    {
                        var itemTemplates = GenContext.ToolBox.Repo.GetAll().Where(t => t.GetFrameworkList().Contains(framework)
                                                                && (t.GetTemplateType() == TemplateType.Page || t.GetTemplateType() == TemplateType.Feature)
                                                                && t.GetLanguage() == ProgrammingLanguages.CSharp
                                                                && !t.GetIsHidden());

                        foreach (var itemTemplate in itemTemplates)
                        {
                            result.Add(new object[]
                                { itemTemplate.Name, projectType, framework, itemTemplate.Identity, ProgrammingLanguages.CSharp });
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
