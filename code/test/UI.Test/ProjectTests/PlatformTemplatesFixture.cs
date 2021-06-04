// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using Microsoft.Templates.Core;
using Microsoft.Templates.Core.Gen;
using Microsoft.Templates.Core.Locations;
using Microsoft.Templates.Fakes;

namespace Microsoft.UI.Test.ProjectTests
{
    public class PlatformTemplatesFixture
    {
        public TemplatesRepository Repository { get; private set; }

        [SuppressMessage(
             "Usage",
             "VSTHRD002:Synchronously waiting on tasks or awaiters may cause deadlocks",
             Justification = "Required for unit testing.")]
        public void InitializeFixture(string platform, string language)
        {
            var path = $"{Path.GetPathRoot(Environment.CurrentDirectory)}\\UIT\\UI\\";

            var source = new LocalTemplatesSource(null, $"UITest{platform}");

            GenContext.Bootstrap(source, new FakeGenShell(platform, language), platform, language);

            GenContext.ToolBox.Repo.SynchronizeAsync(true).Wait();

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
