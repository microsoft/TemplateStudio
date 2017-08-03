// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Localization
{
    internal static class XmlUtility
    {
        internal static XmlDocument LoadXmlFile(string filePath)
        {
            XmlDocument xmlFile = new XmlDocument();
            xmlFile.Load(filePath);
            return xmlFile;
        }

        internal static XmlNode GetNode(XmlDocument xmlFile, string nodeName)
        {
            XmlNodeList nodes = xmlFile.GetElementsByTagName(nodeName);
            if (nodes == null || nodes.Count == 0)
                throw new Exception($"Node \"{nodeName}\" not found in XmlDocument.");
            if (nodes.Count > 1)
                throw new Exception($"There were more than one \"{nodeName}\" node XmlDocument.");
            return nodes[0];
        }

        internal static void SetNodeText(XmlDocument xmlFile, string nodeName, string nodeText)
        {
            XmlNode node = GetNode(xmlFile, nodeName);
            node.InnerText = nodeText;
        }

        internal static void InsertNewNodeAfter(XmlNode node, string newNodeName, string newNodeText)
        {
            XmlDocument xmlFile = node.OwnerDocument;
            XmlNode newNode = xmlFile.CreateNode(XmlNodeType.Element, newNodeName, node.NamespaceURI);
            newNode.InnerText = newNodeText;
            node.ParentNode.InsertAfter(newNode, node);
        }

        internal static void AppendNewChildNode(XmlDocument xmlFile, string nodeName, string newNodeName, string newNodeText)
        {
            XmlNode node = GetNode(xmlFile, nodeName);
            XmlNode newNode = xmlFile.CreateNode(XmlNodeType.Element, newNodeName, node.NamespaceURI);
            newNode.InnerText = newNodeText;
            node.AppendChild(newNode);
        }
    }
}
