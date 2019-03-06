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
    [Collection("StyleCopCollection")]
    [Trait("ExecutionSet", "Minimum")]
    [Trait("ExecutionSet", "BuildStyleCop")]
    public class StyleCopProjectGenerationTests : BaseGenAndBuildTests
    {
        public StyleCopProjectGenerationTests(StyleCopGenerationTestsFixture fixture)
            : base(fixture)
        {
        }

        [Theory]
        [MemberData(nameof(GetProjectTemplatesForStyleCop))]
        [Trait("Type", "CodeStyle")]
        public async Task GenerateAllPagesAndFeaturesAndCheckWithStyleCopAsync(string projectType, string framework, string platform)
        {
            Func<ITemplateInfo, bool> selector =
                    t => t.GetTemplateType() == TemplateType.Project
                    && t.GetLanguage() == ProgrammingLanguages.CSharp
                    && t.GetPlatform() == platform
                    && t.GetProjectTypeList().Contains(projectType)
                    && t.GetFrontEndFrameworkList().Contains(framework);

            Func<ITemplateInfo, bool> templateSelector =
                t => ((t.GetTemplateType() == TemplateType.Page || t.GetTemplateType() == TemplateType.Feature)
                        && t.GetFrontEndFrameworkList().Contains(framework)
                        && t.GetPlatform() == platform
                        && !t.GetIsHidden())
                     || (t.Name == "Feature.Testing.StyleCop");

            var projectName = $"{projectType}{framework}AllStyleCop";

            var projectPath = await AssertGenerateProjectAsync(selector, projectName, projectType, framework, platform, ProgrammingLanguages.CSharp, templateSelector, BaseGenAndBuildFixture.GetDefaultName, false);

            AssertBuildProjectAsync(projectPath, projectName, platform);
        }

        public static IEnumerable<object[]> GetProjectTemplatesForStyleCop()
        {
            return StyleCopGenerationTestsFixture.GetProjectTemplatesForStyleCop();
        }
    }
}
