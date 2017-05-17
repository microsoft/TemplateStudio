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
        private const string VersionFileName = "version.txt";

        public override string Id { get => Configuration.Current.Environment; }

        public override void Acquire(string targetFolder)
        {
            var tempFolder = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());

            string downloadedFile = Download(tempFolder);

            if (File.Exists(downloadedFile))
            {
                ExtractContent(downloadedFile, tempFolder);

                MoveContent(tempFolder, targetFolder);
            }
        }

        private string Download(string tempFolder)
        {
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
                string msg = "The templates content can't be downloaded.";

                AppHealth.Current.Exception.TrackAsync(ex, msg).FireAndForget();

                throw;
            }
        }

        private void ExtractContent(string file, string tempFolder)
        {
            try
            {
                Templatex.Extract(file, tempFolder);
                AppHealth.Current.Verbose.TrackAsync($"Templates content extracted to {tempFolder}.").FireAndForget();
            }
            catch (Exception ex)
            {
                var msg = "The templates content can't be extracted.";

                AppHealth.Current.Exception.TrackAsync(ex, msg).FireAndForget();

                throw;
            }
        }

        private void MoveContent(string tempFolder, string targetFolder)
        {
            string sourcePath = Path.Combine(tempFolder, SourceFolderName);
            string verFile = Path.Combine(sourcePath, VersionFileName);
            Version ver = GetVersionFromFile(verFile);

            Fs.EnsureFolder(targetFolder);

            var finalDestination = Path.Combine(targetFolder, ver.ToString());

            if (!Directory.Exists(finalDestination))
            {
                Fs.SafeDeleteFile(verFile);
                Fs.SafeMoveDirectory(sourcePath, finalDestination);
            }

            Fs.SafeDeleteDirectory(tempFolder);
        }

        private static Version GetVersionFromFile(string versionFilePath)
        {
            var version = "0.0.0.0";

            if (File.Exists(versionFilePath))
            {
                version = File.ReadAllText(versionFilePath);
            }

            if (!Version.TryParse(version, out Version result))
            {
                result = new Version(0, 0, 0, 0);
            }

            return result;
        }
    }
}
