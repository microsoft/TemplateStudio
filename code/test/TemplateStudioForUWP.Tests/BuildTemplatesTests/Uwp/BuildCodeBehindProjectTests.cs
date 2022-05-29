// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.TemplateEngine.Abstractions;
using Microsoft.Templates.Core;
using Microsoft.Templates.Core.Extensions;
using Microsoft.Templates.Core.Gen;
using TemplateStudioForUWP.Tests;
using Xunit;

namespace Microsoft.Templates.Test.UWP.Build
{
    [Trait("Group", "BuildUWP")]
    [Collection(nameof(UwpBuildTemplatesTestCollection))]
    public class BuildCodeBehindProjectTests : UwpBaseGenAndBuildTests
    {
        public BuildCodeBehindProjectTests(UwpBuildTemplatesTestFixture fixture)
            : base(fixture, null, Frameworks.CodeBehind)
        {
        }

        [Theory]
        [MemberData(nameof(UwpBaseGenAndBuildTests.GetProjectTemplatesForBuild), Frameworks.CodeBehind, "", Platforms.Uwp)]
        public async Task Build_EmptyProject_InferConfig_UwpAsync(string projectType, string framework, string platform, string language)
        {
            var context = new UserSelectionContext(language, platform)
            {
                ProjectType = projectType,
                FrontEndFramework = framework
            };

            var (projectName, projectPath) = await GenerateEmptyProjectAsync(context);

            // Don't delete after build test as used in inference test, which will then delete.
            AssertBuildProject(projectPath, projectName, platform, deleteAfterBuild: false);

            EnsureCanInferConfigInfo(context, projectPath);
        }

        [Theory]
        [MemberData(nameof(UwpBaseGenAndBuildTests.GetProjectTemplatesForBuild), Frameworks.CodeBehind, "", Platforms.Uwp)]
        public async Task Build_All_ProjectNameValidation_G1_UwpAsync(string projectType, string framework, string platform, string language)
        {
            bool templateSelector(ITemplateInfo t) => t.GetTemplateType().IsItemTemplate()
                && (t.GetProjectTypeList().Contains(projectType) || t.GetProjectTypeList().Contains(All))
                && (t.GetFrontEndFrameworkList().Contains(framework) || t.GetFrontEndFrameworkList().Contains(All))
                && t.GetPlatform() == platform
                && !excludedTemplates_Uwp_Group2.Contains(t.GroupIdentity)
                && !excludedTemplatesGroup2VB.Contains(t.GroupIdentity)
                && !t.GetIsHidden();

            var projectName = $"{(projectType)}{CharactersThatMayCauseProjectNameIssues()}G1{ShortLanguageName(language)}";

            var context = new UserSelectionContext(language, platform)
            {
                ProjectType = projectType,
                FrontEndFramework = framework
            };

            var projectPath = await AssertGenerateProjectAsync(projectName, context, templateSelector, BaseGenAndBuildFixture.GetRandomName);

            AssertBuildProject(projectPath, projectName, platform);
        }

        [Theory]
        [MemberData(nameof(UwpBaseGenAndBuildTests.GetProjectTemplatesForBuild), Frameworks.CodeBehind, "", Platforms.Uwp)]
        public async Task Build_All_ProjectNameValidation_G2_UwpAsync(string projectType, string framework, string platform, string language)
        {
            bool templateSelector(ITemplateInfo t) => t.GetTemplateType().IsItemTemplate()
                && (t.GetProjectTypeList().Contains(projectType) || t.GetProjectTypeList().Contains(All))
                && (t.GetFrontEndFrameworkList().Contains(framework) || t.GetFrontEndFrameworkList().Contains(All))
                && t.GetPlatform() == platform
                && !excludedTemplates_Uwp_Group1.Contains(t.GroupIdentity)
                && !excludedTemplatesGroup1VB.Contains(t.GroupIdentity)
                && !t.GetIsHidden();

            var projectName = $"{ShortProjectType(projectType)}{CharactersThatMayCauseProjectNameIssues()}G2{ShortLanguageName(language)}";

            var context = new UserSelectionContext(language, platform)
            {
                ProjectType = projectType,
                FrontEndFramework = framework
            };

            var projectPath = await AssertGenerateProjectAsync(projectName, context, templateSelector, BaseGenAndBuildFixture.GetRandomName);

            AssertBuildProject(projectPath, projectName, platform);
        }

        [Theory]
        [MemberData(nameof(UwpBaseGenAndBuildTests.GetProjectTemplatesForBuild), Frameworks.CodeBehind, ProgrammingLanguages.CSharp, Platforms.Uwp)]
        [Trait("Group", "MinimumUWP")]
        public async Task BuildAndTest_All_CheckWithStyleCop_G2_UwpAsync(string projectType, string framework, string platform, string language)
        {
            bool templateSelector(ITemplateInfo t) => t.GetTemplateType().IsItemTemplate()
                && (t.GetProjectTypeList().Contains(projectType) || t.GetProjectTypeList().Contains(All))
                && (t.GetFrontEndFrameworkList().Contains(framework) || t.GetFrontEndFrameworkList().Contains(All))
                && t.GetPlatform() == platform
                && !t.GetIsHidden()
                && !excludedTemplates_Uwp_Group1.Contains(t.GroupIdentity)
                || t.Identity == "ts.Feat.StyleCop";

            var projectName = $"{projectType}{framework}AllStyleCopG2";

            var context = new UserSelectionContext(language, platform)
            {
                ProjectType = projectType,
                FrontEndFramework = framework
            };

            var styleCopTemplates = GetAdditionalTemplates(System.IO.Path.Combine(Environment.CurrentDirectory, @"..\..\TestData\UWP")).Where(t => t.GetLanguage() == language);
            GenContext.ToolBox.Repo.AddAdditionalTemplates(styleCopTemplates);

            var projectPath = await AssertGenerateProjectAsync(projectName, context, templateSelector, BaseGenAndBuildFixture.GetDefaultName, includeMultipleInstances: true, styleCopTemplates);

            AssertBuildProjectThenRunTests(projectPath, projectName, platform);
        }

        [Theory]
        [MemberData(nameof(UwpBaseGenAndBuildTests.GetProjectTemplatesForBuild), Frameworks.CodeBehind, ProgrammingLanguages.CSharp, Platforms.Uwp)]
        [Trait("Group", "MinimumUWP")]
        public async Task BuildAndTest_All_CheckWithStyleCop_G1_UwpAsync(string projectType, string framework, string platform, string language)
        {
            bool templateSelector(ITemplateInfo t) => t.GetTemplateType().IsItemTemplate()
                && (t.GetProjectTypeList().Contains(projectType) || t.GetProjectTypeList().Contains(All))
                && (t.GetFrontEndFrameworkList().Contains(framework) || t.GetFrontEndFrameworkList().Contains(All))
                && t.GetPlatform() == platform
                && !t.GetIsHidden()
                && !excludedTemplates_Uwp_Group2.Contains(t.GroupIdentity)
                || t.Identity == "ts.Feat.StyleCop";

            var projectName = $"{projectType}{framework}AllStyleCopG1";

            var context = new UserSelectionContext(language, platform)
            {
                ProjectType = projectType,
                FrontEndFramework = framework
            };

            var styleCopTemplates = GetAdditionalTemplates(System.IO.Path.Combine(Environment.CurrentDirectory, @"..\..\TestData\UWP")).Where(t => t.GetLanguage() == language);
            GenContext.ToolBox.Repo.AddAdditionalTemplates(styleCopTemplates);

            var projectPath = await AssertGenerateProjectAsync(projectName, context, templateSelector, BaseGenAndBuildFixture.GetDefaultName, includeMultipleInstances: false, styleCopTemplates);

            AssertBuildProjectThenRunTests(projectPath, projectName, platform);
        }

        [Theory]
        [MemberData(nameof(UwpBaseGenAndBuildTests.GetProjectTemplatesForBuild), Frameworks.CodeBehind, "", Platforms.Uwp)]
        public async Task Build_Empty_AddRightClick_UwpAsync(string projectType, string framework, string platform, string language)
        {
            var projectName = $"{ShortProjectType(projectType)}AllR{ShortLanguageName(language)}";

            var context = new UserSelectionContext(language, platform)
            {
                ProjectType = projectType,
                FrontEndFramework = framework
            };

            var projectPath = await AssertGenerateRightClickAsync(projectName, context, true);

            AssertBuildProject(projectPath, projectName, platform);
        }

        [Theory]
        [MemberData(nameof(UwpBaseGenAndBuildTests.GetPageAndFeatureTemplatesForBuild), Frameworks.CodeBehind, ProgrammingLanguages.CSharp, Platforms.Uwp, "")]
        public async Task Build_CodeBehind_CS_OneByOneItems_UwpAsync(string itemName, string projectType, string framework, string platform, string itemId, string language)
        {
            var context = new UserSelectionContext(language, platform)
            {
                ProjectType = projectType,
                FrontEndFramework = framework
            };

            var (ProjectPath, ProjecName) = await AssertGenerationOneByOneAsync(itemName, context, itemId, false);

            AssertBuildProject(ProjectPath, ProjecName, platform);
        }

        [Theory]
        [MemberData(nameof(UwpBaseGenAndBuildTests.GetPageAndFeatureTemplatesForBuild), Frameworks.CodeBehind, ProgrammingLanguages.VisualBasic, Platforms.Uwp, "")]
        public async Task Build_CodeBehind_VB_OneByOneItems_UwpAsync(string itemName, string projectType, string framework, string platform, string itemId, string language)
        {
            var context = new UserSelectionContext(language, platform)
            {
                ProjectType = projectType,
                FrontEndFramework = framework
            };

            var (ProjectPath, ProjecName) = await AssertGenerationOneByOneAsync(itemName, context, itemId, false);

            AssertBuildProject(ProjectPath, ProjecName, platform);
        }
    }
}
