// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.IO;
using System.Text;

using Microsoft.Templates.Core.Locations;
using Microsoft.Templates.Core.Packaging;

namespace Microsoft.Templates.Core.Test.Locations
{
    public sealed class UnitTestsTemplatesSource : TemplatesSource
    {
        private string _localVersion = "0.0.0.0";

        public override string Id => "UnitTest";
        protected override bool VerifyPackageSignatures => false;
        public override bool ForcedAcquisition => true;

        protected override string AcquireMstx()
        {
            var tempFolder = Path.Combine(GetTempFolder(), SourceFolderName);

            Copy($@"..\..\TestData\{SourceFolderName}", tempFolder);

            File.WriteAllText(Path.Combine(tempFolder, "version.txt"), _localVersion, Encoding.UTF8);

            return TemplatePackage.Pack(tempFolder);
        }

        private static void Copy(string sourceFolder, string targetFolder)
        {
            Fs.SafeDeleteDirectory(targetFolder);
            Fs.CopyRecursive(sourceFolder, targetFolder);
        }
    }
}
