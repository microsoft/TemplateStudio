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
using System.Linq;
using System.Xml.Linq;

namespace Microsoft.Templates.Core
{
    public static class XElementExtensions
    {
        public static XElement Select(this XElement xelement, string path)
        {
            var pathChunks = path.Split("/".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);

            if (pathChunks.Length <= 1)
            {
                return xelement;
            }

            var result = xelement;

            for (int i = 1; i < pathChunks.Length; i++)
            {
                var chunk = pathChunks[i];
                result = result.Element(xelement.GetDefaultNamespace() + chunk);

                if (result == null)
                {
                    return null;
                }
            }

            return result;
        }

        public static void CopyNamespaces(this XElement xelement, XElement source)
        {
            foreach (var ns in source.Attributes().Where(a => a.IsNamespaceDeclaration).ToList())
            {
                if (!xelement.Attributes().Any(a => a.IsNamespaceDeclaration && a.Name.LocalName == ns.Name.LocalName))
                {
                    xelement.Add(ns);
                }
            }
        }
    }
}
