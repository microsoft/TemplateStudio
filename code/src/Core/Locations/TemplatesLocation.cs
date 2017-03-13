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
        public const string TemplatesName = "Templates";
        public const string VersionFileName = "version.txt";
        public const string ProjectTypes = "Projects";
        public const string Frameworks = "Frameworks";
        public const string TempFolderName = "TempDownloads";

        public string TemplatesFolder { get; protected set; }
        public string TempDownloadsFolder { get; protected set; }

        public string CurrentTemplatesVersionFilePath { get; protected set; }
        public string CurrentTemplatesVersionFolder { get; protected set; }

        public string LocationFolder { get; protected set; }

        private string _workingFolder = String.Empty;
        public void InitializeWorkingFolder(string workingFolder)
        {
            _workingFolder = workingFolder;
            RefreshWorkingFolders();
        }

        protected void RefreshWorkingFolders()
        {
            LocationFolder = Path.Combine(_workingFolder, LocationId);
            TemplatesFolder = Path.Combine(LocationFolder, TemplatesName);
            TempDownloadsFolder = Path.Combine(LocationFolder, TempFolderName);
            CurrentTemplatesVersionFolder = Path.Combine(TemplatesFolder, GetCurrentTemplatesFolderName());
            CurrentTemplatesVersionFilePath = Path.Combine(CurrentTemplatesVersionFolder, VersionFileName);
        }

        protected abstract string LocationId { get; }
        public abstract void Adquire();
        public abstract bool Update();
        public abstract void Purge();
        protected abstract string GetCurrentTemplatesFolderName();

        public string GetVersion()
        {
            return GetVersionFromFile(CurrentTemplatesVersionFilePath).ToString();
        }

        protected static void SafeCopyFile(string sourceFile, string destFolder)
        {
            if (!Directory.Exists(destFolder))
            {
                Directory.CreateDirectory(destFolder);
            }
            File.Copy(sourceFile, Path.Combine(destFolder, Path.GetFileName(sourceFile)));
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
    }
}
