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
            //NO ADQUSITION REQUIRED;
        }
        public override bool Update(string workingFolder)
        {
            var targetFolder = Path.Combine(workingFolder, TemplatesName);
            Copy($@"..\..\..\..\..\{TemplatesLocation.TemplatesName}", targetFolder);
            File.WriteAllText(Path.Combine(targetFolder, VersionFileName), $"1.0.0-local{DateTime.Now.ToString("yyyyMMddHHmmss")}");
            return true;
        }

        protected static void Copy(string sourceFolder, string targetFolder)
        {
            SafeDelete(targetFolder);
            CopyRecursive(sourceFolder, targetFolder);
        }
    }
}
