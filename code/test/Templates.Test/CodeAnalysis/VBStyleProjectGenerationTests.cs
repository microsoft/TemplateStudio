// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.TemplateEngine.Abstractions;
using Microsoft.Templates.Core;
using Microsoft.Templates.Core.Extensions;
using Microsoft.Templates.Fakes;
using Xunit;

namespace Microsoft.Templates.Test
{
    [Collection("VBStyleCollection")]
    [Trait("ExecutionSet", "BuildVBStyle")]
    public class VBStyleProjectGenerationTests : BaseGenAndBuildTests
    {
        public VBStyleProjectGenerationTests(VBStyleGenerationTestsFixture fixture)
            : base(fixture)
        {
        }

        [Theory]
        [MemberData(nameof(GetProjectTemplatesForVBStyle))]
        [Trait("Type", "CodeStyle")]
        public async Task GenerateAllPagesAndFeaturesForcedLoginAndCheckWithVBStyleAsync(string projectType, string framework, string platform)
        {
            Func<ITemplateInfo, bool> templateSelector =
                t => t.GetTemplateType().IsItemTemplate()
                && (t.GetProjectTypeList().Contains(projectType) || t.GetProjectTypeList().Contains(All))
                && t.GetFrontEndFrameworkList().Contains(framework)
                && t.GetPlatform() == platform
                && !t.GetIsHidden()
                && t.GroupIdentity != "wts.Feat.IdentityOptionalLogin.VB"
                || (t.Name == "Feature.Testing.VBStyleAnalysis");

            var projectName = $"{projectType}{framework}AllVBStyle";

            var projectPath = await AssertGenerateProjectAsync(projectName, projectType, framework, platform, ProgrammingLanguages.VisualBasic, templateSelector, BaseGenAndBuildFixture.GetDefaultName);

            AssertBuildProjectAsync(projectPath, projectName, platform);
        }

        [Theory]
        [MemberData(nameof(GetProjectTemplatesForVBStyle))]
        [Trait("Type", "CodeStyle")]
        public async Task GenerateAllPagesAndFeaturesOptionalLoginAndCheckWithVBStyleAsync(string projectType, string framework, string platform)
        {
            Func<ITemplateInfo, bool> templateSelector =
                t => t.GetTemplateType().IsItemTemplate()
                && (t.GetProjectTypeList().Contains(projectType) || t.GetProjectTypeList().Contains(All))
                && t.GetFrontEndFrameworkList().Contains(framework)
                && t.GetPlatform() == platform
                && !t.GetIsHidden()
                && t.GroupIdentity != "wts.Feat.IdentityForcedLogin.VB"
                || (t.Name == "Feature.Testing.VBStyleAnalysis");

            var projectName = $"{projectType}{framework}AllVBStyle";

            var projectPath = await AssertGenerateProjectAsync(projectName, projectType, framework, platform, ProgrammingLanguages.VisualBasic, templateSelector, BaseGenAndBuildFixture.GetDefaultName);

            AssertBuildProjectAsync(projectPath, projectName, platform);
        }

        public static IEnumerable<object[]> GetProjectTemplatesForVBStyle()
        {
            return VBStyleGenerationTestsFixture.GetProjectTemplatesForVBStyle();
        }
    }
}
