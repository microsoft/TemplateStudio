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

        public bool ExitsNewerVersion(string currentUsedFolder)
        {
            if (ExistsContent(LatestContentFolder))
            {
                Version currentVersion = GetVersion(currentUsedFolder);
                Version latestVersion = GetVersion(LatestContentFolder);
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
            return ExistsContent(overVersionFolder);
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
        private Version GetVersion(string contentFolder)
        {
            string versionPart = Path.GetDirectoryName(contentFolder);
            Version.TryParse(versionPart, out Version v);
            return v;
        }

        private string GetLatestContentFolder(bool ensureWizardAligmnent)
        {
            Version latestVersion = new Version(0, 0, 0, 0);
            string latestContent = string.Empty;
            if (Directory.Exists(TemplatesFolder))
            {
                DirectoryInfo di = new DirectoryInfo(TemplatesFolder);
                foreach (DirectoryInfo sdi in di.EnumerateDirectories())
                {
                    Version.TryParse(sdi.Name, out Version v);

                    if (v > latestVersion)
                    {
                        if (!ensureWizardAligmnent || (ensureWizardAligmnent && IsWizardAligned(v)))
                        {
                            latestVersion = v;
                            latestContent = sdi.Name;
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
