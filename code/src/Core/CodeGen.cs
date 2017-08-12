﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;

using Microsoft.TemplateEngine.Abstractions;
using Microsoft.TemplateEngine.Edge.Settings;
using Microsoft.TemplateEngine.Edge.Template;
using Microsoft.TemplateEngine.Orchestrator.RunnableProjects;
using Microsoft.TemplateEngine.Utils;
using Microsoft.Templates.Core.Resources;

namespace Microsoft.Templates.Core
{
    public class CodeGen
    {
        public const string BaseName = "BaseName";

        public static CodeGen Instance { get; private set; }

        public TemplateCreator Creator { get; }
        public EngineEnvironmentSettings Settings { get; }
        public TemplateCache Cache
        {
            get
            {
                return ((SettingsLoader)Settings.SettingsLoader).UserTemplateCache;
            }
        }

        private CodeGen(string locationId, string hostVersion)
        {
            var host = CreateHost(locationId, hostVersion);
            Settings = new EngineEnvironmentSettings(host, x => new SettingsLoader(x));
            Creator = new TemplateCreator(Settings);
        }

        public static void Initialize(string locationId, string hostVersion)
        {
            Instance = new CodeGen(locationId, hostVersion);
            Instance.Init();
        }

        ////public static void Initialize(string locationId)
        ////{
        ////    Initialize(locationId, GetHostVersion());
        ////}

        public string GetCurrentContentSource(string workingFolder)
        {
            var result = string.Empty;

            foreach (var mp in Instance?.Settings.SettingsLoader.MountPoints)
            {
                if (Directory.Exists(mp.Place) && IsHigherVersion(result, mp.Place))
                {
                    result = mp.Place;
                }
            }

            return result;
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

        private static ITemplateEngineHost CreateHost(string locationId, string hostVersion)
        {
            return new DefaultTemplateEngineHost($"{BaseName}_{locationId}", hostVersion, CultureInfo.CurrentUICulture.Name, new Dictionary<string, string>());
        }

        ////private static string GetHostVersion()
        ////{
        ////    string assemblyLocation = Assembly.GetExecutingAssembly().Location;
        ////    var versionInfo = FileVersionInfo.GetVersionInfo(assemblyLocation);
        ////    Version.TryParse(versionInfo.FileVersion, out Version v);

        ////    return $"{v.Major}.{v.Minor}";
        ////}
    }
}
