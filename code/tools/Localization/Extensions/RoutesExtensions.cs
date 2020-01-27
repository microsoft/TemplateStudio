// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.IO;

namespace Localization.Extensions
{
    public static class RoutesExtensions
    {
        public static DirectoryInfo GetOrCreateDirectory(string path)
        {
            var directory = new DirectoryInfo(path);

            if (!directory.Exists)
            {
                directory.Create();
            }

            return directory;
        }

        public static DirectoryInfo GetDirectory(string path)
        {
            var directory = new DirectoryInfo(path);

            return directory;
        }

        public static FileInfo GetFile(string path)
        {
            var file = new FileInfo(path);

            if (!file.Exists)
            {
                throw new FileNotFoundException($"File \"{file.FullName}\" not found.");
            }

            return file;
        }
    }
}
