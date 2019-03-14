// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.TemplateEngine.Abstractions;
using Microsoft.Templates.Core;
using Microsoft.Templates.Fakes;
using Xunit;

namespace Microsoft.Templates.Test
{
    [Collection("SonarLintCollection")]
    [Trait("ExecutionSet", "BuildVBStyle")]
    public class SonarLintProjectGenerationTests : BaseGenAndBuildTests
    {
        public SonarLintProjectGenerationTests(SonarLintGenerationTestsFixture fixture)
            : base(fixture)
        {
        }

        [Theory]
        [MemberData(nameof(GetProjectTemplatesForSonarLint))]
        [Trait("Type", "CodeStyle")]
        public async Task GenerateAllPagesAndFeaturesAndCheckWithSonarLintAsync(string projectType, string framework, string platform)
        {

            Func<ITemplateInfo, bool> templateSelector =
                    t => ((t.GetTemplateType() == TemplateType.Page || t.GetTemplateType() == TemplateType.Feature)
                    && (t.GetProjectTypeList().Contains(projectType) || t.GetProjectTypeList().Contains(All))
                    && t.GetFrontEndFrameworkList().Contains(framework)
                    && t.GetPlatform() == platform
                    && !t.GetIsHidden())
                    || (t.Name == "Feature.Testing.SonarLint.VB");

            var projectName = $"{projectType}{framework}AllSonarLint";

            var projectPath = await AssertGenerateProjectAsync(projectName, projectType, framework, platform, ProgrammingLanguages.VisualBasic, templateSelector, BaseGenAndBuildFixture.GetDefaultName);

            AssertBuildProjectAsync(projectPath, projectName, platform);
        }

        public static IEnumerable<object[]> GetProjectTemplatesForSonarLint()
        {
            return SonarLintGenerationTestsFixture.GetProjectTemplatesForSonarLint();
        }
    }
}
