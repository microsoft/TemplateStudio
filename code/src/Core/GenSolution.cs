using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Templates.Core
{
    public class GenSolution
    {
        public string ProjectName { get; protected set; }
        public string OutputPath { get; protected set; }

        private GenSolution(string projectName, string outputPath)
        {
            ProjectName = projectName;
            OutputPath = outputPath;
        }

        public static GenSolution Create(string name, string location, string solutionName = null)
        {
            return new GenSolution(name, Path.Combine(location, name, name));
        }

        public static GenSolution Create(Dictionary<string, string> replacements)
        {
            var destinationDirectory = new DirectoryInfo(replacements["$destinationdirectory$"]);

            return new GenSolution(replacements["$safeprojectname$"], destinationDirectory.FullName);
        }
    }
}
