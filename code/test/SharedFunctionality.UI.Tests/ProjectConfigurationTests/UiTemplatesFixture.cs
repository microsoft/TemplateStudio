// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using Microsoft.Templates.Core;
using Microsoft.Templates.Core.Gen;
using Microsoft.Templates.Fakes;
using Microsoft.Templates.Fakes.GenShell;

namespace Microsoft.UI.Test.ProjectConfigurationTests
{
    public class UITemplatesFixture
    {
        public TemplatesRepository Repository { get; private set; }

        [SuppressMessage(
             "Usage",
             "VSTHRD002:Synchronously waiting on tasks or awaiters may cause deadlocks",
             Justification = "Required for unit testing.")]
        public void InitializeFixture(string platform, string language)
        {
            var path = $"{Path.GetPathRoot(Environment.CurrentDirectory)}\\UIT\\UI\\";

            var source = new UITestsTemplatesSource();
            GenContext.Bootstrap(source, new FakeGenShell(platform, language), platform, language, "0.0.0.0");

            Repository = GenContext.ToolBox.Repo;

            GenContext.Current = new FakeContextProvider
            {
                ProjectName = "Test",
                DestinationPath = path,
                GenerationOutputPath = path,
            };
        }
    }
}
