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
using System.IO;
using Newtonsoft.Json;

namespace Microsoft.Templates.Core
{
    /// <summary>
    /// This class manages settings configured through VS > Tools > Options
    /// in a way that is easy to access in the template generator
    /// </summary>
    public static class CustomSettings
    {
        public static string CustomTemplatePath
        {
            get => GetProperty(nameof(CustomTemplatePath));
            set => SetProperty(nameof(CustomTemplatePath), value);
        }

        private const string FileName = "WtsSettings.json";

        private static string GetConfigFileName()
        {
            var workingFolder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), Configuration.Current.RepositoryFolderName);
            var configFile = Path.Combine(workingFolder, FileName);

            return configFile;
        }

        private static string GetProperty(string propertyName)
        {
            var result = string.Empty;

            var configFile = GetConfigFileName();

            if (File.Exists(configFile))
            {
                var contents = File.ReadAllText(configFile);
                var configSettings = JsonConvert.DeserializeObject<Dictionary<string, string>>(contents);

                result = configSettings[propertyName] ?? string.Empty;
            }

            return result;
        }

        private static void SetProperty(string propertyName, string value)
        {
            var configFile = GetConfigFileName();

            Dictionary<string, string> configSettings = new Dictionary<string, string>();

            if (File.Exists(configFile))
            {
                var contents = File.ReadAllText(configFile);
                configSettings = JsonConvert.DeserializeObject<Dictionary<string, string>>(contents);
            }

            configSettings[propertyName] = value;

            File.WriteAllText(configFile, JsonConvert.SerializeObject(configSettings));
        }
    }
}
