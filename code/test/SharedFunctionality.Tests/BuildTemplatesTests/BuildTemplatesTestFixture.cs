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
    public class BuildTemplatesTestFixture : BaseGenAndBuildFixture, IDisposable
    {
        private string _testExecutionTimeStamp = DateTime.Now.FormatAsDateHoursMinutes();

        private string _framework;

        private static Dictionary<string, bool> syncExecuted = new Dictionary<string, bool>();

        public override string GetTestRunPath() => $"{Path.GetPathRoot(Environment.CurrentDirectory)}\\UIT\\{ShortFrameworkName(_framework)}\\{_testExecutionTimeStamp}\\";

        public override void InitializeFixture(IContextProvider contextProvider, string framework)
        {
            GenContext.Current = contextProvider;
            _framework = framework;

            InitializeTemplates(Source, ProgrammingLanguages.CSharp);
        }
    }
}
