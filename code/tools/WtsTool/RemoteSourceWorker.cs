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
using Microsoft.WindowsAzure.Storage.Blob;
using Newtonsoft.Json;
using WtsTool.CommandOptions;
using Microsoft.Templates.Core;

namespace WtsTool
{
    public static class RemoteSourceWorker
    {
        public static void ListMainVersions(RemoteSourceCommonOptions options, TextWriter output, TextWriter error)
        {
            try
            {
                output.WriteCommandHeader($"Main versions info environment {options.Env.ToString()} ({options.StorageAccount})");
                RemoteSourceVersionsInfo versionsInfo = RemoteSource.GetVersionsInfo(options.StorageAccount, options.Env.ToString().ToLowerInvariant());

                WriteSummary(versionsInfo, output);
                WritePackagesTable(versionsInfo.AvailableVersions, output);
            }
            catch (Exception ex)
            {
                error.WriteException(ex, $"Unable to list remote source info from enviorment {options.Env.ToString()} (storage account: {options.StorageAccount}).");
            }
        }

        public static void ListDetailedVersions(RemoteSourceCommonOptions options, TextWriter output, TextWriter error)
        {
            try
            {
                output.WriteCommandHeader($"Detailed versions info environment {options.Env.ToString()} ({options.StorageAccount})");
                RemoteSourceVersionsInfo versionsInfo = RemoteSource.GetVersionsInfo(options.StorageAccount, options.Env.ToString().ToLowerInvariant());

                WriteSummary(versionsInfo, output);
                WritePackagesTable(versionsInfo.Versions, output);
            }
            catch (Exception ex)
            {
                error.WriteException(ex, $"Unable to list remote source info from enviorment {options.Env.ToString()} (storage account: {options.StorageAccount}).");
            }
        }

        public static void ListSummaryInfo(RemoteSourceCommonOptions options, TextWriter output, TextWriter error)
        {
            try
            {
                output.WriteCommandHeader($"List Summary Info for environment {options.Env.ToString()} ({options.StorageAccount})");
                RemoteSourceVersionsInfo versionsInfo = RemoteSource.GetVersionsInfo(options.StorageAccount, options.Env.ToString().ToLowerInvariant());
                WriteSummary(versionsInfo, output);
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

        public static void DownloadContent(RemoteSourceDownloadOptions options, TextWriter output, TextWriter error)
        {
            try
            {
                output.WriteCommandHeader($"Downloading templates content from environment {options.Env.ToString()} ({options.StorageAccount})");

                RemoteSourceVersionsInfo versionsInfo = RemoteSource.GetVersionsInfo(options.StorageAccount, options.Env.ToString().ToLowerInvariant());
                RemotePackageInfo package = null;
                if (options.Latest)
                {
                    package = versionsInfo.LatestVersionInfo;
                }
                else
                {
                    package = ResolvePackageForVersion(versionsInfo, options.Version);
                }

                var result = RemoteSource.DownloadCdnElement(Environments.CdnUrls[options.Env], package.Name, options.Destination);

                output.WriteLine();
                output.WriteCommandText($"Successfully downloaded '{result}'");
                output.WriteLine();
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

        private static RemotePackageInfo ResolvePackageForVersion(RemoteSourceVersionsInfo versionsInfo, string version)
        {
            Version v = new Version(version);
            RemotePackageInfo match = null;
            if (v.Build == 0 && v.Revision == 0)
            {
                match = versionsInfo.AvailableVersions.Where(p => p.Version.Major == v.Major && p.Version.Minor == v.Minor)
                    .OrderByDescending(p => p.Date).FirstOrDefault();
            }
            else
            {
                match = versionsInfo.AvailableVersions.Where(p => p.Version.Major == v.Major && p.Version.Minor == v.Minor && p.Version.Build == v.Build && p.Version.Revision == v.Revision)
                    .OrderByDescending(p => p.Date).FirstOrDefault();
            }

            return match;
        }

        private static void WriteSummary(RemoteSourceVersionsInfo versionsInfo, TextWriter output)
        {
            output.WriteCommandText($"Templates Package count: {versionsInfo.PackageCount}");
            output.WriteCommandText($"Latest Version: {versionsInfo.LatestVersionInfo?.MainVersion} ({versionsInfo.LatestVersionInfo.Version?.ToString()})");
            output.WriteCommandText($"Latest Version Uri: {versionsInfo.LatestVersionInfo.Uri}");

            output.WriteLine();
            output.WriteCommandText($"Available Versions: {string.Join(", ", versionsInfo.AvailableVersions.Select(e => e.MainVersion).ToArray())}");
        }

        private static void WritePackagesTable(IEnumerable<RemotePackageInfo> packages, TextWriter output)
        {
            string c1 = nameof(RemotePackageInfo.Version);
            string c2 = nameof(RemotePackageInfo.Date);
            string c3 = nameof(RemotePackageInfo.Uri);

            output.WriteLine();
            string tableHeader = string.Format("{0,-14}   {1,-24}   {2}", c1, c2, c3);
            output.WriteCommandText(tableHeader);

            string tableSeparator = string.Format("{0,-14}   {1,-24}   {2}", new string('-', c1.Length), new string('-', c2.Length), new string('-', c3.Length));
            output.WriteCommandText(tableSeparator);

            foreach (var info in packages)
            {
                string row = string.Format("{0,-14}   {1,-24}   {2}", info.Version, info.Date, info.Uri);
                output.WriteCommandText(row);
            }
        }

        private static void PublishUpdatedRemoteSourceConfig(RemoteSourcePublishOptions options, TextWriter output)
        {
            output.WriteLine();
            output.WriteCommandText("Generating updated configuration file info (config.json)...");

            var targetFile = Path.Combine(Path.Combine(Path.GetTempPath(), "config.json"));

            Fs.SafeDeleteFile(targetFile);

            RemoteSourceVersionsInfo versionsInfo = RemoteSource.GetVersionsInfo(options.StorageAccount, options.Env.ToString().ToLowerInvariant());
            using (FileStream fs = new FileStream(targetFile, FileMode.CreateNew, FileAccess.Write))
            {
                StreamWriter sw = new StreamWriter(fs, Encoding.UTF8);
                string content = JsonConvert.SerializeObject(versionsInfo);
                sw.WriteLine(content);
                sw.Flush();
            }

            output.WriteCommandText("Updating CND configuration file (config.json)...");
            output.WriteCommandText(RemoteSource.UploadElement(options.StorageAccount, options.AccountKey, options.Env.ToString().ToLowerInvariant(), targetFile));
        }
    }
}
