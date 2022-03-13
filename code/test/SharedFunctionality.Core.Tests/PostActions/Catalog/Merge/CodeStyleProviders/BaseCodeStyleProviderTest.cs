// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;
using System.Linq;
using Microsoft.Templates.Core.PostActions.Catalog.Merge.CodeStyleProviders;
using Xunit;

namespace Microsoft.Templates.Core.Test.PostActions.Catalog.Merge.CodeStyleProviders
{
    [Trait("Group", "Minimum")]
    public class BaseCodeStyleProviderTest
    {
        [Fact]
        public void HandlesAdditionSuccessful_InsertBeforeComment()
        {
            var insertionBuffer = new List<string>()
            {
                "FunctionA();",
            };
            var lastContextLine = string.Empty;
            var nextContextLine = "// My comment";

            var styleProvider = new BaseCodeStyleProvider();
            var output = styleProvider.AdaptInsertionBlock(insertionBuffer, lastContextLine, nextContextLine);

            Assert.Equal(string.Empty, output.Last());
        }

        [Fact]
        public void HandlesInlineAdditionSuccessful_InsertBetweenParentesis()
        {
            var addition = "IService myService";
            var contextStart = "public SomeClass(";
            var contextEnd = ")";

            var styleProvider = new BaseCodeStyleProvider();
            var output = styleProvider.AdaptInlineAddition(addition, contextStart, contextEnd);

            Assert.Equal(addition, output);
        }

        [Fact]
        public void HandlesInlineAdditionSuccessful_InsertBetweenParentesisAfterOtherService()
        {
            var addition = "IService myService";
            var contextStart = "public SomeClass(IOtherService otherService";
            var contextEnd = ")";

            var styleProvider = new BaseCodeStyleProvider();
            var output = styleProvider.AdaptInlineAddition(addition, contextStart, contextEnd);

            Assert.Equal(", IService myService", output);
        }
    }
}
