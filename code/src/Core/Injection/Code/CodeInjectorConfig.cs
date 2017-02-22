
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Templates.Core.Injection.Code
{
    public class CodeInjectorConfig
    {
        public string[] usings { get; set; }
        public string path { get; set; }
        public string before { get; set; }
        public string content { get; set; }
    }
}
