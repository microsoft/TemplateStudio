// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.IO;
using System.Linq;

using Microsoft.Templates.Core.PostActions.Catalog.Merge;
using Microsoft.Templates.Core.PostActions.Catalog.Merge.CodeStyleProviders;
using Xunit;

namespace Microsoft.Templates.Core.Test.PostActions.Catalog
{
    [Trait("Group", "Minimum")]
    public class MergeTest
    {
        [Fact]
        public void Merge()
        {
            var source = File.ReadAllLines(@".\TestData\Merge\Source.cs");
            var merge = File.ReadAllLines(@".\TestData\Merge\Source_postaction.cs");
            var expected = File.ReadAllText(@".\TestData\Merge\Source_expected.cs");

            var mergeHandler = new MergeHandler(new CSharpStyleProvider());
            var result = mergeHandler.Merge(source, merge);

            // Remove all new line chars to avoid differentiation with the new line characters
            expected = expected.Replace("\r\n", string.Empty).Replace("\n", string.Empty);

            Assert.Equal(expected, string.Join(string.Empty, result.Result.ToArray()));
            Assert.True(result.Success);
            Assert.Equal(string.Empty, result.ErrorLine);
        }

        [Fact]
        public void MergeWithOptionalContextLines()
        {
            var source = File.ReadAllLines(@".\TestData\Merge\SourceWithOptionalContextLines.cs");
            var merge = File.ReadAllLines(@".\TestData\Merge\Source_postaction.cs");
            var expected = File.ReadAllText(@".\TestData\Merge\Source_expectedWithOptionalContextLines.cs");

            var mergeHandler = new MergeHandler(new CSharpStyleProvider());
            var result = mergeHandler.Merge(source, merge);

            // Remove all new line chars to avoid differentiation with the new line characters
            expected = expected.Replace("\r\n", string.Empty).Replace("\n", string.Empty);

            Assert.Equal(expected, string.Join(string.Empty, result.Result.ToArray()));
            Assert.True(result.Success);
            Assert.Equal(string.Empty, result.ErrorLine);
        }
    }
}
