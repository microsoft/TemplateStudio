// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Threading.Tasks;

using Microsoft.TemplateEngine.Abstractions;
using Microsoft.Templates.Core;
using Microsoft.Templates.Core.Extensions;
using Microsoft.Templates.Core.Gen;
using TemplateStudioForWinUICpp.Tests;
using Xunit;

namespace Microsoft.Templates.Test.WinUICpp.Build
{
    [Trait("Group", "TS4WinUICPP")]
    [Collection(nameof(WinUICppBuildTemplatesTestCollection))]
    public class BuildNoneProjectTests : WinUICppBaseGenAndBuildTests
    {
        public BuildNoneProjectTests(WinUICppBuildTemplatesTestFixture fixture)
            : base(fixture, null, Frameworks.None)
        {
        }

        [Theory]
        [MemberData(nameof(WinUICppBaseGenAndBuildTests.GetProjectTemplatesForBuild), Frameworks.None, ProgrammingLanguages.Cpp, Platforms.WinUI)]
        [Trait("Group", "MinimumWinUICPP")]
        [Trait("Type", "BuildProjects")]
        public async Task Build_EmptyProject_NoneCppAsync(string projectType, string framework, string platform, string language, string appModel)
        {
            var context = new UserSelectionContext(language, platform)
            {
                ProjectType = projectType,
                FrontEndFramework = framework,
            };
            context.AddAppModel(appModel);

            var (projectName, projectPath) = await GenerateEmptyProjectAsync(context);

            // Don't delete after build test as used in inference test, which will then delete.
            AssertBuildProject(projectPath, projectName, platform, language, deleteAfterBuild: false);

            EnsureCanInferConfigInfo(context, projectPath);
        }

        [Theory]
        [MemberData(nameof(WinUICppBaseGenAndBuildTests.GetProjectTemplatesForBuild), Frameworks.None, ProgrammingLanguages.Cpp, Platforms.WinUI)]
        [Trait("Type", "BuildAllPagesAndFeatures")]
        [Trait("Type", "BuildRandomNames")]
        public async Task Build_All_ProjectNameValidation_WinUICppAsync(string projectType, string framework, string platform, string language, string appModel)
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

            AssertBuildProject(projectPath, projectName, platform, language);
        }
    }
}
