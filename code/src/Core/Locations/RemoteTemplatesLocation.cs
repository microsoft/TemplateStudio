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

        public override (LocationCopyStatus Status, string Message) Copy(string workingFolder)
        {
            return Download(workingFolder, CdnTemplatesFileName);
        }

        public override string GetVersion(string workingFolder)
        {
            var version = "1.0.0";
            try
            {
                var sourceUrl = $"{CdnUrl}/{VersionFileName}";
                var destinationFile = Path.Combine(workingFolder, VersionFileName);

                var wc = new WebClient();
                wc.DownloadFile(sourceUrl, destinationFile);

                version = File.ReadAllText(destinationFile);

                File.Delete(destinationFile);
            }
            catch (Exception ex)
            {
                AppHealth.Current.Error.TrackAsync("Error downloading the version file.",ex).FireAndForget();
            }

            return version;
        }

        private (LocationCopyStatus Status, string Message) Download(string workingFolder, string fileName)
        {
            var status = LocationCopyStatus.Started;
            var message = "Content adquisition started.";

            EnsureWorkingFolder(workingFolder);

            var sourceUrl = $"{CdnUrl}/{fileName}";
            var tempFoder = Path.Combine(workingFolder, "temp");
            var file = Path.Combine(workingFolder, fileName);

           
            if (DownloadContent(sourceUrl, file, out message))
            {
                status = LocationCopyStatus.SourceAdquired;
                AppHealth.Current.Verbose.TrackAsync($"LocationCopyStatus: {status.ToString()}. {message}").FireAndForget();

                if (ExtractContent(file, tempFoder, out message))
                {
                    AppHealth.Current.Verbose.TrackAsync($"LocationCopyStatus: {status.ToString()}. {message}").FireAndForget();
                    if (UpdateTemplatesFolder(tempFoder, workingFolder, out message))
                    {
                        status = LocationCopyStatus.TargetUpdated;
                        AppHealth.Current.Verbose.TrackAsync($"LocationCopyStatus: {status.ToString()}. {message}").FireAndForget();

                        if (CleanUpTempFiles(tempFoder, file, out message))
                        {
                            status = LocationCopyStatus.Finished;
                            AppHealth.Current.Verbose.TrackAsync($"LocationCopyStatus: {status.ToString()}. {message}").FireAndForget();
                        }
                    }
                }
            }
            return (status, message);
        }

        private static bool ExtractContent(string file, string workingFolder, out string message)
        {
            try
            {
                Templatex.Extract(file, workingFolder);
                message = "Content extracted successfully.";
                return true;
            }
            catch(Exception ex)
            {
                var msg = "The templates content can't be extracted.";
                AppHealth.Current.Exception.TrackAsync(ex, msg).FireAndForget();
                message = $"{msg}. Error message: {ex.Message}";
                return false;
            }
        }

        private static bool CleanUpTempFiles(string usedTempFolder, string mstxFile, out string message)
        {
            try
            {
                File.Delete(mstxFile);
                Directory.Delete(usedTempFolder, true);
                message = "Templates content updated successfully.";
                return true;
            }
            catch(Exception ex)
            {
                var msg = "The cleanup task can't be executed.";
                AppHealth.Current.Exception.TrackAsync(ex, msg).FireAndForget();
                message = $"{msg}. Error message: {ex.Message}";
                return false;
            }
        }

        private static bool UpdateTemplatesFolder(string tempFoder, string workingFolder, out string message)
        {
            try
            {
                SafeDelete(Path.Combine(workingFolder, TemplatesName));
                CopyRecursive(Path.Combine(tempFoder, TemplatesName), Path.Combine(workingFolder, TemplatesName));
                message = "Templates content updated successfully.";
                return true;
            }
            catch(Exception ex)
            {
                var msg = "The templates content can't be updated.";
                AppHealth.Current.Exception.TrackAsync(ex, msg).FireAndForget();
                message = $"{msg}. Error message: {ex.Message}";
                return false;
            }
        }

        private bool DownloadContent(string sourceUrl, string file, out string message)
        {
            try
            {
                var wc = new WebClient();
                wc.DownloadFile(sourceUrl, file);
                message = "Templates content downloaded successfully.";
                return true;

            }
            catch (Exception ex)
            {
                string msg = "The templates content can't be downloaded.";
                AppHealth.Current.Exception.TrackAsync(ex, msg).FireAndForget();
                message = $"{msg}. Error message: {ex.Message}";
                return false;
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
