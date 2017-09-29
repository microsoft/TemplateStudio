// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.IO;
using System.Threading.Tasks;
using System.Reflection;
using System.Threading;

using Microsoft.Templates.Core.Diagnostics;
using Microsoft.Templates.Core.Resources;

namespace Microsoft.Templates.Core.Locations
{
    public class TemplatesSynchronization
    {
        public event Action<object, SyncStatusEventArgs> SyncStatusChanged;

        private static readonly string FolderName = Configuration.Current.RepositoryFolderName;

        private readonly Lazy<string> _workingFolder = new Lazy<string>(() => Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), FolderName));

        private readonly TemplatesSource _source;

        private readonly TemplatesContent _content;

        private static object syncLock = new object();

        private readonly TimeSpan instanceSyncWait = new TimeSpan(0, 0, 5);

        private readonly TimeSpan instanceMaxSyncWait = new TimeSpan(0, 0, 30);

        public string WorkingFolder => _workingFolder.Value;

        public string CurrentTemplatesFolder { get => _content?.TemplatesFolder; }

        public string CurrentContentFolder { get; private set; }

        public Version CurrentContentVersion { get => GetCurrentContentVersion(); }

        public Version CurrentWizardVersion { get; private set; }

        public static bool SyncInProgress { get; private set; }

        public TemplatesSynchronization(TemplatesSource source, Version wizardVersion)
        {
            _source = source ?? throw new ArgumentNullException(nameof(source));
            _content = new TemplatesContent(WorkingFolder, source.Id, wizardVersion);
            CurrentContentFolder = CodeGen.Instance?.GetCurrentContentSource(WorkingFolder, source.Id);
        }

        public async Task DoAsync()
        {
            await EnsureVsInstancesSyncingAsync();

            if (LockSync())
            {
                try
                {
                    await CheckInstallDeployedContentAsync();

                    var acquireCalled = await CheckMandatoryAcquireContentAsync();

                    await UpdateTemplatesCacheAsync();

                    if (!acquireCalled)
                    {
                        await AcquireContentAsync();
                    }

                    await CheckContentStatusAsync();

                    PurgeContentAsync().FireAndForget();

                    TelemetryService.Current.SetContentVersionToContext(CurrentContentVersion);
                }
                finally
                {
                    UnlockSync();
                }
            }
        }

        public async Task RefreshAsync()
        {
            await UpdateTemplatesCacheAsync();
        }

        public async Task CheckForNewContentAsync()
        {
            if (LockSync())
            {
                try
                {
                    await AcquireContentAsync(true);
                    await CheckContentStatusAsync();
                }
                finally
                {
                    UnlockSync();
                }
            }
        }

        private async Task CheckContentStatusAsync()
        {
            await CheckContentUnderVersionAsync();
            await CheckNewVersionAvailableAsync();
            await CheckContentOverVersionAsync();
        }

        private async Task CheckInstallDeployedContentAsync()
        {
            if (!_content.Exists() || RequireExtractInstalledContent())
            {
                await ExtractInstalledContentAsync();
            }
        }
        private async Task<bool> CheckMandatoryAcquireContentAsync()
        {
            return await AcquireContentAsync(_source.ForcedAcquisition || _content.ExistUnderVersion());
        }

        private async Task<bool> AcquireContentAsync(bool force = false)
        {
            bool acquireContentCalled = false;
            if (force || _content.IsExpired(CurrentContentFolder))
            {
                SyncStatusChanged?.Invoke(this, new SyncStatusEventArgs { Status = SyncStatus.Acquiring });

                await Task.Run(() =>
                {
                    AcquireContent();
                });
                acquireContentCalled = true;
                SyncStatusChanged?.Invoke(this, new SyncStatusEventArgs { Status = SyncStatus.Acquired });
            }
            return await Task.FromResult<bool>(acquireContentCalled);
        }

        private async Task ExtractInstalledContentAsync()
        {
            SyncStatusChanged?.Invoke(this, new SyncStatusEventArgs { Status = SyncStatus.Preparing });

            await Task.Run(() => ExtractInstalledContent());

            SyncStatusChanged?.Invoke(this, new SyncStatusEventArgs { Status = SyncStatus.Prepared });
        }

        private void AcquireContent()
        {
            try
            {
                _source.Acquire(_content.TemplatesFolder);
            }
            catch (Exception ex)
            {
                throw new RepositorySynchronizationException(SyncStatus.Acquiring, ex);
            }
        }

        private void ExtractInstalledContent()
        {
            try
            {
                string installedTemplatesPath = GetInstalledTemplatesPath();
                if (File.Exists(installedTemplatesPath))
                {
                    _source.Extract(installedTemplatesPath, _content.TemplatesFolder);
                }
            }
            catch (Exception ex)
            {
                throw new RepositorySynchronizationException(SyncStatus.Acquiring, ex);
            }
        }

        private string GetInstalledTemplatesPath()
        {
            return Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "InstalledTemplates", "Templates.mstx");
        }

        private async Task UpdateTemplatesCacheAsync()
        {
            SyncStatusChanged?.Invoke(this, new SyncStatusEventArgs { Status = SyncStatus.Updating });

            await Task.Run(() => UpdateTemplatesCache());

            SyncStatusChanged?.Invoke(this, new SyncStatusEventArgs { Status = SyncStatus.Updated });
        }

        private async Task CheckContentOverVersionAsync()
        {
            await Task.Run(() =>
            {
                if (_content.ExistOverVersion())
                {
                    if (CurrentContentVersion.IsNull())
                    {
                        SyncStatusChanged?.Invoke(this, new SyncStatusEventArgs { Status = SyncStatus.OverVersionNoContent });
                    }
                    else
                    {
                        SyncStatusChanged?.Invoke(this, new SyncStatusEventArgs { Status = SyncStatus.OverVersion });
                    }
                }
            });
        }

        private async Task CheckContentUnderVersionAsync()
        {
            await Task.Run(() =>
            {
                bool result = _content.ExistUnderVersion();

                if (result)
                {
                    SyncStatusChanged?.Invoke(this, new SyncStatusEventArgs { Status = SyncStatus.UnderVersion });
                }
            });
        }

        private async Task CheckNewVersionAvailableAsync()
        {
            await Task.Run(() =>
            {
                if (_content.IsNewVersionAvailable(CurrentContentFolder))
                {
                    SyncStatusChanged?.Invoke(this, new SyncStatusEventArgs { Status = SyncStatus.NewVersionAvailable });
                }
            });
        }

        private void UpdateTemplatesCache()
        {
            try
            {
                if (_content.RequiresUpdate(CurrentContentFolder) || CodeGen.Instance.Cache.TemplateInfo.Count == 0)
                {
                    CodeGen.Instance.Cache.DeleteAllLocaleCacheFiles();
                    CodeGen.Instance.Cache.Scan(_content.LatestContentFolder);

                    CodeGen.Instance.Settings.SettingsLoader.Save();

                    CurrentContentFolder = CodeGen.Instance.GetCurrentContentSource(WorkingFolder, _source.Id);
                }
            }
            catch (Exception ex)
            {
                throw new RepositorySynchronizationException(SyncStatus.Updating, ex);
            }
        }

        private async Task PurgeContentAsync()
        {
            try
            {
                await Task.Run(() => _content.Purge(CurrentContentFolder));
            }
            catch (Exception ex)
            {
                await AppHealth.Current.Warning.TrackAsync(StringRes.TemplatesSynchronizationPurgeContentAsyncMessage, ex);
            }
        }

        private bool RequireExtractInstalledContent()
        {
            return CurrentContentVersion.IsNull() || CurrentContentVersion < _source.GetVersionFromMstx(GetInstalledTemplatesPath());
        }

        private Version GetCurrentContentVersion()
        {
            return _content?.GetVersionFromFolder(CurrentContentFolder);
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
                await AppHealth.Current.Info.TrackAsync(StringRes.TemplatesSynchronizationWaitingOtherInstanceMessage);
                await Task.Delay(instanceSyncWait);
            }
        }

        private bool IsOtherInstanceSyncing()
        {
            try
            {
                FileInfo fileInfo = new FileInfo(Path.Combine(CurrentTemplatesFolder, ".lock"));
                return fileInfo.Exists && DateTime.Now < fileInfo.CreationTime.Add(instanceMaxSyncWait);
            }
            catch (Exception ex)
            {
                AppHealth.Current.Warning.TrackAsync(StringRes.TemplatesSynchronizationWarnReadingLockFileMessage, ex).FireAndForget();

                // No matter the exception. If there is one, we behave exactly the same as if we don't have instance syncronization exclusion.
                return false;
            }
        }

        private void SetInstanceSyncLock()
        {
            try
            {
                FileInfo fileInfo = new FileInfo(Path.Combine(CurrentTemplatesFolder, ".lock"));
                if (fileInfo.Exists)
                {
                    fileInfo.Delete();
                }
                File.WriteAllText(fileInfo.FullName, "Instance syncing");
            }
            catch (Exception ex)
            {
                AppHealth.Current.Warning.TrackAsync(StringRes.TemplatesSynchronizationWarnCreatingLockFileMessage, ex).FireAndForget();
            }
        }
        private void RemoveInstanceSyncLock()
        {
            try
            {
                FileInfo fileInfo = new FileInfo(Path.Combine(CurrentTemplatesFolder, ".lock"));
                if (fileInfo.Exists)
                {
                    fileInfo.Delete();
                }
            }
            catch (Exception ex)
            {
                AppHealth.Current.Warning.TrackAsync(StringRes.TemplatesSynchronizationWarnDeletingLockFileMessage, ex).FireAndForget();
            }
        }
    }
}
