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

using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

using Microsoft.TemplateEngine.Abstractions;

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
                    throw new InvalidCompositionQueryException($"The query \"{rawQuery}\" is not valid. Allowed operators are '==' or '!='. Multiple conditions are separated by '&'.");
                }
            }

            return query;
        }

        private static bool Validate(MatchCollection queryMatches, string rawQuery)
        {
            //Basic validation: matches concatenation must be equal than rawQuery
            var sb = new StringBuilder();

            foreach(Match m in queryMatches)
            {
                sb.Append(m.Value.Replace(" ", "").Trim());
            }

            return sb.ToString() == rawQuery.Replace(" ", "").Trim();
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
            return query
                    .All(q => queryableProperties.SafeGet(q.Field).Compare(q));
        }
    }
}
