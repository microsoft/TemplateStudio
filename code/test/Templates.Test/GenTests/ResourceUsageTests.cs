// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Xml;
using Microsoft.Templates.Core;
using Microsoft.Templates.UI;
using Xunit;

namespace Microsoft.Templates.Test
{
    [Collection("GenerationCollection")]
    public class ResourceUsageTests : BaseGenAndBuildTests
    {
        public ResourceUsageTests(GenerationFixture fixture)
        {
            _fixture = fixture;
            _fixture.InitializeFixture(this);
        }

        [Theory]
        [MemberData("GetProjectTemplatesForGenerationAsync")]
        [Trait("ExecutionSet", "ManualOnly")]
        [Trait("Type", "GenerationResourceUsage")]
        public async Task EnsureReswResourceInGeneratedProjectsAreUsedAsync(string projectType, string framework, string platform, string language)
        {
            if (language == ProgrammingLanguages.VisualBasic || platform != Platforms.Uwp)
            {
                // Only test C# projects as other tests for language comparison should identify differences between languages
                // Easiser than filtering out in the member data generation
                return;
            }

            BaseGenAndBuildFixture.SetCurrentLanguage(language);
            BaseGenAndBuildFixture.SetCurrentPlatform(platform);

            // To initialize the list correctly
            var projectTemplate = _fixture.Templates().FirstOrDefault(t => t.GetTemplateType() == TemplateType.Project && t.GetProjectTypeList().Contains(projectType) && t.GetFrameworkList().Contains(framework));

            ProjectName = $"{projectType}{framework}Resx";

            DestinationPath = Path.Combine(_fixture.TestProjectsPath, ProjectName, ProjectName);
            OutputPath = DestinationPath;

            var userSelection = _fixture.SetupProject(projectType, framework, platform, language);

            var itemTemplates = _fixture.Templates().Where(t => (t.GetTemplateType() == TemplateType.Page || t.GetTemplateType() == TemplateType.Feature)
                                                                && t.GetFrameworkList().Contains(framework)
                                                                && t.GetPlatform() == platform
                                                                && !t.GetIsHidden());

            _fixture.AddItems(userSelection, itemTemplates, BaseGenAndBuildFixture.GetDefaultName);

            await NewProjectGenController.Instance.UnsafeGenerateProjectAsync(userSelection);

            var resultPath = Path.Combine(_fixture.TestProjectsPath, ProjectName);

            var reswFilePath = Path.Combine(resultPath, ProjectName, "strings", "en-us", "Resources.resw");

            var xdoc = new XmlDocument();
            xdoc.Load(reswFilePath);

            var searchTerms = new List<string>();

            foreach (XmlElement element in xdoc.GetElementsByTagName("data"))
            {
                var interestedPartOfName = element.Attributes["name"].Value;

                if (interestedPartOfName.Contains("."))
                {
                    interestedPartOfName = interestedPartOfName.Substring(0, interestedPartOfName.IndexOf(".", StringComparison.Ordinal));
                }

                if (!searchTerms.Contains(interestedPartOfName))
                {
                    searchTerms.Add(interestedPartOfName);
                }
            }

            foreach (var fileExt in new[] { "*.xaml", "*.appxmanifest", "*.cs" })
            {
                foreach (var file in Directory.GetFiles(resultPath, fileExt, SearchOption.AllDirectories))
                {
                    var fileContents = File.ReadAllText(file);

                    for (var i = searchTerms.Count - 1; i >= 0; i--)
                    {
                        var usage = searchTerms[i];

                        switch (fileExt)
                        {
                            case "*.appxmanifest":
                                usage = "ms-resource:" + usage;
                                break;
                            case "*.cs":
                                usage = $"\"{usage}\".GetLocalized()";
                                break;
                            case "*.xaml":
                                usage = $"x:Uid=\"{usage}\"";
                                break;
                        }

                        if (fileContents.Contains(usage))
                        {
                            searchTerms.RemoveAt(i);
                        }
                    }
                }
            }

            Assert.True(searchTerms.Count == 0, $"Unused string resources: {string.Join(", ", searchTerms)}");

            Fs.SafeDeleteDirectory(resultPath);
        }
    }
}
