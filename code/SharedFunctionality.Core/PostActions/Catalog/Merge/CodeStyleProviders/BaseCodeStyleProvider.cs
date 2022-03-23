// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Linq;

namespace Microsoft.Templates.Core.PostActions.Catalog.Merge.CodeStyleProviders
{
    public class BaseCodeStyleProvider
    {
        private const char OpeningParentesis = '(';
        private const char ClosingParentesis = ')';

        public virtual string CommentSymbol => "//";

        public virtual string InlineCommentStart => "/*";

        public virtual string InlineCommentEnd => "*/";

        public virtual List<string> AdaptInsertionBlock(List<string> insertionBuffer, string lastContextLine, string nextContextLine)
        {
            EnsureWhiteLineBeforeComment(insertionBuffer, nextContextLine);

            return insertionBuffer;
        }

        public virtual string AdaptInlineAddition(string addition, string contextStart, string contextEnd)
        {
            addition = EnsureCommaSeparatorBetweenParentesis(addition, contextStart, contextEnd);

            return addition;
        }

        private void EnsureWhiteLineBeforeComment(List<string> insertionBuffer, string nextContextLine)
        {
            if (nextContextLine.Trim().StartsWith(CommentSymbol + string.Empty, StringComparison.Ordinal) && insertionBuffer.Last().Trim() != string.Empty)
            {
                insertionBuffer.Add(string.Empty);
            }
        }

        private string EnsureCommaSeparatorBetweenParentesis(string addition, string contextStart, string contextEnd)
        {
            if (contextStart.Contains(OpeningParentesis) && contextEnd.Contains(ClosingParentesis) && !contextStart.Last().Equals(OpeningParentesis))
            {
                if (!addition.StartsWith(", ", StringComparison.Ordinal))
                {
                    return addition = $", {addition}";
                }
            }

            return addition;
        }
    }
}
