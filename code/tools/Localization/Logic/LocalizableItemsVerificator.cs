// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Localization
{
    public class LocalizableItemsVerificator
    {
        private DirectoryInfo _sourceDir;
        private IEnumerable<string> _cultures;
        private List<string> _notFoundFiles;

        internal LocalizableItemsVerificator(string sourceDir, IEnumerable<string> cultures)
        {
            _sourceDir = new DirectoryInfo(sourceDir);

            if (!_sourceDir.Exists)
                throw new DirectoryNotFoundException($"Source directory \"{_sourceDir.FullName}\" not found.");

            _cultures = cultures;
            _notFoundFiles = new List<string>();
        }

        internal bool VerificateAllFiles()
        {
            _notFoundFiles.Clear();

            Console.WriteLine("\nVerify vsix");
            VerifyVsix();

            Console.WriteLine("Verify project templates");
            VerifyProjectTemplates();

            Console.WriteLine("Verify command templates");
            VerifyCommandTemplates();

            Console.WriteLine("Verify template pages");
            VerifyTemplatePages();

            Console.WriteLine("Verify template features");
            VerifyTemplateFeatures();

            Console.WriteLine("Verify project types");
            VerifyWtsProjectTypes();

            Console.WriteLine("Verify project frameworks");
            VerifyWtsFrameworks();

            Console.WriteLine("Verify resources");
            VerifyResourceFiles();

            return _notFoundFiles.Any();

        }

        private void VerifyVsix()
        {
            VerifyFile(Routes.VsixRootDirPath, Routes.VsixManifestFile);
            VerifyFilesByCulture(Routes.VsixRootDirPath, Routes.VsixLangDirPattern);
        }

        private void VerifyProjectTemplates()
        {
            VerifyFile(Routes.ProjectTemplatePathCS, Routes.ProjectTemplateFileCS);
            VerifyFilesByCulture(Routes.ProjectTemplatePathCS, Routes.ProjectTemplateFileNamePatternCS);

            VerifyFile(Routes.ProjectTemplatePathVB, Routes.ProjectTemplateFileVB);
            VerifyFilesByCulture(Routes.ProjectTemplatePathVB, Routes.ProjectTemplateFileNamePatternVB);
        }

        private void VerifyCommandTemplates()
        {
            VerifyFile(Routes.CommandTemplateRootDirPath, Routes.RelayCommandFile);

            VerifyFilesByCulture(Routes.CommandTemplateRootDirPath, Routes.RelayCommandFileNamePattern);
            VerifyFilesByCulture(Routes.CommandTemplateRootDirPath, Routes.VspackageFileNamePattern);

        }

        private void VerifyTemplatePages()
        {
            VerifyTemplateItem(Routes.TemplatesPagesPath);
        }

        private void VerifyTemplateFeatures()
        {
            VerifyTemplateItem(Routes.TemplatesFeaturesPath);
        }

        private void VerifyWtsProjectTypes()
        {
            VerifyWtsItem(Routes.WtsProjectTypes);
        }

        private void VerifyWtsFrameworks()
        {
            VerifyWtsItem(Routes.WtsFrameworks);
        }

        private void VerifyResourceFiles()
        {
            foreach (string directory in Routes.ResoureceDirectories)
            {
                VerifyFile(directory, Routes.ResourcesFilePath);
                VerifyFilesByCulture(directory, Routes.ResourcesFilePathPattern);
            }
        }

        private void VerifyFile(string directoryPath, string fileName)
        {
            var principalDirectory = Path.Combine(_sourceDir.FullName, directoryPath);
            var principalFile = new FileInfo(Path.Combine(principalDirectory, fileName));

            if (!principalFile.Exists)
            {
                _notFoundFiles.Add(principalFile.FullName);
                Console.WriteLine(string.Format("\t {0} not found.", principalFile.FullName));
            }
        }

        private void VerifyFilesByCulture(string directory, string filePattern)
        {
            foreach (var culture in _cultures)
            {
                var languageFile = new FileInfo(Path.Combine(_sourceDir.FullName, directory, string.Format(filePattern, culture)));

                if (!languageFile.Exists)
                {
                    _notFoundFiles.Add(languageFile.FullName);
                    Console.WriteLine(string.Format("\t {0} not found.", languageFile.FullName));
                }
            }
        }

        private void VerifyTemplateItem(string directoryPath)
        {
            var directory = new DirectoryInfo(Path.Combine(_sourceDir.FullName, directoryPath));
            var subDirectories = directory.EnumerateDirectories().Select(d => d.Name);

            foreach (var itemTemplate in subDirectories)
            {
                var templateDirectory = Path.Combine(directoryPath, itemTemplate, Routes.TemplateConfigDir);

                VerifyFile(templateDirectory, Routes.TemplateJsonFile);
                VerifyFilesByCulture(templateDirectory, string.Concat("{0}.", Routes.TemplateJsonFile));

                VerifyFile(templateDirectory, Routes.TemplateDescriptionFile);
                VerifyFilesByCulture(templateDirectory, string.Concat("{0}.", Routes.TemplateDescriptionFile));
            }
        }

        private void VerifyWtsItem(string wtsTemplateName)
        {
            var fileName = string.Concat(wtsTemplateName, ".json");

            VerifyFile(Routes.WtsTemplatesRootDirPath, fileName);
            VerifyFilesByCulture(Routes.WtsTemplatesRootDirPath, string.Concat("{0}.", fileName));

            var filePath = Path.Combine(_sourceDir.FullName, Routes.WtsTemplatesRootDirPath, fileName);
            var fileContent = File.ReadAllText(filePath);
            var content = JsonConvert.DeserializeObject<List<JObject>>(fileContent);
            var wtsItems = content.Select(json => json.GetValue("name").Value<string>());

            var wtsItemDirectory = Path.Combine(Routes.WtsTemplatesRootDirPath, wtsTemplateName);

            foreach (var wtsItem in wtsItems)
            {
                var itemFileName = string.Concat(wtsItem, ".md");
                VerifyFile(wtsItemDirectory, itemFileName);
                VerifyFilesByCulture(wtsItemDirectory, string.Concat("{0}.", itemFileName));
            }
        }
    }
}
