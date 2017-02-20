using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Templates.Core.Locations
{
    public enum LocationCopyStatus
    {
        Started,
        SourceAdquired,
        TargetUpdated,
        Finished
    }

    public abstract class TemplatesLocation
    {
        public const string PackagesName = "Packages";
        public const string TemplatesName = "Templates";
        public const string VersionFileName = "version.txt";
        public const string ProjectTypes = "Projects";
        public const string Frameworks = "Frameworks";

        public abstract (LocationCopyStatus Status, string Message) Copy(string workingFolder);
        public abstract string GetVersion(string workingFolder);

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
    }
}
