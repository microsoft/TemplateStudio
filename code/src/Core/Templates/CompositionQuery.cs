using Microsoft.TemplateEngine.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Microsoft.Templates.Core
{
    public class CompositionQuery
    {
        private const string QueryPattern = "(?<field>[^=&]*)={2}(?<value>[^=&]*)&?";
        private const string ContextPrefix = "$";

        public Dictionary<string, string> Items { get; }

        private CompositionQuery(Dictionary<string, string> queryItems)
        {
            Items = queryItems;
        }

        public static CompositionQuery Parse(string value)
        {
            var queryItems = new Dictionary<string, string>();

            if (!string.IsNullOrWhiteSpace(value))
            {
                var queryMatches = Regex.Matches(value, QueryPattern);
                for (int i = 0; i < queryMatches.Count; i++)
                {
                    var m = queryMatches[i];
                    queryItems.Add(m.Groups["field"].Value, m.Groups["value"].Value);
                }
            }

            return new CompositionQuery(queryItems);
        }


        public bool Execute(ITemplateInfo source, IEnumerable<ITemplateInfo> context)
        {
            var itemQuery = Items
                                .Where(i => !i.Key.StartsWith(ContextPrefix))
                                .ToList();
            var contextQuery = Items
                                .Where(i => i.Key.StartsWith(ContextPrefix))
                                .Select(i => new KeyValuePair<string, string>(i.Key.Replace(ContextPrefix, string.Empty), i.Value))
                                .ToList();

            var itemResult = Match(itemQuery, source);
            var contextResult = !contextQuery.Any();

            if (itemResult && context != null)
            {
                contextResult = context.Any(t => Match(contextQuery, t));
            }

            return itemResult && contextResult;
        }

        private static bool Match(List<KeyValuePair<string, string>> query, ITemplateInfo template)
        {
            return query.All(i => SafeGet(template.GetQueryableProperties(), i.Key) == i.Value);
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
