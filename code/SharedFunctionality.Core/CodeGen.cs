// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using Microsoft.TemplateEngine.Abstractions;
using Microsoft.TemplateEngine.Abstractions.Mount;
using Microsoft.TemplateEngine.Edge;
using Microsoft.TemplateEngine.Edge.Template;
using Microsoft.TemplateEngine.Utils;
using static Microsoft.TemplateEngine.Abstractions.TemplateFiltering.MatchInfo;

namespace Microsoft.Templates.Core
{
    public class CodeGen
    {
        private IMountPoint _mountPoint;

        public static CodeGen Instance { get; private set; }

        public TemplateCreator Creator { get; }

        public EngineEnvironmentSettings Settings { get; }

        public Microsoft.TemplateEngine.Edge.Settings.Scanner Scanner { get; private set; }

        public TemplateEngine.Edge.Settings.ScanResult Cache { get; set; }

        private CodeGen(string locationId, string hostVersion)
        {
            var host = CreateHost(locationId, hostVersion);
            //// Settings = new EngineEnvironmentSettings(x => new SettingsLoader(x));

            Settings = new EngineEnvironmentSettings(host, virtualizeSettings: true, environment: null);

            Creator = new TemplateCreator(Settings);
            Scanner = new TemplateEngine.Edge.Settings.Scanner(Settings);
        }

        public static void Initialize(string locationId, string hostVersion, string templatePath)
        {
            Instance = new CodeGen(locationId, hostVersion);
            Instance.Init(templatePath);
        }

        public string GetCurrentContentSource(string repositoryFolderName, string sourceId, string platform, string language)
        {
            ////var result = string.Empty;

            ////foreach (var mp in Instance?.Settings.SettingsLoader.MountPoints)
            ////{
            ////    if (mp != null
            ////        && Directory.Exists(mp.Place)
            ////        && IsHigherVersion(result, mp.Place)
            ////        && (mp.Place.IndexOf(repositoryFolderName) != -1)
            ////        && (mp.Place.IndexOf(sourceId, StringComparison.OrdinalIgnoreCase) != -1)
            ////        && (mp.Place.IndexOf(platform, StringComparison.OrdinalIgnoreCase) != -1)
            ////        && (mp.Place.IndexOf(language, StringComparison.OrdinalIgnoreCase) != -1))
            ////    {
            ////        result = mp.Place;
            ////    }
            ////}

            return GetTemplatesRootPath();
        }

        private string GetTemplatesRootPath()
        {
            // This is the path to the templates that ship inside the VSIX
            var assLoc = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
            return Path.Combine(assLoc, "Templates");
        }

        private bool IsHigherVersion(string currentPlace, string newPlace)
        {
            Version.TryParse(Path.GetFileName(currentPlace), out Version current);
            Version.TryParse(Path.GetFileName(newPlace), out Version newp);

            if (newp == null)
            {
                return false;
            }

            if (newp != null && current == null)
            {
                return true;
            }

            return newp > current;
        }

        private void Init(string templateRootPath)
        {
            Settings.TryGetMountPoint(templateRootPath, out _mountPoint);

            ////if (!Settings.SettingsLoader.Components.OfType<IGenerator>().Any())
            ////{
            ////    typeof(IGenerator).Assembly
            ////                                        .GetTypes()
            ////                                        .ToList()
            ////                                        .ForEach(t => Settings.SettingsLoader.Components.Register(t));
            ////}
        }

        private static ITemplateEngineHost CreateHost(string locationId, string hostVersion)
        {
            //return new TemplateEngine.Edge.DefaultTemplateEngineHost($"{locationId}", hostVersion, new Dictionary<string, string>());

            var builtIns = new List<(Type, IIdentifiedComponent)>();
            builtIns.AddRange(TemplateEngine.Edge.Components.AllComponents);
            builtIns.AddRange(Microsoft.TemplateEngine.Orchestrator.RunnableProjects.Components.AllComponents);
            var prefs = new Dictionary<string, string>();

            //return new TemplateStudioHost(locationId, hostVersion, null, null, Array.Empty<string>(), null);
            //return new TemplateStudioHost(locationId, hostVersion, prefs, builtIns, Array.Empty<string>(), null);

            return new TemplateEngine.Edge.DefaultTemplateEngineHost(locationId, hostVersion, prefs, builtIns);

        }
    }
}
