// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Localization.Extensions;

namespace Localization
{
    internal class OriginalLocalizableItemsExtractor
    {
        private RoutesManager _routesManager;

        internal OriginalLocalizableItemsExtractor(string sourceDirPath, string destinationDirPath)
        {
            _routesManager = new RoutesManager(sourceDirPath, destinationDirPath);
        }

        internal void Extract()
        {
            // vsix
            _routesManager.CopyFromSourceToDest(Routes.VsixRootDirPath, Routes.VsixManifestFile);

            // project templates
            _routesManager.CopyFromSourceToDest(Routes.ProjectTemplatePathCSUwp, Routes.ProjectTemplateFileCSUwp);
            _routesManager.CopyFromSourceToDest(Routes.ProjectTemplatePathCSWpf, Routes.ProjectTemplateFileCSWpf);
            _routesManager.CopyFromSourceToDest(Routes.ProjectTemplatePathVBUwp, Routes.ProjectTemplateFileVBUwp);

            // command templates
            _routesManager.CopyFromSourceToDest(Routes.CommandTemplateRootDirPath, Routes.RelayCommandFile);
            _routesManager.CopyFromSourceToDest(Routes.CommandTemplateRootDirPath, Routes.VspackageFile);

            // templates
            foreach (string platform in Routes.TemplatesPlatforms)
            {
                CopyTemplatesFiles(platform, Routes.TemplatesPagesPatternPath);
                CopyTemplatesFiles(platform, Routes.TemplatesFeaturesPatternPath);
                CopyTemplatesFiles(platform, Routes.TemplatesServicesPatternPath);
                CopyTemplatesFiles(platform, Routes.TemplatesTestingPatternPath);

                // _catalog
                CopyCatalogType(platform, Routes.WtsProjectTypes);
                CopyCatalogType(platform, Routes.WtsFrameworks);
            }

            // resources
            foreach (string directory in Routes.ResoureceDirectories)
            {
                _routesManager.CopyFromSourceToDest(directory, Routes.ResourcesFilePath);
            }
        }

        private void CopyTemplatesFiles(string platform, string templateType)
        {
            var baseDir = Path.Combine(Routes.TemplatesRootDirPath, platform, templateType);
            var templatesDirectory = _routesManager.GetDirectoryFromSource(baseDir);

            if (templatesDirectory.Exists)
            {
                var directories = templatesDirectory.GetDirectories().Where(c => !c.Name.EndsWith("VB", StringComparison.OrdinalIgnoreCase));

                foreach (var directory in directories)
                {
                    var templatePath = Path.Combine(baseDir, directory.Name, Routes.TemplateConfigDir);

                    _routesManager.CopyFromSourceToDest(templatePath, Routes.TemplateJsonFile);
                    _routesManager.CopyFromSourceToDest(templatePath, Routes.TemplateDescriptionFile);
                }
            }
        }

        private void CopyCatalogType(string platform, string routeType)
        {
            var baseDir = Path.Combine(Routes.TemplatesRootDirPath, platform, Routes.CatalogPath);
            if (Directory.Exists(baseDir))
            {
                _routesManager.CopyFromSourceToDest(baseDir, routeType + ".json");

                var path = Path.Combine(baseDir, routeType);
                foreach (var name in GetNamesByRouteType(platform, routeType))
                {
                    _routesManager.CopyFromSourceToDest(path, name + ".md");
                }
            }
        }

        private IEnumerable<string> GetNamesByRouteType(string platform, string routeType)
        {
            var baseDir = Path.Combine(Routes.TemplatesRootDirPath, platform, Routes.CatalogPath);

            var jsonFile = _routesManager.GetFileFromSource(baseDir, routeType + ".json");
            return JsonExtensions.GetValuesByName(jsonFile.FullName, "name");
        }
    }
}
