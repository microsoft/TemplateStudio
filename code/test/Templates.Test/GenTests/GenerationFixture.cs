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

namespace Microsoft.Templates.Test
{
    public sealed class GenerationFixture : BaseGenAndBuildFixture, IDisposable
    {
        private string _testExecutionTimeStamp = DateTime.Now.FormatAsDateHoursMinutes();
        public override string GetTestRunPath() => $"{Path.GetPathRoot(Environment.CurrentDirectory)}\\UIT\\Gen\\{_testExecutionTimeStamp}\\";

        public TemplatesSource Source => new LocalTemplatesSource(null, "TestGen");

        private static bool syncExecuted;

        public static IEnumerable<object[]> GetProjectTemplates()
        {
            InitializeTemplates(new LocalTemplatesSource(null, "TestGen"));
            return GetAllProjectTemplates();
        }

        public static IEnumerable<object[]> GetPageAndFeatureTemplatesForGeneration(string frameworkFilter)
        {
            InitializeTemplates(new LocalTemplatesSource(null, "TestGen"));

            return BaseGenAndBuildFixture.GetPageAndFeatureTemplates(frameworkFilter);
        }

        [SuppressMessage(
         "Usage",
         "VSTHRD002:Synchronously waiting on tasks or awaiters may cause deadlocks",
         Justification = "Required for unit testing.")]
        private static void InitializeTemplates(TemplatesSource source)
        {

            if (!syncExecuted)
            {
                GenContext.Bootstrap(source, new FakeGenShell(Platforms.Uwp, ProgrammingLanguages.CSharp), Platforms.Uwp, ProgrammingLanguages.CSharp);

                GenContext.ToolBox.Repo.SynchronizeAsync(true).Wait();
                syncExecuted = true;
            }
        }

        public override void InitializeFixture(IContextProvider contextProvider, string framework = "")
        {
            GenContext.Current = contextProvider;

            InitializeTemplates(Source);
        }
    }
}
