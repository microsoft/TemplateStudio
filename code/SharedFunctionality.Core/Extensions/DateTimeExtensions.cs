// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;

namespace Microsoft.Templates.Core
{
    public static class DateTimeExtensions
    {
        public static string FormatAsDateForFilePath(this DateTime date)
        {
            return date.ToString("yyyyMMdd");
        }

        public static string FormatAsFullDateTime(this DateTime date)
        {
            return date.ToString("yyyy-MM-dd HH:mm:ss.fff");
        }

        public static string FormatAsTime(this DateTime date)
        {
            return date.ToString("HH:mm:ss.fff");
        }

        public static string FormatAsShortDateTime(this DateTime date)
        {
            return date.ToString("yyyyMMdd_HHmmss");
        }

        public static string FormatAsDateHoursMinutes(this DateTime date)
        {
            return date.ToString("ddHHmmss");
        }
    }
}
