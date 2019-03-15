// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;

using Microsoft.Templates.Core.Diagnostics;
using Microsoft.Templates.Core.PostActions.Catalog.Merge;

namespace Microsoft.Templates.Core.Gen
{
    public interface IContextProvider
    {
        string ProjectName { get; }

        string SafeProjectName { get; }

        string GenerationOutputPath { get; }

        string DestinationPath { get; }

        ProjectInfo ProjectInfo { get; }

        List<string> FilesToOpen { get; }

        List<FailedMergePostActionInfo> FailedMergePostActions { get; }

        Dictionary<string, List<MergeInfo>> MergeFilesFromProject { get; }

        Dictionary<ProjectMetricsEnum, double> ProjectMetrics { get; }
    }
}
