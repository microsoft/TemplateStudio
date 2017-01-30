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
        private const string CdnPackagesFileName = "packages.nupkg";
        private const string CdnTemplatesFileName = "UWPTemplates.zip";

        public override void Copy(string workingFolder)
        {
            Download(workingFolder, CdnPackagesFileName, PackagesName, SafeCopyFile);
            Download(workingFolder, CdnTemplatesFileName, TemplatesName, ZipFile.ExtractToDirectory);
        }

        private void Download(string workingFolder, string fileName, string folderName, Action<string, string> postDownload)
        {
            EnsureWorkingFolder(workingFolder);

            var sourceUrl = $"{CdnUrl}/{fileName}";
            var destFolder = Path.Combine(workingFolder, folderName);
            var file = Path.Combine(workingFolder, fileName);

            SafeDelete(destFolder);

            var wc = new WebClient();
            wc.DownloadFile(sourceUrl, file);

            postDownload(file, destFolder);

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
