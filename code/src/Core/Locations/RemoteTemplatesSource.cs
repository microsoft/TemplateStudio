// ******************************************************************
// Copyright (c) Microsoft. All rights reserved.
// This code is licensed under the MIT License (MIT).
// THE CODE IS PROVIDED “AS IS”, WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED,
// INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.
// IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
// DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT,
// TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH
// THE CODE OR THE USE OR OTHER DEALINGS IN THE CODE.
// ******************************************************************

using System;
using System.IO;
using System.Net;

using Microsoft.Templates.Core.Diagnostics;

namespace Microsoft.Templates.Core.Locations
{
    public class RemoteTemplatesSource : TemplatesSource
    {
        private readonly string CdnUrl = Configuration.Current.CdnUrl;
        private const string TemplatesPackageFileName = "Templates.mstx";

        protected override string AcquireMstx()
        {
            var tempFolder = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());

            var sourceUrl = $"{CdnUrl}/{TemplatesPackageFileName}";
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
                AppHealth.Current.Verbose.TrackAsync($"Templates content downloaded to {file} from {sourceUrl}.").FireAndForget();
            }
            catch (Exception ex)
            {
                string msg = $"Templates content can't be downloaded right now, we will try it later.";
                AppHealth.Current.Info.TrackAsync(msg).FireAndForget();
                AppHealth.Current.Error.TrackAsync($"Error downloading from {sourceUrl}. Internet connection is required to download template updates.", ex).FireAndForget();
            }
        }

    }
}
