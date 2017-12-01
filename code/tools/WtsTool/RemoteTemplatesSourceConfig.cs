// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace WtsTool
{
    public class RemoteTemplatesSourceConfig
    {
        public RemoteTemplatesPackage Latest { get; set; }

        public int VersionCount { get; set; }

        public List<RemoteTemplatesPackage> Versions { get; set; }

        public static RemoteTemplatesSourceConfig LoadFromFile(string filePath)
        {
            RemoteTemplatesSourceConfig result = null;
            try
            {
                if (File.Exists(filePath))
                {
                    string configData = File.ReadAllText(filePath, Encoding.UTF8);

                    JsonSerializerSettings settings = new JsonSerializerSettings();
                    settings.NullValueHandling = NullValueHandling.Ignore;
                    settings.Converters.Add(new StringEnumConverter());
                    result = JsonConvert.DeserializeObject<RemoteTemplatesSourceConfig>(configData, settings);
                }
            }
            catch (Exception)
            {
                // TODO: Log error
                result = null;
            }

            return result;
        }
    }
}
