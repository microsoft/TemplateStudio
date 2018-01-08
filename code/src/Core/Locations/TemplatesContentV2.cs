// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using Microsoft.Templates.Core.Diagnostics;
using Microsoft.Templates.Core.Resources;

namespace Microsoft.Templates.Core.Locations
{
    public class TemplatesContentV2
    {
        private const string TemplatesFolderName = "Templates";

        public string TemplatesFolder { get; private set; }

        public Version WizardVersion { get; private set; }

        public TemplatesSourceV2 Source { get; private set; }

        public string LatestContentFolder => GetContentFolder();

        public TemplatesContentInfo Current { get; private set; }

        public List<TemplatesContentInfo> All { get; private set; }

        public TemplatesContentInfo Latest { get => GetLatestContent(); }

        public TemplatesContentV2(string workingFolder, string sourceId, Version wizardVersion, TemplatesSourceV2 source, string tengineCurrentContent)
        {
            TemplatesFolder = Path.Combine(workingFolder, TemplatesFolderName, sourceId);

            LoadAvailableContents();

            // TODO: Ensure is the bestplace RAGC
            source.LoadConfig();
            Source = source;
            SetCurrentContent(tengineCurrentContent);

            WizardVersion = wizardVersion;
        }

        public bool Exists()
        {
            return Current != null;
        }

        public bool RequiresContentUpdate()
        {
            if (Current != null)
            {
                return Current.Version < Latest.Version;
            }
            else
            {
                return false;
            }
        }

        public bool IsNewVersionAvailable()
        {
            if (Current != null)
            {
                return !Current.Version.IsNull() && Current.Version < Source.Config.ResolvePackage(WizardVersion).Version;
            }
            else
            {
                return false;
            }
        }

        public bool IsWizardUpdateAvailable()
        {
            if (Current != null)
            {
                return !Current.Version.IsNull() && (Current.Version.Major < Source.Config.Latest.Version.Major || Current.Version.Minor < Source.Config.Latest.Version.Minor);
            }
            else
            {
                return false;
            }
        }

        public void GetNewVersionContent()
        {
            var latestPackage = Source.Config.ResolvePackage(WizardVersion);

            TemplatesContentInfo content = Source.GetContent(latestPackage, TemplatesFolder);
            var alreadyExists = All.Where(p => p.Version == latestPackage.Version).FirstOrDefault();
            if (alreadyExists != null)
            {
                All.Remove(alreadyExists);
            }

            All.Add(content);
        }

        internal void Extract(string mstxFilePath)
        {
            TemplatesPackageInfo installedPackage = null;
            if (Source is RemoteTemplatesSourceV2 && File.Exists(mstxFilePath))
            {
                installedPackage = new TemplatesPackageInfo()
                {
                    Name = Path.GetFileName(mstxFilePath),
                    LocalPath = mstxFilePath,
                    Version = TemplatesPackageInfo.GetVersionFromMstx(mstxFilePath)
                };
            }
            else
            {
                installedPackage = LocalTemplatesSourceV2.VersionZero;
            }

            var package = Source.GetContent(installedPackage, TemplatesFolder);
            Current = package;
            All.Add(package);
        }

        public void Purge()
        {
            if (Directory.Exists(TemplatesFolder))
            {
                var di = new DirectoryInfo(TemplatesFolder);

                foreach (var sdi in di.EnumerateDirectories().Where(d => d.FullName != Current.Path))
                {
                    Version.TryParse(sdi.Name, out Version v);
                    if (!v.IsNull() && v < Current.Version)
                    {
                        Fs.SafeDeleteDirectory(sdi.FullName, false);
                    }
                }
            }
        }

        private Version GetVersionFromFolder(string contentFolder)
        {
            string versionPart = Path.GetFileName(contentFolder);

            Version.TryParse(versionPart, out Version v);

            return v;
        }

        private bool ExistsContent(string folder)
        {
            bool result = false;

            if (Directory.Exists(folder))
            {
                var di = new DirectoryInfo(folder);

                result = di.EnumerateFiles("*", SearchOption.AllDirectories).Any();
            }

            return result;
        }

        private string GetContentFolder()
        {
            var latestVersion = new Version(0, 0, 0, 0);
            string latestContent = Path.Combine(TemplatesFolder, "0.0.0.0");

            if (Directory.Exists(TemplatesFolder))
            {
                var di = new DirectoryInfo(TemplatesFolder);

                foreach (DirectoryInfo sdi in di.EnumerateDirectories())
                {
                    Version.TryParse(sdi.Name, out Version v);

                    if (v >= latestVersion)
                    {
                        latestVersion = v;
                        latestContent = sdi.FullName;
                    }
                }
            }

            return latestContent;
        }

        private void LoadAvailableContents()
        {
            var latestVersion = new Version(0, 0, 0, 0);
            string latestContent = Path.Combine(TemplatesFolder, "0.0.0.0");
            All = new List<TemplatesContentInfo>();

            if (Directory.Exists(TemplatesFolder))
            {
                var di = new DirectoryInfo(TemplatesFolder);

                foreach (DirectoryInfo sdi in di.EnumerateDirectories())
                {
                    Version.TryParse(sdi.Name, out Version v);

                    if (!v.IsNull())
                    {
                        TemplatesContentInfo t = new TemplatesContentInfo()
                        {
                            Path = sdi.FullName,
                            Version = v,
                            Date = sdi.CreationTime
                        };

                        All.Add(t);
                    }
                }
            }
        }

        private TemplatesContentInfo GetLatestContent()
        {
            return All.OrderByDescending(c => c.Version).ThenByDescending(c => c.Date).FirstOrDefault();
        }

        private void SetCurrentContent(string tengineCurrentContent)
        {
            Current = All.Where(c => c.Path.Equals(tengineCurrentContent, StringComparison.OrdinalIgnoreCase)).FirstOrDefault();
        }

        internal void RefreshContentFolder(string tengineCurrentContent)
        {
            Current = All.Where(c => c.Path.Equals(tengineCurrentContent, StringComparison.OrdinalIgnoreCase)).FirstOrDefault();
        }
    }
}
