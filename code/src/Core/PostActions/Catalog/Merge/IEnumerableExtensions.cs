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
        private const string MacroBeforeMode = "^^";
        private const string MacroStartGroup = "{[{";
        private const string MarcoEndGroup = "}]}";
        private const string MacroStartDelete = "//{--{";
        private const string MacroEndDelete = "//}--}";

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

        public static IEnumerable<string> Merge(this IEnumerable<string> source, IEnumerable<string> merge)
        {
            int lastLineIndex = -1;
            var insertionBuffer = new List<string>();

            bool beforeMode = false;
            bool isInBlock = false;

            var diffTrivia = FindDiffLeadingTrivia(source, merge);
            var result = source.ToList();
            var currentLineIndex = -1;

            foreach (var mergeLine in merge)
            {
                if (!isInBlock)
                {
                    currentLineIndex = result.SafeIndexOf(mergeLine.WithLeadingTrivia(diffTrivia), lastLineIndex);
                }

                if (currentLineIndex > -1)
                {
                    TryAddBufferContent(insertionBuffer, result, lastLineIndex, currentLineIndex, beforeMode);

                    if (beforeMode)
                    {
                        beforeMode = false;
                    }

                    lastLineIndex = currentLineIndex + insertionBuffer.Count;
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
                    else
                    {
                        insertionBuffer.Add(mergeLine.WithLeadingTrivia(diffTrivia));
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
                var toRemove = mergeString.Substring(startIndex + MacroStartDelete.Length,
                    endIndex - startIndex - MacroStartDelete.Length);

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
                var lengthOfDeletion = endIndex - startIndex + MacroStartDelete.Length;

                if (mergeString.Substring(startIndex + lengthOfDeletion).StartsWith(Environment.NewLine))
                {
                    lengthOfDeletion += Environment.NewLine.Length;
                }

                mergeString = mergeString.Remove(startIndex, lengthOfDeletion);
            }

            return mergeString.Split(new[] { Environment.NewLine }, StringSplitOptions.None).ToList();
        }

        private static void TryAddBufferContent(List<string> insertionBuffer, List<string> result, int lastLineIndex, int currentLineIndex = 0, bool beforeMode = false)
        {
            if (insertionBuffer.Any() && !BlockExists(insertionBuffer, result, lastLineIndex) && currentLineIndex > -1)
            {
                var insertIndex = GetInsertLineIndex(currentLineIndex, lastLineIndex, beforeMode);

                if (insertIndex < result.Count)
                {
                    result.InsertRange(insertIndex, insertionBuffer);
                }
            }
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

            var firstMerge = merge.First();
            var firstTarget = target.FirstOrDefault(t => t.Trim().Equals(firstMerge.Trim(), StringComparison.OrdinalIgnoreCase));

            if (firstTarget == null)
            {
                return 0;
            }

            return firstTarget.GetLeadingTrivia() - firstMerge.GetLeadingTrivia();
        }
    }
}
