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
using Microsoft.Templates.Core.Extensions;
using Microsoft.Templates.Core.Gen;
using Microsoft.Templates.Core.Helpers;
using Microsoft.Templates.Fakes;
using Microsoft.Templates.UI;
using Xunit;

namespace Microsoft.Templates.Test
{
    [Collection("GenerationCollection")]
    public class ResourceUsageTests : BaseGenAndBuildTests
    {
        private readonly string _emptyBackendFramework = string.Empty;

        public ResourceUsageTests(GenerationFixture fixture)
            : base(fixture)
        {
        }

        [Theory]
        [MemberData(nameof(BaseGenAndBuildTests.GetCSharpUwpProjectTemplatesForGeneration))]
        [Trait("ExecutionSet", "ManualOnly")]
        [Trait("Type", "GenerationResourceUsage")]
        public async Task EnsureReswResourceInGeneratedProjectsAreUsedAsync(string projectType, string framework, string platform, string language)
        {
            var projectName = $"{projectType}{framework}Resw";

            var resultPath = await GenerateProjectForTestingAsync(projectName, projectType, framework, platform, language);

            var reswFilePath = Path.Combine(resultPath, projectName, "strings", "en-us", "Resources.resw");

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
                        string usage2 = null;

                        switch (fileExt)
                        {
                            case "*.appxmanifest":
                                usage = "ms-resource:" + usage;
                                usage2 = null;
                                break;
                            case "*.cs":
                                usage = $"\"{usage}\".GetLocalized()";
                                usage2 = usage.Replace("\".Get", "/Header\".Get");
                                break;
                            case "*.xaml":
                                usage = $"x:Uid=\"{usage}\"";
                                usage2 = null;
                                break;
                        }

                        if (fileContents.Contains(usage))
                        {
                            searchTerms.RemoveAt(i);
                        }

                        if (usage2 != null && fileContents.Contains(usage2))
                        {
                            searchTerms.RemoveAt(i);
                        }
                    }
                }
            }

            Assert.True(searchTerms.Count == 0, $"Unused string resources: {string.Join(", ", searchTerms)}");

            Fs.SafeDeleteDirectory(resultPath);
        }

        [Theory]
        [MemberData(nameof(BaseGenAndBuildTests.GetCSharpUwpProjectTemplatesForGeneration))]
        [Trait("ExecutionSet", "ManualOnly")]
        [Trait("Type", "GenerationResourceUsage")]
        public async Task EnsureDefinedUidsHaveResourceEntriesAsync(string projectType, string framework, string platform, string language)
        {
            var projectName = $"{projectType}{framework}Uids";

            var resultPath = await GenerateProjectForTestingAsync(projectName, projectType, framework, platform, language);

            var reswFilePath = Path.Combine(resultPath, projectName, "strings", "en-us", "Resources.resw");

            var uidsWithoutResources = new List<string>();

            var xdoc = new XmlDocument();
            xdoc.Load(reswFilePath);

            var resources = new List<string>();

            foreach (XmlElement element in xdoc.GetElementsByTagName("data"))
            {
                var interestedPartOfName = element.Attributes["name"].Value;

                if (interestedPartOfName.Contains("."))
                {
                    interestedPartOfName = interestedPartOfName.Substring(0, interestedPartOfName.IndexOf(".", StringComparison.Ordinal));
                }

                if (!resources.Contains(interestedPartOfName))
                {
                    resources.Add(interestedPartOfName);
                }
            }

            foreach (var file in Directory.GetFiles(resultPath, "*.xaml", SearchOption.AllDirectories))
            {
                var fileContents = File.ReadAllText(file);

                if (fileContents.Contains("x:Uid"))
                {
                    var keepSearching = true;

                    var pos = 0;

                    while (keepSearching)
                    {
                        pos = fileContents.IndexOf("x:Uid=\"", pos + 1, StringComparison.Ordinal);

                        if (pos >= 0)
                        {
                            var uid = fileContents.Substring(pos + 7, fileContents.IndexOf("\"", pos + 7, StringComparison.Ordinal) - pos - 7);

                            if (!resources.Contains(uid))
                            {
                                uidsWithoutResources.Add(uid);
                            }
                        }
                        else
                        {
                            keepSearching = false;
                        }
                    }
                }
            }

            var validExceptions = new[] { "CameraPage_CameraControl", "PivotPage" };

            foreach (var validException in validExceptions)
            {
                uidsWithoutResources.Remove(validException);
            }

            Assert.True(uidsWithoutResources.Count == 0, $"Uids without string resources: {string.Join(", ", uidsWithoutResources)}");

            Fs.SafeDeleteDirectory(resultPath);
        }

        private async Task<string> GenerateProjectForTestingAsync(string projectName, string projectType, string framework, string platform, string language)
        {
            BaseGenAndBuildFixture.SetCurrentLanguage(language);
            BaseGenAndBuildFixture.SetCurrentPlatform(platform);

            // To initialize the list correctly
            var projectTemplate = _fixture.Templates().FirstOrDefault(t =>
                                                                        t.GetTemplateType() == TemplateType.Project &&
                                                                        t.GetProjectTypeList().Contains(projectType) &&
                                                                        t.GetFrontEndFrameworkList().Contains(framework));

            var destinationPath = Path.Combine(_fixture.TestProjectsPath, projectName, projectName);

            GenContext.Current = new FakeContextProvider
            {
                ProjectName = projectName,
                DestinationPath = destinationPath,
                GenerationOutputPath = destinationPath,
            };

            var context = new UserSelectionContext(language, platform)
            {
                ProjectType = projectType,
                FrontEndFramework = framework,
            };

            var userSelection = _fixture.SetupProject(context);

            var templates = _fixture.Templates()
                .Where(t => t.GetTemplateType().IsItemTemplate()
                && t.GetFrontEndFrameworkList().Contains(framework)
                && t.GetPlatform() == platform
                && !excludedTemplates_Uwp_Group1.Contains(t.GroupIdentity)
                && !excludedTemplatesGroup1VB.Contains(t.GroupIdentity)
                && !t.GetIsHidden());

            var templatesInfo = GenContext.ToolBox.Repo.GetTemplatesInfo(templates, context);

            _fixture.AddItems(userSelection, templatesInfo, BaseGenAndBuildFixture.GetDefaultName);

            await NewProjectGenController.Instance.UnsafeGenerateProjectAsync(userSelection);

            var resultPath = Path.Combine(_fixture.TestProjectsPath, projectName);

            return resultPath;
        }
    }
}
