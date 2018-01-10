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
    [Trait("ExecutionSet", "BuildCodeBehind")]
    public class BuildCodeBehindProjectTests : BaseGenAndBuildTests
    {
        public BuildCodeBehindProjectTests(BuildTemplatesTestFixture fixture)
        {
            _fixture = fixture;
            _fixture.InitializeFixtureAsync(this, "CodeBehind");
        }

        [Theory]
        [MemberData("GetProjectTemplatesForBuildAsync", "CodeBehind")]
        [Trait("Type", "BuildProjects")]
        public async Task BuildEmptyProjectAsync(string projectType, string framework, string language)
        {
            Func<ITemplateInfo, bool> selector =
                t => t.GetTemplateType() == TemplateType.Project
                    && t.GetProjectTypeList().Contains(projectType)
                    && t.GetFrameworkList().Contains(framework)
                    && !t.GetIsHidden()
                    && t.GetLanguage() == language;

            var projectName = $"{projectType}{framework}{ShortLanguageName(language)}";

            var projectPath = await AssertGenerateProjectAsync(selector, projectName, projectType, framework, language, null, false);

            AssertBuildProjectAsync(projectPath, projectName);
        }

        [Theory]
        [MemberData("GetProjectTemplatesForBuildAsync", "CodeBehind")]
        [Trait("Type", "BuildAllPagesAndFeatures")]
        public async Task BuildAllPagesAndFeaturesAsync(string projectType, string framework, string language)
        {
            Func<ITemplateInfo, bool> selector =
                t => t.GetTemplateType() == TemplateType.Project
                    && t.GetProjectTypeList().Contains(projectType)
                    && t.GetFrameworkList().Contains(framework)
                    && !t.GetIsHidden()
                    && t.GetLanguage() == language;

            var projectName = $"{projectType}{framework}All{ShortLanguageName(language)}";

            var projectPath = await AssertGenerateProjectAsync(selector, projectName, projectType, framework, language, BaseGenAndBuildFixture.GetDefaultName, false);

            AssertBuildProjectAsync(projectPath, projectName);
        }

        [Theory]
        [MemberData("GetProjectTemplatesForBuildAsync", "CodeBehind", ProgrammingLanguages.CSharp)]
        [Trait("Type", "BuildRandomNames")]
        [Trait("ExecutionSet", "Minimum")]
        [Trait("ExecutionSet", "BuildMinimum")]
        public async Task BuildAllPagesAndFeaturesRandomNamesCSAsync(string projectType, string framework, string language)
        {
            await BuildAllPagesAndFeaturesRandomNamesAsync(projectType, framework, language);
        }

        [Theory]
        [MemberData("GetProjectTemplatesForBuildAsync", "CodeBehind", ProgrammingLanguages.VisualBasic)]
        [Trait("Type", "BuildRandomNames")]
        [Trait("ExecutionSet", "Minimum")]
        [Trait("ExecutionSet", "BuildMinimumVB")]
        public async Task BuildAllPagesAndFeaturesRandomNamesVBAsync(string projectType, string framework, string language)
        {
            await BuildAllPagesAndFeaturesRandomNamesAsync(projectType, framework, language);
        }

        private async Task BuildAllPagesAndFeaturesRandomNamesAsync(string projectType, string framework, string language)
        {
            Func<ITemplateInfo, bool> selector =
                t => t.GetTemplateType() == TemplateType.Project
                     && t.GetProjectTypeList().Contains(projectType)
                     && t.GetFrameworkList().Contains(framework)
                     && !t.GetIsHidden()
                     && t.GetLanguage() == language;

            var projectName = $"{projectType}{framework}AllRandom{ShortLanguageName(language)}";

            var projectPath = await AssertGenerateProjectAsync(selector, projectName, projectType, framework, language, BaseGenAndBuildFixture.GetRandomName, false);

            AssertBuildProjectAsync(projectPath, projectName);
        }

        [Theory]
        [MemberData("GetProjectTemplatesForBuildAsync", "CodeBehind")]
        [Trait("Type", "BuildRightClick")]
        public async Task BuildEmptyProjectWithAllRightClickItemsAsync(string projectType, string framework, string language)
        {
            var projectName = $"{projectType}{framework}AllRightClick{ShortLanguageName(language)}";

            var projectPath = await AssertGenerateRightClickAsync(projectName, projectType, framework, language, true, false);

            AssertBuildProjectAsync(projectPath, projectName);
        }

        [Theory]
        [MemberData("GetProjectTemplatesForBuildAsync", "CodeBehind")]
        [Trait("Type", "BuildRightClick")]
        public async Task BuildCompleteProjectWithAllRightClickItemsAsync(string projectType, string framework, string language)
        {
            var projectName = $"{projectType}{framework}AllRightClick2{ShortLanguageName(language)}";

            var projectPath = await AssertGenerateRightClickAsync(projectName, projectType, framework, language, false, false);

            AssertBuildProjectAsync(projectPath, projectName);
        }

        [Theory]
        [MemberData("GetPageAndFeatureTemplatesForBuildAsync", "CodeBehind")]
        [Trait("Type", "BuildOneByOneCodeBehind")]
        public async Task BuildCodeBehindOneByOneItemsAsync(string itemName, string projectType, string framework, string itemId, string language)
        {
            var result = await AssertGenerationOneByOneAsync(itemName, projectType, framework, itemId, language, false);

            AssertBuildProjectAsync(result.ProjectPath, result.ProjecName);
        }
    }
}
