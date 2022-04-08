﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Threading.Tasks;

using Microsoft.TemplateEngine.Abstractions;
using Microsoft.Templates.Core;
using Microsoft.Templates.Core.Extensions;
using Microsoft.Templates.Core.Gen;
using TemplateStudioForWPF.Tests;
using Xunit;

namespace Microsoft.Templates.Test.WPF.Build
{
    [Trait("Group", "BuildWPF")]
    [Collection(nameof(WpfBuildTemplatesTestCollection))]
    public class BuildCodeBehindProjectTests : WpfBaseGenAndBuildTests
    {
        public BuildCodeBehindProjectTests(WpfBuildTemplatesTestFixture fixture)
            : base(fixture, null, Frameworks.CodeBehind)
        {
        }

        [Theory]
        [MemberData(nameof(WpfBaseGenAndBuildTests.GetProjectTemplatesForBuild), Frameworks.CodeBehind, "", Platforms.Wpf)]
        public async Task Build_EmptyProject_WpfAsync(string projectType, string framework, string platform, string language)
        {
            var context = new UserSelectionContext(language, platform)
            {
                ProjectType = projectType,
                FrontEndFramework = framework
            };

            var (projectName, projectPath) = await GenerateEmptyProjectAsync(context);

            AssertBuildProject(projectPath, projectName, platform, deleteAfterBuild: true);
        }

        [Theory]
        [MemberData(nameof(WpfBaseGenAndBuildTests.GetProjectTemplatesForBuild), Frameworks.CodeBehind, "", Platforms.Wpf)]
        public async Task Build_All_ProjectNameValidation_G1_WpfAsync(string projectType, string framework, string platform, string language)
        {
            bool templateSelector(ITemplateInfo t) => t.GetTemplateType().IsItemTemplate()
                && (t.GetProjectTypeList().Contains(projectType) || t.GetProjectTypeList().Contains(All))
                && (t.GetFrontEndFrameworkList().Contains(framework) || t.GetFrontEndFrameworkList().Contains(All))
                && t.GetPlatform() == platform
                && !excludedTemplates_Wpf_Group2.Contains(t.GroupIdentity)
                && t.Identity != "ts.WPF.Feat.MSIXPackaging"
                && !t.GetIsHidden();

            var projectName = $"{ShortProjectType(projectType)}{CharactersThatMayCauseProjectNameIssues()}G1{ShortLanguageName(language)}";

            var context = new UserSelectionContext(language, platform)
            {
                ProjectType = projectType,
                FrontEndFramework = framework
            };

            var projectPath = await AssertGenerateProjectAsync(projectName, context, templateSelector, BaseGenAndBuildFixture.GetRandomName);

            AssertBuildProject(projectPath, projectName, platform);
        }

        [Theory]
        [MemberData(nameof(WpfBaseGenAndBuildTests.GetProjectTemplatesForBuild), Frameworks.CodeBehind, "", Platforms.Wpf)]
        public async Task Build_All_ProjectNameValidation_G2_WpfAsync(string projectType, string framework, string platform, string language)
        {
            bool templateSelector(ITemplateInfo t) => t.GetTemplateType().IsItemTemplate()
                && (t.GetProjectTypeList().Contains(projectType) || t.GetProjectTypeList().Contains(All))
                && (t.GetFrontEndFrameworkList().Contains(framework) || t.GetFrontEndFrameworkList().Contains(All))
                && t.GetPlatform() == platform
                 && !excludedTemplates_Wpf_Group1.Contains(t.GroupIdentity)
                && t.Identity != "ts.WPF.Feat.MSIXPackaging"
                && !t.GetIsHidden();

            var projectName = $"{ShortProjectType(projectType)}{CharactersThatMayCauseProjectNameIssues()}G1{ShortLanguageName(language)}";

            var context = new UserSelectionContext(language, platform)
            {
                ProjectType = projectType,
                FrontEndFramework = framework
            };

            var projectPath = await AssertGenerateProjectAsync(projectName, context, templateSelector, BaseGenAndBuildFixture.GetRandomName);

            AssertBuildProject(projectPath, projectName, platform);
        }

        [Theory]
        [MemberData(nameof(WpfBaseGenAndBuildTests.GetProjectTemplatesForBuild), Frameworks.CodeBehind, ProgrammingLanguages.CSharp, Platforms.Wpf)]
        [Trait("Group", "MinimumWPF")]
        public async Task Build_All_CheckWithStyleCop_G1_WpfAsync(string projectType, string framework, string platform, string language)
        {
            bool templateSelector(ITemplateInfo t) => t.GetTemplateType().IsItemTemplate()
                && (t.GetProjectTypeList().Contains(projectType) || t.GetProjectTypeList().Contains(All))
                && (t.GetFrontEndFrameworkList().Contains(framework) || t.GetFrontEndFrameworkList().Contains(All))
                && t.GetPlatform() == platform
                && !excludedTemplates_Wpf_Group2.Contains(t.GroupIdentity)
                && !t.GetIsHidden()
                && t.Identity != "ts.WPF.Feat.MSIXPackaging"
                || t.Identity == "ts.WPF.Feat.StyleCop";

            var projectName = $"{projectType}{framework}AllG1";

            var context = new UserSelectionContext(language, platform)
            {
                ProjectType = projectType,
                FrontEndFramework = framework
            };

            var styleCopTemplates = GetAdditionalTemplates(System.IO.Path.Combine(Environment.CurrentDirectory, @"..\..\TestData\WPF"));
            GenContext.ToolBox.Repo.AddAdditionalTemplates(styleCopTemplates);

            var projectPath = await AssertGenerateProjectAsync(projectName, context, templateSelector, BaseGenAndBuildFixture.GetDefaultName, includeMultipleInstances: false, styleCopTemplates);

            AssertBuildProject(projectPath, projectName, platform);
        }

        [Theory]
        [MemberData(nameof(WpfBaseGenAndBuildTests.GetProjectTemplatesForBuild), Frameworks.CodeBehind, ProgrammingLanguages.CSharp, Platforms.Wpf)]
        [Trait("Group", "MinimumWPF")]
        public async Task Build_All_CheckWithStyleCop_G2_WpfAsync(string projectType, string framework, string platform, string language)
        {
            bool templateSelector(ITemplateInfo t) => t.GetTemplateType().IsItemTemplate()
                && (t.GetProjectTypeList().Contains(projectType) || t.GetProjectTypeList().Contains(All))
                && (t.GetFrontEndFrameworkList().Contains(framework) || t.GetFrontEndFrameworkList().Contains(All))
                && t.GetPlatform() == platform
                && !excludedTemplates_Wpf_Group1.Contains(t.GroupIdentity)
                && !t.GetIsHidden()
                && t.Identity != "ts.WPF.Feat.MSIXPackaging"
                || t.Identity == "ts.WPF.Feat.StyleCop";

            var projectName = $"{projectType}{framework}AllG2";

            var context = new UserSelectionContext(language, platform)
            {
                ProjectType = projectType,
                FrontEndFramework = framework
            };

            var styleCopTemplates = GetAdditionalTemplates(System.IO.Path.Combine(Environment.CurrentDirectory, @"..\..\TestData\WPF"));
            GenContext.ToolBox.Repo.AddAdditionalTemplates(styleCopTemplates);

            var projectPath = await AssertGenerateProjectAsync(projectName, context, templateSelector, BaseGenAndBuildFixture.GetDefaultName, includeMultipleInstances: true, styleCopTemplates);

            AssertBuildProject(projectPath, projectName, platform);
        }

        [Theory]
        [MemberData(nameof(WpfBaseGenAndBuildTests.GetProjectTemplatesForBuild), Frameworks.CodeBehind, ProgrammingLanguages.CSharp, Platforms.Wpf)]
        public async Task Build_AllWithMsix_G1_WpfAsync(string projectType, string framework, string platform, string language)
        {
            bool templateSelector(ITemplateInfo t) => t.GetTemplateType().IsItemTemplate()
                && (t.GetProjectTypeList().Contains(projectType) || t.GetProjectTypeList().Contains(All))
                && (t.GetFrontEndFrameworkList().Contains(framework) || t.GetFrontEndFrameworkList().Contains(All))
                && t.GetPlatform() == platform
                && !excludedTemplates_Wpf_Group2.Contains(t.GroupIdentity)
                && !t.GetIsHidden()
                || t.Identity == "ts.WPF.Feat.StyleCop";

            var projectName = $"{projectType}{framework}AllMsix";

            var context = new UserSelectionContext(language, platform)
            {
                ProjectType = projectType,
                FrontEndFramework = framework
            };

            var projectPath = await AssertGenerateProjectAsync(projectName, context, templateSelector, BaseGenAndBuildFixture.GetDefaultName);

            AssertBuildProjectWpfWithMsix(projectPath, projectName, platform);
        }

        [Theory]
        [MemberData(nameof(WpfBaseGenAndBuildTests.GetProjectTemplatesForBuild), Frameworks.CodeBehind, ProgrammingLanguages.CSharp, Platforms.Wpf)]
        public async Task Build_AllWithMsix_G2_WpfAsync(string projectType, string framework, string platform, string language)
        {
            bool templateSelector(ITemplateInfo t) => t.GetTemplateType().IsItemTemplate()
                && (t.GetProjectTypeList().Contains(projectType) || t.GetProjectTypeList().Contains(All))
                && (t.GetFrontEndFrameworkList().Contains(framework) || t.GetFrontEndFrameworkList().Contains(All))
                && t.GetPlatform() == platform
                && !excludedTemplates_Wpf_Group1.Contains(t.GroupIdentity)
                && !t.GetIsHidden()
                || t.Identity == "ts.WPF.Feat.StyleCop";

            var projectName = $"{projectType}{framework}AllMsix";

            var context = new UserSelectionContext(language, platform)
            {
                ProjectType = projectType,
                FrontEndFramework = framework
            };

            var projectPath = await AssertGenerateProjectAsync(projectName, context, templateSelector, BaseGenAndBuildFixture.GetDefaultName);

            AssertBuildProjectWpfWithMsix(projectPath, projectName, platform);
        }

        [Theory]
        [MemberData(nameof(WpfBaseGenAndBuildTests.GetProjectTemplatesForBuild), Frameworks.CodeBehind, "", Platforms.Wpf)]
        public async Task Build_Empty_AddRightClick_WpfAsync(string projectType, string framework, string platform, string language)
        {
            var projectName = $"{ShortProjectType(projectType)}AllRC{ShortLanguageName(language)}";

            var context = new UserSelectionContext(language, platform)
            {
                ProjectType = projectType,
                FrontEndFramework = framework
            };

            var projectPath = await AssertGenerateRightClickAsync(projectName, context, true);

            AssertBuildProject(projectPath, projectName, platform);
        }

        [Theory]
        [MemberData(nameof(WpfBaseGenAndBuildTests.GetPageAndFeatureTemplatesForBuild), Frameworks.CodeBehind, ProgrammingLanguages.CSharp, Platforms.Wpf, "ts.WPF.Feat.MSIXPackaging")]
        public async Task Build_CodeBehind_OneByOneItems_WpfAsync(string itemName, string projectType, string framework, string platform, string itemId, string language)
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
