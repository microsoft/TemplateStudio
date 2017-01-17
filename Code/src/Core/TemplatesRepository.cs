using Microsoft.TemplateEngine.Abstractions;
using Microsoft.TemplateEngine.Edge.Settings;
using Microsoft.TemplateEngine.Edge.Template;
using Microsoft.TemplateEngine.Utils;
using Microsoft.Templates.Core.Locations;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Templates.Core
{
    public class TemplatesRepository
    {
        private const string FolderName = "UWPTemplates";

        TemplatesLocation _location;

        private Lazy<string> _workingFolder = new Lazy<string>(() =>
        {
            return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), FolderName);
        });
        public string WorkingFolder
        {
            get
            {
                return _workingFolder.Value;
            }
        }

        public TemplatesRepository(TemplatesLocation location)
        {
            _location = location;
        }

        public void Sync()
        {
            EnsureHostInitialized();

            if (_location != null)
            {
                _location.Copy(WorkingFolder);

                TemplateCache.Scan(WorkingFolder + $@"\{TemplatesLocation.PackagesName}\*");
                TemplateCache.Scan(WorkingFolder + $@"\{TemplatesLocation.TemplatesName}");

                TemplateCache.WriteTemplateCaches();
            }
        }

        public IEnumerable<ITemplateInfo> GetAll()
        {
            EnsureHostInitialized();

            var queryResult = TemplateCreator.List(false, WellKnownSearchFilters.LanguageFilter("C#"));

            return queryResult
                        .Where(r => r.IsMatch)
                        .Select(r => r.Info)
                        .ToList();
        }

        public ITemplateInfo Find(string name)
        {
            throw new NotImplementedException();
        }

        private void EnsureHostInitialized()
        {
            if (EngineEnvironmentSettings.Host == null)
            {
                EngineEnvironmentSettings.Host = CreateHost();
            }
        }

        private static ITemplateEngineHost CreateHost()
        {
            //TODO: REVIEW THIS
            return new DefaultTemplateEngineHost(FolderName, "1.0.0", CultureInfo.CurrentCulture.Name, new Dictionary<string, string>());
        }
    }
}
