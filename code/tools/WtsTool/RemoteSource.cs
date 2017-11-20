// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.WindowsAzure.Storage.Blob;
using WtsTool.CommandOptions;
using System.IO;

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

            Version versionInFile = ParseVersion(sourceFile);
            if (versionInFile != null && versionInFile != specifedVersion)
            {
                throw new ArgumentException($"Parameter '{nameof(sourceFile)}' (with value '{sourceFile}') contains a version that do not match with the value specified in the parameter '{nameof(version)}' (with value '{version}').");
            }

            string blobName = (versionInFile == null) ? $"{Path.GetFileNameWithoutExtension(sourceFile)}_{version}.mstx" : sourceFile;
            var container = GetContainer(storageAccount, key, env);
            return UploadElement(container, sourceFile, blobName);
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

                return $"Uploaded {(bytes / 1024f) / 1024f} mbytes in {elapsed.TotalSeconds} seconds.";
            }
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
    }
}
