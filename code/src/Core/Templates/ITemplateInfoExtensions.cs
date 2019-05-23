// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

using Microsoft.TemplateEngine.Abstractions;
using Microsoft.TemplateEngine.Abstractions.Mount;
using Microsoft.Templates.Core.Composition;
using Microsoft.Templates.Core.Gen;
using Newtonsoft.Json;

namespace Microsoft.Templates.Core
{
    public static class ITemplateInfoExtensions
    {
        private const string Separator = "|";
        private const string TagPrefix = "wts.";
        private const string LicensesPattern = @"\[(?<text>.*?)\]\((?<url>.*?)\)\" + Separator + "?";

        public static TemplateType GetTemplateType(this ITemplateInfo ti)
        {
            var type = GetValueFromTag(ti, TagPrefix + "type");
            switch (type?.ToUpperInvariant())
            {
                case "PROJECT":
                    return TemplateType.Project;
                case "PAGE":
                    return TemplateType.Page;
                case "FEATURE":
                    return TemplateType.Feature;
                case "COMPOSITION":
                    return TemplateType.Composition;
                default:
                    return TemplateType.Unspecified;
            }
        }

        public static TemplateOutputType GetTemplateOutputType(this ITemplateInfo ti)
        {
            var type = GetValueFromTag(ti, "type");
            switch (type?.ToUpperInvariant())
            {
                case "PROJECT":
                    return TemplateOutputType.Project;
                case "ITEM":
                    return TemplateOutputType.Item;
                default:
                    return TemplateOutputType.Unspecified;
            }
        }

        public static string GetIcon(this ITemplateInfo ti)
        {
            var configDir = GetConfigDir(ti);

            return Directory.EnumerateFiles(configDir, "icon.*").FirstOrDefault();
        }

        public static string GetRichDescription(this ITemplateInfo ti)
        {
            var configDir = GetConfigDir(ti);

            if (configDir != null)
            {
                var descriptionFile = Directory
                    .EnumerateFiles(configDir, $"{CultureInfo.CurrentUICulture.IetfLanguageTag}.description.md")
                    .FirstOrDefault();
                if (string.IsNullOrWhiteSpace(descriptionFile) || !File.Exists(descriptionFile))
                {
                    descriptionFile = Directory.EnumerateFiles(configDir, "description.md").FirstOrDefault();
                }

                if (!string.IsNullOrEmpty(descriptionFile))
                {
                    return File.ReadAllText(descriptionFile);
                }
            }

            return null;
        }

        public static string GetCompositionFilter(this ITemplateInfo ti)
        {
            return GetValueFromTag(ti, TagPrefix + "compositionFilter");
        }

        public static string GetLanguage(this ITemplateInfo ti)
        {
            return GetValueFromTag(ti, "language");
        }

        public static IEnumerable<TemplateLicense> GetLicenses(this ITemplateInfo ti)
        {
            var licenses = GetValueFromTag(ti, TagPrefix + "licenses");

            if (string.IsNullOrWhiteSpace(licenses))
            {
                return Enumerable.Empty<TemplateLicense>();
            }

            var result = new List<TemplateLicense>();
            var licensesMatches = Regex.Matches(licenses, LicensesPattern);

            for (int i = 0; i < licensesMatches.Count; i++)
            {
                var m = licensesMatches[i];

                if (m.Success)
                {
                    result.Add(new TemplateLicense
                    {
                        Text = m.Groups["text"].Value,
                        Url = m.Groups["url"].Value,
                    });
                }
            }

            return result;
        }

        public static QueryablePropertyDictionary GetQueryableProperties(this ITemplateInfo ti)
        {
            var properties = new QueryablePropertyDictionary();

            if (ti != null)
            {
                properties.Add(new QueryableProperty(nameof(ti.Name).ToLowerInvariant(), ti.Name));
                properties.Add(new QueryableProperty(nameof(ti.Identity).ToLowerInvariant(), ti.Identity));
                properties.Add(new QueryableProperty(nameof(ti.GroupIdentity).ToLowerInvariant(), ti.GroupIdentity));

                foreach (var t in ti.Tags)
                {
                    properties.Add(new QueryableProperty(t.Key, t.Value.DefaultValue));
                }
            }

            return properties;
        }

        public static IDictionary<string, string> GetExports(this ITemplateInfo ti)
        {
            if (ti == null || ti.Tags == null)
            {
                return new Dictionary<string, string>();
            }

            return ti.Tags
                        .Where(t => t.Key.Contains(TagPrefix + "export."))
                        .ToDictionary(t => t.Key.Replace(TagPrefix + "export.", string.Empty), v => v.Value.DefaultValue);
        }

        public static List<string> GetFrontEndFrameworkList(this ITemplateInfo ti)
        {
            var frontEndFrameworks = GetValueFromTag(ti, TagPrefix + "frontendframework");

            var result = new List<string>();

            if (!string.IsNullOrEmpty(frontEndFrameworks))
            {
                result.AddRange(frontEndFrameworks.Split(Separator.ToCharArray(), StringSplitOptions.RemoveEmptyEntries));
            }

            return result;
        }

        public static List<string> GetBackEndFrameworkList(this ITemplateInfo ti)
        {
            var backEndFrameworks = GetValueFromTag(ti, TagPrefix + "backendframework");

            var result = new List<string>();

            if (!string.IsNullOrEmpty(backEndFrameworks))
            {
                result.AddRange(backEndFrameworks.Split(Separator.ToCharArray(), StringSplitOptions.RemoveEmptyEntries));
            }

            return result;
        }

        public static string GetPlatform(this ITemplateInfo ti)
        {
            return GetValueFromTag(ti, TagPrefix + "platform") ?? string.Empty;
        }

        public static List<string> GetDependencyList(this ITemplateInfo ti)
        {
            var dependencies = GetValueFromTag(ti, TagPrefix + "dependencies");
            var result = new List<string>();

            if (!string.IsNullOrEmpty(dependencies))
            {
                result.AddRange(dependencies.Split(Separator.ToCharArray(), StringSplitOptions.RemoveEmptyEntries));
            }

            return result;
        }

        public static string GetGroup(this ITemplateInfo ti)
        {
            return GetValueFromTag(ti, TagPrefix + "group");
        }

        public static bool GetIsGroupExclusiveSelection(this ITemplateInfo ti)
        {
            var result = GetValueFromTag(ti, TagPrefix + "isGroupExclusiveSelection");

            if (!string.IsNullOrEmpty(result))
            {
                if (bool.TryParse(result, out bool boolResult))
                {
                    return boolResult;
                }
            }

            return false;
        }

        public static string GetVersion(this ITemplateInfo ti)
        {
            return GetValueFromTag(ti, TagPrefix + "version");
        }

        public static int GetDisplayOrder(this ITemplateInfo ti)
        {
            var rawOrder = GetValueFromTag(ti, TagPrefix + "displayOrder");

            if (!string.IsNullOrEmpty(rawOrder))
            {
                if (int.TryParse(rawOrder, out int order))
                {
                    return order;
                }
            }

            return int.MaxValue;
        }

        public static int GetCompositionOrder(this ITemplateInfo ti)
        {
            var rawOrder = GetValueFromTag(ti, TagPrefix + "compositionOrder");

            if (!string.IsNullOrEmpty(rawOrder))
            {
                if (int.TryParse(rawOrder, out int order))
                {
                    return order;
                }
            }

            return int.MaxValue;
        }

        public static bool GetIsHidden(this ITemplateInfo ti)
        {
            var result = GetValueFromTag(ti, TagPrefix + "isHidden");

            if (!string.IsNullOrEmpty(result))
            {
                if (bool.TryParse(result, out bool boolResult))
                {
                    return boolResult;
                }
            }

            return false;
        }

        public static List<string> GetProjectTypeList(this ITemplateInfo ti)
        {
            var projectTypes = GetValueFromTag(ti, TagPrefix + "projecttype");

            var result = new List<string>();

            if (!string.IsNullOrEmpty(projectTypes))
            {
                result.AddRange(projectTypes.Split(Separator.ToCharArray(), StringSplitOptions.RemoveEmptyEntries));
            }

            return result;
        }

        public static IEnumerable<LayoutItem> GetLayout(this ITemplateInfo ti)
        {
            var configDir = GetConfigDir(ti);

            var layoutFile = Directory.EnumerateFiles(configDir, "Layout.json").FirstOrDefault();

            if (string.IsNullOrEmpty(layoutFile))
            {
                return Enumerable.Empty<LayoutItem>();
            }

            var layoutContent = File.ReadAllText(layoutFile);

            return JsonConvert.DeserializeObject<List<LayoutItem>>(layoutContent);
        }

        public static string GetDefaultName(this ITemplateInfo ti)
        {
            var result = GetValueFromTag(ti, TagPrefix + "defaultInstance");

            if (string.IsNullOrEmpty(result))
            {
                result = ti.Name;
            }

            return result;
        }

        public static bool GetMultipleInstance(this ITemplateInfo ti)
        {
            var result = GetValueFromTag(ti, TagPrefix + "multipleInstance");

            if (!string.IsNullOrEmpty(result))
            {
                if (bool.TryParse(result, out bool boolResult))
                {
                    return boolResult;
                }
            }

            return true;
        }

        public static int GetGenGroup(this ITemplateInfo ti)
        {
            var result = GetValueFromTag(ti, TagPrefix + "genGroup");

            if (!string.IsNullOrEmpty(result))
            {
                if (int.TryParse(result, out int intResult))
                {
                    return intResult;
                }
            }

            return 0;
        }

        public static bool GetRightClickEnabled(this ITemplateInfo ti)
        {
            var result = GetValueFromTag(ti, TagPrefix + "rightClickEnabled");

            if (!string.IsNullOrEmpty(result))
            {
                if (bool.TryParse(result, out bool boolResult))
                {
                    return boolResult;
                }
            }

            return false;
        }

        public static bool GetItemNameEditable(this ITemplateInfo ti)
        {
            return ti.GetMultipleInstance();
        }

        public static bool GetOutputToParent(this ITemplateInfo ti)
        {
            var result = GetValueFromTag(ti, TagPrefix + "outputToParent");

            if (!string.IsNullOrEmpty(result))
            {
                if (bool.TryParse(result, out bool boolResult))
                {
                    return boolResult;
                }
            }

            return false;
        }

        private static string GetConfigDir(ITemplateInfo ti)
        {
            CodeGen.Instance.Settings.SettingsLoader.TryGetFileFromIdAndPath(ti.ConfigMountPointId, ti.ConfigPlace, out IFile file, out IMountPoint mountPoint);

            if (file?.Parent == null || mountPoint == null)
            {
                return null;
            }

            return Path.GetFullPath(mountPoint.Info.Place + file.Parent.FullPath);
        }

        public static string GetTelemetryName(this ITemplateInfo ti)
        {
            var telemName = GetValueFromTag(ti, TagPrefix + "telemName");

            if (telemName != null)
            {
                return telemName;
            }
            else
            {
                return ti.Name;
            }
        }

        private static string GetValueFromTag(this ITemplateInfo templateInfo, string tagName)
        {
            if (templateInfo.Tags != null && !string.IsNullOrEmpty(tagName) && templateInfo.Tags.TryGetValue(tagName, out ICacheTag tagValue))
            {
                return tagValue.DefaultValue;
            }

            return null;
        }
    }
}
