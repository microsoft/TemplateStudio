// ******************************************************************
// Copyright (c) Microsoft. All rights reserved.
// This code is licensed under the MIT License (MIT).
// THE CODE IS PROVIDED “AS IS”, WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED,
// INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.
// IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
// DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT,
// TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH
// THE CODE OR THE USE OR OTHER DEALINGS IN THE CODE.
// ******************************************************************

using System;
using System.IO;

using Microsoft.Templates.Core.Diagnostics;
using Microsoft.Templates.Core.Resources;

namespace Microsoft.Templates.Core.Locations
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

        public static void CopyRecursive(string sourceDir, string targetDir)
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

        public static void SafeCopyFile(string sourceFile, string destFolder)
        {
            try
            {
                if (!Directory.Exists(destFolder))
                {
                    Directory.CreateDirectory(destFolder);
                }
                File.Copy(sourceFile, Path.Combine(destFolder, Path.GetFileName(sourceFile)));
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

        public static void SafeMoveDirectory(string sourceDir, string targetDir)
        {
            try
            {
                if (Directory.Exists(sourceDir))
                {
                    CopyRecursive(sourceDir, targetDir);
                    SafeDeleteDirectory(sourceDir);
                }
            }
            catch (Exception ex)
            {
                AppHealth.Current.Warning.TrackAsync(string.Format(StringRes.FsSafeMoveDirectoryMessage, sourceDir, targetDir, ex.Message), ex).FireAndForget();
            }
        }

        public static void SafeDeleteDirectory(string dir)
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
                AppHealth.Current.Warning.TrackAsync(string.Format(StringRes.FsSafeDeleteDirectoryMessage, dir, ex.Message), ex).FireAndForget();
            }
        }
    }
}
