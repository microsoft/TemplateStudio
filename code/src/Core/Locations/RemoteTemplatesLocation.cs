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

        protected override string LocationId { get => "Remote_" + CdnUrl.Obfuscate(); }

        public override void Adquire()
        {
            Download();
        }

        public override bool Update()
        {
            bool updated = UpdateTemplates();
            if (updated)
            {
                RefreshWorkingFolders();
            }
            return updated;
        }
        public override void Purge()
        {
            PurgeOldContent();
        }
        protected override string GetCurrentTemplatesFolderName()
        {
            Version latestVersion = new Version(0, 0, 0, 0);
            string currentTemplatesFolder = string.Empty;
            if (Directory.Exists(TemplatesFolder))
            {
                DirectoryInfo di = new DirectoryInfo(TemplatesFolder);
                foreach (DirectoryInfo sdi in di.EnumerateDirectories())
                {
                    Version.TryParse(sdi.Name, out Version v);

                    if (v > latestVersion)
                    {
                        currentTemplatesFolder = sdi.Name;
                    }

                    //TODO: Version DOWNLOADED (EXISTING IN TEMP FOLDER MUST BE COORDINATED WIHT THE EXTENSION Mayor.Minor;
                }
            }
            return currentTemplatesFolder;
        }
        
        private void Download()
        {

            if (IsDownloadExpired())
            {
                var sourceUrl = $"{CdnUrl}/{CdnTemplatesFileName}";
                var tempFolder = Path.Combine(TempDownloadsFolder, Path.GetRandomFileName());
                var file = Path.Combine(LocationFolder, CdnTemplatesFileName);

                EnsureFolder(LocationFolder);
                EnsureFolder(tempFolder);

                DownloadContent(sourceUrl, file);

                ExtractContent(file, tempFolder);
            }
        }
        private bool IsDownloadExpired()
        {
            var currentFileVersion = CurrentTemplatesVersionFilePath;
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

        private void ExtractContent(string file, string intermediateTempFolder)
        {
            try
            {
                var ver = GetVersionFromPackage(file);

                Templatex.Extract(file, intermediateTempFolder);

                var finalTarget = Path.Combine(intermediateTempFolder, ver.ToString());

                SafeMoveDirectory(Path.Combine(intermediateTempFolder, TemplatesName), finalTarget);

                AppHealth.Current.Verbose.TrackAsync($"Templates content extracted to {finalTarget}. Version adquired: {ver.ToString()}").FireAndForget();
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

        private bool UpdateTemplates()
        {
            bool templatesUpdated = false;
            try
            {
                var tempContentFolder = GetTempContentFolder();
                if (!String.IsNullOrEmpty(tempContentFolder) && Directory.Exists(tempContentFolder))
                {
                    templatesUpdated = CopyContentIfNeeded(tempContentFolder);
                    //SafeDeleteDirectory(tempContentFolder);
                }
            }
            catch (Exception ex)
            {
                var msg = "The templates content can't be updated.";
                AppHealth.Current.Exception.TrackAsync(ex, msg).FireAndForget();
                throw;
            }
            return templatesUpdated;
        }

        private string GetTempContentFolder()
        {
            DateTime latest = new DateTime(1900, 1, 1);
            string latestDir = null;
            if (Directory.Exists(TempDownloadsFolder))
            {
                DirectoryInfo di = new DirectoryInfo(TempDownloadsFolder);
                foreach (DirectoryInfo sdi in di.EnumerateDirectories())
                {
                    if (sdi.LastWriteTimeUtc > latest)
                    {
                        latest = sdi.LastWriteTimeUtc;
                        latestDir = sdi.FullName;
                    }

                    //TODO: Version DOWNLOADED (EXISTING IN TEMP FOLDER MUST BE COORDINATED WIHT THE EXTENSION Mayor.Minor;
                }
            }
            return latestDir;
        }

        private bool CopyContentIfNeeded(string tempContentFolder)
        {
            bool updated = false;

            if (UpdateAvailable(tempContentFolder))
            {
                CopyRecursive(tempContentFolder, TemplatesFolder);

                AppHealth.Current.Warning.TrackAsync($"Templates successfully updated").FireAndForget();
                updated = true;
            }
            return updated;
        }

        private bool UpdateAvailable(string tempContentFolder)
        {
            string tempFileVersion = GetVersionFilePathFromTempContentFolder(tempContentFolder);

            var tempVersion = GetVersionFromFile(tempFileVersion);
            var currentVersion = GetVersionFromFile(CurrentTemplatesVersionFilePath);

            bool update = tempVersion > currentVersion;

            if (update)
            {
                AppHealth.Current.Verbose.TrackAsync($"New templates content available. Current version:{currentVersion}; New version: {tempVersion}").FireAndForget();
            }

            if (!update && tempVersion == currentVersion)
            {
                RefreshVersionFileExpiration(CurrentTemplatesVersionFilePath, currentVersion.ToString());
            }
            return update;
        }

        private string GetVersionFilePathFromTempContentFolder(string tempContentFolder)
        {
            if (Directory.Exists(tempContentFolder))
            {
                return Directory.EnumerateFiles(tempContentFolder, VersionFileName, SearchOption.AllDirectories).FirstOrDefault();
            }
            else
            {
                return String.Empty;
            }
        }

        private static void RefreshVersionFileExpiration(string installedVersionFile, string installedVersion)
        {
            if (File.Exists(installedVersionFile))
            {
                File.WriteAllText(installedVersionFile, installedVersion);
            }
        }


        private static void EnsureFolder(string folder)
        {
            if (!Directory.Exists(folder))
            {
                Directory.CreateDirectory(folder);
            }
        }

        private void PurgeOldContent()
        {
            DirectoryInfo di = new DirectoryInfo(TempDownloadsFolder);
            var tobedeleted = di.EnumerateDirectories().Where(sdi => sdi.LastWriteTimeUtc.AddDays(Configuration.Current.DaysToKeepTempDownloads) < DateTime.UtcNow);
            foreach (var sdi in tobedeleted)
            {
                SafeDeleteDirectory(sdi.FullName);
            }

            //TODO: Purge old templates.
        }
    }
}
