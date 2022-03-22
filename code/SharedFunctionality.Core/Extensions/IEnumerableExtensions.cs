// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Linq;

namespace Microsoft.Templates.Core
{
    public static class IEnumerableExtensions
    {
        public static int SafeIndexOf(this IEnumerable<string> source, string item, int skip, bool ignoreWhiteLines = true, bool compareUpToItemLength = false)
        {
            if (source == null || (string.IsNullOrWhiteSpace(item) && ignoreWhiteLines))
            {
                return -1;
            }

            if (skip == -1)
            {
                skip = 0;
            }

            var actual = source.Skip(skip).ToList();

            for (int i = 0; i < actual.Count; i++)
            {
                if (compareUpToItemLength)
                {
                    if (actual[i].Length > item.TrimEnd().Length)
                    {
                        if (actual[i].Substring(0, item.TrimEnd().Length).Equals(item.TrimEnd(), StringComparison.OrdinalIgnoreCase))
                        {
                            return skip + i;
                        }
                    }
                }
                else
                {
                    if (actual[i].TrimEnd().Equals(item.TrimEnd(), StringComparison.OrdinalIgnoreCase))
                    {
                        return skip + i;
                    }
                }
            }

            return -1;
        }
    }
}
