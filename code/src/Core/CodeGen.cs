using Microsoft.TemplateEngine.Abstractions;
using Microsoft.TemplateEngine.Edge.Settings;
using Microsoft.TemplateEngine.Edge.Template;
using Microsoft.TemplateEngine.Orchestrator.RunnableProjects;
using Microsoft.TemplateEngine.Utils;
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
    public class CodeGen
    {
        public const string BaseName = "UWPTemplates";

        public EngineEnvironmentSettings Settings { get; }
        public TemplateCache Cache { get; }
        public TemplateCreator Creator { get; }

        public static CodeGen Instance { get; private set; }

        private CodeGen(string locationId)
        {
            var host = CreateHost(locationId);
            Settings = new EngineEnvironmentSettings(host, x => new SettingsLoader(x));
            Cache = new TemplateCache(Settings);
            Creator = new TemplateCreator(Settings);
        }

        public static void Initialize(string locationId)
        {
            Instance = new CodeGen(locationId);
            Instance.Init();
        }

        public string GetCurrentContentSource(string workingFolder)
        {
            string result = String.Empty;

            foreach(var mp in Instance?.Settings.SettingsLoader.MountPoints)
            {
                if (Directory.Exists(mp.Place))
                {
                    result = mp.Place;
                }
            }
            return result;
        }


        private void Init()
        {
            if (!Settings.SettingsLoader.Components.OfType<IGenerator>().Any())
            {
                typeof(RunnableProjectGenerator).Assembly
                                                    .GetTypes()
                                                    .ToList()
                                                    .ForEach(t => Settings.SettingsLoader.Components.Register(t));
            }
        }

        private static ITemplateEngineHost CreateHost(string locationId)
        {
            return new DefaultTemplateEngineHost($"{BaseName}_{locationId}", GetVersion(), CultureInfo.CurrentCulture.Name, new Dictionary<string, string>());
        }

        private static string GetVersion()
        {
            string assemblyLocation = Assembly.GetExecutingAssembly().Location;
            var versionInfo = FileVersionInfo.GetVersionInfo(assemblyLocation);
            Version.TryParse(versionInfo.FileVersion, out Version v);

            return v.ToString();
        }

    }
}
