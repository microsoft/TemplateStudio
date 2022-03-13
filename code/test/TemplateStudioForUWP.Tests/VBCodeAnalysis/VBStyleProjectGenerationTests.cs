// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.TemplateEngine.Abstractions;
using Microsoft.Templates.Core;
using Microsoft.Templates.Core.Extensions;
using Microsoft.Templates.Core.Gen;
using Xunit;

namespace Microsoft.Templates.Test.Build.Uwp
{
    [Collection("VBStyleCollection")]
    [Trait("ExecutionSet", "BuildVBStyle")]
    [Trait("ExecutionSet", "_Full")]
    public class VBStyleProjectGenerationTests : BaseGenAndBuildTests
    {
        public VBStyleProjectGenerationTests(VBStyleGenerationTestsFixture fixture)
            : base(fixture)
        {
        }

        [Theory]
        [MemberData(nameof(GetProjectTemplatesForVBStyle))]
        [Trait("Type", "CodeStyle")]
        public async Task GenerateAllWithOptionalLoginRunTestsAndCheckWithVBStyleAsync(string projectType, string framework, string platform)
        {
            bool templateSelector(ITemplateInfo t) => t.GetTemplateType().IsItemTemplate()
                && (t.GetProjectTypeList().Contains(projectType) || t.GetProjectTypeList().Contains(All))
                && (t.GetFrontEndFrameworkList().Contains(framework) || t.GetFrontEndFrameworkList().Contains(All))
                && t.GetPlatform() == platform
                && !t.GetIsHidden()
                && !excludedTemplatesGroup2VB.Contains(t.GroupIdentity)
                || (t.Identity == "wts.Feat.VBStyleAnalysis");

            var projectName = $"{projectType}{framework}AllVBStyleG1";

            var context = new UserSelectionContext(ProgrammingLanguages.VisualBasic, platform)
            {
                ProjectType = projectType,
                FrontEndFramework = framework
            };

            var styleTemplates = GetAdditionalTemplates(System.IO.Path.Combine(Environment.CurrentDirectory, @"..\..\TestData\UWP")).Where(t => t.GetLanguage() == ProgrammingLanguages.VisualBasic);
            GenContext.ToolBox.Repo.AddAdditionalTemplates(styleTemplates);

            var projectPath = await AssertGenerateProjectAsync(projectName, context, templateSelector, BaseGenAndBuildFixture.GetDefaultName, includeMultipleInstances: false, styleTemplates);

            AssertBuildProjectThenRunTests(projectPath, projectName, platform);
        }

        [Theory]
        [MemberData(nameof(GetProjectTemplatesForVBStyle))]
        [Trait("Type", "CodeStyle")]
        public async Task GenerateAllWithForcedLoginRunTestsAndCheckWithVBStyleAsync(string projectType, string framework, string platform)
        {
            bool templateSelector(ITemplateInfo t) => t.GetTemplateType().IsItemTemplate()
                && (t.GetProjectTypeList().Contains(projectType) || t.GetProjectTypeList().Contains(All))
                && (t.GetFrontEndFrameworkList().Contains(framework) || t.GetFrontEndFrameworkList().Contains(All))
                && t.GetPlatform() == platform
                && !t.GetIsHidden()
                && !excludedTemplatesGroup1VB.Contains(t.GroupIdentity)
                || (t.Identity == "wts.Feat.VBStyleAnalysis");

            var projectName = $"{projectType}{framework}AllVBStyleG2";

            var context = new UserSelectionContext(ProgrammingLanguages.VisualBasic, platform)
            {
                ProjectType = projectType,
                FrontEndFramework = framework
            };

            var styleTemplates = GetAdditionalTemplates(System.IO.Path.Combine(Environment.CurrentDirectory, @"..\..\TestData\UWP")).Where(t => t.GetLanguage() == ProgrammingLanguages.VisualBasic);
            GenContext.ToolBox.Repo.AddAdditionalTemplates(styleTemplates);

            var projectPath = await AssertGenerateProjectAsync(projectName, context, templateSelector, BaseGenAndBuildFixture.GetDefaultName, includeMultipleInstances: true, styleTemplates);
            AssertBuildProjectThenRunTests(projectPath, projectName, platform);
        }

        public static IEnumerable<object[]> GetProjectTemplatesForVBStyle()
        {
            return VBStyleGenerationTestsFixture.GetProjectTemplatesForVBStyle();
        }
    }
}
