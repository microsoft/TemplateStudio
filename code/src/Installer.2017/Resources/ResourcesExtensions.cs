// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Text.RegularExpressions;

namespace Microsoft.Templates.Extension
{
    internal static class ResourcesExtensions
    {
        public static string UseParams(this string res, params object[] args)
        {
            string pattern = @"(\{\d\})+";

            if (Regex.IsMatch(res, pattern))
            {
                try
                {
                    return string.Format(res, args);
                }
                catch
                {
                    return res + "<INVALID FORMAT>";
                }
            }
            else
            {
                return res;
            }
        }
    }
}
