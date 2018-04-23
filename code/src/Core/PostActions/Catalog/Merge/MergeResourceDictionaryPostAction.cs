// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.Linq;
using Microsoft.Templates.Core.Gen;
using Microsoft.Templates.Core.Resources;

namespace Microsoft.Templates.Core.PostActions.Catalog.Merge
{
    public class MergeResourceDictionaryPostAction : MergePostAction
    {
        private const string MergeDictionaryPattern = @"<ResourceDictionary.MergedDictionaries>
    <!--^^-->
    <!--{[{-->
    <ResourceDictionary Source=""{filePath}""/>
    <!--}]}-->
</ResourceDictionary.MergedDictionaries>";

        public MergeResourceDictionaryPostAction(string relatedTemplate, MergeConfiguration config)
            : base(relatedTemplate, config)
        {
        }

        internal override void ExecuteInternal()
        {
            string originalFilePath = GetFilePath();
            if (!File.Exists(originalFilePath))
            {
                File.Copy(Config.FilePath, originalFilePath);
                GenContext.Current.ProjectItems.Add(originalFilePath.Replace(GenContext.Current.OutputPath, GenContext.Current.DestinationPath));
                AddToMergeDictionary(originalFilePath);
            }
            else
            {
                var mergeRoot = XElement.Load(Config.FilePath);
                var sourceRoot = XElement.Load(originalFilePath);

                foreach (var node in GetNodesToMerge(mergeRoot))
                {
                    var sourceNode = sourceRoot.Elements().FirstOrDefault(e => GetKey(e) == GetKey(node));
                    if (sourceNode == null)
                    {
                        AddNodeToSource(sourceRoot, node);
                    }
                    else
                    {
                        if (!XNode.DeepEquals(node, sourceNode))
                        {
                            var errorMessage = string.Format(StringRes.FailedMergePostActionKeyAlreadyDefined, GetKey(node), RelatedTemplate);
                            if (Config.FailOnError)
                            {
                                throw new InvalidDataException(errorMessage);
                            }
                            else
                            {
                                AddFailedMergePostActions(originalFilePath, MergeFailureType.KeyAlreadyDefined, errorMessage);
                                File.Delete(Config.FilePath);
                                return;
                            }
                        }
                    }
                }

                using (TextWriter writeFile = new StreamWriter(originalFilePath))
                {
                    var writer = new ResourceDictionaryWriter(writeFile);
                    writer.WriteResourceDictionary(sourceRoot);
                    writer.Flush();
                }
            }

            File.Delete(Config.FilePath);
        }

        private static void AddToMergeDictionary(string originalFilePath)
        {
            var relPath = originalFilePath.Replace(GenContext.Current.OutputPath, string.Empty).Replace(@"\", @"/");
            var postactionContent = MergeDictionaryPattern.Replace("{filePath}", relPath);
            var mergeDictionaryName = Path.GetFileNameWithoutExtension(originalFilePath);
            File.WriteAllText(GenContext.Current.OutputPath + $"/App${mergeDictionaryName}_gpostaction.xaml", postactionContent);
        }

        private static void AddNodeToSource(XElement sourceRoot, XElement node)
        {
            if (node.PreviousNode != null && node.PreviousNode.NodeType == XmlNodeType.Comment)
            {
                sourceRoot.Add(node.PreviousNode);
            }

            sourceRoot.Add(node);
        }

        private string GetKey(XElement node)
        {
            XNamespace ns = node.GetNamespaceOfPrefix("x");
            return node.Attribute(ns + "Key")?.Value;
        }

        private IEnumerable<XElement> GetNodesToMerge(XElement rootNode)
        {
            return rootNode.Descendants().Where(e => e.Attributes().Any(a => a.Name.LocalName == "Key"));
        }

        private string GetFilePath()
        {
            return Regex.Replace(Config.FilePath, MergeConfiguration.PostactionRegex, ".");
        }
    }
}
