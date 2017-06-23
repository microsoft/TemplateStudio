// ******************************************************************
// Copyright (c) Microsoft. All rights reserved.
// This code is licensed under the MIT License (MIT).
// THE CODE IS PROVIDED “AS IS”, WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED,
// INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.
// IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
// DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT,
// TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH
// THE CODE OR THE USE OR OTHER DEALINGS IN THE CODE.
// ******************************************************************

using System.Collections.Generic;

using Microsoft.Templates.Core.PostActions.Catalog.SortUsings;

using Xunit;

namespace Microsoft.Templates.Core.Test.PostActions.Catalog
{
    public class SortUsingsTest
    {
        [Fact]
        public void Sort()
        {
            var factData = new List<string>
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
                "",
                "namespace Microsoft.Templates",
                "{",
                "}"
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
                "}"
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
                "",
                "namespace Microsoft.Templates",
                "{",
                "}"
                };

            var expected = new List<string>
            {
                "",
                "namespace Microsoft.Templates",
                "{",
                "}"
            };

            var result = factData.SortUsings();

            Assert.False(result);
        }

        [Fact]
        public void Sort_OnlyUsings()
        {
            var factData = new List<string>
            {
                "using XUnit;",
                "using System.Text;",
                "using Microsoft.Templates;",
                "",
                "using System.Collections.Generic;",
                "using System;",
                "using Microsoft.Templates.Core;",
                "using System.Threading.Tasks;",
                "using System.Linq;"
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
                "using System.Linq;"
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
                "}"
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
                "}"
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
                "using System.Text;",
                "using Microsoft.Templates;",
                "",
                "using System;",
                "using Microsoft.Templates.Core;",
                "",
                "namespace Microsoft.Templates",
                "{",
                "}"
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
                "}"
            };

            var result = factData.SortUsings();

            Assert.True(result);
            Assert.Equal(expected, factData);
        }

    }
}
