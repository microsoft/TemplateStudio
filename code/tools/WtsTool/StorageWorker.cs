// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.WindowsAzure.Storage.Blob;
using WtsTool.CommandOptions;
using System.Text.RegularExpressions;

namespace WtsTool
{
    public class RemotePackageData
    {
        public string Name { get; set; }

        public Uri Uri { get; set; }

        public string Date { get; set; }

        public Version Version { get; set; }

        public string MayorMinorVersion => $"{Version.Major.ToString()}.{Version.Major.ToString()}";

        public EnvEnum Env { get; set; }
    }

    public static class StorageWorker
    {
        public static void ListMayorMinorVersions(CommonOptions options,  TextWriter output, TextWriter error)
        {

        }

        public static void ListDetailedVersions(CommonOptions options, TextWriter output, TextWriter error)
        {

        }

        public static void ListSummaryInfo(CommonOptions options, TextWriter output, TextWriter error)
        {
            CloudBlobContainer container = Storage.GetContainer(options.StorageAccount, options.AccountKey, options.Env.ToString().ToLowerInvariant());
            var remoteElements = Storage.GetAllElements(container);
            var remteInfo = remoteElements.Select((e) => new RemotePackageData
            {
                Name = e.Name,
                Uri = e.Uri,
                Date = e.Metadata["date"],
                Version = ParseVersion(e),
                Env = ParseEnv(e)
            });
        }

        private static EnvEnum ParseEnv(CloudBlockBlob e)
        {
            return EnvEnum.Dev;
        }

        private static Version ParseVersion(CloudBlockBlob e)
        {
            string versionPattern = @"\d+.\d+.\d+.\d+";
            Regex versionRegEx = new Regex(versionPattern, RegexOptions.Compiled);
            var match = versionRegEx.Match(e.Name);
            if (match.Success)
            {
                return new Version(match.Value);
            }
            else
            {
                return new Version(0, 0, 0, 0);
            }
        }
    }
}
