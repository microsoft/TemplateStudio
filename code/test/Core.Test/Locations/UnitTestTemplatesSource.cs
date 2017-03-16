using Microsoft.Templates.Core.Locations;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Templates.Core.Test.Locations
{
    public class UnitTestsTemplatesSource : TemplatesSource
    {
        private string LocalVersion = "0.0.0.0";
        public override string Id { get => "UnitTest"; }
        public override void Adquire(string targetFolder)
        {
            var targetVersionFolder = Path.Combine(targetFolder, LocalVersion);
            Copy($@"..\..\TestData\{SourceFolderName}", targetVersionFolder);
        }

        protected static void Copy(string sourceFolder, string targetFolder)
        {
            Fs.SafeDeleteDirectory(targetFolder);
            Fs.CopyRecursive(sourceFolder, targetFolder);
        }
    }
}
