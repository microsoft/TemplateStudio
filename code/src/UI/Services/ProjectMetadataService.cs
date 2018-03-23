// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using Microsoft.Templates.Core;
using Microsoft.Templates.Core.Diagnostics;
using Microsoft.Templates.Core.Gen;

namespace Microsoft.Templates.UI.Services
{
    public static class ProjectMetadataService
    {
        private const string ProjectTypeLiteral = "projectType";
        private const string FrameworkLiteral = "framework";
        private const string MetadataLiteral = "Metadata";
        private const string NameAttribLiteral = "Name";
        private const string ValueAttribLiteral = "Value";
        private const string ItemLiteral = "Item";

        public static ProjectMetadata GetProjectMetadata()
        {
            var projectMetadata = new ProjectMetadata();

            try
            {
                var path = Path.Combine(GenContext.Current.ProjectPath, "Package.appxmanifest");
                if (File.Exists(path))
                {
                    var manifest = XElement.Load(path);
                    XNamespace ns = "http://schemas.microsoft.com/appx/developer/windowsTemplateStudio";

                    var metadata = manifest.Descendants().FirstOrDefault(e => e.Name.LocalName == MetadataLiteral && e.Name.Namespace == ns);

                    // TODO: Falta la version!!
                    projectMetadata.ProjectType = metadata?.Descendants().FirstOrDefault(m => m.Attribute(NameAttribLiteral)?.Value == ProjectTypeLiteral)?.Attribute(ValueAttribLiteral)?.Value;
                    projectMetadata.Framework = metadata?.Descendants().FirstOrDefault(m => m.Attribute(NameAttribLiteral)?.Value == FrameworkLiteral)?.Attribute(ValueAttribLiteral)?.Value;
                }
            }
            catch (Exception ex)
            {
                AppHealth.Current.Warning.TrackAsync("Exception reading projectType and framework from Package.appxmanifest", ex).FireAndForget();
            }

            return projectMetadata;
        }

        public static void SaveProjectMetadata(string projectType, string framework)
        {
            try
            {
                var path = Path.Combine(GenContext.Current.ProjectPath, "Package.appxmanifest");
                if (File.Exists(path))
                {
                    var manifest = XElement.Load(path);
                    XNamespace ns = "http://schemas.microsoft.com/appx/developer/windowsTemplateStudio";

                    var metadata = manifest.Descendants().FirstOrDefault(e => e.Name.LocalName == MetadataLiteral && e.Name.Namespace == ns);
                    metadata.Add(new XElement(ns + ItemLiteral, new XAttribute(NameAttribLiteral, ProjectTypeLiteral), new XAttribute(ValueAttribLiteral, projectType)));
                    metadata.Add(new XElement(ns + ItemLiteral, new XAttribute(NameAttribLiteral, FrameworkLiteral), new XAttribute(ValueAttribLiteral, framework)));

                    manifest.Save(path);
                }
            }
            catch (Exception ex)
            {
                AppHealth.Current.Warning.TrackAsync("Exception saving inferred projectType and framework to Package.appxmanifest", ex).FireAndForget();
                throw;
            }
        }
    }

    public class ProjectMetadata
    {
        public string TemplateVersion { get; set; }

        public string ProjectType { get; set; }

        public string Framework { get; set; }
    }
}
