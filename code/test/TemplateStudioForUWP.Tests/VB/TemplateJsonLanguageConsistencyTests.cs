// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.Templates.Core.Composition;
using Newtonsoft.Json;
using TemplateValidator;
using Xunit;

namespace TemplateStudioForUwp.Tests
{
    [Trait("Group", "ManualOnlyUWP")]
    public class TemplateJsonLanguageConsistencyTests
    {
        [Fact]
        public void EnsureVisualBasicTemplatesHaveEquivalentPrimaryOutputsAndFilters()
        {
            var errors = new List<string>();
            var allTemplates = TemplateJsonValidationTests.GetAllRelativeTemplateJsonFiles();

            try
            {
                foreach (var template in allTemplates)
                {
                    var filePath = template[0]?.ToString() ?? string.Empty;

                    if (!string.IsNullOrEmpty(filePath) && filePath.Contains("._VB"))
                    {
                        var vbFileContents = File.ReadAllText(filePath);
                        var vbTemplate = JsonConvert.DeserializeObject<ValidationTemplateInfo>(vbFileContents);

                        if (vbTemplate == null)
                        {
                            errors.Add($"Invalid VB Template found: '{filePath}'");
                            throw new FileLoadException();
                        }

                        var csFileContents = File.ReadAllText(filePath.Replace("._VB", string.Empty));
                        var csTemplate = JsonConvert.DeserializeObject<ValidationTemplateInfo>(csFileContents);

                        if (csTemplate == null)
                        {
                            errors.Add($"Invalid CS Template found: '{filePath.Replace("._VB", string.Empty)}'");
                            throw new FileLoadException();
                        }

                        if (vbTemplate?.PrimaryOutputs != null)
                        {
                            if (vbTemplate.PrimaryOutputs.Count != csTemplate.PrimaryOutputs.Count)
                            {
                                errors.Add($"{template[0]} should have {csTemplate.PrimaryOutputs.Count} primary outputs.");
                            }
                        }

                        if (csTemplate.TagsCollection.ContainsKey("wts.compositionFilter"))
                        {
                            var vbFilter = vbTemplate?.TagsCollection["wts.compositionFilter"];
                            var csFilter = csTemplate.TagsCollection["wts.compositionFilter"];

                            var vbquery = CompositionQuery.Parse(vbFilter);
                            var csquery = CompositionQuery.Parse(csFilter);
                            if (vbquery.Items.Count != csquery.Items.Count)
                            {
                                errors.Add($"{template[0]}: {vbFilter} should have contained {csquery.Items.Count} items.");
                            }
                            else
                            {
                                foreach (var vbItem in vbquery.Items)
                                {
                                    var csItem = csquery.Items.First(n => n.Field == vbItem.Field);

                                    if (csItem.Value.Split('|').Length != vbItem.Value.Split('|').Length)
                                    {
                                        if (!csItem.Value.Contains("Prism"))
                                        {
                                            errors.Add($"{template[0]}: check {vbItem.Field} in composition query.");
                                        }
                                    }
                                }
                            }
                        }

                        var csCompOrder = csTemplate.TagsCollection.ContainsKey("wts.compositionOrder") ? csTemplate.TagsCollection["wts.compositionOrder"] : null;
                        var vbCompOrder = vbTemplate?.TagsCollection.ContainsKey("wts.compositionOrder") ?? false ? vbTemplate.TagsCollection["wts.compositionOrder"] : null;

                        if (csCompOrder != vbCompOrder)
                        {
                            errors.Add($"CompostionOrder issue in {template[0]}");
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
