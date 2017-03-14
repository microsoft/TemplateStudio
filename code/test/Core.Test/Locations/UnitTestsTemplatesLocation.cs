using System.IO;
using Microsoft.Templates.Core.Locations;
using System;

namespace Microsoft.Templates.Core.Test.Locations
{
    public class UnitTestsTemplatesLocation : TemplatesLocation
    {
        public override string Id { get => "Unit"; }
        public override void Adquire()
        {
            Copy($@"..\..\TestData\{TemplatesLocation.TemplatesFolderName}", CurrentVersionFolder);
            File.WriteAllText(CurrentVersionFilePath, $"0.0.0.0");
        }

        public override bool UpdateAvailable()
        {
            return true;
        }

        protected override string GetLatestTemplateFolder()
        {
            return "0.0.0.0-unit";
        }

        protected static void Copy(string sourceFolder, string targetFolder)
        {
            SafeDeleteDirectory(targetFolder);
            CopyRecursive(sourceFolder, targetFolder);
        }
    }
}