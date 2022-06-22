// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Microsoft.Templates.Core;
using Microsoft.Templates.SharedResources;

namespace Microsoft.Templates.UI.Extensions
{
    public static class StringExtensions
    {
        public static string GetRequiredWorkloadDisplayName(this string requiredWorkload)
        {
            switch (requiredWorkload)
            {
                case "Microsoft.VisualStudio.Workload.ManagedDesktop":
                    return Resources.WorkloadDisplayNameManagedDesktop;
                case "Microsoft.VisualStudio.Workload.Universal":
                    return Resources.WorkloadDisplayNameUniversal;
                case "Microsoft.VisualStudio.Workload.NetWeb":
                    return Resources.WorkloadDisplayNameNetWeb;
                case "Microsoft.VisualStudio.ComponentGroup.MSIX.Packaging":
                    return Resources.WorkloadDisplayNameMsixPackaging;
                default:
                    return requiredWorkload;
            }
        }

        public static string GetPlatformDisplayName(this string platform)
        {
            switch (platform)
            {
                case Platforms.Uwp:
                    return Resources.UWP;
                case Platforms.Wpf:
                    return Resources.WPF;
                case Platforms.WinUI:
                    return Resources.WinUI;
                default:
                    return platform;
            }
        }
    }
}
