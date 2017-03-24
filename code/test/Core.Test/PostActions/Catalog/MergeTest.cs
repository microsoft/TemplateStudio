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

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

            var result = source.Merge(merge);

            Assert.Equal(expected, string.Join(Environment.NewLine, result.ToArray()));
        }
    }
}
