// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;
using Xunit;

namespace Microsoft.Templates.Core.Test.Extensions
{
    [Trait("Group", "Minimum")]
    public class IEnumerableExtensionsTests
    {
        [Theory]
        [InlineData(new string[0], "public void SomeMethod()", 0)]
        [InlineData(new string[0], "public void SomeMethod()", 5)]
        [InlineData(null, "public void SomeMethod()", 0)]
        [InlineData(null, "public void SomeMethod()", 5)]
        [InlineData(new[] { " " }, "public void SomeMethod()", 0)]
        [InlineData(new[] { " " }, "public void SomeMethod()", 5)]
        public void SafeIndexOf_NoAvailableLinesToMatch_ShouldReturnNotFound(IEnumerable<string> lines, string lineToMatch, int linesToSkip)
        {
            var expected = -1;
            var actual = lines.SafeIndexOf(lineToMatch, linesToSkip);

            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData(new string[0], "public void SomeMethod()")]
        [InlineData(new[] { "public void SomeMetod()", "{", "}" }, "public void SomeMethod()")]
        [InlineData(new[] { "public void SomeMethod()", "{", "}" }, "no matching line")]
        [InlineData(new[] { " public void SomeMethod()", "{", "}" }, "public void SomeMethod()")]
        [InlineData(new[] { "", "", "public void SomeMethod()", "{", "}" }, "public void SmeMethod()")]
        public void SafeIndexOf_NoMatchingLinesFound_ShouldReturnNotFound(IEnumerable<string> lines, string linesToMatch)
        {
            var expected = -1;
            var linesToSkip = 0;
            var actual = lines.SafeIndexOf(linesToMatch, linesToSkip);

            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData(new[] { "public void SomeMethod()", "{", "}" }, "no matching line", -1)]
        [InlineData(new[] { "public void SomeMethod()", "{", "}" }, "public void SomeMethod()", 0)]
        [InlineData(new[] { "", "", "public void SomeMethod()", "{", "}" }, "public void SomeMethod()", 2)]
        public void SafeIndexOf_NoLinesToSkip_ShouldReturnIndexOfMatchingLine(IEnumerable<string> lines, string lineToMatch, int indexOfLine)
        {
            var actual = lines.SafeIndexOf(lineToMatch, 0);

            Assert.Equal(indexOfLine, actual);
        }

        [Theory]
        [InlineData(new[] { "public void SomeMethod()", "{", "}" }, "public void SomeMethod()", 0)]
        [InlineData(new[] { "public void SomeMethod()", "{", "}" }, "{", 1)]
        [InlineData(new[] { " ", "public void SomeMethod()", "{", "}" }, "{", 2)]
        [InlineData(new[] { "public void SomeMethod()", "{", "}", "//line to match" }, "//line to match", 3)]
        public void SafeIndexOf_SkipLinesIsNegativeNumber_ShouldReturnIndexOfMatchingLine(IEnumerable<string> lines, string lineToMatch, int indexOfLine)
        {
            var actual = lines.SafeIndexOf(lineToMatch, -1);

            Assert.Equal(indexOfLine, actual);
        }

        [Theory]
        [InlineData(new[] { "public void SomeMethod()", "", "{", "}" }, "public void SomeMethod()", 0, 0)]
        [InlineData(new[] { "public void SomeMethod()", "", "{", "}" }, "public void SomeMethod()", 1, -1)]
        [InlineData(new[] { "", "", "public void SomeMethod()", "{", "}" }, "public void SomeMethod()", 1, 2)]
        [InlineData(new[] { "public void SomeMethod()", "{", "//line to match", "}", "//line to match" }, "//line to match", 1, 2)]
        [InlineData(new[] { "public void SomeMethod()", "{", "//line to match", "}", "//line to match" }, "//line to match", 4, 4)]
        [InlineData(new[] { "public void SomeMethod()", "{", "//line to match", "}", "//line to match" }, "//line to match", 5, -1)]
        [InlineData(new[] { "public void SomeMethod()", "{", "}", "//line to match" }, "//line to match", 1, 3)]
        [InlineData(new[] { "public void SomeMethod()", "{", "}", "//line to match" }, "//line to match", 4, -1)]
        [InlineData(new[] { "", " ", "public void SomeMethod()", "//line to match", "{ ", "}", "//line to match" }, "//line to match", 1, 3)]
        [InlineData(new[] { "//line to match", " ", "public void SomeMethod()", "//line to match", "{ ", "}", "//line to match" }, "//line to match", 1, 3)]
        public void SafeIndexOf_SkippingLines_TakeIntoAccountWhiteLines_ShouldReturnIndexOfMatchingLine(IEnumerable<string> lines, string lineToMatch, int linesToSkip, int indexOfMatchingLine)
        {
            var actual = lines.SafeIndexOf(lineToMatch, linesToSkip, false);

            Assert.Equal(indexOfMatchingLine, actual);
        }

        [Theory]
        [InlineData(new[] { "public SomeClass(/*{[{*/ISomeService someService/*}]}*/)", "{", "}" }, "public SomeCass(", -1)]
        [InlineData(new[] { "public SomeClass(/*{[{*/ISomeService someService/*}]}*/)", "{", "}" }, "public SomeClass(", 0)]
        [InlineData(new[] { " public SomeClass(/*{[{*/ISomeService someService/*}]}*/)", "{", "}" }, "public SomeClass(", -1)]
        [InlineData(new[] { "", "public SomeClass(/*{[{*/ISomeService someService/*}]}*/)", "{", "}" }, "public SomeClass(", 1)]
        [InlineData(new[] { "", " ", "public SomeClass(/*{[{*/ISomeService someService/*}]}*/)", "{", "}" }, "public SomeClass(", 2)]
        public void SafeIndexOf_ReplaceInline_CompareUpToItemLenghtIsTrue_ShouldReturnIndexOfMatchingLine(IEnumerable<string> lines, string lineToMatch, int indexOfMatchingLine)
        {
            var actual = lines.SafeIndexOf(lineToMatch, 0, true, true);

            Assert.Equal(indexOfMatchingLine, actual);
        }
    }
}
