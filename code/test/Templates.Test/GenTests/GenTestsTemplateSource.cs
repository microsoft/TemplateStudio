using Microsoft.Templates.Core;
using Microsoft.Templates.Core.Locations;
using Microsoft.Templates.Core.Packaging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Templates.Test
{
    public sealed class GenTestTemplatesSource : TemplatesSource
    {
        public string LocalTemplatesVersion { get; private set; }

        public string LocalWizardVersion { get; private set; }

        protected override bool VerifyPackageSignatures => false;

        public override bool ForcedAcquisition { get => base.ForcedAcquisition; protected set => base.ForcedAcquisition = value; }
        public string Origin => $@"..\..\..\..\..\{SourceFolderName}";

        private object lockObject = new object();

        public override string Id => "TestGen";

        public GenTestTemplatesSource() : this("0.0.0.0", "0.0.0.0")
        {
            base.ForcedAcquisition = true;
        }

        public GenTestTemplatesSource(string wizardVersion, string templatesVersion, bool forcedAdquisition = true)
        {
            base.ForcedAcquisition = forcedAdquisition;
            LocalTemplatesVersion = templatesVersion;
            LocalWizardVersion = wizardVersion;
        }

        protected override string AcquireMstx()
        {
            // Compress Content adding version return TemplatePackage path.
            var tempFolder = Path.Combine(GetTempFolder(), SourceFolderName);

            Copy(Origin, tempFolder);

            File.WriteAllText(Path.Combine(tempFolder, "version.txt"), LocalTemplatesVersion, Encoding.UTF8);

            return TemplatePackage.Pack(tempFolder);
        }

        private void Copy(string sourceFolder, string targetFolder)
        {
            Fs.SafeDeleteDirectory(targetFolder);
            Fs.CopyRecursive(sourceFolder, targetFolder);
        }
    }
}
