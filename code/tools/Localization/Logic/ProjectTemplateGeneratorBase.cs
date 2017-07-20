// ******************************************************************
// Copyright (c) Microsoft. All rights reserved.
// This code is licensed under the MIT License (MIT).
// THE CODE IS PROVIDED “AS IS”, WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED,
// INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.
// IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
// DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT,
// TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH
// THE CODE OR THE USE OR OTHER DEALINGS IN THE CODE.
// ******************************************************************

using System;
using System.Collections.Generic;
using System.IO;

namespace Localization
{
    abstract class ProjectTemplateGeneratorBase
    {
        private DirectoryInfo sourceDir;
        private DirectoryInfo destinationDir;

        protected virtual string sourceDirRelPath { get; }
        protected virtual string sourceDirNamePattern { get; }
        protected virtual string sourceFileNamePattern { get; }
        protected virtual string destinationDirNamePattern { get; }
        protected virtual string destinationFileNamePattern { get; }

        internal ProjectTemplateGeneratorBase(string sourceDirPath, string destinationDirPath)
        {
            sourceDirPath = Path.Combine(sourceDirPath + sourceDirRelPath);
            this.sourceDir = new DirectoryInfo(sourceDirPath);

            if (!this.sourceDir.Exists)
            {
                throw new DirectoryNotFoundException($"Source directory \"{sourceDirPath}\" not found.");
            }

            if (this.sourceDir.Name.ToLower() != sourceDirNamePattern.ToLower())
            {
                throw new Exception($"Source directory \"{this.sourceDir.Name}\" is not valid. Directory name should be \"{sourceDirNamePattern}\".");
            }

            this.destinationDir = new DirectoryInfo(destinationDirPath);

            if (!this.destinationDir.Exists)
            {
                this.destinationDir.Create();
            }
        }

        internal void GenerateProjectTemplates(List<string> cultures)
        {
            DirectoryInfo templateDirectory = new DirectoryInfo(Path.Combine(this.destinationDir.FullName, destinationDirNamePattern));

            if (templateDirectory.Exists)
            {
                return;
            }

            templateDirectory.Create();
            var vstemplateFilePath = Path.Combine(sourceDir.FullName, sourceFileNamePattern);
            var vstemplateFile = new FileInfo(vstemplateFilePath);

            if (!vstemplateFile.Exists)
            {
                throw new FileNotFoundException($"File \"{vstemplateFilePath}\" not found.");
            }

            foreach (string culture in cultures)
            {
                vstemplateFile.CopyTo(Path.Combine(templateDirectory.FullName, string.Format(destinationFileNamePattern, culture)), false);
            }
        }
    }
}
