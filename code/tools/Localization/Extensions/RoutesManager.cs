// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;
using System.IO;
using System.Linq;

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

        public IEnumerable<FileInfo> GetAllFilesFromSource()
        {
            return _sourceDir.EnumerateFiles("*", SearchOption.AllDirectories);
        }

        public FileInfo GetFileFromSource(string relativePath, string fileName)
        {
            return GetFileFromSource(Path.Combine(relativePath, fileName));
        }

        public DirectoryInfo GetOrCreateDestDirectory(string path)
        {
            return RoutesExtensions.GetOrCreateDirectory(Path.Combine(_destinationDir.FullName, path));
        }

        public FileInfo GetFileFromDestination(string path)
        {
            return RoutesExtensions.GetFile(Path.Combine(_destinationDir.FullName, path));
        }

        public FileInfo GetFileFromDestination(string relativePath, string fileName)
        {
            return this.GetFileFromDestination(Path.Combine(relativePath, fileName));
        }

        public bool ExistInSource(string relativePath)
        {
            return File.Exists(Path.Combine(_sourceDir.FullName, relativePath));
        }

        public bool ExistInDestination(string relativePath)
        {
            return File.Exists(Path.Combine(_destinationDir.FullName, relativePath));
        }

        public bool ExistInSourceAndDestination(string relativePath)
        {
            return ExistInSource(relativePath) && ExistInDestination(relativePath);
        }

        public string GetRelativePathFromSourceFile(FileInfo file)
        {
            return file.FullName.StartsWith(_sourceDir.FullName)
                ? file.FullName.Substring(_sourceDir.FullName.Length + 1)
                : string.Empty;
        }

        public string GetRelativeDirectoryFromSource(DirectoryInfo directory)
        {
            return directory.FullName.StartsWith(_sourceDir.FullName)
                ? directory.FullName.Substring(_sourceDir.FullName.Length + 1)
                : string.Empty;
        }
    }
}
