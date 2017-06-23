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
        private const string sourceDirNamePattern = "CSharp.UWP.2017.Solution";
        private const string sourceFileNamePattern = "CSharp.UWP.VS2017.Solution";
        private const string destinationDirNamePattern = "CSharp.UWP.2017.{0}.Solution";
        private const string destinationFileNamePattern = "CSharp.UWP.VS2017.{0}.Solution";

        internal ProjectTemplateGenerator(string sourceDirPath, string destinationDirPath)
        {
            this.sourceDir = new DirectoryInfo(sourceDirPath);
            if (!this.sourceDir.Exists)
                throw new DirectoryNotFoundException($"Source directory \"{sourceDirPath}\" not found.");
            if (this.sourceDir.Name.ToLower() != sourceDirNamePattern.ToLower())
                throw new Exception($"Source directory \"{this.sourceDir.Name}\" is not valid. Directory name should be \"{sourceDirNamePattern}\".");
            this.destinationDir = new DirectoryInfo(destinationDirPath);
            if (!this.destinationDir.Exists)
                this.destinationDir.Create();
        }

        internal bool GenerateProjectTemplate(string culture)
        {
            DirectoryInfo templateDirectory = new DirectoryInfo(Path.Combine(this.destinationDir.FullName, string.Format(destinationDirNamePattern, culture)));
            if (templateDirectory.Exists)
                return false;
            templateDirectory.Create();
            this.CopyDirectory(this.sourceDir, templateDirectory);
            this.LocalizeCsprojFile(culture, templateDirectory);
            return true;
        }

        private void LocalizeCsprojFile(string culture, DirectoryInfo templateDirectory)
        {
            string csprojFilePath = Path.Combine(templateDirectory.FullName, sourceFileNamePattern + ".csproj");
            FileInfo csprojFile = new FileInfo(csprojFilePath);
            if (!csprojFile.Exists)
                throw new FileNotFoundException($"File \"{csprojFilePath}\" not found.");
            XmlDocument xmlProjFile = XmlUtility.LoadXmlFile(csprojFilePath);
            XmlNode node = XmlUtility.GetNode(xmlProjFile, "CodeAnalysisRuleSet");
            XmlUtility.InsertNewNodeAfter(node, "CreateVsixContainer", "False");
            XmlUtility.InsertNewNodeAfter(node, "CopyVsixExtensionFiles", "False");
            XmlUtility.SetNodeText(xmlProjFile, "ProjectGuid", Guid.NewGuid().ToString("B"));
            XmlUtility.AppendNewChildNode(xmlProjFile, "VSTemplate", "Culture", new CultureInfo(culture).LCID.ToString());
            xmlProjFile.Save(Path.Combine(templateDirectory.FullName, string.Format(destinationFileNamePattern, culture) + ".csproj"));
            csprojFile.Delete();
        }

        private void CopyDirectory(DirectoryInfo sourceDirectory, DirectoryInfo destinationDirectory)
        {
            foreach (FileInfo file in sourceDirectory.GetFiles())
            {
                file.CopyTo(Path.Combine(destinationDirectory.FullName, file.Name));
            }
            DirectoryInfo tmpDir;
            foreach (DirectoryInfo directory in sourceDirectory.GetDirectories())
            {
                tmpDir = new DirectoryInfo(Path.Combine(destinationDirectory.FullName, directory.Name));
                if (!tmpDir.Exists)
                    tmpDir.Create();
                this.CopyDirectory(directory, tmpDir);
            }
        }
    }
}
