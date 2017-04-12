using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Microsoft.Templates.Core.PostActions.Catalog.SortUsings
{
    public class UsingComparer : IComparer<string>
    {
        public const string UsingKeyword = @"using";

        private const string UsingPattern = UsingKeyword + @"\s(?<Ns>.*?);";

        public int Compare(string x, string y)
        {
            var xNs = ExtractNs(x);
            var yNs = ExtractNs(y);

            return xNs.CompareTo(yNs);
        }

        public static string ExtractNs(string rawValue)
        {
            var m = Regex.Match(rawValue, UsingPattern);
            if (m.Success)
            {
                return m.Groups["Ns"].Value;
            }
            return string.Empty;
        }
    }
}
