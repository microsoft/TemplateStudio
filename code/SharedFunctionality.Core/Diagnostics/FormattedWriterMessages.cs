// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Diagnostics;

using Microsoft.Templates.Resources;

namespace Microsoft.Templates.Core.Diagnostics
{
    public class FormattedWriterMessages
    {
        private static string exHeader = $"===================== {StringRes.ExceptionInfoString} =====================";

        public static string ExHeader
        {
            get { return exHeader; }
        }

        public const string ExFooter = "----------------------------------------------------------";

        public static string LogEntryStart
        {
            get
            {
                return $"[{DateTime.Now.FormatAsFullDateTime()}]\t{Environment.UserName}\t{Process.GetCurrentProcess().Id}({System.Threading.Thread.CurrentThread.ManagedThreadId})\t";
            }
        }
    }
}
