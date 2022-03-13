// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;
using System.Linq;
using Microsoft.Templates.Core.PostActions.Catalog.Merge.CodeStyleProviders;
using Xunit;

namespace Microsoft.Templates.Core.Test.PostActions.Catalog
{
    [Trait("Group", "Minimum")]
    public class CSharpStyleProviderTest
    {
        [Fact]
        public void HandlesAdditionSuccessful_InsertAfterClosingBrace()
        {
            var insertionBuffer = new List<string>()
            {
                "FunctionA();",
            };
            var lastContextLine = "}";
            var nextContextLine = string.Empty;

            var styleProvider = new CSharpStyleProvider();
            var output = styleProvider.AdaptInsertionBlock(insertionBuffer, lastContextLine, nextContextLine);

            Assert.Equal(string.Empty, output.First());
        }

        [Fact]
        public void HandlesAdditionSuccessful_InsertBeforeClosingBrace()
        {
            var insertionBuffer = new List<string>()
            {
                "FunctionA();",
                string.Empty,
            };
            var lastContextLine = string.Empty;
            var nextContextLine = "}";

            var styleProvider = new CSharpStyleProvider();
            var output = styleProvider.AdaptInsertionBlock(insertionBuffer, lastContextLine, nextContextLine);

            Assert.Equal("FunctionA();", output.Last());
        }

        [Fact]
        public void HandlesAdditionSuccessful_InsertAfterOpeningBrace()
        {
            var insertionBuffer = new List<string>()
            {
                string.Empty,
                "FunctionA();",
            };
            var lastContextLine = "{";
            var nextContextLine = string.Empty;

            var styleProvider = new CSharpStyleProvider();
            var output = styleProvider.AdaptInsertionBlock(insertionBuffer, lastContextLine, nextContextLine);

            Assert.Equal("FunctionA();", output.First());
        }

        [Fact]
        public void HandlesAdditionSuccessful_InsertBlock()
        {
            var insertionBuffer = new List<string>()
            {
                "if (true)",
                "{",
                "    FunctionA();",
                "}",
            };
            var lastContextLine = string.Empty;
            var nextContextLine = "FunctionB();";

            var styleProvider = new CSharpStyleProvider();
            var output = styleProvider.AdaptInsertionBlock(insertionBuffer, lastContextLine, nextContextLine);

            Assert.Equal(string.Empty, output.Last());
        }

        [Fact]
        public void HandlesAdditionSuccessful_InsertElseBlock()
        {
            var insertionBuffer = new List<string>()
            {
                "else",
                "{",
                " // Else do this",
                "}",
            };
            var lastContextLine = "}";
            var nextContextLine = string.Empty;

            var styleProvider = new CSharpStyleProvider();
            var output = styleProvider.AdaptInsertionBlock(insertionBuffer, lastContextLine, nextContextLine);

            Assert.Equal("else", output.First());
        }

        [Fact]
        public void HandlesInlineAdditionSuccessful_InsertInterface()
        {
            var addition = "IService";
            var contextStart = "public class Test";
            var contextEnd = string.Empty;

            var styleProvider = new CSharpStyleProvider();
            var output = styleProvider.AdaptInlineAddition(addition, contextStart, contextEnd);

            Assert.Equal(": IService", output);
        }

        [Fact]
        public void HandlesInlineAdditionSuccessful_InsertInterfaceAfterOtherInterface()
        {
            var addition = "IService";
            var contextStart = "public class Test : IOtherService";
            var contextEnd = string.Empty;

            var styleProvider = new CSharpStyleProvider();
            var output = styleProvider.AdaptInlineAddition(addition, contextStart, contextEnd);

            Assert.Equal(", IService", output);
        }
    }
}
