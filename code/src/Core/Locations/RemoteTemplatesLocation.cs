using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Templates.Core.Locations
{
    public class RemoteTemplatesLocation : TemplatesLocation
    {
        private readonly string CdnUrl = Configuration.Current.CdnUrl;
        private const string CdnTemplatesFileName = "Templates.mstx";

        public override void Copy(string workingFolder)
        {
            Download(workingFolder, CdnTemplatesFileName);
        }

        public override string GetVersion(string workingFolder)
        {
            //TODO: ERROR HANDLING
            var sourceUrl = $"{CdnUrl}/{VersionFileName}";
            var destinationFile = Path.Combine(workingFolder, VersionFileName);

            var wc = new WebClient();
            wc.DownloadFile(sourceUrl, destinationFile);

            var version = File.ReadAllText(destinationFile);

            File.Delete(destinationFile);

            return version;
        }

        private void Download(string workingFolder, string fileName)
        {
            EnsureWorkingFolder(workingFolder);

            var sourceUrl = $"{CdnUrl}/{fileName}";
            var file = Path.Combine(workingFolder, fileName);

            SafeDelete(Path.Combine(workingFolder, TemplatesName));

            var wc = new WebClient();
            wc.DownloadFile(sourceUrl, file);

            Templatex.Extract(file, workingFolder);

            File.Delete(file);
        }

        private static void EnsureWorkingFolder(string workingFolder)
        {
            if (!Directory.Exists(workingFolder))
            {
                Directory.CreateDirectory(workingFolder);
            }
        }
    }
}
