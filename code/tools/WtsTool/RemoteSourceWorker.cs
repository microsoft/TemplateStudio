// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.Templates.Core;
using Microsoft.Templates.Core.Locations;
using Microsoft.WindowsAzure.Storage.Blob;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using WtsTool.CommandOptions;

namespace WtsTool
{
    public static class RemoteSourceWorker
    {
        public static void ListSummaryInfo(RemoteSourceListOptions options, TextWriter output, TextWriter error)
        {
            try
            {
                output.WriteCommandHeader($"Summary Info for environment {options.Env.ToString()} ({options.StorageAccount})");
                TemplatesSourceConfig config = GetConfig(options, output);
                output.WriteLine();

                WriteSummary(config, output);
            }
            catch (Exception ex)
            {
                error.WriteException(ex, $"Unable to list remote source info from enviorment {options.Env.ToString()} (storage account: {options.StorageAccount}).");
            }
        }

        public static void ListDetailedVersions(RemoteSourceListOptions options, TextWriter output, TextWriter error)
        {
            try
            {
                output.WriteCommandHeader($"Versions for environment {options.Env.ToString()} ({options.StorageAccount})");

                TemplatesSourceConfig config = GetConfig(options, output);
                output.WriteLine();

                WriteSummary(config, output);
                WritePackagesTable(config.Versions, output);
            }
            catch (Exception ex)
            {
                error.WriteException(ex, $"Unable to list remote source info from enviorment {options.Env.ToString()} (storage account: {options.StorageAccount}).");
            }
        }

        public static void PublishContent(RemoteSourcePublishOptions options, TextWriter output, TextWriter error)
        {
            try
            {
                output.WriteCommandHeader($"Publishing {options.File} for environment {options.Env.ToString()} ({options.StorageAccount})");
                output.WriteCommandText("Uploading template package...");
                output.WriteCommandText(RemoteSource.UploadTemplatesContent(options.StorageAccount, options.AccountKey, options.Env.ToString().ToLowerInvariant(), options.File, options.Version));

                PublishUpdatedRemoteSourceConfig(options, output);
            }
            catch (Exception ex)
            {
                error.WriteException(ex, $"Unable to publish the file {options.File} content to the specified environment container.");
            }
        }

        public static void PublishConfig(RemoteSourcePublishOptions options, TextWriter output, TextWriter error)
        {
            try
            {
                output.WriteCommandHeader($"Publishing the config.json file for environment {options.Env.ToString()} ({options.StorageAccount})");
                PublishUpdatedRemoteSourceConfig(options, output);
            }
            catch (Exception ex)
            {
                error.WriteException(ex, $"Unable to publish an updated configuration file to specified environment container.");
            }
        }

        public static void DownloadContent(RemoteSourceDownloadOptions options, TextWriter output, TextWriter error)
        {
            try
            {
                output.WriteCommandHeader($"Downloading templates content from environment {options.Env.ToString()} ({options.StorageAccount})");

                TemplatesSourceConfig config = GetConfigFromCdn(options.Env);

                TemplatesPackageInfo package = null;
                if (options.Version != null)
                {
                    package = ResolvePackageForVersion(config, options.Version, output);
                }
                else
                {
                    package = config.Latest;
                }

                if (package != null)
                {
                    Fs.EnsureFolder(options.Destination);

                    var result = RemoteSource.DownloadCdnElement(Environments.CdnUrls[options.Env], package.Name, options.Destination);
                    output.WriteLine();
                    output.WriteCommandText($"Successfully downloaded '{result}'");
                    output.WriteLine();
                }
                else
                {
                    output.WriteLine();
                    output.WriteCommandText($"Package not found for the version '{options.Version}'");
                    output.WriteLine();
                }
            }
            catch (Exception ex)
            {
                error.WriteException(ex, $"Unable to download the file content from the specified environment.");
            }
        }

        public static void DownloadConfig(RemoteSourceDownloadOptions options, TextWriter output, TextWriter error)
        {
            try
            {
                output.WriteCommandHeader($"Downloading template source config file from environment {options.Env.ToString()} ({options.StorageAccount})");
                output.WriteLine();

                var result = RemoteSource.DownloadCdnElement(Environments.CdnUrls[options.Env], "config.json", options.Destination);

                output.WriteCommandText($"Successfully downloaded '{result}'");
                output.WriteLine();
            }
            catch (Exception ex)
            {
                error.WriteException(ex, $"Unable to download the config file from the specified environment.");
            }
        }

        private static TemplatesSourceConfig GetConfig(RemoteSourceListOptions options, TextWriter output)
        {
            if (options.Build)
            {
                output.WriteCommandText("Building Remote Templates Source Configuration information...");
                return RemoteSource.GetTemplatesSourceConfig(options.StorageAccount, options.Env);
            }
            else
            {
                output.WriteCommandText("Getting config file from the CDN (config.json)...");
                return GetConfigFromCdn(options.Env);
            }
        }

        private static TemplatesSourceConfig GetConfigFromCdn(EnvEnum env)
        {
            Fs.SafeDeleteFile(Path.Combine(Path.GetTempPath(), "config.json"));
            string configFile = RemoteSource.DownloadCdnElement(Environments.CdnUrls[env], "config.json", Path.GetTempPath());

            TemplatesSourceConfig config = TemplatesSourceConfig.LoadFromFile(configFile);
            return config;
        }

        private static TemplatesPackageInfo ResolvePackageForVersion(TemplatesSourceConfig config, string version, TextWriter output)
        {
            Version v = new Version(version);
            if (v.Build != 0 || v.Revision != 0)
            {
                output.WriteCommandText($"WARN: Downloading main version for {v.Major}.{v.Minor}, ignoring the version parts build and revision ({v.Build}.{v.Revision}).");
            }

            TemplatesPackageInfo match = config.ResolvePackage(v);

            return match;
        }

        private static void WriteSummary(TemplatesSourceConfig config, TextWriter output)
        {
            output.WriteCommandText($"Latest Version: {config.Latest?.MainVersion} ({config.Latest.Version?.ToString()})");
            output.WriteCommandText($"Latest Version Uri: {config.RootUri + config.Latest.Name}");

            output.WriteLine();
            output.WriteCommandText($"Available Versions: {string.Join(", ", config.Versions.Select(e => e.MainVersion).ToArray())}");
        }

        private static void WritePackagesTable(IEnumerable<TemplatesPackageInfo> packages, TextWriter output)
        {
            string c1 = nameof(TemplatesPackageInfo.Version);
            string c2 = nameof(TemplatesPackageInfo.Date);
            string c3 = nameof(TemplatesPackageInfo.Name);

            output.WriteLine();
            string tableHeader = string.Format("{0,-14}   {1,-24}   {2}", c1, c2, c3);
            output.WriteCommandText(tableHeader);

            string tableSeparator = string.Format("{0,-14}   {1,-24}   {2}", new string('-', c1.Length), new string('-', c2.Length), new string('-', c3.Length));
            output.WriteCommandText(tableSeparator);

            foreach (var info in packages)
            {
                string row = string.Format("{0,-14}   {1,-24}   {2}", info.Version, info.Date, info.Name);
                output.WriteCommandText(row);
            }
        }

        private static void PublishUpdatedRemoteSourceConfig(RemoteSourcePublishOptions options, TextWriter output)
        {
            output.WriteLine();
            output.WriteCommandText("Generating updated configuration file info (config.json)...");

            var targetFile = Path.Combine(Path.Combine(Path.GetTempPath(), "config.json"));

            Fs.SafeDeleteFile(targetFile);

            TemplatesSourceConfig config = RemoteSource.GetTemplatesSourceConfig(options.StorageAccount, options.Env);
            using (FileStream fs = new FileStream(targetFile, FileMode.CreateNew, FileAccess.Write))
            {
                StreamWriter sw = new StreamWriter(fs, Encoding.UTF8);

                JsonSerializerSettings settings = new JsonSerializerSettings();
                settings.NullValueHandling = NullValueHandling.Ignore;
                settings.Converters.Add(new StringEnumConverter());
                string content = JsonConvert.SerializeObject(config, settings);

                sw.WriteLine(content);
                sw.Flush();
            }

            output.WriteCommandText("Updating CND configuration file (config.json)...");
            output.WriteCommandText(RemoteSource.UploadElement(options.StorageAccount, options.AccountKey, options.Env.ToString().ToLowerInvariant(), targetFile));
        }
    }
}
