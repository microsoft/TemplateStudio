// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.IO;
using Localization.Extensions;

namespace Localization
{
    public class LocalizableItemsMerger
    {
        private RoutesManager _routesManager;

        public LocalizableItemsMerger(string sourceDir, string destDir)
        {
            _routesManager = new RoutesManager(sourceDir, destDir);
        }

        public void MergeFiles()
        {
            foreach (var file in _routesManager.GetAllFilesFromSource())
            {
                if (file.Extension == ".resx")
                {
                    MergeResxFile(file);
                }
                else
                {
                    MergeFile(file);
                }
            }
        }

        public void MergeResxFile(FileInfo file)
        {
            var relativePath = _routesManager.GetRelativePathFromSourceFile(file);
            var destFile = _routesManager.GetFileFromDestination(relativePath);

            ResourcesExtensions.MergeResxFiles(file, destFile);
        }

        public void MergeFile(FileInfo file)
        {
            var relativePath = _routesManager.GetRelativePathFromSourceFile(file);
            var destFile = _routesManager.GetFileFromDestination(relativePath);

            file.CopyTo(destFile.FullName, true);
        }
    }
}
