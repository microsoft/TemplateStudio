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
    [Collection("BuildRightClickWithLegacy")]
    [Trait("ExecutionSet", "BuildLegacy")]
    public class BuildRightClickWithLegacyTests : BaseGenAndBuildTests
    {
        private new BuildRightClickWithLegacyFixture _fixture;

        public BuildRightClickWithLegacyTests(BuildRightClickWithLegacyFixture fixture)
        {
            _fixture = fixture;
        }

        [Theory]
        [MemberData("GetProjectTemplatesForBuildAsync", "")]
        [Trait("Type", "BuildRightClickLegacy")]
        public async Task BuildEmptyProjectWithAllRightClickItemsAsync(string projectType, string framework, string language)
        {
            var projectName = $"{projectType}AllLegacy";

            Func<ITemplateInfo, bool> selector =
               t => t.GetTemplateType() == TemplateType.Project
                   && t.GetProjectTypeList().Contains(projectType)
                   && t.GetFrameworkList().Contains(framework)
                   && !t.GetIsHidden()
                   && t.GetLanguage() == language;

            var projectPath = await AssertGenerateProjectAsync(selector, projectName, projectType, framework, language, GenerationFixture.GetRandomName, false);

            await _fixture.ChangeTemplatesSourceAsync(_fixture.LocalSource, language);

            await AddRightClickTemplatesAsync(projectName, projectType, framework, language);

            AssertBuildProjectAsync(projectPath, projectName);
        }
    }
}
