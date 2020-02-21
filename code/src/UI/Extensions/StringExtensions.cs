using Microsoft.Templates.Core;
using Microsoft.Templates.UI.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
                default:
                    return platform;
            }
        }
    }
}
