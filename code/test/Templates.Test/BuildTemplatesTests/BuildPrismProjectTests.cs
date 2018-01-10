// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.Templates.Core;
using Microsoft.Templates.Core.Gen;
using Microsoft.Templates.Core.Locations;
using Microsoft.Templates.Fakes;
using Microsoft.Templates.UI;
using Microsoft.VisualStudio.Threading;
using Microsoft.TemplateEngine.Abstractions;

using Xunit;

namespace Microsoft.Templates.Test
{
    [Collection("BuildTemplateTestCollection")]
    [Trait("ExecutionSet", "BuildPrism")]
    [Trait("ExecutionSet", "Build")]
    public class BuildPrismProjectTests : BaseGenAndBuildTests
    {
        public BuildPrismProjectTests(BuildTemplatesTestFixture fixture)
        {
            _fixture = fixture;
            _fixture.InitializeFixtureAsync(this, "Prism");
        }

        [Theory]
        [MemberData("GetProjectTemplatesForBuildAsync", "Prism", ProgrammingLanguages.CSharp)]
        [Trait("Type", "BuildProjects")]
        public async Task BuildEmptyProjectAsync(string projectType, string framework, string language)
        {
            Func<ITemplateInfo, bool> selector =
                t => t.GetTemplateType() == TemplateType.Project
                    && t.GetProjectTypeList().Contains(projectType)
                    && t.GetFrameworkList().Contains(framework)
                    && !t.GetIsHidden()
                    && t.GetLanguage() == language;

            var projectName = $"{projectType}";

            var projectPath = await AssertGenerateProjectAsync(selector, projectName, projectType, framework, language, null, false);

            AssertBuildProjectAsync(projectPath, projectName);
        }

        [Theory]
        [MemberData("GetProjectTemplatesForBuildAsync", "Prism", ProgrammingLanguages.CSharp)]
        [Trait("Type", "BuildAllPagesAndFeatures")]
        public async Task BuildAllPagesAndFeaturesAsync(string projectType, string framework, string language)
        {
            Func<ITemplateInfo, bool> selector =
                t => t.GetTemplateType() == TemplateType.Project
                    && t.GetProjectTypeList().Contains(projectType)
                    && t.GetFrameworkList().Contains(framework)
                    && !t.GetIsHidden()
                    && t.GetLanguage() == language;

            var projectName = $"{projectType}All";

            var projectPath = await AssertGenerateProjectAsync(selector, projectName, projectType, framework, language, GenerationFixture.GetDefaultName, false);

            AssertBuildProjectAsync(projectPath, projectName);
        }

        [Theory]
        [MemberData("GetProjectTemplatesForBuildAsync", "Prism", ProgrammingLanguages.CSharp)]
        [Trait("Type", "BuildRandomNames")]
        [Trait("ExecutionSet", "Minimum")]
        [Trait("ExecutionSet", "BuildMinimum")]
        public async Task BuildAllPagesAndFeaturesRandomNamesAsync(string projectType, string framework, string language)
        {
            Func<ITemplateInfo, bool> selector =
                t => t.GetTemplateType() == TemplateType.Project
                    && t.GetProjectTypeList().Contains(projectType)
                    && t.GetFrameworkList().Contains(framework)
                    && !t.GetIsHidden()
                    && t.GetLanguage() == language;

            var projectName = $"{projectType}AllRandom";

            var projectPath = await AssertGenerateProjectAsync(selector, projectName, projectType, framework, language, GenerationFixture.GetRandomName, false);

            AssertBuildProjectAsync(projectPath, projectName);
        }

        [Theory]
        [MemberData("GetProjectTemplatesForBuildAsync", "Prism", ProgrammingLanguages.CSharp)]
        [Trait("Type", "BuildRightClick")]
        public async Task BuildEmptyProjectWithAllRightClickItemsAsync(string projectType, string framework, string language)
        {
            var projectName = $"{projectType}AllRightClick";

            var projectPath = await AssertGenerateRightClickAsync(projectName, projectType, framework, language, true, false);

            AssertBuildProjectAsync(projectPath, projectName);
        }

        [Theory]
        [MemberData("GetProjectTemplatesForBuildAsync", "Prism", ProgrammingLanguages.CSharp)]
        [Trait("Type", "BuildRightClick")]
        public async Task BuildCompleteProjectWithAllRightClickItemsAsync(string projectType, string framework, string language)
        {
            var projectName = $"{projectType}AllRightClick2";

            var projectPath = await AssertGenerateRightClickAsync(projectName, projectType, framework, language, false, false);

            AssertBuildProjectAsync(projectPath, projectName);
        }

        [Theory]
        [MemberData("GetPageAndFeatureTemplatesForBuildAsync", "Prism")]
        [Trait("Type", "BuildOneByOnePrism")]
        public async Task BuildPrismOneByOneItemsAsync(string itemName, string projectType, string framework, string itemId, string language)
        {
            var result = await AssertGenerationOneByOneAsync(itemName, projectType, framework, itemId, language, false);

            AssertBuildProjectAsync(result.ProjectPath, result.ProjecName);
        }
    }
}
