// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using Microsoft.Templates.Core.Composition;
using Newtonsoft.Json;

namespace TemplateValidator
{
    public static class TemplateFolderVerifier
    {
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
                var allDependencies = new Dictionary<string, string>(); // filepath, dependency
                var allFileHashes = new Dictionary<string, string>();   // filehash, filepath
                var allCompFilters = new Dictionary<string, string>();  // filepath, filter

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
                         && !file.FullName.Contains(".template.config"))
                        {
                            // Use of FileInfo and Path to handle comparison of relative and exact paths
                            if (template.PrimaryOutputs.All(p => file.FullName != new FileInfo(Path.Combine(templateRoot, p.Path)).FullName))
                            {
                                results.Add($"'{file.FullName}' is not used in the template.");
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
                    }
                }
            }

            var success = results.Count == 0;

            if (success)
            {
                results.Add("All looks good.");
            }

            return new VerifierResult(success, results);
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
