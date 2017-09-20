// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.IO;
using System.Text;
using Microsoft.Templates.Core;
using Microsoft.Templates.Core.Locations;
using Microsoft.Templates.Core.Packaging;

namespace Microsoft.Templates.Test
{
    public sealed class BuildMVVMBasicTestTemplatesSource : TemplatesSource
    {
        public string LocalTemplatesVersion { get; private set; }

        protected override bool VerifyPackageSignatures => false;

        public override bool ForcedAcquisition => true;

        public string Origin => $@"..\..\..\..\..\{SourceFolderName}";

        public override string Id => "TestBuildMVVMBasic";

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
