// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Linq;
using System.Text;

namespace Microsoft.Templates.Core.Casing
{
#pragma warning disable CA1308 // Normalize strings to uppercase
    public static class StringCasingExtensions
    {
        private static readonly char[] Separators = { ' ', '-', '_' };

        public static string ToKebabCase(this string name)
        {
            return Transform(name, '-').ToLowerInvariant();
        }

        public static string ToSnakeCase(this string name)
        {
            return Transform(name, '_').ToLowerInvariant();
        }

        public static string ToPascalCase(this string name)
        {
            return Transform(name, null, ToUpperCase);
        }

        public static string ToCamelCase(this string name)
        {
            return Transform(name, null, ToUpperCaseExceptFirstLetter);
        }

        public static string ToLowerCase(this string name)
        {
            return Transform(name, null).ToLowerInvariant();
        }

        private static char ToUpperCase(char c, int i)
        {
            return char.ToUpperInvariant(c);
        }

        private static char ToUpperCaseExceptFirstLetter(char c, int i)
        {
            if (i == 0)
            {
                return char.ToLowerInvariant(c);
            }
            else
            {
                return char.ToUpperInvariant(c);
            }
        }

        private static string Transform(string name, char? separator, Func<char, int, char> firstLetterTreatment = null)
        {
            name = name.Trim();
            var builder = new StringBuilder();
            var lastChar = name[0];

            for (var i = 0; i < name.Length; i++)
            {
                var currentCharacter = name[i];

                if (Separators.Contains(currentCharacter))
                {
                    if (!Separators.Contains(lastChar))
                    {
                        builder.Append(separator);
                    }
                }

                if (char.IsLetterOrDigit(currentCharacter))
                {
                    if (IsNewWord(currentCharacter, lastChar, i))
                    {
                        // new word treatment
                        // do not insert duplicate separators and do not use separators at beginning
                        if (i != 0 && separator.HasValue && !Separators.Contains(lastChar))
                        {
                            builder.Append(separator);
                        }

                        if (firstLetterTreatment != null)
                        {
                            builder.Append(firstLetterTreatment(currentCharacter, i));
                        }
                        else
                        {
                            builder.Append(currentCharacter);
                        }
                    }
                    else
                    {
                        builder.Append(currentCharacter);
                    }
                }

                lastChar = name[i];
            }

            return builder.ToString();
        }

        private static bool IsNewWord(char currentCharacter, char lastChar, int i)
        {
            if (i == 0)
            {
                return true;
            }

            if (char.IsUpper(currentCharacter) && char.IsLower(lastChar))
            {
                return true;
            }

            if (char.IsDigit(currentCharacter) && !char.IsDigit(lastChar))
            {
                return true;
            }

            if (Separators.Contains(lastChar))
            {
                return true;
            }

            return false;
        }
    }
#pragma warning restore CA1308 // Normalize strings to uppercase
}
