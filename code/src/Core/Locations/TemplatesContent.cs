// ******************************************************************
// Copyright (c) Microsoft. All rights reserved.
// This code is licensed under the MIT License (MIT).
// THE CODE IS PROVIDED “AS IS”, WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED,
// INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.
// IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
// DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT,
// TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH
// THE CODE OR THE USE OR OTHER DEALINGS IN THE CODE.
// ******************************************************************

using System;
using System.IO;
using System.Linq;

using Microsoft.Templates.Core.Diagnostics;

namespace Microsoft.Templates.Core.Locations
{
    public class TemplatesContent
    {
        private const string TemplatesFolderName = "Templates";
        private string _defaultContentFolder;

        public string TemplatesFolder { get; private set; }
        public Version WizardVersion { get; private set; }

        public string LatestContentFolder => GetLatestContentFolder(true);

        public TemplatesContent(string workingFolder, string sourceId, Version wizardVersion)
        {
            TemplatesFolder = Path.Combine(workingFolder, TemplatesFolderName, sourceId);
            _defaultContentFolder = Path.Combine(TemplatesFolder, "0.0.0.0");
            WizardVersion = wizardVersion;
        }

        public bool Exists()
        {
            return ExistsContent(LatestContentFolder);
        }

        public bool RequiresUpdate(string currentContentFolder)
        {
            if (ExistsContent(LatestContentFolder))
            {
                Version currentVersion = GetVersionFromFolder(currentContentFolder);
                Version latestVersion = GetVersionFromFolder(LatestContentFolder);

                return currentVersion == null || currentVersion < latestVersion || latestVersion.IsZero();
            }
            else
            {
                return false;
            }
        }
        public bool ExistOverVersion()
        {
            string targetFolder = GetLatestContentFolder(false);
            Version targetVersion = GetVersionFromFolder(targetFolder);

            if (ExistsContent(targetFolder) && !targetVersion.IsZero())
            {
                return IsVersionOverWizard(targetVersion);
            }
            else
            {
                return false;
            }
        }

        public bool ExistUnderVersion()
        {
            string targetFolder = GetLatestContentFolder(false);
            Version targetVersion = GetVersionFromFolder(targetFolder);

            if (ExistsContent(targetFolder) && !targetVersion.IsZero())
            {
                return IsVersionUnderWizard(targetVersion);
            }
            else
            {
                return false;
            }
        }

        public bool IsExpired(string currentContent)
        {
            if (!Directory.Exists(currentContent))
            {
                return true;
            }

            var directory = new DirectoryInfo(currentContent);
            var expiration = directory.LastWriteTime.AddMinutes(Configuration.Current.VersionCheckingExpirationMinutes);
            AppHealth.Current.Verbose.TrackAsync($"Current content expiration: {expiration.ToString()}").FireAndForget();

            return expiration <= DateTime.Now;
        }

        public void Purge(string currentContent)
        {
            if (Directory.Exists(TemplatesFolder))
            {
                var di = new DirectoryInfo(TemplatesFolder);

                foreach (var sdi in di.EnumerateDirectories())
                {
                    Version.TryParse(sdi.Name, out Version v);

                    if (!v.IsZero() && v < GetVersionFromFolder(currentContent))
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
            bool result = false;

            if (Directory.Exists(folder))
            {
                var di = new DirectoryInfo(folder);

                result = di.EnumerateFiles("*", SearchOption.AllDirectories).Any();
            }

            if (!result)
            {
                result = CheckDefaultVersionContent();
            }

            return result;
        }

        private string GetLatestContentFolder(bool ensureWizardAligmnent)
        {
            var latestVersion = new Version(0, 0, 0, 0);
            string latestContent = _defaultContentFolder;

            if (Directory.Exists(TemplatesFolder))
            {
                var di = new DirectoryInfo(TemplatesFolder);

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

        private bool CheckDefaultVersionContent()
        {
            return Directory.Exists(_defaultContentFolder);
        }

        private bool IsVersionOverWizard(Version v)
        {
            if (IsWizardAligned(v))
            {
                return false;
            }
            else
            {
                return (v.Major > WizardVersion.Major || (v.Major == WizardVersion.Major && (v.Minor > WizardVersion.Minor)));
            }
        }

        private bool IsVersionUnderWizard(Version v)
        {
            if (IsWizardAligned(v))
            {
                return false;
            }
            else
            {
                return (v.Major < WizardVersion.Major || (v.Major == WizardVersion.Major && (v.Minor < WizardVersion.Minor)));
            }
        }

        private bool IsWizardAligned(Version v)
        {
            return WizardVersion.IsZero() || (v.Major == WizardVersion.Major && v.Minor == WizardVersion.Minor);
        }
    }
}
