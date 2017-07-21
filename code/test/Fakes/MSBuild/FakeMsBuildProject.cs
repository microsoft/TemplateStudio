// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace Microsoft.Templates.Fakes
{

    public class FakeMsBuildProject
    {
        private const string MsBuildNs = "http://schemas.microsoft.com/developer/msbuild/2003";
        private string _path;
        private XElement _root;

        public string Name { get; }
        public string Namespace
        {
            get
            {
                var nsElement = _root.Descendants().FirstOrDefault(e => e.Name.LocalName == "RootNamespace");
                return nsElement?.Value;
            }
        }

        public string Guid
        {
            get
            {
                var nsElement = _root.Descendants().FirstOrDefault(e => e.Name.LocalName == "ProjectGuid");
                return nsElement?.Value;
            }
        }

        private FakeMsBuildProject(string path)
        {
            _path = path;
            Name = Path.GetFileNameWithoutExtension(path);
            _root = XElement.Load(path);
        }

        public static FakeMsBuildProject Load(string path)
        {
            return new FakeMsBuildProject(path);
        }

        public void AddItem(string itemPath)
        {
            var itemRelativePath = itemPath.Replace($@"{Path.GetDirectoryName(_path)}\", "").Replace(@".\", "");
            if (ItemExists(itemRelativePath))
            {
                return;
            }

            var itemsContainer = new XElement(_root.GetDefaultNamespace() + "ItemGroup");

            XElement element = GetItemType(itemRelativePath).GetXmlDefinition(itemRelativePath);
            ApplyNs(element);
            itemsContainer.Add(element);

            var lastItems = _root.Descendants().LastOrDefault(d => d.Name.LocalName == "ItemGroup");
            lastItems?.AddAfterSelf(itemsContainer);
        }

        public void AddProjectReference(string projectPath, string projguid, string projectName)
        {
            var container = new XElement(_root.GetDefaultNamespace() + "ItemGroup");
            string itemRelativePath = "..\\" + projectPath.Replace($@"{Path.GetDirectoryName(Path.GetDirectoryName(_path))}\", "");
            XElement element = GetProjectReferenceXElement(itemRelativePath, projguid, projectName);
            ApplyNs(element);
            container.Add(element);

            var lastItems = _root.Descendants().LastOrDefault(d => d.Name.LocalName == "ItemGroup");
            lastItems?.AddAfterSelf(container);
        }

        private static XElement GetProjectReferenceXElement(string includePath, string projectGuid, string projectName)
        {
            var sb = new StringBuilder();
            sb.Append($"<ProjectReference Include=\"{includePath}\"");
            sb.AppendLine(">");
            sb.AppendLine($"<Project>{projectGuid}</Project>");
            sb.AppendLine($"<Name>{projectName}</Name>");
            sb.AppendLine("</ProjectReference>");

            var sr = new StringReader(sb.ToString());
            var itemElement = XElement.Load(sr);
            return itemElement;
        }

        public void Save()
        {
            _root.Save(_path);
        }

        private void ApplyNs(XElement e)
        {
            foreach (var d in e.DescendantsAndSelf())
            {
                d.Name = _root.GetDefaultNamespace() + d.Name.LocalName;
            }
        }

        private bool ItemExists(string itemPath)
        {
            return _root.Descendants().Any(d => d.Attribute("Include") != null
                && d.Attribute("Include").Value.Equals(itemPath, StringComparison.OrdinalIgnoreCase));
        }

        private VsItemType GetItemType(string fileName)
        {
            string ext = Path.GetExtension(fileName).ToLower();
            if (ext == ".cs")
            {
                if (fileName.ToLower().Contains(".xaml.cs"))
                {
                    return VsItemType.CompiledWithDependant;
                }
                else
                {
                    return VsItemType.Compiled;
                }
            }
            else if (ext == ".vb")
            {
                if (fileName.ToLower().Contains(".xaml.vb"))
                {
                    return VsItemType.CompiledWithDependant;
                }
                else
                {
                    return VsItemType.Compiled;
                }
            }
            else if (ext == ".xaml")
            {
                return VsItemType.XamlPage;
            }
            else if (ext == ".resw")
            {
                return VsItemType.Resource;
            }
            else
            {
                return VsItemType.Content;
            }
        }

    }
}

