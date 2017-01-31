using Microsoft.TemplateEngine.Abstractions;
using Microsoft.TemplateEngine.Edge.Settings;
using Microsoft.TemplateEngine.Edge.Template;
using Microsoft.TemplateEngine.Orchestrator.RunnableProjects;
using Microsoft.TemplateEngine.Utils;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Templates.Core
{
    public class CodeGen
    {
        public const string Name = "UWPTemplates";

        private static readonly Lazy<CodeGen> _instance = new Lazy<CodeGen>(() => CreateNew(), true);

        public EngineEnvironmentSettings Settings { get; }
        public TemplateCache Cache { get; }
        public TemplateCreator Creator { get; }

        public static CodeGen Instance => _instance.Value;

        private CodeGen()
        {
            var host = CreateHost();
            Settings = new EngineEnvironmentSettings(host, x => new SettingsLoader(x));
            Cache = new TemplateCache(Settings);
            Creator = new TemplateCreator(Settings);
        }

        private static CodeGen CreateNew()
        {
            var instance = new CodeGen();
            instance.Initialize();

            return instance;
        }

        private void Initialize()
        {
            if (!Settings.SettingsLoader.Components.OfType<IGenerator>().Any())
            {
                typeof(RunnableProjectGenerator).Assembly
                                                    .GetTypes()
                                                    .ToList()
                                                    .ForEach(t => Settings.SettingsLoader.Components.Register(t));
            }
        }

        private static ITemplateEngineHost CreateHost()
        {
            return new DefaultTemplateEngineHost(Name, GetVersion(), CultureInfo.CurrentCulture.Name, new Dictionary<string, string>());
        }

        private static string GetVersion()
        {
            var v = Assembly.GetExecutingAssembly().GetName().Version;
            return $"{v.Major}.{v.Minor}.{v.Build}";
        }
    }
}
