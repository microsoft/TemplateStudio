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
using System.Collections.Generic;

namespace Microsoft.Templates.Core.Locations
{
    public abstract class TemplatesSource
    {
        protected const string SourceFolderName = "Templates";
        private const string VersionFileName = "version.txt";

        private List<string> _tempFoldersUsed = new List<string>();

        public virtual string Id { get => Configuration.Current.Environment; }

        public void Acquire(string targetFolder)
        {
            string mstxFilePath = ObtainMstx();

            Acquire(mstxFilePath, targetFolder);
        }

        public void Acquire(string mstxFilePath, string targetFolder)
        {
            string extractedContent = ExtractMstx(mstxFilePath);

            SetContent(extractedContent, targetFolder);

            CleanUpTemps();
        }

        protected abstract string ObtainMstx();

        public string ExtractMstx(string mstxFilePath)
        {
            var tempFolder = GetTempFolder();

            if (File.Exists(mstxFilePath))
            {
                ExtractContent(mstxFilePath, tempFolder);
                return Path.Combine(tempFolder, SourceFolderName);
            }
            else
            {
                return String.Empty;
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

        private void SetContent(string sourcePath, string finalTargetFolder)
        {
            string verFile = Path.Combine(sourcePath, VersionFileName);
            Version ver = GetVersionFromFile(verFile);

            Fs.EnsureFolder(finalTargetFolder);

            var finalDestination = Path.Combine(finalTargetFolder, ver.ToString());

            if (!Directory.Exists(finalDestination))
            {
                Fs.SafeDeleteFile(verFile);
                Fs.SafeMoveDirectory(sourcePath, finalDestination);
            }
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

        protected string GetTempFolder()
        {
            var tempFolder = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());
            _tempFoldersUsed.Add(tempFolder);
            return tempFolder;
        }

        private void CleanUpTemps()
        {
            foreach(string tempFolder in _tempFoldersUsed)
            {
                Fs.SafeDeleteDirectory(tempFolder);
            }
        }
    }
}
