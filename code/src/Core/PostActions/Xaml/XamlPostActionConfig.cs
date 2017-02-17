using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Templates.Core.PostActions.Xaml
{
    public class XamlPostActionConfig
    {
        public Element[] elements { get; set; }
    }

    public class Element
    {
        public string path { get; set; }
        public string content { get; set; }
    }
}
