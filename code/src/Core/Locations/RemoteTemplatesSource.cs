// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using Microsoft.Templates.Core.Diagnostics;
using Microsoft.Templates.Core.Packaging;
using Microsoft.Templates.Core.Resources;

namespace Microsoft.Templates.Core.Locations
{
    public class RemoteTemplatesSource : TemplatesSource
    {
        private readonly string _cdnUrl = Configuration.Current.CdnUrl;

        private Version _version;

        public override async Task<TemplatesContentInfo> GetContentAsync(TemplatesPackageInfo packageInfo, string workingFolder)
        {
            var extractionFolder = await ExtractAsync(packageInfo);

            var finalDestination = Path.Combine(workingFolder, packageInfo.Version.ToString());

             await Fs.SafeMoveDirectoryAsync(Path.Combine(extractionFolder, "Templates"), finalDestination, true, ReportCopyProgress);
                Fs.SafeDeleteDirectory(Path.GetDirectoryName(packageInfo.LocalPath));

            var templatesInfo = new TemplatesContentInfo()
            {
                Date = packageInfo.Date,
                Path = finalDestination,
                Version = packageInfo.Version
            };

            return templatesInfo;
        }

        public override async Task AcquireAsync(TemplatesPackageInfo packageInfo)
        {
            if (string.IsNullOrEmpty(packageInfo.LocalPath) || !File.Exists(packageInfo.LocalPath))
            {
                _version = packageInfo.Version;

                var tempFolder = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());
                var sourceUrl = $"{_cdnUrl}/{packageInfo.Name}";
                var fileTarget = Path.Combine(tempFolder, packageInfo.Name);
                Fs.EnsureFolder(tempFolder);

                await DownloadContentAsync(sourceUrl, fileTarget);
                packageInfo.LocalPath = fileTarget;
            }
        }

        public override void LoadConfig()
        {
            var tempFolder = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());
            var sourceUrl = $"{_cdnUrl}/config.json";
            var fileTarget = Path.Combine(tempFolder, "config.json");
            Fs.EnsureFolder(tempFolder);

            DownloadContent(sourceUrl, fileTarget);

            Config = TemplatesSourceConfig.LoadFromFile(fileTarget);

            Fs.SafeDeleteDirectory(tempFolder);
        }

        private static void DownloadContent(string sourceUrl, string file)
        {
            try
            {
                var wc = new WebClient();

                wc.DownloadFile(sourceUrl, file);
                AppHealth.Current.Verbose.TrackAsync(string.Format(StringRes.RemoteTemplatesSourceDownloadContentOkMessage, file, sourceUrl)).FireAndForget();
            }
            catch (Exception ex)
            {
                AppHealth.Current.Info.TrackAsync(StringRes.RemoteTemplatesSourceDownloadContentKoInfoMessage).FireAndForget();
                AppHealth.Current.Error.TrackAsync(string.Format(StringRes.RemoteTemplatesSourceDownloadContentKoErrorMessage, sourceUrl), ex).FireAndForget();
            }
        }

        private async Task DownloadContentAsync(string sourceUrl, string file)
        {
            try
            {
                var wc = new WebClient();

                wc.DownloadProgressChanged += Wc_DownloadProgressChanged;
                await wc.DownloadFileTaskAsync(sourceUrl, file);

                AppHealth.Current.Verbose.TrackAsync(string.Format(StringRes.RemoteTemplatesSourceDownloadContentOkMessage, file, sourceUrl)).FireAndForget();
            }
            catch (Exception ex)
            {
                AppHealth.Current.Info.TrackAsync(StringRes.RemoteTemplatesSourceDownloadContentKoInfoMessage).FireAndForget();
                AppHealth.Current.Error.TrackAsync(string.Format(StringRes.RemoteTemplatesSourceDownloadContentKoErrorMessage, sourceUrl), ex).FireAndForget();
                throw;
            }
        }

        private void Wc_DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            OnNewVersionAcquisitionProgress(this, new ProgressEventArgs() { Version = _version, Progress = e.ProgressPercentage });
        }

        private async Task<string> ExtractAsync(TemplatesPackageInfo packageInfo, bool verifyPackageSignatures = true)
        {
            _version = packageInfo.Version;
            if (!string.IsNullOrEmpty(packageInfo.LocalPath))
            {
                await ExtractAsync(packageInfo.LocalPath, Path.GetDirectoryName(packageInfo.LocalPath), verifyPackageSignatures);
                return Path.GetDirectoryName(packageInfo.LocalPath);
            }
            else
            {
                AppHealth.Current.Error.TrackAsync(StringRes.TemplatesSourceLocalPathEmptyMessage).FireAndForget();
                return null;
            }
        }

        private async Task ExtractAsync(string mstxFilePath, string versionedFolder, bool verifyPackageSignatures = true)
        {
            try
            {
                await TemplatePackage.ExtractAsync(mstxFilePath, versionedFolder, verifyPackageSignatures, ReportExtractionProgress);
                AppHealth.Current.Verbose.TrackAsync($"{StringRes.TemplatesContentExtractedToString} {versionedFolder}.").FireAndForget();
            }
            catch (Exception ex)
            {
                AppHealth.Current.Exception.TrackAsync(ex, StringRes.TemplatesSourceExtractContentMessage).FireAndForget();
                throw;
            }
        }

        private void ReportExtractionProgress(int progress)
        {
            OnGetContentProgress(this, new ProgressEventArgs() { Version = _version, Progress = progress });
        }

        private void ReportCopyProgress(int progress)
        {
            OnCopyProgress(this, new ProgressEventArgs() { Version = _version, Progress = progress });
        }
    }
}
