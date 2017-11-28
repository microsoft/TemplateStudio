// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.Templates.Core.Diagnostics;
using Microsoft.Templates.Core.Packaging;
using Microsoft.Templates.Core.Resources;
using Newtonsoft.Json;

namespace Microsoft.Templates.Core.Locations
{
    public class TemplatesPackageInfo
    {
        public string Name { get; set; }

        public Uri Uri { get; set; }

        [JsonIgnore]
        public string LocalPath { get; set; }

        public DateTime Date { get; set; }

        public long Bytes { get; set; }

        public Version Version { get; set; }

        [JsonIgnore]
        public string MainVersion { get => !Version.IsNull() ? $"{Version.Major.ToString()}.{Version.Minor.ToString()}" : "NoVersion"; }

        public static Version ParseVersion(string packageName)
        {
            string versionPattern = @"\d+.\d+.\d+.\d+";
            Regex versionRegEx = new Regex(versionPattern, RegexOptions.Compiled & RegexOptions.IgnoreCase & RegexOptions.CultureInvariant);
            var match = versionRegEx.Match(packageName);
            if (match.Success)
            {
                return new Version(match.Value);
            }
            else
            {
                return null;
            }
        }

        public static void Extract(TemplatesPackageInfo packageInfo, string workingFolder, bool verifyPackageSignatures = true)
        {
            if (!string.IsNullOrEmpty(packageInfo.LocalPath))
            {
                Extract(packageInfo.LocalPath, Path.Combine(workingFolder, packageInfo.Version.ToString()), verifyPackageSignatures);
            }
            else
            {
                // TODO: Package Not Adquired
            }
        }

        private static void Extract(string mstxFilePath, string versionedFolder, bool verifyPackageSignatures = true)
        {
            try
            {
                TemplatePackage.Extract(mstxFilePath, versionedFolder, verifyPackageSignatures);
                AppHealth.Current.Verbose.TrackAsync($"{StringRes.TemplatesContentExtractedToString} {versionedFolder}.").FireAndForget();
            }
            catch (Exception ex)
            {
                AppHealth.Current.Exception.TrackAsync(ex, StringRes.TemplatesSourceExtractContentMessage).FireAndForget();
                throw;
            }
        }
    }
}
