// ******************************************************************
// Copyright (c) Microsoft. All rights reserved.
// This code is licensed under the MIT License (MIT).
// THE CODE IS PROVIDED “AS IS”, WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED,
// INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.
// IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
// DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT,
// TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH
// THE CODE OR THE USE OR OTHER DEALINGS IN THE CODE.
// ******************************************************************

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Templates.Core.PostActions.Catalog.Merge
{
    public static class IEnumerableExtensions
    {
        internal const string MacroBeforeMode = "^^";
        internal const string MacroStartGroup = "{[{";
        internal const string MarcoEndGroup = "}]}";

        // TODO: [ML] need to add comments to existing VB merge files
        internal const string MacroStartDocumentation = "{**";
        internal const string MacroEndDocumentation = "**}";

        private const string MacroStartDelete = "{--{";
        private const string MacroEndDelete = "}--}";

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

        /// <summary>
        /// Removes anything from the target file that should be deleted.
        /// </summary>
        public static List<string> HandleRemovals(this IEnumerable<string> source, IEnumerable<string> merge)
        {
            var mergeString = string.Join(Environment.NewLine, merge);
            var sourceString = string.Join(Environment.NewLine, source);

            var startIndex = mergeString.IndexOf(MacroStartDelete);
            var endIndex = mergeString.IndexOf(MacroEndDelete);

            if (startIndex > 0 && endIndex > startIndex)
            {
                int commentIndicatorLength;

                if (mergeString[startIndex - 1] == '\'')
                {
                    // VB Code so account for single comment indicator character
                    commentIndicatorLength = 1;
                }
                else
                {
                    // C# Code so account for two character comment indicator
                    commentIndicatorLength = 2;
                }

                var toRemove = mergeString.Substring((startIndex - commentIndicatorLength) + (MacroStartDelete.Length + commentIndicatorLength),
                    (endIndex - commentIndicatorLength) - (startIndex - commentIndicatorLength) - (MacroStartDelete.Length + commentIndicatorLength));

                sourceString = sourceString.Replace(toRemove, string.Empty);
            }

            return sourceString.Split(new[] { Environment.NewLine }, StringSplitOptions.None).ToList();
        }

        /// <summary>
        /// Remove any comments from the merged file that indicate something should be removed.
        /// </summary>
        public static List<string> RemoveRemovals(this IEnumerable<string> merge)
        {
            var mergeString = string.Join(Environment.NewLine, merge);

            var startIndex = mergeString.IndexOf(MacroStartDelete);
            var endIndex = mergeString.IndexOf(MacroEndDelete);

            if (startIndex > 0 && endIndex > startIndex)
            {
                int commentIndicatorLength;

                if (mergeString[startIndex - 1] == '\'')
                {
                    // VB Code so account for single comment indicator character
                    commentIndicatorLength = 1;
                }
                else
                {
                    // C# Code so account for two character comment indicator
                    commentIndicatorLength = 2;
                }

                var lengthOfDeletion = endIndex - startIndex + MacroStartDelete.Length + commentIndicatorLength;
            
                if (mergeString.Substring(startIndex + lengthOfDeletion).StartsWith(Environment.NewLine))
                {
                    lengthOfDeletion += Environment.NewLine.Length;
                }

                mergeString = mergeString.Remove(startIndex - commentIndicatorLength, lengthOfDeletion);
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
                    result.InsertRange(insertIndex, insertionBuffer);
                    return insertionBuffer.Count;
                }
            }
            return 0;
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
