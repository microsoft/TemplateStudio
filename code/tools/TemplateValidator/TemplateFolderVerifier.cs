// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Xml;
using Microsoft.Templates.Core;
using Microsoft.Templates.Core.Composition;
using Newtonsoft.Json;

namespace TemplateValidator
{
    public static class TemplateFolderVerifier
    {
        private static string[] excludedPrimaryOutputFiles = new string[]
        {
            @"\WinUI\Pages\Blank.Cpp\wts.ItemNamePage.idl",
        };

        public static VerifierResult VerifyTemplateFolders(bool showWarnings, params string[] templateFolders)
        {
            var results = new List<string>();

            if (templateFolders == null)
            {
                results.Add("Paths to template folders not provided.");
            }
            else
            {
                results.AddRange(from templateFolder
                                 in templateFolders
                                 where !Directory.Exists(templateFolder)
                                 select $"Folder '{templateFolder}' does not exist.");
            }

            // Don't bother with actual tests if not all folders are valid
            if (!results.Any())
            {
                var allTemplateFilePaths = new List<string>();

                foreach (var rootFolder in templateFolders)
                {
                    allTemplateFilePaths.AddRange(new DirectoryInfo(rootFolder).GetFiles("template.json", SearchOption.AllDirectories)
                                                                               .Select(file => file.FullName));
                }

                var allIdentities = new Dictionary<string, string>();   // identity, filepath
                var allGroupIdentities = new List<string>();   // identity, identity
                var allDependencies = new Dictionary<string, string>(); // filepath, dependency
                var allRequirements = new Dictionary<string, string>(); // filepath, requirement
                var allExclusions = new Dictionary<string, string>(); // filepath, exclusion
                var allFileHashes = new Dictionary<string, string>();   // filehash, filepath
                var allCompFilters = new Dictionary<string, string>();  // filepath, filter
                var allPageIdentities = new List<string>();
                var allFeatureIdentities = new List<string>();
                var allServiceIdentities = new List<string>();
                var allTestingIdentities = new List<string>();

                foreach (var templateFilePath in allTemplateFilePaths)
                {
                    var fileContents = File.ReadAllText(templateFilePath);

                    var template = JsonConvert.DeserializeObject<ValidationTemplateInfo>(fileContents);

                    if (template.Identity != null)
                    {
                        if (allIdentities.ContainsKey(template.Identity))
                        {
                            results.Add($"Duplicate Identity detected in: '{templateFilePath}' & '{allIdentities[template.Identity]}'");
                        }
                        else
                        {
                            allIdentities.Add(template.Identity, templateFilePath);

                            if (!allGroupIdentities.Contains(template.GroupIdentity))
                            {
                                allGroupIdentities.Add(template.GroupIdentity);
                            }

                            if (template.GetTemplateType() == TemplateType.Page)
                            {
                                allPageIdentities.Add(template.Identity);
                            }
                            else if (template.GetTemplateType() == TemplateType.Feature)
                            {
                                allFeatureIdentities.Add(template.Identity);
                            }
                            else if (template.GetTemplateType() == TemplateType.Service)
                            {
                                allServiceIdentities.Add(template.Identity);
                            }
                            else if (template.GetTemplateType() == TemplateType.Testing)
                            {
                                allTestingIdentities.Add(template.Identity);
                            }
                        }

                        // Check that localized files have the same identity
                        foreach (var localizedFile in new DirectoryInfo(Path.GetDirectoryName(templateFilePath)).EnumerateFiles("*.template.json"))
                        {
                            var localizedContents = File.ReadAllText(localizedFile.FullName);
                            var localizedTemplate = JsonConvert.DeserializeObject<ValidationTemplateInfo>(localizedContents);

                            if (template.Identity != localizedTemplate.Identity)
                            {
                                results.Add($"'{localizedFile.FullName}' does not have the correct identity.");
                            }

                            if (template.Name != localizedTemplate.Name)
                            {
                                results.Add($"'{localizedFile.FullName}' does not have the correct name.");
                            }
                        }
                    }
                    else
                    {
                        if (allIdentities.ContainsKey(template.Name))
                        {
                            results.Add($"Duplicate Identity detected in: '{templateFilePath}' & '{allIdentities[template.Name]}'");
                        }
                        else
                        {
                            allIdentities.Add(template.Name, templateFilePath);
                        }
                    }

                    // Get list of dependencies while the file is open. These are all checked later
                    if (template.TemplateTags.ContainsKey("wts.dependencies"))
                    {
                        allDependencies.Add(templateFilePath, template.TemplateTags["wts.dependencies"]);
                    }

                    // Get list of requirements while the file is open. These are all checked later
                    if (template.TemplateTags.ContainsKey("wts.requirements"))
                    {
                        allRequirements.Add(templateFilePath, template.TemplateTags["wts.requirements"]);
                    }

                    // Get list of exclusions while the file is open. These are all checked later
                    if (template.TemplateTags.ContainsKey("wts.exclusions"))
                    {
                        allExclusions.Add(templateFilePath, template.TemplateTags["wts.exclusions"]);
                    }

                    // Get list of filters while the file is open. These are all checked later
                    if (template.TemplateTags.ContainsKey("wts.compositionFilter"))
                    {
                        allCompFilters.Add(templateFilePath, template.TemplateTags["wts.compositionFilter"]);
                    }

                    var templateRoot = templateFilePath.Replace("\\.template.config\\template.json", string.Empty);

                    foreach (var file in new DirectoryInfo(templateRoot).GetFiles("*.*", SearchOption.AllDirectories))
                    {
                        // Filter out files the following tests cannot handle
                        if (!file.Name.Contains("_postaction")
                         && !file.Name.Contains("_gpostaction")
                         && !file.Name.Contains("_searchreplace")
                         && !file.FullName.Contains("\\Projects\\Default")
                         && !file.FullName.Contains("\\Test\\")
                         && !file.FullName.Contains(".template.config"))
                        {
                            // Projects will include files that aren't directly referenced
                            if (template.GetTemplateOutputType() == TemplateOutputType.Item)
                            {
                                // Use of FileInfo and Path to handle comparison of relative and exact paths
                                if (template.PrimaryOutputs.All(p => file.FullName != new FileInfo(Path.Combine(templateRoot, p.Path)).FullName && !excludedPrimaryOutputFiles.Any(f => file.FullName.EndsWith(f))))
                                {
                                    results.Add($"'{file.FullName}' is not used in the template.");
                                }
                            }

                            // Duplicate file checking can be avoided as some duplicate files exist in the official templates at the time of writing.
                            // It is done by default to encourage anyone creating new templates to follow this guidance.
                            if (showWarnings)
                            {
                                // Ignore xaml files as we know these are duplicated across VB & C# versions of the same template
                                if (file.Extension != ".xaml")
                                {
                                    var hash = GetFileHash(file.FullName);

                                    // if hash is already in the dictionary then write to results as a duplicate file
                                    // if not add to the dictionary
                                    if (allFileHashes.ContainsKey(hash))
                                    {
                                        results.Add($"WARNING: '{file.FullName}' and '{allFileHashes[hash]}' have identical contents and could be combined into a single template.");
                                    }
                                    else
                                    {
                                        allFileHashes.Add(hash, file.FullName);
                                    }
                                }
                            }
                        }
                    }
                }

                foreach (var dependencies in allDependencies)
                {
                    foreach (var dependency in dependencies.Value.Split('|'))
                    {
                        if (!allIdentities.ContainsKey(dependency))
                        {
                            results.Add($"'{dependencies.Key}' contains dependency '{dependency}' that does not exist.");
                        }
                    }
                }

                foreach (var requirements in allRequirements)
                {
                    foreach (var requirement in requirements.Value.Split('|'))
                    {
                        if (!allIdentities.ContainsKey(requirement))
                        {
                            results.Add($"'{requirements.Key}' contains requirement '{requirements}' that does not exist.");
                        }
                    }
                }

                foreach (var exclusions in allExclusions)
                {
                    foreach (var exclusion in exclusions.Value.Split('|'))
                    {
                        if (!allGroupIdentities.Contains(exclusion))
                        {
                            results.Add($"'{exclusions.Key}' contains exclusion '{exclusion}' that does not exist.");
                        }
                    }
                }

                foreach (var compFilter in allCompFilters)
                {
                    var query = CompositionQuery.Parse(compFilter.Value);

                    foreach (var queryItem in query.Items)
                    {
                        if (queryItem.Field == "identity")
                        {
                            foreach (var templateIdentity in queryItem.Value.Split('|'))
                            {
                                if (!allIdentities.Keys.Contains(templateIdentity))
                                {
                                    results.Add($"'{compFilter.Key}' contains composition filter identity '{templateIdentity}' that does not exist.");
                                }
                            }
                        }
                        else if (queryItem.Field == "page")
                        {
                            foreach (var templateIdentity in queryItem.Value.Split('|'))
                            {
                                if (!allPageIdentities.Contains(templateIdentity))
                                {
                                    results.Add($"'{compFilter.Key}' contains composition filter page identity '{templateIdentity}' that does not exist.");
                                }
                            }
                        }
                        else if (queryItem.Field == "feature")
                        {
                            foreach (var templateIdentity in queryItem.Value.Split('|'))
                            {
                                if (!allFeatureIdentities.Contains(templateIdentity))
                                {
                                    results.Add($"'{compFilter.Key}' contains composition filter feature identity '{templateIdentity}' that does not exist.");
                                }
                            }
                        }
                        else if (queryItem.Field == "service")
                        {
                            foreach (var templateIdentity in queryItem.Value.Split('|'))
                            {
                                if (!allServiceIdentities.Contains(templateIdentity))
                                {
                                    results.Add($"'{compFilter.Key}' contains composition filter service identity '{templateIdentity}' that does not exist.");
                                }
                            }
                        }
                        else if (queryItem.Field == "testing")
                        {
                            foreach (var templateIdentity in queryItem.Value.Split('|'))
                            {
                                if (!allTestingIdentities.Contains(templateIdentity))
                                {
                                    results.Add($"'{compFilter.Key}' contains composition filter testing identity '{templateIdentity}' that does not exist.");
                                }
                            }
                        }
                    }
                }

                if (!results.Any())
                {
                    CheckIconXamlFiles(templateFolders, results);
                }
            }

            var success = results.Count == 0;

            if (success)
            {
                results.Add("All looks good.");
            }

            return new VerifierResult(success, results);
        }

        private static void CheckIconXamlFiles(string[] templateFolders, List<string> results)
        {
            var iconFiles = new List<string>();

            foreach (var rootFolder in templateFolders)
            {
                iconFiles.AddRange(new DirectoryInfo(rootFolder)
                    .GetFiles("icon.xaml", SearchOption.AllDirectories)
                    .Select(file => file.FullName));
            }

            foreach (var iconFile in iconFiles)
            {
                var doc = new XmlDocument();
                doc.Load(iconFile);

                var ellipses = doc.GetElementsByTagName("Ellipse");

                foreach (XmlNode ellipse in ellipses)
                {
                    if (ellipse.Attributes != null && ellipse.Attributes.Count > 1
                        && ellipse.Attributes["Fill"] != null && ellipse.Attributes["Stroke"] != null)
                    {
                        results.Add(
                            $"'{iconFile}' contains an Ellipse with both Fill and Stroke specified when use of only one is supported.");
                        break;
                    }
                }

                var rectangles = doc.GetElementsByTagName("Rectangle");

                foreach (XmlNode rectangle in rectangles)
                {
                    if (rectangle.Attributes != null && rectangle.Attributes.Count > 1
                        && rectangle.Attributes["Fill"] != null && rectangle.Attributes["Stroke"] != null)
                    {
                        results.Add(
                            $"'{iconFile}' contains a Rectangle with both Fill and Stroke specified when use of only one is supported.");
                        break;
                    }
                }
            }
        }

        private static string GetFileHash(string filePath)
        {
            using (var stream = File.OpenRead(filePath))
            {
                var sha = new SHA256Managed();
                byte[] hash = sha.ComputeHash(stream);
                return BitConverter.ToString(hash).Replace("-", string.Empty);
            }
        }
    }
}
