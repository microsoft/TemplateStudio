using Microsoft.TemplateEngine.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Templates.UI
{
    public class TemplateSelection
    {        
        public string Name { get; set; }
        public bool IsHome { get; set; }
        public ITemplateInfo Template { get; set; }
    }
}
