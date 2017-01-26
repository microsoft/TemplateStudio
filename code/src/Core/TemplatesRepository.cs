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
            try
            {
                EnsureHostInitialized();

                AppDomain.CurrentDomain.AssemblyResolve += CurrentDomain_AssemblyResolve;

                if (_location != null)
                {
                    _location.Copy(WorkingFolder);

                    TemplateCache.DeleteAllLocaleCacheFiles();

                    TemplateCache.Scan(WorkingFolder + $@"\{TemplatesLocation.PackagesName}\*");
                    TemplateCache.Scan(WorkingFolder + $@"\{TemplatesLocation.TemplatesName}");

                    TemplateCache.WriteTemplateCaches();
                }
            }
            catch (Exception ex)
            {
                //TODO: LOG THIS
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

        private static void EnsureHostInitialized()
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

        //TODO: THIS IS TEMPORAL WHILE TEMPLATING TEAM RESOLVES APPDOMAIN LOAD ISSUES
        private static Assembly CurrentDomain_AssemblyResolve(object sender, ResolveEventArgs args)
        {
            try
            {
                string path = Assembly.GetExecutingAssembly().Location;
                path = Path.GetDirectoryName(path);

                if (args.Name.ToLower().Contains("templateengine") || args.Name.ToLower().Contains("newtonsoft"))
                {
                    var nameChunks = args.Name.Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                    var name = nameChunks[0].Trim();
                    path = Path.Combine(path, $"{name}.dll");
                    Assembly ret = Assembly.LoadFrom(path);
                    return ret;
                }
            }
            catch
            {
                //TODO: LOG THIS
            }

            return null;
        }
    }
}
