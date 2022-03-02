// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Linq;

namespace Microsoft.Templates.Core.PostActions.Catalog.Merge.CodeStyleProviders
{
    public class CSharpStyleProvider : BaseCodeStyleProvider
    {
        private const string OpeningBrace = "{";
        private const string ClosingBrace = "}";

        public override string CommentSymbol => "//";

        public override List<string> AdaptInsertionBlock(List<string> insertionBuffer, string lastContextLine, string nextContextLine)
        {
            var buffer = base.AdaptInsertionBlock(insertionBuffer, lastContextLine, nextContextLine);

            AdaptWhiteLinesToBraces(buffer, lastContextLine, nextContextLine);

            return buffer;
        }

        public override string AdaptInlineAddition(string addition, string contextStart, string contextEnd)
        {
            addition = base.AdaptInlineAddition(addition, contextStart, contextEnd);

            addition = EnsureInterfaceSeparation(addition, contextStart, contextEnd);

            return addition;
        }

        private static void AdaptWhiteLinesToBraces(List<string> insertionBuffer, string lastContextLine, string nextContextLine)
        {
            if (InsertionStartsWithBlankLine(insertionBuffer) && LastLineHasOpeningBrace(lastContextLine))
            {
                insertionBuffer.RemoveAt(0);
            }

            if (LastLineHasClosingBrace(lastContextLine) && !InsertionStartsWithBlankLine(insertionBuffer) && !InsertionStartsWithElseLine(insertionBuffer))
            {
                insertionBuffer.Insert(0, string.Empty);
            }

            if (InsertionEndsWithBlankLine(insertionBuffer) && NextLineHasClosingBraces(nextContextLine))
            {
                insertionBuffer.RemoveAt(insertionBuffer.Count - 1);
            }

            if (InsertionEndsWithClosingBraces(insertionBuffer) && !NextLineHasClosingBraces(nextContextLine) && !NextLineIsEmpty(nextContextLine))
            {
                insertionBuffer.Add(string.Empty);
            }
        }

        private static bool InsertionEndsWithClosingBraces(List<string> insertionBuffer)
        {
            return insertionBuffer.Last().Trim().Equals(ClosingBrace, StringComparison.Ordinal);
        }

        private static bool NextLineIsEmpty(string nextContextLine)
        {
            return nextContextLine.Trim().Equals(string.Empty, StringComparison.Ordinal);
        }

        private static bool NextLineHasClosingBraces(string nextContextLine)
        {
            return nextContextLine.Trim().Equals(ClosingBrace, StringComparison.Ordinal);
        }

        private static bool InsertionEndsWithBlankLine(List<string> insertionBuffer)
        {
            return insertionBuffer.Last().Trim() == string.Empty;
        }

        private static bool LastLineHasClosingBrace(string lastContextLine)
        {
            return lastContextLine.Trim().Equals(ClosingBrace, StringComparison.Ordinal);
        }

        private static bool InsertionStartsWithBlankLine(List<string> insertionBuffer)
        {
            return insertionBuffer.First().Trim() == string.Empty;
        }

        private static bool InsertionStartsWithElseLine(List<string> insertionBuffer)
        {
            return insertionBuffer.First().Trim().StartsWith("else", StringComparison.Ordinal);
        }

        private static bool LastLineHasOpeningBrace(string lastContextLine)
        {
            return lastContextLine.Trim().Equals(OpeningBrace, StringComparison.Ordinal);
        }

        private string EnsureInterfaceSeparation(string addition, string contextStart, string contextEnd)
        {
            if (contextStart.Contains(" class "))
            {
                if (contextStart.Contains(":"))
                {
                    return addition = $", {addition}";
                }
                else
                {
                    return addition = $": {addition}";
                }
            }

            return addition;
        }
    }
}
