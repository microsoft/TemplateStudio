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
    public class TemplatesRepository
    {
        private const string ProjectTypes = "Projects";
        private const string Frameworks = "Frameworks";
        private const string Metadata = ".metadata";

        public TemplatesSynchronization Sync { get; private set; }

        public string CurrentContentFolder { get => Sync?.CurrentContentFolder; }

        public TemplatesRepository(TemplatesSource source)
        {
            Sync = new TemplatesSynchronization(source);
        }


        public string GetVersion()
        {
            return Sync.CurrentContentVersion.ToString();
        }

        public async Task SynchronizeAsync(bool force = false)
        {
            await Sync.Do(force);
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


        public MetadataInfo GetProjectTypeInfo(string projectType)
        {
            return GetMetadataInfo(Path.Combine(Sync.CurrentContentFolder, ProjectTypes, projectType, Metadata));
        }

        public MetadataInfo GetFrameworkTypeInfo(string fxType)
        {
            return GetMetadataInfo(Path.Combine(Sync.CurrentContentFolder, Frameworks, fxType, Metadata));
        }

        private MetadataInfo GetMetadataInfo(string folderName)
        {
            if (!Directory.Exists(folderName))
            {
                return null;
            }

            string descriptionFile = Path.Combine(folderName, $"description.txt");

            var metadataInfo = new MetadataInfo()
            {
                Description = File.Exists(descriptionFile) ? File.ReadAllText(descriptionFile) : String.Empty,
                Icon = Path.Combine(folderName, $"icon.png")
            };
            return metadataInfo;
        }
    }
}
