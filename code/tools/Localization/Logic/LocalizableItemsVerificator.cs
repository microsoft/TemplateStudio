// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Resources;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Localization.Extensions;

namespace Localization
{
    public class LocalizableItemsVerificator
    {
        private DirectoryInfo _sourceDir;
        private IEnumerable<string> _cultures;
        private List<string> _errors;
        private List<string> _warnings;

        private bool _verificationResult;

        internal LocalizableItemsVerificator(string sourceDir, IEnumerable<string> cultures)
        {
            _sourceDir = new DirectoryInfo(sourceDir);

            if (!_sourceDir.Exists)
            {
                throw new DirectoryNotFoundException($"Source directory \"{_sourceDir.FullName}\" not found.");
            }

            _cultures = cultures;
            _errors = new List<string>();
            _warnings = new List<string>();
        }

        internal bool VerificateAllFiles()
        {
            _verificationResult = true;

            Execute(VerifyVsix, "Verifying vsix");
            Execute(VerifyProjectTemplates, "Verifying project templates");
            Execute(VerifyCommandTemplates, "Verifying command templates");
            Execute(VerifyTemplatePages, "Verifying template pages");
            Execute(VerifyTemplateFeatures, "Verifying template features");
            Execute(VerifyWtsProjectTypes, "Verifying project types");
            Execute(VerifyWtsFrameworks, "Verifying project frameworks");
            Execute(VerifyResourceFiles, "Verifying resources");

            return _verificationResult;
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
                VerifyResourceContent(directory, Routes.ResourcesFilePath);
            }
        }

        private void VerifyFile(string directoryPath, string fileName)
        {
            var principalDirectory = Path.Combine(_sourceDir.FullName, directoryPath);
            var file = new FileInfo(Path.Combine(principalDirectory, fileName));

            if (!file.Exists)
            {
                _verificationResult = false;
                _errors.Add(string.Format("{0} not found.", file.FullName));
            }
        }

        private void VerifyFilesByCulture(string directory, string filePattern)
        {
            foreach (var culture in _cultures)
            {
                var languageFile = new FileInfo(Path.Combine(_sourceDir.FullName, directory, string.Format(filePattern, culture)));

                if (!languageFile.Exists)
                {
                    _verificationResult = false;
                    _errors.Add(string.Format("{0} not found.", languageFile.FullName));
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

        private void VerifyResourceContent(string directory, string fileName)
        {
            var resxFile = Path.Combine(_sourceDir.FullName, directory, fileName);
            var resNames = GetResourceNamesByFile(resxFile);

            foreach (var culture in _cultures)
            {
                var languageFile = new FileInfo(Path.Combine(_sourceDir.FullName, directory, string.Format(Routes.ResourcesFilePathPattern, culture)));

                if (languageFile.Exists)
                {
                    var cultureNames = GetResourceNamesByFile(languageFile.FullName);

                    resNames.Except(cultureNames)
                        .ToList()
                        .ForEach(name =>
                        _errors.Add(string.Format("{0} not contain \"{1}\" resource name", languageFile.FullName, name)));

                    cultureNames.Except(resNames)
                        .ToList()
                        .ForEach(name =>
                        _warnings.Add(string.Format("{0} contain \"{1}\" resource name but not in {2}", languageFile.FullName, name, resxFile)));
                }
            }
        }

        private IEnumerable<string> GetResourceNamesByFile(string filePath)
        {
            if (!File.Exists(filePath))
            {
                 return Enumerable.Empty<string>();
            }

            using (var resx = new ResXResourceReader(filePath))
            {
                resx.UseResXDataNodes = true;
                return resx.Cast<DictionaryEntry>().Select(x => x.Key as string);
            }
        }

        private void Execute(Action action, string message)
        {
            Console.WriteLine();
            Console.Write(message);

            _errors.Clear();
            action.Invoke();

            if (_errors.Any())
            {
                ConsoleExt.WriteError(" - ERROR");
            }
            else if (_warnings.Any())
            {
                ConsoleExt.WriteWarning(" - WARNING");
            }
            else
            {
                ConsoleExt.WriteSuccess(" - OK");
            }

            _errors.ToList().ForEach(e => ConsoleExt.WriteError(string.Concat(" - ", e)));
            _warnings.ToList().ForEach(e => ConsoleExt.WriteWarning(string.Concat(" - ", e)));
        }
    }
}
