// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;

namespace Microsoft.Templates.Core.Gen.Shell
{
    public interface IGenShellVisualStudio
    {
        string GetVsVersion(); // return "0.0.0.0";

        string GetVsVersionAndInstance(); // return "0.0.0.0-i";

        bool IsDebuggerEnabled();

        bool IsBuildInProgress();

        bool IsSdkInstalled(string version);

        List<string> GetInstalledPackageIds();
    }
}
