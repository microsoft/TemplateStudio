// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.IO;

namespace Microsoft.Templates.Core.Locations
{
    public class LocalTemplatesSource : TemplatesSource
    {
        public string LocalTemplatesVersion { get; private set; }

        public string LocalWizardVersion { get; private set; }

        protected override bool VerifyPackageSignatures => false;

        public string Origin => $@"..\..\..\..\..\{SourceFolderName}";

        private string _id;

        public override string Id { get => _id; }

        protected string FinalDestination { get; set; }

        public LocalTemplatesSource()
            : this("0.0.0.0", "0.0.0.0", true)
        {
            _id = Configuration.Current.Environment + GetAgentName();
        }

        public LocalTemplatesSource(string id)
            : this("0.0.0.0", "0.0.0.0", true)
        {
            _id = id + GetAgentName();
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors", Justification = "Used to override the default value for this property in the local (test) source")]
        public LocalTemplatesSource(string wizardVersion, string templatesVersion, bool forcedAdquisition = true)
        {
            ForcedAcquisition = forcedAdquisition;
            LocalTemplatesVersion = templatesVersion;
            LocalWizardVersion = wizardVersion;
            if (string.IsNullOrEmpty(_id))
            {
                _id = Configuration.Current.Environment + GetAgentName();
            }
        }

        protected override string AcquireMstx()
        {
            return Origin;
        }

        public override void Extract(string source, string targetFolder)
        {
            if (source.EndsWith("mstx", StringComparison.OrdinalIgnoreCase))
            {
                base.Extract(source, targetFolder);
            }
            else
            {
                SetLocalContent(source, targetFolder, new Version(LocalTemplatesVersion));
            }
        }

        private void SetLocalContent(string sourcePath, string finalTargetFolder, Version version)
        {
            Version ver = version;

            FinalDestination = PrepareFinalDestination(finalTargetFolder, ver);

            if (!Directory.Exists(FinalDestination))
            {
                Fs.CopyRecursive(sourcePath, FinalDestination, true);
            }
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
