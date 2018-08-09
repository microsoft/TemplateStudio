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
using Microsoft.Templates.Core.Gen;
using Microsoft.Templates.Fakes;
using Microsoft.Templates.UI;
using Xunit;
using Microsoft.VisualStudio.Threading;

namespace Microsoft.Templates.Test
{
    [Collection("StyleCopCollection")]
    [Trait("ExecutionSet", "Minimum")]
    [Trait("ExecutionSet", "BuildStyleCop")]
    public class StyleCopProjectGenerationTests : BaseGenAndBuildTests
    {
        public StyleCopProjectGenerationTests(StyleCopGenerationTestsFixture fixture)
        {
            _fixture = fixture;
            _fixture.InitializeFixture(this);
        }

        [Theory]
        [MemberData("GetProjectTemplatesForStyleCop")]
        [Trait("Type", "CodeStyle")]
        public async Task GenerateAllPagesAndFeaturesAndCheckWithStyleCopAsync(string projectType, string framework, string platform)
        {
            Func<ITemplateInfo, bool> selector =
                    t => t.GetTemplateType() == TemplateType.Project
                    && t.GetLanguage() == ProgrammingLanguages.CSharp
                    && t.GetPlatform() == platform
                    && t.GetProjectTypeList().Contains(projectType)
                    && t.GetFrameworkList().Contains(framework);

            Func<ITemplateInfo, bool> templateSelector =
                t => ((t.GetTemplateType() == TemplateType.Page || t.GetTemplateType() == TemplateType.Feature)
                        && t.GetFrameworkList().Contains(framework)
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
