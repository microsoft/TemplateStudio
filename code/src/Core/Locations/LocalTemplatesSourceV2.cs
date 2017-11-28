// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;

namespace Microsoft.Templates.Core.Locations
{
    public class LocalTemplatesSourceV2 : TemplatesSourceV2
    {
        private enum SymbolicLink
        {
            File = 0,
            Directory = 1
        }

        [DllImport("kernel32.dll")]
        private static extern bool CreateSymbolicLink(string lpSymlinkFileName, string lpTargetFileName, SymbolicLink dwFlags);

        public static readonly TemplatesPackageInfo VersionZero = new TemplatesPackageInfo()
        {
            Name = "LocalTemplates",
            LocalPath = $@"..\..\..\..\..\{SourceFolderName}",
            Bytes = 1024,
            Date = DateTime.Now,
            Uri = new Uri("file://"),
            Version = new Version(0, 0, 0, 0)
        };

        public string LocalWizardVersion { get; private set; }

        protected override bool VerifyPackageSignatures => false;

        public string Origin => $@"..\..\..\..\..\{SourceFolderName}";

        private string _id;

        public override string Id { get => _id; }

        protected string FinalDestination { get; set; }

        public LocalTemplatesSourceV2()
            : this("0.0.0.0", "0.0.0.0", true)
        {
            _id = Configuration.Current.Environment + GetAgentName();
        }

        public LocalTemplatesSourceV2(string id)
            : this("0.0.0.0", "0.0.0.0", true)
        {
            _id = id + GetAgentName();
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors", Justification = "Used to override the default value for this property in the local (test) source")]
        public LocalTemplatesSourceV2(string wizardVersion, string templatesVersion, bool forcedAdquisition = true)
        {
            ForcedAcquisition = forcedAdquisition;

            if (string.IsNullOrEmpty(_id))
            {
                _id = Configuration.Current.Environment + GetAgentName();
            }
        }

        public override void LoadConfig()
        {
            Config = new TemplatesSourceConfig()
            {
                PackagesInfo = new List<TemplatesPackageInfo>() { VersionZero },
                Latest = VersionZero,
                VersionCount = 1
            };
        }

        public override TemplatesContentInfo GetContent(TemplatesPackageInfo packageInfo, string workingFolder)
        {
            string targetFolder = Path.Combine(workingFolder, packageInfo.Version.ToString());
            JunctionPoint.Create(Origin, targetFolder, true);

            return new TemplatesContentInfo()
            {
                Version = packageInfo.Version,
                Path = targetFolder,
                Date = packageInfo.Date
            };
        }

        public override void Adquire(ref TemplatesPackageInfo packageInfo)
        {
            packageInfo.LocalPath = Origin;
        }

        private static string GetAgentName()
        {
            // If running tests in VSTS concurrently in different agents avoids the collison in templates folders
            string agentName = Environment.GetEnvironmentVariable("AGENT_NAME");
            if (string.IsNullOrEmpty(agentName))
            {
                return string.Empty;
            }
            else
            {
                return $"-{agentName}";
            }
        }
    }
}
