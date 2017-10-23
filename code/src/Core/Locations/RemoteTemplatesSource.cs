// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.IO;
using System.Net;

using Microsoft.Templates.Core.Diagnostics;
using Microsoft.Templates.Core.Resources;

namespace Microsoft.Templates.Core.Locations
{
    public sealed class RemoteTemplatesSource : TemplatesSource
    {
        public override bool ForcedAcquisition => false;

        private readonly string _cdnUrl = Configuration.Current.CdnUrl;

        private const string TemplatesPackageFileName = "Templates.mstx";

        protected override string AcquireMstx()
        {
            var tempFolder = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());

            var sourceUrl = $"{_cdnUrl}/{TemplatesPackageFileName}";
            var fileTarget = Path.Combine(tempFolder, TemplatesPackageFileName);

            Fs.EnsureFolder(tempFolder);

            DownloadContent(sourceUrl, fileTarget);

            return fileTarget;
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
    }
}
