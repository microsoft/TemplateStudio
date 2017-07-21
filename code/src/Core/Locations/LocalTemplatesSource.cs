// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.IO;
using System.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace Microsoft.Templates.Core.Locations
{
    public sealed class LocalTemplatesSource : TemplatesSource
    {
        public string LocalTemplatesVersion { get; private set; }

        public string LocalWizardVersion { get; private set; }

        protected override bool VerifyPackageSignatures => false;

        public override bool ForcedAcquisition { get => base.ForcedAcquisition; protected set => base.ForcedAcquisition = value; }
        public string Origin => $@"..\..\..\..\..\{SourceFolderName}";

        private object lockObject = new object();

        public LocalTemplatesSource() : this("0.0.0.0", "0.0.0.0")
        {
            base.ForcedAcquisition = true;
        }

        public LocalTemplatesSource(string wizardVersion, string templatesVersion, bool forcedAdquisition = true)
        {
            base.ForcedAcquisition = forcedAdquisition;
            LocalTemplatesVersion = templatesVersion;
            LocalWizardVersion = wizardVersion;
        }

        protected override string AcquireMstx()
        {
            // Compress Content adding version return templatex path.
            var tempFolder = Path.Combine(GetTempFolder(), SourceFolderName);

            Copy(Origin, tempFolder);

            File.WriteAllText(Path.Combine(tempFolder, "version.txt"), LocalTemplatesVersion, Encoding.UTF8);

            return Templatex.Pack(tempFolder);
        }

        private void Copy(string sourceFolder, string targetFolder)
        {
            Fs.SafeDeleteDirectory(targetFolder);
            Fs.CopyRecursive(sourceFolder, targetFolder);
        }
    }
}
