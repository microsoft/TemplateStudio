// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.IO;

using Microsoft.Templates.Core.Diagnostics;
using Microsoft.Templates.Core.Resources;

namespace Microsoft.Templates.Core
{
    public static class Fs
    {
        public static void EnsureFolder(string folder)
        {
            if (!Directory.Exists(folder))
            {
                Directory.CreateDirectory(folder);
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
                if (!Directory.Exists(destFolder))
                {
                    Directory.CreateDirectory(destFolder);
                }

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

        public static void SafeMoveDirectory(string sourceDir, string targetDir, bool overwrite = false)
        {
            try
            {
                if (Directory.Exists(sourceDir))
                {
                    CopyRecursive(sourceDir, targetDir, overwrite);
                    SafeDeleteDirectory(sourceDir);
                }
            }
            catch (Exception ex)
            {
                AppHealth.Current.Warning.TrackAsync(string.Format(StringRes.FsSafeMoveDirectoryMessage, sourceDir, targetDir, ex.Message), ex).FireAndForget();
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
                var msg = string.Format(StringRes.FsEnsureFileEditableException, filePath);
                AppHealth.Current.Warning.TrackAsync(msg, ex).FireAndForget();
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
    }
}
