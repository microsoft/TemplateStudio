using Microsoft.Templates.Core.PostActions.Catalog.Merge;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

            var result = source.Merge(merge);

            Assert.Equal(expected, string.Join(Environment.NewLine, result.ToArray()));
        }
    }
}
