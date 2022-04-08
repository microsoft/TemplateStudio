// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Threading.Tasks;

using Microsoft.TemplateEngine.Abstractions;
using Microsoft.Templates.Core;
using Microsoft.Templates.Core.Extensions;
using Microsoft.Templates.Core.Gen;
using TemplateStudioForWinUICs.Tests;
using Xunit;

namespace Microsoft.Templates.Test.WinUICs.Build
{
    [Trait("Group", "BuildWinUICs")]
    [Collection(nameof(WinUICsBuildTemplatesTestCollection))]
    public class BuildMVVMToolkitProjectTests : WinUICsBaseGenAndBuildTests
    {
        public BuildMVVMToolkitProjectTests(WinUICsBuildTemplatesTestFixture fixture)
            : base(fixture, null, Frameworks.MVVMToolkit)
        {
        }

        [Theory]
        [MemberData(nameof(WinUICsBaseGenAndBuildTests.GetProjectTemplatesForBuild), Frameworks.MVVMToolkit, ProgrammingLanguages.CSharp, Platforms.WinUI)]
        public async Task Build_EmptyProject_MVVMToolkitAsync(string projectType, string framework, string platform, string language, string appModel)
        {
            var context = new UserSelectionContext(language, platform)
            {
                ProjectType = projectType,
                FrontEndFramework = framework,
            };
            context.AddAppModel(appModel);

            var (projectName, projectPath) = await GenerateEmptyProjectAsync(context);

            // Don't delete after build test as used in inference test, which will then delete.
            AssertBuildProject(projectPath, projectName, platform, deleteAfterBuild: false);

            EnsureCanInferConfigInfo(context, projectPath);
        }

        [Theory]
        [MemberData(nameof(WinUICsBaseGenAndBuildTests.GetProjectTemplatesForBuild), Frameworks.MVVMToolkit, ProgrammingLanguages.CSharp, Platforms.WinUI)]
        public async Task Build_All_ProjectNameValidation_WinUIAsync(string projectType, string framework, string platform, string language, string appModel)
        {
            bool templateSelector(ITemplateInfo t) => t.GetTemplateType().IsItemTemplate()
                && (t.GetProjectTypeList().Contains(projectType) || t.GetProjectTypeList().Contains(All))
                && (t.GetFrontEndFrameworkList().Contains(framework) || t.GetFrontEndFrameworkList().Contains(All))
                && t.GetPlatform() == platform
                && t.GetPropertyBagValuesList("appmodel").Contains(appModel)
                && !t.GetIsHidden();

            var projectName = $"{ShortProjectType(projectType)}{CharactersThatMayCauseProjectNameIssues()}";

            var context = new UserSelectionContext(language, platform)
            {
                ProjectType = projectType,
                FrontEndFramework = framework,
            };
            context.AddAppModel(appModel);

            var projectPath = await AssertGenerateProjectAsync(projectName, context, templateSelector, BaseGenAndBuildFixture.GetRandomName);

            AssertBuildProject(projectPath, projectName, platform);
        }

        [Theory]
        [MemberData(nameof(WinUICsBaseGenAndBuildTests.GetProjectTemplatesForBuild), Frameworks.MVVMToolkit, ProgrammingLanguages.CSharp, Platforms.WinUI)]
        [Trait("Group", "MinimumWinUICs")]
        public async Task Build_All_CheckWithStyleCop_WinUIAsync(string projectType, string framework, string platform, string language, string appModel)
        {
            bool templateSelector(ITemplateInfo t) => t.GetTemplateType().IsItemTemplate()
                && (t.GetProjectTypeList().Contains(projectType) || t.GetProjectTypeList().Contains(All))
                && (t.GetFrontEndFrameworkList().Contains(framework) || t.GetFrontEndFrameworkList().Contains(All))
                && t.GetPlatform() == platform
                && t.GetPropertyBagValuesList("appmodel").Contains(appModel)
                && !t.GetIsHidden()
                || (t.Identity == "ts.WinUI.Feat.StyleCop");

            var projectName = $"{projectType}{framework}AllStyleCop";

            var context = new UserSelectionContext(language, platform)
            {
                ProjectType = projectType,
                FrontEndFramework = framework,
            };
            context.AddAppModel(appModel);

            var styleCopTemplates = GetAdditionalTemplates(System.IO.Path.Combine(Environment.CurrentDirectory, @"..\..\TestData\WinUI"));
            GenContext.ToolBox.Repo.AddAdditionalTemplates(styleCopTemplates);

            var projectPath = await AssertGenerateProjectAsync(projectName, context, templateSelector, BaseGenAndBuildFixture.GetDefaultName, includeMultipleInstances: true, styleCopTemplates);

            AssertBuildProject(projectPath, projectName, platform);
        }

        [Theory]
        [MemberData(nameof(WinUICsBaseGenAndBuildTests.GetProjectTemplatesForBuild), Frameworks.MVVMToolkit, ProgrammingLanguages.CSharp, Platforms.WinUI)]
        public async Task Build_Empty_AddRightClick_WinUIAsync(string projectType, string framework, string platform, string language, string appModel)
        {
            var projectName = $"{ShortProjectType(projectType)}AllRC";

            var context = new UserSelectionContext(language, platform)
            {
                ProjectType = projectType,
                FrontEndFramework = framework,
            };
            context.AddAppModel(appModel);

            var projectPath = await AssertGenerateRightClickAsync(projectName, context, true);

            AssertBuildProject(projectPath, projectName, platform);
        }

        [Theory]
        [MemberData(nameof(WinUICsBaseGenAndBuildTests.GetPageAndFeatureTemplatesForBuild), Frameworks.MVVMToolkit, ProgrammingLanguages.CSharp, Platforms.WinUI, "")]
        public async Task Build_MVVMToolkit_OneByOneItems_WinUIAsync(string itemName, string projectType, string framework, string platform, string itemId, string language, string appModel)
        {
            var context = new UserSelectionContext(language, platform)
            {
                ProjectType = projectType,
                FrontEndFramework = framework,
            };
            context.AddAppModel(appModel);

            var (ProjectPath, ProjecName) = await AssertGenerationOneByOneAsync(itemName, context, itemId, false);

            AssertBuildProject(ProjectPath, ProjecName, platform);
        }
    }
}
