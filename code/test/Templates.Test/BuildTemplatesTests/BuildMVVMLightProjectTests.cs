// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Threading.Tasks;

using Microsoft.Templates.Core;
using Microsoft.TemplateEngine.Abstractions;

using Xunit;

namespace Microsoft.Templates.Test
{
    [Collection("BuildTemplateTestCollection")]
    [Trait("ExecutionSet", "BuildMVVMLight")]
    public class BuildMVVMLightProjectTests : BaseGenAndBuildTests
    {
        public BuildMVVMLightProjectTests(BuildTemplatesTestFixture fixture)
        {
            _fixture = fixture;
            _fixture.InitializeFixtureAsync(this, "MVVMLight");
        }

        [Theory]
        [MemberData("GetProjectTemplatesForBuildAsync", "MVVMLight")]
        [Trait("Type", "BuildProjects")]
        public async Task BuildEmptyProjectAsync(string projectType, string framework, string platform, string language)
        {
            Func<ITemplateInfo, bool> selector =
                t => t.GetTemplateType() == TemplateType.Project
                    && t.GetProjectTypeList().Contains(projectType)
                    && t.GetFrameworkList().Contains(framework)
                    && t.GetPlatform() == platform
                    && !t.GetIsHidden()
                    && t.GetLanguage() == language;

            var projectName = $"{ShortProjectType(projectType)}{ShortLanguageName(language)}";

            var projectPath = await AssertGenerateProjectAsync(selector, projectName, projectType, framework, platform, language, null, false);

            AssertBuildProjectAsync(projectPath, projectName, platform);
        }

        [Theory]
        [MemberData("GetProjectTemplatesForBuildAsync", "MVVMLight")]
        [Trait("Type", "BuildAllPagesAndFeatures")]
        public async Task BuildAllPagesAndFeaturesAsync(string projectType, string framework, string platform, string language)
        {
            Func<ITemplateInfo, bool> selector =
                t => t.GetTemplateType() == TemplateType.Project
                    && t.GetProjectTypeList().Contains(projectType)
                    && t.GetFrameworkList().Contains(framework)
                    && t.GetPlatform() == platform
                    && !t.GetIsHidden()
                    && t.GetLanguage() == language;

            var projectName = $"{ShortProjectType(projectType)}All{ShortLanguageName(language)}";

            var projectPath = await AssertGenerateProjectAsync(selector, projectName, projectType, framework, platform, language, BaseGenAndBuildFixture.GetDefaultName, false);

            AssertBuildProjectAsync(projectPath, projectName, platform);
        }

        [Theory]
        [MemberData("GetProjectTemplatesForBuildAsync", "MVVMLight", ProgrammingLanguages.CSharp, Platforms.Uwp)]
        [Trait("Type", "BuildRandomNames")]
        [Trait("ExecutionSet", "Minimum")]
        [Trait("ExecutionSet", "BuildMinimum")]
        public async Task BuildAllPagesAndFeaturesRandomNamesCSAsync(string projectType, string framework, string platform, string language)
        {
            await BuildAllPagesAndFeaturesRandomNamesAsync(projectType, framework, platform, language);
        }

        [Theory]
        [MemberData("GetProjectTemplatesForBuildAsync", "MVVMLight", ProgrammingLanguages.VisualBasic, Platforms.Uwp)]
        [Trait("Type", "BuildRandomNames")]
        [Trait("ExecutionSet", "Minimum")]
        [Trait("ExecutionSet", "BuildMinimumVB")]
        public async Task BuildAllPagesAndFeaturesRandomNamesVBAsync(string projectType, string framework, string platform, string language)
        {
            await BuildAllPagesAndFeaturesRandomNamesAsync(projectType, framework, platform, language);
        }

        public async Task BuildAllPagesAndFeaturesRandomNamesAsync(string projectType, string framework, string platform, string language)
        {
            Func<ITemplateInfo, bool> selector =
                t => t.GetTemplateType() == TemplateType.Project
                    && t.GetProjectTypeList().Contains(projectType)
                    && t.GetFrameworkList().Contains(framework)
                    && t.GetPlatform() == platform
                    && !t.GetIsHidden()
                    && t.GetLanguage() == language;

            var projectName = $"{ShortProjectType(projectType)}AllRandom{ShortLanguageName(language)}";

            var projectPath = await AssertGenerateProjectAsync(selector, projectName, projectType, framework, platform, language, BaseGenAndBuildFixture.GetRandomName, false);

            AssertBuildProjectAsync(projectPath, projectName, platform);
        }

        [Theory]
        [MemberData("GetProjectTemplatesForBuildAsync", "MVVMLight")]
        [Trait("Type", "BuildRightClick")]
        public async Task BuildEmptyProjectWithAllRightClickItemsAsync(string projectType, string framework, string platform, string language)
        {
            var projectName = $"{ShortProjectType(projectType)}AllRC{ShortLanguageName(language)}";

            var projectPath = await AssertGenerateRightClickAsync(projectName, projectType, framework, platform, language, true, false);

            AssertBuildProjectAsync(projectPath, projectName, platform);
        }

        [Theory]
        [MemberData("GetProjectTemplatesForBuildAsync", "MVVMLight")]
        [Trait("Type", "BuildRightClick")]
        public async Task BuildCompleteProjectWithAllRightClickItemsAsync(string projectType, string framework, string platform, string language)
        {
            var projectName = $"{ShortProjectType(projectType)}AllRC2{ShortLanguageName(language)}";

            var projectPath = await AssertGenerateRightClickAsync(projectName, projectType, framework, platform, language, false, false);

            AssertBuildProjectAsync(projectPath, projectName, platform);
        }

        [Theory]
        [MemberData("GetPageAndFeatureTemplatesForBuildAsync", "MVVMLight")]
        [Trait("Type", "BuildOneByOneMVVMLight")]
        public async Task BuildMVVMLightOneByOneItemsAsync(string itemName, string projectType, string framework, string platform, string itemId, string language)
        {
            var result = await AssertGenerationOneByOneAsync(itemName, projectType, framework, platform, itemId, language, false);

            AssertBuildProjectAsync(result.ProjectPath, result.ProjecName, platform);
        }
    }
}
