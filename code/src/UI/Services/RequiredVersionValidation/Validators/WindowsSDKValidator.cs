// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using Microsoft.Templates.Core.Gen;

namespace Microsoft.Templates.UI.Services
{
    public class WindowsSDKValidator : IRequirementValidator
    {
        public const string Id = "UAP";
        public const string DisplayName = "[Windows SDK](https://developer.microsoft.com/windows/downloads/windows-10-sdk)";

        public bool IsVersionInstalled(Version version)
        {
            return GenContext.ToolBox.Shell.IsSdkInstalled(version.ToString());
        }
    }
}
