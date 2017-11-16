// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Linq;

namespace Microsoft.Templates.Core.PostActions.Catalog.Merge
{
    public static class IEnumerableExtensions
    {
        internal const string MacroBeforeMode = "^^";
        internal const string MacroStartGroup = "{[{";
        internal const string MarcoEndGroup = "}]}";

        internal const string MacroStartDocumentation = "{**";
        internal const string MacroEndDocumentation = "**}";

        private const string MacroStartDelete = "{--{";
        private const string MacroEndDelete = "}--}";

        private const string OpeningBrace = "{";
        private const string ClosingBrace = "}";

        private const string CSharpComment = "// ";
        private const string VBComment = "' ";

        public static int SafeIndexOf(this IEnumerable<string> source, string item, int skip)
        {
            if (string.IsNullOrWhiteSpace(item))
            {
                return -1;
            }

            if (skip == -1)
            {
                skip = 0;
            }

            var actual = source.Skip(skip).ToList();

            for (int i = 0; i < actual.Count; i++)
            {
                if (actual[i].TrimEnd().Equals(item.TrimEnd(), StringComparison.OrdinalIgnoreCase))
                {
                    return skip + i;
                }
            }

            return -1;
        }

        public static IEnumerable<string> Merge(this IEnumerable<string> source, IEnumerable<string> merge, out string errorLine)
        {
            errorLine = string.Empty;
            int lastLineIndex = -1;
            var insertionBuffer = new List<string>();

            bool beforeMode = false;
            bool isInBlock = false;
            bool isInDocumentation = false;

            var diffTrivia = FindDiffLeadingTrivia(source, merge);
            var result = source.ToList();
            var currentLineIndex = -1;

            foreach (var mergeLine in merge)
            {
                if (!isInBlock && !isInDocumentation)
                {
                    currentLineIndex = result.SafeIndexOf(mergeLine.WithLeadingTrivia(diffTrivia), lastLineIndex);
                }

                if (currentLineIndex > -1)
                {
                    var linesAdded = TryAddBufferContent(insertionBuffer, result, lastLineIndex, currentLineIndex, beforeMode);

                    if (beforeMode)
                    {
                        beforeMode = false;
                    }

                    lastLineIndex = currentLineIndex + linesAdded;
                    insertionBuffer.Clear();
                }
                else
                {
                    if (mergeLine.Contains(MacroBeforeMode))
                    {
                        beforeMode = true;
                    }
                    else if (mergeLine.Contains(MacroStartGroup))
                    {
                        isInBlock = true;
                    }
                    else if (mergeLine.Contains(MarcoEndGroup))
                    {
                        isInBlock = false;
                    }
                    else if (!isInDocumentation && (isInBlock || mergeLine == string.Empty))
                    {
                        insertionBuffer.Add(mergeLine.WithLeadingTrivia(diffTrivia));
                    }
                    else if (mergeLine.Contains(MacroStartDocumentation))
                    {
                        isInDocumentation = true;
                    }
                    else if (mergeLine.Contains(MacroEndDocumentation))
                    {
                        isInDocumentation = false;
                    }
                    else if (!isInDocumentation)
                    {
                        errorLine = mergeLine;
                        return source;
                    }
                }
            }

            TryAddBufferContent(insertionBuffer, result, lastLineIndex);

            return result;
        }

        // Removes anything from the target file that should be deleted.
        public static List<string> HandleRemovals(this IEnumerable<string> source, IEnumerable<string> merge)
        {
            var mergeString = string.Join(Environment.NewLine, merge);
            var sourceString = string.Join(Environment.NewLine, source);

            var startIndex = mergeString.IndexOf(MacroStartDelete, StringComparison.OrdinalIgnoreCase);
            var endIndex = mergeString.IndexOf(MacroEndDelete, StringComparison.OrdinalIgnoreCase);

            while (startIndex > 0 && endIndex > startIndex)
            {
                // VB uses a single character (') to start the comment, C# uses two (//)
                int commentIndicatorLength = mergeString[startIndex - 1] == '\'' ? 1 : 2;

                var toRemove = mergeString.Substring(
                    (startIndex - commentIndicatorLength) + (MacroStartDelete.Length + commentIndicatorLength),
                    (endIndex - commentIndicatorLength) - (startIndex - commentIndicatorLength) - (MacroStartDelete.Length + commentIndicatorLength));

                sourceString = sourceString.Replace(toRemove, string.Empty);
                mergeString = mergeString.Substring(endIndex + MacroEndDelete.Length);
                startIndex = mergeString.IndexOf(MacroStartDelete, StringComparison.InvariantCultureIgnoreCase);
                endIndex = mergeString.IndexOf(MacroEndDelete, StringComparison.InvariantCultureIgnoreCase);
            }

            return sourceString.Split(new[] { Environment.NewLine }, StringSplitOptions.None).ToList();
        }

        // Remove any comments from the merged file that indicate something should be removed.
        public static List<string> RemoveRemovals(this IEnumerable<string> merge)
        {
            var mergeString = string.Join(Environment.NewLine, merge);

            var startIndex = mergeString.IndexOf(MacroStartDelete, StringComparison.OrdinalIgnoreCase);
            var endIndex = mergeString.IndexOf(MacroEndDelete, StringComparison.OrdinalIgnoreCase);

            while (startIndex > 0 && endIndex > startIndex)
            {
                // VB uses a single character (') to start the comment, C# uses two (//)
                int commentIndicatorLength = mergeString[startIndex - 1] == '\'' ? 1 : 2;

                var lengthOfDeletion = endIndex - startIndex + MacroStartDelete.Length + commentIndicatorLength;

                if (mergeString.Substring(startIndex + lengthOfDeletion - commentIndicatorLength).StartsWith(Environment.NewLine, StringComparison.OrdinalIgnoreCase))
                {
                    lengthOfDeletion += Environment.NewLine.Length;
                }

                mergeString = mergeString.Remove(startIndex - commentIndicatorLength, lengthOfDeletion);
                startIndex = mergeString.IndexOf(MacroStartDelete, StringComparison.InvariantCultureIgnoreCase);
                endIndex = mergeString.IndexOf(MacroEndDelete, StringComparison.InvariantCultureIgnoreCase);
            }

            return mergeString.Split(new[] { Environment.NewLine }, StringSplitOptions.None).ToList();
        }

        private static int TryAddBufferContent(List<string> insertionBuffer, List<string> result, int lastLineIndex, int currentLineIndex = 0, bool beforeMode = false)
        {
            if (insertionBuffer.Any() && !BlockExists(insertionBuffer, result, lastLineIndex) && currentLineIndex > -1)
            {
                var insertIndex = GetInsertLineIndex(currentLineIndex, lastLineIndex, beforeMode);

                if (insertIndex < result.Count)
                {
                    EnsureNoWhiteLinesNextToBraces(insertionBuffer, result, insertIndex);

                    EnsureWhiteLineBeforeComment(insertionBuffer, result, insertIndex);

                    result.InsertRange(insertIndex, insertionBuffer);
                    return insertionBuffer.Count;
                }
            }

            return 0;
        }

        private static void EnsureNoWhiteLinesNextToBraces(List<string> insertionBuffer, List<string> result, int insertIndex)
        {
            if (LastLineIsOpeningBrace(result, insertIndex) && insertionBuffer.First().Trim() == string.Empty)
            {
                insertionBuffer.RemoveAt(0);
            }

            if (NextLineIsClosingBrace(result, insertIndex) && insertionBuffer.Last().Trim() == string.Empty)
            {
                insertionBuffer.RemoveAt(insertionBuffer.Count - 1);
            }
        }

        private static void EnsureWhiteLineBeforeComment(List<string> insertionBuffer, List<string> result, int insertIndex)
        {
            if (NextLineIsComment(result, insertIndex) && insertionBuffer.Last().Trim() != string.Empty)
            {
                insertionBuffer.Add(string.Empty);
            }
        }

        private static bool LastLineIsOpeningBrace(List<string> result, int insertIndex)
        {
            var lastLineIndex = insertIndex - 1;
            return insertIndex > 0 && result[lastLineIndex].Trim() == OpeningBrace;
        }

        private static bool NextLineIsClosingBrace(List<string> result, int insertIndex)
        {
            return insertIndex < result.Count - 1 && result[insertIndex].Trim() == ClosingBrace;
        }

        private static bool NextLineIsComment(List<string> result, int insertIndex)
        {
            return insertIndex < result.Count - 1 && (result[insertIndex].Trim().StartsWith(CSharpComment, StringComparison.Ordinal) || result[insertIndex].Trim().StartsWith(VBComment, StringComparison.Ordinal));
        }

        private static bool BlockExists(IEnumerable<string> blockBuffer, IEnumerable<string> target, int skip)
        {
            return blockBuffer
                        .Where(b => !string.IsNullOrWhiteSpace(b))
                        .All(b => target.SafeIndexOf(b, skip) > -1);
        }

        private static int GetInsertLineIndex(int currentLine, int lastLine, bool isBeforeMode)
        {
            if (isBeforeMode)
            {
                return currentLine;
            }
            else
            {
                return lastLine + 1;
            }
        }

        private static int FindDiffLeadingTrivia(IEnumerable<string> target, IEnumerable<string> merge)
        {
            if (!target.Any() || !merge.Any())
            {
                return 0;
            }

            var documentactionEnd = merge.FirstOrDefault(m => m.Contains(MacroEndDocumentation));
            var documentationEndIndex = merge.SafeIndexOf(documentactionEnd, 0);
            var firstMerge = merge.Skip(documentationEndIndex + 1).First(m => !string.IsNullOrEmpty(m));
            var firstTarget = target.FirstOrDefault(t => t.Trim().Equals(firstMerge.Trim(), StringComparison.OrdinalIgnoreCase));

            if (firstTarget == null)
            {
                return 0;
            }

            return firstTarget.GetLeadingTrivia() - firstMerge.GetLeadingTrivia();
        }
    }
}
