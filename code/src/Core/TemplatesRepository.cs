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
        Adquired = 4
    }

    public class TemplatesRepository
    {
        public event Action<object, SyncStatus> SyncStatusChanged;

        private static readonly string FolderName = Configuration.Current.RepositoryFolderName;

        private readonly TemplatesLocation _location;

        private readonly Lazy<string> _workingFolder = new Lazy<string>(() => Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), FolderName));
        public string WorkingFolder => _workingFolder.Value;

        public string CurrentTemplatesFolder { get => _location.CurrentVersionFolder; }

        public TemplatesRepository(TemplatesLocation location)
        {
            _location = location ?? throw new ArgumentNullException("location");
            _location.Initialize(WorkingFolder);
        }

        public async Task SynchronizeAsync(bool forceUpdate = false)
        {
            _location.RefreshFolders();

            if (forceUpdate || !ExistsTemplates())
            {
                await AdquireContentAsync();
                await UpdateTemplatesCacheAsync();
            }
            else
            {
                await UpdateTemplatesCacheAsync();
                await AdquireContentAsync();
            }

            await PurgeContentAsync();
        }

        public string GetVersion()
        {
            return _location.CurrentVersion.ToString();
        }

        private async Task AdquireContentAsync()
        {
            SyncStatusChanged?.Invoke(this, SyncStatus.Adquiring);
            await Task.Run(() => AdquireContent());
            SyncStatusChanged?.Invoke(this, SyncStatus.Adquired);
        }

        private void AdquireContent()
        {
            try
            {
                _location.Adquire();
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

        private void UpdateTemplatesCache()
        {
            try
            {
                if (_location.UpdateAvailable() || CodeGen.Instance.Cache.TemplateInfo.Count == 0)
                {
                    CodeGen.Instance.Cache.DeleteAllLocaleCacheFiles();
                    CodeGen.Instance.Cache.Scan(CurrentTemplatesFolder);
                    CodeGen.Instance.Cache.WriteTemplateCaches();
                }
            }
            catch (Exception ex)
            {
                throw new RepositorySynchronizationException(SyncStatus.Updating, ex);
            }
        }
        private bool ExistsTemplates()
        {
            if (!Directory.Exists(CurrentTemplatesFolder))
            {
                return false;
            }
            else
            {
                DirectoryInfo di = new DirectoryInfo(CurrentTemplatesFolder);
                return di.EnumerateFiles("*", SearchOption.AllDirectories).Any();
            }
        }

        private async Task PurgeContentAsync()
        {
            try
            {
                await Task.Run(() => _location.Purge());
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
            return GetProyectInfo(Path.Combine(CurrentTemplatesFolder, TemplatesLocation.ProjectTypes, projectType, "Info"));
        }

        public ProjectInfo GetFrameworkTypeInfo(string fxType)
        {
            return GetProyectInfo(Path.Combine(CurrentTemplatesFolder, TemplatesLocation.Frameworks, fxType, "Info"));
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
