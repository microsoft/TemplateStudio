// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.IO;

using Microsoft.Templates.Core.Locations;
using System.Security;
using System.Security.Cryptography.X509Certificates;

namespace Microsoft.Templates.Core.Test.Locations
{
    public sealed class UnitTestsTemplatesSource : TemplatesSource
    {
        private string LocalVersion = "0.0.0.0";

        public override string Id => "UnitTest"; 
        protected override bool VerifyPackageSignatures => false;
        public override bool ForcedAcquisition => true; 

        protected override string AcquireMstx()
        {
            var tempFolder = Path.Combine(GetTempFolder(), SourceFolderName);

            var sourcePath = $@"..\..\TestData\{SourceFolderName}";

            Copy(sourcePath, tempFolder);

            File.WriteAllText(Path.Combine(tempFolder, "version.txt"), LocalVersion);

            return Templatex.Pack(tempFolder);
        }

        private static void Copy(string sourceFolder, string targetFolder)
        {
            Fs.SafeDeleteDirectory(targetFolder);
            Fs.CopyRecursive(sourceFolder, targetFolder);
        }
    }
}
