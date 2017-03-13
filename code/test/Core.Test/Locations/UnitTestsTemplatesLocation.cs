using System.IO;
using Microsoft.Templates.Core.Locations;
using System;

namespace Microsoft.Templates.Core.Test.Locations
{
    public class UnitTestsTemplatesLocation : TemplatesLocation
    {
        protected override string LocationId { get => "Unit"; }
        public override void Adquire()
        {            
            //NO ADQUSITION REQUIRED;
        }

        public override bool Update()
        {
            Copy($@"..\..\TestData\{TemplatesLocation.TemplatesName}", CurrentTemplatesVersionFolder);
            File.WriteAllText(CurrentTemplatesVersionFilePath, $"0.0.0.0");
            return true;
        }

        protected override string GetCurrentTemplatesFolderName()
        {
            return "0.0.0.0-unit";
        }
        public override void Purge()
        {
            //No purge required
        }

        protected static void Copy(string sourceFolder, string targetFolder)
        {
            SafeDeleteDirectory(targetFolder);
            CopyRecursive(sourceFolder, targetFolder);
        }
    }
}