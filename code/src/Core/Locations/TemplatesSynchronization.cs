// ******************************************************************
// Copyright (c) Microsoft. All rights reserved.
// This code is licensed under the MIT License (MIT).
// THE CODE IS PROVIDED “AS IS”, WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED,
// INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.
// IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
// DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT,
// TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH
// THE CODE OR THE USE OR OTHER DEALINGS IN THE CODE.
// ******************************************************************

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

        public string WorkingFolder => _workingFolder.Value;

        public string CurrentTemplatesFolder { get => _content?.TemplatesFolder; }
        public string CurrentContentFolder { get; private set; }
        public Version CurrentContentVersion { get => GetCurrentContentVersion(); }
        public Version CurrentWizardVersion { get; private set; }

        private static object syncLock = new object();
        public static bool SyncInProgress { get; private set; }

        public TemplatesSynchronization(TemplatesSource source, Version wizardVersion)
        {
            _source = source ?? throw new ArgumentNullException("location");
            _content = new TemplatesContent(WorkingFolder, source.Id, wizardVersion);
            CurrentContentFolder = CodeGen.Instance?.GetCurrentContentSource(WorkingFolder);
        }

        public async Task Do()
        {
            if (LockSync())
            {
                try
                {
                    await CheckInstallDeployedContent();

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
            await CheckContentUnderVersion();
            await CheckNewVersionAvailableAsync();
            await CheckContentOverVersion();
        }

        private async Task CheckInstallDeployedContent()
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
                _source.Extract(installedTemplatesPath, _content.TemplatesFolder);
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

        private async Task CheckContentOverVersion()
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

        private async Task CheckContentUnderVersion()
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

                    CurrentContentFolder = CodeGen.Instance.GetCurrentContentSource(WorkingFolder);
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
                SyncInProgress = true;
                return true;
            }
        }

        private void UnlockSync()
        {
            lock (syncLock)
            {
                SyncInProgress = false;
            }
        }
    }
}
