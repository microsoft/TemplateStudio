using Microsoft.Templates.Core.Diagnostics;
using Microsoft.Templates.Core.Extensions;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Templates.Core.Locations
{
    public abstract class TemplatesLocation
    {
        public const string TemplatesFolderName = "Templates";
        public const string VersionFileName = "version.txt";
        public const string ProjectTypes = "Projects";
        public const string Frameworks = "Frameworks";
        public const string DownloadsFolderName = "Downloads";


        public string RootWorkingFolder { get; private set; }
        public string RootTemplatesFolder { get; protected set; }
        public string DownloadsFolder { get; protected set; }
        public string LocationFolder { get; protected set; }
        public string CurrentVersionFilePath { get; protected set; }
        public string CurrentVersionFolder { get; protected set; }
        public Version CurrentVersion { get; private set; }



        public void Initialize(string workingFolder)
        {
            RootWorkingFolder = workingFolder;
            RefreshFolders();
        }
        public void RefreshFolders()
        {
            RootTemplatesFolder = Path.Combine(RootWorkingFolder, TemplatesFolderName);
            DownloadsFolder = Path.Combine(RootWorkingFolder, DownloadsFolderName);

            LocationFolder = Path.Combine(RootTemplatesFolder, Id);

            CurrentVersionFolder = Path.Combine(LocationFolder, GetLatestTemplateFolder());
            CurrentVersionFilePath = Path.Combine(CurrentVersionFolder, VersionFileName);

            SetCurrentVersion();
        }

        public abstract string Id { get; }
        public abstract void Adquire();
        public abstract bool UpdateAvailable();
        protected abstract string GetLatestTemplateFolder();

        public void Purge()
        {
            CleanUpDownloads();
            CleanUpOldVersions();
        }

        protected static void CopyRecursive(string sourceDir, string targetDir)
        {
            Directory.CreateDirectory(targetDir);

            foreach (var file in Directory.GetFiles(sourceDir))
            {
                File.Copy(file, Path.Combine(targetDir, Path.GetFileName(file)));
            }

            foreach (var directory in Directory.GetDirectories(sourceDir))
            {
                CopyRecursive(directory, Path.Combine(targetDir, Path.GetFileName(directory)));
            }
        }
        protected static void SafeDeleteDirectory(string dir)
        {
            try
            {
                if (Directory.Exists(dir))
                {
                    Directory.Delete(dir, true);
                }
            }
            catch (Exception ex)
            {
                var msg = $"The folder {dir} can't be delete. Error: {ex.Message}";
                AppHealth.Current.Warning.TrackAsync(msg, ex).FireAndForget();
            }
        }

        protected static void SafeMoveDirectory(string sourceDir, string targetDir)
        {
            try
            {
                if (Directory.Exists(sourceDir))
                {
                    Directory.Move(sourceDir, targetDir);
                }
            }
            catch (Exception ex)
            {
                var msg = $"The folder {sourceDir} can't be moved to {targetDir}. Error: {ex.Message}";
                AppHealth.Current.Warning.TrackAsync(msg, ex).FireAndForget();
            }
        }

        protected static Version GetVersionFromFile(string versionFilePath)
        {
            var version = "0.0.0.0";
            if (File.Exists(versionFilePath))
            {
                version = File.ReadAllText(versionFilePath).Replace("v", ""); //TODO: quitar cuando no sea necesario.
            }
            if (!Version.TryParse(version, out Version result))
            {
                result = new Version(0, 0, 0, 0);
            }
            return result;
        }

        protected static Version GetVersionFromPackage(string packagePath)
        {
            string version = String.Empty;
            if (File.Exists(packagePath))
            {
                using (ZipArchive zip = ZipFile.Open(packagePath, ZipArchiveMode.Read))
                {
                    var versionFile = zip.Entries.Where(e => e.Name == $"{VersionFileName}").FirstOrDefault();
                    if (versionFile != null)
                    {
                        using (StreamReader sr = new StreamReader(versionFile.Open()))
                        {
                            version = sr.ReadToEnd().Replace("v", ""); //TODO: Quitar una vez se haya configurado bien la build.
                        }
                    }
                }
            }
            if (!Version.TryParse(version, out Version result))
            {
                result = new Version(0, 0, 0, 0);
            }
            return result;
        }

        protected void SetCurrentVersion()
        {
            if (File.Exists(CurrentVersionFilePath))
            {
                CurrentVersion = GetVersionFromFile(CurrentVersionFilePath);
            }
            else
            {
                CurrentVersion = new Version(0, 0, 0, 0);
            }
        }

        protected static void EnsureFolder(string folder)
        {
            if (!Directory.Exists(folder))
            {
                Directory.CreateDirectory(folder);
            }
        }

        protected static void SafeCopyFile(string sourceFile, string destFolder)
        {
            if (!Directory.Exists(destFolder))
            {
                Directory.CreateDirectory(destFolder);
            }
            File.Copy(sourceFile, Path.Combine(destFolder, Path.GetFileName(sourceFile)));
        }

        private void CleanUpDownloads()
        {
            if (Directory.Exists(DownloadsFolder))
            {
                DirectoryInfo di = new DirectoryInfo(DownloadsFolder);
                foreach (var sdi in di.EnumerateDirectories())
                {
                    SafeDeleteDirectory(sdi.FullName);
                }
            }
        }

        private void CleanUpOldVersions()
        {
            if (Directory.Exists(LocationFolder))
            {
                DirectoryInfo di = new DirectoryInfo(LocationFolder);
                foreach (var sdi in di.EnumerateDirectories())
                {
                    Version.TryParse(sdi.Name, out Version v);
                    if (v < CurrentVersion)
                    {
                        SafeDeleteDirectory(sdi.FullName);
                    }
                }
            }
        }
    }
}
