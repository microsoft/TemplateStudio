using System.IO;
using Microsoft.Templates.Core.Locations;
using System;

namespace Microsoft.Templates.Core.Test.Locations
{
    public class UnitTestsTemplatesLocation : TemplatesLocation
    {
        public override void Adquire(string workingFolder)
        {            
            //NO ADQUSITION REQUIRED;
        }

        public override bool Update(string workingFolder)
        {
            var targetFolder = Path.Combine(workingFolder, TemplatesName);
            Copy($@"..\..\TestData\{TemplatesLocation.TemplatesName}", targetFolder);
            return true;
        }

        protected static void Copy(string sourceFolder, string targetFolder)
        {
            SafeDelete(targetFolder);
            CopyRecursive(sourceFolder, targetFolder);
        }
    }
}