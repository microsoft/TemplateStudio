// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Collections.Generic;

using Microsoft.Templates.Core.Diagnostics;
using Microsoft.Templates.Core.Resources;
using Microsoft.Templates.Core.Packaging;

namespace Microsoft.Templates.Core.Locations
{
    public abstract class TemplatesSource
    {
        protected const string SourceFolderName = "Templates";
        private const string VersionFileName = "version.txt";

        private List<string> _tempFoldersUsed = new List<string>();
        public virtual bool ForcedAcquisition { get; protected set; }

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

        public Version GetVersionFromMstx(string mstxFilePath)
        {
            string content = GetFileContent(mstxFilePath, VersionFileName);

            if (!Version.TryParse(content, out Version result))
            {
                result = new Version(0, 0, 0, 0);
            }

            return result;
        }

        private static string GetFileContent(string mstxFilePath, string fileName)
        {
            string result = string.Empty;
            if (File.Exists(mstxFilePath))
            {
                using (ZipArchive zip = ZipFile.Open(mstxFilePath, ZipArchiveMode.Read))
                {
                    var entry = zip.Entries.Where(e => e.Name == fileName).FirstOrDefault();
                    if (entry != null)
                    {
                        using (StreamReader sr = new StreamReader(entry.Open()))
                        {
                            result = sr.ReadToEnd();
                        }
                    }
                }
            }
            return result;
        }

        protected void ExtractContent(string file, string tempFolder)
        {
            try
            {
                TemplatePackage.Extract(file, tempFolder, VerifyPackageSignatures);
                AppHealth.Current.Verbose.TrackAsync($"{StringRes.TemplatesContentExtractedToString} {tempFolder}.").FireAndForget();
            }
            catch (Exception ex)
            {
                AppHealth.Current.Exception.TrackAsync(ex, StringRes.TemplatesSourceExtractContentMessage).FireAndForget();
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
