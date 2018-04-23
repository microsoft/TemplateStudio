using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Localization.Extensions
{
    public class RoutesManager
    {
        private DirectoryInfo _sourceDir;
        private DirectoryInfo _destinationDir;

        public RoutesManager(string sourceDirPath, string destinationDirPath)
        {
            _sourceDir = RoutesExtensions.GetDirectory(sourceDirPath);
            _destinationDir = RoutesExtensions.GetOrCreateDirectory(destinationDirPath);
        }

        public void CopyFromSourceToDest(string relativePath, string fileName)
        {
            var sourceFile = GetFileFromSource(Path.Combine(relativePath, fileName));
            var destDirectory = RoutesExtensions.GetOrCreateDirectory(Path.Combine(_destinationDir.FullName, relativePath));
            var destFile = Path.Combine(destDirectory.FullName, fileName);
            sourceFile.CopyTo(destFile, true);
        }

        public DirectoryInfo GetDirectoryFromSource(string path)
        {
            return RoutesExtensions.GetDirectory(Path.Combine(_sourceDir.FullName, path));
        }

        public FileInfo GetFileFromSource(string path)
        {
            return RoutesExtensions.GetFile(Path.Combine(_sourceDir.FullName, path));
        }

        public FileInfo GetFileFromSource(string relativePath, string fileName)
        {
            return RoutesExtensions.GetFile(Path.Combine(_sourceDir.FullName, relativePath, fileName));
        }

        public DirectoryInfo GetOrCreateDestDirectory(string path)
        {
            return RoutesExtensions.GetOrCreateDirectory(Path.Combine(_destinationDir.FullName, path));
        }
    }
}
