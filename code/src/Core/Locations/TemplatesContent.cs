using Microsoft.Templates.Core.Diagnostics;
using Microsoft.Templates.Core.Extensions;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Templates.Core.Locations
{
    public class TemplatesContent
    {
        private string TemplatesFolderName = "Templates";
        public string TemplatesFolder { get; private set; }
        public string LatestContentFolder { get => GetLatestContentFolder(true); }

        public TemplatesContent(string workingFolder, string sourceId)
        {
            TemplatesFolder = Path.Combine(workingFolder, TemplatesFolderName, sourceId);
        }

        public bool Exists()
        {
            return ExistsContent(LatestContentFolder);
        }

        public bool ExitsNewerVersion(string currentContentFolder)
        {
            if (ExistsContent(LatestContentFolder))
            {
                Version currentVersion = GetVersionFromFolder(currentContentFolder);
                Version latestVersion = GetVersionFromFolder(LatestContentFolder);
                return currentVersion < latestVersion;
            }
            else
            {
                return false;
            }
        }
        public bool ExistOverVersion()
        {
            string overVersionFolder = GetLatestContentFolder(false);

            if (ExistsContent(overVersionFolder))
            {
                Version overVersion = GetVersionFromFolder(overVersionFolder);
                return !IsWizardAligned(overVersion);
            }
            else
            {
                return false;
            }
        }

        public bool IsExpired(string currentContent)
        {
            if (!File.Exists(currentContent))
            {
                return true;
            }

            var directory = new DirectoryInfo(currentContent);
            var expiration = directory.LastWriteTimeUtc.AddMinutes(Configuration.Current.VersionCheckingExpirationMinutes);
            AppHealth.Current.Verbose.TrackAsync($"Current content expiration: {expiration.ToString()}").FireAndForget();
            return expiration <= DateTime.UtcNow;
        }

        public void Purge(string currentContent)
        {
            if (Directory.Exists(TemplatesFolder))
            {
                DirectoryInfo di = new DirectoryInfo(TemplatesFolder);
                foreach (var sdi in di.EnumerateDirectories())
                {
                    Version.TryParse(sdi.Name, out Version v);
                    if (v < GetVersionFromFolder(currentContent))
                    {
                        Fs.SafeDeleteDirectory(sdi.FullName);
                    }
                }
            }
        }

        public Version GetVersionFromFolder(string contentFolder)
        {
            string versionPart = Path.GetFileName(contentFolder);
            Version.TryParse(versionPart, out Version v);
            return v;
        }

        private bool ExistsContent(string folder)
        {
            if (!Directory.Exists(folder))
            {
                return false;
            }
            else
            {
                DirectoryInfo di = new DirectoryInfo(folder);
                return di.EnumerateFiles("*", SearchOption.AllDirectories).Any();
            }
        }

        private string GetLatestContentFolder(bool ensureWizardAligmnent)
        {
            Version latestVersion = new Version(0, 0, 0, 0);
            string latestContent = "";
            if (Directory.Exists(TemplatesFolder))
            {
                DirectoryInfo di = new DirectoryInfo(TemplatesFolder);
                foreach (DirectoryInfo sdi in di.EnumerateDirectories())
                {
                    Version.TryParse(sdi.Name, out Version v);

                    if (v >= latestVersion)
                    {
                        if (!ensureWizardAligmnent || (ensureWizardAligmnent && IsWizardAligned(v)))
                        {
                            latestVersion = v;
                            latestContent = sdi.FullName;
                        }
                    }
                }
            }
            return latestContent;
        }

        private static Version GetWizardVersion()
        {
            string assemblyLocation = Assembly.GetExecutingAssembly().Location;
            var versionInfo = FileVersionInfo.GetVersionInfo(assemblyLocation);
            Version.TryParse(versionInfo.FileVersion, out Version v);

            return v;
        }

        protected bool IsWizardAligned(Version v)
        {
            Version wizardVersion = GetWizardVersion();
            return v.Major == wizardVersion.Major && v.Minor == wizardVersion.Minor;
        }
    }
}
