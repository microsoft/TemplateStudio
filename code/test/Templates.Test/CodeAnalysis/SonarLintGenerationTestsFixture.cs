// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;

using Microsoft.Templates.Core;
using Microsoft.Templates.Core.Gen;
using Microsoft.Templates.Core.Locations;
using Microsoft.Templates.Fakes;
using Microsoft.Templates.UI;

namespace Microsoft.Templates.Test
{
    public sealed class SonarLintGenerationTestsFixture : BaseGenAndBuildFixture, IDisposable
    {
        private string _testExecutionTimeStamp = DateTime.Now.FormatAsDateHoursMinutes();

        private static bool syncExecuted = false;

        public override string GetTestRunPath() => $"{Path.GetPathRoot(Environment.CurrentDirectory)}\\UIT\\SL\\{_testExecutionTimeStamp}\\";

        public TemplatesSource Source => new LocalTemplatesSource("SonarLint");

        [SuppressMessage(
         "Usage",
         "VSTHRD002:Synchronously waiting on tasks or awaiters may cause deadlocks",
         Justification = "Required for unit testing.")]
        private static void InitializeTemplates(TemplatesSource source)
        {
            GenContext.Bootstrap(source, new FakeGenShell(Platforms.Uwp, ProgrammingLanguages.VisualBasic), ProgrammingLanguages.VisualBasic);

            if (!syncExecuted == true)
            {
                GenContext.ToolBox.Repo.SynchronizeAsync(true).Wait();
                syncExecuted = true;
            }
        }

        public override void InitializeFixture(IContextProvider contextProvider, string framework = "")
        {
            GenContext.Current = contextProvider;

            InitializeTemplates(Source);
        }

        public static IEnumerable<object[]> GetProjectTemplatesForSonarLint()
        {
            InitializeTemplates(new LocalTemplatesSource("SonarLint"));

            List<object[]> result = new List<object[]>();

            var platform = Platforms.Uwp;

            var projectTemplates =
               GenContext.ToolBox.Repo.GetAll().Where(
                   t => t.GetTemplateType() == TemplateType.Project
                    && t.GetLanguage() == ProgrammingLanguages.VisualBasic);

            foreach (var projectTemplate in projectTemplates)
            {
                var projectTypeList = projectTemplate.GetProjectTypeList();

                foreach (var projectType in projectTypeList)
                {
                    var frameworks = GenComposer.GetSupportedFx(projectType, platform);

                    foreach (var framework in frameworks)
                    {
                        result.Add(new object[] { projectType, framework, platform });
                    }
                }
            }

            return result;
        }
    }
}
