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
using System.Collections.Generic;
using System.IO;

using Microsoft.Templates.Core.Diagnostics;

namespace Microsoft.Templates.Core.Locations
{
    public abstract class TemplatesSource
    {
        protected const string SourceFolderName = "Templates";
        private const string VersionFileName = "version.txt";

        private List<string> _tempFoldersUsed = new List<string>();
        protected virtual bool VerifyPackageSignatures { get => true; }
        public virtual string Id { get => Configuration.Current.Environment; }

        public void Acquire(string targetFolder)
        {
            string mstxFilePath = AcquireMstx();

            Extract(mstxFilePath, targetFolder);
        }

        public void Extract(string mstxFilePath, string targetFolder)
        {
            string extractedContent = ExtractMstx(mstxFilePath);

            SetContent(extractedContent, targetFolder);

            CleanUpTemps();
        }

        protected abstract string AcquireMstx();

        public string ExtractMstx(string mstxFilePath)
        {
            if (File.Exists(mstxFilePath))
            {
                var tempFolder = GetTempFolder();

                ExtractContent(mstxFilePath, tempFolder);
                return Path.Combine(tempFolder, SourceFolderName);
            }
            else
            {
                return string.Empty;
            }
        }

        protected void ExtractContent(string file, string tempFolder)
        {
            try
            {
                Templatex.Extract(file, tempFolder, VerifyPackageSignatures);
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

            string finalDestination = PrepareFinalDestination(finalTargetFolder, ver);

            if (!Directory.Exists(finalDestination))
            {
                Fs.SafeDeleteFile(verFile);
                Fs.SafeMoveDirectory(sourcePath, finalDestination);
            }
        }

        private static string PrepareFinalDestination(string finalTargetFolder, Version ver)
        {
            Fs.EnsureFolder(finalTargetFolder);

            var finalDestination = Path.Combine(finalTargetFolder, ver.ToString());

            if (ver.IsNullOrZero() && Directory.Exists(finalDestination))
            {
                Fs.SafeDeleteDirectory(finalDestination);
            }

            return finalDestination;
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
            List<string> removedFolders = new List<string>();
            foreach (string tempFolder in _tempFoldersUsed)
            {
                Fs.SafeDeleteDirectory(tempFolder);
                removedFolders.Add(tempFolder);
            }
            foreach (string folder in removedFolders)
            {
                _tempFoldersUsed.Remove(folder);
            }
        }
    }
}
