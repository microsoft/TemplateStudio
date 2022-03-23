// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;

namespace Microsoft.Templates.Core.Gen
{
    public class ProjectInfo
    {
        public List<string> Projects { get; } = new List<string>();

        public List<string> ProjectItems { get;  } = new List<string>();

        public List<SdkReference> SdkReferences { get;  } = new List<SdkReference>();

        public List<NugetReference> NugetReferences { get; } = new List<NugetReference>();

        public List<ProjectReference> ProjectReferences { get; } = new List<ProjectReference>();

        public List<ProjectConfiguration> ProjectConfigurations { get; } = new List<ProjectConfiguration>();
    }
}
