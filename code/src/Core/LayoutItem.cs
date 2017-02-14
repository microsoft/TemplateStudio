using Microsoft.TemplateEngine.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Templates.Core
{
    public class LayoutItem
    {
        public string name { get; set; }
        public string templateIdentity { get; set; }
        public bool @readonly { get; set; }
    }
}
