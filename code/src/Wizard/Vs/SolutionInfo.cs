using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Templates.Wizard.Vs
{
    public class SolutionInfo
    {
        public SolutionInfo(string directory, string name, string fileName, string templateCategory)
        {
            Directory = directory;
            //ProjectDirectory = projectDirectory;
            Name = name;
            FileName = fileName;
            TemplateCategory = templateCategory;
            FullName = Path.Combine(directory, fileName);

        }
        public SolutionInfo(Dictionary<string, string> replacements)
        {

            DirectoryInfo di = new DirectoryInfo(replacements["$destinationdirectory$"]);
            string solDir = di.Parent.FullName;

            Directory = solDir;
            ProjectDirectory = di.FullName;
            Name = replacements["$safeprojectname$"];
            FileName = replacements["$safeprojectname$"] + ".sln";
            TemplateCategory = replacements["$uwptemplates.category$"];
            FullName = Path.Combine(solDir, replacements["$safeprojectname$"] + ".sln");
        }
        public string Name { get; set; }
        public string Directory { get; set; }
        public string ProjectDirectory { get; set; }
        public string FileName { get; set; }
        public string TemplateCategory { get; set; }
        public string FullName { get; set; }

    }
}

