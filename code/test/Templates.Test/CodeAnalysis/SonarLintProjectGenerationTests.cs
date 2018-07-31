// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.TemplateEngine.Abstractions;
using Microsoft.Templates.Core;
using Microsoft.Templates.UI;
using Xunit;
using Microsoft.VisualStudio.Threading;

namespace Microsoft.Templates.Test
{
    [Collection("SonarLintCollection")]
    [Trait("ExecutionSet", "BuildVBStyle")]
    public class SonarLintProjectGenerationTests : BaseGenAndBuildTests
    {
        public SonarLintProjectGenerationTests(SonarLintGenerationTestsFixture fixture)
        {
            _fixture = fixture;
            _fixture.InitializeFixture(this);
        }

        [Theory]
        [MemberData("GetProjectTemplatesForSonarLint")]
        [Trait("Type", "CodeStyle")]
        public async Task GenerateAllPagesAndFeaturesAndCheckWithSonarLintAsync(string projectType, string framework, string platform)
        {
            Func<ITemplateInfo, bool> selector =
                    t => t.GetTemplateType() == TemplateType.Project
                    && t.GetLanguage() == ProgrammingLanguages.VisualBasic
                    && t.GetPlatform() == platform
                    && t.GetProjectTypeList().Contains(projectType)
                    && t.GetFrameworkList().Contains(framework);

            Func<ITemplateInfo, bool> templateSelector =
                t => ((t.GetTemplateType() == TemplateType.Page || t.GetTemplateType() == TemplateType.Feature)
                        && t.GetFrameworkList().Contains(framework)
                        && t.GetPlatform() == platform
                        && !t.GetIsHidden())
                     || (t.Name == "Feature.Testing.SonarLint.VB");

            var projectName = $"{projectType}{framework}AllSonarLint";

            var projectPath = await AssertGenerateProjectAsync(selector, projectName, projectType, framework, platform, ProgrammingLanguages.VisualBasic, templateSelector, BaseGenAndBuildFixture.GetDefaultName, false);

            AssertBuildProjectAsync(projectPath, projectName, platform);
        }

        public static IEnumerable<object[]> GetProjectTemplatesForSonarLint()
        {
            return SonarLintGenerationTestsFixture.GetProjectTemplatesForSonarLint();
        }
    }
}
