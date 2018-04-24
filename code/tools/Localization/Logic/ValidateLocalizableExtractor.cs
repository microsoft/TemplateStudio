// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using LibGit2Sharp;
using Localization.Extensions;
using Localization.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Localization
{
    internal class ValidateLocalizableExtractor
    {
        private RoutesManager _routesManager;

        internal ValidateLocalizableExtractor(string originalExtractPath, string actualExtractPath)
        {
            _routesManager = new RoutesManager(originalExtractPath, actualExtractPath);
        }

        internal bool HasVsixChanges()
        {
            var originalVsix = _routesManager.GetFileFromSource(Routes.VsixRootDirPath, Routes.VsixManifestFile).FullName;
            var actualVsix = _routesManager.GetFileFromDestination(Routes.VsixRootDirPath, Routes.VsixManifestFile).FullName;

            var originalManifest = XmlUtility.LoadXmlFile(originalVsix);
            var originalName = originalManifest.GetNode("DisplayName").InnerText.Trim();
            var originalDescription = originalManifest.GetNode("Description").InnerText.Trim();

            var actualManifest = XmlUtility.LoadXmlFile(originalVsix);
            var actualName = actualManifest.GetNode("DisplayName").InnerText.Trim();
            var actualDescription = actualManifest.GetNode("Description").InnerText.Trim();

            return originalName != actualName || originalDescription != actualDescription;
        }

        internal bool HasVsTemplatesChanges(string language)
        {
            var relativePath = language == "VB" ? Routes.ProjectTemplatePathVB : Routes.ProjectTemplatePathCS;
            var fileName = language == "VB" ? Routes.ProjectTemplateFileVB : Routes.ProjectTemplateFileCS;

            var originalVsTemplate = _routesManager.GetFileFromSource(relativePath, fileName).FullName;
            var actualVsTemplate = _routesManager.GetFileFromDestination(relativePath, fileName).FullName;

            var originalManifest = XmlUtility.LoadXmlFile(originalVsTemplate);
            var originalName = originalManifest.GetNode("Name").InnerText.Trim();
            var originalDescription = originalManifest.GetNode("Description").InnerText.Trim();

            var actualManifest = XmlUtility.LoadXmlFile(actualVsTemplate);
            var actualName = actualManifest.GetNode("Name").InnerText.Trim();
            var actualDescription = actualManifest.GetNode("Description").InnerText.Trim();

            return originalName != actualName || originalDescription != actualDescription;
        }

        internal bool HasRelayCommandPackageChanges()
        {
            var originalRelayCommandFile = _routesManager.GetFileFromSource(Routes.CommandTemplateRootDirPath, Routes.RelayCommandFile).FullName;
            var actualRelayCommandFile = _routesManager.GetFileFromDestination(Routes.CommandTemplateRootDirPath, Routes.RelayCommandFile).FullName;

            var originalResources = XmlUtility
                .LoadXmlFile(originalRelayCommandFile)
                .GetNodes("Strings")
                .GetInnerText();

            var actualResources = XmlUtility
                .LoadXmlFile(actualRelayCommandFile)
                .GetNodes("Strings")
                .GetInnerText();

            return originalResources.Count() != actualResources.Count()
                || !originalResources.SequenceEqual(actualResources);
        }

        internal bool HasVsPackageResxChanges()
        {
            var originalVsPackageFile = _routesManager.GetFileFromSource(Routes.CommandTemplateRootDirPath, Routes.VspackageFile).FullName;
            var actualVsPackageFile = _routesManager.GetFileFromDestination(Routes.CommandTemplateRootDirPath, Routes.VspackageFile).FullName;

            var (originalName, originalDescription) = GetVsPackageLocalizationValues(originalVsPackageFile);
            var (actualName, actualDescription) = GetVsPackageLocalizationValues(actualVsPackageFile);

            return !(originalName == actualName && originalDescription == actualDescription);
        }

        private (string name, string description) GetVsPackageLocalizationValues(string vspackagePath)
        {
            var xml = XmlUtility.LoadXmlFile(vspackagePath);

            var name = xml.GetNodeByAttribute("//data[@name='110']").InnerText.Trim();
            var description = xml.GetNodeByAttribute("//data[@name='112']").InnerText.Trim();

            return (name, description);
        }

        internal bool HasTemplateJsonChanges(string jsonPath)
        {
            var originalJson = _routesManager.GetFileFromSource(jsonPath).FullName;
            var actualJson = _routesManager.GetFileFromDestination(jsonPath).FullName;

            var originalJsonContent = JsonConvert.DeserializeObject<JObject>(File.ReadAllText(originalJson));
            var actualJsonContent = JsonConvert.DeserializeObject<JObject>(File.ReadAllText(actualJson));

            var originalDescription = originalJsonContent.GetValue("description", StringComparison.Ordinal).Value<string>();
            var actualDescription = actualJsonContent.GetValue("description", StringComparison.Ordinal).Value<string>();

            return originalDescription != actualDescription;
        }

        internal bool HasTemplateMdChanges(string mdPath)
        {
            var originalMd = _routesManager.GetFileFromSource(mdPath).FullName;
            var actualMd = _routesManager.GetFileFromDestination(mdPath).FullName;

            var originalMdContent = File.ReadAllText(originalMd);
            var actualMdContent = File.ReadAllText(actualMd);

            return originalMdContent != actualMdContent;
        }

        internal bool HasCatalogJsonChanges(string jsonPath)
        {
            var originalJson = _routesManager.GetFileFromSource(jsonPath).FullName;
            var actualJson = _routesManager.GetFileFromDestination(jsonPath).FullName;

            var originalContent = JsonConvert
                .DeserializeObject<IEnumerable<CatalogLocalizationValue>>(
                File.ReadAllText(originalJson))
                .OrderBy(v => v.Name);

            var actualContent = JsonConvert
                .DeserializeObject<IEnumerable<CatalogLocalizationValue>>(
                File.ReadAllText(actualJson))
                .OrderBy(v => v.Name);

            return originalContent.Count() != actualContent.Count()
                || !originalContent.SequenceEqual(actualContent);
        }

        internal bool HasResourceChanges(string resPath)
        {
            var originalResxPath = _routesManager.GetFileFromSource(resPath).FullName;
            var actualResxPath = _routesManager.GetFileFromDestination(resPath).FullName;

            var originalResx = ResourcesExtensions.GetResourcesByFile(originalResxPath);
            var actualResx = ResourcesExtensions.GetResourcesByFile(actualResxPath);

            // compare num items
            if (originalResx.Keys.Count != actualResx.Keys.Count)
            {
                return true;
            }

            // compare keys names
            if (actualResx.Keys.Any(k => !originalResx.Keys.Contains(k)))
            {
                return true;
            }

            // compare values
            if (actualResx.Any(i => i.Value != originalResx[i.Key]))
            {
                return true;
            }

            return false;
        }
    }
}
