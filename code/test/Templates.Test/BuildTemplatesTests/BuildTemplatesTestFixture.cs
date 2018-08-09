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

        public static IEnumerable<object[]> GetProjectTemplates(string frameworkFilter, string programmingLanguage, string selectedPlatform)
        {
            InitializeTemplates(new LocalTemplatesSource(ShortFrameworkName(frameworkFilter)));

            List<object[]> result = new List<object[]>();

            var languagesOfInterest = ProgrammingLanguages.GetAllLanguages().ToList();

            if (!string.IsNullOrWhiteSpace(programmingLanguage))
            {
                languagesOfInterest.Clear();
                languagesOfInterest.Add(programmingLanguage);
            }

            var platformsOfInterest = Platforms.GetAllPlatforms().ToList();

            if (!string.IsNullOrWhiteSpace(selectedPlatform))
            {
                platformsOfInterest.Clear();
                platformsOfInterest.Add(selectedPlatform);
            }

            foreach (var language in languagesOfInterest)
            {
                SetCurrentLanguage(language);
                foreach (var platform in platformsOfInterest)
                {
                    SetCurrentPlatform(platform);
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
                            result.Add(new object[] { projectType, framework, platform, language });
                        }
                    }
                }
            }

            return result;
        }

        public static IEnumerable<object[]> GetPageAndFeatureTemplatesForBuild(string frameworkFilter)
        {
            InitializeTemplates(new LocalTemplatesSource(ShortFrameworkName(frameworkFilter)));

            return BaseGenAndBuildFixture.GetPageAndFeatureTemplates(frameworkFilter);
        }

        [SuppressMessage(
         "Usage",
         "VSTHRD002:Synchronously waiting on tasks or awaiters may cause deadlocks",
         Justification = "Required for unit testing.")]
        private static void InitializeTemplates(TemplatesSource source)
        {
            GenContext.Bootstrap(source, new FakeGenShell(Platforms.Uwp, ProgrammingLanguages.CSharp), ProgrammingLanguages.CSharp);

            if (syncExecuted.ContainsKey(source.Id) && syncExecuted[ShortFrameworkName(source.Id)] == true)
            {
                return;
            }

            GenContext.ToolBox.Repo.SynchronizeAsync(true).Wait();

            syncExecuted.Add(source.Id, true);
        }

        public override void InitializeFixture(IContextProvider contextProvider, string framework)
        {
            GenContext.Current = contextProvider;
            _framework = framework;

            InitializeTemplates(Source);
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
