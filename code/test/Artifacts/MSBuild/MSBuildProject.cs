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
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Microsoft.Templates.Test.Artifacts.MSBuild
{

    public class MsBuildProject
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

        private MsBuildProject(string path)
        {
            _path = path;
            Name = Path.GetFileNameWithoutExtension(path);
            _root = XElement.Load(path);
        }

        public static MsBuildProject Load(string path)
        {
            return new MsBuildProject(path);
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
            StringBuilder sb = new StringBuilder();
            sb.Append($"<ProjectReference Include=\"{includePath}\"");
            sb.AppendLine(">");
            sb.AppendLine($"<Project>{projectGuid}</Project>");
            sb.AppendLine($"<Name>{projectName}</Name>");
            sb.AppendLine("</ProjectReference>");

            StringReader sr = new StringReader(sb.ToString());
            XElement itemElement = XElement.Load(sr);
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

