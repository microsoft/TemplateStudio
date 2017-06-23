using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Localization
{
    internal class LocalizableItemsExtractor
    {
        private DirectoryInfo sourceDir;
        private DirectoryInfo destinationDir;
        private const string projectTemplateFileNamePattern = "CSharp.UWP.VS2017.Solution.vstemplate";
        private const string projectTemplateDirNamePattern = "CSharp.UWP.2017.{0}.Solution";
        private const string projectTemplateRootDirPath = "ProjectTemplates";

        internal LocalizableItemsExtractor(string sourceDirPath, string destinationDirPath)
        {
            this.sourceDir = new DirectoryInfo(sourceDirPath);
            if (!this.sourceDir.Exists)
                throw new DirectoryNotFoundException($"Source directory \"{sourceDirPath}\" not found.");
            this.destinationDir = new DirectoryInfo(destinationDirPath);
            if (!this.destinationDir.Exists)
                this.destinationDir.Create();
        }

        internal bool ExtractProjectTemplate(string culture)
        {
            DirectoryInfo templateDesDirectory = new DirectoryInfo(Path.Combine(this.destinationDir.FullName, projectTemplateRootDirPath, string.Format(projectTemplateDirNamePattern, culture)));
            if (templateDesDirectory.Exists)
                return false;
            templateDesDirectory.Create();
            DirectoryInfo templateSrcDirectory = new DirectoryInfo(Path.Combine(this.sourceDir.FullName, projectTemplateRootDirPath, string.Format(projectTemplateDirNamePattern, culture)));
            if (!templateSrcDirectory.Exists)
                throw new DirectoryNotFoundException($"Source directory \"{templateSrcDirectory.FullName}\" not found.");
            FileInfo file = new FileInfo(Path.Combine(templateSrcDirectory.FullName, projectTemplateFileNamePattern));
            if (!file.Exists)
                throw new FileNotFoundException($"File \"{file.FullName}\" not found.");
            file.CopyTo(Path.Combine(templateDesDirectory.FullName, projectTemplateFileNamePattern));
            return true;
        }
    }
}
