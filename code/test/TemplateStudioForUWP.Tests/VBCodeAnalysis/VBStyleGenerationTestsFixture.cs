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
    public sealed class VBStyleGenerationTestsFixture : BaseGenAndBuildFixture, IDisposable
    {
        private readonly string _testExecutionTimeStamp = DateTime.Now.FormatAsDateHoursMinutes();

        private static bool syncExecuted = false;

        public override string GetTestRunPath() => $"{Path.GetPathRoot(Environment.CurrentDirectory)}\\UIT\\VBSA\\{_testExecutionTimeStamp}\\";

        private static readonly TemplatesSource StaticSource = new UwpTestsTemplatesSource("VBStyle");

        public TemplatesSource Source => StaticSource;

        [SuppressMessage(
   "Usage",
   "VSTHRD002:Synchronously waiting on tasks or awaiters may cause deadlocks",
   Justification = "Required for unit testing.")]
        private static void InitializeTemplates(TemplatesSource source)
        {    
            if (!syncExecuted == true)
            {
                GenContext.Bootstrap(source, new FakeGenShell(Platforms.Uwp, ProgrammingLanguages.VisualBasic), Platforms.Uwp, ProgrammingLanguages.VisualBasic, TestConstants.TemplateVersionNumber);

            //    GenContext.ToolBox.Repo.SynchronizeAsync(true).Wait();
                syncExecuted = true;
            }
        }

        public override void InitializeFixture(IContextProvider contextProvider, string framework = "")
        {
            GenContext.Current = contextProvider;

            InitializeTemplates(Source);
        }

        public static IEnumerable<object[]> GetProjectTemplatesForVBStyle()
        {
            InitializeTemplates(StaticSource);
            return GetVBProjectTemplates();
        }
    }
}
