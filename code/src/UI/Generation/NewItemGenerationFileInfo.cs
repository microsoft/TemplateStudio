using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Templates.UI
{
    public class NewItemGenerationFileInfo
    {
        public string Name { get; set; }

        public string NewItemGenerationFilePath { get; set; }

        public string ProjectFilePath { get; set; }

        public Dictionary<int, IEnumerable<string>> MergeSnippets { get; set; } 

        public NewItemGenerationFileInfo(string name, string newItemGenerationFilePath, string projectFilePath)
        {
            Name = name;
            NewItemGenerationFilePath = newItemGenerationFilePath;
            ProjectFilePath = projectFilePath;
        }
    }
}
