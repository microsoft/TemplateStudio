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
        public static T GetJsonContent<T>(string path)
            where T : new()
        {
            if (!File.Exists(path))
            {
                return new T();
            }

            var fileContent = File.ReadAllText(path);
            var content = JsonConvert.DeserializeObject<T>(fileContent);
            return content;
        }

        public static IEnumerable<string> GetValuesByName(string path, string name)
        {
            var content = GetJsonContent<List<JObject>>(path);
            return content.Select(item => item.GetValue(name, StringComparison.Ordinal).Value<string>());
        }

        public static string GetTemplateTag(string path, string tagName)
        {
            var content = GetJsonContent<JObject>(path);
            var value = (string)content["tags"][tagName];
            return value;
        }
    }
}
