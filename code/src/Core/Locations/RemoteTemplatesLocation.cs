using Microsoft.Templates.Core.Diagnostics;
using Microsoft.Templates.Core.Extensions;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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

        public override string Id { get => Configuration.Current.Environment; }
        
        public override void Adquire()
        {
            Download();
        }

        public override bool UpdateAvailable()
        { 
            bool updatedAvailable = CurrentVersion < GetLatestAlignedVersionAvailable();
            if (updatedAvailable)
            {
                RefreshFolders();
            }
            return updatedAvailable;
        }

        public override bool ExistsContentWithHigherVersionThanWizard()
        {
            return (GetLatestTemplateVersionFolder(false).ToString() != "0.0.0.0");
        }

        protected override string GetLatestTemplateFolder()
        {
            return GetLatestTemplateVersionFolder(true);
        }

        private Version GetLatestAlignedVersionAvailable()
        {
            string s = Path.Combine(LocationFolder, GetLatestTemplateFolder());
            return GetVersionFromFile(Path.Combine(s, VersionFileName));
        }

        private string GetLatestTemplateVersionFolder(bool ensureWizardAligmnent)
        {
            Version latestVersion = new Version(0, 0, 0, 0);
            string currentTemplatesFolder = string.Empty;
            if (Directory.Exists(LocationFolder))
            {
                DirectoryInfo di = new DirectoryInfo(LocationFolder);
                foreach (DirectoryInfo sdi in di.EnumerateDirectories())
                {
                    Version.TryParse(sdi.Name, out Version v);

                    if (v > latestVersion)
                    {
                        if (!ensureWizardAligmnent || (ensureWizardAligmnent && VersionIsAlignedWithWizard(v)))
                        {
                            latestVersion = v;
                            currentTemplatesFolder = sdi.Name;
                        }
                    }
                }
            }
            return currentTemplatesFolder;
        }

        private void Download()
        {
            if (IsDownloadExpired())
            {
                var sourceUrl = $"{CdnUrl}/{CdnTemplatesFileName}";
                var tempFolder = Path.Combine(DownloadsFolder, Path.GetRandomFileName());
                var file = Path.Combine(LocationFolder, CdnTemplatesFileName);

                EnsureFolder(LocationFolder);
                EnsureFolder(tempFolder);

                DownloadContent(sourceUrl, file);

                ExtractContent(file, tempFolder);
            }
        }
        private bool IsDownloadExpired()
        {
            var currentFileVersion = CurrentVersionFilePath;
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

                if (ver > CurrentVersion)
                {
                    Templatex.Extract(file, intermediateTempFolder);

                    var finalTarget = Path.Combine(LocationFolder, ver.ToString());

                    //TODO: Ensure FinalTarget does not exists.

                    SafeMoveDirectory(Path.Combine(intermediateTempFolder, TemplatesFolderName), finalTarget);

                    AppHealth.Current.Verbose.TrackAsync($"Templates content extracted to {finalTarget}. Version adquired: {ver.ToString()}").FireAndForget();
                }
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
    }
}
