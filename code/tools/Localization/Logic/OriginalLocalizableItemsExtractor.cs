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
            _routesManager.CopyFromSourceToDest(Routes.ProjectTemplatePathCS, Routes.ProjectTemplateFileCS);
            _routesManager.CopyFromSourceToDest(Routes.ProjectTemplatePathVB, Routes.ProjectTemplateFileVB);

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
            }

            // _catalog
            CopyCatalogType(Routes.WtsProjectTypes);
            CopyCatalogType(Routes.WtsFrameworks);

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
            var directories = templatesDirectory.GetDirectories().Where(c => !c.Name.EndsWith("VB", StringComparison.OrdinalIgnoreCase));

            foreach (var directory in directories)
            {
                var templatePath = Path.Combine(baseDir, directory.Name, Routes.TemplateConfigDir);
                _routesManager.CopyFromSourceToDest(templatePath, Routes.TemplateJsonFile);
                _routesManager.CopyFromSourceToDest(templatePath, Routes.TemplateDescriptionFile);
            }

            ////var templatesDirectory = _routesManager.GetDirectoryFromSource(Path.Combine(Routes.TemplatesRootDirPath, platform));
            ////var allDirectories = templatesDirectory.GetDirectories(templateType, SearchOption.AllDirectories);
            ////var names = allDirectories.SelectMany(d => d.GetDirectories().Where(c => !c.Name.EndsWith("VB", StringComparison.OrdinalIgnoreCase))).Select(d => d.Name);

            ////foreach (var name in names)
            ////{
            ////    var templatePath = Path.Combine(Routes.TemplatesRootDirPath, platform, templateType, name, Routes.TemplateConfigDir);
            ////    _routesManager.CopyFromSourceToDest(templatePath, Routes.TemplateJsonFile);
            ////    _routesManager.CopyFromSourceToDest(templatePath, Routes.TemplateDescriptionFile);
            ////}
        }

        private void CopyCatalogType(string routeType)
        {
            _routesManager.CopyFromSourceToDest(Routes.WtsTemplatesRootDirPath, routeType + ".json");

            var path = Path.Combine(Routes.WtsTemplatesRootDirPath, routeType);
            foreach (var name in GetNamesByRouteType(routeType))
            {
                _routesManager.CopyFromSourceToDest(path, name + ".md");
            }
        }

        private IEnumerable<string> GetNamesByRouteType(string routeType)
        {
            var jsonFile = _routesManager.GetFileFromSource(Routes.WtsTemplatesRootDirPath, routeType + ".json");
            return JsonExtensions.GetValuesByName(jsonFile.FullName, "name");
        }
    }
}
