// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Text;

using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Microsoft.Templates.Core
{
    public class Configuration
    {
        public string Environment { get; set; } = "LocalEnv";

        public string CdnUrl { get; set; } = "https://wtsrepository.blob.core.windows.net/dev";

        // Set your Application Insights telemetry instrumentation key here (configure it in a WindowsTemplateStudio.config.json located in the working folder).
        public string RemoteTelemetryKey { get; set; } = "<SET_YOUR_OWN_KEY>";

        public string LogFileFolderPath { get; set; } = @"WindowsTemplateStudio\Logs";

        public string RepositoryFolderName { get; set; } = @"WindowsTemplateStudio";

        public string BackupFolderPath { get; set; } = @"WindowsTemplateStudio\Backups";

        public string TempGenerationFolderPath { get; set; } = "WTSTempGeneration";

        public TraceEventType DiagnosticsTraceLevel { get; set; } = TraceEventType.Verbose;

        public int DaysToKeepTempGenerations { get; set; } = 5;

        public int DaysToKeepDiagnosticsLogs { get; set; } = 5;

        public List<string> AllowedPublicKeysPins { get; set; } = new List<string>() { };

        public string CustomTelemetryEndpoint { get; set; } = string.Empty;

        public string GitHubDocsUrl { get; set; } = "https://github.com/Microsoft/WindowsTemplateStudio/blob/master/docs/";

        public static string LoadedConfigFile { get; private set; }

        public const string DefaultJsonConfigFileName = "WindowsTemplateStudio.config.json";

        private static Configuration _current;

        public static Configuration Current
        {
            get
            {
                if (_current == null)
                {
                    string currentConfigFile = GetJsonConfigFilePath();

                    if (File.Exists(currentConfigFile))
                    {
                        _current = LoadFromFile(currentConfigFile);
                        LoadedConfigFile = currentConfigFile;

                        TraceUsingDefault($"Using configuration file {currentConfigFile}");
                    }
                    else
                    {
                        _current = new Configuration();
                        LoadedConfigFile = "None";

                        TraceUsingDefault($"Tried to use the configuration file located at {currentConfigFile}, but not found. Using default configuration.");
                    }
                }

                return _current;
            }
        }

        public static string GetJsonConfigFilePath()
        {
            TraceUsingDefault("Resoving JsonConfigFilePath");
            TraceUsingDefault("1. Check 'JsonConfigFile' appSetting is defined");

            string jsonConfigFile = ConfigurationManager.AppSettings["JsonConfigFile"];

            if (string.IsNullOrWhiteSpace(jsonConfigFile) || !File.Exists(jsonConfigFile))
            {
                jsonConfigFile = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), DefaultJsonConfigFileName);

                TraceUsingDefault($"2. AppSetting config file is not defined. Returning default config file in executing directory: {jsonConfigFile}");
            }
            else
            {
                TraceUsingDefault($"2. AppSetting config file is defined. Returning configured file: {jsonConfigFile}");
            }

            return jsonConfigFile;
        }

        public static Configuration LoadFromFile(string jsonFilePath)
        {
            Configuration loadedConfig = null;

            if (!string.IsNullOrWhiteSpace(jsonFilePath) && File.Exists(jsonFilePath))
            {
                loadedConfig = DeserializeConfiguration(jsonFilePath);
            }
            else
            {
                throw new FileNotFoundException($"The file '{jsonFilePath}' does not exists. Can't load the configuration.");
            }

            return loadedConfig;
        }

        public Configuration()
        {
        }

        private static Configuration DeserializeConfiguration(string path)
        {
            try
            {
                var serializer = new JsonSerializer();
                serializer.Converters.Add(new JavaScriptDateTimeConverter());
                serializer.NullValueHandling = NullValueHandling.Ignore;
                serializer.Converters.Add(new StringEnumConverter());
                var jsonData = File.ReadAllText(path, Encoding.UTF8);
                return JsonConvert.DeserializeObject<Configuration>(jsonData);
            }
            catch (Exception ex)
            {
                TraceUsingDefault($"Error deserializing configuration from file '{path}'. Exception:\n\r{ex.ToString()}");
                throw new ConfigurationErrorsException($"Error deserializing configuration data from file '{path}'.", ex);
            }
        }

        private static void TraceUsingDefault(string info)
        {
            Debug.Write(info);
            Trace.TraceWarning(info);
        }
    }
}
