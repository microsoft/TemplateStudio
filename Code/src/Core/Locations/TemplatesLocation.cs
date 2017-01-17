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

        public abstract void Copy(string workingFolder);

        protected static void SafeDelete(string directoryPath)
        {
            if (Directory.Exists(directoryPath))
            {
                Directory.Delete(directoryPath, true);
            }
        }

        protected static void SafeCopyFile(string sourceFile, string destFolder)
        {
            Directory.CreateDirectory(destFolder);
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
