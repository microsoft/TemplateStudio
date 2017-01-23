using Microsoft.TemplateEngine.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Templates.Wizard.Host
{
    //TODO: MOVE THIS SOMEWHERE
    public class TemplateConfig
    {
        public Dictionary<string, string> Parameters { get; set; }
        public ITemplateInfo Info { get; set; }
    }
}
