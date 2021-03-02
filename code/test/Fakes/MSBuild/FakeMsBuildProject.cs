// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using Microsoft.Templates.Core.Gen;

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

                // Generate a GUID if the proj file doesn't include one (such as NetStandard projects)
                return nsElement?.Value ?? string.Empty;
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
            if (itemPath.EndsWith("xaml.cpp"))
            {
                AddItem(itemPath.Replace("xaml.cpp", "idl"));
            }
        }

        public void AddNugetReference(NugetReference nugetReference)
        {
            var isCpsProject = IsCpsProject();
            if (NugetReferenceExists(nugetReference, isCpsProject))
            {
                return;
            }

            XElement element = GetNugetReferenceXElement(nugetReference.PackageId, nugetReference.Version.ToString(), isCpsProject);
            ApplyNs(element);

            var firstPackageReference = _root.Descendants().FirstOrDefault(d => d.Name.LocalName == "PackageReference");

            if (firstPackageReference != null)
            {
                firstPackageReference.AddBeforeSelf(element);
            }
            else
            {
                var itemsContainer = new XElement(_root.GetDefaultNamespace() + "ItemGroup");
                itemsContainer.Add(element);
                _root.Add(itemsContainer);
            }
        }

        public void AddNugetImport(NugetReference nugetReference)
        {
            if (NugetImportExists(nugetReference))
            {
                return;
            }

            XElement targetsElement = GetPackageImportXElement(nugetReference.PackageId, nugetReference.Version.ToString(), "targets");
            ApplyNs(targetsElement);

            var importGroup = _root.Descendants().FirstOrDefault(d => d.Name.LocalName == "ImportGroup" && d.FirstAttribute.Value == "ExtensionTargets");

            if (importGroup != null)
            {
                importGroup.AddFirst(targetsElement);
            }

            XElement propsElement = GetPackageImportXElement(nugetReference.PackageId, nugetReference.Version.ToString(), "props");
            ApplyNs(propsElement);
            _root.AddFirst(propsElement);
        }

        public void AddSDKReference(SdkReference sdkReference)
        {
            if (ItemExists(sdkReference.Sdk))
            {
                return;
            }

            var itemsContainer = new XElement(_root.GetDefaultNamespace() + "ItemGroup");

            XElement element = GetSdkReferenceXElement(sdkReference.Name, sdkReference.Sdk.ToString());
            ApplyNs(element);
            itemsContainer.Add(element);

            _root.Add(itemsContainer);
        }

        public void AddProjectReference(string projectPath, string projguid, string projectName)
        {
            string itemRelativePath = "..\\" + projectPath.Replace($@"{Path.GetDirectoryName(Path.GetDirectoryName(_path))}\", string.Empty);

            if (ItemExists(itemRelativePath))
            {
                return;
            }

            var container = new XElement(_root.GetDefaultNamespace() + "ItemGroup");

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

        private static XElement GetNugetReferenceXElement(string package, string version, bool isCoreProject)
        {
            var sb = new StringBuilder();
            if (isCoreProject)
            {
                sb.Append($"<PackageReference Include=\"{package}\" Version=\"{version}\" />");
            }
            else
            {
                sb.Append($"<PackageReference Include=\"{package}\"");
                sb.AppendLine(">");
                sb.AppendLine($"<Version>{version}</Version>");
                sb.AppendLine("</PackageReference>");
            }

            var itemElement = XElement.Parse(sb.ToString());

            return itemElement;
        }

        private static XElement GetPackageImportXElement(string package, string version, string importFile)
        {
            var sb = new StringBuilder();
            sb.Append($"<Import Project=\"..\\packages\\{package}.{version}\\build\\native\\{package}.{importFile}\" Condition=\"Exists(\'..\\packages\\{package}.{version}\\build\\native\\{package}.{importFile}\')\"/>");

            var itemElement = XElement.Parse(sb.ToString());

            return itemElement;
        }

        private static XElement GetSdkReferenceXElement(string name, string sdkReference)
        {
            var sb = new StringBuilder();

            sb.Append($"<SDKReference Include=\"{sdkReference}\"");
            sb.AppendLine(">");
            sb.AppendLine($"<Name>{name}</Name>");
            sb.AppendLine("</SDKReference>");

            var itemElement = XElement.Parse(sb.ToString());

            return itemElement;
        }

        public void Save()
        {
            if (IsCpsProject())
            {
                using (var writer = XmlWriter.Create(_path, new XmlWriterSettings { OmitXmlDeclaration = true, Indent = true }))
                {
                    _root.Save(writer);
                }
            }
            else
            {
                _root.Save(_path);
            }
        }

        private void ApplyNs(XElement e)
        {
            foreach (var d in e.DescendantsAndSelf())
            {
                d.Name = _root.GetDefaultNamespace() + d.Name.LocalName;
            }
        }

        private bool IsCpsProject()
        {
            return _root.Descendants().Any(d => d.Name == "TargetFramework" || d.Name == "TargetFrameworks");
        }

        private bool ItemExists(string itemPath)
        {
            return _root.Descendants().Any(d => d.Attribute("Include") != null
                && d.Attribute("Include").Value.Equals(itemPath, StringComparison.OrdinalIgnoreCase));
        }

        private bool NugetReferenceExists(NugetReference nuget, bool isCpsProject)
        {
            if (isCpsProject)
            {
                return _root.Descendants().Any(
                    d => d.Attribute("Include") != null &&
                    d.Attribute("Include").Value.Equals(nuget.PackageId, StringComparison.OrdinalIgnoreCase) &&
                    d.Attribute("Version") != null &&
                    d.Attribute("Version").Value.Equals(nuget.PackageId, StringComparison.OrdinalIgnoreCase));
            }
            else
            {
                return _root.Descendants().Any(
                    d => d.Attribute("Include") != null &&
                    d.Attribute("Include").Value.Equals(nuget.PackageId, StringComparison.OrdinalIgnoreCase) &&
                    d.Value == nuget.Version );
            }
        }

        private bool NugetImportExists(NugetReference nuget)
        {
                return _root.Descendants().Any(
                    d => d.Attribute("Project") != null &&
                    d.Attribute("Project").Value.Contains($"{nuget.PackageId}.{nuget.Version}"));
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
                case ".idl":
                    returnType = VsItemType.Midl;

                    break;
                case ".h":
                    if (fileName.EndsWith(".xaml.h", StringComparison.OrdinalIgnoreCase))
                    {
                        returnType = VsItemType.ClIncludeWithDependant;
                    }
                    else
                    {
                        returnType = VsItemType.ClInclude;
                    }

                    break;
                case ".cpp":
                    if (fileName.EndsWith(".xaml.cpp", StringComparison.OrdinalIgnoreCase))
                    {
                        returnType = VsItemType.ClCompiledWithDependant;
                    }
                    else
                    {
                        returnType = VsItemType.ClCompiled;
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
