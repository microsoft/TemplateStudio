// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Microsoft.Templates.Core;
using Microsoft.Templates.UI.Resources;

namespace Microsoft.Templates.UI.Extensions
{
    public static class StringExtensions
    {
        public static string GetRequiredWorkloadDisplayName(this string requiredWorkload)
        {
            switch (requiredWorkload)
            {
                case "Microsoft.VisualStudio.Workload.ManagedDesktop":
                    return StringRes.WorkloadDisplayNameManagedDesktop;
                case "Microsoft.VisualStudio.Workload.Universal":
                    return StringRes.WorkloadDisplayNameUniversal;
                case "Microsoft.VisualStudio.Workload.NetWeb":
                    return StringRes.WorkloadDisplayNameNetWeb;
                case "Microsoft.VisualStudio.ComponentGroup.MSIX.Packaging":
                    return StringRes.WorkloadDisplayNameMsixPackaging;
                default:
                    return requiredWorkload;
            }
        }

        public static string GetPlatformDisplayName(this string platform)
        {
            switch (platform)
            {
                case Platforms.Uwp:
                    return StringRes.UWP;
                case Platforms.Wpf:
                    return StringRes.WPF;
                case Platforms.WinUI:
                    return StringRes.WinUI;
                default:
                    return platform;
            }
        }
    }
}
