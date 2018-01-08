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
using Microsoft.Templates.Core.Locations;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.WindowsAzure.Storage.Blob;
using WtsTool.CommandOptions;

namespace WtsTool
{
    public static class RemoteSource
    {
        public static TemplatesSourceConfig GetTemplatesSourceConfig(string storageAccount, EnvEnum environment)
        {
            string env = environment.ToString().ToLowerInvariant();

            CloudBlobContainer container = GetContainerAnonymous(storageAccount, env);
            var remoteElements = RemoteSource.GetAllElements(container);
            var remotePackages = remoteElements.Where(e => e != null && e.Name.StartsWith(env, StringComparison.OrdinalIgnoreCase) && e.Name.EndsWith(".mstx", StringComparison.OrdinalIgnoreCase))
                .Select((e) =>
                    new TemplatesPackageInfo()
                    {
                        Name = e.Name,
                        Bytes = e.Properties.Length,
                        Date = e.Properties.LastModified.Value.DateTime
                    })
                .OrderByDescending(e => e.Date)
                .OrderByDescending(e => e.Version)
                .GroupBy(e => e.MainVersion)
                .Select(e => e.FirstOrDefault());

            TemplatesSourceConfig config = new TemplatesSourceConfig()
            {
                Latest = remotePackages.FirstOrDefault(),
                Versions = remotePackages.ToList(),
                RootUri = container.Uri
            };
            return config;
        }

        public static string UploadTemplatesContent(string storageAccount, string key, string env, string sourceFile, string version)
        {
            if (!File.Exists(sourceFile))
            {
                throw new ArgumentException($"Invalid parameter '{nameof(sourceFile)}' value. The file '{sourceFile}' does not exists.");
            }

            if (string.IsNullOrWhiteSpace(sourceFile))
            {
                throw new ArgumentException($"Parameter '{nameof(sourceFile)}' can not be null, empty or whitespace.");
            }

            string blobName = GetBlobName(env, sourceFile, version);

            var container = GetContainer(storageAccount, key, env);
            return UploadElement(container, sourceFile, blobName);
        }

        public static string UploadElement(string storageAccount, string key, string env, string sourceFile)
        {
            if (!File.Exists(sourceFile))
            {
                throw new ArgumentException($"Invalid parameter '{nameof(sourceFile)}' value. The file '{sourceFile}' does not exists.");
            }

            if (string.IsNullOrWhiteSpace(sourceFile))
            {
                throw new ArgumentException($"Parameter '{nameof(sourceFile)}' can not be null, empty or whitespace.");
            }

            string blobName = Path.GetFileName(sourceFile);
            var container = GetContainer(storageAccount, key, env);
            return UploadElement(container, sourceFile, blobName);
        }

        public static string DownloadCdnElement(string cndUrl, string elementName, string destination)
        {
            Uri elementUri = new Uri($"{cndUrl}/{elementName}");
            string destFile = Path.Combine(destination, elementName);

            var wc = new WebClient();
            wc.DownloadFile(elementUri, destFile);

            return destFile;
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

        private static string GetBlobName(string env, string sourceFile, string version)
        {
            Version specifedVersion;
            if (!Version.TryParse(version, out specifedVersion))
            {
                throw new ArgumentException($"The value '{version}' is not valid for parameter '{nameof(version)}'.");
            }

            Version versionInFile = TemplatesPackageInfo.ParseVersion(Path.GetFileName(sourceFile));
            if (versionInFile != null && versionInFile != specifedVersion)
            {
                throw new ArgumentException($"Parameter '{nameof(sourceFile)}' (with value '{sourceFile}') contains the version {versionInFile.ToString()} that do not match with the value specified in the parameter '{nameof(version)}' (with value '{version}').");
            }

            var envInFile = ParseEnv(Path.GetFileNameWithoutExtension(sourceFile));
            string prefix = string.Empty;
            if (!envInFile.ToString().Equals(env, StringComparison.OrdinalIgnoreCase) || envInFile == EnvEnum.Unknown)
            {
                prefix = env + ".";
            }

            string blobName = (versionInFile == null) ? $"{prefix}{Path.GetFileNameWithoutExtension(sourceFile)}_{version}.mstx" : sourceFile;
            return blobName;
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
    }
}
