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
