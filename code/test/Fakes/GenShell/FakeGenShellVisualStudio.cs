// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;
using System.Linq;
using Microsoft.Templates.Core.Gen.Shell;

namespace Microsoft.Templates.Fakes.GenShell
{
    public class FakeGenShellVisualStudio : IGenShellVisualStudio
    {
        public string GetVsVersion() => "0.0.0.0";

        public string GetVsVersionAndInstance() => "0.0.0.0-i";

        public bool IsBuildInProgress() => false;

        public bool IsDebuggerEnabled() => false;

        public bool IsSdkInstalled(string version) => true;

        public List<string> GetInstalledPackageIds() => Enumerable.Empty<string>().ToList();
    }
}
