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
        private const string CdnTemplatesFileName = "UWPTemplates.tmpltx";

        public override void Copy(string workingFolder)
        {
            Download(workingFolder, CdnTemplatesFileName, TemplatesName, ZipFile.ExtractToDirectory, true);
            Download(workingFolder, VersionFileName, TemplatesName, SafeCopyFile);
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

        private void Download(string workingFolder, string fileName, string folderName, Action<string, string> postDownload, bool clean = false)
        {
            EnsureWorkingFolder(workingFolder);

            var sourceUrl = $"{CdnUrl}/{fileName}";
            var destFolder = Path.Combine(workingFolder, folderName);
            var file = Path.Combine(workingFolder, fileName);

            if (clean)
            {
                SafeDelete(destFolder);
            }

            var signedFile = file + Templatex.DefaultExtension;
            var signedSourceUrl = sourceUrl + Templatex.DefaultExtension;
            var wc = new WebClient();
            wc.DownloadFile(signedSourceUrl, signedFile);
            Templatex.Extract(signedFile, workingFolder);

            postDownload(file, destFolder);

            File.Delete(file);
            File.Delete(signedFile);
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
