using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Templates.Core.PostActions.Code
{
    public class CodePostActionConfig
    {
        public string[] usings { get; set; }
        public string path { get; set; }
        public string content { get; set; }
    }
}
