using Microsoft.Templates.Core.PostActions.Code;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Microsoft.Templates.Core.Test.PostActions
{
    public class CodePostActionTest
    {
        [Fact]
        public void Execute()
        {
            var config = new CodePostActionConfig
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

            var target = new CodePostAction();
            var result = target.Execute(config, code);

            var expected = File.ReadAllText(@".\TestData\PostActions\Code\Execute_Expected.cs");

            Assert.Equal(expected, result);
        }
    }
}
