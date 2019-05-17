// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Localization
{
    public class LocalizableItemsGenerator
    {
        private readonly string _path;
        private readonly IEnumerable<string> _cultures;

        public LocalizableItemsGenerator(string path, IEnumerable<string> cultures)
        {
            _path = path;
            _cultures = cultures;
        }

        public void GenerateCatalogProjectTypes()
        {
            GenerateCatalogFiles(Routes.WtsProjectTypes);
            GenerateCatalogSubfolderFile(Routes.WtsProjectTypes);
        }

        public void GenerateCatalogFramework()
        {
            GenerateCatalogFiles(Routes.WtsFrameworks);
            GenerateCatalogSubfolderFile(Routes.WtsFrameworks);
        }

        public void GeneratePages()
        {
            GenerateTemplatesFiles(Routes.TemplatesPagesPatternPath);
        }

        public void GenerateFeatures()
        {
            GenerateTemplatesFiles(Routes.TemplatesFeaturesPatternPath);
        }

        private void GenerateTemplatesFiles(string patternPath)
        {
            var srcDirectory = new DirectoryInfo(Path.Combine(_path, Routes.TemplatesRootDirPath));
            var directories = srcDirectory.GetDirectories(patternPath, SearchOption.AllDirectories);
            var templatesDirectories = directories.SelectMany(d => d.GetDirectories());

            foreach (var directory in templatesDirectories)
            {
                var jsonFile = new FileInfo(Path.Combine(directory.FullName, Routes.TemplateConfigDir, Routes.TemplateJsonFile));
                GenerateTemplateJsonFiles(jsonFile);

                var mdFile = new FileInfo(Path.Combine(directory.FullName, Routes.TemplateConfigDir, Routes.TemplateDescriptionFile));
                GenerateTemplateMdFiles(mdFile);
            }
        }

        private void GenerateTemplateJsonFiles(FileInfo jsonFile)
        {
            if (!jsonFile.Exists)
            {
                return;
            }

            var desDirectory = jsonFile.Directory;
            var metadata = JsonConvert.DeserializeObject<JObject>(File.ReadAllText(jsonFile.FullName));
            var con = new
            {
                author = metadata.GetValue("author", StringComparison.Ordinal).Value<string>(),
                name = metadata.GetValue("name", StringComparison.Ordinal).Value<string>(),
                description = metadata.GetValue("description", StringComparison.Ordinal).Value<string>(),
                identity = metadata.GetValue("identity", StringComparison.Ordinal).Value<string>(),
            };

            foreach (string culture in _cultures)
            {
                string desFile = Path.Combine(desDirectory.FullName, culture + "." + Routes.TemplateJsonFile);

                if (!File.Exists(desFile))
                {
                    string content = JsonConvert.SerializeObject(con, Newtonsoft.Json.Formatting.Indented);
                    File.WriteAllText(desFile, content, Encoding.UTF8);
                    Console.WriteLine($"Generate {desFile}");
                }
            }
        }

        private void GenerateTemplateMdFiles(FileInfo mdFile)
        {
            if (!mdFile.Exists)
            {
                return;
            }

            foreach (string culture in _cultures)
            {
                var desFile = Path.Combine(mdFile.Directory.FullName, culture + "." + Routes.TemplateDescriptionFile);

                if (!File.Exists(desFile))
                {
                    mdFile.CopyTo(desFile, true);
                    Console.WriteLine($"Generate {desFile}");
                }
            }
        }

        private void GenerateCatalogFiles(string fileName)
        {
            var destDirectory2 = new DirectoryInfo(Path.Combine(_path, Routes.WtsTemplatesRootDirPath)).FullName;
            var destDirectory = Path.Combine(_path, Routes.WtsTemplatesRootDirPath);
            var file = new FileInfo(Path.Combine(destDirectory, fileName + ".json"));

            if (file.Exists)
            {
                var fileContent = File.ReadAllText(file.FullName);
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
                    var desFile = Path.Combine(destDirectory, culture + "." + file.Name);
                    if (!File.Exists(desFile))
                    {
                        File.WriteAllText(desFile, data, Encoding.UTF8);
                        Console.WriteLine($"Generate {desFile}");
                    }
                }
            }
        }

        private void GenerateCatalogSubfolderFile(string routeType)
        {
            var srcFile = Path.Combine(_path, Routes.WtsTemplatesRootDirPath, routeType + ".json");
            var fileContent = File.ReadAllText(srcFile);
            var content = JsonConvert.DeserializeObject<List<JObject>>(fileContent);
            var names = content.Select(json => json.GetValue("name", StringComparison.Ordinal).Value<string>());

            foreach (var name in names)
            {
                var destDirectory = Path.Combine(_path, Routes.WtsTemplatesRootDirPath, routeType);
                var mdFile = new FileInfo(Path.Combine(destDirectory, name + ".md"));

                if (mdFile.Exists)
                {
                    foreach (string culture in _cultures)
                    {
                        var desFile = Path.Combine(destDirectory, culture + "." + mdFile.Name);
                        if (!File.Exists(desFile))
                        {
                            mdFile.CopyTo(desFile, true);
                            Console.WriteLine($"Generate {desFile}");
                        }
                    }
                }
            }
        }
    }
}
