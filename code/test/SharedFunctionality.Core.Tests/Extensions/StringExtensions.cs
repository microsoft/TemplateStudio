// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace CoreTemplateStudio.Core.Test.Extensions
{
    public static class StringExtensions
    {
        // This is the same substitution as VS makes with new projects
        public static string MakeSafeProjectName(this string projectName)
        {
            if (projectName == null)
            {
                return null;
            }

            var stringBuilder = new StringBuilder(projectName);

            for (var i = 0; i < stringBuilder.Length; i++)
            {
                var unicodeCategory = char.GetUnicodeCategory(stringBuilder[i]);

                var flag = unicodeCategory == UnicodeCategory.UppercaseLetter ||
                           unicodeCategory == UnicodeCategory.LowercaseLetter ||
                           unicodeCategory == UnicodeCategory.TitlecaseLetter ||
                           unicodeCategory == UnicodeCategory.OtherLetter ||
                           unicodeCategory == UnicodeCategory.LetterNumber ||
                           stringBuilder[i] == '\u005F';

                var flag1 = unicodeCategory == UnicodeCategory.NonSpacingMark ||
                            unicodeCategory == UnicodeCategory.SpacingCombiningMark ||
                            unicodeCategory == UnicodeCategory.ModifierLetter ||
                            unicodeCategory == UnicodeCategory.DecimalDigitNumber;

                if (i == 0)
                {
                    if (!flag)
                    {
                        stringBuilder[i] = '\u005F';
                    }
                }
                else if (!flag & !flag1)
                {
                    stringBuilder[i] = '\u005F';
                }
            }

            return stringBuilder.ToString();
        }
    }
}
