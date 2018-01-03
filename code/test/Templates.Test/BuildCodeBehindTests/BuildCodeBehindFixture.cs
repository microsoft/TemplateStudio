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
    public sealed class BuildCodeBehindFixture : BaseGenAndBuildFixture, IDisposable
    {
        private string testExecutionTimeStamp = DateTime.Now.FormatAsDateHoursMinutes();
        public override string GetTestRunPath() => $"{Path.GetPathRoot(Environment.CurrentDirectory)}\\UIT\\CB\\{testExecutionTimeStamp}\\";

        public TemplatesSource Source => new LocalTemplatesSource("TstBldCodeBehind");

        private static bool syncExecuted;

        public static async Task<IEnumerable<object[]>> GetProjectTemplatesAsync(string frameworkFilter, string programmingLanguage)
        {
            List<object[]> result = new List<object[]>();

            var languagesOfInterest = ProgrammingLanguages.GetAllLanguages().ToList();

            if (!string.IsNullOrWhiteSpace(programmingLanguage))
            {
                languagesOfInterest.Clear();
                languagesOfInterest.Add(programmingLanguage);
            }

            foreach (var language in languagesOfInterest)
            {
                await InitializeTemplatesForLanguageAsync(new LocalTemplatesSource("TstBldCodeBehind"), language);

                var projectTemplates = GenContext.ToolBox.Repo.GetAll().Where(t => t.GetTemplateType() == TemplateType.Project
                                                         && t.GetFrameworkList().Contains(frameworkFilter) && t.GetLanguage() == language);

                foreach (var projectTemplate in projectTemplates)
                {
                    var projectTypeList = projectTemplate.GetProjectTypeList();

                    foreach (var projectType in projectTypeList)
                    {
                        var frameworks = GenComposer.GetSupportedFx(projectType).Where(f => f == frameworkFilter);

                        foreach (var framework in frameworks)
                        {
                            result.Add(new object[] { projectType, framework, language });
                        }
                    }
                }
            }

            return result;
        }

        public static async Task<IEnumerable<object[]>> GetPageAndFeatureTemplatesAsync(string frameworkFilter)
        {
            List<object[]> result = new List<object[]>();
            foreach (var language in ProgrammingLanguages.GetAllLanguages())
            {
                await InitializeTemplatesForLanguageAsync(new LocalTemplatesSource("TstBldCodeBehind"), language);

                var projectTemplates = GenContext.ToolBox.Repo.GetAll().Where(t => t.GetTemplateType() == TemplateType.Project
                                                         && t.GetFrameworkList().Contains(frameworkFilter) && t.GetLanguage() == language);

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
