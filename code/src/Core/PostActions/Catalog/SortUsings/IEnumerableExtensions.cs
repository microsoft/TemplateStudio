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
using System.Text.RegularExpressions;

namespace Microsoft.Templates.Core.PostActions.Catalog.SortUsings
{
    public static class ListStringExtensions
    {
        private const string SystemNs = "System";

        public static bool SortUsings(this List<string> classContent)
        {
            if (classContent == null || !classContent.Any())
            {
                return false;
            }

            var startUsingIndex = classContent.IndexOf(l => l.TrimStart().StartsWith(UsingComparer.UsingKeyword));
            var endUsingIndex = classContent.LastIndexOfWhile(startUsingIndex, l => l.TrimStart().StartsWith(UsingComparer.UsingKeyword) || string.IsNullOrWhiteSpace(l));

            if (startUsingIndex == -1 || endUsingIndex == -1)
            {
                return false;
            }

            var usingsLinesCount = endUsingIndex - startUsingIndex;
            var usings = classContent
                                .Skip(startUsingIndex)
                                .Take(usingsLinesCount)
                                .Where(u => !string.IsNullOrWhiteSpace(u))
                                .Select(u => (UsingComparer.ExtractNs(u), u))
                                .GroupBy(s => ExtractRootNs(s.Item1), s => s.Item2)
                                .ToList();

            classContent.RemoveRange(startUsingIndex, usingsLinesCount);

            var orderedUsings = new List<string>();
            var orderedKeys = GetOrderedNs(usings.Select(u => u.Key)).ToList();

            foreach (var key in orderedKeys)
            {
                var usingsGroup = usings
                                    .FirstOrDefault(u => u.Key.Equals(key))
                                    .Distinct()
                                    .ToList();

                usingsGroup.Sort(new UsingComparer());

                orderedUsings.AddRange(usingsGroup);
                orderedUsings.Add(string.Empty);
            }

            classContent.InsertRange(startUsingIndex, orderedUsings);

            return true;
        }

        public static int IndexOf(this List<string> classContent, Func<string, bool> expression)
        {
            for (int i = 0; i < classContent.Count; i++)
            {
                if (expression(classContent[i]))
                {
                    return i;
                }
            }

            return -1;
        }

        public static int LastIndexOfWhile(this List<string> classContent, int startIndex, Func<string, bool> expression)
        {
            var skipContent = classContent
                                    .Skip(startIndex)
                                    .ToList();

            for (int i = 0; i < skipContent.Count; i++)
            {
                if (!expression(skipContent[i]))
                {
                    return i;
                }
            }

            return -1;
        }

        private static string ExtractRootNs(string ns)
        {
            return Regex.Split(ns, @"\.").FirstOrDefault();
        }

        private static IEnumerable<string> GetOrderedNs(IEnumerable<string> usingGroups)
        {
            if (usingGroups.Contains(SystemNs))
            {
                yield return SystemNs;
            }

            var filtered = usingGroups
                                .Where(u => !u.Equals(SystemNs))
                                .OrderBy(u => u)
                                .ToList();

            foreach (var item in filtered)
            {
                yield return item;
            }
        }
    }
}
