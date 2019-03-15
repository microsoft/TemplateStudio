// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Xml.Linq;

using Microsoft.Templates.Core.Diagnostics;
using Microsoft.Templates.Core.Gen;

namespace Microsoft.Templates.Core.Services
{
    public static class ProjectMetadataService
    {
        private const string WindowsTemplateStudioLiteral = "Windows Template Studio";
        private const string GeneratorLiteral = "generator";
        private const string ProjectTypeLiteral = "projectType";
        private const string FrameworkLiteral = "framework";
        private const string TemplatesVersionLiteral = "templatesVersion";
        private const string PlatformLiteral = "platform";
        private const string MetadataLiteral = "Metadata";
        private const string NameAttribLiteral = "Name";
        private const string ValueAttribLiteral = "Value";
        private const string VersionAttribLiteral = "Version";
        private const string ItemLiteral = "Item";

        private static readonly XNamespace NS = "http://schemas.microsoft.com/appx/developer/windowsTemplateStudio";

        public static ProjectMetadata GetProjectMetadata()
        {
            var projectMetadata = new ProjectMetadata();

            try
            {
                var path = Path.Combine(GenContext.ToolBox.Shell.GetActiveProjectPath(), "Package.appxmanifest");
                if (File.Exists(path))
                {
                    var manifest = XElement.Load(path);
                    XNamespace ns = "http://schemas.microsoft.com/appx/developer/windowsTemplateStudio";

                    var metadata = manifest.Descendants().FirstOrDefault(e => e.Name.LocalName == MetadataLiteral && e.Name.Namespace == ns);

                    projectMetadata.ProjectType = metadata?.Descendants().FirstOrDefault(m => m.Attribute(NameAttribLiteral)?.Value == ProjectTypeLiteral)?.Attribute(ValueAttribLiteral)?.Value;
                    projectMetadata.Framework = metadata?.Descendants().FirstOrDefault(m => m.Attribute(NameAttribLiteral)?.Value == FrameworkLiteral)?.Attribute(ValueAttribLiteral)?.Value;
                    projectMetadata.Platform = metadata?.Descendants().FirstOrDefault(m => m.Attribute(NameAttribLiteral)?.Value == PlatformLiteral)?.Attribute(ValueAttribLiteral)?.Value;
                    projectMetadata.TemplatesVersion = metadata?.Descendants().FirstOrDefault(m => m.Attribute(NameAttribLiteral)?.Value == TemplatesVersionLiteral)?.Attribute(VersionAttribLiteral)?.Value;
                }
            }
            catch (Exception ex)
            {
                AppHealth.Current.Warning.TrackAsync("Exception reading projectType and framework from Package.appxmanifest", ex).FireAndForget();
            }

            return projectMetadata;
        }

        public static void SaveProjectMetadata(ProjectMetadata data)
        {
            try
            {
                var path = Path.Combine(GenContext.ToolBox.Shell.GetActiveProjectPath(), "Package.appxmanifest");
                if (File.Exists(path))
                {
                    var manifest = XElement.Load(path);
                    var metadata = manifest.Descendants().FirstOrDefault(e => e.Name.LocalName == MetadataLiteral && e.Name.Namespace == NS);
                    if (metadata == null)
                    {
                        metadata = new XElement(NS + MetadataLiteral);
                        manifest.Add(metadata);
                    }

                    metadata
                        .TryAddMetadaElement(GeneratorLiteral, WindowsTemplateStudioLiteral)
                        .TryAddMetadaElement(ProjectTypeLiteral, data.ProjectType)
                        .TryAddMetadaElement(FrameworkLiteral, data.Framework)
                        .TryAddMetadaElement(PlatformLiteral, data.Platform);

                    manifest.Save(path);
                }
            }
            catch (Exception ex)
            {
                AppHealth.Current.Warning.TrackAsync("Exception saving inferred projectType and framework to Package.appxmanifest", ex).FireAndForget();
                throw;
            }
        }

        private static XElement TryAddMetadaElement(this XElement metadata, string name, string value)
        {
            if (metadata?.Descendants().FirstOrDefault(m => m.Attribute(NameAttribLiteral)?.Value == name) == null)
            {
                metadata.Add(new XElement(NS + ItemLiteral, new XAttribute(NameAttribLiteral, name), new XAttribute(ValueAttribLiteral, value)));
            }

            return metadata;
        }
    }

    [SuppressMessage(
       "StyleCop.CSharp.MaintainabilityRules",
       "SA1402:File may only contain a single class",
       Justification = "For simplicity we're allowing generic and non-generic versions in one file.")]
    public class ProjectMetadata
    {
        public string TemplatesVersion { get; set; }

        public string ProjectType { get; set; }

        public string Framework { get; set; }

        public string Platform { get; set; }
    }
}
