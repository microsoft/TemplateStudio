// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Localization.Extensions
{
    public static class JsonExtensions
    {
        public static List<JObject> GetJsonContent(string path)
        {
            if (!File.Exists(path))
            {
                return new List<JObject>();
            }

            var fileContent = File.ReadAllText(path);
            var content = JsonConvert.DeserializeObject<List<JObject>>(fileContent);
            return content;
        }

        public static IEnumerable<string> GetValuesByName(string path, string name)
        {
            var content = GetJsonContent(path);
            return content.Select(item => item.GetValue(name, StringComparison.Ordinal).Value<string>());
        }
    }
}
