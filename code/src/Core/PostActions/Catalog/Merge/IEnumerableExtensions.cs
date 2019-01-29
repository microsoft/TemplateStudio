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
        internal const string MacroEndGroup = "}]}";

        internal const string MacroStartDocumentation = "{**";
        internal const string MacroEndDocumentation = "**}";

        private const string MacroStartDelete = "{--{";
        private const string MacroEndDelete = "}--}";

        internal const string MacroStartOptionalContext = "{??{";
        internal const string MacroEndOptionalContext = "}??}";

        private static string[] macros = new string[] { MacroBeforeMode, MacroStartGroup, MacroEndGroup, MacroStartDocumentation, MacroEndDocumentation, MacroStartDelete, MacroEndDelete, MacroStartOptionalContext, MacroEndOptionalContext };

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
            var removalBuffer = new List<string>();

            bool beforeMode = false;
            MergeMode mergeMode = MergeMode.Context;

            var diffTrivia = FindDiffLeadingTrivia(source, merge);
            var result = source.ToList();
            var currentLineIndex = -1;

            foreach (var mergeLine in merge)
            {
                // try to find line
                if (mergeMode == MergeMode.Context || mergeMode == MergeMode.OptionalContext)
                {
                    currentLineIndex = result.SafeIndexOf(mergeLine.WithLeadingTrivia(diffTrivia), lastLineIndex);
                }

                // if line is found, add buffer if any
                if (currentLineIndex > -1)
                {
                    var linesAdded = TryAddBufferContent(insertionBuffer, result, lastLineIndex, currentLineIndex, beforeMode);
                    var linesRemoved = TryRemoveBufferContent(removalBuffer, result, lastLineIndex, currentLineIndex);

                    if (linesAdded > 0)
                    {
                        lastLineIndex = currentLineIndex + linesAdded;
                    }

                    if (linesRemoved > 0)
                    {
                        lastLineIndex = currentLineIndex - linesRemoved;
                    }

                    if (beforeMode)
                    {
                        beforeMode = false;
                    }

                    if (linesRemoved == 0 && linesAdded == 0)
                    {
                        lastLineIndex = currentLineIndex;
                    }

                    insertionBuffer.Clear();
                    removalBuffer.Clear();
                }
                else
                {
                    // if line is not found check if merge direction, else add to buffer
                    if (IsMergeDirection(mergeLine))
                    {
                        mergeMode = GetMergeMode(mergeLine, mergeMode);
                    }
                    else
                    {
                        if (mergeMode == MergeMode.Insert || mergeMode == MergeMode.InsertBefore || mergeLine == string.Empty)
                        {
                            if (mergeMode == MergeMode.InsertBefore)
                            {
                                beforeMode = true;
                            }

                            insertionBuffer.Add(mergeLine.WithLeadingTrivia(diffTrivia));
                        }
                        else if (mergeMode == MergeMode.Remove)
                        {
                            removalBuffer.Add(mergeLine.WithLeadingTrivia(diffTrivia));
                        }
                        else if (mergeMode == MergeMode.OptionalContext)
                        {
                            currentLineIndex = lastLineIndex;
                        }
                        else if (mergeMode == MergeMode.Context)
                        {
                            errorLine = mergeLine;
                            return source;
                        }
                    }
                }
            }

            TryAddBufferContent(insertionBuffer, result, lastLineIndex);
            TryRemoveBufferContent(removalBuffer, result, lastLineIndex);

            return result;
        }

        private static bool IsMergeDirection(string mergeLine)
        {
            return macros.Any(c => mergeLine.Contains(c));
        }

        private static MergeMode GetMergeMode(string mergeLine, MergeMode current)
        {
            if (mergeLine.Contains(MacroBeforeMode))
            {
                return MergeMode.InsertBefore;
            }
            else if (mergeLine.Contains(MacroStartGroup))
            {
                if (current == MergeMode.InsertBefore)
                {
                    return MergeMode.InsertBefore;
                }
                else
                {
                    return MergeMode.Insert;
                }
            }
            else if (mergeLine.Contains(MacroEndGroup))
            {
                return MergeMode.Context;
            }
            else if (mergeLine.Contains(MacroStartDocumentation))
            {
                return MergeMode.Documentation;
            }
            else if (mergeLine.Contains(MacroEndDocumentation))
            {
                return MergeMode.Context;
            }
            else if (mergeLine.Contains(MacroStartDelete))
            {
                return MergeMode.Remove;
            }
            else if (mergeLine.Contains(MacroEndDelete))
            {
                return MergeMode.Context;
            }
            else if (mergeLine.Contains(MacroStartOptionalContext))
            {
                return MergeMode.OptionalContext;
            }
            else if (mergeLine.Contains(MacroEndOptionalContext))
            {
                return MergeMode.Context;
            }

            return current;
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
                startIndex = mergeString.IndexOf(MacroStartDelete, StringComparison.OrdinalIgnoreCase);
                endIndex = mergeString.IndexOf(MacroEndDelete, StringComparison.OrdinalIgnoreCase);
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

        private static int TryRemoveBufferContent(List<string> removalBuffer, List<string> result, int lastLineIndex, int currentLineIndex = 0)
        {
            if (removalBuffer.Any() && BlockExists(removalBuffer, result, lastLineIndex) && currentLineIndex > -1)
            {
                var index = result.SafeIndexOf(removalBuffer[0], lastLineIndex);
                if (index <= currentLineIndex)
                {
                    result.RemoveRange(index, removalBuffer.Count);
                    return removalBuffer.Count;
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

        private static int GetInsertLineIndex(int currentLine, int lastLine, bool beforeMode)
        {
            if (beforeMode)
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
