// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Localization
{
    internal class LocalizableItemsExtractor
    {
        private DirectoryInfo _sourceDir;
        private DirectoryInfo _destinationDir;
        private ValidateLocalizableExtractor _validator;
        private IEnumerable<string> _cultures;

        internal LocalizableItemsExtractor(string sourceDirPath, string destinationDirPath, IEnumerable<string> cultures, ValidateLocalizableExtractor validator)
        {
            _sourceDir = GetDirectory(sourceDirPath);
            _destinationDir = GetOrCreateDirectory(destinationDirPath);
            _cultures = cultures;
            _validator = validator;
        }

        internal void ExtractVsix()
        {
            if (!_validator.HasChanges(Routes.VsixValidatePath))
            {
                 return;
            }

            FileInfo manifiestFile = GetFile(Path.Combine(_sourceDir.FullName, Routes.VsixRootDirPath, Routes.VsixManifestFile));
            XmlDocument xmlManifiestFile = XmlUtility.LoadXmlFile(manifiestFile.FullName);
            string name = XmlUtility.GetNode(xmlManifiestFile, "DisplayName").InnerText.Trim();
            string description = XmlUtility.GetNode(xmlManifiestFile, "Description").InnerText.Trim();

            foreach (var culture in _cultures)
            {
                var vsixDesDirectory = GetOrCreateDirectory(Path.Combine(_destinationDir.FullName, Routes.VsixRootDirPath, culture));
                var langpackFile = new FileInfo(Path.Combine(vsixDesDirectory.FullName, Routes.VsixLangpackFile));

                using (TextWriter writer = langpackFile.CreateText())
                {
                    writer.Write(string.Format(Routes.VsixLangpackContent, name, description, culture));
                }
            }
        }

        internal void ExtractProjectTemplates()
        {
            if (_validator.HasChanges(Routes.ProjectTemplateFileNameValidateCS))
            {
                ExtractProjectTemplatesByLanguage(
                    Routes.ProjectTemplatePathCS,
                    Routes.ProjectTemplateFileCS,
                    Routes.ProjectTemplateFileNamePatternCS);
            }

            if (_validator.HasChanges(Routes.ProjectTemplateFileNameValidateVB))
            {
                ExtractProjectTemplatesByLanguage(
                    Routes.ProjectTemplatePathVB,
                    Routes.ProjectTemplateFileVB,
                    Routes.ProjectTemplateFileNamePatternVB);
            }
        }

        private void ExtractProjectTemplatesByLanguage(string projectTemplatePath, string projectTemplateFile, string projectTemplateFileNamePattern)
        {
            FileInfo srcFile = GetFile(Path.Combine(_sourceDir.FullName, projectTemplatePath, projectTemplateFile));
            var desDirectory = GetOrCreateDirectory(Path.Combine(_destinationDir.FullName, projectTemplatePath));

            foreach (string culture in _cultures)
            {
                string desFile = Path.Combine(desDirectory.FullName, string.Format(projectTemplateFileNamePattern, culture));
                srcFile.CopyTo(desFile, true);
            }
        }

        internal void ExtractCommandTemplates()
        {
            if (_validator.HasChanges(Routes.RelayCommandFileNameValidate))
            {
                LocalizeFileType(Routes.RelayCommandFileNameValidate, Routes.RelayCommandFileNamePattern);
            }

            if (_validator.HasChanges(Routes.VspackageFileNameValidate))
            {
                LocalizeFileType(Routes.VspackageFileNameValidate, Routes.VspackageFileNamePattern);
            }
        }

        private void LocalizeFileType(string filePath, string searchPattern)
        {
            FileInfo file = GetFile(Path.Combine(_sourceDir.FullName, filePath));
            DirectoryInfo desDirectory = GetOrCreateDirectory(Path.Combine(_destinationDir.FullName, Routes.CommandTemplateRootDirPath));

            foreach (string culture in _cultures)
            {
                file.CopyTo(Path.Combine(desDirectory.FullName, string.Format(searchPattern, culture)), true);
            }
        }

        internal void ExtractTemplatePages()
        {
            ExtractTemplateEngineTemplates(Routes.TemplatesPagesPath);
        }

        internal void ExtractTemplateFeatures()
        {
            ExtractTemplateEngineTemplates(Routes.TemplatesFeaturesPath);
        }

        private void ExtractTemplateEngineTemplates(string templatePath)
        {
            var srcDirectory = GetDirectory(Path.Combine(_sourceDir.FullName, templatePath));
            foreach (var subDirectory in srcDirectory.GetDirectories())
            {
                string pageSrcDirectory = Path.Combine(templatePath, subDirectory.Name, Routes.TemplateConfigDir);

                ExtractTemplateJson(pageSrcDirectory);
                ExtractTemplateDescription(pageSrcDirectory);
            }
        }

        private void ExtractTemplateJson(string srcDirectory)
        {
            string fileJson = Path.Combine(srcDirectory, Routes.TemplateJsonFile);
            FileInfo file = new FileInfo(Path.Combine(_sourceDir.FullName, fileJson));

            if (file.Exists && _validator.HasChanges(fileJson))
            {
                var desDirectory = GetOrCreateDirectory(Path.Combine(_destinationDir.FullName, srcDirectory));
                var metadata = JsonConvert.DeserializeObject<JObject>(File.ReadAllText(file.FullName));

                foreach (string culture in _cultures)
                {
                    string filePath = Path.Combine(desDirectory.FullName, culture + "." + Routes.TemplateJsonFile);
                    var con = new
                    {
                        author = metadata.GetValue("author").Value<string>(),
                        name = metadata.GetValue("name").Value<string>(),
                        description = metadata.GetValue("description").Value<string>(),
                        identity = metadata.GetValue("identity").Value<string>()
                    };
                    string content = JsonConvert.SerializeObject(con, Newtonsoft.Json.Formatting.Indented);
                    File.WriteAllText(filePath, content, Encoding.UTF8);
                }
            }
        }

        private void ExtractTemplateDescription(string srcDirectory)
        {
            string fileMd = Path.Combine(srcDirectory, Routes.TemplateDescriptionFile);
            FileInfo file = new FileInfo(Path.Combine(_sourceDir.FullName, fileMd));

            if (file.Exists && _validator.HasChanges(fileMd))
            {
                var desDirectory = GetOrCreateDirectory(Path.Combine(_destinationDir.FullName, srcDirectory));

                foreach (string culture in _cultures)
                {
                    FileInfo desFile = new FileInfo(Path.Combine(desDirectory.FullName, culture + "." + Routes.TemplateDescriptionFile));
                    file.CopyTo(desFile.FullName, true);
                }
            }
        }

        internal void ExtractWtsProjectTypes()
        {
            if (_validator.HasChanges(Routes.WtsProjectTypesValidate))
            {
                ExtractWtsTemplateFiles(Routes.WtsProjectTypes);
            }
            ExtractWtsTemplateSubfolderFiles(Routes.WtsProjectTypes);
        }

        internal void ExtractWtsFrameworks()
        {
            if (_validator.HasChanges(Routes.WtsFrameworksValidate))
            {
                ExtractWtsTemplateFiles(Routes.WtsFrameworks);
            }
            ExtractWtsTemplateSubfolderFiles(Routes.WtsFrameworks);
        }

        private void ExtractWtsTemplateFiles(string routeType)
        {
            var desDirectory = GetOrCreateDirectory(Path.Combine(_destinationDir.FullName, Routes.WtsTemplatesRootDirPath));
            var srcDirectory = new DirectoryInfo(Path.Combine(_sourceDir.FullName, Routes.WtsTemplatesRootDirPath, routeType));
            var srcFile = GetFile(srcDirectory.FullName + ".json");

            var fileContent = File.ReadAllText(srcFile.FullName);
            var content = JsonConvert.DeserializeObject<List<JObject>>(fileContent);
            var projects = content.Select(json => new
            {
                name = json.GetValue("name").Value<string>(),
                displayName = json.GetValue("displayName").Value<string>(),
                summary = json.GetValue("summary").Value<string>()
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
            var desDirectory = GetOrCreateDirectory(Path.Combine(_destinationDir.FullName, Routes.WtsTemplatesRootDirPath, routeType));
            var srcDirectory = new DirectoryInfo(Path.Combine(_sourceDir.FullName, Routes.WtsTemplatesRootDirPath, routeType));

            var srcFile = GetFile(srcDirectory.FullName + ".json");
            var fileContent = File.ReadAllText(srcFile.FullName);
            var content = JsonConvert.DeserializeObject<List<JObject>>(fileContent);
            var projectNames = content.Select(json => json.GetValue("name").Value<string>());

            foreach (string culture in _cultures)
            {
                foreach (var name in projectNames)
                {
                    var mdFilePath = Path.Combine(Routes.WtsTemplatesRootDirPath, routeType, name + ".md");
                    var mdFile = new FileInfo(Path.Combine(_sourceDir.FullName, mdFilePath));
                    mdFile.CopyTo(Path.Combine(desDirectory.FullName, culture + "." + name + ".md"), true);
                }
            }
        }

        internal void ExtractResourceFiles()
        {
            foreach (string directory in Routes.ResoureceDirectories)
            {
                var srcResFile = Path.Combine(directory, Routes.ResourcesFilePath);
                if (_validator.HasChanges(srcResFile))
                {
                    var desDirectory = GetOrCreateDirectory(Path.Combine(_destinationDir.FullName, directory));
                    FileInfo resourceFile = GetFile(Path.Combine(_sourceDir.FullName, srcResFile));

                    foreach (string culture in _cultures)
                    {
                        string destResFile = Path.Combine(desDirectory.FullName, string.Format(Routes.ResourcesFilePathPattern, culture));
                        resourceFile.CopyTo(destResFile, true);
                    }
                }
            }
        }

        private DirectoryInfo GetDirectory(string path)
        {
            var directory = new DirectoryInfo(path);

            if (!directory.Exists)
                throw new DirectoryNotFoundException($"Source directory \"{directory.FullName}\" not found.");

            return directory;
        }

        private DirectoryInfo GetOrCreateDirectory(string path)
        {
            var directory = new DirectoryInfo(path);

            if (!directory.Exists)
            {
                directory.Create();
            }

            return directory;
        }

        private FileInfo GetFile(string path)
        {
            var file = new FileInfo(path);

            if (!file.Exists)
                throw new FileNotFoundException($"File \"{file.FullName}\" not found.");

            return file;
        }
    }
}
