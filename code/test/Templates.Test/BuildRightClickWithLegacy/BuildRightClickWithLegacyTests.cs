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
    [Collection("BuildRightClickWithLegacyCollection")]
    public class BuildRightClickWithLegacyTests : BaseGenAndBuildTests
    {
        private string[] excludedTemplates = { };

        public BuildRightClickWithLegacyTests(BuildRightClickWithLegacyFixture fixture)
        {
            _fixture = fixture;
        }

        [Theory]
        [MemberData("GetProjectTemplatesForBuildAsync", "LegacyFrameworks")]
        [Trait("ExecutionSet", "BuildRightClickWithLegacy")]
        [Trait("Type", "BuildRightClickLegacy")]
        public async Task BuildEmptyLegacyProjectWithAllRightClickItemsAsync(string projectType, string framework, string language)
        {
            await _fixture.InitializeFixtureAsync(this);

            var projectName = $"{projectType}{framework}Legacy";

            Func<ITemplateInfo, bool> selector =
               t => t.GetTemplateType() == TemplateType.Project
                   && t.GetProjectTypeList().Contains(projectType)
                   && t.GetFrameworkList().Contains(framework)
                   && !t.GetIsHidden()
                   && t.GetLanguage() == language;

            var projectPath = await AssertGenerateProjectAsync(selector, projectName, projectType, framework, language, null, false);

            var fixture = _fixture as BuildRightClickWithLegacyFixture;
            await fixture.ChangeTemplatesSourceAsync(fixture.LocalSource);

            var rightClickTemplates = _fixture.Templates().Where(
                                          t => (t.GetTemplateType() == TemplateType.Feature || t.GetTemplateType() == TemplateType.Page)
                                            && t.GetFrameworkList().Contains(framework)
                                            && !excludedTemplates.Contains(t.GroupIdentity)
                                            && !t.GetIsHidden()
                                            && t.GetRightClickEnabled());

            await AddRightClickTemplatesAsync(rightClickTemplates, projectName, projectType, framework, language);

            AssertBuildProjectAsync(projectPath, projectName);
        }

        [Theory]
        [MemberData("GetProjectTemplatesForBuildAsync", "LegacyFrameworks")]
        [Trait("ExecutionSet", "ManualOnly")]
        ////This test sets up projects for further manual tests. It generates legacy projects with all pages and features.
        public async Task GenerateLegacyProjectWithAllPagesAndFeaturesAsync(string projectType, string framework, string language)
        {
            await _fixture.InitializeFixtureAsync(this);

            var projectName = $"{projectType}{framework}AllLegacy";

            Func<ITemplateInfo, bool> selector =
               t => t.GetTemplateType() == TemplateType.Project
                   && t.GetProjectTypeList().Contains(projectType)
                   && t.GetFrameworkList().Contains(framework)
                   && !t.GetIsHidden()
                   && t.GetLanguage() == language;

            var projectPath = await AssertGenerateProjectAsync(selector, projectName, projectType, framework, language, BaseGenAndBuildFixture.GetDefaultName, false);
        }
    }
}
