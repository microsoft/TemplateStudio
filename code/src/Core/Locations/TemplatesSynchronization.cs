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

        private readonly TemplatesSourceV2 _source;

        private readonly TemplatesContentV2 _content;

        private static object syncLock = new object();

        private readonly TimeSpan instanceSyncWait = new TimeSpan(0, 0, 5);

        private readonly TimeSpan instanceMaxSyncWait = new TimeSpan(0, 0, 30);

        public string WorkingFolder => _workingFolder.Value;

        public TemplatesContentInfo CurrentContent { get => _content?.Current; }

        public Version CurrentWizardVersion { get; private set; }

        public static bool SyncInProgress { get; private set; }

        public TemplatesSynchronization(TemplatesSourceV2 source, Version wizardVersion)
        {
            _source = source ?? throw new ArgumentNullException(nameof(source));

            string currentContentFolder = CodeGen.Instance?.GetCurrentContentSource(WorkingFolder, source.Id);
            _content = new TemplatesContentV2(WorkingFolder, source.Id, wizardVersion, source, currentContentFolder);

            CurrentWizardVersion = wizardVersion;
        }

        public async Task DoAsync(bool force)
        {
            await EnsureVsInstancesSyncingAsync();

            if (LockSync())
            {
                try
                {
                    await CheckInstallDeployedContentAsync(force);

                    await UpdateTemplatesCacheAsync(force);

                    await AcquireContentAsync();

                    ////TODO: Check if this can be removed SM
                    //await CheckContentStatusAsync();

                    PurgeContentAsync().FireAndForget();

                    TelemetryService.Current.SetContentVersionToContext(CurrentContent.Version);
                }
                finally
                {
                    UnlockSync();
                }
            }
        }

        public async Task RefreshAsync(bool force)
        {
            await UpdateTemplatesCacheAsync(force);
        }

        public async Task CheckForNewContentAsync()
        {
            if (LockSync())
            {
                try
                {
                    await AcquireContentAsync();
                    ////TODO: Check if this can be removed SM
                    //await CheckContentStatusAsync();
                }
                finally
                {
                    UnlockSync();
                }
            }
        }

        ////TODO: Check if this can be removed SM
        //private async Task CheckContentStatusAsync()
        //{
        //    await CheckNewVersionAvailableAsync();
        //}

        private async Task CheckInstallDeployedContentAsync(bool force)
        {
            if (RequireExtractInstalledContent() || force)
            {
                await ExtractInstalledContentAsync();
            }
        }

        private async Task AcquireContentAsync()
        {
            try
            {
                if (_content.IsNewVersionAvailable())
                {
                    SyncStatusChanged?.Invoke(this, new SyncStatusEventArgs { Status = SyncStatus.Acquiring });

                    await Task.Run(() =>
                    {
                        AcquireContent();
                    });
                    SyncStatusChanged?.Invoke(this, new SyncStatusEventArgs { Status = SyncStatus.Acquired });
                    SyncStatusChanged?.Invoke(this, new SyncStatusEventArgs { Status = SyncStatus.NewVersionAvailable });
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
                SyncStatusChanged?.Invoke(this, new SyncStatusEventArgs { Status = SyncStatus.Preparing });

                await Task.Run(() => ExtractInstalledContent());

                SyncStatusChanged?.Invoke(this, new SyncStatusEventArgs { Status = SyncStatus.Prepared });
            }
            catch (Exception ex)
            {
                SyncStatusChanged?.Invoke(this, new SyncStatusEventArgs { Status = SyncStatus.None });
                AppHealth.Current.Error.TrackAsync(StringRes.TemplatesSynchronizationErrorExtracting, ex).FireAndForget();
            }
        }

        private void AcquireContent()
        {
            try
            {
                var package = _source.Config.ResolvePackage(CurrentWizardVersion);
                // TODO: Is this correct here?? SM
                _source.Acquire(ref package);
                _content.GetNewVersionContent();
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
                _content.Extract(GetInstalledTemplatesPath());
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

        private async Task UpdateTemplatesCacheAsync(bool force)
        {
            try
            {
                SyncStatusChanged?.Invoke(this, new SyncStatusEventArgs { Status = SyncStatus.Updating });

                await Task.Run(() => UpdateTemplatesCache(force));

                SyncStatusChanged?.Invoke(this, new SyncStatusEventArgs { Status = SyncStatus.Updated });
            }
            catch (Exception ex)
            {
                SyncStatusChanged?.Invoke(this, new SyncStatusEventArgs { Status = SyncStatus.None });
                AppHealth.Current.Error.TrackAsync(StringRes.TemplatesSynchronizationErrorUpdating, ex).FireAndForget();
            }
        }

        //private async Task CheckNewVersionAvailableAsync()
        //{
        //    await Task.Run(() =>
        //    {
        //        if (_content.IsNewVersionAvailable())
        //        {
        //            SyncStatusChanged?.Invoke(this, new SyncStatusEventArgs { Status = SyncStatus.NewVersionAvailable });
        //        }
        //    });
        //}

        private void UpdateTemplatesCache(bool force)
        {
            try
            {
                if (force || _content.RequiresContentUpdate() || CodeGen.Instance.Cache.TemplateInfo.Count == 0)
                {
                    CodeGen.Instance.Cache.DeleteAllLocaleCacheFiles();
                    CodeGen.Instance.Cache.Scan(_content.LatestContentFolder);

                    CodeGen.Instance.Settings.SettingsLoader.Save();

                    _content.RefreshContentFolder(CodeGen.Instance.GetCurrentContentSource(WorkingFolder, _source.Id));
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
                await Task.Run(() => _content.Purge());
            }
            catch (Exception ex)
            {
                await AppHealth.Current.Warning.TrackAsync(StringRes.TemplatesSynchronizationPurgeContentAsyncMessage, ex);
            }
        }

        private bool RequireExtractInstalledContent()
        {
            return !_content.Exists();
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
