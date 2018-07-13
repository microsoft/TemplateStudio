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
using Microsoft.Templates.UI;
using Xunit;

namespace Microsoft.Templates.Test
{
    [Collection("BuildRightClickWithLegacyCollection")]
    public class BuildRightClickWithLegacyTests : BaseGenAndBuildTests
    {
        private string[] excludedTemplates = { };

        public BuildRightClickWithLegacyTests(BuildRightClickWithLegacyFixture fixture)
        {
            _fixture = fixture;
            _fixture.InitializeFixture(this);
        }

        [Theory]
        [MemberData("GetProjectTemplatesForBuild", "LegacyFrameworks")]
        [Trait("ExecutionSet", "BuildRightClickWithLegacy")]
        [Trait("Type", "BuildRightClickLegacy")]
        public async Task BuildEmptyLegacyProjectWithAllRightClickItemsAsync(string projectType, string framework, string platform, string language)
        {
            var projectName = $"{projectType}{framework}Legacy";

            Func<ITemplateInfo, bool> selector =
               t => t.GetTemplateType() == TemplateType.Project
                   && t.GetProjectTypeList().Contains(projectType)
                   && t.GetFrameworkList().Contains(framework)
                   && !t.GetIsHidden()
                   && t.GetLanguage() == language;

            var projectPath = await AssertGenerateProjectAsync(selector, projectName, projectType, framework, Platforms.Uwp, language, null, null, false);

            var fixture = _fixture as BuildRightClickWithLegacyFixture;
            fixture.ChangeTemplatesSource(fixture.LocalSource, language, Platforms.Uwp);

            var rightClickTemplates = _fixture.Templates().Where(
                                          t => (t.GetTemplateType() == TemplateType.Feature || t.GetTemplateType() == TemplateType.Page)
                                            && t.GetFrameworkList().Contains(framework)
                                            && !excludedTemplates.Contains(t.GroupIdentity)
                                            && t.GetPlatform() == platform
                                            && !t.GetIsHidden()
                                            && t.GetRightClickEnabled());

            await AddRightClickTemplatesAsync(rightClickTemplates, projectName, projectType, framework, platform, language);

            AssertBuildProjectAsync(projectPath, projectName, platform);
        }

        [Theory]
        [MemberData("GetProjectTemplatesForBuild", "LegacyFrameworks")]
        [Trait("ExecutionSet", "ManualOnly")]
        ////This test sets up projects for further manual tests. It generates legacy projects with all pages and features.
        public async Task GenerateLegacyProjectWithAllPagesAndFeaturesAsync(string projectType, string framework, string platform, string language)
        {
            var projectName = $"{ShortProjectType(projectType)}{framework}AllLegacy";

            Func<ITemplateInfo, bool> selector =
               t => t.GetTemplateType() == TemplateType.Project
                   && t.GetProjectTypeList().Contains(projectType)
                   && t.GetFrameworkList().Contains(framework)
                   && !t.GetIsHidden()
                   && t.GetLanguage() == language;

            // var projectPath = await AssertGenerateProjectAsync(selector, projectName, projectType, framework, platform, language, BaseGenAndBuildFixture.GetDefaultName, false);
            // Temp workaround to be able to generate project from template without platform
            var projectPath = await AssertGenerateProjectWithOutPlatformAsync(selector, projectName, projectType, framework, language, BaseGenAndBuildFixture.GetDefaultName, false);
        }

        protected async Task<string> AssertGenerateProjectWithOutPlatformAsync(Func<ITemplateInfo, bool> projectTemplateSelector, string projectName, string projectType, string framework, string language, Func<ITemplateInfo, string> getName = null, bool cleanGeneration = true)
        {
            BaseGenAndBuildFixture.SetCurrentLanguage(language);

            var targetProjectTemplate = _fixture.Templates().FirstOrDefault(projectTemplateSelector);

            ProjectName = projectName;

            DestinationPath = Path.Combine(_fixture.TestProjectsPath, projectName, projectName);
            DestinationParentPath = Path.Combine(_fixture.TestProjectsPath, projectName);

            var userSelection = _fixture.SetupProject(projectType, framework, string.Empty, language);

            if (getName != null)
            {
                _fixture.AddItems(userSelection, GetTemplates(framework), getName);
            }

            await NewProjectGenController.Instance.UnsafeGenerateProjectAsync(userSelection);

            var resultPath = Path.Combine(_fixture.TestProjectsPath, projectName);

            // Assert
            Assert.True(Directory.Exists(resultPath));
            Assert.True(Directory.GetFiles(resultPath, "*.*", SearchOption.AllDirectories).Count() > 2);

            // Clean
            if (cleanGeneration)
            {
                Fs.SafeDeleteDirectory(resultPath);
            }

            return resultPath;
        }

        public IEnumerable<ITemplateInfo> GetTemplates(string framework)
        {
            return GenContext.ToolBox.Repo.GetAll().Where(t => t.GetFrameworkList().Contains(framework));
        }
    }
}
