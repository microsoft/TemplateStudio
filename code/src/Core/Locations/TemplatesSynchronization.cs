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

using Microsoft.Templates.Core.Diagnostics;
using System.Reflection;

namespace Microsoft.Templates.Core.Locations
{
    public class TemplatesSynchronization
    {
        public event Action<object, SyncStatus> SyncStatusChanged;

        private static readonly string FolderName = Configuration.Current.RepositoryFolderName;

        private readonly Lazy<string> _workingFolder = new Lazy<string>(() => Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), FolderName));
        private readonly TemplatesSource _source;
        private readonly TemplatesContent _content;

        public string WorkingFolder => _workingFolder.Value;

        public string CurrentTemplatesFolder { get => _content?.TemplatesFolder; }
        public string CurrentContentFolder { get; private set; }
        public Version CurrentContentVersion { get => GetCurrentContentVersion(); }
        public Version CurrentWizardVersion { get; private set; }

        public TemplatesSynchronization(TemplatesSource source, Version wizardVersion)
        {
            _source = source ?? throw new ArgumentNullException("location");
            _content = new TemplatesContent(WorkingFolder, source.Id, wizardVersion);
            CurrentContentFolder = CodeGen.Instance?.GetCurrentContentSource(WorkingFolder);
        }

        public async Task Do(bool forced = false)
        {
            bool contentIsUnderVersion = _content.ExistUnderVersion();

            if (forced || contentIsUnderVersion || CurrentContentVersion.IsNullOrZero())
            {
                await CheckMandatoryAdquisitionAsync(true);
                await UpdateTemplatesCacheAsync();
                await CheckContentUnderVersion();
            }
            else
            {
                await CheckMandatoryAdquisitionAsync(forced);
                await UpdateTemplatesCacheAsync();
                await CheckExpirationAdquisitionAsync();
                await CheckContentOverVersion();
            }

            PurgeContentAsync().FireAndForget();
            TelemetryService.Current.SetContentVersionToContext(CurrentContentVersion);
        }

        private void SafeSetContentVersionInTelemetry()
        {
            
        }

        private async Task AdquireContentAsync()
        {
            SyncStatusChanged?.Invoke(this, SyncStatus.Adquiring);

            await Task.Run(() => AdquireContent());

            SyncStatusChanged?.Invoke(this, SyncStatus.Adquired);
        }

        private async Task ExtractInstalledContentAsync()
        {
            SyncStatusChanged?.Invoke(this, SyncStatus.Preparing);

            await Task.Run(() => ExtractInstalledContent());

            SyncStatusChanged?.Invoke(this, SyncStatus.Prepared);
        }

        private async Task CheckExpirationAdquisitionAsync()
        {
            if (_content.IsExpired(CurrentContentFolder))
            {
                await AdquireContentAsync();
            }
        }

        private async Task CheckMandatoryAdquisitionAsync(bool forceUpdate)
        {
            if (forceUpdate)
            {
                await AdquireContentAsync();
            }

            if (!_content.Exists())
            {
                await ExtractInstalledContentAsync();
            }
        }

        private void AdquireContent()
        {
            try
            {
                _source.Acquire(_content.TemplatesFolder);
            }
            catch (Exception ex)
            {
                throw new RepositorySynchronizationException(SyncStatus.Adquiring, ex);
            }
        }

        private void ExtractInstalledContent()
        {
            try
            {
                string installedTemplatesPath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "InstalledTemplates", "Templates.mstx");
                _source.Extract(installedTemplatesPath, _content.TemplatesFolder);

            }
            catch (Exception ex)
            {
                throw new RepositorySynchronizationException(SyncStatus.Adquiring, ex);
            }
        }

        private async Task UpdateTemplatesCacheAsync()
        {
            SyncStatusChanged?.Invoke(this, SyncStatus.Updating);

            await Task.Run(() => UpdateTemplatesCache());

            SyncStatusChanged?.Invoke(this, SyncStatus.Updated);
        }

        private async Task CheckContentOverVersion()
        {
            await Task.Run(() =>
            {
                if (_content.ExistOverVersion())
                {
                    if (CurrentContentVersion.IsNull())
                    {
                        SyncStatusChanged?.Invoke(this, SyncStatus.OverVersionNoContent);
                    }
                    else
                    {
                        SyncStatusChanged?.Invoke(this, SyncStatus.OverVersion);
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
                    SyncStatusChanged?.Invoke(this, SyncStatus.UnderVersion);
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
                await AppHealth.Current.Warning.TrackAsync("Unable to purge old content.", ex);
            }
        }

        private Version GetCurrentContentVersion()
        {
            return _content?.GetVersionFromFolder(CurrentContentFolder);
        }
    }
}
