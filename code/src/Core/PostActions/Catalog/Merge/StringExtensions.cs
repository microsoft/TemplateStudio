// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Linq;

namespace Microsoft.Templates.Core.PostActions.Catalog.Merge
{
    public static class StringExtensions
    {
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
