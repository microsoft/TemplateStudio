// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Threading.Tasks;

using Microsoft.Templates.Core;
using Microsoft.TemplateEngine.Abstractions;

using Xunit;
using Microsoft.Templates.Core.Gen;
using System.Linq;
using Microsoft.Templates.Core.Extensions;

namespace Microsoft.Templates.Test
{
    [Collection("BuildTemplateTestCollection")]
    public class BuildPrismProjectTests : BaseGenAndBuildTests
    {
        public BuildPrismProjectTests(BuildTemplatesTestFixture fixture)
            : base(fixture, null, "Prism")
        {
        }

        [Theory]
        [MemberData(nameof(BaseGenAndBuildTests.GetProjectTemplatesForBuild), "Prism")]
        [Trait("ExecutionSet", "BuildPrism")]
        [Trait("ExecutionSet", "_Full")]
        [Trait("Type", "BuildProjects")]
        public async Task BuildEmptyProjectAsync(string projectType, string framework, string platform, string language)
        {
            var (projectName, projectPath) = await GenerateEmptyProjectAsync(projectType, framework, platform, language);

            AssertBuildProjectAsync(projectPath, projectName, platform);
        }

        [Theory]
        [MemberData(nameof(BaseGenAndBuildTests.GetProjectTemplatesForBuild), "Prism")]
        [Trait("ExecutionSet", "BuildPrism")]
        [Trait("ExecutionSet", "_Full")]
        [Trait("Type", "BuildProjects")]
        public async Task BuildEmptyProjectAndInferConfigAsync(string projectType, string framework, string platform, string language)
        {
            var (projectName, projectPath) = await GenerateEmptyProjectAsync(projectType, framework, platform, language);
            _fixture.BuildSolution(projectName, projectPath, platform);

            EnsureCanInferConfigInfo(projectType, framework, platform, projectPath);
        }

        [Theory]
        [MemberData(nameof(BaseGenAndBuildTests.GetProjectTemplatesForBuild), "Prism")]
        [Trait("ExecutionSet", "BuildPrism")]
        [Trait("ExecutionSet", "_Full")]
        [Trait("Type", "BuildAllPagesAndFeatures")]
        public async Task BuildAllPagesAndFeaturesAsync(string projectType, string framework, string platform, string language)
        {
            var (projectName, projectPath) = await GenerateAllPagesAndFeaturesAsync(projectType, framework, platform, language);

            AssertBuildProjectAsync(projectPath, projectName, platform);
        }

        [Theory]
        [MemberData(nameof(BaseGenAndBuildTests.GetProjectTemplatesForBuild), "Prism")]
        [Trait("ExecutionSet", "BuildPrism")]
        [Trait("ExecutionSet", "_Full")]
        [Trait("Type", "BuildAllPagesAndFeatures")]
        public async Task BuildAllPagesAndFeaturesThenRunTestsAsync(string projectType, string framework, string platform, string language)
        {
            var (projectName, projectPath) = await GenerateAllPagesAndFeaturesAsync(projectType, framework, platform, language);

            AssertBuildProjectThenRunTestsAsync(projectPath, projectName, platform);
        }

        [Theory]
        [MemberData(nameof(BaseGenAndBuildTests.GetProjectTemplatesForBuild), "Prism")]
        [Trait("ExecutionSet", "BuildPrism")]
        [Trait("ExecutionSet", "_Full")]
        [Trait("Type", "BuildAllPagesAndFeatures")]
        public async Task BuildAllPagesAndFeaturesProjectNameValidationAsync(string projectType, string framework, string platform, string language)
        {
            // get first item from each exclusive selection group
            var exclusiveSelectionGroups = GenContext.ToolBox.Repo.GetAll().Where(t =>
                t.GetTemplateType().IsItemTemplate()
                && (t.GetProjectTypeList().Contains(projectType) || t.GetProjectTypeList().Contains(All))
                && t.GetFrontEndFrameworkList().Contains(framework)
                && t.GetPlatform() == platform
                && t.GetIsGroupExclusiveSelection()).GroupBy(t => t.GetGroup(), (key, g) => g.First());

            Func<ITemplateInfo, bool> templateSelector =
                t => t.GetTemplateType().IsItemTemplate()
                && (t.GetProjectTypeList().Contains(projectType) || t.GetProjectTypeList().Contains(All))
                && t.GetFrontEndFrameworkList().Contains(framework)
                && t.GetPlatform() == platform
                && (!t.GetIsGroupExclusiveSelection() || (t.GetIsGroupExclusiveSelection() && exclusiveSelectionGroups.Contains(t)))
                && !t.GetIsHidden();

            var projectName = $"{ShortProjectType(projectType)}{CharactersThatMayCauseProjectNameIssues()}{ShortLanguageName(language)}";

            var projectPath = await AssertGenerateProjectAsync(projectName, projectType, framework, platform, language, templateSelector, BaseGenAndBuildFixture.GetDefaultName);

            AssertBuildProjectAsync(projectPath, projectName, platform);
        }

        [Theory]
        [MemberData(nameof(BaseGenAndBuildTests.GetProjectTemplatesForBuild), "Prism", ProgrammingLanguages.CSharp, Platforms.Uwp)]
        [Trait("ExecutionSet", "Minimum")]
        [Trait("ExecutionSet", "MinimumPrism")]
        [Trait("ExecutionSet", "_CIBuild")]
        [Trait("ExecutionSet", "_Full")]
        [Trait("Type", "CodeStyle")]
        public async Task GenerateAllPagesAndFeaturesAndCheckWithStyleCopAsyncWithForcedLogin(string projectType, string framework, string platform, string language)
        {
            Func<ITemplateInfo, bool> templateSelector =
                t => t.GetTemplateType().IsItemTemplate()
                && (t.GetProjectTypeList().Contains(projectType) || t.GetProjectTypeList().Contains(All))
                && t.GetFrontEndFrameworkList().Contains(framework)
                && t.GetPlatform() == platform
                && !t.GetIsHidden()
                && t.GroupIdentity != "wts.Service.IdentityOptionalLogin"
                || t.Identity == "wts.Feat.StyleCop";

            var projectName = $"{projectType}{framework}AllStyleCopF";

            var projectPath = await AssertGenerateProjectAsync(projectName, projectType, framework, platform, language, templateSelector, BaseGenAndBuildFixture.GetDefaultName);

            AssertBuildProjectAsync(projectPath, projectName, platform);
        }

        [Theory]
        [MemberData(nameof(BaseGenAndBuildTests.GetProjectTemplatesForBuild), "Prism", ProgrammingLanguages.CSharp, Platforms.Uwp)]
        [Trait("ExecutionSet", "Minimum")]
        [Trait("ExecutionSet", "MinimumPrism")]
        [Trait("ExecutionSet", "_CIBuild")]
        [Trait("ExecutionSet", "_Full")]
        [Trait("Type", "CodeStyle")]
        public async Task GenerateAllPagesAndFeaturesAndCheckWithStyleCopAsyncWithOptionalLogin(string projectType, string framework, string platform, string language)
        {
            Func<ITemplateInfo, bool> templateSelector =
                t => t.GetTemplateType().IsItemTemplate()
                && (t.GetProjectTypeList().Contains(projectType) || t.GetProjectTypeList().Contains(All))
                && t.GetFrontEndFrameworkList().Contains(framework)
                && t.GetPlatform() == platform
                && !t.GetIsHidden()
                && t.GroupIdentity != "wts.Service.IdentityForcedLogin"
                || t.Identity == "wts.Feat.StyleCop";

            var projectName = $"{projectType}{framework}AllStyleCopO";

            var projectPath = await AssertGenerateProjectAsync(projectName, projectType, framework, platform, language, templateSelector, BaseGenAndBuildFixture.GetDefaultName);

            AssertBuildProjectAsync(projectPath, projectName, platform);
        }

        [Theory]
        [MemberData(nameof(BaseGenAndBuildTests.GetProjectTemplatesForBuild), "Prism")]
        [Trait("Type", "BuildRandomNames")]
        [Trait("ExecutionSet", "Minimum")]
        [Trait("ExecutionSet", "BuildPrism")]
        [Trait("ExecutionSet", "_Full")]
        public async Task BuildAllPagesAndFeaturesRandomNamesAsync(string projectType, string framework, string platform, string language)
        {
            // get first item from each exclusive selection group
            var exclusiveSelectionGroups = GenContext.ToolBox.Repo.GetAll().Where(t =>
                t.GetTemplateType().IsItemTemplate()
                && (t.GetProjectTypeList().Contains(projectType) || t.GetProjectTypeList().Contains(All))
                && t.GetFrontEndFrameworkList().Contains(framework)
                && t.GetPlatform() == platform
                && t.GetIsGroupExclusiveSelection()).GroupBy(t => t.GetGroup(), (key, g) => g.First());

            Func<ITemplateInfo, bool> templateSelector =
                t => t.GetTemplateType().IsItemTemplate()
                && (t.GetProjectTypeList().Contains(projectType) || t.GetProjectTypeList().Contains(All))
                && t.GetFrontEndFrameworkList().Contains(framework)
                && t.GetPlatform() == platform
                && (!t.GetIsGroupExclusiveSelection() || (t.GetIsGroupExclusiveSelection() && exclusiveSelectionGroups.Contains(t)))
                && !t.GetIsHidden();

            var projectName = $"{ShortProjectType(projectType)}AllRandom";

            var projectPath = await AssertGenerateProjectAsync(projectName, projectType, framework, platform, language, templateSelector, GenerationFixture.GetRandomName);

            AssertBuildProjectAsync(projectPath, projectName, platform);
        }

        [Theory]
        [MemberData(nameof(BaseGenAndBuildTests.GetProjectTemplatesForBuild), "Prism")]
        [Trait("ExecutionSet", "BuildPrism")]
        [Trait("ExecutionSet", "_Full")]
        [Trait("Type", "BuildRightClick")]
        public async Task BuildEmptyProjectWithAllRightClickItemsAsync(string projectType, string framework, string platform, string language)
        {
            var projectName = $"{ShortProjectType(projectType)}AllRightClick";

            var projectPath = await AssertGenerateRightClickAsync(projectName, projectType, framework, platform, language, true);

            AssertBuildProjectAsync(projectPath, projectName, platform);
        }

        [Theory]
        [MemberData(nameof(BaseGenAndBuildTests.GetProjectTemplatesForBuild), "Prism")]
        [Trait("ExecutionSet", "BuildPrism")]
        [Trait("ExecutionSet", "_Full")]
        [Trait("Type", "BuildRightClick")]
        public async Task BuildCompleteProjectWithAllRightClickItemsWithForcedLoginAsync(string projectType, string framework, string platform, string language)
        {
            var projectName = $"{ShortProjectType(projectType)}AllRCF";

            var projectPath = await AssertGenerateRightClickAsync(projectName, projectType, framework, platform, language, false, "wts.Service.IdentityOptionalLogin");

            AssertBuildProjectAsync(projectPath, projectName, platform);
        }

        [Theory]
        [MemberData(nameof(BaseGenAndBuildTests.GetProjectTemplatesForBuild), "Prism")]
        [Trait("ExecutionSet", "BuildPrism")]
        [Trait("ExecutionSet", "_Full")]
        [Trait("Type", "BuildRightClick")]
        public async Task BuildCompleteProjectWithAllRightClickItemsWithOptionalLoginAsync(string projectType, string framework, string platform, string language)
        {
            var projectName = $"{ShortProjectType(projectType)}AllRCO";

            var projectPath = await AssertGenerateRightClickAsync(projectName, projectType, framework, platform, language, false, "wts.Service.IdentityForcedLogin");

            AssertBuildProjectAsync(projectPath, projectName, platform);
        }

        [Theory]
        [MemberData(nameof(BaseGenAndBuildTests.GetPageAndFeatureTemplatesForBuild), "Prism", ProgrammingLanguages.CSharp)]
        [Trait("ExecutionSet", "BuildOneByOnePrism")]
        [Trait("ExecutionSet", "_OneByOne")]
        [Trait("Type", "BuildOneByOnePrism")]
        public async Task BuildPrismOneByOneItemsAsync(string itemName, string projectType, string framework, string platform, string itemId, string language)
        {
            var result = await AssertGenerationOneByOneAsync(itemName, projectType, framework, platform, itemId, language, false);

            AssertBuildProjectAsync(result.ProjectPath, result.ProjecName, platform);
        }

     
    }
}
