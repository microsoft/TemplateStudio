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
    [Collection("VBStyleCollection")]
    [Trait("ExecutionSet", "BuildVBStyle")]
    public class VBStyleProjectGenerationTests : BaseTestContextProvider
    {
        private readonly VBStyleGenerationTestsFixture _fixture;

        public VBStyleProjectGenerationTests(VBStyleGenerationTestsFixture fixture)
        {
            _fixture = fixture;
        }

        private async Task SetUpFixtureForTestingAsync()
        {
            await _fixture.InitializeFixtureAsync(this);
        }

        [Theory]
        [MemberData("GetProjectTemplatesForVBStyleAsync")]
        [Trait("Type", "CodeStyle")]
        public async Task GenerateAllPagesAndFeaturesAndCheckWithVBStyleAsync(string projectType, string framework, string platform)
        {
            await SetUpFixtureForTestingAsync();

            var targetProjectTemplate = VBStyleGenerationTestsFixture.Templates
                .FirstOrDefault(t => t.GetTemplateType() == TemplateType.Project
                                  && t.GetLanguage() == ProgrammingLanguages.VisualBasic
                                  && t.GetPlatform() == platform
                                  && t.GetProjectTypeList().Contains(projectType)
                                  && t.GetFrameworkList().Contains(framework));

            var projectName = $"{projectType}{framework}AllVBStyle";

            ProjectName = projectName;
            DestinationPath = Path.Combine(_fixture.TestProjectsPath, projectName, projectName);
            OutputPath = DestinationPath;

            var userSelection = new UserSelection(projectType, framework, platform, ProgrammingLanguages.VisualBasic)
            {
                HomeName = "Main"
            };

            AddLayoutItems(userSelection);
            _fixture.AddItems(userSelection, GetTemplates(framework, platform, TemplateType.Page), _fixture.GetDefaultName);
            _fixture.AddItems(userSelection, GetTemplates(framework, platform, TemplateType.Feature), _fixture.GetDefaultName);

            var x = VBStyleGenerationTestsFixture.Templates.First(t => t.Name == "Feature.Testing.VBStyleAnalysis");

            _fixture.AddItem(userSelection, "VBStyleTesting", x);

            await NewProjectGenController.Instance.UnsafeGenerateProjectAsync(userSelection);

            // Build solution
            var outputPath = Path.Combine(_fixture.TestProjectsPath, projectName);
            var result = _fixture.BuildSolution(projectName, outputPath);

            Assert.True(result.exitCode.Equals(0), $"Solution {targetProjectTemplate.Name} was not built successfully. {Environment.NewLine}Errors found: {_fixture.GetErrorLines(result.outputFile)}.{Environment.NewLine}Please see {Path.GetFullPath(result.outputFile)} for more details.");

            // Clean
            Fs.SafeDeleteDirectory(outputPath);
        }

        public static IEnumerable<object[]> GetProjectTemplatesForVBStyleAsync()
        {
            JoinableTaskContext context = new JoinableTaskContext();
            JoinableTaskCollection tasks = context.CreateCollection();
            context.CreateFactory(tasks);
            var result = context.Factory.Run(() => VBStyleGenerationTestsFixture.GetProjectTemplatesForVBStyleAsync());
            return result;
        }

        private IEnumerable<ITemplateInfo> GetTemplates(string framework, string platform, TemplateType templateType)
        {
            return VBStyleGenerationTestsFixture.Templates
                                         .Where(t => t.GetFrameworkList().Contains(framework)
                                                  && t.GetPlatform() == platform && t.GetTemplateType() == templateType);
        }

        private void AddLayoutItems(UserSelection userSelection)
        {
            var layouts = GenComposer.GetLayoutTemplates(userSelection.ProjectType, userSelection.Framework, userSelection.Platform);

            foreach (var item in layouts)
            {
                _fixture.AddItem(userSelection, item.Layout.Name, item.Template);
            }
        }
    }
}
