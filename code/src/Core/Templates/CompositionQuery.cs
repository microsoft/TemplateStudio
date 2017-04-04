using Microsoft.TemplateEngine.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Microsoft.Templates.Core
{
    public class QueryNode
    {
        private const string ContextPrefix = "$";

        public QueryNode(string field, string @operator, string value)
        {
            IsContext = field.StartsWith(ContextPrefix);
            Field = field?.Replace(ContextPrefix, string.Empty);
            Operator = ParseOperator(@operator);
            Value = value;
        }

        private QueryOperator ParseOperator(string @operator)
        {
            if (@operator?.Equals("==") == true)
            {
                return QueryOperator.Equals;
            }
            else
            {
                return QueryOperator.NotEquals;
            }
        }

        public string Field { get; set; }
        public QueryOperator Operator { get; set; }
        public string Value { get; set; }

        public bool IsContext { get; }
    }

    public enum QueryOperator
    {
        Equals,
        NotEquals
    }

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
            foreach (var itemQuery in query)
            {
                if (itemQuery.Operator == QueryOperator.Equals)
                {
                    if (!SafeGet(queryableProperties, itemQuery.Field).Equals(itemQuery.Value))
                    {
                        return false;
                    }
                }
                else
                {
                    if (SafeGet(queryableProperties, itemQuery.Field).Equals(itemQuery.Value))
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        private static string SafeGet(Dictionary<string, string> properties, string key)
        {
            if (properties.ContainsKey(key))
            {
                return properties[key];
            }
            return null;
        }
    }
}
