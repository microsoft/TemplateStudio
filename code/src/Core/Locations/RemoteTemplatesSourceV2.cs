// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.IO;
using System.Net;

using Microsoft.Templates.Core.Diagnostics;
using Microsoft.Templates.Core.Packaging;
using Microsoft.Templates.Core.Resources;

namespace Microsoft.Templates.Core.Locations
{
    public sealed class RemoteTemplatesSourceV2 : TemplatesSourceV2
    {
        private readonly string _cdnUrl = Configuration.Current.CdnUrl;

        public override TemplatesContentInfo GetContent(TemplatesPackageInfo packageInfo, string workingFolder)
        {
            var extractionFolder = Extract(packageInfo);

            var finalDestination = Path.Combine(workingFolder, packageInfo.Version.ToString());

            Fs.SafeMoveDirectory(Path.Combine(extractionFolder, "Templates"), finalDestination);

            return new TemplatesContentInfo()
            {
                Date = packageInfo.Date,
                Path = finalDestination,
                Version = packageInfo.Version
            };

            // TODO REMOVE TEMPS
        }

        //TODO: Check if this is used SM
        //public override void Adquire(ref TemplatesPackageInfo packageInfo)
        //{
        //    var tempFolder = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());
        //    var sourceUrl = $"{_cdnUrl}/{packageInfo.Name}";
        //    var fileTarget = Path.Combine(tempFolder, packageInfo.Name);
        //    Fs.EnsureFolder(tempFolder);

        //    DownloadContent(sourceUrl, fileTarget);

        //    packageInfo.LocalPath = fileTarget;
        //}

        public override void LoadConfig()
        {
            var tempFolder = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());
            var sourceUrl = $"{_cdnUrl}/config.json";
            var fileTarget = Path.Combine(tempFolder, "config.json");
            Fs.EnsureFolder(tempFolder);

            DownloadContent(sourceUrl, fileTarget);

            Config = TemplatesSourceConfig.LoadFromFile(fileTarget);

            Fs.SafeDeleteDirectory(tempFolder);
        }

        private static void DownloadContent(string sourceUrl, string file)
        {
            try
            {
                var wc = new WebClient();

                wc.DownloadFile(sourceUrl, file);
                AppHealth.Current.Verbose.TrackAsync(string.Format(StringRes.RemoteTemplatesSourceDownloadContentOkMessage, file, sourceUrl)).FireAndForget();
            }
            catch (Exception ex)
            {
                AppHealth.Current.Info.TrackAsync(StringRes.RemoteTemplatesSourceDownloadContentKoInfoMessage).FireAndForget();
                AppHealth.Current.Error.TrackAsync(string.Format(StringRes.RemoteTemplatesSourceDownloadContentKoErrorMessage, sourceUrl), ex).FireAndForget();
            }
        }

        private static string Extract(TemplatesPackageInfo packageInfo, bool verifyPackageSignatures = true)
        {
            if (!string.IsNullOrEmpty(packageInfo.LocalPath))
            {
                Extract(packageInfo.LocalPath, Path.GetDirectoryName(packageInfo.LocalPath), verifyPackageSignatures);
                return Path.GetDirectoryName(packageInfo.LocalPath);
            }
            else
            {
                // TODO: Package Not Adquired
                // ERROR
                return null;
            }
        }

        private static void Extract(string mstxFilePath, string versionedFolder, bool verifyPackageSignatures = true)
        {
            try
            {
                TemplatePackage.Extract(mstxFilePath, versionedFolder, verifyPackageSignatures);
                AppHealth.Current.Verbose.TrackAsync($"{StringRes.TemplatesContentExtractedToString} {versionedFolder}.").FireAndForget();
            }
            catch (Exception ex)
            {
                AppHealth.Current.Exception.TrackAsync(ex, StringRes.TemplatesSourceExtractContentMessage).FireAndForget();
                throw;
            }
        }
    }
}
