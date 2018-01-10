// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Microsoft.Templates.Core;
using Microsoft.Templates.Core.Diagnostics;
using Microsoft.Templates.Core.Gen;
using Microsoft.Templates.Core.Locations;
using Microsoft.Templates.Core.PostActions.Catalog.Merge;
using Microsoft.Templates.Fakes;

namespace Microsoft.UI.Test
{
    public class TemplatesFixture : IContextProvider
    {
        private static bool syncExecuted = false;

        public string ProjectName => "Test";

        public string OutputPath => string.Empty;

        public string ProjectPath => string.Empty;

        public List<string> ProjectItems => null;

        public List<string> FilesToOpen => null;

        public List<FailedMergePostAction> FailedMergePostActions => null;

        public Dictionary<string, List<MergeInfo>> MergeFilesFromProject => null;

        public Dictionary<ProjectMetricsEnum, double> ProjectMetrics => null;

        public TemplatesRepository Repository { get; private set; }

        [SuppressMessage(
        "Usage",
        "VSTHRD002:Synchronously waiting on tasks or awaiters may cause deadlocks",
        Justification = "Required por unit testing.")]
        public void InitializeFixture(string language)
        {
            var source = new LocalTemplatesSource();
            GenContext.Bootstrap(source, new FakeGenShell(language), language);
            GenContext.Current = this;
            if (!syncExecuted)
            {
                GenContext.ToolBox.Repo.SynchronizeAsync(true).Wait();
                syncExecuted = true;
            }

            Repository = GenContext.ToolBox.Repo;
        }
    }
}
