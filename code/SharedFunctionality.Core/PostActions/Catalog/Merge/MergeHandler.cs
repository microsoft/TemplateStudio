// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Templates.Core.PostActions.Catalog.Merge.CodeStyleProviders;

namespace Microsoft.Templates.Core.PostActions.Catalog.Merge
{
    public class MergeHandler
    {
        private BaseCodeStyleProvider _codeStyleProvider;
        private List<string> _result;

        private MergeMode mergeMode = MergeMode.Context;
        private bool insertBefore = false;

        private List<string> _insertionBuffer = new List<string>();
        private List<string> _removalBuffer = new List<string>();

        private int _currentContextLineIndex = -1;
        private int _lastContextLineIndex = -1;

        public MergeHandler(BaseCodeStyleProvider codeStyleProvider)
        {
            _codeStyleProvider = codeStyleProvider;
        }

        public MergeResult Merge(IEnumerable<string> source, IEnumerable<string> merge)
        {
            _result = source.ToList();

            var documentationEndIndex = merge.SafeIndexOf(merge.FirstOrDefault(m => m.Contains(MergeMacros.MacroEndDocumentation)), 0);
            var diffTrivia = FindDiffLeadingTrivia(source, merge, documentationEndIndex);

            foreach (var mergeLine in merge)
            {
                // try to find context line
                if (mergeMode == MergeMode.Context || mergeMode == MergeMode.OptionalContext || mergeMode == MergeMode.Remove)
                {
                    if (LineHasInlineAdditions(mergeLine))
                    {
                        _currentContextLineIndex = FindAndModifyContextLine(mergeLine, diffTrivia);
                    }
                    else
                    {
                        _currentContextLineIndex = FindContextLine(mergeLine, diffTrivia);
                    }
                }

                // if line is found, add buffer if any
                if (_currentContextLineIndex > -1)
                {
                    var linesAdded = TryAddBufferContent();
                    var linesRemoved = TryRemoveBufferContent();

                    _lastContextLineIndex = _currentContextLineIndex + linesAdded - linesRemoved;

                    CleanBuffers();
                }

                // get new merge direction or add to buffer
                if (IsMergeDirection(mergeLine))
                {
                    mergeMode = GetMergeMode(mergeLine, mergeMode, _codeStyleProvider?.CommentSymbol);
                }
                else
                {
                    switch (mergeMode)
                    {
                        case MergeMode.InsertBefore:
                        case MergeMode.Insert:
                            _insertionBuffer.Add(mergeLine.WithLeadingTrivia(diffTrivia));
                            break;
                        case MergeMode.Remove:
                            _removalBuffer.Add(mergeLine.WithLeadingTrivia(diffTrivia));
                            break;
                        case MergeMode.Context:
                            if (mergeLine == string.Empty)
                            {
                                _insertionBuffer.Add(mergeLine);
                            }
                            else if (_currentContextLineIndex == -1)
                            {
                                return new MergeResult()
                                {
                                    Success = false,
                                    ErrorLine = mergeLine,
                                    Result = source,
                                };
                            }

                            break;
                    }
                }
            }

            // Add remaining buffers before finishing
            TryAddBufferContent();
            TryRemoveBufferContent();

            return new MergeResult()
            {
                Success = true,
                ErrorLine = string.Empty,
                Result = _result,
            };
    }

        private bool LineHasInlineAdditions(string mergeLine)
        {
            return mergeLine.Contains(_codeStyleProvider.InlineCommentStart + MergeMacros.MacroStartGroup + _codeStyleProvider.InlineCommentEnd)
                                    && mergeLine.Contains(_codeStyleProvider.InlineCommentStart + MergeMacros.MacroEndGroup + _codeStyleProvider.InlineCommentEnd);
        }

        private void CleanBuffers()
        {
            insertBefore = false;
            _insertionBuffer.Clear();
            _removalBuffer.Clear();
        }

        private int TryAddBufferContent()
        {
            if (_insertionBuffer.Any() && !BlockExists(_insertionBuffer, _result, _lastContextLineIndex))
            {
                var insertIndex = GetInsertLineIndex(_currentContextLineIndex, _lastContextLineIndex, insertBefore);

                if (insertIndex <= _result.Count && insertIndex > -1)
                {
                    var lastContextLine = insertIndex != 0 ? _result[insertIndex - 1] : string.Empty;
                    var nextContextLine = insertIndex != _result.Count() ? _result[insertIndex] : string.Empty;

                    if (_codeStyleProvider != null)
                    {
                        _insertionBuffer = _codeStyleProvider.AdaptInsertionBlock(_insertionBuffer, lastContextLine, nextContextLine);
                    }

                    _result.InsertRange(insertIndex, _insertionBuffer);
                    return _insertionBuffer.Count;
                }
            }

            return 0;
        }

        private int TryRemoveBufferContent()
        {
            if (_removalBuffer.Any() && BlockExists(_removalBuffer, _result, _lastContextLineIndex))
            {
                var index = _result.SafeIndexOf(_removalBuffer[0], _lastContextLineIndex, false);
                if (index <= _currentContextLineIndex && index > -1)
                {
                    _result.RemoveRange(index, _removalBuffer.Count);
                    return _removalBuffer.Count;
                }
            }

            return 0;
        }

        private static bool IsMergeDirection(string mergeLine)
        {
            return MergeMacros.Macros.Any(c => mergeLine.Contains(c));
        }

        private MergeMode GetMergeMode(string mergeLine, MergeMode current, string commentSymbol)
        {
            if (mergeLine.Trim().StartsWith(commentSymbol + MergeMacros.MacroBeforeMode, StringComparison.Ordinal))
            {
                return MergeMode.InsertBefore;
            }
            else if (mergeLine.Trim().StartsWith(commentSymbol + MergeMacros.MacroStartGroup, StringComparison.Ordinal))
            {
                if (current == MergeMode.InsertBefore)
                {
                    insertBefore = true;
                    return MergeMode.InsertBefore;
                }
                else
                {
                    return MergeMode.Insert;
                }
            }
            else if (mergeLine.Trim().StartsWith(commentSymbol + MergeMacros.MacroEndGroup, StringComparison.Ordinal))
            {
                return MergeMode.Context;
            }
            else if (mergeLine.Trim().StartsWith(commentSymbol + MergeMacros.MacroStartDocumentation, StringComparison.Ordinal))
            {
                return MergeMode.Documentation;
            }
            else if (mergeLine.Trim().StartsWith(commentSymbol + MergeMacros.MacroEndDocumentation, StringComparison.Ordinal))
            {
                return MergeMode.Context;
            }
            else if (mergeLine.Trim().StartsWith(commentSymbol + MergeMacros.MacroStartDelete, StringComparison.Ordinal))
            {
                return MergeMode.Remove;
            }
            else if (mergeLine.Trim().StartsWith(commentSymbol + MergeMacros.MacroEndDelete, StringComparison.Ordinal))
            {
                return MergeMode.Context;
            }
            else if (mergeLine.Trim().StartsWith(commentSymbol + MergeMacros.MacroStartOptionalContext, StringComparison.Ordinal))
            {
                return MergeMode.OptionalContext;
            }
            else if (mergeLine.Trim().StartsWith(commentSymbol + MergeMacros.MacroEndOptionalContext, StringComparison.Ordinal))
            {
                return MergeMode.Context;
            }

            return current;
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

        private int FindContextLine(string mergeLine, int diffTrivia)
        {
            return _result.SafeIndexOf(mergeLine.WithLeadingTrivia(diffTrivia), _lastContextLineIndex);
        }

        private int FindAndModifyContextLine(string mergeLine, int diffTrivia)
        {
            var macroInlineAdditionStart = _codeStyleProvider.InlineCommentStart + MergeMacros.MacroStartGroup + _codeStyleProvider.InlineCommentEnd;
            var macroInlineAdditionEnd = _codeStyleProvider.InlineCommentStart + MergeMacros.MacroEndGroup + _codeStyleProvider.InlineCommentEnd;

            var mergeLineStart = mergeLine.Substring(0, mergeLine.IndexOf(macroInlineAdditionStart, StringComparison.Ordinal));
            var mergeLineEnd = mergeLine.Substring(mergeLine.IndexOf(macroInlineAdditionEnd, StringComparison.Ordinal) + macroInlineAdditionEnd.Length, mergeLine.Length - mergeLine.IndexOf(macroInlineAdditionEnd, StringComparison.Ordinal) - macroInlineAdditionEnd.Length);

            var additionStartIndex = mergeLine.IndexOf(macroInlineAdditionStart, StringComparison.Ordinal) + macroInlineAdditionStart.Length;
            var additionEndIndex = mergeLine.IndexOf(macroInlineAdditionEnd, StringComparison.Ordinal);

            var addition = mergeLine.Substring(additionStartIndex, additionEndIndex - additionStartIndex);
            var lineIndex = _result.SafeIndexOf(mergeLineStart.WithLeadingTrivia(diffTrivia), _lastContextLineIndex, true, true);
            var insertIndex = _result[lineIndex].Length;

            if (lineIndex != -1 && !_result[lineIndex].Contains(addition))
            {
                if (_codeStyleProvider != null)
                {
                    var nextChar = string.Empty;

                    if (mergeLineEnd != string.Empty)
                    {
                        insertIndex = _result[lineIndex].IndexOf(mergeLineEnd, StringComparison.Ordinal);
                        nextChar = mergeLineEnd.Substring(1);
                    }

                    // Get complete line content until insert index
                    var lineStart = insertIndex > 0 ? _result[lineIndex].Substring(0, insertIndex) : string.Empty;

                    addition = _codeStyleProvider.AdaptInlineAddition(addition, lineStart, mergeLineEnd);
                }

                _result[lineIndex] = _result[lineIndex].Insert(insertIndex, addition);
            }

            return lineIndex;
        }

        private static bool BlockExists(IEnumerable<string> blockBuffer, IEnumerable<string> target, int skip)
        {
            return blockBuffer
                        .Where(b => !string.IsNullOrWhiteSpace(b))
                        .All(b => target.SafeIndexOf(b, skip) > -1);
        }

        public int FindDiffLeadingTrivia(IEnumerable<string> target, IEnumerable<string> merge, int startIndex)
        {
            if (!target.Any() || !merge.Any())
            {
                return 0;
            }

            var firstMerge = merge.Skip(startIndex + 1).First(m => !string.IsNullOrEmpty(m));
            string firstTarget = null;
            if (LineHasInlineAdditions(firstMerge))
            {
                var contextStart = firstMerge.Substring(0, firstMerge.IndexOf(_codeStyleProvider.InlineCommentStart, StringComparison.Ordinal));
                firstTarget = target.FirstOrDefault(t => t.Trim().StartsWith(contextStart.Trim(), StringComparison.OrdinalIgnoreCase));
            }
            else
            {
                firstTarget = target.FirstOrDefault(t => t.Trim().Equals(firstMerge.Trim(), StringComparison.OrdinalIgnoreCase));
            }

            if (firstTarget == null)
            {
                return 0;
            }

            return firstTarget.GetLeadingTrivia() - firstMerge.GetLeadingTrivia();
        }
    }
}
