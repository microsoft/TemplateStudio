// ******************************************************************
// Copyright (c) Microsoft. All rights reserved.
// This code is licensed under the MIT License (MIT).
// THE CODE IS PROVIDED “AS IS”, WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED,
// INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.
// IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
// DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT,
// TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH
// THE CODE OR THE USE OR OTHER DEALINGS IN THE CODE.
// ******************************************************************

using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Templates.Core.Diagnostics;

using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Microsoft.Templates.Core
{
    public class Configuration
    {
        public string Environment { get; set; } = "Local";
        public string CdnUrl { get; set; } = "https://wtsrepository.blob.core.windows.net/dev/Latest";
        public string RemoteTelemetryKey { get; set; } = "<SET_YOUR_OWN_KEY>"; //Or configure it in a WindowsTemplateStudio.config.json located in the working folder.
        public string LogFileFolderPath { get; set; } = @"WindowsTemplateStudio\Logs";
        public string RepositoryFolderName { get; set; } = @"WindowsTemplateStudio";
        public TraceEventType DiagnosticsTraceLevel { get; set; } = TraceEventType.Verbose;
        public int DaysToKeepDiagnosticsLogs { get; set; } = 5;
        public int VersionCheckingExpirationMinutes { get; set; } = 0;
        public List<string> AllowedPublicKeysPins { get; set; } = new List<string>() { };

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

                        //TODO: Review recurrence...
                        AppHealth.Current.Verbose.TrackAsync($"Using configuration file {currentConfigFile}").FireAndForget();
                    }
                    else
                    {

                        _current = new Configuration();
                        AppHealth.Current.Verbose.TrackAsync($"Tried to use the configuration file located at {currentConfigFile}, but not found. Using default configuration.").FireAndForget();
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
        
            if (String.IsNullOrWhiteSpace(jsonConfigFile) || !File.Exists(jsonConfigFile))
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
            if (!String.IsNullOrWhiteSpace(jsonFilePath) && File.Exists(jsonFilePath))
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
                JsonSerializer serializer = new JsonSerializer();
                serializer.Converters.Add(new JavaScriptDateTimeConverter());
                serializer.NullValueHandling = NullValueHandling.Ignore;
                serializer.Converters.Add(new StringEnumConverter());
                var jsonData = File.ReadAllText(path, Encoding.UTF8);
                return JsonConvert.DeserializeObject<Configuration>(jsonData);
            }
            catch(Exception ex)
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
