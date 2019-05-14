// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;
using System.IO;
using System.Linq;
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

        private void MergeFile(FileInfo file)
        {
            var relativePath = _routesManager.GetRelativePathFromSourceFile(file);
            var destFile = _routesManager.GetFileFromDestination(relativePath);

            file.CopyTo(destFile.FullName, true);
        }

        private void MergeResxFile(FileInfo file)
        {
            var mainKeys = GetmainResxKeys(file);
            var relativePath = _routesManager.GetRelativePathFromSourceFile(file);
            var destFile = _routesManager.GetFileFromDestination(relativePath);

            var newResxItems = ResourcesExtensions.GetResourcesByFile(file.FullName);
            var sourceResxItems = ResourcesExtensions.GetResourcesByFile(destFile.FullName)
                .Where(item => mainKeys.Contains(item.Key) && !newResxItems.Keys.Contains(item.Key));

            foreach (var item in sourceResxItems)
            {
                newResxItems.Add(item.Key, item.Value);
            }

            ResourcesExtensions.CreateResxFile(destFile.FullName, newResxItems);
        }

        private IEnumerable<string> GetmainResxKeys(FileInfo file)
        {
            var relativeDirectory = _routesManager.GetRelativeDirectoryFromSource(file.Directory);
            var mainFile = _routesManager.GetFileFromDestination(Path.Combine(relativeDirectory, Routes.ResourcesFilePath));
            return ResourcesExtensions.GetResourcesByFile(mainFile.FullName).Keys;
        }
    }
}
