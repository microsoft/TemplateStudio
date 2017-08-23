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
        private DirectoryInfo _sourceDir;
        private DirectoryInfo _destinationDir;

        protected virtual string SourceDirRelPath { get; }
        protected virtual string SourceDirNamePattern { get; }
        protected virtual string SourceFileNamePattern { get; }
        protected virtual string DestinationDirNamePattern { get; }
        protected virtual string DestinationFileNamePattern { get; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        internal ProjectTemplateGeneratorBase(string sourceDirPath, string destinationDirPath)
        {
            sourceDirPath = Path.Combine(sourceDirPath + SourceDirRelPath);
            _sourceDir = new DirectoryInfo(sourceDirPath);

            if (!_sourceDir.Exists)
            {
                throw new DirectoryNotFoundException($"Source directory \"{sourceDirPath}\" not found.");
            }

            if (_sourceDir.Name.ToLower() != SourceDirNamePattern.ToLower())
            {
                throw new Exception($"Source directory \"{_sourceDir.Name}\" is not valid. Directory name should be \"{SourceDirNamePattern}\".");
            }

            _destinationDir = new DirectoryInfo(destinationDirPath);

            if (!_destinationDir.Exists)
            {
                _destinationDir.Create();
            }
        }

        internal void GenerateProjectTemplates(List<string> cultures)
        {
            DirectoryInfo templateDirectory = new DirectoryInfo(Path.Combine(_destinationDir.FullName, DestinationDirNamePattern));

            if (templateDirectory.Exists)
            {
                return;
            }

            templateDirectory.Create();

            string vstemplateFilePath = Path.Combine(_sourceDir.FullName, SourceFileNamePattern);
            FileInfo vstemplateFile = new FileInfo(vstemplateFilePath);

            if (!vstemplateFile.Exists)
            {
                throw new FileNotFoundException($"File \"{vstemplateFilePath}\" not found.");
            }

            foreach (string culture in cultures)
            {
                vstemplateFile.CopyTo(Path.Combine(templateDirectory.FullName, string.Format(DestinationFileNamePattern, culture)), false);
            }
        }
    }
}
