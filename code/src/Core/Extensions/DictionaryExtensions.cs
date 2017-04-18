using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
