using Microsoft.TemplateEngine.Abstractions;
using Microsoft.TemplateEngine.Edge.Template;
using Microsoft.Templates.Core.Locations;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Microsoft.Templates.Core
{
    public class TemplatesRepository
    {
        private const string FolderName = "UWPTemplates";

        private readonly TemplatesLocation _location;

        private readonly Lazy<string> _workingFolder = new Lazy<string>(() => Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), FolderName));
        public string WorkingFolder => _workingFolder.Value;

        private string FileVersionPath => Path.Combine(WorkingFolder, TemplatesLocation.TemplatesName, TemplatesLocation.VersionFileName);

        public TemplatesRepository(TemplatesLocation location)
        {
            _location = location;
        }

        public void Sync()
        {
            if (_location != null && IsUpdateAvailable())
            {
                _location.Copy(WorkingFolder);

                CodeGen.Instance.Cache.DeleteAllLocaleCacheFiles();
                CodeGen.Instance.Cache.Scan(WorkingFolder + $@"\{TemplatesLocation.TemplatesName}");
                CodeGen.Instance.Cache.WriteTemplateCaches();
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

        public ITemplateInfo Find(string name)
        {
            throw new NotImplementedException();
        }

        private bool IsUpdateAvailable()
        {
            if (!IsFileVersionExpired())
            {
                return false;
            }
            var repoVersion = _location.GetVersion(WorkingFolder);
            var installedVersion = GetInstalledVersion();

            return repoVersion != installedVersion;
        }

        private string GetInstalledVersion()
        {
            if (File.Exists(FileVersionPath))
            {
                return File.ReadAllText(FileVersionPath);
            }
            else
            {
                return "1.0.0";
            }
        }

        private bool IsFileVersionExpired()
        {
            if (!File.Exists(FileVersionPath))
            {
                return true;
            }

            var fileVersion = new FileInfo(FileVersionPath);
            return fileVersion.LastWriteTime.AddMinutes(Configuration.Current.VersionCheckingExpirationMinutes) <= DateTime.Now;
        }


        public ProjectInfo GetProjectTypeInfo(string projectType)
        {
            return GetProyectInfo(Path.Combine(WorkingFolder, TemplatesLocation.TemplatesName, TemplatesLocation.ProjectTypes, projectType));
        }

        public ProjectInfo GetFrameworkTypeInfo(string projectType)
        {
            return GetProyectInfo(Path.Combine(WorkingFolder, TemplatesLocation.TemplatesName, TemplatesLocation.FrameworkTypes, projectType));
        }

        private ProjectInfo GetProyectInfo(string folderName)
        {
            var projectInfo = new ProjectInfo();
            string descriptionFile = Path.Combine(folderName, $"description.txt");
            projectInfo.Description = File.Exists(descriptionFile) ? File.ReadAllText(descriptionFile) : String.Empty;
            projectInfo.Icon = Path.Combine(folderName, $"icon.png");
            return projectInfo;
        }
    }

    public class ProjectInfo
    {
        public string Icon { get; set; }
        public string Description { get; set; }
    }
}
