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
            return UpdateTemplates(Path.Combine(workingFolder, TempFolderName), workingFolder);
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
                AppHealth.Current.Info.TrackAsync($"Templates content downloaded from {sourceUrl}.").FireAndForget();
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
                AppHealth.Current.Info.TrackAsync($"Templates content extracted to {targetFolder}. Version adquired: {ver}").FireAndForget();
                AppHealth.Current.Info.TrackAsync($"Templates will be updated if required next time you launch the wizard.").FireAndForget();
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

        private static bool UpdateTemplates(string tempFolder, string workingFolder)
        {
            bool templatesUpdated = false;
            try
            {
                string downloadedTemplatesDir = Path.Combine(tempFolder, TemplatesName);
                string downloadedVersionFile = Path.Combine(downloadedTemplatesDir, VersionFileName);
                string installedTemplatesDir = Path.Combine(workingFolder, TemplatesName);
                string installedVersionFile = Path.Combine(installedTemplatesDir, VersionFileName);

                var downloadedVersion = GetVersionFromFile(downloadedVersionFile);
                var installedVersion = GetVersionFromFile(installedVersionFile);

                if (downloadedVersion != "0.0.0" && downloadedVersion != installedVersion)
                {
                    AppHealth.Current.Info.TrackAsync($"There is a new version available for templates content. Previous version:{downloadedVersion}, Current Version: {installedVersion}").FireAndForget();

                    SafeDelete(installedTemplatesDir);
                    CopyRecursive(downloadedTemplatesDir, installedTemplatesDir);

                    AppHealth.Current.Info.TrackAsync($"Templates has been updated with version {downloadedVersion}.").FireAndForget();

                    SafeCleanUpTempFolder(tempFolder);

                    templatesUpdated = true;
                }
                else
                {
                    RefreshVersionFileExpiration(installedVersionFile, installedVersion);
                }
            }
            catch(Exception ex)
            {
                var msg = "The templates content can't be updated.";
                AppHealth.Current.Exception.TrackAsync(ex, msg).FireAndForget();
                throw;
            }
            return templatesUpdated;
        }

        private static void RefreshVersionFileExpiration(string installedVersionFile, string installedVersion)
        {
            File.WriteAllText(installedVersionFile, installedVersion);
        }

        private static bool IsUpdateAvailableInternal(string downloadedVersionFile, string installedVersionFile)
        {
            var downloadedVersion = GetVersionFromFile(downloadedVersionFile);
            var installedVersion = GetVersionFromFile(installedVersionFile);
            
            return 
        }
        public static string GetVersionFromFile(string versionFilePath)
        {
            var version = "0.0.0";
            if (File.Exists(versionFilePath))
            {
                version = File.ReadAllText(versionFilePath);
            }
            return version;
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
                AppHealth.Current.Error.TrackAsync(msg).FireAndForget();
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
