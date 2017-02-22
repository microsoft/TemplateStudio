using Microsoft.Templates.Core.Injection.Code;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Microsoft.Templates.Core.Test.Injection
{
    public class CodeInjectorTest
    {
        [Fact]
        public void Inject()
        {
            var config = new CodeInjectorConfig
            {
                usings = new string[]
                {
                    "System.IO",
                    "System.Globalization"
                },
                path = "Microsoft.Templates.Core.Test.PostActions.Class1::Main",
                content = "var v1 = \"v1 value\";\r\nvar v2 = \"v2 value\";"
            };
            var code = File.ReadAllText(@".\TestData\PostActions\Code\Execute.cs");

            var target = new CodeInjector(config);

            var result = target.Inject(code);

            var expected = File.ReadAllText(@".\TestData\PostActions\Code\Execute_Expected.cs");

            Assert.Equal(expected, result);
        }
    }
}
