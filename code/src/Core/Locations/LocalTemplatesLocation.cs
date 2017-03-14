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
        public override string Id { get => "Local"; }

        public override void Adquire()
        {
            Copy($@"..\..\..\..\..\{TemplatesLocation.TemplatesFolderName}", CurrentVersionFolder);
            File.WriteAllText(CurrentVersionFilePath, $"0.0.0.0");
        }
        public override bool UpdateAvailable()
        {
            return true;
        }

        public override bool ExistsContentWithHigherVersionThanWizard()
        {
            return false;
        }

        protected override string GetLatestTemplateFolder() {
            return "0.0.0.0";
        }

        protected static void Copy(string sourceFolder, string targetFolder)
        {
            SafeDeleteDirectory(targetFolder);
            CopyRecursive(sourceFolder, targetFolder);
        }
    }
}
