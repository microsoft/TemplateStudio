// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Threading.Tasks;

using Microsoft.Templates.Core;
using Microsoft.TemplateEngine.Abstractions;

using Xunit;
using Microsoft.Templates.Fakes;

namespace Microsoft.Templates.Test
{
    [Collection("BuildTemplateTestCollection")]
    [Trait("ExecutionSet", "BuildPrism")]
    [Trait("ExecutionSet", "Build")]
    public class BuildPrismProjectTests : BaseGenAndBuildTests
    {
        public BuildPrismProjectTests(BuildTemplatesTestFixture fixture)
            : base(fixture, null, "Prism")
        {
        }

        [Theory]
        [MemberData(nameof(BaseGenAndBuildTests.GetProjectTemplatesForBuild), "Prism")]
        [Trait("Type", "BuildProjects")]
        public async Task BuildEmptyProjectAsync(string projectType, string framework, string platform, string language)
        {
            var (projectName, projectPath) = await GenerateEmptyProjectAsync(projectType, framework, platform, language);

            AssertBuildProjectAsync(projectPath, projectName, platform);
        }

        [Theory]
        [MemberData(nameof(BaseGenAndBuildTests.GetProjectTemplatesForBuild), "Prism")]
        [Trait("Type", "BuildProjects")]
        public async Task BuildEmptyProjectAndInferConfigAsync(string projectType, string framework, string platform, string language)
        {
            var (projectName, projectPath) = await GenerateEmptyProjectAsync(projectType, framework, platform, language);
            _fixture.BuildSolution(projectName, projectPath, platform);

            EnsureCanInferConfigInfo(projectType, framework, platform, projectPath);
        }

        [Theory]
        [MemberData(nameof(BaseGenAndBuildTests.GetProjectTemplatesForBuild), "Prism")]
        [Trait("Type", "BuildAllPagesAndFeatures")]
        public async Task BuildAllPagesAndFeaturesAsync(string projectType, string framework, string platform, string language)
        {
            var (projectName, projectPath) = await GenerateAllPagesAndFeaturesAsync(projectType, framework, platform, language);

            AssertBuildProjectAsync(projectPath, projectName, platform);
        }

        [Theory]
        [MemberData(nameof(BaseGenAndBuildTests.GetProjectTemplatesForBuild), "Prism")]
        [Trait("Type", "BuildAllPagesAndFeatures")]
        public async Task BuildAllPagesAndFeaturesThenRunTestsAsync(string projectType, string framework, string platform, string language)
        {
            var (projectName, projectPath) = await GenerateAllPagesAndFeaturesAsync(projectType, framework, platform, language);

            AssertBuildProjectThenRunTestsAsync(projectPath, projectName, platform);
        }

        [Theory]
        [MemberData(nameof(BaseGenAndBuildTests.GetProjectTemplatesForBuild), "Prism")]
        [Trait("Type", "BuildAllPagesAndFeatures")]
        public async Task BuildAllPagesAndFeaturesProjectNameValidationAsync(string projectType, string framework, string platform, string language)
        {
            Func<ITemplateInfo, bool> selector =
                t => t.GetTemplateType() == TemplateType.Project
                     && t.GetProjectTypeList().Contains(projectType)
                     && t.GetFrameworkList().Contains(framework)
                     && t.GetPlatform() == platform
                     && !t.GetIsHidden()
                     && t.GetLanguage() == language;

            Func<ITemplateInfo, bool> templateSelector =
                t => (t.GetTemplateType() == TemplateType.Page || t.GetTemplateType() == TemplateType.Feature)
                     && t.GetFrameworkList().Contains(framework)
                     && t.GetPlatform() == platform
                     && !t.GetIsHidden();

            var projectName = $"{ShortProjectType(projectType)}{CharactersThatMayCauseProjectNameIssues()}{ShortLanguageName(language)}";

            var projectPath = await AssertGenerateProjectAsync(selector, projectName, projectType, framework, platform, language, templateSelector, BaseGenAndBuildFixture.GetDefaultName, false);

            AssertBuildProjectAsync(projectPath, projectName, platform);
        }

        [Theory]
        [MemberData(nameof(BaseGenAndBuildTests.GetProjectTemplatesForBuild), "Prism")]
        [Trait("Type", "BuildRandomNames")]
        [Trait("ExecutionSet", "Minimum")]
        [Trait("ExecutionSet", "BuildMinimum")]
        public async Task BuildAllPagesAndFeaturesRandomNamesAsync(string projectType, string framework, string platform, string language)
        {
            Func<ITemplateInfo, bool> selector =
                t => t.GetTemplateType() == TemplateType.Project
                    && t.GetProjectTypeList().Contains(projectType)
                    && t.GetFrameworkList().Contains(framework)
                    && !t.GetIsHidden()
                    && t.GetLanguage() == language;

            Func<ITemplateInfo, bool> templateSelector =
                t => (t.GetTemplateType() == TemplateType.Page || t.GetTemplateType() == TemplateType.Feature)
                    && t.GetFrameworkList().Contains(framework)
                    && t.GetPlatform() == platform
                    && !t.GetIsHidden();

            var projectName = $"{ShortProjectType(projectType)}AllRandom";

            var projectPath = await AssertGenerateProjectAsync(selector, projectName, projectType, framework, platform, language, templateSelector, GenerationFixture.GetRandomName, false);

            AssertBuildProjectAsync(projectPath, projectName, platform);
        }

        [Theory]
        [MemberData(nameof(BaseGenAndBuildTests.GetProjectTemplatesForBuild), "Prism")]
        [Trait("Type", "BuildRightClick")]
        public async Task BuildEmptyProjectWithAllRightClickItemsAsync(string projectType, string framework, string platform, string language)
        {
            var projectName = $"{ShortProjectType(projectType)}AllRightClick";

            var projectPath = await AssertGenerateRightClickAsync(projectName, projectType, framework, platform, language, true, false);

            AssertBuildProjectAsync(projectPath, projectName, platform);
        }

        [Theory]
        [MemberData(nameof(BaseGenAndBuildTests.GetProjectTemplatesForBuild), "Prism")]
        [Trait("Type", "BuildRightClick")]
        public async Task BuildCompleteProjectWithAllRightClickItemsAsync(string projectType, string framework, string platform, string language)
        {
            var projectName = $"{ShortProjectType(projectType)}AllRightClick2";

            var projectPath = await AssertGenerateRightClickAsync(projectName, projectType, framework, platform, language, false, false);

            AssertBuildProjectAsync(projectPath, projectName, platform);
        }

        [Theory]
        [MemberData(nameof(BaseGenAndBuildTests.GetPageAndFeatureTemplatesForBuild), "Prism")]
        [Trait("Type", "BuildOneByOnePrism")]
        public async Task BuildPrismOneByOneItemsAsync(string itemName, string projectType, string framework, string platform, string itemId, string language)
        {
            var result = await AssertGenerationOneByOneAsync(itemName, projectType, framework, platform, itemId, language, false);

            AssertBuildProjectAsync(result.ProjectPath, result.ProjecName, platform);
        }
    }
}
