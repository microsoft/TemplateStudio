// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.IO;
using System.Linq;

using Microsoft.Templates.Core.Diagnostics;
using Microsoft.Templates.Core.Resources;

namespace Microsoft.Templates.Core.Locations
{
    public class TemplatesContentV2
    {

        //BASE CONTENT IN TEMPLATECONTENTINFO

        private const string TemplatesFolderName = "Templates";

        public string TemplatesFolder { get; private set; }

        public Version WizardVersion { get; private set; }

        public TemplatesSourceV2 Source { get; private set; }

        public string LatestContentFolder => GetContentFolder();

        public TemplatesContentV2(string workingFolder, string sourceId, Version wizardVersion, TemplatesSourceV2 source)
        {
            TemplatesFolder = Path.Combine(workingFolder, TemplatesFolderName, sourceId);

            WizardVersion = wizardVersion;

            // TODO: Ensure is the bestplace RAGC
            source.LoadConfig();
        }

        public bool Exists()
        {
            return ExistsContent(LatestContentFolder);
        }

        public bool RequiresContentUpdate(string currentContentFolder)
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

        public bool IsNewVersionAvailable(string currentContentFolder)
        {
            if (ExistsContent(LatestContentFolder))
            {
                Version currentVersion = GetVersionFromFolder(currentContentFolder);
                Version latestVersion = Source.Config.ResolvePackage(WizardVersion).Version;

                return !currentVersion.IsNull() && currentVersion < latestVersion;
            }
            else
            {
                return false;
            }
        }

        public void GetNewVersionContent(string currentContentFolder)
        {
            if (IsNewVersionAvailable(currentContentFolder))
            {
                var latestPackage = Source.Config.ResolvePackage(WizardVersion);

                // TODO: Esto lo tiene que gestionar TemplatesContent (que debe tener una lista de TemplatesContentInfo -> la cagamos o la creamos de cero)
                // Source.Get(latestPackage, Path.Combine(TemplatesFolder, latestPackage.Version.ToString()));


            }
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
    }
}
