// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.WindowsAzure.Storage.Blob;
using WtsTool.CommandOptions;

namespace WtsTool
{
    public static class RemoteSource
    {
        public static RemoteSourceVersionsInfo GetVersionsInfo(string storageAccount, string env)
        {
            CloudBlobContainer container = RemoteSource.GetContainerAnonymous(storageAccount, env);
            var remoteElements = RemoteSource.GetAllElements(container);
            var remotePackageInfoItems = remoteElements.Where(e => e != null && e.Name.StartsWith(env, StringComparison.OrdinalIgnoreCase) && e.Name.EndsWith(".mstx", StringComparison.OrdinalIgnoreCase)).Select((e) =>
                new RemotePackageInfo()
                {
                    Name = e.Name,
                    Uri = e.Uri,
                    Date = e.Properties.LastModified.Value.DateTime,
                    Version = ParseVersion(e.Name),
                    Env = ParseEnv(e.Name)
                }).OrderByDescending(info => info.Date);

            RemoteSourceVersionsInfo summary = new RemoteSourceVersionsInfo()
            {
                PackageCount = remotePackageInfoItems.Count(),
                LatestVersionInfo = remotePackageInfoItems.GetLatestVersion(),
                AvailableVersions = remotePackageInfoItems.GetAvailableVersions(),
                Versions = remotePackageInfoItems
            };
            return summary;
        }

        public static string PublishContent(string storageAccount, string key, string env, string sourceFile, string version)
        {
            if (!File.Exists(sourceFile))
            {
                throw new ArgumentException($"Invalid parameter '{nameof(sourceFile)}' value. The file '{sourceFile}' does not exists.");
            }

            if (string.IsNullOrWhiteSpace(sourceFile))
            {
                throw new ArgumentException($"Parameter '{nameof(sourceFile)}' can not be null, empty or whitespace.");
            }

            Version specifedVersion;
            if (!Version.TryParse(version, out specifedVersion))
            {
                throw new ArgumentException($"The value '{version}' is not valid for parameter '{nameof(version)}'.");
            }

            Version versionInFile = ParseVersion(Path.GetFileName(sourceFile));
            if (versionInFile != null && versionInFile != specifedVersion)
            {
                throw new ArgumentException($"Parameter '{nameof(sourceFile)}' (with value '{sourceFile}') contains the version {versionInFile.ToString()} that do not match with the value specified in the parameter '{nameof(version)}' (with value '{version}').");
            }

            string blobName = (versionInFile == null) ? $"{Path.GetFileNameWithoutExtension(sourceFile)}_{version}.mstx" : sourceFile;
            var container = GetContainer(storageAccount, key, env);
            return UploadElement(container, sourceFile, blobName);
        }

        public static void DownloadContent(RemoteSourceVersionsInfo versionsInfo, string downloadDirectory)
        {
            if (versionsInfo.LatestVersionInfo != null)
            {
                DownloadContent(versionsInfo.LatestVersionInfo, downloadDirectory);
            }
            else
            {
                throw new ArgumentException($"The parameter '{nameof(versionsInfo)}' provided does not have a vaild '{nameof(versionsInfo.LatestVersionInfo)}' (is null).");
            }
        }

        public static void DownloadContent(RemoteSourceVersionsInfo versionsInfo, int major, int minor, string downloadDirectory)
        {
            var math = versionsInfo.AvailableVersions.Where(p => p.Version.Major == major && p.Version.Minor == minor)
                .OrderByDescending(p => p.Date).FirstOrDefault();
            if (math != null)
            {
                DownloadElement(math, downloadDirectory);
            }
            else
            {
                throw new ArgumentException($"The parameter '{nameof(versionsInfo)}' provided does not contains a templates content package matching the version '{major.ToString()}.{minor.ToString()}'.");
            }
        }

        public static void DownloadContent(RemoteSourceVersionsInfo versionsInfo, Version v, string downloadDirectory)
        {
            var math = versionsInfo.AvailableVersions.Where(p => p.Version.Major == v.Major && p.Version.Minor == v.Minor && p.Version.Build == v.Build && p.Version.Revision == v.Revision)
                .OrderByDescending(p => p.Date).FirstOrDefault();

            if (math != null)
            {
                DownloadElement(math, downloadDirectory);
            }
            else
            {
                throw new ArgumentException($"The parameter '{nameof(versionsInfo)}' provided does not contains a templates content package matching the version '{v.ToString()}'.");
            }
        }

        public static void DownloadContent(RemotePackageInfo packageInfo, string downloadDirectory)
        {
            if (packageInfo == null)
            {
                throw new ArgumentNullException(nameof(packageInfo));
            }

            DownloadElement(packageInfo, downloadDirectory);
        }

        private static IEnumerable<RemotePackageInfo> GetAvailableVersions(this IEnumerable<RemotePackageInfo> remotePackageInfoItems)
        {
            return remotePackageInfoItems.GroupBy(e => e.MainVersion).Select(e => e.FirstOrDefault()).OrderByDescending(e => e.Version);
        }

        private static RemotePackageInfo GetLatestVersion(this IEnumerable<RemotePackageInfo> remotePackageInfoItems)
        {
            return remotePackageInfoItems?.OrderByDescending(e => e.Date).ThenByDescending(e => e.Version).FirstOrDefault();
        }

        private static EnvEnum ParseEnv(string name)
        {
            EnvEnum parsedEnv = EnvEnum.Unknown;
            string pattern = @"^pro|pre|dev|test";
            Regex regex = new Regex(pattern, RegexOptions.Compiled & RegexOptions.IgnoreCase & RegexOptions.CultureInvariant);

            var match = regex.Match(name);
            if (match.Success)
            {
                Enum.TryParse(match.Value, true, out parsedEnv);
            }

            return parsedEnv;
        }

        private static Version ParseVersion(string name)
        {
            string versionPattern = @"\d+.\d+.\d+.\d+";
            Regex versionRegEx = new Regex(versionPattern, RegexOptions.Compiled & RegexOptions.IgnoreCase & RegexOptions.CultureInvariant);
            var match = versionRegEx.Match(name);
            if (match.Success)
            {
                return new Version(match.Value);
            }
            else
            {
                return null;
            }
        }

        private static CloudBlobContainer GetContainer(string account, string key, string container)
        {
            StorageCredentials credentials = new StorageCredentials(account, key);

            CloudStorageAccount storageAccount = new CloudStorageAccount(credentials, true);

            CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();

            CloudBlobContainer blobContainer = blobClient.GetContainerReference(container);

            return blobContainer;
        }

        private static CloudBlobContainer GetContainerAnonymous(string account, string container)
        {
            CloudBlobClient blobClient = new CloudBlobClient(new Uri($"https://{account}.blob.core.windows.net"));

            CloudBlobContainer blobContainer = blobClient.GetContainerReference(container);

            return blobContainer;
        }

        private static IEnumerable<CloudBlockBlob> GetAllElements(CloudBlobContainer container)
        {
            BlobContinuationToken token = new BlobContinuationToken();

            List<CloudBlockBlob> result = new List<CloudBlockBlob>();

            while (token != null)
            {
                result.AddRange(GetElements(container, ref token));
            }

            return result;
        }

        private static IEnumerable<CloudBlockBlob> GetElements(CloudBlobContainer container, ref BlobContinuationToken token)
        {
            if (!container.Exists())
            {
                throw new ArgumentException($"The container {container.Uri} does not exists or is not public.");
            }

            BlobResultSegment result = container.ListBlobsSegmented(token);

            token = result.ContinuationToken;
            return result.Results.Select((i) => i as CloudBlockBlob);
        }

        private static string UploadElement(CloudBlobContainer container, string sourceFile, string blobName)
        {
            CloudBlockBlob blob = container.GetBlockBlobReference(blobName);
            using (var fileStreame = File.Open(sourceFile, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                BlobRequestOptions options = new BlobRequestOptions();
                options.ParallelOperationThreadCount = 4;

                OperationContext context = new OperationContext();

                blob.UploadFromStream(fileStreame, null, options, context);

                float bytes = 0;
                foreach (var result in context.RequestResults)
                {
                    bytes += result.EgressBytes;
                }

                TimeSpan elapsed = context.EndTime - context.StartTime;

                return $"Uploaded {Math.Round(bytes / 1024f, 2)} Kbytes in {elapsed.TotalSeconds} seconds.";
            }
        }

        private static void DownloadElement(RemotePackageInfo packageInfo, string downloadDirectory)
        {
            Uri elementUri = new Uri($"{Environments.CdnUrls[packageInfo.Env]}/{packageInfo.Name}");

            var wc = new WebClient();
            wc.DownloadFile(elementUri, Path.Combine(downloadDirectory, packageInfo.Name));
        }

        public static void DownloadConfigCdn(EnvEnum env)
        {
            Uri elementUri = new Uri($"{Environments.CdnUrls[env]}/config.txt");

            Console.Out.Write($"Downloading {elementUri.ToString()}");
            var wc = new WebClient();
            wc.DownloadFile(elementUri, Path.Combine(@"C:\temp", "config.txt"));
        }

        public static void DownloadConfigStorage(EnvEnum env)
        {
            Uri elementUri = new Uri($"{Environments.StorageUrls[env]}/config.txt");

            Console.Out.Write($"Downloading {elementUri.ToString()}");
            var wc = new WebClient();
            wc.DownloadFile(elementUri, Path.Combine(@"C:\temp", "config.txt"));
        }
    }
}
