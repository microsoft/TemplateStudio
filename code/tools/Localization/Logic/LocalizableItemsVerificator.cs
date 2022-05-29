// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Resources;
using System.Text.RegularExpressions;
using Localization.Extensions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

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
            Execute(VerifyItemTemplates, "Verifying item templates");
            Execute(VerifyCommandTemplates, "Verifying command templates");
            Execute(VerifyTemplatePages, "Verifying template pages");
            Execute(VerifyTemplateFeatures, "Verifying template features");
            Execute(VerifyTemplateServices, "Verifying template services");
            Execute(VerifyTemplateTesting, "Verifying template testing");
            Execute(VerifyTSProjectTypes, "Verifying project types");
            Execute(VerifyTSFrameworks, "Verifying project frameworks");
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
            VerifyVisualStudioTemplate(Routes.VSProjectTemplatePaths);
        }

        private void VerifyItemTemplates()
        {
            VerifyVisualStudioTemplate(Routes.VSItemTemplatePaths);
        }

        private void VerifyCommandTemplates()
        {
            VerifyFile(Routes.CommandTemplateRootDirPath, Routes.RelayCommandFile);

            VerifyFilesByCulture(Routes.CommandTemplateRootDirPath, Routes.RelayCommandFileNamePattern);
            VerifyFilesByCulture(Routes.CommandTemplateRootDirPath, Routes.VspackageFileNamePattern);
        }

        private void VerifyTemplatePages()
        {
            VerifyTemplateItem(Routes.TemplatesPagesPatternPath);
        }

        private void VerifyTemplateFeatures()
        {
            VerifyTemplateItem(Routes.TemplatesFeaturesPatternPath);
        }

        private void VerifyTemplateServices()
        {
            VerifyTemplateItem(Routes.TemplatesServicesPatternPath);
        }

        private void VerifyTemplateTesting()
        {
            VerifyTemplateItem(Routes.TemplatesTestingPatternPath);
        }

        private void VerifyTSProjectTypes()
        {
            VerifyTSItem(Routes.WtsProjectTypes);
        }

        private void VerifyTSFrameworks()
        {
            VerifyTSItem(Routes.WtsFrameworks);
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

        private void VerifyVisualStudioTemplate((string Path, string FileName, string FileNamePattern)[] templates)
        {
            foreach (var template in templates)
            {
                VerifyFile(template.Path, template.FileName);
                if (!string.IsNullOrEmpty(template.FileNamePattern))
                {
                    VerifyFilesByCulture(template.Path, template.FileNamePattern);
                }
            }
        }

        private void VerifyFile(string directoryPath, string fileName)
        {
            var principalDirectory = Path.Combine(_sourceDir.FullName, directoryPath);
            var file = new FileInfo(Path.Combine(principalDirectory, fileName));

            if (!file.Exists)
            {
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
                    _errors.Add(string.Format("{0} not found.", languageFile.FullName));
                }
            }
        }

        private void VerifyTemplateItem(string patternPath)
        {
            var templatesDirectories = GetPlatformsTemplatesDirectory(patternPath).ToList();

            foreach (var itemTemplate in templatesDirectories)
            {
                var templateDirectory = Path.Combine(itemTemplate, Routes.TemplateConfigDir);
                VerifyFile(templateDirectory, Routes.TemplateJsonFile);

                VerifyFilesByCulture(templateDirectory, string.Concat("{0}.", Routes.TemplateJsonFile));

                VerifyFile(templateDirectory, Routes.TemplateDescriptionFile);
                VerifyFilesByCulture(templateDirectory, string.Concat("{0}.", Routes.TemplateDescriptionFile));
            }
        }

        private void VerifyTSItem(string wtsTemplateName)
        {
            var fileName = string.Concat(wtsTemplateName, ".json");
            foreach (string platform in Routes.TemplatesPlatforms)
            {
                var catalogDir = Path.Combine(Routes.TemplatesRootDirPath, platform, Routes.CatalogPath);

                VerifyFile(catalogDir, fileName);
                VerifyFilesByCulture(catalogDir, string.Concat("{0}.", fileName));

                var filePath = Path.Combine(_sourceDir.FullName, catalogDir, fileName);
                var fileContent = File.ReadAllText(filePath);
                var content = JsonConvert.DeserializeObject<List<JObject>>(fileContent);
                var wtsItems = content.Select(json => json.GetValue("name", StringComparison.Ordinal).Value<string>());

                var wtsItemDirectory = Path.Combine(catalogDir, wtsTemplateName);

                foreach (var wtsItem in wtsItems)
                {
                    var itemFileName = string.Concat(wtsItem, ".md");
                    VerifyFile(wtsItemDirectory, itemFileName);
                    VerifyFilesByCulture(wtsItemDirectory, string.Concat("{0}.", itemFileName));
                }
            }
        }

        private void VerifyResourceContent(string directory, string fileName)
        {
            var resxFile = Path.Combine(_sourceDir.FullName, directory, fileName);
            var resources = ResourcesExtensions.GetResourcesByFile(resxFile);

            foreach (var culture in _cultures)
            {
                var cultureFile = new FileInfo(Path.Combine(_sourceDir.FullName, directory, string.Format(Routes.ResourcesFilePathPattern, culture)));

                if (cultureFile.Exists)
                {
                    var cultureResources = ResourcesExtensions.GetResourcesByFile(cultureFile.FullName);
                    VerifyResourceValues(resources.Keys, cultureResources.Keys, resxFile, cultureFile.FullName);
                    VerifyResourcesFormat(resources, cultureResources, resxFile, cultureFile.FullName);
                }
            }
        }

        private void VerifyResourceValues(IEnumerable<string> originalValues, IEnumerable<string> cultureValues, string resxFile, string cultureFile)
        {
            originalValues.Except(cultureValues)
                        .ToList()
                        .ForEach(name =>
                        _errors.Add(string.Format("Missing resource: {0} not contain \"{1}\" resource name", cultureFile, name)));

            cultureValues.Except(originalValues)
                .ToList()
                .ForEach(name =>
                _warnings.Add(string.Format("Missing resource: {0} contain \"{1}\" resource name but not in {2}", cultureFile, name, resxFile)));
        }

        private void VerifyResourcesFormat(Dictionary<string, ResxItem> resources, Dictionary<string, ResxItem> cultureResources, string resxFile, string cultureFile)
        {
            string pattern = @"([.^{^}]*(?<p>{\d+}))+";

            var resWithStringFormat = resources.Select(r => new { r.Key, Regex.Matches(r.Value.Text, pattern).Count });
            var resCultureWithStringFormat = cultureResources.Select(r => new { r.Key, Regex.Matches(r.Value.Text, pattern).Count });
            var resWithDistinctFormats = resWithStringFormat.Where(r => resCultureWithStringFormat.Any(c => c.Key == r.Key && c.Count != r.Count));

            foreach (var res in resWithDistinctFormats)
            {
                _errors.Add($"Format Error: {cultureFile} contains distinct string format that default in {res.Key}.");
            }
        }

        private void Execute(Action action, string actionInfo)
        {
            Console.WriteLine();
            Console.Write(actionInfo);

            _errors.Clear();
            action.Invoke();

            if (_errors.Any())
            {
                _verificationResult = false;
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

        private IEnumerable<string> GetPlatformsTemplatesDirectory(string patternPath)
        {
            foreach (string platform in Routes.TemplatesPlatforms)
            {
                var baseDir = Path.Combine(Routes.TemplatesRootDirPath, platform, patternPath);
                var templatesDir = new DirectoryInfo(Path.Combine(_sourceDir.FullName, baseDir));

                if (templatesDir.Exists)
                {
                    foreach (var directory in templatesDir.GetDirectories())
                    {
                        yield return Path.Combine(baseDir, directory.Name);
                    }
                }
            }
        }
    }
}
