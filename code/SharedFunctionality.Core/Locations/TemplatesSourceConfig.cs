﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

using Microsoft.Templates.Core.Diagnostics;
using Microsoft.Templates.SharedResources;

using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Microsoft.Templates.Core.Locations
{
    public class TemplatesSourceConfig
    {
        public TemplatesPackageInfo Latest { get; set; }

        public Uri RootUri { get; set; }

        public List<TemplatesPackageInfo> Versions { get; set; }

        public static TemplatesSourceConfig LoadFromFile(string filePath)
        {
            TemplatesSourceConfig result = null;
            try
            {
                if (File.Exists(filePath))
                {
                    string configData = File.ReadAllText(filePath, Encoding.UTF8);

                    JsonSerializerSettings settings = new JsonSerializerSettings();
                    settings.NullValueHandling = NullValueHandling.Ignore;
                    settings.Converters.Add(new StringEnumConverter());
                    result = JsonConvert.DeserializeObject<TemplatesSourceConfig>(configData, settings);
                }
            }
            catch (Exception ex)
            {
                AppHealth.Current.Exception.TrackAsync(ex, Resources.TemplatesSourceErrorLoadingConfigFileMessage).FireAndForget();
                result = null;
            }

            return result;
        }

        public TemplatesPackageInfo ResolvePackage(string v, string platform, string language)
        {
            TemplatesPackageInfo match = Versions
                        .Where(p => p.Platform == platform || p.Platform == null)
                        .Where(p => p.Language == language || p.Language == null)
                        .Where(p => (p.WizardVersions != null && p.WizardVersions.Any(wv => wv == v))
                                 || (p.WizardVersions == null && p.Version == v))
                        .OrderByDescending(p => p.Date).FirstOrDefault();

            return match;
        }
    }
}
