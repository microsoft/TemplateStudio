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


        public override void Adquire(string workingFolder)
        {
            Download(workingFolder, CdnTemplatesFileName);
        }

        public override bool Update(string workingFolder)
        {
            return UpdateTemplates(workingFolder);
        }

        private void Download(string workingFolder, string fileName)
        {

            if (IsDownloadExpired(workingFolder))
            {
                var sourceUrl = $"{CdnUrl}/{fileName}";
                var tempFolder = Path.Combine(workingFolder, TempFolderName);
                var file = Path.Combine(workingFolder, fileName);

                EnsureWorkingFolder(workingFolder);

                DownloadContent(sourceUrl, file);

                ExtractContent(file, tempFolder);
            }
        }
        private bool IsDownloadExpired(string workingFolder)
        {
            var currentFileVersion = Path.Combine(workingFolder, Path.Combine(TemplatesName, VersionFileName));
            if (!File.Exists(currentFileVersion))
            {
                return true;
            }

            var fileVersion = new FileInfo(currentFileVersion);
            var downloadExpiration = fileVersion.LastWriteTime.AddMinutes(Configuration.Current.VersionCheckingExpirationMinutes);
            AppHealth.Current.Verbose.TrackAsync($"Current templates content expiration: {downloadExpiration.ToString()}").FireAndForget();
            return downloadExpiration <= DateTime.Now;
        }

        private static void DownloadContent(string sourceUrl, string file)
        {
            try
            {
                var wc = new WebClient();
                wc.DownloadFile(sourceUrl, file);
                AppHealth.Current.Verbose.TrackAsync($"Templates content downloaded from {sourceUrl}.").FireAndForget();
            }
            catch (Exception ex)
            {
                string msg = "The templates content can't be downloaded.";
                AppHealth.Current.Exception.TrackAsync(ex, msg).FireAndForget();
                throw;
            }
        }

        private static void ExtractContent(string file, string targetFolder)
        {
            try
            {
                Templatex.Extract(file, targetFolder);
                var ver = GetVersionFromFile(Path.Combine(targetFolder, Path.Combine(TemplatesName, VersionFileName)));
                AppHealth.Current.Verbose.TrackAsync($"Templates content extracted to {targetFolder}. Version adquired: {ver}").FireAndForget();
            }
            catch (Exception ex)
            {
                var msg = "The templates content can't be extracted.";
                AppHealth.Current.Exception.TrackAsync(ex, msg).FireAndForget();
                throw;
            }
            finally
            {
                if (File.Exists(file))
                {
                    File.Delete(file);
                }
            }
        }

        private static bool UpdateTemplates(string workingFolder)
        {
            bool templatesUpdated = false;
            var tempFolder = Path.Combine(workingFolder, TempFolderName);
            try
            {

                templatesUpdated = CopyContentIfNeeded(workingFolder);
                SafeCleanUpTempFolder(tempFolder);
            }
            catch (Exception ex)
            {
                var msg = "The templates content can't be updated.";
                AppHealth.Current.Exception.TrackAsync(ex, msg).FireAndForget();
                throw;
            }
            return templatesUpdated;
        }

        private static bool CopyContentIfNeeded(string workingFolder)
        {
            bool updated = false;
            string tempContentDir = Path.Combine(workingFolder, Path.Combine(TempFolderName, TemplatesName));
            string currentContentDir = Path.Combine(workingFolder, TemplatesName);

            if (UpdateAvailable(workingFolder))
            {
                SafeDelete(currentContentDir);
                CopyRecursive(tempContentDir, currentContentDir);

                AppHealth.Current.Warning.TrackAsync($"Templates successfully updated").FireAndForget();
                updated = true;
            }
            return updated;
        }

        private static bool UpdateAvailable(string workingFolder)
        {
            string tempContentDir = Path.Combine(workingFolder, Path.Combine(TempFolderName, TemplatesName));
            string tempFileVersion = Path.Combine(tempContentDir, VersionFileName);
            string currentFileVersion = Path.Combine(workingFolder, Path.Combine(TemplatesName, VersionFileName));

            var tempVersion = GetVersionFromFile(tempFileVersion);
            var currentVersion = GetVersionFromFile(currentFileVersion);

            bool update = tempVersion != "0.0.0" && tempVersion != currentVersion;

            if (update)
            {
                AppHealth.Current.Verbose.TrackAsync($"New templates content available. Current version:{currentVersion}; New version: {tempVersion}").FireAndForget();
            }

            if (!update && tempVersion == currentVersion)
            {
                RefreshVersionFileExpiration(currentFileVersion, currentVersion);
            }
            return update;
        }
        private static void RefreshVersionFileExpiration(string installedVersionFile, string installedVersion)
        {
            if (File.Exists(installedVersionFile))
            {
                File.WriteAllText(installedVersionFile, installedVersion);
            }
        }

        private static void SafeCleanUpTempFolder(string usedTempFolder)
        {
            try
            {
                if (Directory.Exists(usedTempFolder))
                {
                    Directory.Delete(usedTempFolder, true);
                }
            }
            catch (Exception ex)
            {
                var msg = $"The temp folder {usedTempFolder} can't be delete. Error: {ex.Message}";
                AppHealth.Current.Warning.TrackAsync(msg).FireAndForget();
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
