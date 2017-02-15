using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.TemplateEngine.Edge.Template;
using System.IO;
using System.Xml.Linq;
using System.Text.RegularExpressions;

namespace Microsoft.Templates.Wizard.PostActions.Catalog
{
    public class LocalizationPostAction : PostActionBase
    {
        private const string AnchorPattern = @"\""" + LocAnchor.Tag + @"(?<Key>\w+)~(?<Value>.+?)\""";

        public LocalizationPostAction() : base("Localization", "Create localization resources", null)
        {
        }

        public override PostActionResult Execute(string outputPath, GenInfo context, TemplateCreationResult generationResult, GenShell shell)
        {
            var projectResources = GetResources(shell.ProjectPath);

            if (projectResources == null || !projectResources.Any())
            {
                //TODO: SET MESSAGE
                return new PostActionResult
                {
                    ResultCode = ResultCode.Error
                };
            }

            foreach (var projectItemFile in Directory.EnumerateFiles(shell.ProjectPath, "*", SearchOption.AllDirectories))
            {
                //TODO: THIS SHOULD BE DONE IN UPDATER
                var fileContent = File.ReadAllText(projectItemFile);
                if (fileContent.IndexOf(LocAnchor.Tag) > -1)
                {
                    var updater = GetUpdater(projectItemFile);
                    if (updater == null)
                    {
                        continue;
                    }
                    var updateResult = updater.Update(fileContent);

                    if (updateResult == null)
                    {
                        //TODO: SET MESSAGE
                        return new PostActionResult
                        {
                            ResultCode = ResultCode.Error
                        };
                    }
                    foreach (var anchor in updateResult.Anchors)
                    {
                        projectResources.ForEach(r => r.element.Add(anchor.ToXml()));
                    }


                    File.WriteAllText(projectItemFile, updateResult.Content);
                }
            }

            projectResources.ForEach(r => r.element.Save(r.fileName));

            return new PostActionResult
            {
                ResultCode = ResultCode.Success
            };
        }

        public static bool CleanUpAnchors(ref string fileContent)
        {
            var modified = false;

            if (fileContent.IndexOf(LocAnchor.Tag) > -1)
            {
                var matches = Regex.Matches(fileContent, AnchorPattern);
                for (int i = 0; i < matches.Count; i++)
                {
                    var m = matches[i];

                    var replacement = $"\"{m.Groups["Value"].Value}\"";
                    fileContent = fileContent.Replace(m.Value, replacement);

                    modified = true;
                }
            }

            return modified;
        }

        private static List<(string fileName, XElement element)> GetResources(string path)
        {
            return Directory.EnumerateFiles(path, "*.resw", SearchOption.AllDirectories)
                                .Select(f => (f, XElement.Load(f)))
                                .ToList();
        }

        private static FileLocUpdater GetUpdater(string fileName)
        {
            if (Path.GetExtension(fileName) == ".xaml")
            {
                return new XmlLocUpdater();
            }
            else if (Path.GetExtension(fileName) == ".cs")
            {
                return new CsLocUpdater();
            }

            return null;
        }

    }

    abstract class FileLocUpdater
    {
        public abstract UpdateResult Update(string content);
    }

    class XmlLocUpdater : FileLocUpdater
    {
        private const string AnchorPattern = @"(?<Attribute>[a-zA-Z0-9]+)=\""" + LocAnchor.Tag + @"(?<Key>\w+)~(?<Value>.+?)\""";

        public override UpdateResult Update(string content)
        {
            var result = new UpdateResult();
            result.Content = content;

            var matches = Regex.Matches(content, AnchorPattern);
            for (int i = 0; i < matches.Count; i++)
            {
                var m = matches[i];

                result.Anchors.Add(new LocAnchor
                {
                    Key = $"{m.Groups["Key"].Value}.{m.Groups["Attribute"].Value}",
                    Value = m.Groups["Value"].Value
                });

                var replacement = $"x:Uid=\"{m.Groups["Key"].Value}\"";
                if (result.Content.IndexOf(replacement) > -1)
                {
                    result.Content = result.Content.Replace(m.Value, string.Empty);
                }
                else
                {
                    result.Content = result.Content.Replace(m.Value, replacement);
                }
            }

            return result;
        }
    }

    class CsLocUpdater : FileLocUpdater
    {
        private const string AnchorPattern = @"\""" + LocAnchor.Tag + @"(?<Key>\w+)~(?<Value>.+?)\""";
        private const string ExtensionMethod = "GetLocalized";

        public override UpdateResult Update(string content)
        {
            var result = new UpdateResult()
            {
                Content = content
            };

            var matches = Regex.Matches(content, AnchorPattern);
            for (int i = 0; i < matches.Count; i++)
            {
                var m = matches[i];

                result.Anchors.Add(new LocAnchor
                {
                    Key = m.Groups["Key"].Value,
                    Value = m.Groups["Value"].Value
                });
                var replacement = $"\"{m.Groups["Key"].Value}\".{ExtensionMethod}()";

                result.Content = result.Content.Replace(m.Value, replacement);
            }

            return result;
        }
    }

    class UpdateResult
    {
        public string Content { get; set; }
        public List<LocAnchor> Anchors { get; } = new List<LocAnchor>();
    }

    class LocAnchor
    {
        public const string Tag = "LOC_ANCHOR:";

        public string Key { get; set; }
        public string Value { get; set; }

        public XElement ToXml()
        {

            var xml = new XElement("data"
                                    , new XAttribute("name", Key)
                                    , new XAttribute(XNamespace.Xml + "space", "preserve")
                                    , new XElement("value", Value)
                                    );
            return xml;
        }
    }
}
