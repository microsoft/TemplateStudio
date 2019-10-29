// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using Localization.Extensions;
using Localization.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Localization
{
    internal class LocalizableItemsExtractor
    {
        private RoutesManager _routesManager;
        private ValidateLocalizableExtractor _validator;
        private IEnumerable<string> _cultures;

        internal LocalizableItemsExtractor(string sourceDir, string destDir, ValidateLocalizableExtractor validator, IEnumerable<string> cultures)
        {
            _routesManager = new RoutesManager(sourceDir, destDir);
            _cultures = cultures;
            _validator = validator;
        }

        internal void ExtractVsix()
        {
            if (!_validator.HasVsixChanges())
            {
                 return;
            }

            var manifiestFile = _routesManager.GetFileFromSource(Routes.VsixRootDirPath, Routes.VsixManifestFile);
            var manifest = XmlUtility.LoadXmlFile(manifiestFile.FullName);
            string name = manifest.GetNode("DisplayName").InnerText.Trim();
            string description = manifest.GetNode("Description").InnerText.Trim();

            foreach (var culture in _cultures)
            {
                var desDirectory = _routesManager.GetOrCreateDestDirectory(Path.Combine(Routes.VsixRootDirPath, culture));
                var langpackFile = new FileInfo(Path.Combine(desDirectory.FullName, Routes.VsixLangpackFile));

                using (TextWriter writer = langpackFile.CreateText())
                {
                    writer.Write(string.Format(Routes.VsixLangpackContent, name, description, culture));
                }
            }
        }

        internal void ExtractProjectTemplates()
        {
            var csPath = Path.Combine(Routes.ProjectTemplatePathCS, Routes.ProjectTemplateFileCS);
            if (_validator.HasVsTemplatesChanges(csPath))
            {
                ExtractProjectTemplatesByLanguage(
                    Routes.ProjectTemplatePathCS,
                    Routes.ProjectTemplateFileCS,
                    Routes.ProjectTemplateFileNamePatternCS);
            }

            var vbPath = Path.Combine(Routes.ProjectTemplatePathVB, Routes.ProjectTemplateFileVB);
            if (_validator.HasVsTemplatesChanges(vbPath))
            {
                ExtractProjectTemplatesByLanguage(
                    Routes.ProjectTemplatePathVB,
                    Routes.ProjectTemplateFileVB,
                    Routes.ProjectTemplateFileNamePatternVB);
            }
        }

        private void ExtractProjectTemplatesByLanguage(string projectTemplatePath, string projectTemplateFile, string projectTemplateFileNamePattern)
        {
            FileInfo srcFile = _routesManager.GetFileFromSource(Path.Combine(projectTemplatePath, projectTemplateFile));
            var desDirectory = _routesManager.GetOrCreateDestDirectory(projectTemplatePath);

            foreach (string culture in _cultures)
            {
                string desFile = Path.Combine(desDirectory.FullName, string.Format(projectTemplateFileNamePattern, culture));
                srcFile.CopyTo(desFile, true);
            }
        }

        internal void ExtractCommandTemplates()
        {
            if (_validator.HasRelayCommandPackageChanges())
            {
                LocalizeFileType(Routes.RelayCommandFileNameValidate, Routes.RelayCommandFileNamePattern);
            }

            if (_validator.HasVsPackageResxChanges())
            {
                LocalizeFileType(Routes.VspackageFileNameValidate, Routes.VspackageFileNamePattern);
            }
        }

        private void LocalizeFileType(string filePath, string searchPattern)
        {
            FileInfo file = _routesManager.GetFileFromSource(filePath);
            DirectoryInfo desDirectory = _routesManager.GetOrCreateDestDirectory(Routes.CommandTemplateRootDirPath);

            foreach (string culture in _cultures)
            {
                file.CopyTo(Path.Combine(desDirectory.FullName, string.Format(searchPattern, culture)), true);
            }
        }

        internal void ExtractTemplatePages()
        {
            ExtractTemplateEngineTemplates(Routes.TemplatesPagesPatternPath);
        }

        internal void ExtractTemplateFeatures()
        {
            ExtractTemplateEngineTemplates(Routes.TemplatesFeaturesPatternPath);
        }

        internal void ExtractTemplateServices()
        {
            ExtractTemplateEngineTemplates(Routes.TemplatesServicesPatternPath);
        }

        internal void ExtractTemplateTesting()
        {
            ExtractTemplateEngineTemplates(Routes.TemplatesTestingPatternPath);
        }

        private void ExtractTemplateEngineTemplates(string patternPath)
        {
            foreach (string platform in Routes.TemplatesPlatforms)
            {
                var baseDir = Path.Combine(Routes.TemplatesRootDirPath, platform, patternPath);
                var templatesDirectory = _routesManager.GetDirectoryFromSource(baseDir);
                var templatesDirectories = templatesDirectory.GetDirectories().Where(c => !c.Name.EndsWith("VB", StringComparison.OrdinalIgnoreCase));

                foreach (var directory in templatesDirectories)
                {
                    string templateSrcDirectory = Path.Combine(baseDir, directory.Name, Routes.TemplateConfigDir);

                    ExtractTemplateJson(templateSrcDirectory);
                    ExtractTemplateDescription(templateSrcDirectory);
                }
            }
        }

        private void ExtractTemplateJson(string srcDirectory)
        {
            string fileJson = Path.Combine(srcDirectory, Routes.TemplateJsonFile);
            FileInfo file = _routesManager.GetFileFromSource(fileJson);

            if (file.Exists && _validator.HasTemplateJsonChanges(fileJson))
            {
                var desDirectory = _routesManager.GetOrCreateDestDirectory(srcDirectory);
                var metadata = JsonConvert.DeserializeObject<JObject>(File.ReadAllText(file.FullName));

                foreach (string culture in _cultures)
                {
                    string filePath = Path.Combine(desDirectory.FullName, culture + "." + Routes.TemplateJsonFile);
                    var con = new
                    {
                        author = metadata.GetValue("author", StringComparison.Ordinal).Value<string>(),
                        name = metadata.GetValue("name", StringComparison.Ordinal).Value<string>(),
                        description = metadata.GetValue("description", StringComparison.Ordinal).Value<string>(),
                        identity = metadata.GetValue("identity", StringComparison.Ordinal).Value<string>(),
                    };
                    string content = JsonConvert.SerializeObject(con, Newtonsoft.Json.Formatting.Indented);
                    File.WriteAllText(filePath, content, Encoding.UTF8);
                }
            }
        }

        private void ExtractTemplateDescription(string srcDirectory)
        {
            string fileMd = Path.Combine(srcDirectory, Routes.TemplateDescriptionFile);
            FileInfo file = _routesManager.GetFileFromSource(fileMd);

            if (file.Exists && _validator.HasTemplateMdChanges(fileMd))
            {
                var desDirectory = _routesManager.GetOrCreateDestDirectory(srcDirectory);

                foreach (string culture in _cultures)
                {
                    FileInfo desFile = new FileInfo(Path.Combine(desDirectory.FullName, culture + "." + Routes.TemplateDescriptionFile));
                    file.CopyTo(desFile.FullName, true);
                }
            }
        }

        internal void ExtractWtsProjectTypes()
        {
            if (_validator.HasCatalogJsonChanges(Routes.WtsProjectTypesValidate))
            {
                ExtractWtsTemplateFiles(Routes.WtsProjectTypes);
            }

            ExtractWtsTemplateSubfolderFiles(Routes.WtsProjectTypes);
        }

        internal void ExtractWtsFrameworks()
        {
            if (_validator.HasCatalogJsonChanges(Routes.WtsFrameworksValidate))
            {
                ExtractWtsTemplateFiles(Routes.WtsFrameworks);
            }

            ExtractWtsTemplateSubfolderFiles(Routes.WtsFrameworks);
        }

        private void ExtractWtsTemplateFiles(string routeType)
        {
            var desDirectory = _routesManager.GetOrCreateDestDirectory(Routes.WtsTemplatesRootDirPath);
            var srcFile = _routesManager.GetFileFromSource(Routes.WtsTemplatesRootDirPath, routeType + ".json");

            var fileContent = File.ReadAllText(srcFile.FullName);
            var content = JsonConvert.DeserializeObject<List<JObject>>(fileContent);
            var projects = content.Select(json => new
            {
                name = json.GetValue("name", StringComparison.Ordinal).Value<string>(),
                displayName = json.GetValue("displayName", StringComparison.Ordinal).Value<string>(),
                summary = json.GetValue("summary", StringComparison.Ordinal).Value<string>(),
            });

            var data = JsonConvert.SerializeObject(projects, Newtonsoft.Json.Formatting.Indented);

            foreach (string culture in _cultures)
            {
                var desFile = Path.Combine(desDirectory.FullName, culture + "." + srcFile.Name);
                File.WriteAllText(desFile, data, Encoding.UTF8);
            }
        }

        private void ExtractWtsTemplateSubfolderFiles(string routeType)
        {
            var srcFile = _routesManager.GetFileFromSource(Routes.WtsTemplatesRootDirPath, routeType + ".json");
            var fileContent = File.ReadAllText(srcFile.FullName);
            var content = JsonConvert.DeserializeObject<List<JObject>>(fileContent);
            var projectNames = content.Select(json => json.GetValue("name", StringComparison.Ordinal).Value<string>());

            foreach (var name in projectNames)
            {
                var mdFilePath = Path.Combine(Routes.WtsTemplatesRootDirPath, routeType, name + ".md");
                if (_validator.HasTemplateMdChanges(mdFilePath))
                {
                    var mdFile = _routesManager.GetFileFromSource(mdFilePath);
                    var desDirectory = _routesManager.GetOrCreateDestDirectory(Path.Combine(Routes.WtsTemplatesRootDirPath, routeType));

                    foreach (string culture in _cultures)
                    {
                        mdFile.CopyTo(Path.Combine(desDirectory.FullName, culture + "." + name + ".md"), true);
                    }
                }
            }
        }

        internal void ExtractResourceFiles()
        {
            foreach (string directory in Routes.ResoureceDirectories)
            {
                var srcResFile = Path.Combine(directory, Routes.ResourcesFilePath);
                var newResxValues = _validator.GetResourcesWithChanges(srcResFile);

                if (newResxValues.Any())
                {
                    var desDirectory = _routesManager.GetOrCreateDestDirectory(directory).FullName;
                    var resourceFile = ResourcesExtensions.CreateResxFile(Path.Combine(desDirectory, Routes.ResourcesFilePath), newResxValues);

                    foreach (string culture in _cultures)
                    {
                        string destResFile = Path.Combine(desDirectory, string.Format(Routes.ResourcesFilePathPattern, culture));
                        resourceFile.CopyTo(destResFile, true);
                    }

                    resourceFile.Delete();
                }
            }
        }
    }
}
