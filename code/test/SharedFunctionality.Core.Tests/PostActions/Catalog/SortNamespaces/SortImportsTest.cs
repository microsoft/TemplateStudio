// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;
using Microsoft.Templates.Core.PostActions.Catalog.SortNamespaces;
using Xunit;

namespace Microsoft.Templates.Core.Test.PostActions.Catalog
{
    [Trait("Group", "Minimum")]
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
                "End Namespace",
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
                "End Namespace",
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
                "End Namespace",
            };

            var expected = new List<string>
            {
                "Namespace Microsoft.Templates",
                "    ' some content",
                "End Namespace",
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
                "Imports System.Linq",
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
                "Imports System.Linq",
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
                "End Namespace",
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
                "End Namespace",
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
                "End Namespace",
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
                "End Namespace",
            };

            var result = factData.SortImports();

            Assert.True(result);
            Assert.Equal(expected, factData);
        }
    }
}
