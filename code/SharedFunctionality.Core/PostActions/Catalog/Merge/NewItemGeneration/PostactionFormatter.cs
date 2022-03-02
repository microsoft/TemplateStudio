// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Linq;

namespace Microsoft.Templates.Core.PostActions.Catalog.Merge
{
    public static class PostactionFormatter
    {
        private const string UserFriendlyPostActionMacroBeforeMode = "Include the following block at the end of the containing block.";
        public const string UserFriendlyPostActionMacroStartGroup = "Block to be included";
        private const string UserFriendlyPostActionMacroEndGroup = "End of block";
        private const string UserFriendlyPostActionMacroStartDocumentation = "***";
        private const string UserFriendlyPostActionMacroEndDocumentation = "***";

        public static string AsUserFriendlyPostAction(this string postactionCode)
        {
            var output = postactionCode
                            .Replace(MergeMacros.MacroBeforeMode, UserFriendlyPostActionMacroBeforeMode)
                            .Replace(MergeMacros.MacroStartDocumentation, UserFriendlyPostActionMacroStartDocumentation)
                            .Replace(MergeMacros.MacroEndDocumentation, UserFriendlyPostActionMacroEndDocumentation)
                            .Replace(MergeMacros.MacroStartGroup, UserFriendlyPostActionMacroStartGroup)
                            .Replace(MergeMacros.MacroEndGroup, UserFriendlyPostActionMacroEndGroup)
                            .Replace("//" + MergeMacros.MacroStartOptionalContext, string.Empty)
                            .Replace("//" + MergeMacros.MacroEndOptionalContext, string.Empty)
                            .Replace("'" + MergeMacros.MacroStartOptionalContext, string.Empty)
                            .Replace("'" + MergeMacros.MacroEndOptionalContext, string.Empty);

            var cleanRemovals = RemoveRemovals(output.Split(new[] { Environment.NewLine }, StringSplitOptions.None));
            output = string.Join(Environment.NewLine, cleanRemovals);
            return output;
        }

        private static List<string> RemoveRemovals(IEnumerable<string> merge)
        {
            var mergeString = string.Join(Environment.NewLine, merge);

            var startIndex = mergeString.IndexOf(MergeMacros.MacroStartDelete, StringComparison.OrdinalIgnoreCase);
            var endIndex = mergeString.IndexOf(MergeMacros.MacroEndDelete, StringComparison.OrdinalIgnoreCase);

            while (startIndex > 0 && endIndex > startIndex)
            {
                // VB uses a single character (') to start the comment, C# uses two (//)
                int commentIndicatorLength = mergeString[startIndex - 1] == '\'' ? 1 : 2;

                var lengthOfDeletion = endIndex - startIndex + MergeMacros.MacroStartDelete.Length + commentIndicatorLength;

                if (mergeString.Substring(startIndex + lengthOfDeletion - commentIndicatorLength).StartsWith(Environment.NewLine, StringComparison.OrdinalIgnoreCase))
                {
                    lengthOfDeletion += Environment.NewLine.Length;
                }

                mergeString = mergeString.Remove(startIndex - commentIndicatorLength, lengthOfDeletion);
                startIndex = mergeString.IndexOf(MergeMacros.MacroStartDelete, StringComparison.OrdinalIgnoreCase);
                endIndex = mergeString.IndexOf(MergeMacros.MacroEndDelete, StringComparison.OrdinalIgnoreCase);
            }

            return mergeString.Split(new[] { Environment.NewLine }, StringSplitOptions.None).ToList();
        }
    }
}
