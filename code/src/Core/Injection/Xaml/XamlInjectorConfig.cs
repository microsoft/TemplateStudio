using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Templates.Core.Injection.Xaml
{
    public class XamlInjectorConfig
    {
        public Element[] elements { get; set; }
    }

    public class Element
    {
        public string path { get; set; }
        public string content { get; set; }
        public Attr[] attributes { get; set; }
    }

    public class Attr
    {
        public string name { get; set; }
        public string value { get; set; }
    }
}
