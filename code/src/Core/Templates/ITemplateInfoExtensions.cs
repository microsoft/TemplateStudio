// ******************************************************************
// Copyright (c) Microsoft. All rights reserved.
// This code is licensed under the MIT License (MIT).
// THE CODE IS PROVIDED “AS IS”, WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED,
// INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.
// IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
// DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT,
// TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH
// THE CODE OR THE USE OR OTHER DEALINGS IN THE CODE.
// ******************************************************************

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

using Microsoft.TemplateEngine.Abstractions;
using Microsoft.TemplateEngine.Abstractions.Mount;
using Microsoft.Templates.Core.Composition;

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
            switch (type?.ToLower())
            {
                case "project":
                    return TemplateType.Project;
                case "page":
                    return TemplateType.Page;
                case "feature":
                    return TemplateType.Feature;
                case "composition":
                    return TemplateType.Composition;
                default:
                    return TemplateType.Unspecified;
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
            var descriptionFile = Directory.EnumerateFiles(configDir, "description.md").FirstOrDefault();

            if (!string.IsNullOrEmpty(descriptionFile))
            {
                return File.ReadAllText(descriptionFile);
            }

            return null;
        }

        public static string GetSafeIdentity(this ITemplateInfo ti)
        {
            if (!string.IsNullOrEmpty(ti.GroupIdentity))
            {
                var identityChunks = ti.GroupIdentity.Split(".".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                return identityChunks.Last();
            }

            return ti.Identity;
        }

        public static string GetCompositionFilter(this ITemplateInfo ti)
        {
            return GetValueFromTag(ti, TagPrefix + "compositionFilter");
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
                        Url = m.Groups["url"].Value
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
                properties.Add(new QueryableProperty(nameof(ti.Name).ToLower(), ti.Name));
                properties.Add(new QueryableProperty(nameof(ti.Identity).ToLower(), ti.Identity));
                properties.Add(new QueryableProperty(nameof(ti.GroupIdentity).ToLower(), ti.GroupIdentity));

                foreach (var t in ti.Tags)
                {
                    properties.Add(new QueryableProperty(t.Key, t.Value.DefaultValue));
                }
            }

            return properties;
        }

        public static IEnumerable<(string name, string value)> GetExports(this ITemplateInfo ti)
        {
            if (ti == null || ti.Tags == null)
            {
                return Enumerable.Empty<(string name, string value)>();
            }

            return ti.Tags
                        .Where(t => t.Key.Contains(TagPrefix + "export."))
#pragma warning disable SA1008 // Opening parenthesis must be spaced correctly - StyleCop can't handle Tuples
                        .Select(t => (t.Key.Replace(TagPrefix + "export.", string.Empty), t.Value.DefaultValue))
#pragma warning restore SA1008 // Opening parenthesis must be spaced correctly
                        .ToList();
        }

        public static List<string> GetFrameworkList(this ITemplateInfo ti)
        {
            var frameworks = GetValueFromTag(ti, TagPrefix + "framework");
            var result = new List<string>();

            if (!string.IsNullOrEmpty(frameworks))
            {
                result.AddRange(frameworks.Split(Separator.ToCharArray(), StringSplitOptions.RemoveEmptyEntries));
            }

            return result;
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

        public static string GetVersion(this ITemplateInfo ti)
        {
            return GetValueFromTag(ti, TagPrefix + "version");
        }

        public static int GetOrder(this ITemplateInfo ti)
        {
            var rawOrder = GetValueFromTag(ti, TagPrefix + "order");

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

        public static bool GetItemNameEditable(this ITemplateInfo ti)
        {
            return (ti.GetTemplateType() == TemplateType.Page || ti.GetMultipleInstance());
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
