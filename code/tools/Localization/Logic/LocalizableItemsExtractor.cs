// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;
using System.IO;
using System.Xml;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Localization
{
    internal class LocalizableItemsExtractor
    {
        private DirectoryInfo sourceDir;
        private DirectoryInfo destinationDir;

        private const string projectTemplateFileNamePattern = "CSharp.UWP.VS2017.Solution.{0}.vstemplate";
        private const string projectTemplateDirNamePattern = "CSharp.UWP.2017.Solution";
        private const string projectTemplateRootDirPath = "code\\src\\ProjectTemplates";

        private const string commandTemplateRootDirPath = "code\\src\\Installer.2017\\Commands";
        private const string relayCommandFileNamePattern = "RelayCommandPackage.{0}.vsct";
        private const string vspackageFileNamePattern = "VSPackage.{0}.resx";

        private string[] rightClickMdsFolders = new string[] { "_composition", "Features", "Pages" };
        private const string rightClickFileSearchPattern = "*postaction.md";

        private const string templatesRootDirPath = "templates";
        private const string templateDescriptionFile = "description.md";
        private const string templateJsonFile = "template.json";

        private const string wtsTemplatesRootDirPath = "templates\\_catalog";
        private const string wtsProjectTypes = "projectTypes";
        private const string wtsFrameworks = "frameworks";

        private const string vsixRootDirPath = "code\\src\\Installer.2017";
        private const string vsixManifestFile = "source.extension.vsixmanifest";
        private const string vsixLangpackFile = "Extension.vsixlangpack";
        private const string vsixLangpackContent = @"<?xml version=""1.0"" encoding=""utf-8"" ?>  
<VsixLanguagePack Version = ""1.0.0"" xmlns=""http://schemas.microsoft.com/developer/vsx-schema-lp/2010"" >  
  <LocalizedName>{0}</LocalizedName>  
  <LocalizedDescription>{1}</LocalizedDescription>  
  <License>Content\EULA.{2}.rtf</License>
  <MoreInfoUrl>https://github.com/Microsoft/WindowsTemplateStudio/</MoreInfoUrl>  
</VsixLanguagePack> ";
        private const string templateEngineJsonContent = @"{{
    ""author"": ""{0}"",
    ""name"": ""{1}"",
    ""description"": ""{2}"",
    ""identity"": ""{3}""
}}";
        private const string wtsTemplateJsonContent = @"    {{
        ""name"": ""{0}"",
        ""displayName"": ""{1}"",
        ""summary"": ""{2}""
    }}";

        private const string resourcesFilePathPattern = "StringRes.{0}";
        private string[] resoureceDirectories = new string[] { "Core", "Installer.2017", "UI" };

        internal LocalizableItemsExtractor(string sourceDirPath, string destinationDirPath)
        {
            sourceDir = new DirectoryInfo(sourceDirPath);
            if (!sourceDir.Exists)
                throw new DirectoryNotFoundException($"Source directory \"{sourceDirPath}\" not found.");

            destinationDir = new DirectoryInfo(destinationDirPath);
            if (!destinationDir.Exists)
                destinationDir.Create();
        }

        internal void ExtractVsix(List<string> cultures)
        {
            DirectoryInfo vsixDesDirectory = new DirectoryInfo(Path.Combine(destinationDir.FullName, vsixRootDirPath));

            if (vsixDesDirectory.Exists)
                return;

            vsixDesDirectory.Create();
            vsixDesDirectory.CreateSubdirectory("Content");
            DirectoryInfo vsixSrcDirectory = new DirectoryInfo(Path.Combine(sourceDir.FullName, vsixRootDirPath));

            if (!vsixSrcDirectory.Exists)
                throw new DirectoryNotFoundException($"Source directory \"{vsixSrcDirectory.FullName}\" not found.");

            FileInfo manifiestFile = new FileInfo(Path.Combine(vsixSrcDirectory.FullName, vsixManifestFile));
            if (!manifiestFile.Exists)
                throw new FileNotFoundException($"File \"{manifiestFile.FullName}\" not found.");

            FileInfo eulaFile = new FileInfo(Path.Combine(vsixSrcDirectory.FullName, "Content\\EULA.rtf"));
            if (!eulaFile.Exists)
                throw new FileNotFoundException($"File \"{eulaFile.FullName}\" not found.");

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
                langpackFile = new FileInfo(Path.Combine(vsixLocDesDirectory.FullName, vsixLangpackFile));

                if (!langpackFile.Exists)
                {
                    using (TextWriter writer = langpackFile.CreateText())
                    {
                        writer.Write(string.Format(vsixLangpackContent, localizedName, localizedDescription, culture));
                    }
                }
                eulaFile.CopyTo(Path.Combine(vsixDesDirectory.FullName, $"Content\\EULA.{culture}.rtf"));
            }
        }

        internal void ExtractProjectTemplates(List<string> cultures)
        {
            DirectoryInfo templateDesDirectory = new DirectoryInfo(Path.Combine(this.destinationDir.FullName, projectTemplateRootDirPath, projectTemplateDirNamePattern));
            if (templateDesDirectory.Exists)
                return;
            templateDesDirectory.Create();
            DirectoryInfo templateSrcDirectory = new DirectoryInfo(Path.Combine(this.sourceDir.FullName, projectTemplateRootDirPath, projectTemplateDirNamePattern));
            if (!templateSrcDirectory.Exists)
                throw new DirectoryNotFoundException($"Source directory \"{templateSrcDirectory.FullName}\" not found.");
            foreach (string culture in cultures)
            {
                FileInfo file = new FileInfo(Path.Combine(templateSrcDirectory.FullName, string.Format(projectTemplateFileNamePattern, culture)));
                if (!file.Exists)
                    throw new FileNotFoundException($"File \"{file.FullName}\" not found.");
                file.CopyTo(Path.Combine(templateDesDirectory.FullName, string.Format(projectTemplateFileNamePattern, culture)));
            }
        }

        internal void ExtractCommandTemplates(List<string> cultures)
        {
            DirectoryInfo templateDesDirectory = new DirectoryInfo(Path.Combine(this.destinationDir.FullName, commandTemplateRootDirPath));
            if (templateDesDirectory.Exists)
                return;
            templateDesDirectory.Create();
            DirectoryInfo templateSrcDirectory = new DirectoryInfo(Path.Combine(this.sourceDir.FullName, commandTemplateRootDirPath));
            if (!templateSrcDirectory.Exists)
                throw new DirectoryNotFoundException($"Source directory \"{templateSrcDirectory.FullName}\" not found.");

            this.LocalizeFileType(templateSrcDirectory, templateDesDirectory, relayCommandFileNamePattern, cultures);
            this.LocalizeFileType(templateSrcDirectory, templateDesDirectory, vspackageFileNamePattern, cultures);
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
            DirectoryInfo templateDesDirectory = new DirectoryInfo(Path.Combine(this.destinationDir.FullName, templatesRootDirPath, templateType));
            if (!templateDesDirectory.Exists)
                templateDesDirectory.Create();
            DirectoryInfo templateSrcDirectory = new DirectoryInfo(Path.Combine(this.sourceDir.FullName, templatesRootDirPath, templateType));
            if (!templateSrcDirectory.Exists)
                throw new DirectoryNotFoundException($"Source directory \"{templateSrcDirectory.FullName}\" not found.");
            ExtractTemplatesFromDirectory(templateSrcDirectory, templateDesDirectory, cultures);
        }

        private void ExtractTemplatesFromDirectory(DirectoryInfo sourceDirectory, DirectoryInfo destinationDirectroy, List<string> cultures)
        {
            FileInfo file = new FileInfo(Path.Combine(sourceDirectory.FullName, templateJsonFile));
            if (file.Exists)
            {
                FileInfo srcFile = new FileInfo(Path.Combine(sourceDirectory.FullName, templateDescriptionFile));
                var metadata = JsonConvert.DeserializeObject<JObject>(File.ReadAllText(file.FullName));
                if (!destinationDirectroy.Exists)
                    destinationDirectroy.Create();
                foreach (string culture in cultures)
                {
                    FileInfo desFile = new FileInfo(Path.Combine(destinationDirectroy.FullName, culture + "." + templateJsonFile));
                    if (!desFile.Exists)
                    {
                        using (TextWriter writer = desFile.CreateText())
                        {
                            writer.Write(string.Format(templateEngineJsonContent, metadata.GetValue("author").Value<string>(), metadata.GetValue("name").Value<string>(), metadata.GetValue("description").Value<string>(), metadata.GetValue("identity").Value<string>()));
                        }
                    }
                    desFile = new FileInfo(Path.Combine(destinationDirectroy.FullName, culture + "." + templateDescriptionFile));
                    if (!desFile.Exists)
                    {
                        srcFile.CopyTo(desFile.FullName);
                    }
                }
            }
            else
            {
                foreach (DirectoryInfo subDirectory in sourceDirectory.GetDirectories())
                {
                    ExtractTemplatesFromDirectory(subDirectory, new DirectoryInfo(Path.Combine(destinationDirectroy.FullName, subDirectory.Name)), cultures);
                }
            }
        }

        internal void ExtractWtsTemplates(List<string> cultures)
        {
            DirectoryInfo templateDesDirectory = new DirectoryInfo(Path.Combine(this.destinationDir.FullName, wtsTemplatesRootDirPath));
            if (templateDesDirectory.Exists)
                return;
            templateDesDirectory.Create();
            DirectoryInfo projectsDesDirectory = new DirectoryInfo(Path.Combine(templateDesDirectory.FullName, wtsProjectTypes));
            projectsDesDirectory.Create();
            DirectoryInfo frameworksDesDirectory = new DirectoryInfo(Path.Combine(templateDesDirectory.FullName, wtsFrameworks));
            frameworksDesDirectory.Create();
            DirectoryInfo templateSrcDirectory = new DirectoryInfo(Path.Combine(this.sourceDir.FullName, wtsTemplatesRootDirPath));
            if (!templateSrcDirectory.Exists)
                throw new DirectoryNotFoundException($"Source directory \"{templateSrcDirectory.FullName}\" not found.");
            var projectMetadata = JsonConvert.DeserializeObject<List<JObject>>(File.ReadAllText(Path.Combine(templateSrcDirectory.FullName, wtsProjectTypes + ".json")));
            var frameworkMetadata = JsonConvert.DeserializeObject<List<JObject>>(File.ReadAllText(Path.Combine(templateSrcDirectory.FullName, wtsFrameworks + ".json")));
            string currentName;
            FileInfo projectDesFile, projectSrcFile, frameworkDesFile, frameworkSrcFile;
            foreach (string culture in cultures)
            {
                projectDesFile = new FileInfo(Path.Combine(templateDesDirectory.FullName, culture + "." + wtsProjectTypes + ".json"));
                if (!projectDesFile.Exists)
                {
                    using (TextWriter writer = projectDesFile.CreateText())
                    {
                        int index = 1;
                        foreach (JObject json in projectMetadata)
                        {
                            currentName = json.GetValue("name").Value<string>();
                            writer.Write((index == 1 ? "[\r\n" : (index <= projectMetadata.Count ? ",\r\n" : string.Empty)) + string.Format(wtsTemplateJsonContent, currentName, json.GetValue("displayName").Value<string>(), json.GetValue("summary").Value<string>()) + (index >= projectMetadata.Count ? "\r\n]" : string.Empty));
                            projectSrcFile = new FileInfo(Path.Combine(templateSrcDirectory.FullName, wtsProjectTypes, currentName + ".md"));
                            projectSrcFile.CopyTo(Path.Combine(projectsDesDirectory.FullName, culture + "." + currentName + ".md"));
                            index++;
                        }
                    }
                }
                frameworkDesFile = new FileInfo(Path.Combine(templateDesDirectory.FullName, culture + "." + wtsFrameworks + ".json"));
                if (!frameworkDesFile.Exists)
                {
                    using (TextWriter writer = frameworkDesFile.CreateText())
                    {
                        int index = 1;
                        foreach (JObject json in frameworkMetadata)
                        {
                            currentName = json.GetValue("name").Value<string>();
                            writer.Write((index == 1 ? "[\r\n" : (index <= projectMetadata.Count ? ",\r\n" : string.Empty)) + string.Format(wtsTemplateJsonContent, currentName, json.GetValue("displayName").Value<string>(), json.GetValue("summary").Value<string>()) + (index >= projectMetadata.Count ? "\r\n]" : string.Empty));
                            frameworkSrcFile = new FileInfo(Path.Combine(templateSrcDirectory.FullName, wtsFrameworks, currentName + ".md"));
                            frameworkSrcFile.CopyTo(Path.Combine(frameworksDesDirectory.FullName, culture + "." + currentName + ".md"));
                            index++;
                        }
                    }
                }
            }
        }

        internal void ExtractResourceFiles(List<string> cultures)
        {
            foreach (string directory in resoureceDirectories)
            {
                DirectoryInfo resourceDesDirectory = new DirectoryInfo(Path.Combine(this.destinationDir.FullName, "code\\src", directory, "Resources"));
                if (!resourceDesDirectory.Exists)
                    resourceDesDirectory.Create();
                DirectoryInfo resourceSrcDirectory = new DirectoryInfo(Path.Combine(this.sourceDir.FullName, "code\\src", directory, "Resources"));
                if (!resourceSrcDirectory.Exists)
                    throw new DirectoryNotFoundException($"Source directory \"{resourceSrcDirectory.FullName}\" not found.");
                FileInfo resourceFile = new FileInfo(Path.Combine(resourceSrcDirectory.FullName, string.Format(resourcesFilePathPattern, "resx")));
                if (!resourceFile.Exists)
                    throw new FileNotFoundException($"File \"{resourceFile.FullName}\" not found.");
                foreach (string culture in cultures)
                {
                    resourceFile.CopyTo(Path.Combine(resourceDesDirectory.FullName, string.Format(resourcesFilePathPattern, culture + ".resx")));
                }
            }
        }
    }
}
