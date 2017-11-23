// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;
using System.Linq;

namespace WtsTool
{
    public class RemoteSourceVersionsInfo
    {
        public int PackageCount { get; internal set; }

        public RemotePackageInfo LatestVersionInfo { get; internal set; }

        public IEnumerable<RemotePackageInfo> AvailableVersions { get; internal set; }

        public IOrderedEnumerable<RemotePackageInfo> Versions { get; internal set; }
    }
}
