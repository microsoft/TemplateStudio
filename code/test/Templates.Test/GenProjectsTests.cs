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
    [Collection("GenerationCollection")]
    [Trait("ExecutionSet", "Generation")]
    public class GenProjectTests : BaseGenAndBuildTests
    {
        public GenProjectTests(GenerationFixture fixture)
        {
            _fixture = fixture;
        }

        [Theory]
        [MemberData("GetProjectTemplatesAsync")]
        [Trait("Type", "GenerationProjects")]
        public async Task GenEmptyProjectAsync(string projectType, string framework, string language)
        {
            Func<ITemplateInfo, bool> selector =
                t => t.GetTemplateType() == TemplateType.Project
                    && t.GetProjectTypeList().Contains(projectType)
                    && t.GetFrameworkList().Contains(framework)
                    && !t.GetIsHidden()
                    && t.GetLanguage() == language;

            var projectName = $"{projectType}{framework}";

            await AssertGenerateProjectAsync(selector, projectName, projectType, framework, language);
        }

        [Theory]
        [MemberData("GetProjectTemplatesAsync")]
        [Trait("Type", "GenerationAllPagesAndFeatures")]
        public async Task GenAllPagesAndFeaturesAsync(string projectType, string framework, string language)
        {
            Func<ITemplateInfo, bool> selector =
                t => t.GetTemplateType() == TemplateType.Project
                    && t.GetProjectTypeList().Contains(projectType)
                    && t.GetFrameworkList().Contains(framework);

            var projectName = $"{projectType}{framework}All";

            await AssertGenerateProjectAsync(selector, projectName, projectType, framework, language, GenerationFixture.GetDefaultName);
        }

        [Theory]
        [MemberData("GetProjectTemplatesAsync")]
        [Trait("Type", "GenerationRandomNames")]
        public async Task GenAllPagesAndFeaturesRandomNamesAsync(string projectType, string framework, string language)
        {
            Func<ITemplateInfo, bool> selector =
                t => t.GetTemplateType() == TemplateType.Project
                    && t.GetProjectTypeList().Contains(projectType)
                    && t.GetFrameworkList().Contains(framework)
                    && !t.GetIsHidden()
                    && t.GetLanguage() == language;

            var projectName = $"{projectType}{framework}AllRandom";

            await AssertGenerateProjectAsync(selector, projectName, projectType, framework, language, GenerationFixture.GetRandomName);
        }

        [Theory]
        [MemberData("GetProjectTemplatesAsync")]
        [Trait("Type", "GenerationRightClick")]
        public async Task GenProjectWithAllRightClickItemsAsync(string projectType, string framework, string language)
        {
            var projectName = $"{projectType}{framework}AllRandom";
            var projectPath = Path.Combine(_fixture.TestProjectsPath, projectName, projectName);

            await AssertGenerateRightClickAsync(projectName, projectType, framework, language);
        }

        [Theory]
        [MemberData("GetPageAndFeatureTemplatesAsync", "MVVMLight")]
        [Trait("Type", "GenerationOneByOneMVVMLight")]
        public async Task GenMVVMLightOneByOneItemsAsync(string itemName, string projectType, string framework, string itemId, string language)
        {
            await AssertGenerationOneByOneAsync(itemName, projectType, framework, itemId, language);
        }

        [Theory]
        [MemberData("GetPageAndFeatureTemplatesAsync", "MVVMBasic")]
        [Trait("Type", "GenerationOneByOneMVVMBasic")]
        public async Task GenMVVMBasicOneByOneItemsAsync(string itemName, string projectType, string framework, string itemId, string language)
        {
            await AssertGenerationOneByOneAsync(itemName, projectType, framework, itemId, language);
        }

        [Theory]
        [MemberData("GetPageAndFeatureTemplatesAsync", "CodeBehind")]
        [Trait("Type", "GenerationOneByOneCodeBehind")]
        public async Task GenCodeBehindOneByOneItemsAsync(string itemName, string projectType, string framework, string itemId, string language)
        {
            await AssertGenerationOneByOneAsync(itemName, projectType, framework, itemId, language);
        }
    }
}
