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
using Microsoft.Templates.Core.PostActions.Catalog.SortNamespaces;
using Xunit;

namespace Microsoft.Templates.Core.Test.PostActions.Catalog
{
    public class SortImportsTest
    {
        [Fact]
        public void Sort()
        {
            var factData = new List<string>
            {
                "Imports XUnit",
                "Imports System.Text",
                "Imports Microsoft.Templates",
                "",
                "Imports System.Collections.Generic",
                "Imports System",
                "Imports Microsoft.Templates.Core",
                "Imports System.Threading.Tasks",
                "Imports System.Linq",
                "",
                "Namespace Microsoft.Templates",
                "",
                "End Namespace"
            };

            var expected = new List<string>
            {
                "Imports System",
                "Imports System.Collections.Generic",
                "Imports System.Linq",
                "Imports System.Text",
                "Imports System.Threading.Tasks",
                "",
                "Imports Microsoft.Templates",
                "Imports Microsoft.Templates.Core",
                "",
                "Imports XUnit",
                "",
                "Namespace Microsoft.Templates",
                "",
                "End Namespace"
            };

            var result = factData.SortImports();

            Assert.True(result);
            Assert.Equal(expected, factData);
        }

        [Fact]
        public void Sort_NoImports()
        {
            var factData = new List<string>
            {
                "Namespace Microsoft.Templates",
                "    ' some content",
                "End Namespace"
            };

            var expected = new List<string>
            {
                "Namespace Microsoft.Templates",
                "    ' some content",
                "End Namespace"
            };

            var result = factData.SortImports();

            Assert.False(result);
            Assert.Equal(expected, factData);
        }

        [Fact]
        public void Sort_OnlyImports()
        {
            var factData = new List<string>
            {
                "Imports XUnit",
                "Imports System.Text",
                "Imports Microsoft.Templates",
                "",
                "Imports System.Collections.Generic",
                "Imports System",
                "Imports Microsoft.Templates.Core",
                "Imports System.Threading.Tasks",
                "Imports System.Linq"
            };

            var expected = new List<string>
            {
                "Imports XUnit",
                "Imports System.Text",
                "Imports Microsoft.Templates",
                "",
                "Imports System.Collections.Generic",
                "Imports System",
                "Imports Microsoft.Templates.Core",
                "Imports System.Threading.Tasks",
                "Imports System.Linq"
            };

            var result = factData.SortImports();

            Assert.False(result);
            Assert.Equal(expected, factData);
        }

        [Fact]
        public void Sort_AndRemoveDuplicates()
        {
            // "System"*3 & "XUnit"*2
            var factData = new List<string>
            {
                "Imports System",
                "Imports System",
                "Imports XUnit",
                "Imports System.Text",
                "Imports Microsoft.Templates",
                "",
                "Imports System.Collections.Generic",
                "Imports System",
                "Imports Microsoft.Templates.Core",
                "Imports System.Threading.Tasks",
                "Imports XUnit",
                "Imports System.Linq",
                "",
                "Namespace Microsoft.Templates",
                "",
                "End Namespace"
            };

            var expected = new List<string>
            {
                "Imports System",
                "Imports System.Collections.Generic",
                "Imports System.Linq",
                "Imports System.Text",
                "Imports System.Threading.Tasks",
                "",
                "Imports Microsoft.Templates",
                "Imports Microsoft.Templates.Core",
                "",
                "Imports XUnit",
                "",
                "Namespace Microsoft.Templates",
                "",
                "End Namespace"
            };

            var result = factData.SortImports();

            Assert.True(result);
            Assert.Equal(expected, factData);
        }

        [Fact]
        public void Sort_ImportsNotAtTopOfFile()
        {
            var factData = new List<string>
            {
                "' comment",
                "' comment",
                "' comment",
                "' comment",
                "' comment",
                "' comment",
                "' comment",
                "",
                "",
                "Imports System.Text",
                "Imports Microsoft.Templates",
                "",
                "Imports System",
                "Imports Microsoft.Templates.Core",
                "",
                "Namespace Microsoft.Templates",
                "",
                "End Namespace"
            };

            var expected = new List<string>
            {
                "' comment",
                "' comment",
                "' comment",
                "' comment",
                "' comment",
                "' comment",
                "' comment",
                "",
                "Imports System",
                "Imports System.Text",
                "",
                "Imports Microsoft.Templates",
                "Imports Microsoft.Templates.Core",
                "",
                "Namespace Microsoft.Templates",
                "",
                "End Namespace"
            };

            var result = factData.SortImports();

            Assert.True(result);
            Assert.Equal(expected, factData);
        }
    }
}
