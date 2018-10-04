// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Templates.Core.Diagnostics;
using Microsoft.Templates.Core.Packaging;
using Microsoft.Templates.Core.Resources;

namespace Microsoft.Templates.Core.Locations
{
    public class RemoteTemplatesSource : TemplatesSource
    {
        private readonly string _tmpExtension = "_tmp";

        private readonly string _cdnUrl = Configuration.Current.CdnUrl;

        private Version _version;

        public override string Language { get; }

        public override string Platform { get; }

        public RemoteTemplatesSource(string shortLanguage, string platform)
        {
            Language = shortLanguage;
            Platform = platform;
        }

        public override async Task<TemplatesContentInfo> GetContentAsync(TemplatesPackageInfo packageInfo, string workingFolder, CancellationToken ct)
        {
            var finalDestination = Path.Combine(workingFolder, packageInfo.Version.ToString());
            var finalDestinationTemp = string.Concat(finalDestination, _tmpExtension);

            RemoveTemplatesTempFolders(workingFolder);

            //Only extract if final destination does not exist (TODO: Verify this) 
            if (!Directory.Exists(finalDestination))
            {
                var extractionFolder = await ExtractAsync(packageInfo, finalDestinationTemp, false, ct);
                Fs.SafeRenameDirectory(finalDestinationTemp, finalDestination);
            }

            var templatesInfo = new TemplatesContentInfo()
            {
                Date = packageInfo.Date,
                Path = finalDestination,
                Version = packageInfo.Version
            };

            return templatesInfo;
        }

        public override async Task AcquireAsync(TemplatesPackageInfo packageInfo, CancellationToken ct)
        {
            if (string.IsNullOrEmpty(packageInfo.LocalPath) || !File.Exists(packageInfo.LocalPath))
            {
                _version = packageInfo.Version;

                var tempFolder = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());
                var sourceUrl = $"{_cdnUrl}/{packageInfo.Name}";
                var fileTarget = Path.Combine(tempFolder, packageInfo.Name);
                Fs.EnsureFolder(tempFolder);

                await DownloadContentAsync(sourceUrl, fileTarget, ct);
                packageInfo.LocalPath = fileTarget;
            }
        }

        public override async Task LoadConfigAsync(CancellationToken ct)
        {
            var tempFolder = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());
            var sourceUrl = $"{_cdnUrl}/config.json";
            var fileTarget = Path.Combine(tempFolder, "config.json");
            Fs.EnsureFolder(tempFolder);

            await DownloadContentAsync(sourceUrl, fileTarget, ct);

            Config = TemplatesSourceConfig.LoadFromFile(fileTarget);

            Fs.SafeDeleteDirectory(tempFolder);
        }

        private async Task DownloadContentAsync(string sourceUrl, string file, CancellationToken ct)
        {
            var wc = new WebClient();
            try
            {
                ct.Register(() => wc.CancelAsync());

                wc.DownloadProgressChanged += Wc_DownloadProgressChanged;
                await wc.DownloadFileTaskAsync(sourceUrl, file);

                AppHealth.Current.Verbose.TrackAsync(string.Format(StringRes.RemoteTemplatesSourceDownloadContentOkMessage, file, sourceUrl)).FireAndForget();
            }
            catch (WebException ex)
            {
                if (ex.Status == WebExceptionStatus.RequestCanceled)
                {
                    throw new OperationCanceledException(ct);
                }
                else
                {
                    AppHealth.Current.Info.TrackAsync(StringRes.RemoteTemplatesSourceDownloadContentKoInfoMessage).FireAndForget();
                    AppHealth.Current.Error.TrackAsync(string.Format(StringRes.RemoteTemplatesSourceDownloadContentKoErrorMessage, sourceUrl), ex).FireAndForget();
                    throw;
                }
            }
            finally
            {
                wc.DownloadProgressChanged -= Wc_DownloadProgressChanged;
            }
        }

        private void Wc_DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            OnNewVersionAcquisitionProgress(this, new ProgressEventArgs() { Version = _version, Progress = e.ProgressPercentage });
        }

        private async Task<string> ExtractAsync(TemplatesPackageInfo packageInfo, string finalDest, bool verifyPackageSignatures = true, CancellationToken ct = default(CancellationToken))
        {
            _version = packageInfo.Version;
            if (!string.IsNullOrEmpty(packageInfo.LocalPath))
            {
                await ExtractAsync(packageInfo.LocalPath, finalDest, verifyPackageSignatures, ct);
                return Path.GetDirectoryName(packageInfo.LocalPath);
            }
            else
            {
                AppHealth.Current.Error.TrackAsync(StringRes.TemplatesSourceLocalPathEmptyMessage).FireAndForget();
                return null;
            }
        }

        private async Task ExtractAsync(string mstxFilePath, string versionedFolder, bool verifyPackageSignatures = true, CancellationToken ct = default(CancellationToken))
        {
            try
            {
                await TemplatePackage.ExtractAsync(mstxFilePath, versionedFolder, verifyPackageSignatures, ReportExtractionProgress, ct);
                AppHealth.Current.Verbose.TrackAsync($"{StringRes.TemplatesContentExtractedToString} {versionedFolder}.").FireAndForget();
            }
            catch (Exception ex) when (ex.GetType() != typeof(OperationCanceledException))
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

        private void RemoveTemplatesTempFolders(string workingFolder)
        {
            if (!Directory.Exists(workingFolder))
            {
                return;
            }

            var searchOptions = $"*{_tmpExtension}";
            var tempDirectories = Directory.EnumerateDirectories(workingFolder, searchOptions);

            foreach (var dir in tempDirectories)
            {
                Fs.SafeDeleteDirectory(dir, true);
            }
        }
    }
}
