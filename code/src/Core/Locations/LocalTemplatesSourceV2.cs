// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.IO;

namespace Microsoft.Templates.Core.Locations
{
    public class LocalTemplatesSourceV2 : TemplatesSourceV2
    {
        private readonly TemplatesPackageInfo versionZero = new TemplatesPackageInfo()
        {
            Name = "LocalTemplates",
            LocalPath = $@"..\..\..\..\..\{SourceFolderName}",
            Bytes = 1024,
            Date = DateTime.Now,
            Uri = new Uri("file://"),
            Version = new Version(0, 0, 0, 0)
        };

        public string LocalTemplatesVersion { get; private set; }

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
            LocalTemplatesVersion = templatesVersion;
            LocalWizardVersion = wizardVersion;
            if (string.IsNullOrEmpty(_id))
            {
                _id = Configuration.Current.Environment + GetAgentName();
            }
        }

        public override void LoadConfig()
        {
            Config = new TemplatesSourceConfig()
            {
                PackagesInfo = new List<TemplatesPackageInfo>() { versionZero },
                Latest = versionZero,
                VersionCount = 1
            };
        }

        public override TemplatesPackageInfo Get(TemplatesPackageInfo packageInfo)
        {
            return new TemplatesPackageInfo()
            {
                Version = packageInfo.Version,
                LocalPath = Origin,
                Date = packageInfo.Date
            };
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
