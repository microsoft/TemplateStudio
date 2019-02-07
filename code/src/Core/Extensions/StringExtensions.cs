// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Globalization;
using System.Security.Cryptography;
using System.Text;

namespace Microsoft.Templates.Core
{
    public static class StringExtensions
    {
        public static string ObfuscateSHA(this string data)
        {
            string result = data;
            byte[] b64data = Encoding.UTF8.GetBytes(data);

            using (SHA512 sha2 = SHA512.Create())
            {
                result = GetHash(sha2, b64data);
            }

            return result.ToUpperInvariant();
        }

        private static string GetHash(HashAlgorithm md5Hash, byte[] inputData)
        {
            byte[] data = md5Hash.ComputeHash(inputData);

            var sb = new StringBuilder();

            for (int i = 0; i < data.Length; i++)
            {
                sb.Append(data[i].ToString("x2"));
            }

            return sb.ToString();
        }

        public static string[] GetMultiValue(this string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return new string[0];
            }

            return value.Split("|".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
        }

        public static bool IsMultiValue(this string value)
        {
            return value.GetMultiValue().Length > 1;
        }

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
