using Microsoft.TemplateEngine.Abstractions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using Microsoft.TemplateEngine.Abstractions.Mount;
using Microsoft.TemplateEngine.Edge.Settings;
using System.Text.RegularExpressions;
using Newtonsoft.Json;

namespace Microsoft.Templates.Core
{
    public static class ITemplateInfoExtensions
    {
        private const string TagPrefix = "uct.";
        private const string LicencesPattern = @"\[(?<text>.*?)\]\((?<url>.*?)\)\|?";

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
                case "framework":
                    return TemplateType.Framework;
                case "devfeature":
                    return TemplateType.DevFeature;
                default:
                    return TemplateType.Unspecified;
            }
        }

        public static string GetPostActionConfigPath(this ITemplateInfo ti)
        {
            var configDir = GetConfigDir(ti);

            return Directory.EnumerateFiles(configDir, "postactions.json").FirstOrDefault();
        }

        public static string GetIcon(this ITemplateInfo ti)
        {
            var configDir = GetConfigDir(ti);

            return Directory.EnumerateFiles(configDir, "icon.*").FirstOrDefault();
        }

        public static IEnumerable<(string text, string url)> GetLicences(this ITemplateInfo ti)
        {
            var licences = GetValueFromTag(ti, TagPrefix + "licences");
            if (string.IsNullOrWhiteSpace(licences))
            {
                return Enumerable.Empty<(string text, string url)>();
            }
            var result = new List<(string text, string url)>();

            var licencesMatches = Regex.Matches(licences, LicencesPattern);
            for (int i = 0; i < licencesMatches.Count; i++)
            {
                var m = licencesMatches[i];
                if (m.Success)
                {
                    result.Add((m.Groups["text"].Value, m.Groups["url"].Value));
                }

            }
            return result;
        }

        public static IEnumerable<(string name, string value)> GetExports(this ITemplateInfo ti)
        {
            if (ti == null || ti.Tags == null)
            {
                return Enumerable.Empty<(string name, string value)>();
            }

            return ti.Tags
                        .Where(t => t.Key.Contains(TagPrefix + "export."))
                        .Select(t => (t.Key.Replace(TagPrefix + "export.", string.Empty), t.Value))
                        .ToList();
        }

        public static List<string> GetFrameworkList(this ITemplateInfo ti)
        {
            var frameworks = GetValueFromTag(ti, TagPrefix + "framework");

            var result = new List<string>();

            if (!string.IsNullOrEmpty(frameworks))
            {
                result.AddRange(frameworks.Split(";".ToCharArray(), StringSplitOptions.RemoveEmptyEntries));
            }

            return result;
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
                int order;
                if (int.TryParse(rawOrder, out order))
                {
                    return order;
                }
            }
            return int.MaxValue;
        }

        public static string GetProjectType(this ITemplateInfo ti)
        {
            return GetValueFromTag(ti, TagPrefix + "ProjectType");
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

        private static string GetConfigDir(ITemplateInfo ti)
        {
            CodeGen.Instance.Settings.SettingsLoader.TryGetFileFromIdAndPath(ti.ConfigMountPointId, ti.ConfigPlace, out IFile file);
            if (file?.Parent == null)
            {
                return null;
            }
            return Path.GetFullPath(file.Parent.FullPath);
        }

        private static string GetValueFromTag(this ITemplateInfo templateInfo, string tagName)
        {
            if (templateInfo.Tags != null && !string.IsNullOrEmpty(tagName) && templateInfo.Tags.TryGetValue(tagName, out string tagValue))
            {
                return tagValue;
            }
            return null;
        }
    }
}
