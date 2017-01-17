using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Templates.Core.Locations
{
    public class TestTemplatesLocation : TemplatesLocation
    {
        public override void Copy(string workingFolder)
        {
            Copy($@"..\..\..\{TemplatesLocation.PackagesName}", workingFolder);
            Copy($@"..\..\..\..\..\{TemplatesLocation.TemplatesName}", workingFolder);
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
