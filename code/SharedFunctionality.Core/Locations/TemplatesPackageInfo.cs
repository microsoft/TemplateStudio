// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text.RegularExpressions;

using Newtonsoft.Json;

namespace Microsoft.Templates.Core.Locations
{
    public class TemplatesPackageInfo
    {
        private string _version;

        public string Name { get; set; }

        [JsonIgnore]
        public string LocalPath { get; set; }

        public string Language { get; set; }

        public string Platform { get; set; }

        public List<string> WizardVersions { get; set; }

        public DateTime Date { get; set; }

        public long Bytes { get; set; }

        [JsonIgnore]
        public string Version { get => GetVersion(); }

        private string GetVersion()
        {
            if (_version == null)
            {
                _version = ParseVersion(Name).ToString();
                if (_version == null)
                {
                    _version = GetVersionFromMstx(LocalPath).ToString();
                }
            }

            return _version;
        }

        [JsonIgnore]
        public string MainVersion { get => Version ?? "NoVersion"; }

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

        private static Version GetVersionFromMstx(string mstxFilePath)
        {
            string content = GetFileContent(mstxFilePath, "Version.txt");

            if (!System.Version.TryParse(content, out Version result))
            {
                result = new Version(0, 0, 0, 0);
            }

            return result;
        }

        private static string GetFileContent(string mstxFilePath, string fileName)
        {
            string result = string.Empty;
            if (File.Exists(mstxFilePath))
            {
                using (ZipArchive zip = ZipFile.Open(mstxFilePath, ZipArchiveMode.Read))
                {
                    var entry = zip.Entries.Where(e => e.Name.Equals(fileName, StringComparison.OrdinalIgnoreCase)).FirstOrDefault();
                    if (entry != null)
                    {
                        using (StreamReader sr = new StreamReader(entry.Open()))
                        {
                            result = sr.ReadToEnd();
                        }
                    }
                }
            }

            return result;
        }
    }
}
