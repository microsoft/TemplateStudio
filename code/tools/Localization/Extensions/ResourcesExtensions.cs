// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Resources;

namespace Localization.Extensions
{
    public static class ResourcesExtensions
    {
        public static Dictionary<string, string> GetResourcesByFile(string filePath)
        {
            if (!File.Exists(filePath))
            {
                return new Dictionary<string, string>();
            }

            using (var resx = new ResXResourceReader(filePath))
            {
                return resx.Cast<DictionaryEntry>()
                            .ToDictionary(k => k.Key.ToString(), v => v.Value.ToString());
            }
        }

        public static string GetResourceValue(string filePath, string name)
        {
            using (ResXResourceSet resxSet = new ResXResourceSet(filePath))
            {
                return resxSet.GetString(name);
            }
        }
    }
}
