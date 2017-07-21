// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Localization
{
    internal class ProjectTemplateGenerator
    {
        private DirectoryInfo sourceDir;
        private DirectoryInfo destinationDir;
        private const string sourceDirRelPath = @"\code\src\ProjectTemplates\CSharp.UWP.2017.Solution";
        private const string sourceDirNamePattern = "CSharp.UWP.2017.Solution";
        private const string sourceFileNamePattern = "CSharp.UWP.VS2017.Solution.vstemplate";
        private const string destinationDirNamePattern = "CSharp.UWP.2017.Solution";
        private const string destinationFileNamePattern = "CSharp.UWP.VS2017.Solution.{0}.vstemplate";

        internal ProjectTemplateGenerator(string sourceDirPath, string destinationDirPath)
        {
            sourceDirPath = Path.Combine(sourceDirPath + sourceDirRelPath);
            this.sourceDir = new DirectoryInfo(sourceDirPath);
            if (!this.sourceDir.Exists)
                throw new DirectoryNotFoundException($"Source directory \"{sourceDirPath}\" not found.");
            if (this.sourceDir.Name.ToLower() != sourceDirNamePattern.ToLower())
                throw new Exception($"Source directory \"{this.sourceDir.Name}\" is not valid. Directory name should be \"{sourceDirNamePattern}\".");
            this.destinationDir = new DirectoryInfo(destinationDirPath);
            if (!this.destinationDir.Exists)
                this.destinationDir.Create();
        }

        internal void GenerateProjectTemplates(List<string> cultures)
        {
            DirectoryInfo templateDirectory = new DirectoryInfo(Path.Combine(this.destinationDir.FullName, destinationDirNamePattern));
            if (templateDirectory.Exists)
                return;
            templateDirectory.Create();
            string vstemplateFilePath = Path.Combine(sourceDir.FullName, sourceFileNamePattern);
            FileInfo vstemplateFile = new FileInfo(vstemplateFilePath);
            if (!vstemplateFile.Exists)
                throw new FileNotFoundException($"File \"{vstemplateFilePath}\" not found.");
            foreach (string culture in cultures)
            {
                vstemplateFile.CopyTo(Path.Combine(templateDirectory.FullName, string.Format(destinationFileNamePattern, culture)), false);
            }
        }
    }
}
