// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.IO;
using System.Linq;

using Microsoft.Templates.Core.PostActions.Catalog.Merge;

using Xunit;

namespace Microsoft.Templates.Core.Test.PostActions.Catalog
{
    public class MergeTest
    {
        [Fact]
        public void Merge()
        {
            var source = File.ReadAllLines(@".\TestData\Merge\Source.cs");
            var merge = File.ReadAllLines(@".\TestData\Merge\Source_postaction.cs");
            var expected = File.ReadAllText(@".\TestData\Merge\Source_expected.cs");
            var result = source.Merge(merge, out string errorLine);

            Assert.Equal(expected, string.Join(Environment.NewLine, result.ToArray()));
            Assert.Equal(errorLine, string.Empty);
        }
    }
}
