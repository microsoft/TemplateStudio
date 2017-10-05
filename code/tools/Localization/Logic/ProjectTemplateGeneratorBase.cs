// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.IO;

namespace Localization
{
    abstract class ProjectTemplateGeneratorBase
    {
        private ProjectTemplateGeneratorConfig _config;
        private DirectoryInfo _sourceDir;
        private DirectoryInfo _destinationDir;

        public ProjectTemplateGeneratorBase(ProjectTemplateGeneratorConfig config)
        {
            _config = config;
            _sourceDir = new DirectoryInfo(_config.SourceDir);

            if (!_sourceDir.Exists)
            {
                throw new DirectoryNotFoundException($"Source directory \"{_config.SourceDir}\" not found.");
            }

            if (_sourceDir.Name.ToLower() != _config.SourceDirNamePattern.ToLower())
            {
                throw new Exception($"Source directory \"{_sourceDir.Name}\" is not valid. Directory name should be \"{_config.SourceDirNamePattern}\".");
            }

            _destinationDir = new DirectoryInfo(_config.DestinationDirPath);

            if (!_destinationDir.Exists)
            {
                _destinationDir.Create();
            }
        }

        internal void GenerateProjectTemplates(List<string> cultures)
        {
            DirectoryInfo templateDirectory = new DirectoryInfo(Path.Combine(_destinationDir.FullName, _config.DestinationDirNamePattern));

            if (templateDirectory.Exists)
            {
                return;
            }

            templateDirectory.Create();

            string vstemplateFilePath = Path.Combine(_sourceDir.FullName, _config.SourceFileNamePattern);
            FileInfo vstemplateFile = new FileInfo(vstemplateFilePath);

            if (!vstemplateFile.Exists)
            {
                throw new FileNotFoundException($"File \"{vstemplateFilePath}\" not found.");
            }

            foreach (string culture in cultures)
            {
                vstemplateFile.CopyTo(Path.Combine(templateDirectory.FullName, string.Format(_config.DestinationFileNamePattern, culture)), false);
            }
        }
    }
}
