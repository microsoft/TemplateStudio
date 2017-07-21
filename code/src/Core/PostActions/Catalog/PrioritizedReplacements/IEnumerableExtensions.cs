// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;
using System.Linq;

namespace Microsoft.Templates.Core.PostActions.Catalog.PrioritizedReplacements
{
    public static class ListStringExtensions
    {
        private const string ReplacementIndicator = "//REP:";
        private const string PriorityIndicator = "|PRI:";

        public static bool RemovePlaceholders(this List<string> classContent)
        {
            if (classContent == null || !classContent.Any())
            {
                return false;
            }

            bool changesMade = false;

            for (var index = classContent.Count - 1; index > 0; index--)
            {
                var line = classContent[index];

                if (line.Contains(ReplacementIndicator))
                {
                    var startPos = line.IndexOf(ReplacementIndicator);

                    classContent[index] = line.Substring(0, startPos);

                    changesMade = true;
                }
            }

            return changesMade;
        }

        public static IEnumerable<string> MakePriorityReplacements(this IEnumerable<string> source, IEnumerable<string> replacements)
        {
            var result = source.ToList();

            foreach (var replace in replacements)
            {
                var repComment = replace.Substring(replace.IndexOf(ReplacementIndicator));

                if (!string.IsNullOrWhiteSpace(repComment))
                {
                    var repId = repComment.Substring(0, repComment.IndexOf(PriorityIndicator));
                    var priority = repComment.Substring(repComment.IndexOf(PriorityIndicator) + PriorityIndicator.Length);

                    for (var index = 0; index < result.Count; index++)
                    {
                        var codeLine = result[index];
                        if (codeLine.Contains(repId))
                        {
                            var existingPriorty =
                                codeLine.Substring(codeLine.IndexOf(PriorityIndicator) + PriorityIndicator.Length);

                            if (int.Parse(priority) > int.Parse(existingPriorty))
                            {
                                result[index] = replace;
                            }

                            break;
                        }
                    }
                }
            }

            return result;
        }
    }
}
