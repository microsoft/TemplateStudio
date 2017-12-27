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

        public TemplatesSourceV2 Source => new LocalTemplatesSourceV2("TemplateTest");

        private static bool syncExecuted;

        public static async Task<IEnumerable<object[]>> GetProjectTemplatesAsync(string frameworkFilter, string programmingLanguage)
        {
            await InitializeTemplatesAsync(new LocalTemplatesSourceV2("TemplateTest"));

            List<object[]> result = new List<object[]>();

            var languagesOfInterest = ProgrammingLanguages.GetAllLanguages().ToList();

            if (!string.IsNullOrWhiteSpace(programmingLanguage))
            {
                languagesOfInterest.Clear();
                languagesOfInterest.Add(programmingLanguage);
            }

            foreach (var language in languagesOfInterest)
            {
                SetCurrentLanguage(language);

                var projectTemplates = GenContext.ToolBox.Repo.GetAll().Where(t => t.GetTemplateType() == TemplateType.Project
                                                         && t.GetLanguage() == language && t.GetFrameworkList().Contains(frameworkFilter));

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
            await InitializeTemplatesAsync(new LocalTemplatesSourceV2("TemplateTest"));

            List<object[]> result = new List<object[]>();
            foreach (var language in ProgrammingLanguages.GetAllLanguages())
            {
                SetCurrentLanguage(language);

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

        private static async Task InitializeTemplatesAsync(TemplatesSourceV2 source)
        {
            GenContext.Bootstrap(source, new FakeGenShell(ProgrammingLanguages.CSharp), ProgrammingLanguages.CSharp);

            if (!syncExecuted)
            {
                await GenContext.ToolBox.Repo.SynchronizeAsync();
                await GenContext.ToolBox.Repo.RefreshAsync(true);
                syncExecuted = true;
            }
        }

        public override async Task InitializeFixtureAsync(IContextProvider contextProvider)
        {
            GenContext.Current = contextProvider;

            await InitializeTemplatesAsync(Source);
        }
    }
}
