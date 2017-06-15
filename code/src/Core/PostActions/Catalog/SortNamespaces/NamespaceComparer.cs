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

namespace Microsoft.Templates.Core.PostActions.Catalog.SortNamespaces
{
    public abstract class NamespaceComparer : IComparer<string>
    {
        public abstract string UsingKeyword { get; }

        protected abstract string UsingPattern { get; }

        protected abstract bool StripTrailingCharacter { get; }

        public int Compare(string x, string y)
        {
            var xNs = ExtractNs(x);
            var yNs = ExtractNs(y);

            return xNs.CompareTo(yNs);
        }

        public string ExtractNs(string rawValue)
        {
            if (rawValue.StartsWith(UsingKeyword))
            {
                var ns = rawValue.Substring(UsingKeyword.Length + 1);

                if (StripTrailingCharacter)
                {
                    ns = ns.Substring(0, ns.Length -1);
                }

                return ns;
            }

            return string.Empty;
        }
    }
}
