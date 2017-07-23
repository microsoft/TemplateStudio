// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;
using System.Text.RegularExpressions;

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
