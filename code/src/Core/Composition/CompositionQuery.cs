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
using System.Text.RegularExpressions;

using Microsoft.TemplateEngine.Abstractions;

namespace Microsoft.Templates.Core.Composition
{
    public class CompositionQuery
    {
        private const string QueryPattern = "(?<field>[^=&]*)(?<operator>={2}|!=)(?<value>[^=&]*)&?";

        public List<QueryNode> Items { get; } = new List<QueryNode>();

        private CompositionQuery()
        {
        }

        public static CompositionQuery Parse(string value)
        {
            var query = new CompositionQuery();

            if (!string.IsNullOrWhiteSpace(value))
            {
                var queryMatches = Regex.Matches(value, QueryPattern);
                for (int i = 0; i < queryMatches.Count; i++)
                {
                    var m = queryMatches[i];
                    query.Items.Add(new QueryNode(m.Groups["field"].Value, m.Groups["operator"].Value, m.Groups["value"].Value));
                }
            }

            return query;
        }


        public bool Execute(ITemplateInfo source, IEnumerable<ITemplateInfo> context)
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
                contextResult = context.Any(t => Match(contextQuery, t));
            }

            return itemResult && contextResult;
        }

        private static bool Match(IEnumerable<QueryNode> query, ITemplateInfo template)
        {
            var queryableProperties = template.GetQueryableProperties();

            return query
                    .All(q => queryableProperties.SafeGet(q.Field).Compare(q));
        }
    }
}
