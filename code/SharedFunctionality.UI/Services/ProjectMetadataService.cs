// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Xml.Linq;

using Microsoft.Templates.Core.Diagnostics;

namespace Microsoft.Templates.Core.Services
{
    public static class ProjectMetadataService
    {
        private const string WindowsTemplateStudioLiteral = "Template Studio";
        private const string GeneratorLiteral = "generator";
        private const string ProjectTypeLiteral = "projectType";
        private const string FrameworkLiteral = "framework";
        private const string WizardVersionLiteral = "wizardVersion";
        private const string PlatformLiteral = "platform";
        private const string AppModelLiteral = "appmodel";
        private const string MetadataLiteral = "Metadata";
        private const string NameAttribLiteral = "Name";
        private const string ValueAttribLiteral = "Value";
        private const string VersionAttribLiteral = "Version";
        private const string ItemLiteral = "Item";

        private static readonly XNamespace NS = "http://schemas.microsoft.com/appx/developer/templatestudio";

        public static ProjectMetadata GetProjectMetadata(string projectPath)
        {
            var projectMetadata = new ProjectMetadata();

            try
            {
                if (!string.IsNullOrEmpty(projectPath))
                {
                    var metadataFileNames = new List<string>() { "Package.appxmanifest", "TemplateStudio.xml" };
                    var metadataFile = metadataFileNames.FirstOrDefault(fileName => File.Exists(Path.Combine(projectPath, fileName)));
                    if (!string.IsNullOrEmpty(metadataFile))
                    {
                        var manifest = XElement.Load(Path.Combine(projectPath, metadataFile));
                        XNamespace ns = "http://schemas.microsoft.com/appx/developer/templatestudio";

                        var metadata = manifest.Descendants().FirstOrDefault(e => e.Name.LocalName == MetadataLiteral && e.Name.Namespace == ns);

                        projectMetadata.ProjectType = metadata?.Descendants().FirstOrDefault(m => m.Attribute(NameAttribLiteral)?.Value == ProjectTypeLiteral)?.Attribute(ValueAttribLiteral)?.Value;
                        projectMetadata.Framework = metadata?.Descendants().FirstOrDefault(m => m.Attribute(NameAttribLiteral)?.Value == FrameworkLiteral)?.Attribute(ValueAttribLiteral)?.Value;
                        projectMetadata.Platform = metadata?.Descendants().FirstOrDefault(m => m.Attribute(NameAttribLiteral)?.Value == PlatformLiteral)?.Attribute(ValueAttribLiteral)?.Value;
                        projectMetadata.AppModel = metadata?.Descendants().FirstOrDefault(m => m.Attribute(NameAttribLiteral)?.Value == AppModelLiteral)?.Attribute(ValueAttribLiteral)?.Value;
                        projectMetadata.WizardVersion = metadata?.Descendants().FirstOrDefault(m => m.Attribute(NameAttribLiteral)?.Value == WizardVersionLiteral)?.Attribute(VersionAttribLiteral)?.Value;
                    }
                }
            }
            catch (Exception ex)
            {
                AppHealth.Current.Warning.TrackAsync("Exception reading projectType and framework from Package.appxmanifest", ex).FireAndForget();
            }

            return projectMetadata;
        }

        public static void SaveProjectMetadata(ProjectMetadata data, string projectPath)
        {
            try
            {
                var projectFiles = Directory.GetParent(projectPath).GetFiles("*.*", SearchOption.AllDirectories);
                var metadataFileNames = new List<string>() { "Package.appxmanifest", "TemplateStudio.xml" };
                var metadataFiles = projectFiles.Where(f => metadataFileNames.Any(mf => mf == f.Name));
                foreach (var file in metadataFiles)
                {
                    var manifest = XElement.Load(file.FullName);
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
                        .TryAddMetadaElement(PlatformLiteral, data.Platform)
                        .TryAddMetadaElement(AppModelLiteral, data.AppModel);

                    manifest.Save(file.FullName);
                }
            }
            catch (Exception ex)
            {
                AppHealth.Current.Warning.TrackAsync("Exception saving inferred projectType and framework to metadata file", ex).FireAndForget();
                throw;
            }
        }

        private static XElement TryAddMetadaElement(this XElement metadata, string name, string value)
        {
            if (metadata?.Descendants().FirstOrDefault(m => m.Attribute(NameAttribLiteral)?.Value == name) == null && !string.IsNullOrEmpty(value))
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
        public string ProjectType { get; set; }

        public string Framework { get; set; }

        public string Platform { get; set; }

        public string AppModel { get; set; }

        public string WizardVersion { get; set; }
    }
}
