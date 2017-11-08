// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.IO;
using System.Linq;

using EnvDTE;

namespace Microsoft.Templates.UI.VisualStudio
{
    public static class VsShellExtensions
    {
        public static string GetSafeValue(this Properties props, string propertyName)
        {
            if (props != null)
            {
                if (props.Cast<Property>().Where(p => p.Name == propertyName).Any())
                {
                    return props.Item(propertyName).Value.ToString();
                }
                else
                {
                    return string.Empty;
                }
            }
            else
            {
                return string.Empty;
            }
        }

        public static T GetSafeValue<T>(this Properties props, string propertyName)
        {
            if (props != null)
            {
                if (props.Cast<Property>().Where(p => p.Name == propertyName).Any())
                {
                    return (T)props.Item(propertyName).Value;
                }
                else
                {
                    return default(T);
                }
            }
            else
            {
                return default(T);
            }
        }

        public static string RemoveTailDirectorySparator(this string target)
        {
            if (target.EndsWith(Path.DirectorySeparatorChar.ToString(), StringComparison.Ordinal))
            {
                return target.Substring(0, target.Length - 1);
            }
            else
            {
                return target;
            }
        }

        public static string RemoveStartDirectorySparator(this string target)
        {
            if (target.StartsWith(Path.DirectorySeparatorChar.ToString(), StringComparison.Ordinal))
            {
                return target.Substring(1, target.Length - 1);
            }
            else
            {
                return target;
            }
        }

       public static string SafeGetFileName(this EnvDTE.Project p)
        {
            try
            {
                return p.FullName;
            }
            catch
            {
                return string.Empty;
            }
        }
    }
}
