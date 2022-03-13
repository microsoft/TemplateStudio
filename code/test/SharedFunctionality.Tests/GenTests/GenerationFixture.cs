// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Templates.Core;
using Microsoft.Templates.Core.Gen;

namespace Microsoft.Templates.Test
{
    public class GenerationFixture : BaseGenAndBuildFixture, IDisposable
    {
        private string _testExecutionTimeStamp = DateTime.Now.FormatAsDateHoursMinutes();
        public override string GetTestRunPath() => $"{Path.GetPathRoot(Environment.CurrentDirectory)}\\UIT\\Gen\\{_testExecutionTimeStamp}\\";

        public static string AppPlatform = "not set -- override me";

        public static string ProgrammingLanguage = "not set -- override me";

        public static IEnumerable<object[]> GetProjectTemplates()
        {
            // This throws because nothing should be calling this directly as it doesn't have a specific TemplatesSource
            throw new NotImplementedException();
        }

        public static IEnumerable<object[]> GetPageAndFeatureTemplatesForGeneration(string frameworkFilter)
        {
            // This throws because nothing should be calling this directly as it doesn't have a specific TemplatesSource
            throw new NotImplementedException();
        }

        public override void InitializeFixture(IContextProvider contextProvider, string framework = "")
        {
            GenContext.Current = contextProvider;

            InitializeTemplates(Source, ProgrammingLanguages.CSharp);
        }
    }
}
