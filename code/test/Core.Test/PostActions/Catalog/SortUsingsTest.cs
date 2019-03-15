// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

using Microsoft.Templates.Core.PostActions.Catalog.SortNamespaces;

using Xunit;

namespace Microsoft.Templates.Core.Test.PostActions.Catalog
{
    [SuppressMessage("StyleCop", "SA1122", Justification = "The code is cleaner")]
    [Trait("ExecutionSet", "Minimum")]
    public class SortUsingsTest
    {
        [Fact]
        public void Sort()
        {
            var factData = new List<string>
            {
                "",
                "",
                "using XUnit;",
                "using System.Text;",
                "using Microsoft.Templates;",
                "",
                "using System.Collections.Generic;",
                "using System;",
                "using Microsoft.Templates.Core;",
                "using System.Threading.Tasks;",
                "using System.Linq;",
                "",
                "namespace Microsoft.Templates",
                "{",
                "}",
            };

            var expected = new List<string>
            {
                "using System;",
                "using System.Collections.Generic;",
                "using System.Linq;",
                "using System.Text;",
                "using System.Threading.Tasks;",
                "",
                "using Microsoft.Templates;",
                "using Microsoft.Templates.Core;",
                "",
                "using XUnit;",
                "",
                "namespace Microsoft.Templates",
                "{",
                "}",
            };

            var result = factData.SortUsings();

            Assert.True(result);
            Assert.Equal(expected, factData);
        }

        [Fact]
        public void Sort_NoUsings()
        {
            var factData = new List<string>
            {
                "namespace Microsoft.Templates",
                "{",
                "}",
            };

            var expected = new List<string>
            {
                "namespace Microsoft.Templates",
                "{",
                "}",
            };

            var result = factData.SortUsings();

            Assert.False(result);
        }

        [Fact]
        public void Sort_OnlyUsings()
        {
            var factData = new List<string>
            {
                "",
                "using XUnit;",
                "using System.Text;",
                "using Microsoft.Templates;",
                "",
                "using System.Collections.Generic;",
                "using System;",
                "using Microsoft.Templates.Core;",
                "using System.Threading.Tasks;",
                "using System.Linq;",
            };

            var expected = new List<string>
            {
                "using XUnit;",
                "using System.Text;",
                "using Microsoft.Templates;",
                "",
                "using System.Collections.Generic;",
                "using System;",
                "using Microsoft.Templates.Core;",
                "using System.Threading.Tasks;",
                "using System.Linq;",
            };

            var result = factData.SortUsings();

            Assert.False(result);
        }

        [Fact]
        public void Sort_AndRemoveDuplicates()
        {
            // "System"*3 & "XUnit"*2
            var factData = new List<string>
            {
                "using System;",
                "using System;",
                "using XUnit;",
                "using System.Text;",
                "using Microsoft.Templates;",
                "",
                "using System.Collections.Generic;",
                "using System;",
                "using Microsoft.Templates.Core;",
                "using System.Threading.Tasks;",
                "using XUnit;",
                "using System.Linq;",
                "",
                "namespace Microsoft.Templates",
                "{",
                "}",
            };

            var expected = new List<string>
            {
                "using System;",
                "using System.Collections.Generic;",
                "using System.Linq;",
                "using System.Text;",
                "using System.Threading.Tasks;",
                "",
                "using Microsoft.Templates;",
                "using Microsoft.Templates.Core;",
                "",
                "using XUnit;",
                "",
                "namespace Microsoft.Templates",
                "{",
                "}",
            };

            var result = factData.SortUsings();

            Assert.True(result);
            Assert.Equal(expected, factData);
        }

        [Fact]
        public void Sort_UsingsNotAtTopOfFile()
        {
            var factData = new List<string>
            {
                "// comment",
                "// comment",
                "// comment",
                "// comment",
                "// comment",
                "// comment",
                "// comment",
                "",
                "",
                "using System.Text;",
                "using Microsoft.Templates;",
                "",
                "using System;",
                "using Microsoft.Templates.Core;",
                "",
                "namespace Microsoft.Templates",
                "{",
                "}",
            };

            var expected = new List<string>
            {
                "// comment",
                "// comment",
                "// comment",
                "// comment",
                "// comment",
                "// comment",
                "// comment",
                "",
                "using System;",
                "using System.Text;",
                "",
                "using Microsoft.Templates;",
                "using Microsoft.Templates.Core;",
                "",
                "namespace Microsoft.Templates",
                "{",
                "}",
            };

            var result = factData.SortUsings();

            Assert.True(result);
            Assert.Equal(expected, factData);
        }
    }
}
