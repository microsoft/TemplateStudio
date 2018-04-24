// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;
using Microsoft.Templates.Core.Diagnostics;
using Microsoft.Templates.Core.Gen;
using Microsoft.Templates.Core.PostActions.Catalog.Merge;

namespace Microsoft.Templates.VsEmulator
{
    public class FakeContextProvider : IContextProvider
    {
        public string ProjectName { get; set; }

        public string OutputPath { get; set; }

        public string ProjectPath { get; set; }

        public List<string> ProjectItems { get; set; }

        public List<string> FilesToOpen { get; set; }

        public List<FailedMergePostAction> FailedMergePostActions { get; set; }

        public Dictionary<string, List<MergeInfo>> MergeFilesFromProject { get; set; }

        public Dictionary<ProjectMetricsEnum, double> ProjectMetrics { get; set; }

        public string DestinationPath
        {
            get
            {
                return this.ProjectPath;
            }
        }

        public string DestinationParentPath
        {
            get
            {
                return System.IO.Path.GetDirectoryName(this.ProjectPath);
            }
        }

        public string TempGenerationPath => throw new System.NotImplementedException();

        List<FailedMergePostActionInfo> IContextProvider.FailedMergePostActions => throw new System.NotImplementedException();
    }
}
