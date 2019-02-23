// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Microsoft.Templates.Core.PostActions.Catalog.SortNamespaces
{
    public static class ListStringExtensions
    {
        private const string SystemNs = "System";

        public static bool SortUsings(this List<string> classContent)
        {
            return classContent.SortNamespaceReferences(new UsingComparer());
        }

        public static bool SortImports(this List<string> classContent)
        {
            return classContent.SortNamespaceReferences(new ImportsComparer());
        }

        public static bool SortNamespaceReferences(this List<string> classContent, NamespaceComparer comparer)
        {
            if (classContent == null || !classContent.Any())
            {
                return false;
            }

            var startUsingIndex = classContent.IndexOf(l => l.TrimStart().StartsWith(comparer.Keyword, StringComparison.OrdinalIgnoreCase) || string.IsNullOrWhiteSpace(l));
            var endUsingIndex = classContent.LastIndexOfWhile(startUsingIndex, l => l.TrimStart().StartsWith(comparer.Keyword, StringComparison.OrdinalIgnoreCase) || string.IsNullOrWhiteSpace(l));

            if (startUsingIndex == -1 || endUsingIndex == -1)
            {
                return false;
            }

            var usingsLinesCount = endUsingIndex - startUsingIndex;
            var usings = classContent
                                .Skip(startUsingIndex)
                                .Take(usingsLinesCount)
                                .Where(u => !string.IsNullOrWhiteSpace(u))
                                .Select(u => new { ns = comparer.ExtractNs(u), usingStatement = u })
                                .GroupBy(s => ExtractRootNs(s.ns), s => s.usingStatement)
                                .ToList();

            classContent.RemoveRange(startUsingIndex, usingsLinesCount);

            var orderedUsings = new List<string>();
            if (startUsingIndex > 0)
            {
                orderedUsings.Add(string.Empty);
            }

            var orderedKeys = GetOrderedNs(usings.Select(u => u.Key)).ToList();

            foreach (var key in orderedKeys)
            {
                var usingsGroup = usings
                                    .FirstOrDefault(u => u.Key.Equals(key))
                                    .Distinct()
                                    .ToList();

                usingsGroup.Sort(comparer);

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
                    return i + startIndex;
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
