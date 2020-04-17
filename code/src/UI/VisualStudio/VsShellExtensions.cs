// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using EnvDTE;
using Microsoft.Templates.UI.Threading;
using Microsoft.VisualStudio.Shell;

namespace Microsoft.Templates.UI.VisualStudio
{
    public static class VsShellExtensions
    {
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
            ThreadHelper.ThrowIfNotOnUIThread();

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
