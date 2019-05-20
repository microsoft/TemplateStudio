// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;
using System.Linq;

namespace Localization.Extensions
{
    internal static class DictionaryExtensions
    {
        internal static bool AreEquals(this Dictionary<string, string> d1, Dictionary<string, string> d2)
        {
            // compare num items
            if (d1.Keys.Count != d2.Keys.Count)
            {
                return false;
            }

            // compare keys names
            if (d1.Keys.Any(k => !d2.Keys.Contains(k)))
            {
                return false;
            }

            // compare values
            if (d1.Any(i => i.Value != d2[i.Key]))
            {
                return false;
            }

            return true;
        }

        internal static Dictionary<string, ResxItem> GetChangesFrom(this Dictionary<string, ResxItem> d1, Dictionary<string, ResxItem> d2)
        {
            // return new entries and changed entries
            var result = d1
                .Where(entry => !d2.Keys.Contains(entry.Key) || d2[entry.Key].Text != entry.Value.Text)
                .ToDictionary(k => k.Key, v => v.Value);

            return result;
        }
    }
}
