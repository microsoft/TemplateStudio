using System.IO;
using Microsoft.Templates.Core.Locations;
using System;

namespace Microsoft.Templates.Core.Test
{
    public class UnitTestsTemplatesLocation : TemplatesLocation
    {
        public override void Copy(string workingFolder)
        {
            Copy($@"..\..\TestData\{TemplatesLocation.TemplatesName}", workingFolder);
        }

        public override string GetVersion(string workingFolder)
        {
            return string.Empty;
        }

        protected static void Copy(string sourceFolder, string workingFolder)
        {
            var sourceFolderName = new DirectoryInfo(Path.GetFullPath(sourceFolder)).Name;
            workingFolder = Path.Combine(workingFolder, sourceFolderName);

            SafeDelete(workingFolder);

            CopyRecursive(sourceFolder, workingFolder);
        }
    }
}