using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

            if (!directory.Exists)
            {
                throw new DirectoryNotFoundException($"Source directory \"{directory.FullName}\" not found.");
            }

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
