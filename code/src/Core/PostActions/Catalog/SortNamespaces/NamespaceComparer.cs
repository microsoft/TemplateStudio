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

            return string.Compare(xNs, yNs, StringComparison.InvariantCultureIgnoreCase);
        }

        public string ExtractNs(string rawValue)
        {
            if (rawValue.StartsWith(Keyword, StringComparison.InvariantCultureIgnoreCase))
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
