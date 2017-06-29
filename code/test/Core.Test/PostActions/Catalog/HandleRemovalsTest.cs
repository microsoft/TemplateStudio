// ******************************************************************
// Copyright (c) Microsoft. All rights reserved.
// This code is licensed under the MIT License (MIT).
// THE CODE IS PROVIDED ?AS IS?, WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED,
// INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.
// IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
// DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT,
// TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH
// THE CODE OR THE USE OR OTHER DEALINGS IN THE CODE.
// ******************************************************************

using System.Linq;
using Microsoft.Templates.Core.PostActions.Catalog.Merge;
using Xunit;

namespace Microsoft.Templates.Core.Test.PostActions.Catalog
{
    public class HandleRemovalsTest
    {
        [Fact]
        public void HandlesSuccessful()
        {
            var source = new[]
            {
                "public void SomeMethod()",
                "{",
                "    yield break;",
                "}"
            };
            var merge = new[] {
                "public void SomeMethod()",
                "{",
                "//{--{",
                "    yield break;//}--}",
                "}"
            };
            var expected = new[] {
                "public void SomeMethod()",
                "{",
                "}" };
            var result = source.HandleRemovals(merge);

            Assert.Equal(expected, result);
        }

        [Fact]
        public void SingleRemovalsAddMerge()
        {
            var source = new[]
            {
                "public void SomeMethod()",
                "{",
                "    yield break;",
                "}"
            };
            var merge1 = new[] {
                "public void SomeMethod()",
                "{",
                "    //{[{",
                "    // Merge1",
                "    //}]}",
                "//{--{",
                "    yield break;//}--}",
                "}"
            };
            var expected = new[] {
                "public void SomeMethod()",
                "{",
                "    // Merge1",
                "}" };
            var result = source.HandleRemovals(merge1);
            result = result.Merge(merge1.RemoveRemovals(), out string errorLine).ToList();

            Assert.Equal(expected, result);
        }

        [Fact]
        public void MultipleRemovalsAddMerges()
        {
            var source = new[]
            {
                "public void SomeMethod()",
                "{",
                "    yield break;",
                "}"
            };
            var merge1 = new[] {
                "public void SomeMethod()",
                "{",
                "    //{[{",
                "    // Merge1",
                "    //}]}",
                "//{--{",
                "    yield break;//}--}",
                "}"
            };
            var merge2 = new[] {
                "public void SomeMethod()",
                "{",
                "    //{[{",
                "    // Merge2",
                "    //}]}",
                "//{--{",
                "    yield break;//}--}",
                "}"
            };
            var expected = new[] {
                "public void SomeMethod()",
                "{",
                "    // Merge2",
                "    // Merge1",
                "}" };
            var result = source.HandleRemovals(merge1);
            result = result.Merge(merge1.RemoveRemovals(), out string errorLine).ToList();
            result = result.HandleRemovals(merge2);
            result = result.Merge(merge2.RemoveRemovals(), out errorLine).ToList();

            Assert.Equal(expected, result);
        }

        [Fact]
        public void NothingToRemove()
        {
            var source = new[]
            {
                "public void SomeMethod()",
                "{",
                "    yield break;",
                "}"
            };
            var merge = new[] {
                "public void SomeOtherMethod()",
                "{",
                "    // Something unrelated to deletion",
                "}"
            };
            var expected = new[] {
                "public void SomeMethod()",
                "{",
                "    yield break;",
                "}" };
            var result = source.HandleRemovals(merge);

            Assert.Equal(expected, result);
        }

        [Fact]
        public void AlreadyRemoved()
        {
            var source = new[]
            {
                "public void SomeMethod()",
                "{",
                "}"
            };
            var merge = new[] {
                "public void SomeMethod()",
                "{",
                "//{--{",
                "    yield break;//}--}",
                "}"
            };
            var expected = new[] {
                "public void SomeMethod()",
                "{",
                "}" };
            var result = source.HandleRemovals(merge);

            Assert.Equal(expected, result);
        }
    }
}
