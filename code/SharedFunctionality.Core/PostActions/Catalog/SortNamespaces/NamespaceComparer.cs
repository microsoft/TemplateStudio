// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;

namespace Microsoft.Templates.Core.PostActions.Catalog.SortNamespaces
{
    public abstract class NamespaceComparer : IComparer<string>
    {
        public abstract string Keyword { get; }

        protected abstract string Pattern { get; }

        protected abstract bool StripTrailingCharacter { get; }

        public int Compare(string x, string y)
        {
            var xNs = ExtractNs(x);
            var yNs = ExtractNs(y);

            return string.Compare(xNs, yNs, StringComparison.OrdinalIgnoreCase);
        }

        public string ExtractNs(string rawValue)
        {
            if (rawValue.StartsWith(Keyword, StringComparison.OrdinalIgnoreCase))
            {
                var ns = rawValue.Substring(Keyword.Length + 1);

                if (StripTrailingCharacter)
                {
                    ns = ns.Substring(0, ns.Length - 1);
                }

                return ns;
            }

            return string.Empty;
        }
    }
}
