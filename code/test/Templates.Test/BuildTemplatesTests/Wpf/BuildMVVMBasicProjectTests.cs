// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Threading.Tasks;

using Microsoft.TemplateEngine.Abstractions;
using Microsoft.Templates.Core;
using Microsoft.Templates.Core.Extensions;
using Xunit;

namespace Microsoft.Templates.Test.Wpf
{
    [Collection("BuildTemplateTestCollection")]
    public class BuildMVVMBasicProjectTests : BaseGenAndBuildTests
    {
        public BuildMVVMBasicProjectTests(BuildTemplatesTestFixture fixture)
            : base(fixture, null, "MVVMBasic")
        {
        }

        [Theory]
        [MemberData(nameof(BaseGenAndBuildTests.GetProjectTemplatesForBuild), "MVVMBasic", "", Platforms.Wpf)]
        [Trait("ExecutionSet", "BuildMVVMBasicWpf")]
        [Trait("ExecutionSet", "_Full")]
        [Trait("Type", "BuildProjects")]
        public async Task BuildEmptyProjectWpfAsync(string projectType, string framework, string platform, string language)
        {
            var (projectName, projectPath) = await GenerateEmptyProjectAsync(projectType, framework, platform, language);

            AssertBuildProjectAsync(projectPath, projectName, platform, true);
        }

        [Theory]
        [MemberData(nameof(BaseGenAndBuildTests.GetProjectTemplatesForBuild), "MVVMBasic", "", Platforms.Wpf)]
        [Trait("ExecutionSet", "BuildMVVMBasicWpf")]
        [Trait("ExecutionSet", "_Full")]
        [Trait("Type", "BuildAllPagesAndFeaturesWpf")]
        [Trait("Type", "BuildRandomNamesWpf")]
        public async Task BuildAllPagesAndFeaturesProjectNameValidationWpfAsync(string projectType, string framework, string platform, string language)
        {
            Func<ITemplateInfo, bool> templateSelector =
                t => t.GetTemplateType().IsItemTemplate()
                && (t.GetProjectTypeList().Contains(projectType) || t.GetProjectTypeList().Contains(All))
                && (t.GetFrontEndFrameworkList().Contains(framework) || t.GetFrontEndFrameworkList().Contains(All))
                && t.GetPlatform() == platform
                && t.Identity != "wts.Wpf.Feat.MSIXPackaging"
                && !t.GetIsHidden();

            var projectName = $"{ShortProjectType(projectType)}{CharactersThatMayCauseProjectNameIssues()}G1{ShortLanguageName(language)}";

            var projectPath = await AssertGenerateProjectAsync(projectName, projectType, framework, platform, language, templateSelector, BaseGenAndBuildFixture.GetRandomName);

            AssertBuildProjectAsync(projectPath, projectName, platform);
        }

        [Theory]
        [MemberData(nameof(BaseGenAndBuildTests.GetProjectTemplatesForBuild), "MVVMBasic", ProgrammingLanguages.CSharp, Platforms.Wpf)]
        [Trait("ExecutionSet", "MinimumWpf")]
        [Trait("ExecutionSet", "MinimumMVVMBasicWpf")]
        [Trait("ExecutionSet", "_CIBuild")]
        [Trait("ExecutionSet", "_Full")]
        [Trait("Type", "CodeStyleWpf")]
        public async Task GenerateAllAndCheckWithStyleCopWpfAsync(string projectType, string framework, string platform, string language)
        {
            Func<ITemplateInfo, bool> templateSelector =
                t => t.GetTemplateType().IsItemTemplate()
                && (t.GetProjectTypeList().Contains(projectType) || t.GetProjectTypeList().Contains(All))
                && (t.GetFrontEndFrameworkList().Contains(framework) || t.GetFrontEndFrameworkList().Contains(All))
                && t.GetPlatform() == platform
                && !t.GetIsHidden()
                && t.Identity != "wts.Wpf.Feat.MSIXPackaging"
                || t.Identity == "wts.Wpf.Feat.StyleCop";

            var projectName = $"{projectType}{framework}All";

            var projectPath = await AssertGenerateProjectAsync(projectName, projectType, framework, platform, language, templateSelector, BaseGenAndBuildFixture.GetDefaultName);

            AssertBuildProjectAsync(projectPath, projectName, platform);
        }

        [Theory]
        [MemberData(nameof(BaseGenAndBuildTests.GetPageAndFeatureTemplatesForBuild), "MVVMBasic", ProgrammingLanguages.CSharp, Platforms.Wpf, "wts.Wpf.Feat.MSIXPackaging")]
        [Trait("ExecutionSet", "BuildOneByOneMVVMBasicWpf")]
        [Trait("ExecutionSet", "_OneByOne")]
        [Trait("Type", "BuildOneByOneMVVMBasicWpf")]
        public async Task BuildMVVMBasicOneByOneItemsWpfAsync(string itemName, string projectType, string framework, string platform, string itemId, string language)
        {
            var result = await AssertGenerationOneByOneAsync(itemName, projectType, framework, platform, itemId, language, false);

            AssertBuildProjectAsync(result.ProjectPath, result.ProjecName, platform);
        }
    }
}
