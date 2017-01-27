using Microsoft.TemplateEngine.Abstractions;
using Microsoft.TemplateEngine.Edge.Settings;
using Microsoft.TemplateEngine.Edge.Template;
using Microsoft.TemplateEngine.Orchestrator.RunnableProjects;
using Microsoft.TemplateEngine.Utils;
using Microsoft.Templates.Core.Locations;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Templates.Core
{
    public class TemplatesRepository
    {
        private const string FolderName = "UWPTemplates";

        private readonly TemplatesLocation _location;

        private readonly Lazy<string> _workingFolder = new Lazy<string>(() => Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), FolderName));
        public string WorkingFolder => _workingFolder.Value;

        public TemplatesRepository(TemplatesLocation location)
        {
            _location = location;
        }

        public void Sync()
        {
            if (_location != null)
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
    }
}
