// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;

namespace Microsoft.Templates.Core
{
    public static class DictionaryExtensions
    {
        public static TValue SafeGet<TKey, TValue>(this Dictionary<TKey, TValue> dictionary, TKey key, TValue defaultResult = default(TValue))
        {
            if (dictionary.ContainsKey(key))
            {
                return dictionary[key];
            }

            return defaultResult;
        }
    }
}
