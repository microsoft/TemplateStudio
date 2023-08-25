// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.Templates.Core.Diagnostics;
using Microsoft.Templates.Core.Helpers;
using Microsoft.Templates.SharedResources;

namespace Microsoft.Templates.Core.Locations
{
    public class TemplatesSynchronization
    {
        public event Action<object, SyncStatusEventArgs> SyncStatusChanged;

        private static readonly string FolderName = Configuration.Current.RepositoryFolderName;

        private readonly Lazy<string> _workingFolder = new Lazy<string>(() => Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData, Environment.SpecialFolderOption.Create), FolderName));

        private readonly TemplatesContent _content;

        private static object syncLock = new object();

        private readonly TimeSpan instanceSyncWait = new TimeSpan(0, 0, 5);

        private readonly TimeSpan instanceMaxSyncWait = new TimeSpan(0, 0, 30);

        public string CurrentContentFolder { get; private set; }

        public string WorkingFolder => _workingFolder.Value;

        public TemplatesContentInfo CurrentContent { get => _content?.Current; }

        public string CurrentWizardVersion { get; private set; }

        public static bool SyncInProgress { get; private set; }

        public TemplatesSynchronization(TemplatesSource source, string wizardVersion)
        {
            string contentRootFolder = source.GetContentRootFolder(); // CodeGen.Instance?.GetCurrentContentSource(Configuration.Current.RepositoryFolderName, source.Id, source.Platform, source.Language);

            _content = new TemplatesContent(WorkingFolder, source.Id, wizardVersion, source, contentRootFolder);
            CurrentWizardVersion = wizardVersion;
            CurrentContentFolder = _content.TemplatesFolder;

            //// Doing this here rather than as a part of the downloading & checking for new templates flow
            CodeGen.Instance.Cache = CodeGen.Instance.Scanner.Scan(_content.TemplatesFolder);
            //var result = Task.Run(async () => await CodeGen.Instance.Scanner.ScanAsync(_content.TemplatesFolder)).Result;
            //CodeGen.Instance.Cache = result;
        }

        public async Task EnsureContentAsync(bool force = false, CancellationToken ct = default)
        {
            try
            {
                await EnsureVsInstancesSyncingAsync();

                if (LockSync())
                {
                    try
                    {
                        if (!_content.Exists() || force || CurrentContent.Version != CurrentWizardVersion)
                        {
                            _content.GetContentProgress += OnGetContentProgress;

                            await ExtractInstalledContentAsync(ct);
                        }

                        ////if (CurrentContent != null)
                        ////{
                        ////    TelemetryService.Current.SetContentVersionToContext(CurrentContent.Version);
                        ////}
                        ////else
                        ////{
                        ////    AppHealth.Current.Error.TrackAsync(Resources.TemplatesSynchronizationErrorExtracting).FireAndForget();
                        ////}
                    }
                    finally
                    {
                        _content.GetContentProgress -= OnGetContentProgress;

                        UnlockSync();
                    }
                }
            }
            catch (OperationCanceledException)
            {
            }
        }

        public async Task RefreshTemplateCacheAsync(bool force)
        {
            await EnsureVsInstancesSyncingAsync();

            if (LockSync())
            {
                try
                {
                    await UpdateTemplatesCacheAsync(force);

                    await PurgeContentAsync();

                    ////TelemetryService.Current.SetContentVersionToContext(CurrentContent?.Version);
                }
                finally
                {
                    UnlockSync();
                }
            }
        }

        public async Task GetNewContentAsync(CancellationToken ct)
        {
            if (!CanGetNewContent())
            {
                return;
            }

            await EnsureVsInstancesSyncingAsync();

            if (LockSync())
            {
                try
                {
                    bool notifiedCheckingforUpdates = await LoadConfigFileAsync(ct);

                    _content.NewVersionAcquisitionProgress += OnNewVersionAcquisitionProgress;
                    _content.GetContentProgress += OnGetContentProgress;

                    if (_content.IsNewVersionAvailable(out var version))
                    {
                        await GetNewTemplatesAsync(version.ToString(), ct);
                    }

                    if (notifiedCheckingforUpdates)
                    {
                        SyncStatusChanged?.Invoke(this, new SyncStatusEventArgs { Status = SyncStatus.NoUpdates });
                    }
                }
                catch (OperationCanceledException)
                {
                }
                finally
                {
                    _content.NewVersionAcquisitionProgress -= OnNewVersionAcquisitionProgress;
                    _content.GetContentProgress -= OnGetContentProgress;
                    UnlockSync();
                }
            }
        }

        public void CheckForWizardUpdates()
        {
            if (_content.IsWizardUpdateAvailable(out var version))
            {
                SyncStatusChanged?.Invoke(this, new SyncStatusEventArgs { Status = SyncStatus.NewWizardVersionAvailable, Version = version.ToString() });
            }
        }

        private async Task<bool> LoadConfigFileAsync(CancellationToken ct)
        {
            bool notifyCheckingforUpdates = false;

            try
            {
                Task[] downloadTasks = new Task[2];
                downloadTasks[0] = _content.Source.LoadConfigAsync(ct);
                downloadTasks[1] = Task.Delay(1000, ct);

                Task firstFinishedTask = await Task.WhenAny(downloadTasks);

                if (firstFinishedTask.Id == downloadTasks[1].Id)
                {
                    notifyCheckingforUpdates = true;
                    SyncStatusChanged?.Invoke(this, new SyncStatusEventArgs { Status = SyncStatus.CheckingForUpdates });
                    await downloadTasks[0];
                }

                return notifyCheckingforUpdates;
            }
            catch (Exception ex)
            {
                AppHealth.Current.Error.TrackAsync(Resources.TemplatesSynchronizationErrorDownloadingConfig, ex).FireAndForget();
                return notifyCheckingforUpdates;
            }
        }

        private void OnGetContentProgress(object sender, ProgressEventArgs eventArgs)
        {
            SyncStatusChanged?.Invoke(this, new SyncStatusEventArgs { Status = SyncStatus.Preparing, Version = eventArgs.Version, Progress = eventArgs.Progress });
        }

        private void OnNewVersionAcquisitionProgress(object sender, ProgressEventArgs eventArgs)
        {
            SyncStatusChanged?.Invoke(this, new SyncStatusEventArgs { Status = SyncStatus.Acquiring, Version = eventArgs.Version, Progress = eventArgs.Progress });
        }

        private async Task GetNewTemplatesAsync(string version, CancellationToken ct)
        {
            try
            {
                SyncStatusChanged?.Invoke(this, new SyncStatusEventArgs { Status = SyncStatus.Acquiring, Version = version.ToString() });

                await _content.GetNewVersionContentAsync(ct);
            }
            catch (OperationCanceledException)
            {
            }
            catch (Exception ex)
            {
                SyncStatusChanged?.Invoke(this, new SyncStatusEventArgs { Status = SyncStatus.ErrorAcquiring, Version = version.ToString() });
                AppHealth.Current.Error.TrackAsync(Resources.TemplatesSynchronizationErrorAcquiring, ex).FireAndForget();
            }
        }

        private async Task ExtractInstalledContentAsync(CancellationToken ct)
        {
            try
            {
                var installedPackage = _content.ResolveInstalledContent();

                if (installedPackage != null)
                {
                    SyncStatusChanged?.Invoke(this, new SyncStatusEventArgs { Status = SyncStatus.Preparing, Version = installedPackage.Version.ToString() });

                    await _content.GetInstalledContentAsync(installedPackage, ct);
                }
                else
                {
                    AppHealth.Current.Error.TrackAsync(Resources.TemplatesSynchronizationErrorExtracting).FireAndForget();
                }
            }
            catch (Exception ex) when (ex.GetType() != typeof(OperationCanceledException))
            {
                SyncStatusChanged?.Invoke(this, new SyncStatusEventArgs { Status = SyncStatus.None });
                AppHealth.Current.Error.TrackAsync(Resources.TemplatesSynchronizationErrorExtracting, ex).FireAndForget();
            }
        }

        private async Task UpdateTemplatesCacheAsync(bool force)
        {
            try
            {
                CodeGen.Instance.Cache = CodeGen.Instance.Scanner.Scan(_content.TemplatesFolder);
                //var result = Task.Run(async () => await CodeGen.Instance.Scanner.ScanAsync(_content.TemplatesFolder)).Result;
                //CodeGen.Instance.Cache = result;

                SyncStatusChanged?.Invoke(this, new SyncStatusEventArgs { Status = SyncStatus.Ready });
            }
            catch (Exception ex)
            {
                SyncStatusChanged?.Invoke(this, new SyncStatusEventArgs { Status = SyncStatus.None });
                AppHealth.Current.Error.TrackAsync(Resources.TemplatesSynchronizationErrorUpdating, ex).FireAndForget();
            }

            await Task.CompletedTask;
        }

        private async Task PurgeContentAsync()
        {
            try
            {
                await Task.Run(() =>
                {
                    _content.Purge();
                });
            }
            catch (Exception ex)
            {
                await AppHealth.Current.Warning.TrackAsync(Resources.TemplatesSynchronizationPurgeContentAsyncMessage, ex);
            }
        }

        private bool LockSync()
        {
            lock (syncLock)
            {
                if (SyncInProgress)
                {
                    return false;
                }

                SetInstanceSyncLock();
                SyncInProgress = true;
                return SyncInProgress;
            }
        }

        private void UnlockSync()
        {
            lock (syncLock)
            {
                SyncInProgress = false;
                RemoveInstanceSyncLock();
            }
        }

        private async Task EnsureVsInstancesSyncingAsync()
        {
            while (IsOtherInstanceSyncing())
            {
                await AppHealth.Current.Info.TrackAsync(Resources.TemplatesSynchronizationWaitingOtherInstanceMessage);
                await Task.Delay(instanceSyncWait);
            }
        }

        private bool IsOtherInstanceSyncing()
        {
            try
            {
                FileInfo fileInfo = new FileInfo(Path.Combine(_content.TemplatesFolder, ".lock"));
                return fileInfo.Exists && DateTime.Now < fileInfo.CreationTime.Add(instanceMaxSyncWait);
            }
            catch (Exception ex)
            {
                AppHealth.Current.Warning.TrackAsync(Resources.TemplatesSynchronizationWarnReadingLockFileMessage, ex).FireAndForget();

                // No matter the exception. If there is one, we behave exactly the same as if we don't have instance syncronization exclusion.
                return false;
            }
        }

        private void SetInstanceSyncLock()
        {
            try
            {
                FileInfo fileInfo = new FileInfo(Path.Combine(_content.TemplatesFolder, ".lock"));
                if (fileInfo.Exists)
                {
                    fileInfo.Delete();
                }

                Fs.EnsureFolderExists(_content.TemplatesFolder);
                File.WriteAllText(fileInfo.FullName, "Instance syncing");
            }
            catch (Exception ex)
            {
                AppHealth.Current.Warning.TrackAsync(Resources.TemplatesSynchronizationWarnCreatingLockFileMessage, ex).FireAndForget();
            }
        }

        private void RemoveInstanceSyncLock()
        {
            try
            {
                FileInfo fileInfo = new FileInfo(Path.Combine(_content.TemplatesFolder, ".lock"));
                if (fileInfo.Exists)
                {
                    fileInfo.Delete();
                }
            }
            catch (Exception ex)
            {
                AppHealth.Current.Warning.TrackAsync(Resources.TemplatesSynchronizationWarnDeletingLockFileMessage, ex).FireAndForget();
            }
        }

        private bool CanGetNewContent()
        {
            return true;
        }
    }
}
