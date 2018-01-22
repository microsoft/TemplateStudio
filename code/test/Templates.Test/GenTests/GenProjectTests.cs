// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Text;
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
            _fixture.InitializeFixtureAsync(this);
        }

        [Theory]
        [MemberData("GetProjectTemplatesForGenerationAsync")]
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
        [MemberData("GetProjectTemplatesForGenerationAsync")]
        [Trait("Type", "GenerationProjects")]
        public async Task GenEmptyProjectCorrectInferProjectConfigInfoAsync(string projectType, string framework, string language)
        {
            Func<ITemplateInfo, bool> selector =
                t => t.GetTemplateType() == TemplateType.Project
                    && t.GetProjectTypeList().Contains(projectType)
                    && t.GetFrameworkList().Contains(framework)
                    && !t.GetIsHidden()
                    && t.GetLanguage() == language;

            var projectName = $"{projectType}{framework}";

            string projectPath = await AssertGenerateProjectAsync(selector, projectName, projectType, framework, language, null, false);

            // Remove configuration from the manifest
            RemoveProjectConfigInfoFromProject();

            // Now the configuration must be inferred and should be as expected
            AssertCorrectProjectConfigInfo(projectType, framework);

            AssertProjectConfigInfoRecreated(projectType, framework);
        }

        private static void AssertProjectConfigInfoRecreated(string projectType, string framework)
        {
            string content = File.ReadAllText(Path.Combine(GenContext.Current.ProjectPath, "Package.appxmanifest"));
            string expectedFxText = $"Name=\"framework\" Value=\"{framework}\"";
            string expectedPtText = $"Name=\"projectType\" Value=\"{projectType}\"";

            Assert.Contains(expectedFxText, content, StringComparison.OrdinalIgnoreCase);
            Assert.Contains(expectedPtText, content, StringComparison.OrdinalIgnoreCase);
        }

        private void RemoveProjectConfigInfoFromProject()
        {
            string manifest = Path.Combine(GenContext.Current.ProjectPath, "Package.appxmanifest");
            var lines = File.ReadAllLines(manifest);
            StringBuilder sb = new StringBuilder();
            string fx = $"genTemplate:Item Name=\"framework\"";
            string pt = $"genTemplate:Item Name=\"projectType\"";
            foreach (var line in lines)
            {
                if (!line.Contains(fx) && !line.Contains(pt))
                {
                    sb.AppendLine(line);
                }
            }

            File.Delete(manifest);
            File.WriteAllText(manifest, sb.ToString());
        }

        [Theory]
        [MemberData("GetProjectTemplatesForGenerationAsync")]
        [Trait("Type", "GenerationAllPagesAndFeatures")]
        public async Task GenAllPagesAndFeaturesAsync(string projectType, string framework, string language)
        {
            Func<ITemplateInfo, bool> selector =
                t => t.GetTemplateType() == TemplateType.Project
                    && t.GetProjectTypeList().Contains(projectType)
                    && t.GetFrameworkList().Contains(framework)
                    && !t.GetIsHidden()
                    && t.GetLanguage() == language;

            var projectName = $"{projectType}{framework}All";

            await AssertGenerateProjectAsync(selector, projectName, projectType, framework, language, BaseGenAndBuildFixture.GetDefaultName);
        }

        [Theory]
        [MemberData("GetProjectTemplatesForGenerationAsync")]
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

            await AssertGenerateProjectAsync(selector, projectName, projectType, framework, language, BaseGenAndBuildFixture.GetRandomName);
        }

        [Theory]
        [MemberData("GetProjectTemplatesForGenerationAsync")]
        [Trait("Type", "GenerationRightClick")]
        public async Task GenEmptyProjectWithAllRightClickItemsAsync(string projectType, string framework, string language)
        {
            var projectName = $"{projectType}{framework}AllRightClick";
            var projectPath = Path.Combine(_fixture.TestProjectsPath, projectName, projectName);

            await AssertGenerateRightClickAsync(projectName, projectType, framework, language, true);
        }

        [Theory]
        [MemberData("GetProjectTemplatesForGenerationAsync")]
        [Trait("Type", "GenerationRightClick")]
        public async Task GenCompleteProjectWithAllRightClickItemsAsync(string projectType, string framework, string language)
        {
            var projectName = $"{projectType}{framework}AllRightClick2";
            var projectPath = Path.Combine(_fixture.TestProjectsPath, projectName, projectName);

            await AssertGenerateRightClickAsync(projectName, projectType, framework, language, false);
        }

        [Theory]
        [MemberData("GetPageAndFeatureTemplatesForGenerationAsync", "MVVMLight")]
        [Trait("Type", "GenerationOneByOneMVVMLight")]
        public async Task GenMVVMLightOneByOneItemsAsync(string itemName, string projectType, string framework, string itemId, string language)
        {
            await AssertGenerationOneByOneAsync(itemName, projectType, framework, itemId, language);
        }

        [Theory]
        [MemberData("GetPageAndFeatureTemplatesForGenerationAsync", "CaliburnMicro")]
        [Trait("Type", "GenerationOneByOneCaliburnMicro")]
        public async Task GenCaliburnMicroOneByOneItemsAsync(string itemName, string projectType, string framework, string itemId, string language)
        {
            await AssertGenerationOneByOneAsync(itemName, projectType, framework, itemId, language);
        }

        [Theory]
        [MemberData("GetPageAndFeatureTemplatesForGenerationAsync", "Prism")]
        [Trait("Type", "GenerationOneByOnePrism")]
        public async Task GenPrismOneByOneItemsAsync(string itemName, string projectType, string framework, string itemId, string language)
        {
            await AssertGenerationOneByOneAsync(itemName, projectType, framework, itemId, language);
        }

        [Theory]
        [MemberData("GetPageAndFeatureTemplatesForGenerationAsync", "MVVMBasic")]
        [Trait("Type", "GenerationOneByOneMVVMBasic")]
        public async Task GenMVVMBasicOneByOneItemsAsync(string itemName, string projectType, string framework, string itemId, string language)
        {
            await AssertGenerationOneByOneAsync(itemName, projectType, framework, itemId, language);
        }

        [Theory]
        [MemberData("GetPageAndFeatureTemplatesForGenerationAsync", "CodeBehind")]
        [Trait("Type", "GenerationOneByOneCodeBehind")]
        public async Task GenCodeBehindOneByOneItemsAsync(string itemName, string projectType, string framework, string itemId, string language)
        {
            await AssertGenerationOneByOneAsync(itemName, projectType, framework, itemId, language);
        }
    }
}
