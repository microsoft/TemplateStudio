// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.IO;

namespace Localization
{
    internal class RightClickCommandGenerator
    {
        private DirectoryInfo _sourceDir;
        private DirectoryInfo _destinationDir;

        private const string SourceDirNamePattern = "Commands";
        private const string SourceDirRelPath = @"\code\src\Installer\Commands";
        private const string SourceFileRelayCommandPattern = "RelayCommandPackage.en-US.vsct";
        private const string SourceFileVSPackagePattern = "VSPackage.en-US.resx";
        private const string DestinationDirNamePattern = "Commands";
        private const string DestinationFileRelayCommandPattern = "RelayCommandPackage.{0}.vsct";
        private const string DestinationFileVSPackagePattern = "VSPackage.{0}.resx";

        internal RightClickCommandGenerator(string sourceDirPath, string destinationDirPath)
        {
            sourceDirPath = Path.Combine(sourceDirPath + SourceDirRelPath);
            _sourceDir = new DirectoryInfo(sourceDirPath);

            if (!_sourceDir.Exists)
            {
                throw new DirectoryNotFoundException($"Source directory \"{sourceDirPath}\" not found.");
            }

            if (_sourceDir.Name.ToUpperInvariant() != SourceDirNamePattern.ToUpperInvariant())
            {
                throw new Exception($"Source directory \"{_sourceDir.Name}\" is not valid. Directory name should be \"{SourceDirNamePattern}\".");
            }

            _destinationDir = new DirectoryInfo(destinationDirPath);
            if (!_destinationDir.Exists)
            {
                _destinationDir.Create();
            }
        }

        internal void GenerateRightClickCommands(List<string> cultures)
        {
            DirectoryInfo templateDirectory = new DirectoryInfo(Path.Combine(_destinationDir.FullName, DestinationDirNamePattern));

            if (templateDirectory.Exists)
            {
                return;
            }

            templateDirectory.Create();

            CreateLocalizedFiles(templateDirectory, SourceFileRelayCommandPattern, DestinationFileRelayCommandPattern, cultures);
            CreateLocalizedFiles(templateDirectory, SourceFileVSPackagePattern, DestinationFileVSPackagePattern, cultures);
        }

        private void CreateLocalizedFiles(DirectoryInfo templateDirectory, string sourceFilePattern, string destFilePattern, List<string> cultures)
        {
            string vstemplateFilePath = Path.Combine(_sourceDir.FullName, sourceFilePattern);
            FileInfo vstemplateFile = new FileInfo(vstemplateFilePath);

            if (!vstemplateFile.Exists)
            {
                throw new FileNotFoundException($"File \"{vstemplateFilePath}\" not found.");
            }

            foreach (string culture in cultures)
            {
                vstemplateFile.CopyTo(Path.Combine(templateDirectory.FullName, string.Format(destFilePattern, culture)), false);
            }
        }
    }
}
