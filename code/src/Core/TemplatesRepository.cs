using Microsoft.TemplateEngine.Abstractions;
using Microsoft.TemplateEngine.Edge.Template;
using Microsoft.Templates.Core.Diagnostics;
using Microsoft.Templates.Core.Extensions;
using Microsoft.Templates.Core.Locations;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Microsoft.Templates.Core
{
    public enum SyncStatus
    {
        Undefined = 0,
        Updating = 1,
        Updated = 2,
        Adquiring = 3,
        Adquired = 4,
        OverVersion = 5
    }

    public class TemplatesRepository
    {
        private const string ProjectTypes = "Projects";
        private const string Frameworks = "Frameworks";

        public event Action<object, SyncStatus> SyncStatusChanged;

        private static readonly string FolderName = Configuration.Current.RepositoryFolderName;

        private readonly Lazy<string> _workingFolder = new Lazy<string>(() => Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), FolderName));
        public string WorkingFolder => _workingFolder.Value;
        public string CurrentContentFolder { get; private set; }

        private readonly TemplatesSource _source;
        private readonly TemplatesContent _content;

        public TemplatesRepository(TemplatesSource source)
        {
            _source = source ?? throw new ArgumentNullException("location");
            _content = new TemplatesContent(WorkingFolder, source.Id);

            CurrentContentFolder = CodeGen.Instance?.GetCurrentContentSource(WorkingFolder);
        }

        public async Task SynchronizeAsync(bool forceUpdate = false)
        {
            await ForcedAdquisition(forceUpdate);

            await UpdateTemplatesCacheAsync();

            await ExpirationAdquisition();

            await ExistsOverVersion();

            await PurgeContentAsync();

        }

        public string GetVersion()
        {
            return _content.GetVersionFromFolder(CurrentContentFolder).ToString();
        }

        private async Task AdquireContentAsync()
        {
            SyncStatusChanged?.Invoke(this, SyncStatus.Adquiring);
            await Task.Run(() => AdquireContent());
            SyncStatusChanged?.Invoke(this, SyncStatus.Adquired);
        }
        private async Task ExpirationAdquisition()
        {
            if (_content.IsExpired(CurrentContentFolder))
            {
                await AdquireContentAsync();
            }
        }

        private async Task ForcedAdquisition(bool forceUpdate)
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

        public IEnumerable<ITemplateInfo> GetAll()
        {
            var queryResult = CodeGen.Instance.Creator.List(false, WellKnownSearchFilters.LanguageFilter("C#"));

            return queryResult
                        .Where(r => r.IsMatch)
                        .Select(r => r.Info)
                        .ToList();
        }

        public IEnumerable<ITemplateInfo> Get(Func<ITemplateInfo, bool> predicate)
        {
            return GetAll()
                        .Where(predicate);
        }

        public IEnumerable<ITemplateInfo> GetDependencies(ITemplateInfo ti)
        {
            return ti.GetDependencyList().Select(d => GetAll().FirstOrDefault(t => t.Identity == d));
        }


        public ITemplateInfo Find(Func<ITemplateInfo, bool> predicate)
        {
            return GetAll()
                        .FirstOrDefault(predicate);
        }


        public ProjectInfo GetProjectTypeInfo(string projectType)
        {
            return GetProyectInfo(Path.Combine(CurrentContentFolder, ProjectTypes, projectType, "Info"));
        }

        public ProjectInfo GetFrameworkTypeInfo(string fxType)
        {
            return GetProyectInfo(Path.Combine(CurrentContentFolder, Frameworks, fxType, "Info"));
        }

        private ProjectInfo GetProyectInfo(string folderName)
        {
            if (!Directory.Exists(folderName))
            {
                return null;
            }
            var projectInfo = new ProjectInfo();

            string descriptionFile = Path.Combine(folderName, $"description.txt");
            projectInfo.Description = File.Exists(descriptionFile) ? File.ReadAllText(descriptionFile) : String.Empty;
            projectInfo.Icon = Path.Combine(folderName, $"icon.png");
            return projectInfo;
        }
    }
    public class ProjectInfo
    {
        public string Name { get; set; }
        public string Icon { get; set; }
        public string Description { get; set; }
    }
}
