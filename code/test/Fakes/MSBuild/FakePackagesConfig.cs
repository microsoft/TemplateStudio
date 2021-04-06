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
    public class FakePackagesConfig
    {
        private string _path;

        private XElement _root;

        public string Name { get; }

        private FakePackagesConfig(string path)
        {
            _path = path;
            Name = Path.GetFileNameWithoutExtension(path);
            _root = XElement.Load(path);
        }

        public static FakePackagesConfig Load(string path)
        {
            return new FakePackagesConfig(path);
        }

        public void AddNugetReference(NugetReference nugetReference)
        {
            if (NugetReferenceExists(nugetReference))
            {
                return;
            }

            XElement element = GetNugetReferenceXElement(nugetReference.PackageId, nugetReference.Version.ToString());

            var firstPackageReference = _root.Descendants().FirstOrDefault(d => d.Name.LocalName == "package");

            if (firstPackageReference != null)
            {
                firstPackageReference.AddBeforeSelf(element);
            }
            else
            {
                _root.Add(element);
            }
        }

        private static XElement GetNugetReferenceXElement(string package, string version)
        {
            var sb = new StringBuilder();

            sb.Append($"<package id=\"{package}\" version=\"{version}\" targetFramework=\"native\" />");
            var itemElement = XElement.Parse(sb.ToString());

            return itemElement;
        }

        public void Save()
        {
            _root.Save(_path);
        }

        private bool NugetReferenceExists(NugetReference nuget)
        {
            return _root.Descendants().Any(
                d => d.Attribute("id") != null &&
                d.Attribute("id").Value.Equals(nuget.PackageId, StringComparison.OrdinalIgnoreCase) &&
                d.Attribute("version") != null &&
                d.Attribute("version").Value.Equals(nuget.PackageId, StringComparison.OrdinalIgnoreCase));
        }
    }
}
