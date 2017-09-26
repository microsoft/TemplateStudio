// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.AccessControl;

using Microsoft.Win32;
using Microsoft.Templates.Core.Diagnostics;
using Microsoft.Templates.Core;

namespace Microsoft.Templates.UI
{
    internal enum NetReleases : int
    {
        NotFound = 0,
        LowerThan45 = 1,
        Ver45 = 378389,
        Ver451 = 378675,
        Ver452 = 379893,
        Ver46 = 393295,
        Ver461 = 394254,
        Ver462 = 394802,
        Ver47 = 460798
    }
    internal static class DotNetVersion
    {
        // Based on https://docs.microsoft.com/en-us/dotnet/framework/migration-guide/how-to-determine-which-versions-are-installed

        public const NetReleases MinimumAllowedVersion = NetReleases.Ver47;
        public const string MinimumAllowedVersionLabel = "4.7 or later";

        public static bool IsAllowed()
        {
            const string subkey = @"SOFTWARE\Microsoft\NET Framework Setup\NDP\v4\Full\";

            try
            {
                using (RegistryKey ndpKey = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry32).OpenSubKey(subkey, RegistryRights.ReadKey))
                {
                    if (ndpKey != null && ndpKey.GetValue("Release") != null)
                    {
                        NetReleases installedVersion = GetDotNetReleaseVersion((int)ndpKey.GetValue("Release"));
                        return installedVersion >= MinimumAllowedVersion;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
            catch (Exception ex)
            {
                AppHealth.Current.Warning.TrackAsync("Unable to retrieve .NET version information from registry", ex).FireAndForget();
                return false;
            }
        }

        private static NetReleases GetDotNetReleaseVersion(int releaseKey)
        {
            if (releaseKey >= 460798)
                return NetReleases.Ver47;
            if (releaseKey >= 394802)
                return NetReleases.Ver462;
            if (releaseKey >= 394254)
            {
                return NetReleases.Ver461;
            }
            if (releaseKey >= 393295)
            {
                return NetReleases.Ver46;
            }
            if ((releaseKey >= 379893))
            {
                return NetReleases.Ver452;
            }
            if ((releaseKey >= 378675))
            {
                return NetReleases.Ver451;
            }
            if ((releaseKey >= 378389))
            {
                return NetReleases.Ver45;
            }
            // This code should never execute. A non-null release key should mean
            // that 4.5 or later is installed.
            return NetReleases.LowerThan45;
        }
    }
}
