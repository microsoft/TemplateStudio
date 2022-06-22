// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.IO;
using System.Linq;
using Microsoft.Templates.SharedResources;

namespace Microsoft.Templates.Core
{
    public static class StringExtensions
    {
        public static string[] GetMultiValue(this string value)
        {
            if (string.IsNullOrWhiteSpace(value) || string.IsNullOrEmpty(value.Trim()))
            {
                return new string[0];
            }

            var values = value.Split("|".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
            if (values.Any(v => v != v.Trim()))
            {
                throw new InvalidDataException(string.Format(Resources.ErrorExtraWhitespacesInMultiValues, value));
            }

            return values;
        }

        public static bool IsMultiValue(this string value)
        {
            return value.GetMultiValue().Length > 1;
        }

        public static int GetLeadingTrivia(this string statement)
        {
            return statement.TakeWhile(char.IsWhiteSpace).Count();
        }

        public static string WithLeadingTrivia(this string statement, int triviaCount)
        {
            if (triviaCount < 1)
            {
                return statement;
            }
            else
            {
                return string.Concat(new string(' ', triviaCount), statement);
            }
        }
    }
}
