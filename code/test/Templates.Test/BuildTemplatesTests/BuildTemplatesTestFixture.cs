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
    public sealed class BuildTemplatesTestFixture : BaseGenAndBuildFixture, IDisposable
    {
        private string _testExecutionTimeStamp = DateTime.Now.FormatAsDateHoursMinutes();

        private string _framework;

        private static Dictionary<string, bool> syncExecuted = new Dictionary<string, bool>();

        public override string GetTestRunPath() => $"{Path.GetPathRoot(Environment.CurrentDirectory)}\\UIT\\{ShortFrameworkName(_framework)}\\{_testExecutionTimeStamp}\\";

        public TemplatesSource Source => new LocalTemplatesSource(ShortFrameworkName(_framework));

        public static async Task<IEnumerable<object[]>> GetProjectTemplatesAsync(string frameworkFilter, string programmingLanguage)
        {
            await InitializeTemplatesAsync(new LocalTemplatesSource(ShortFrameworkName(frameworkFilter)));

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
            await InitializeTemplatesAsync(new LocalTemplatesSource(ShortFrameworkName(frameworkFilter)));

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

        private static async Task InitializeTemplatesAsync(TemplatesSource source)
        {
            GenContext.Bootstrap(source, new FakeGenShell(ProgrammingLanguages.CSharp), ProgrammingLanguages.CSharp);

            if (syncExecuted.ContainsKey(source.Id) && syncExecuted[ShortFrameworkName(source.Id)] == true)
            {
                return;
            }

            await GenContext.ToolBox.Repo.SynchronizeAsync(true);

            syncExecuted.Add(source.Id, true);
        }

        public override async Task InitializeFixtureAsync(IContextProvider contextProvider, string framework)
        {
            GenContext.Current = contextProvider;
            _framework = framework;

            await InitializeTemplatesAsync(Source);
        }

        private static string ShortFrameworkName(string framework)
        {
            switch (framework)
            {
                case "CaliburnMicro":
                    return "CM";
                case "Prism":
                    return "P";
                case "CodeBehind":
                    return "CB";
                case "MVVMLight":
                    return "ML";
                case "MVVMBasic":
                    return "MB";
                default:
                    return framework;
            }
        }
    }
}
