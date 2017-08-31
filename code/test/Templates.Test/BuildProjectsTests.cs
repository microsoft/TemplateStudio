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
    [Collection("BuildCollection")]
    [Trait("ExecutionSet", "Build")]
    public class BuildProjectTests : BaseGenAndBuildTests
    {
        public BuildProjectTests(GenerationFixture fixture)
        {
            _fixture = fixture;
        }

        [Theory]
        [MemberData("GetProjectTemplatesAsync")]
        [Trait("Type", "BuildProjects")]
        public async Task BuildEmptyProjectAsync(string projectType, string framework, string language)
        {
            Func<ITemplateInfo, bool> selector =
                t => t.GetTemplateType() == TemplateType.Project
                    && t.GetProjectTypeList().Contains(projectType)
                    && t.GetFrameworkList().Contains(framework)
                    && !t.GetIsHidden()
                    && t.GetLanguage() == language;

            var projectName = $"{projectType}{framework}";

            await AssertGenerateProjectAsync(selector, projectName, projectType, framework, language, null, false);

            AssertBuildProjectAsync(projectName);
        }

        [Theory]
        [MemberData("GetProjectTemplatesAsync")]
        [Trait("Type", "BuildAllPagesAndFeatures")]
        public async Task BuildAllPagesAndFeaturesAsync(string projectType, string framework, string language)
        {
            Func<ITemplateInfo, bool> selector =
                t => t.GetTemplateType() == TemplateType.Project
                    && t.GetProjectTypeList().Contains(projectType)
                    && t.GetFrameworkList().Contains(framework);

            var projectName = $"{projectType}{framework}All";

            await AssertGenerateProjectAsync(selector, projectName, projectType, framework, language, null, false);

            AssertBuildProjectAsync(projectName);
        }

        [Theory]
        [MemberData("GetProjectTemplatesAsync")]
        [Trait("Type", "BuildRandomNames")]
        public async Task BuildAllPagesAndFeaturesRandomNamesAsync(string projectType, string framework, string language)
        {
            Func<ITemplateInfo, bool> selector =
                t => t.GetTemplateType() == TemplateType.Project
                    && t.GetProjectTypeList().Contains(projectType)
                    && t.GetFrameworkList().Contains(framework)
                    && !t.GetIsHidden()
                    && t.GetLanguage() == language;

            var projectName = $"{projectType}{framework}AllRandom";

            await AssertGenerateProjectAsync(selector, projectName, projectType, framework, language, GenerationFixture.GetRandomName, false);

            AssertBuildProjectAsync(projectName);
        }

        [Theory]
        [MemberData("GetProjectTemplatesAsync")]
        [Trait("Type", "BuildRightClick")]
        public async Task BuildProjectWithAllRightClickItemsAsync(string projectType, string framework, string language)
        {
            var projectName = $"{projectType}{framework}AllRandom";
            var projectPath = Path.Combine(_fixture.TestProjectsPath, projectName, projectName);

            await AssertGenerateRightClickAsync(projectName, projectType, framework, language, false);

            AssertBuildProjectAsync(projectName);
        }

        [Theory]
        [MemberData("GetPageAndFeatureTemplatesAsync", "MVVMLight")]
        [Trait("Type", "BuildOneByOneMVVMLight")]
        public async Task GenMVVMLightOneByOneItemsAsync(string itemName, string projectType, string framework, string itemId, string language)
        {
            var projectName = await AssertGenerationOneByOneAsync(itemName, projectType, framework, itemId, language, false);

            AssertBuildProjectAsync(projectName);
        }

        [Theory]
        [MemberData("GetPageAndFeatureTemplatesAsync", "MVVMBasic")]
        [Trait("Type", "BuildOneByOneMVVMBasic")]
        public async Task GenMVVMBasicOneByOneItemsAsync(string itemName, string projectType, string framework, string itemId, string language)
        {
            var projectName = await AssertGenerationOneByOneAsync(itemName, projectType, framework, itemId, language, false);

            AssertBuildProjectAsync(projectName);
        }

        [Theory]
        [MemberData("GetPageAndFeatureTemplatesAsync", "CodeBehind")]
        [Trait("Type", "BuildOneByOneCodeBehind")]
        public async Task GenCodeBehindOneByOneItemsAsync(string itemName, string projectType, string framework, string itemId, string language)
        {
            var projectName = await AssertGenerationOneByOneAsync(itemName, projectType, framework, itemId, language, false);

            AssertBuildProjectAsync(projectName);
        }
    }
}
