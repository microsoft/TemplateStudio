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

namespace Microsoft.Templates.Core
{
    public static class ITemplateInfoExtensions
    {
        private const string TagPrefix = "uct.";

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

        public static string GetLicenceTerms(this ITemplateInfo ti)
        {
            var configDir = GetConfigDir(ti);
            var licenceFilePath = Path.Combine(configDir, "licence.txt");

            if (File.Exists(licenceFilePath))
            {
                return File.ReadAllText(licenceFilePath);
            }

            return null;
        }

        public static string GetFramework(this ITemplateInfo ti)
        {
            return GetValueFromTag(ti, TagPrefix + "framework");
        }

        public static List<string> GetFrameworkList(this ITemplateInfo ti)
        {
            var frameworks = GetValueFromTag(ti, TagPrefix + "framework");

            var result = new List<string>();
            if (!string.IsNullOrEmpty(frameworks))
            {
                if (!frameworks.Contains(';'))
                {
                    result.Add(frameworks);
                }
                else
                {
                    result.AddRange(frameworks.Split(';'));
                }
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

        private static string GetConfigDir(ITemplateInfo ti)
        {
            IFile file;
            CodeGen.Instance.Settings.SettingsLoader.TryGetFileFromIdAndPath(ti.ConfigMountPointId, ti.ConfigPlace, out file);
            if (file?.Parent == null)
            {
                return null;
            }
            return Path.GetFullPath(file.Parent.FullPath);
        }

        private static string GetValueFromTag(this ITemplateInfo templateInfo, string tagName)
        {
            string tagValue;
            if (templateInfo.Tags != null && !string.IsNullOrEmpty(tagName) && templateInfo.Tags.TryGetValue(tagName, out tagValue))
            {
                return tagValue;
            }
            return null;
        }
    }
}
