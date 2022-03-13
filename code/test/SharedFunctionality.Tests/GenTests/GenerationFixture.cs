// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using Microsoft.Templates.Core;
using Microsoft.Templates.Core.Gen;
using Microsoft.Templates.Core.Locations;
using Microsoft.Templates.Fakes.GenShell;

namespace Microsoft.Templates.Test
{
    public class GenerationFixture : BaseGenAndBuildFixture, IDisposable
    {
        private string _testExecutionTimeStamp = DateTime.Now.FormatAsDateHoursMinutes();
        public override string GetTestRunPath() => $"{Path.GetPathRoot(Environment.CurrentDirectory)}\\UIT\\Gen\\{_testExecutionTimeStamp}\\";

        public static TemplatesSource Source = new LocalTemplatesSource(null, "TestGen");

        public static string AppPlatform = "not set -- override me";

        public static string ProgrammingLanguage = "not set -- override me";

        private static Dictionary<string, bool> syncExecuted = new Dictionary<string, bool>();

        public static IEnumerable<object[]> GetProjectTemplates()
        {
            InitializeTemplates(Source);
            return GetAllProjectTemplates();
        }

        public static IEnumerable<object[]> GetPageAndFeatureTemplatesForGeneration(string frameworkFilter)
        {
            InitializeTemplates(Source);

            return BaseGenAndBuildFixture.GetPageAndFeatureTemplates(frameworkFilter);
        }

        [SuppressMessage(
         "Usage",
         "VSTHRD002:Synchronously waiting on tasks or awaiters may cause deadlocks",
         Justification = "Required for unit testing.")]
        private static void InitializeTemplates(TemplatesSource source)
        {
            if (syncExecuted.ContainsKey(source.Id) && syncExecuted[source.Id] == true)
            {
                return;
            }

            GenContext.Bootstrap(source, new FakeGenShell(AppPlatform, ProgrammingLanguage), AppPlatform, ProgrammingLanguage, TestConstants.TemplateVersionNumber);

            //GenContext.ToolBox.Repo.SynchronizeAsync(true).Wait();
            syncExecuted.Add(source.Id, true);
        }

        public override void InitializeFixture(IContextProvider contextProvider, string framework = "")
        {
            GenContext.Current = contextProvider;

            InitializeTemplates(Source);
        }
    }
}
