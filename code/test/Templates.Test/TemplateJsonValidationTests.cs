// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Microsoft.Templates.Core.Composition;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using TemplateValidator;
using Xunit;

namespace Microsoft.Templates.Test
{
    [Trait("Type", "TemplateValidation")]
    [Trait("ExecutionSet", "Minimum")]
    [Trait("ExecutionSet", "TemplateValidation")]
    [Trait("ExecutionSet", "_CIBuild")]
    [Trait("ExecutionSet", "_Full")]
    public class TemplateJsonValidationTests
    {
        public static IEnumerable<object[]> GetAllTemplateJsonFiles()
        {
            // This is the relative path from where the test assembly will run from
            const string templatesRoot = "../../../../../Templates";

            // The following excludes the catalog and project folders, but they only contain a single template file each
            var foldersOfInterest = new[] { "Uwp/_comp", "Uwp/Features", "Uwp/Pages", "Uwp/Services", "Uwp/Testing" };

            foreach (var folder in foldersOfInterest)
            {
                foreach (var file in new DirectoryInfo(Path.Combine(templatesRoot, folder)).GetFiles("template.json", SearchOption.AllDirectories))
                {
                    yield return new object[] { file.FullName };
                }
            }
        }

        private static IEnumerable<string> GetFiles(string directory)
        {
            foreach (var dir in Directory.GetDirectories(directory))
            {
                foreach (var file in Directory.GetFiles(dir))
                {
                    yield return file;
                }

                foreach (var file in GetFiles(dir))
                {
                    yield return file;
                }
            }
        }

        [Theory]
        [MemberData(nameof(GetAllTemplateJsonFiles))]
        public async Task VerifyAllTemplateFilesAsync(string filePath)
        {
            var result = await TemplateJsonVerifier.VerifyTemplatePathAsync(filePath);

            Assert.True(result.Success, $"{filePath}: " + string.Join(Environment.NewLine, result.Messages));
        }
    }

    [Trait("Type", "TemplateValidation")]
    [Trait("ExecutionSet", "ManualOnly")]
    public class TemplateJsonLanguageConsistencyTests
    {
        [Fact]
        public void EnsureVisualBasicTemplatesHaveEquivalentPrimaryOutputsAndFilters()
        {
            var errors = new List<string>();
            var allTemplates = TemplateJsonValidationTests.GetAllTemplateJsonFiles();

            try
            {
                foreach (var template in allTemplates)
                {
                    if (template[0].ToString().Contains("._VB"))
                    {
                        var vbFileContents = File.ReadAllText(template[0].ToString());
                        var vbTemplate = JsonConvert.DeserializeObject<ValidationTemplateInfo>(vbFileContents);

                        var csFileContents = File.ReadAllText(template[0].ToString().Replace("._VB", string.Empty));
                        var csTemplate = JsonConvert.DeserializeObject<ValidationTemplateInfo>(csFileContents);

                        if (vbTemplate.PrimaryOutputs != null)
                        {
                            if (vbTemplate.PrimaryOutputs.Count != csTemplate.PrimaryOutputs.Count)
                            {
                                errors.Add($"{template[0].ToString()} should have {csTemplate.PrimaryOutputs.Count} primary outputs.");
                            }
                        }

                        if (csTemplate.TemplateTags.ContainsKey("wts.compositionFilter"))
                        {
                            var vbFilter = vbTemplate.TemplateTags["wts.compositionFilter"];
                            var csFilter = csTemplate.TemplateTags["wts.compositionFilter"];

                            var vbquery = CompositionQuery.Parse(vbFilter);
                            var csquery = CompositionQuery.Parse(csFilter);
                            if (vbquery.Items.Count != csquery.Items.Count)
                            {
                                errors.Add($"{template[0].ToString()}: {vbFilter} should have contained {csquery.Items.Count} items.");
                            }
                            else
                            {
                                foreach (var vbItem in vbquery.Items)
                                {
                                    var csItem = csquery.Items.First(n => n.Field == vbItem.Field);

                                    if (csItem.Value.Split('|').Count() != vbItem.Value.Split('|').Count())
                                    {
                                        if (!csItem.Value.Contains("Prism") && !csItem.Value.Contains("Caliburn"))
                                        {
                                            errors.Add($"{template[0].ToString()}: check {vbItem.Field} in composition query.");
                                        }
                                    }
                                }
                            }
                        }

                        var csCompOrder = csTemplate.TemplateTags.ContainsKey("wts.compositionOrder") ? csTemplate.TemplateTags["wts.compositionOrder"] : null;
                        var vbCompOrder = vbTemplate.TemplateTags.ContainsKey("wts.compositionOrder") ? vbTemplate.TemplateTags["wts.compositionOrder"] : null;

                        if (csCompOrder != vbCompOrder)
                        {
                            errors.Add($"CompostionOrder issue in {template[0].ToString()}");
                        }
                    }
                }

            }
            catch (Exception exc)
            {
                System.Diagnostics.Debug.WriteLine(exc);
            }

            Assert.True(!errors.Any(), string.Join(Environment.NewLine, errors));
        }
    }
}
