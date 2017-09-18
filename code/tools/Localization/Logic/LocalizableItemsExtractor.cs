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
        private ValidateLocalizableExtractor validator;
        private IEnumerable<string> cultures;

        internal LocalizableItemsExtractor(string sourceDirPath, string destinationDirPath, IEnumerable<string> cultures, string tagName)
        {
            _sourceDir = GetDirectory(sourceDirPath);

            _destinationDir = new DirectoryInfo(destinationDirPath);
            if (!_destinationDir.Exists)
                _destinationDir.Create();

            this.cultures = cultures;
            validator = new ValidateLocalizableExtractor(sourceDirPath, tagName);
        }

        internal void ExtractVsix()
        {
            var desDirectory = Path.Combine(_destinationDir.FullName, Routes.VsixRootDirPath);
            if (Directory.Exists(desDirectory))
                return;

            if (!validator.HasChanges(Routes.VsixValidatePath))
                return;

            FileInfo manifiestFile = GetFile(Path.Combine(_sourceDir.FullName, Routes.VsixRootDirPath, Routes.VsixManifestFile));
            XmlDocument xmlManifiestFile = XmlUtility.LoadXmlFile(manifiestFile.FullName);
            string name = XmlUtility.GetNode(xmlManifiestFile, "DisplayName").InnerText.Trim();
            string description = XmlUtility.GetNode(xmlManifiestFile, "Description").InnerText.Trim();

            foreach (var culture in cultures)
            {
                var vsixDesDirectory = new DirectoryInfo(Path.Combine(desDirectory, culture));
                var langpackFile = new FileInfo(Path.Combine(vsixDesDirectory.FullName, Routes.VsixLangpackFile));

                if (langpackFile.Exists)
                    continue;

                if (!vsixDesDirectory.Exists)
                    vsixDesDirectory.Create();

                using (TextWriter writer = langpackFile.CreateText())
                {
                    writer.Write(string.Format(Routes.VsixLangpackContent, name, description, culture));
                }
            }
        }

        internal void ExtractProjectTemplates()
        {
            var desDirectory = new DirectoryInfo(Path.Combine(_destinationDir.FullName, Routes.ProjectTemplatePath));
            if (desDirectory.Exists)
                return;

            if (!validator.HasChanges(Routes.ProjectTemplateFileNameValidate))
                return;

            FileInfo srcFile = GetFile(Path.Combine(_sourceDir.FullName, Routes.ProjectTemplatePath, Routes.ProjectTemplateFile));
            desDirectory.Create();

            foreach (string culture in cultures)
            {
                string desFile = Path.Combine(desDirectory.FullName, string.Format(Routes.ProjectTemplateFileNamePattern, culture));

                if (File.Exists(desFile))
                    continue;

                srcFile.CopyTo(desFile);
            }
        }

        internal void ExtractCommandTemplates()
        {
            var desDirectory = new DirectoryInfo(Path.Combine(_destinationDir.FullName, Routes.CommandTemplateRootDirPath));
            if (desDirectory.Exists)
                return;

            if (validator.HasChanges(Routes.RelayCommandFileNameValidate))
            {
                LocalizeFileType(Routes.RelayCommandFileNameValidate, desDirectory, Routes.RelayCommandFileNamePattern);
            }

            if (validator.HasChanges(Routes.VspackageFileNameValidate))
            {
                LocalizeFileType(Routes.VspackageFileNameValidate, desDirectory, Routes.VspackageFileNamePattern);
            }
        }

        private void LocalizeFileType(string filePath, DirectoryInfo desDirectory, string searchPattern)
        {
            FileInfo file = GetFile(Path.Combine(_sourceDir.FullName, filePath));

            if (!desDirectory.Exists)
                desDirectory.Create();

            foreach (string culture in cultures)
            {
                file.CopyTo(Path.Combine(desDirectory.FullName, string.Format(searchPattern, culture)));
            }
        }

        internal void ExtractTemplatePagesAndFeatures()
        {
            ExtractTemplateEngineTemplates(Routes.TemplatesPagesPath);
            ExtractTemplateEngineTemplates(Routes.TemplatesFeaturesPath);
        }

        private void ExtractTemplateEngineTemplates(string templatePath)
        {
            var desDirectory = new DirectoryInfo(Path.Combine(_destinationDir.FullName, templatePath));
            if (desDirectory.Exists)
                return;

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

            if (file.Exists && validator.HasChanges(fileJson))
            {
                var desDirectory = new DirectoryInfo(Path.Combine(_destinationDir.FullName, srcDirectory));
                if (!desDirectory.Exists)
                    desDirectory.Create();

                var metadata = JsonConvert.DeserializeObject<JObject>(File.ReadAllText(file.FullName));
                foreach (string culture in cultures)
                {
                    string filePath = Path.Combine(desDirectory.FullName, culture + "." + Routes.TemplateJsonFile);
                    if (!File.Exists(filePath))
                    {
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
        }

        private void ExtractTemplateDescription(string srcDirectory)
        {
            string fileMd = Path.Combine(srcDirectory, Routes.TemplateDescriptionFile);
            FileInfo file = new FileInfo(Path.Combine(_sourceDir.FullName, fileMd));

            if (file.Exists && validator.HasChanges(fileMd))
            {
                var desDirectory = new DirectoryInfo(Path.Combine(_destinationDir.FullName, srcDirectory));
                if (!desDirectory.Exists)
                    desDirectory.Create();

                foreach (string culture in cultures)
                {
                    FileInfo desFile = new FileInfo(Path.Combine(desDirectory.FullName, culture + "." + Routes.TemplateDescriptionFile));
                    if (!desFile.Exists)
                    {
                        file.CopyTo(desFile.FullName);
                    }
                }
            }
        }

        internal void ExtractWtsTemplates()
        {
            var desDirectory = new DirectoryInfo(Path.Combine(_destinationDir.FullName, Routes.WtsTemplatesRootDirPath));
            if (desDirectory.Exists)
                return;

            if (validator.HasChanges(Routes.WtsProjectTypesValidate))
            {
                ExtractWtsTemplateFiles(Routes.WtsProjectTypes);
            }

            if (validator.HasChanges(Routes.WtsFrameworksValidate))
            {
                ExtractWtsTemplateFiles(Routes.WtsFrameworks);
            }

            ExtractWtsTemplateSubfolderFiles(Routes.WtsProjectTypes);
            ExtractWtsTemplateSubfolderFiles(Routes.WtsFrameworks);
        }

        private void ExtractWtsTemplateFiles(string routeType)
        {
            var desDirectory = new DirectoryInfo(Path.Combine(_destinationDir.FullName, Routes.WtsTemplatesRootDirPath));
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

            foreach (string culture in cultures)
            {
                var desFile = Path.Combine(desDirectory.FullName, culture + "." + srcFile.Name);

                if (File.Exists(desFile))
                    continue;

                if (!desDirectory.Exists)
                    desDirectory.Create();

                File.WriteAllText(desFile, data, Encoding.UTF8);
            }
        }

        private void ExtractWtsTemplateSubfolderFiles(string routeType)
        {
            var desDirectory = new DirectoryInfo(Path.Combine(_destinationDir.FullName, Routes.WtsTemplatesRootDirPath, routeType));
            var srcDirectory = new DirectoryInfo(Path.Combine(_sourceDir.FullName, Routes.WtsTemplatesRootDirPath, routeType));

            var srcFile = GetFile(srcDirectory.FullName + ".json");
            var fileContent = File.ReadAllText(srcFile.FullName);
            var content = JsonConvert.DeserializeObject<List<JObject>>(fileContent);
            var projectNames = content.Select(json => json.GetValue("name").Value<string>());

            foreach (string culture in cultures)
            {
                foreach (var name in projectNames)
                {
                    var mdFilePath = Path.Combine(Routes.WtsTemplatesRootDirPath, routeType, name + ".md");

                    if (!validator.HasChanges(mdFilePath))
                        continue;

                    if (!desDirectory.Exists)
                        desDirectory.Create();

                    var mdFile = new FileInfo(Path.Combine(_sourceDir.FullName, mdFilePath));
                    mdFile.CopyTo(Path.Combine(desDirectory.FullName, culture + "." + name + ".md"));
                }
            }
        }

        internal void ExtractResourceFiles()
        {
            foreach (string directory in Routes.ResoureceDirectories)
            {
                var desDirectory = new DirectoryInfo(Path.Combine(_destinationDir.FullName, directory));
                if (desDirectory.Exists)
                    continue;

                var srcResFile = Path.Combine(directory, Routes.ResourcesFilePath);
                if (!validator.HasChanges(srcResFile))
                    continue;

                FileInfo resourceFile = GetFile(Path.Combine(_sourceDir.FullName, srcResFile));
                desDirectory.Create();

                foreach (string culture in cultures)
                {
                    string destResFile = Path.Combine(desDirectory.FullName, string.Format(Routes.ResourcesFilePathPattern, culture));
                    resourceFile.CopyTo(destResFile);
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

        private FileInfo GetFile(string path)
        {
            var file = new FileInfo(path);

            if (!file.Exists)
                throw new FileNotFoundException($"File \"{file.FullName}\" not found.");

            return file;
        }
    }
}
