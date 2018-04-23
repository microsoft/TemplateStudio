// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.IO;
using System.Linq;
using LibGit2Sharp;
using Localization.Extensions;
using Localization.Options;

namespace Localization
{
    public class ValidateLocalizableExtractor
    {
        private RoutesManager _routesManager;

        public ValidateLocalizableExtractor(string originalExtractPath, string actualExtractPath)
        {
            _routesManager = new RoutesManager(originalExtractPath, actualExtractPath);
        }

        public bool HasVsixChanges()
        {
            var originalVsix = _routesManager.GetFileFromSource(Routes.VsixRootDirPath, Routes.VsixManifestFile).FullName;
            var actualVsix = _routesManager.GetFileFromDestination(Routes.VsixRootDirPath, Routes.VsixManifestFile).FullName;

            var originalManifest = XmlUtility.LoadXmlFile(originalVsix);
            var originalName = originalManifest.GetNode("DisplayName").InnerText.Trim();
            var originalDescription = originalManifest.GetNode("Description").InnerText.Trim();

            var actualManifest = XmlUtility.LoadXmlFile(originalVsix);
            var actualName = actualManifest.GetNode("DisplayName").InnerText.Trim();
            var actualDescription = actualManifest.GetNode("Description").InnerText.Trim();

            return !(originalName == actualName && originalDescription == actualDescription);
        }

        public bool HasVsTemplatesChanges(string language)
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

            return !(originalName == actualName && originalDescription == actualDescription);
        }

        public bool HasRelayCommandPackageChanges()
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

            return !originalResources.SequenceEqual(actualResources);
        }

        internal bool HasChanges(string projectTemplateFileNameValidateCS)
        {
            return true;
        }
    }
}
