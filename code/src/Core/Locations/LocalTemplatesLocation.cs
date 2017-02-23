using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Templates.Core.Locations
{
    public class LocalTemplatesLocation : TemplatesLocation
    {
        public override void Adquire(string workingFolder)
        {
            Copy($@"..\..\..\..\..\{TemplatesLocation.TemplatesName}", workingFolder);
        }
        public override bool Update(string workingFolder)
        {
            return true;
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
