// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

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
