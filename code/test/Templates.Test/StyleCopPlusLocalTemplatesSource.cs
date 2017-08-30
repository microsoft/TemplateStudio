// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.IO;

using Microsoft.Templates.Core;
using Microsoft.Templates.Core.Locations;
using Microsoft.Templates.Core.Packaging;

namespace Microsoft.Templates.Test
{
    public class StyleCopPlusLocalTemplatesSource : TemplatesSource
    {
        public string LocalTemplatesVersion { get; private set; }

        public string LocalWizardVersion { get; private set; }

        protected override bool VerifyPackageSignatures => false;

        public string Origin => $@"..\..\..\..\..\{SourceFolderName}";

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public StyleCopPlusLocalTemplatesSource() : this("0.0.0.0", "0.0.0.0")
        {
            ForcedAcquisition = true;
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public StyleCopPlusLocalTemplatesSource(string wizardVersion, string templatesVersion, bool forcedAdquisition = true)
        {
            ForcedAcquisition = forcedAdquisition;
            LocalTemplatesVersion = templatesVersion;
            LocalWizardVersion = wizardVersion;
        }

        protected override string AcquireMstx()
        {
            // Compress Content adding version return TemplatePackage path.
            var tempFolder = Path.Combine(GetTempFolder(), SourceFolderName);

            Copy(Origin, tempFolder);

            Fs.CopyRecursive(@".\TestData\StyleCop", Path.Combine(tempFolder, "Features", "StyleCop"));

            File.WriteAllText(Path.Combine(tempFolder, "version.txt"), LocalTemplatesVersion);

            return TemplatePackage.Pack(tempFolder);
        }

        protected static void Copy(string sourceFolder, string targetFolder)
        {
            Fs.SafeDeleteDirectory(targetFolder);
            Fs.CopyRecursive(sourceFolder, targetFolder);
        }
    }
}
