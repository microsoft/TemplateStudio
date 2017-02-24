using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Globalization;

namespace Microsoft.Templates.Core.Test.PostActions
{
    class Class1
    {
        public string Elements
        {
            get
            {
                return "value";
            }
        }

        public void Main()
        {
            var v0 = "v0 value";
            var v1 = "v1 value";
            var v2 = "v2 value";
            Console.WriteLine(v1);
        }

        public string Prop1
        {
            get;
        }

        public string Prop2
        {
            get;
        }
    }
}