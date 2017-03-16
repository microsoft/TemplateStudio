using Microsoft.Templates.Core.Diagnostics;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Templates.Core.Locations
{
    public enum SyncStatus
    {
        Undefined = 0,
        Updating = 1,
        Updated = 2,
        Adquiring = 3,
        Adquired = 4,
        OverVersion = 5,
        LowerVersion = 6
    }

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
        public Version CurrentContentVersion { get => GetCurrentVersion(); }


        public TemplatesSynchronization(TemplatesSource source)
        {
            _source = source ?? throw new ArgumentNullException("location");
            _content = new TemplatesContent(WorkingFolder, source.Id);
            CurrentContentFolder = CodeGen.Instance?.GetCurrentContentSource(WorkingFolder);
        }

        public async Task Do(bool forced = false)
        {
            if (!await ExistsLowerVersion())
            {
                await MandatoryAdquisitionAsync(forced);

                await UpdateTemplatesCacheAsync();

                await ExpirationAdquisitionAsync();

                await ExistsOverVersion();

                await PurgeContentAsync();
            }
        }

        private async Task AdquireContentAsync()
        {
            SyncStatusChanged?.Invoke(this, SyncStatus.Adquiring);
            await Task.Run(() => AdquireContent());
            SyncStatusChanged?.Invoke(this, SyncStatus.Adquired);
        }
        private async Task ExpirationAdquisitionAsync()
        {
            if (_content.IsExpired(CurrentContentFolder))
            {
                await AdquireContentAsync();
            }
        }

        private async Task MandatoryAdquisitionAsync(bool forceUpdate)
        {
            if (forceUpdate || !_content.Exists())
            {
                await AdquireContentAsync();
            }
        }

        private void AdquireContent()
        {
            try
            {
                _source.Adquire(_content.TemplatesFolder);
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

        private async Task ExistsOverVersion()
        {
            await Task.Run(() =>
            {
                if (_content.ExistOverVersion())
                {
                    SyncStatusChanged?.Invoke(this, SyncStatus.OverVersion);
                }
            });
        }

        private async Task<bool> ExistsLowerVersion()
        {
            return await Task.Run(() =>
            {
                bool result = _content.ExistLowerVersion();
                if (result)
                {
                    SyncStatusChanged?.Invoke(this, SyncStatus.LowerVersion);
                }
                return result;
            });
        }

        private void UpdateTemplatesCache()
        {
            try
            {

                if (_content.ExitsNewerVersion(CurrentContentFolder) || CodeGen.Instance.Cache.TemplateInfo.Count == 0)
                {
                    CodeGen.Instance.Cache.DeleteAllLocaleCacheFiles();
                    CodeGen.Instance.Cache.Scan(_content.LatestContentFolder);
                    CodeGen.Instance.Cache.WriteTemplateCaches();

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


        private Version GetCurrentVersion()
        {
            return _content.GetVersionFromFolder(CurrentContentFolder);
        }
    }
}
