// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Globalization;
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

        private bool _isProjitemsFile;

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

        public string ProjectTypeGuids
        {
            get
            {
                var nsElement = _root.Descendants().FirstOrDefault(e => e.Name.LocalName == "ProjectTypeGuids");
                return nsElement?.Value;
            }
        }

        private FakeMsBuildProject(string path)
        {
            _path = path;
            _isProjitemsFile = Path.GetExtension(_path) == ".projitems" ? true : false;
            Name = Path.GetFileNameWithoutExtension(path);
            _root = XElement.Load(path);
        }

        public static FakeMsBuildProject Load(string path)
        {
            return new FakeMsBuildProject(path);
        }

        public void AddItem(string itemPath)
        {
            var itemRelativePath = itemPath.Replace($@"{Path.GetDirectoryName(_path)}\", string.Empty).Replace(@".\", string.Empty);

            if (_isProjitemsFile)
            {
                itemRelativePath = "$(MSBuildThisFileDirectory)" + itemRelativePath;
            }

            if (ItemExists(itemRelativePath))
            {
                return;
            }

            var itemsContainer = new XElement(_root.GetDefaultNamespace() + "ItemGroup");

            XElement element = GetItemType(itemRelativePath).GetXmlDefinition(itemRelativePath, _isProjitemsFile);
            ApplyNs(element);
            itemsContainer.Add(element);

            var lastItems = _root.Descendants().LastOrDefault(d => d.Name.LocalName == "ItemGroup");
            lastItems?.AddAfterSelf(itemsContainer);
        }

        public void AddProjectReference(string projectPath, string projguid, string projectName)
        {
            var container = new XElement(_root.GetDefaultNamespace() + "ItemGroup");
            string itemRelativePath = "..\\" + projectPath.Replace($@"{Path.GetDirectoryName(Path.GetDirectoryName(_path))}\", string.Empty);
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
            VsItemType returnType = VsItemType.Content;

            switch (Path.GetExtension(fileName).ToLowerInvariant())
            {
                case ".cs":
                    if (fileName.EndsWith(".xaml.cs", StringComparison.OrdinalIgnoreCase))
                    {
                        returnType = VsItemType.CompiledWithDependant;
                    }
                    else
                    {
                        returnType = VsItemType.Compiled;
                    }

                    break;
                case ".vb":
                    if (fileName.EndsWith(".xaml.vb", StringComparison.OrdinalIgnoreCase))
                    {
                        returnType = VsItemType.CompiledWithDependant;
                    }
                    else
                    {
                        returnType = VsItemType.Compiled;
                    }

                    break;
                case ".xaml":
                    returnType = VsItemType.XamlPage;
                    break;
                case ".resw":
                    returnType = VsItemType.Resource;
                    break;
                case ".pfx":
                    returnType = VsItemType.None;
                    break;
                default:
                    returnType = VsItemType.Content;
                    break;
            }

            return returnType;
        }
    }
}
