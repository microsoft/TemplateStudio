// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
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
    }
}
