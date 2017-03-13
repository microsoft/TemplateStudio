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
        protected override string LocationId { get => "Local"; }

        public override void Adquire()
        {
            //NO ADQUSITION REQUIRED;
        }
        public override bool Update()
        {
            Copy($@"..\..\..\..\..\{TemplatesLocation.TemplatesName}", CurrentTemplatesVersionFolder);
            File.WriteAllText(CurrentTemplatesVersionFilePath, $"0.0.0.0");
            return true;
        }
        public override void Purge()
        {
            //No purge required
        }
        protected override string GetCurrentTemplatesFolderName() {
            return "0.0.0.0-local";
        }

        protected static void Copy(string sourceFolder, string targetFolder)
        {
            SafeDeleteDirectory(targetFolder);
            CopyRecursive(sourceFolder, targetFolder);
        }
    }
}
