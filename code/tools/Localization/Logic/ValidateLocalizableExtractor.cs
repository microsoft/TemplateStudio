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
            var path = Path.Combine(Routes.VsixRootDirPath, Routes.VsixManifestFile);
            var nodes = new[] { "DisplayName", "Description" };
            var areEquals = CompareNodesToXmlFilesAreEquals(path, nodes);

            return !areEquals;
        }

        internal bool HasVsTemplatesChanges(string path)
        {
            var nodes = new[] { "Name", "Description" };
            var areEquals = CompareNodesToXmlFilesAreEquals(path, nodes);

            return !areEquals;
        }

        internal bool HasRelayCommandPackageChanges()
        {
            var relayCommandFilePath = Path.Combine(Routes.CommandTemplateRootDirPath, Routes.RelayCommandFile);
            if (!_routesManager.ExistInSourceAndDestination(relayCommandFilePath))
            {
                return true;
            }

            var originalRelayCommandFile = _routesManager.GetFileFromSource(relayCommandFilePath);
            var actualRelayCommandFile = _routesManager.GetFileFromDestination(relayCommandFilePath);

            var originalResources = XmlUtility
                .LoadXmlFile(originalRelayCommandFile.FullName)
                .GetNodes("Strings")
                .GetInnerText();

            var actualResources = XmlUtility
                .LoadXmlFile(actualRelayCommandFile.FullName)
                .GetNodes("Strings")
                .GetInnerText();

            return originalResources.Count() != actualResources.Count()
                || !originalResources.SequenceEqual(actualResources);
        }

        internal bool HasVsPackageResxChanges()
        {
            var path = Path.Combine(Routes.CommandTemplateRootDirPath, Routes.VspackageFile);
            var attributes = new[] { "//data[@name='110']", "//data[@name='112']" };
            var areEquals = CompareNodesToXmlFilesAreEquals(path, attributes, mode: "attribute");

            return !areEquals;
        }

        internal bool HasTemplateJsonChanges(string path)
        {
            var nodes = new[] { "description" };
            var areEquals = CompareJsonPropertiesAreEquals(path, nodes);

            return !areEquals;
        }

        internal bool HasTemplateMdChanges(string mdPath)
        {
            if (!_routesManager.ExistInSourceAndDestination(mdPath))
            {
                return true;
            }

            var originalMd = _routesManager.GetFileFromSource(mdPath);
            var originalMdContent = File.ReadAllText(originalMd.FullName);

            var actualMd = _routesManager.GetFileFromDestination(mdPath);
            var actualMdContent = File.ReadAllText(actualMd.FullName);

            return originalMdContent != actualMdContent;
        }

        internal bool HasCatalogJsonChanges(string jsonPath)
        {
            if (!_routesManager.ExistInSourceAndDestination(jsonPath))
            {
                return true;
            }

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

        // TODO: this method should not be in this class, since it is not a validating method,
        // but the _routesManager contains the necessary paths
        internal Dictionary<string, ResxItem> GetResourcesWithChanges(string resPath)
        {
            var originalResxPath = _routesManager.GetFileFromSource(resPath);
            var originalValues = ResourcesExtensions.GetResourcesByFile(originalResxPath.FullName);

            var actualResxPath = _routesManager.GetFileFromDestination(resPath);
            var newValues = ResourcesExtensions.GetResourcesByFile(actualResxPath.FullName);

            return newValues.GetChangesFrom(originalValues);
        }

        private bool CompareNodesToXmlFilesAreEquals(string path, IEnumerable<string> nodes, string mode = "name")
        {
            if (!_routesManager.ExistInSourceAndDestination(path))
            {
                return false;
            }

            var originalFile = _routesManager.GetFileFromSource(path);
            var originalValues = GetNodesByXml(originalFile, nodes, mode);

            var actualFile = _routesManager.GetFileFromDestination(path);
            var actualValues = GetNodesByXml(actualFile, nodes, mode);

            return originalValues.AreEquals(actualValues);
        }

        private Dictionary<string, string> GetNodesByXml(FileInfo file, IEnumerable<string> nodes, string mode = "name")
        {
            var result = new Dictionary<string, string>();
            var xml = XmlUtility.LoadXmlFile(file.FullName);

            foreach (var node in nodes)
            {
                var xmlNode = mode == "attribute" ? xml.GetNodeByAttribute(node) : xml.GetNode(node);
                var value = xmlNode.InnerText.Trim();
                result.Add(node, value);
            }

            return result;
        }

        private bool CompareJsonPropertiesAreEquals(string path, IEnumerable<string> properties)
        {
            if (!_routesManager.ExistInSourceAndDestination(path))
            {
                return false;
            }

            var originalFile = _routesManager.GetFileFromSource(path);
            var originalValues = GetPropertiesByJson(originalFile, properties);

            var actualFile = _routesManager.GetFileFromDestination(path);
            var actualValues = GetPropertiesByJson(actualFile, properties);

            return originalValues.AreEquals(actualValues);
        }

        private Dictionary<string, string> GetPropertiesByJson(FileInfo file, IEnumerable<string> properties)
        {
            var result = new Dictionary<string, string>();
            var json = JsonConvert.DeserializeObject<JObject>(File.ReadAllText(file.FullName));

            foreach (var property in properties)
            {
                var value = json.GetValue(property, StringComparison.Ordinal).Value<string>();
                result.Add(property, value);
            }

            return result;
        }
    }
}
