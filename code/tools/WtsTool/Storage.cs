// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.WindowsAzure.Storage.Blob;

namespace WtsTool
{
    public static class Storage
    {
        public static CloudBlobContainer GetContainer(string account, string key, string container)
        {
            StorageCredentials credentials = new StorageCredentials(account, key);

            CloudStorageAccount storageAccount = new CloudStorageAccount(credentials, true);

            CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();

            CloudBlobContainer blobContainer = blobClient.GetContainerReference(container);

            return blobContainer;
        }

        public static IEnumerable<CloudBlockBlob> GetAllElements(CloudBlobContainer container)
        {
            BlobContinuationToken token = new BlobContinuationToken();

            List<CloudBlockBlob> result = new List<CloudBlockBlob>();

            while (token != null)
            {
                result.AddRange(GetElements(container, ref token));
            }

            return result;
        }

        public static IEnumerable<CloudBlockBlob> GetElements(CloudBlobContainer container, ref BlobContinuationToken token)
        {
            if (!container.Exists())
            {
                throw new ArgumentException($"The container {container.Uri} does not exists.");
            }

            // TODO: Retrieve max 5000
            BlobResultSegment result = container.ListBlobsSegmented(token);
            token = result.ContinuationToken;
            return result.Results.Select((i) => i as CloudBlockBlob);
        }
    }
}
