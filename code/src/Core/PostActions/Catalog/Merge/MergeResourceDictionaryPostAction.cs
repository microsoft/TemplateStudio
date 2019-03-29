// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.Linq;

using Microsoft.Templates.Core.Extensions;
using Microsoft.Templates.Core.Gen;
using Microsoft.Templates.Core.Resources;

namespace Microsoft.Templates.Core.PostActions.Catalog.Merge
{
    public class MergeResourceDictionaryPostAction : MergePostAction
    {
        private const string MergeDictionaryPattern = @"<ResourceDictionary.MergedDictionaries>
    <!--^^-->
    <!--{[{-->
    <ResourceDictionary Source=""/{filePath}""/>
    <!--}]}-->
</ResourceDictionary.MergedDictionaries>";

        public MergeResourceDictionaryPostAction(string relatedTemplate, MergeConfiguration config)
            : base(relatedTemplate, config)
        {
        }

        internal override void ExecuteInternal()
        {
            string originalFilePath = Regex.Replace(Config.FilePath, MergeConfiguration.PostactionRegex, ".");
            if (!File.Exists(originalFilePath))
            {
                // If original file does not exist, rename the postaction file, add it to projectitems and app.xamls mergedictionary
                File.Copy(Config.FilePath, originalFilePath);
                GenContext.Current.ProjectInfo.ProjectItems.Add(originalFilePath.GetDestinationPath());
                AddToAppMergeDictionary(originalFilePath);
            }
            else
            {
                var mergeRoot = XElement.Load(Config.FilePath);
                var sourceRoot = XElement.Load(originalFilePath);

                var originalEncoding = GetEncoding(originalFilePath);

                // Only check encoding on new project, might have changed on right click
                if (GenContext.Current.GenerationOutputPath == GenContext.Current.DestinationPath)
                {
                    var otherEncoding = GetEncoding(Config.FilePath);

                    if (originalEncoding.EncodingName != otherEncoding.EncodingName
                        || !Enumerable.SequenceEqual(originalEncoding.GetPreamble(), otherEncoding.GetPreamble()))
                    {
                        HandleMismatchedEncodings(originalFilePath, Config.FilePath, originalEncoding, otherEncoding);
                        return;
                    }
                }

                foreach (var node in GetNodesToMerge(mergeRoot))
                {
                    var sourceNode = sourceRoot.Elements().FirstOrDefault(e => GetKey(e) == GetKey(node));
                    if (sourceNode == null)
                    {
                        AddNode(sourceRoot, node);
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
                                var relativeFilePath = originalFilePath.GetPathRelativeToGenerationParentPath();
                                HandleFailedMergePostActions(relativeFilePath, MergeFailureType.KeyAlreadyDefined, MergeConfiguration.Suffix, errorMessage);
                                return;
                            }
                        }
                    }
                }

                using (TextWriter writeFile = new StreamWriter(originalFilePath, false, originalEncoding))
                {
                    var writer = new ResourceDictionaryWriter(writeFile);
                    writer.WriteResourceDictionary(sourceRoot);
                    writer.Flush();
                }
            }

            File.Delete(Config.FilePath);
        }

        private void AddToAppMergeDictionary(string originalFilePath)
        {
            // Write postaction to include this file to mergedictionary
            var relPath = originalFilePath.GetPathRelativeToGenerationPath().Replace(@"\", @"/");
            var postactionContent = MergeDictionaryPattern.Replace("{filePath}", relPath);
            var mergeDictionaryName = Path.GetFileNameWithoutExtension(originalFilePath);
            File.WriteAllText(GenContext.Current.GenerationOutputPath + $"/App${mergeDictionaryName}_gpostaction.xaml", postactionContent, Encoding.UTF8);
        }

        private static void AddNode(XElement sourceRoot, XElement node)
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
    }
}
