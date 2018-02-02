// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Microsoft.Templates.Core.Locations
{
    public class TemplatesContent
    {
        private const string TemplatesFolderName = "Templates";

        public event Action<object, ProgressEventArgs> NewVersionAcquisitionProgress;

        public event Action<object, ProgressEventArgs> GetContentProgress;

        public event Action<object, ProgressEventArgs> CopyProgress;

        public string TemplatesFolder { get; private set; }

        public Version WizardVersion { get; private set; }

        public TemplatesSource Source { get; private set; }

        public string LatestContentFolder => GetContentFolder();

        public TemplatesContentInfo Current { get; private set; }

        public List<TemplatesContentInfo> All { get; private set; }

        public TemplatesContentInfo Latest { get => GetLatestContent(); }

        public TemplatesContent(string workingFolder, string sourceId, Version wizardVersion, TemplatesSource source, string tengineCurrentContent)
        {
            TemplatesFolder = Path.Combine(workingFolder, TemplatesFolderName, sourceId);

            LoadAvailableContents();

            Source = source;
            SetCurrentContent(tengineCurrentContent);

            WizardVersion = wizardVersion;
        }

        public bool Exists()
        {
            if (Current != null)
            {
                return Directory.Exists(Current.Path);
            }
            else
            {
                return false;
            }
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

        public bool IsNewVersionAvailable(out Version version)
        {
            version = Source.Config?.ResolvePackage(WizardVersion)?.Version;
            if (Current != null)
            {
                return !Current.Version.IsNull() && Current.Version < version;
            }
            else
            {
                return WizardVersion < version;
            }
        }

        public bool IsWizardUpdateAvailable(out Version version)
        {
            version = null;

            if (Current != null)
            {
                var result = WizardVersion.Major < Source.Config.Latest.Version.Major || WizardVersion.Minor < Source.Config.Latest.Version.Minor;
                if (result == true)
                {
                    version = new Version(Source.Config.Latest.Version.Major, Source.Config.Latest.Version.Minor);
                }

                return result;
            }
            else
            {
                return false;
            }
        }

        public async Task GetNewVersionContentAsync()
        {
            try
            {
                var latestPackage = Source.Config.ResolvePackage(WizardVersion);

                Source.NewVersionAcquisitionProgress += OnNewVersionAcquisitionProgress;
                Source.GetContentProgress += OnGetContentProgress;
                Source.CopyProgress += OnCopyProgress;

                await Source.AcquireAsync(latestPackage);

                if (latestPackage.LocalPath != null)
                {
                    TemplatesContentInfo content = await Source.GetContentAsync(latestPackage, TemplatesFolder);

                    var alreadyExists = All.Where(p => p.Version == latestPackage.Version).FirstOrDefault();
                    if (alreadyExists != null)
                    {
                        All.Remove(alreadyExists);
                    }

                    Current = content;
                    All.Add(content);
                }
            }
            finally
            {
                Source.NewVersionAcquisitionProgress -= OnNewVersionAcquisitionProgress;
                Source.GetContentProgress -= OnGetContentProgress;
                Source.CopyProgress -= OnCopyProgress;
            }
        }

        private void OnNewVersionAcquisitionProgress(object sender, ProgressEventArgs eventArgs)
        {
            NewVersionAcquisitionProgress?.Invoke(this, eventArgs);
        }

        private void OnGetContentProgress(object sender, ProgressEventArgs eventArgs)
        {
            GetContentProgress?.Invoke(this, eventArgs);
        }

        private void OnCopyProgress(object sender, ProgressEventArgs eventArgs)
        {
            CopyProgress?.Invoke(this, eventArgs);
        }

        internal TemplatesPackageInfo ResolveInstalledContent()
        {
            var mstxFilePath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "InstalledTemplates", "Templates.mstx");

            TemplatesPackageInfo installedPackage = null;
            if (Source is RemoteTemplatesSource && File.Exists(mstxFilePath))
            {
                var tempPath = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());
                Fs.SafeCopyFile(mstxFilePath, tempPath, true);
                installedPackage = new TemplatesPackageInfo()
                {
                    Name = Path.GetFileName(mstxFilePath),
                    LocalPath = Path.Combine(tempPath, Path.GetFileName(mstxFilePath))
                };
            }
            else
            {
                installedPackage = LocalTemplatesSource.VersionZero;
            }

            return installedPackage;
        }

        internal async Task GetInstalledContentAsync(TemplatesPackageInfo packageInfo)
        {
            try
            {
                Source.GetContentProgress += OnGetContentProgress;
                Source.CopyProgress += OnCopyProgress;

                var package = await Source.GetContentAsync(packageInfo, TemplatesFolder);
                Current = package;
                All.Add(package);
            }
            finally
            {
                Source.GetContentProgress -= OnGetContentProgress;
                Source.CopyProgress -= OnCopyProgress;
            }
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
