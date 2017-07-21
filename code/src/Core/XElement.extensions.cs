// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
