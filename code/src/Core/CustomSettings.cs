// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

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
