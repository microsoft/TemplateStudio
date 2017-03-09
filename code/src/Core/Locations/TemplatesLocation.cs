using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Templates.Core.Locations
{
    public abstract class TemplatesLocation
    {
        public const string PackagesName = "Packages";
        public const string TemplatesName = "Templates";
        public const string VersionFileName = "version.txt";
        public const string ProjectTypes = "Projects";
        public const string Frameworks = "Frameworks";
        public const string TempFolderName = "Temp";

        public abstract void Adquire(string workingFolder);
        public abstract bool Update(string workingFolder);

        public string GetVersion(string workingFolder)
        {
            var fileName = Path.Combine(workingFolder, Path.Combine(TemplatesName, VersionFileName));
            return GetVersionFromFile(fileName);
        }

        protected static void SafeDelete(string directoryPath)
        {
            if (Directory.Exists(directoryPath))
            {
                Directory.Delete(directoryPath, true);
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

        protected static string GetVersionFromFile(string versionFilePath)
        {
            var version = "0.0.0";
            if (File.Exists(versionFilePath))
            {
                version = File.ReadAllText(versionFilePath);
            }
            return version;
        }
    }
}
