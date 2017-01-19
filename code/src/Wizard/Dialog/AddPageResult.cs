using Microsoft.TemplateEngine.Abstractions;
using Microsoft.Templates.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Microsoft.Templates.Wizard.Dialog
{
    public class AddPageResult
    {
        public ITemplateInfo PageTemplate { get; set; }
        public string PageName { get; set; }
        public string Namespace { get; set; }
    }
}
