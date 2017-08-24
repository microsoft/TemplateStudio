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
using Microsoft.Templates.Core.PostActions.Catalog.Merge;
using Microsoft.Templates.Core.Locations;
using Microsoft.Templates.Fakes;
using Microsoft.Templates.UI;

using Xunit;

namespace Microsoft.Templates.Test
{
    [Collection("Generation collection")]
    public class NewItemGenerationTests : BaseTestContextProvider
    {
        private GenerationFixture _fixture;

        public NewItemGenerationTests(GenerationFixture fixture)
        {
            _fixture = fixture;
        }

        private void SetUpFixtureForTesting(string language)
        {
            _fixture.InitializeFixture(language, this);
        }

        [Theory]
        [MemberData("GetProjectTemplates")]
        [Trait("Type", "NewItemGeneration")]
        public async Task GenerateProjectWithAllRightClickItemsAsync(string projectType, string framework, string language)
        {
            SetUpFixtureForTesting(language);

            var projectName = $"{projectType}{framework}";

            ProjectName = projectName;
            ProjectPath = Path.Combine(_fixture.TestNewItemPath, projectName, projectName);
            OutputPath = ProjectPath;

            var userSelection = GenerationFixture.SetupProject(projectType, framework, language);
            await NewProjectGenController.Instance.UnsafeGenerateProjectAsync(userSelection);

            // Add new item
            var rightClickTemplates = GenerationFixture.Templates.Where(
                                            t => (t.GetTemplateType() == TemplateType.Feature || t.GetTemplateType() == TemplateType.Page)
                                              && t.GetFrameworkList().Contains(framework)
                                              && !t.GetIsHidden()
                                              && t.GetRightClickEnabled());

            foreach (var item in rightClickTemplates)
            {
                OutputPath = GenContext.GetTempGenerationPath(projectName);

                var newUserSelection = new UserSelection
                {
                    ProjectType = projectType,
                    Framework = framework,
                    HomeName = "",
                    Language = language,
                    ItemGenerationType = ItemGenerationType.GenerateAndMerge
                };

                GenerationFixture.AddItem(newUserSelection, item, GenerationFixture.GetDefaultName);
                await NewItemGenController.Instance.UnsafeGenerateNewItemAsync(item.GetTemplateType(), newUserSelection);
                NewItemGenController.Instance.UnsafeFinishGeneration(newUserSelection);
            }

            // Build solution
            var outputPath = Path.Combine(_fixture.TestNewItemPath, projectName);
            var result = GenerationFixture.BuildSolution(projectName, outputPath);

            // Assert
            Assert.True(result.exitCode.Equals(0), $"Solution {projectName} was not built successfully. {Environment.NewLine}Errors found: {GenerationFixture.GetErrorLines(result.outputFile)}.{Environment.NewLine}Please see {Path.GetFullPath(result.outputFile)} for more details.");

            // Clean
            Directory.Delete(outputPath, true);
        }

        private static string GetTempGenerationPath(string projectName)
        {
            var tempGenerationName = $"{projectName}_{DateTime.Now.FormatAsShortDateTime()}";
            var tempGenerationPath = Path.Combine(Path.GetTempPath(), Configuration.Current.TempGenerationFolderPath);
            var inferredName = Naming.Infer(tempGenerationName, new List<Validator>() { new DirectoryExistsValidator(tempGenerationPath) }, "_");

            return Path.Combine(tempGenerationPath, inferredName);
        }

        public static IEnumerable<object[]> GetProjectTemplates()
        {
            return GenerationFixture.GetProjectTemplates();
        }
    }
}
