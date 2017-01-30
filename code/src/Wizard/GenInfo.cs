using Microsoft.TemplateEngine.Abstractions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Templates.Wizard
{
    public class GenInfo
    {
        public string Name { get; set; }
        public ITemplateInfo Template { get; set; }
        public Dictionary<string, string> Parameters { get; } = new Dictionary<string, string>();
    }
}
