// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;

using Microsoft.Templates.Core.Diagnostics;
using Microsoft.Templates.Core.Resources;

namespace Microsoft.Templates.Core.Locations
{
    public class TemplatesSynchronization
    {
        public event Action<object, SyncStatusEventArgs> SyncStatusChanged;

        private static readonly string FolderName = Configuration.Current.RepositoryFolderName;

        private readonly Lazy<string> _workingFolder = new Lazy<string>(() => Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), FolderName));

        private readonly TemplatesContent _content;

        private static object syncLock = new object();

        private readonly TimeSpan instanceSyncWait = new TimeSpan(0, 0, 5);

        private readonly TimeSpan instanceMaxSyncWait = new TimeSpan(0, 0, 30);

        public string WorkingFolder => _workingFolder.Value;

        public TemplatesContentInfo CurrentContent { get => _content?.Current; }

        public Version CurrentWizardVersion { get; private set; }

        public static bool SyncInProgress { get; private set; }

        public TemplatesSynchronization(TemplatesSource source, Version wizardVersion)
        {
            string currentContentFolder = CodeGen.Instance?.GetCurrentContentSource(WorkingFolder, source.Id);
            _content = new TemplatesContent(WorkingFolder, source.Id, wizardVersion, source, currentContentFolder);

            CurrentWizardVersion = wizardVersion;
        }

        public async Task EnsureContentAsync(bool force = false)
        {
            await EnsureVsInstancesSyncingAsync();

            if (LockSync())
            {
                try
                {
                    if (!_content.Exists() || force || CurrentContent.Version < CurrentWizardVersion)
                    {
                        await ExtractInstalledContentAsync();
                    }

                    TelemetryService.Current.SetContentVersionToContext(CurrentContent.Version);
                }
                finally
                {
                    UnlockSync();
                }
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

                    PurgeContentAsync().FireAndForget();

                    TelemetryService.Current.SetContentVersionToContext(CurrentContent.Version);
                }
                finally
                {
                    UnlockSync();
                }
            }
        }

        public async Task CheckForNewContentAsync()
        {
            await EnsureVsInstancesSyncingAsync();

            if (LockSync())
            {
                try
                {
                    _content.Source.LoadConfig();

                    if (_content.IsNewVersionAvailable(out var version))
                    {
                        await GetNewTemplatesAsync();
                    }

                    CheckForWizardUpdates();
                }
                finally
                {
                    UnlockSync();
                }
            }
        }

        public async Task CheckForUpdatesAsync()
        {
            await EnsureVsInstancesSyncingAsync();

            if (LockSync())
            {
                try
                {
                    SyncStatusChanged?.Invoke(this, new SyncStatusEventArgs { Status = SyncStatus.CheckingForUpdates });

                    _content.Source.LoadConfig();

                    if (_content.IsNewVersionAvailable(out var version))
                    {
                        await GetNewTemplatesAsync();
                    }
                    else
                    {
                        SyncStatusChanged?.Invoke(this, new SyncStatusEventArgs { Status = SyncStatus.CheckedForUpdates });
                    }

                    CheckForWizardUpdates();
                }
                finally
                {
                    UnlockSync();
                }
            }
        }

        private void CheckForWizardUpdates()
        {
            if (_content.IsWizardUpdateAvailable(out var version))
            {
                SyncStatusChanged?.Invoke(this, new SyncStatusEventArgs { Status = SyncStatus.NewWizardVersionAvailable, Version = version });
            }
        }

        private async Task GetNewTemplatesAsync()
        {
            try
            {
                if (_content.IsNewVersionAvailable(out var version))
                {
                    SyncStatusChanged?.Invoke(this, new SyncStatusEventArgs { Status = SyncStatus.Acquiring, Version = version });

                    await Task.Run(() =>
                    {
                        _content.GetNewVersionContent();
                    });

                    SyncStatusChanged?.Invoke(this, new SyncStatusEventArgs { Status = SyncStatus.Acquired });
                }
            }
            catch (Exception ex)
            {
                SyncStatusChanged?.Invoke(this, new SyncStatusEventArgs { Status = SyncStatus.None });
                AppHealth.Current.Error.TrackAsync(StringRes.TemplatesSynchronizationErrorAcquiring, ex).FireAndForget();
            }
        }

        private async Task ExtractInstalledContentAsync()
        {
            try
            {
                var installedPackage = _content.ResolveInstalledContent();

                SyncStatusChanged?.Invoke(this, new SyncStatusEventArgs { Status = SyncStatus.Preparing, Version = installedPackage.Version });

                await Task.Run(() =>
                {
                    _content.GetInstalledContent(installedPackage);
                });

                SyncStatusChanged?.Invoke(this, new SyncStatusEventArgs { Status = SyncStatus.Prepared });
            }
            catch (Exception ex)
            {
                SyncStatusChanged?.Invoke(this, new SyncStatusEventArgs { Status = SyncStatus.None });
                AppHealth.Current.Error.TrackAsync(StringRes.TemplatesSynchronizationErrorExtracting, ex).FireAndForget();
            }
        }

         private async Task UpdateTemplatesCacheAsync(bool force)
         {
            try
            {
                if (force || _content.RequiresContentUpdate() || CodeGen.Instance.Cache.TemplateInfo.Count == 0)
                {
                    SyncStatusChanged?.Invoke(this, new SyncStatusEventArgs { Status = SyncStatus.Updating });
                    await Task.Run(() =>
                    {
                        CodeGen.Instance.Cache.DeleteAllLocaleCacheFiles();
                        CodeGen.Instance.Cache.Scan(_content.LatestContentFolder);

                        CodeGen.Instance.Settings.SettingsLoader.Save();

                        _content.RefreshContentFolder(CodeGen.Instance.GetCurrentContentSource(WorkingFolder, _content.Source.Id));
                    });
                    SyncStatusChanged?.Invoke(this, new SyncStatusEventArgs { Status = SyncStatus.Updated });
                }
                else
                {
                    SyncStatusChanged?.Invoke(this, new SyncStatusEventArgs { Status = SyncStatus.Ready });
                }
            }
            catch (Exception ex)
            {
                SyncStatusChanged?.Invoke(this, new SyncStatusEventArgs { Status = SyncStatus.None });
                AppHealth.Current.Error.TrackAsync(StringRes.TemplatesSynchronizationErrorUpdating, ex).FireAndForget();
            }
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
                await AppHealth.Current.Warning.TrackAsync(StringRes.TemplatesSynchronizationPurgeContentAsyncMessage, ex);
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
                await AppHealth.Current.Info.TrackAsync(StringRes.TemplatesSynchronizationWaitingOtherInstanceMessage);
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
                AppHealth.Current.Warning.TrackAsync(StringRes.TemplatesSynchronizationWarnReadingLockFileMessage, ex).FireAndForget();

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

                Fs.EnsureFolder(_content.TemplatesFolder);
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
                FileInfo fileInfo = new FileInfo(Path.Combine(_content.TemplatesFolder, ".lock"));
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
