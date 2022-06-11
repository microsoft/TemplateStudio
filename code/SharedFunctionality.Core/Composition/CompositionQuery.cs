// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

using Microsoft.TemplateEngine.Abstractions;
using Microsoft.Templates.SharedResources;

namespace Microsoft.Templates.Core.Composition
{
    public class CompositionQuery
    {
        private const string Separator = "&";
        private const string QueryPattern = "(?<field>[^=" + Separator + "]*)(?<operator>={2}|!=)(?<value>[^=" + Separator + "]*)" + Separator + "?";

        public List<QueryNode> Items { get; } = new List<QueryNode>();

        private CompositionQuery()
        {
        }

        public static CompositionQuery Parse(string rawQuery)
        {
            var query = new CompositionQuery();

            if (!string.IsNullOrWhiteSpace(rawQuery))
            {
                var queryMatches = Regex.Matches(rawQuery, QueryPattern);

                if (Validate(queryMatches, rawQuery))
                {
                    for (int i = 0; i < queryMatches.Count; i++)
                    {
                        var m = queryMatches[i];
                        query.Items.Add(new QueryNode(m.Groups["field"].Value.Trim(), m.Groups["operator"].Value.Trim(), m.Groups["value"].Value.Trim()));
                    }
                }
                else
                {
                    throw new InvalidCompositionQueryException(string.Format(Resources.CompositionQueryParseMessage, rawQuery));
                }
            }

            return query;
        }

        private static bool Validate(MatchCollection queryMatches, string rawQuery)
        {
            // Basic validation: matches concatenation must be equal than rawQuery
            var sb = new StringBuilder();

            foreach (Match m in queryMatches)
            {
                sb.Append(m.Value.Replace(" ", string.Empty).Trim());
            }

            return sb.ToString() == rawQuery.Replace(" ", string.Empty).Trim();
        }

        public static CompositionQuery Parse(IEnumerable<string> rawQuery)
        {
            return Parse(string.Join("&", rawQuery.ToArray()));
        }

        public bool Match(ITemplateInfo source, QueryablePropertyDictionary context)
        {
            var itemQuery = Items
                                .Where(i => !i.IsContext)
                                .ToList();
            var contextQuery = Items
                                .Where(i => i.IsContext)
                                .ToList();

            var itemResult = Match(itemQuery, source);
            var contextResult = !contextQuery.Any();

            if (itemResult && context != null)
            {
                contextResult = Match(contextQuery, context);
            }

            return itemResult && contextResult;
        }

        private static bool Match(IEnumerable<QueryNode> query, ITemplateInfo template)
        {
            return Match(query, template.GetQueryableProperties());
        }

        private static bool Match(IEnumerable<QueryNode> query, QueryablePropertyDictionary queryableProperties)
        {
#pragma warning disable CA1308 // Normalize strings to uppercase
            return query
                    .All(q => queryableProperties.SafeGet(q.Field.ToLowerInvariant()).Compare(q));
#pragma warning restore CA1308 // Normalize strings to uppercase
        }
    }
}
