// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.IO;
using System.Text;
using System;

using Microsoft.Templates.Core.Packaging;

namespace Microsoft.Templates.Core.Locations
{
    public class LocalTemplatesSource : TemplatesSource
    {
        public string LocalTemplatesVersion { get; private set; }

        public string LocalWizardVersion { get; private set; }

        protected override bool VerifyPackageSignatures => false;

        public override bool ForcedAcquisition { get => base.ForcedAcquisition; protected set => base.ForcedAcquisition = value; }
        public string Origin => $@"..\..\..\..\..\{SourceFolderName}";

        private object lockObject = new object();

        public LocalTemplatesSource() : this("0.0.0.0", "0.0.0.0")
        {
            ForcedAcquisition = true;
        }

        public LocalTemplatesSource(string wizardVersion, string templatesVersion, bool forcedAdquisition = true)
        {
            ForcedAcquisition = forcedAdquisition;
            LocalTemplatesVersion = templatesVersion;
            LocalWizardVersion = wizardVersion;
        }

        protected override string AcquireMstx()
        {
            return Origin;
        }

        public override void Extract(string source, string targetFolder)
        {
            if (source.ToLower().EndsWith("mstx"))
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

            string finalDestination = PrepareFinalDestination(finalTargetFolder, ver);

            if (!Directory.Exists(finalDestination))
            {
                Fs.CopyRecursive(sourcePath, finalDestination);
            }
        }
    }
}
