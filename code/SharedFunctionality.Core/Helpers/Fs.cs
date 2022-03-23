// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using Microsoft.Templates.Core.Diagnostics;
using Microsoft.Templates.Resources;

namespace Microsoft.Templates.Core.Helpers
{
    public static class Fs
    {
        public static void EnsureFolderExists(string folder)
        {
            try
            {
                if (!Directory.Exists(folder))
                {
                    Directory.CreateDirectory(folder);
                }
            }
            catch (Exception ex)
            {
                var message = string.Format(StringRes.ErrorCreatingFolder, folder, ex.Message);
                AppHealth.Current.Warning.TrackAsync(message, ex).FireAndForget();
            }
        }

        public static void CopyRecursive(string sourceDir, string targetDir, bool overwrite = false)
        {
            Directory.CreateDirectory(targetDir);

            foreach (var file in Directory.GetFiles(sourceDir))
            {
                File.Copy(file, Path.Combine(targetDir, Path.GetFileName(file)), overwrite);
            }

            foreach (var directory in Directory.GetDirectories(sourceDir))
            {
                CopyRecursive(directory, Path.Combine(targetDir, Path.GetFileName(directory)), overwrite);
            }
        }

        public static void SafeCopyFile(string sourceFile, string destFolder, bool overwrite)
        {
            try
            {
                EnsureFolderExists(destFolder);

                var destFile = Path.Combine(destFolder, Path.GetFileName(sourceFile));

                if (File.Exists(destFile))
                {
                    EnsureFileEditable(destFile);
                }

                File.Copy(sourceFile, destFile, overwrite);
            }
            catch (Exception ex)
            {
                AppHealth.Current.Warning.TrackAsync(string.Format(StringRes.FsSafeCopyFileMessage, sourceFile, destFolder, ex.Message), ex).FireAndForget();
            }
        }

        public static void SafeMoveFile(string filePath, string newPath, bool overwrite = true, bool warnOnFailure = true)
        {
            if (!File.Exists(filePath) || (File.Exists(newPath) && !overwrite))
            {
                return;
            }

            try
            {
                if (File.Exists(newPath) && overwrite)
                {
                    File.Delete(newPath);
                }

                File.Move(filePath, newPath);
            }
            catch (Exception ex)
            {
                if (warnOnFailure)
                {
                    AppHealth.Current.Warning.TrackAsync(string.Format(StringRes.FsSafeMoveFileMessage, filePath, newPath, ex.Message), ex).FireAndForget();
                }
            }
        }

        public static void SafeDeleteFile(string filePath)
        {
            try
            {
                if (File.Exists(filePath))
                {
                    File.Delete(filePath);
                }
            }
            catch (Exception ex)
            {
                AppHealth.Current.Warning.TrackAsync(string.Format(StringRes.FsSafeDeleteFileMessage, filePath, ex.Message), ex).FireAndForget();
            }
        }

        public static void EnsureFileEditable(string filePath)
        {
            try
            {
                var fileInfo = new FileInfo(filePath);
                if (fileInfo.IsReadOnly)
                {
                    fileInfo.IsReadOnly = false;
                }
            }
            catch (Exception ex)
            {
                var message = string.Format(StringRes.FsEnsureFileEditableException, filePath);
                AppHealth.Current.Warning.TrackAsync(message, ex).FireAndForget();
            }
        }

        public static void SafeDeleteDirectory(string dir, bool warnOnFailure = true)
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
                if (warnOnFailure)
                {
                    AppHealth.Current.Warning.TrackAsync(string.Format(StringRes.FsSafeDeleteDirectoryMessage, dir, ex.Message), ex).FireAndForget();
                }
            }
        }

        public static string FindFileAtOrAbove(string path, string extension)
        {
            var file = Directory.EnumerateFiles(path, extension, SearchOption.TopDirectoryOnly).FirstOrDefault();

            if (file != null)
            {
                return file;
            }
            else
            {
                var parent = Directory.GetParent(path);
                if (parent != null)
                {
                    return FindFileAtOrAbove(Directory.GetParent(path).FullName, extension);
                }
                else
                {
                    return string.Empty;
                }
            }
        }

        public static void SafeRenameDirectory(string dir, string dirNewName, bool warnOnFailure = true)
        {
            try
            {
                if (Directory.Exists(dir))
                {
                    Directory.Move(dir, dirNewName);
                }
            }
            catch (Exception ex)
            {
                if (warnOnFailure)
                {
                    AppHealth.Current.Warning.TrackAsync(string.Format(StringRes.FsSafeRenameDirectoryMessage, dir, ex.Message), ex).FireAndForget();
                }
            }
        }

        public static IEnumerable<string> GetExistingFolderNames(string rootDir)
        {
            if (Directory.Exists(rootDir))
            {
                return Directory.EnumerateDirectories(rootDir, "*", SearchOption.TopDirectoryOnly).Select(d => Path.GetFileName(d));
            }

            return new List<string>();
        }
    }
}
