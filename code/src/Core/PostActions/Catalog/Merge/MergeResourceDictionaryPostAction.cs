// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.Linq;
using Microsoft.Templates.Core.Gen;
using Microsoft.Templates.Core.Resources;

namespace Microsoft.Templates.Core.PostActions.Catalog.Merge
{
    public class MergeResourceDictionaryPostAction : MergePostAction
    {
        public MergeResourceDictionaryPostAction(MergeConfiguration config) : base(config)
        {
        }

        public override void Execute()
        {
            string originalFilePath = GetFilePath();
            if (!File.Exists(originalFilePath))
            {
                File.Copy(_config.FilePath, originalFilePath);
                GenContext.Current.ProjectItems.Add(originalFilePath);

                // TODO: Create postaction file for App.xaml
            }
            else
            {
                var mergeRoot = XElement.Load(_config.FilePath);
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
                        if (!CompareNodes(node, sourceNode))
                        {
                            var errorMessage = string.Format(StringRes.FailedMergePostActionKeyAlreadyDefined, GetKey(node), GetRelativePath(originalFilePath));
                            if (_config.FailOnError)
                            {
                                // TODO: Throw exception
                                throw new InvalidDataException(errorMessage);
                            }
                            else
                            {
                                AddFailedMergePostActions(originalFilePath, MergeFailureType.KeyAlreadyDefined, errorMessage);
                                File.Delete(_config.FilePath);
                                return;
                            }
                        }
                    }
                }

                XmlWriterSettings settings = new XmlWriterSettings();
                settings.OmitXmlDeclaration = true;
                settings.Indent = true;
                settings.IndentChars = "    ";
                using (XmlWriter writer = XmlTextWriter.Create(originalFilePath, settings))
                {
                    sourceRoot.Save(writer);
                }
            }

            File.Delete(_config.FilePath);
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
            return node.Attribute(ns + "Key").Value;
        }

        private IEnumerable<XElement> GetNodesToMerge(XElement rootNode)
        {
            return rootNode.Descendants().Where(e => e.Attributes().Any(a => a.Name.LocalName == "Key"));
        }

        private bool CompareNodes(XElement mergeNode, XElement sourceNode)
        {
            if (mergeNode.Value != sourceNode.Value)
            {
                return false;
            }
            else
            {
                if (mergeNode.Elements().Count() != sourceNode.Elements().Count())
                {
                    return false;
                }
                else
                {
                    foreach (var mergeChildNode in mergeNode.Elements())
                    {
                        var sourceChildNode = sourceNode.Elements().FirstOrDefault(e => e.Attribute("Property").Value == mergeChildNode.Attribute("Property").Value);

                        if (sourceChildNode == null || sourceChildNode.Attribute("Value").Value != mergeChildNode.Attribute("Value").Value)
                        {
                            return false;
                        }
                    }
                }
            }
            return true;
        }

        private string GetFilePath()
        {
            return Regex.Replace(_config.FilePath, MergeConfiguration.PostactionRegex, ".");
        }
    }
}
