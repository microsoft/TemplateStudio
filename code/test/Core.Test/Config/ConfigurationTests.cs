// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Configuration;
using System.Diagnostics;
using System.IO;

using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

using Xunit;

namespace Microsoft.Templates.Core.Test.Config
{
    [Trait("ExecutionSet", "Minimum")]

    public class ConfigurationTests
    {
        [Fact]
        public void SaveConfigurationSample()
        {
            JsonSerializer serializer = new JsonSerializer();

            serializer.Converters.Add(new JavaScriptDateTimeConverter());
            serializer.NullValueHandling = NullValueHandling.Ignore;
            serializer.Converters.Add(new StringEnumConverter());

            string filePath = Path.Combine(Environment.CurrentDirectory, "tests.save.config.json");

            StreamWriter sw = new StreamWriter(filePath);

            using (JsonWriter writer = new JsonTextWriter(sw))
            {
                serializer.Serialize(writer, new Configuration());
            }

            Assert.True(File.Exists(filePath));
        }

        [Fact]
        public void NewInstanceDefaultConfiguration()
        {
            var configInstance = new Configuration();

            Assert.NotNull(configInstance);
            Assert.Equal(TraceEventType.Verbose, configInstance.DiagnosticsTraceLevel);
            Assert.Equal("https://wtsrepository.blob.core.windows.net/dev", configInstance.CdnUrl);
            Assert.Equal("https://github.com/Microsoft/WindowsTemplateStudio/blob/master/docs/", configInstance.GitHubDocsUrl);
        }

        [Fact]
        public void InstanceFileDoesNotExists()
        {
            Exception ex = Assert.Throws<FileNotFoundException>(() =>
            {
                var configInstance = Configuration.LoadFromFile("FileNotExists.txt");
            });

            Assert.Equal("The file 'FileNotExists.txt' does not exists. Can't load the configuration.", ex.Message);
        }

        [Fact]
        public void InstanceFileExistsWrongContent()
        {
            Exception ex = Assert.Throws<ConfigurationErrorsException>(() =>
            {
                var configInstance = Configuration.LoadFromFile(@"Config\Error.config.no.valid.json");
            });

            Assert.Equal(@"Error deserializing configuration data from file 'Config\Error.config.no.valid.json'.", ex.Message);
        }

        [Fact]
        public void InstanceFileExists()
        {
            var configInstance = Configuration.LoadFromFile(@"Config\Ok.config.valid.json");

            Assert.NotNull(configInstance);

            // Especific configuration must be loaded
            Assert.Equal(TraceEventType.Information, configInstance.DiagnosticsTraceLevel);
            Assert.Equal("myCdnUrldata", configInstance.CdnUrl);
            Assert.Equal("myRemoteTelemetryKey", configInstance.RemoteTelemetryKey);
            Assert.Equal(15, configInstance.DaysToKeepDiagnosticsLogs);
        }

        [Fact]
        public void InstanceFileExistsPartialConfiguration()
        {
            var configInstance = Configuration.LoadFromFile(@"Config\Partial.config.valid.json");

            var configDefault = new Configuration();

            Assert.NotNull(configInstance);

            // Especific configuration must be loaded
            Assert.Equal(configDefault.DiagnosticsTraceLevel, configInstance.DiagnosticsTraceLevel);
            Assert.Equal(configDefault.CdnUrl, configInstance.CdnUrl);
            Assert.NotEqual(configDefault.RemoteTelemetryKey, configInstance.RemoteTelemetryKey);
            Assert.Equal("partialTelemetryKey", configInstance.RemoteTelemetryKey);
            Assert.Equal(configDefault.DaysToKeepDiagnosticsLogs, configInstance.DaysToKeepDiagnosticsLogs);
        }

        [Fact]
        public void CurrentConfigForTests()
        {
            // The App.Config must especify the "JsonConfigFile" appsetting.
            var config = Configuration.Current;

            Assert.NotNull(config);
        }
    }
}
