// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;
using System.IO;
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

        internal LocalizableItemsExtractor(string sourceDirPath, string destinationDirPath, string tagName)
        {
            _sourceDir = new DirectoryInfo(sourceDirPath);
            if (!_sourceDir.Exists)
                throw new DirectoryNotFoundException($"Source directory \"{sourceDirPath}\" not found.");

            _destinationDir = new DirectoryInfo(destinationDirPath);
            if (!_destinationDir.Exists)
                _destinationDir.Create();

            validator = new ValidateLocalizableExtractor(sourceDirPath, tagName);
        }

        internal void ExtractVsix(List<string> cultures)
        {
            if (!validator.ValidVisxFile())
                return;

            DirectoryInfo vsixDesDirectory = new DirectoryInfo(Path.Combine(_destinationDir.FullName, Routes.VsixRootDirPath));
            if (vsixDesDirectory.Exists)
                return;

            vsixDesDirectory.Create();
            vsixDesDirectory.CreateSubdirectory("Content");
            DirectoryInfo vsixSrcDirectory = new DirectoryInfo(Path.Combine(_sourceDir.FullName, Routes.VsixRootDirPath));

            if (!vsixSrcDirectory.Exists)
                throw new DirectoryNotFoundException($"Source directory \"{vsixSrcDirectory.FullName}\" not found.");

            FileInfo manifiestFile = new FileInfo(Path.Combine(vsixSrcDirectory.FullName, Routes.VsixManifestFile));
            if (!manifiestFile.Exists)
                throw new FileNotFoundException($"File \"{manifiestFile.FullName}\" not found.");

            XmlDocument xmlManifiestFile = XmlUtility.LoadXmlFile(manifiestFile.FullName);
            FileInfo langpackFile;
            DirectoryInfo vsixLocDesDirectory;
            string localizedName = XmlUtility.GetNode(xmlManifiestFile, "DisplayName").InnerText.Trim();
            string localizedDescription = XmlUtility.GetNode(xmlManifiestFile, "Description").InnerText.Trim();

            foreach (string culture in cultures)
            {
                vsixLocDesDirectory = new DirectoryInfo(Path.Combine(vsixDesDirectory.FullName, culture));
                if (vsixLocDesDirectory.Exists)
                    continue;

                vsixLocDesDirectory.Create();
                langpackFile = new FileInfo(Path.Combine(vsixLocDesDirectory.FullName, Routes.VsixLangpackFile));

                if (!langpackFile.Exists)
                {
                    using (TextWriter writer = langpackFile.CreateText())
                    {
                        writer.Write(string.Format(Routes.VsixLangpackContent, localizedName, localizedDescription, culture));
                    }
                }
            }
        }

        internal void ExtractProjectTemplates(List<string> cultures)
        {
            if (!validator.ValidProjectTemplateFile())
                return;

            DirectoryInfo templateDesDirectory = new DirectoryInfo(Path.Combine(_destinationDir.FullName, Routes.ProjectTemplateRootDirPath, Routes.ProjectTemplateDirNamePattern));
            if (templateDesDirectory.Exists)
                return;

            templateDesDirectory.Create();
            DirectoryInfo templateSrcDirectory = new DirectoryInfo(Path.Combine(_sourceDir.FullName, Routes.ProjectTemplateRootDirPath, Routes.ProjectTemplateDirNamePattern));
            if (!templateSrcDirectory.Exists)
                throw new DirectoryNotFoundException($"Source directory \"{templateSrcDirectory.FullName}\" not found.");

            foreach (string culture in cultures)
            {
                FileInfo file = new FileInfo(Path.Combine(templateSrcDirectory.FullName, string.Format(Routes.ProjectTemplateFileNamePattern, culture)));

                if (!file.Exists)
                    throw new FileNotFoundException($"File \"{file.FullName}\" not found.");

                file.CopyTo(Path.Combine(templateDesDirectory.FullName, string.Format(Routes.ProjectTemplateFileNamePattern, culture)));
            }
        }

        internal void ExtractCommandTemplates(List<string> cultures)
        {
            DirectoryInfo templateDesDirectory = new DirectoryInfo(Path.Combine(_destinationDir.FullName, Routes.CommandTemplateRootDirPath));
            if (templateDesDirectory.Exists)
                return;

            templateDesDirectory.Create();
            DirectoryInfo templateSrcDirectory = new DirectoryInfo(Path.Combine(_sourceDir.FullName, Routes.CommandTemplateRootDirPath));

            if (!templateSrcDirectory.Exists)
                throw new DirectoryNotFoundException($"Source directory \"{templateSrcDirectory.FullName}\" not found.");

            if (validator.ValidRelayCommandeFile())
                LocalizeFileType(templateSrcDirectory, templateDesDirectory, Routes.RelayCommandFileNamePattern, cultures);

            if (validator.ValidVspackageFile())
                LocalizeFileType(templateSrcDirectory, templateDesDirectory, Routes.VspackageFileNamePattern, cultures);
        }

        private void LocalizeFileType(DirectoryInfo srcDirectory, DirectoryInfo desDirectory, string searchPattern, List<string> cultures)
        {
            foreach (string culture in cultures)
            {
                FileInfo file = new FileInfo(Path.Combine(srcDirectory.FullName, string.Format(searchPattern, culture)));

                if (!file.Exists)
                    throw new FileNotFoundException($"File \"{file.FullName}\" not found.");

                file.CopyTo(Path.Combine(desDirectory.FullName, string.Format(searchPattern, culture)));
            }
        }

        internal void ExtractTemplateEngineTemplates(List<string> cultures)
        {
            ExtractTemplateEngineTemplates("Pages", cultures);
            ExtractTemplateEngineTemplates("Features", cultures);
        }

        private void ExtractTemplateEngineTemplates(string templateType, List<string> cultures)
        {
            DirectoryInfo templateDesDirectory = new DirectoryInfo(Path.Combine(_destinationDir.FullName, Routes.TemplatesRootDirPath, templateType));
            if (!templateDesDirectory.Exists)
                templateDesDirectory.Create();

            string templateSrcDirectory = Path.Combine(Routes.TemplatesRootDirPath, templateType);
            DirectoryInfo fullTemplateSrcDirectory = new DirectoryInfo(Path.Combine(_sourceDir.FullName, templateSrcDirectory));
            if (!fullTemplateSrcDirectory.Exists)
                throw new DirectoryNotFoundException($"Source directory \"{fullTemplateSrcDirectory.FullName}\" not found.");

            ExtractTemplatesFromDirectory(templateSrcDirectory, templateDesDirectory, cultures);
        }

        private void ExtractTemplatesFromDirectory(string templateSrcDirectory, DirectoryInfo destinationDirectroy, List<string> cultures)
        {
            string templateFileDirectory = Path.Combine(templateSrcDirectory, Routes.TemplateJsonFile);
            FileInfo file = new FileInfo(Path.Combine(_sourceDir.FullName, templateFileDirectory));

            if (file.Exists)
            {
                if (!destinationDirectroy.Exists)
                    destinationDirectroy.Create();

                // template.json
                var metadata = JsonConvert.DeserializeObject<JObject>(File.ReadAllText(file.FullName));
                if (validator.CheckModifiedFile(templateFileDirectory))
                {
                    foreach (string culture in cultures)
                    {
                        string filePath = Path.Combine(destinationDirectroy.FullName, culture + "." + Routes.TemplateJsonFile);
                        if (!File.Exists(filePath))
                        {
                            string content = string.Format(Routes.TemplateEngineJsonContent, metadata.GetValue("author").Value<string>(), metadata.GetValue("name").Value<string>(), metadata.GetValue("description").Value<string>(), metadata.GetValue("identity").Value<string>());
                            File.WriteAllText(filePath, content, Encoding.UTF8);
                        }
                    }
                }

                // description.md
                string srcFileDirectory = Path.Combine(templateSrcDirectory, Routes.TemplateDescriptionFile);
                if (validator.CheckModifiedFile(srcFileDirectory))
                {
                    FileInfo srcFile = new FileInfo(Path.Combine(_sourceDir.FullName, srcFileDirectory));
                    foreach (string culture in cultures)
                    {
                        FileInfo desFile = new FileInfo(Path.Combine(destinationDirectroy.FullName, culture + "." + Routes.TemplateDescriptionFile));
                        if (!desFile.Exists)
                        {
                            srcFile.CopyTo(desFile.FullName);
                        }
                    }
                }
            }
            else
            {
                DirectoryInfo sourceDirectory = new DirectoryInfo(Path.Combine(_sourceDir.FullName, templateSrcDirectory));
                foreach (DirectoryInfo subDirectory in sourceDirectory.GetDirectories())
                {
                    var templateSubDirectory = Path.Combine(templateSrcDirectory, subDirectory.Name);
                    ExtractTemplatesFromDirectory(templateSubDirectory, new DirectoryInfo(Path.Combine(destinationDirectroy.FullName, subDirectory.Name)), cultures);
                }
            }
        }

        internal void ExtractWtsTemplates(List<string> cultures)
        {
            DirectoryInfo templateDesDirectory = new DirectoryInfo(Path.Combine(_destinationDir.FullName, Routes.WtsTemplatesRootDirPath));
            if (templateDesDirectory.Exists)
                return;
            templateDesDirectory.Create();

            DirectoryInfo projectsDesDirectory = new DirectoryInfo(Path.Combine(templateDesDirectory.FullName, Routes.WtsProjectTypes));
            projectsDesDirectory.Create();

            DirectoryInfo frameworksDesDirectory = new DirectoryInfo(Path.Combine(templateDesDirectory.FullName, Routes.WtsFrameworks));
            frameworksDesDirectory.Create();

            DirectoryInfo templateSrcDirectory = new DirectoryInfo(Path.Combine(_sourceDir.FullName, Routes.WtsTemplatesRootDirPath));
            if (!templateSrcDirectory.Exists)
                throw new DirectoryNotFoundException($"Source directory \"{templateSrcDirectory.FullName}\" not found.");

            string currentName, projectDesFile, frameworkDesFile;
            FileInfo projectSrcFile, frameworkSrcFile;
            var fileContent = new StringBuilder();

            // project
            if (validator.ValidWtsProjectTypes())
            {
                var projectMetadata = JsonConvert.DeserializeObject<List<JObject>>(File.ReadAllText(Path.Combine(templateSrcDirectory.FullName, Routes.WtsProjectTypes + ".json")));
                foreach (string culture in cultures)
                {
                    projectDesFile = Path.Combine(templateDesDirectory.FullName, culture + "." + Routes.WtsProjectTypes + ".json");

                    if (!File.Exists(projectDesFile))
                    {
                        int index = 1;
                        foreach (JObject json in projectMetadata)
                        {
                            currentName = json.GetValue("name").Value<string>();
                            fileContent.Append((index == 1 ? "[\r\n" : (index <= projectMetadata.Count ? ",\r\n" : string.Empty)) + string.Format(Routes.WtsTemplateJsonContent, currentName, json.GetValue("displayName").Value<string>(), json.GetValue("summary").Value<string>()) + (index >= projectMetadata.Count ? "\r\n]" : string.Empty));

                            projectSrcFile = new FileInfo(Path.Combine(templateSrcDirectory.FullName, Routes.WtsProjectTypes, currentName + ".md"));
                            projectSrcFile.CopyTo(Path.Combine(projectsDesDirectory.FullName, culture + "." + currentName + ".md"));
                            index++;
                        }

                        File.WriteAllText(projectDesFile, fileContent.ToString(), Encoding.UTF8);
                        fileContent.Clear();
                    }
                }
            }

            // framework
            if (validator.ValidWtsFrameworks())
            {
                var frameworkMetadata = JsonConvert.DeserializeObject<List<JObject>>(File.ReadAllText(Path.Combine(templateSrcDirectory.FullName, Routes.WtsFrameworks + ".json")));
                foreach (string culture in cultures)
                {
                    frameworkDesFile = Path.Combine(templateDesDirectory.FullName, culture + "." + Routes.WtsFrameworks + ".json");
                    if (!File.Exists(frameworkDesFile))
                    {
                        int index = 1;
                        foreach (JObject json in frameworkMetadata)
                        {
                            currentName = json.GetValue("name").Value<string>();
                            fileContent.Append((index == 1 ? "[\r\n" : (index <= frameworkMetadata.Count ? ",\r\n" : string.Empty)) + string.Format(Routes.WtsTemplateJsonContent, currentName, json.GetValue("displayName").Value<string>(), json.GetValue("summary").Value<string>()) + (index >= frameworkMetadata.Count ? "\r\n]" : string.Empty));
                            frameworkSrcFile = new FileInfo(Path.Combine(templateSrcDirectory.FullName, Routes.WtsFrameworks, currentName + ".md"));
                            frameworkSrcFile.CopyTo(Path.Combine(frameworksDesDirectory.FullName, culture + "." + currentName + ".md"));
                            index++;
                        }
                        File.WriteAllText(frameworkDesFile, fileContent.ToString(), Encoding.UTF8);
                        fileContent.Clear();
                    }
                }
            }
        }

        internal void ExtractResourceFiles(List<string> cultures)
        {
            foreach (string directory in Routes.ResoureceDirectories)
            {
                DirectoryInfo resourceDesDirectory = new DirectoryInfo(Path.Combine(_destinationDir.FullName, "code\\src", directory, "Resources"));
                if (!resourceDesDirectory.Exists)
                    resourceDesDirectory.Create();

                string resDirectory = Path.Combine("code\\src", directory, "Resources");
                DirectoryInfo resourceSrcDirectory = new DirectoryInfo(Path.Combine(_sourceDir.FullName, resDirectory));
                if (!resourceSrcDirectory.Exists)
                    throw new DirectoryNotFoundException($"Source directory \"{resourceSrcDirectory.FullName}\" not found.");

                string resFileName = string.Format(Routes.ResourcesFilePathPattern, "resx");
                FileInfo resourceFile = new FileInfo(Path.Combine(resourceSrcDirectory.FullName, resFileName));

                if (!resourceFile.Exists)
                    throw new FileNotFoundException($"File \"{resourceFile.FullName}\" not found.");

                var resFile = Path.Combine(resDirectory, resFileName);
                if (validator.CheckModifiedFile(resFile))
                {
                    foreach (string culture in cultures)
                    {
                        resourceFile.CopyTo(Path.Combine(resourceDesDirectory.FullName, string.Format(Routes.ResourcesFilePathPattern, culture + ".resx")));
                    }
                }
            }
        }
    }
}
