// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;
using Microsoft.Templates.Core.Gen;
using Microsoft.Templates.Core.PostActions.Catalog.Merge;
using Microsoft.Templates.Core.Diagnostics;

namespace Microsoft.Templates.Test
{
    public abstract class BaseTestContextProvider : IContextProvider
    {
        public string ProjectName { get; set; }

        public string OutputPath { get; set; }

        public string DestinationPath { get; set; }

        public string DestinationParentPath { get; set; }

        public string TempGenerationPath { get; set; }

        public List<string> ProjectItems { get; } = new List<string>();

        public List<FailedMergePostActionInfo> FailedMergePostActions { get; } = new List<FailedMergePostActionInfo>();

        public Dictionary<string, List<MergeInfo>> MergeFilesFromProject { get; } = new Dictionary<string, List<MergeInfo>>();

        public List<string> FilesToOpen { get; } = new List<string>();

        public Dictionary<ProjectMetricsEnum, double> ProjectMetrics { get; } = new Dictionary<ProjectMetricsEnum, double>();
    }
}
