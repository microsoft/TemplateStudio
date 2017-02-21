using Microsoft.Templates.Core.Diagnostics;
using Microsoft.Templates.Core.Extensions;
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
            var sourceUrl = $"{CdnUrl}/{VersionFileName}";
            var destinationFile = Path.Combine(workingFolder, VersionFileName);

            try
            {
                var wc = new WebClient();
                wc.DownloadFile(sourceUrl, destinationFile);

                var version = File.ReadAllText(destinationFile);
                return version;
            }
            catch (Exception ex)
            {
                AppHealth.Current.Error.TrackAsync("Error downloading the version file.",ex).FireAndForget();
                throw;
            }
            finally
            {
                if (File.Exists(destinationFile))
                {
                    File.Delete(destinationFile);
                }
            }
        }

        private void Download(string workingFolder, string fileName)
        {
            EnsureWorkingFolder(workingFolder);

            var sourceUrl = $"{CdnUrl}/{fileName}";
            var tempFolder = Path.Combine(workingFolder, "temp");
            var file = Path.Combine(workingFolder, fileName);

            DownloadContent(sourceUrl, file);

            ExtractContent(file, tempFolder);

            UpdateTemplates(tempFolder, workingFolder);

            CleanUpTempFiles(tempFolder, file);
        }

        private static void ExtractContent(string file, string workingFolder)
        {
            try
            {
                Templatex.Extract(file, workingFolder);
            }
            catch(Exception ex)
            {
                var msg = "The templates content can't be extracted.";
                AppHealth.Current.Exception.TrackAsync(ex, msg).FireAndForget();
                throw;
            }
        }

        private static void CleanUpTempFiles(string usedTempFolder, string mstxFile)
        {
            try
            {
                File.Delete(mstxFile);
                Directory.Delete(usedTempFolder, true);
            }
            catch(Exception ex)
            {
                var msg = "The cleanup task can't be executed.";
                AppHealth.Current.Exception.TrackAsync(ex, msg).FireAndForget();
                throw;
            }
        }

        private static void UpdateTemplates(string tempFoder, string workingFolder)
        {
            try
            {
                SafeDelete(Path.Combine(workingFolder, TemplatesName));
                CopyRecursive(Path.Combine(tempFoder, TemplatesName), Path.Combine(workingFolder, TemplatesName));
            }
            catch(Exception ex)
            {
                var msg = "The templates content can't be updated.";
                AppHealth.Current.Exception.TrackAsync(ex, msg).FireAndForget();
                throw;
            }
        }

        private static void DownloadContent(string sourceUrl, string file)
        {
            try
            {
                var wc = new WebClient();
                wc.DownloadFile(sourceUrl, file);
            }
            catch (Exception ex)
            {
                string msg = "The templates content can't be downloaded.";
                AppHealth.Current.Exception.TrackAsync(ex, msg).FireAndForget();
                throw;
            }
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
