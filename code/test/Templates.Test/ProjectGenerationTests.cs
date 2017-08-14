// ******************************************************************
// Copyright (c) Microsoft. All rights reserved.
// This code is licensed under the MIT License (MIT).
// THE CODE IS PROVIDED “AS IS”, WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED,
// INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.
// IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
// DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT,
// TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH
// THE CODE OR THE USE OR OTHER DEALINGS IN THE CODE.
// ******************************************************************

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using Microsoft.Templates.Core;
using Microsoft.Templates.Core.Gen;
using Microsoft.Templates.Core.Locations;
using Microsoft.Templates.Core.PostActions.Catalog.Merge;
using Microsoft.Templates.Test.Artifacts;
using Microsoft.Templates.UI;

using Xunit;

namespace Microsoft.Templates.Test
{
    [Collection("Generation collection")]
    public class ProjectGenerationTests : IContextProvider
    {
        private const string Platform = "x86";
        private const string Configuration = "Debug";

        private GenerationFixture _fixture;
        private List<string> _usedNames = new List<string>();

        public string ProjectName { get; set; }
        public string OutputPath { get; set; }

        public string ProjectPath { get; set; }

        public List<string> ProjectItems  { get; } = new List<string>();

        public List<FailedMergePostAction> FailedMergePostActions { get; } = new List<FailedMergePostAction>();

        public Dictionary<string, List<MergeInfo>> MergeFilesFromProject { get; } = new Dictionary<string, List<MergeInfo>>();

        public List<string> FilesToOpen { get; } = new List<string>();


        public ProjectGenerationTests(GenerationFixture fixture)
        {
            _fixture = fixture;
            GenContext.Bootstrap(new LocalTemplatesSource(), new FakeGenShell());
            GenContext.Current = this;

        }


        [Theory, MemberData("GetProjectTemplates"), Trait("Type", "ProjectGeneration")]
        public async void GenerateEmptyProject(string projectType, string framework)
        {
            var projectTemplate = GenerationFixture.Templates.Where(t => t.GetTemplateType() == TemplateType.Project && t.GetProjectTypeList().Contains(projectType) && t.GetFrameworkList().Contains(framework)).FirstOrDefault();
            var projectName = $"{projectType}{framework}";

            ProjectName = projectName;
            ProjectPath = Path.Combine(_fixture.TestProjectsPath, projectName, projectName);
            OutputPath = ProjectPath;

            var userSelection = GenerationFixture.SetupProject(projectType, framework);

            await NewProjectGenController.Instance.UnsafeGenerateProjectAsync(userSelection);

            //Build solution
            var outputPath = Path.Combine(_fixture.TestProjectsPath, projectName);
            var result = GenerationFixture.BuildSolution(projectName, outputPath);

            //Assert
            Assert.True(result.exitCode.Equals(0), $"Solution {projectTemplate.Name} was not built successfully. {Environment.NewLine}Errors found: {GenerationFixture.GetErrorLines(result.outputFile)}.{Environment.NewLine}Please see {Path.GetFullPath(result.outputFile)} for more details.");

            //Clean
            Directory.Delete(outputPath, true);
        }


        [Theory, MemberData("GetPageAndFeatureTemplates"), Trait("Type", "OneByOneItemGeneration")]
        public async void GenerateProjectWithIsolatedItems(string itemName, string projectType, string framework, string itemId)
        {
            var projectTemplate = GenerationFixture.Templates.FirstOrDefault(t => t.GetTemplateType() == TemplateType.Project && t.GetProjectTypeList().Contains(projectType) && t.GetFrameworkList().Contains(framework));
            var itemTemplate = GenerationFixture.Templates.FirstOrDefault(t => t.Identity == itemId);
            var finalName = itemTemplate.GetDefaultName();
            var validators = new List<Validator>()
            {
                new ReservedNamesValidator(),
            };
            if (itemTemplate.GetItemNameEditable())
            {
                validators.Add(new DefaultNamesValidator());
            }

            finalName = Naming.Infer(finalName, validators);

            var projectName = $"{projectType}{framework}{finalName}";

            ProjectName = projectName;
            ProjectPath = Path.Combine(_fixture.TestProjectsPath, projectName, projectName);
            OutputPath = ProjectPath;

            var userSelection = GenerationFixture.SetupProject(projectType, framework);

            GenerationFixture.AddItem(userSelection, itemTemplate, GenerationFixture.GetDefaultName);

            await NewProjectGenController.Instance.UnsafeGenerateProjectAsync(userSelection);

            //Build solution
            var outputPath = Path.Combine(_fixture.TestProjectsPath, projectName);
            var result = GenerationFixture.BuildSolution(projectName, outputPath);

            //Assert
            Assert.True(result.exitCode.Equals(0), $"Solution {projectTemplate.Name} was not built successfully. {Environment.NewLine}Errors found: {GenerationFixture.GetErrorLines(result.outputFile)}.{Environment.NewLine}Please see {Path.GetFullPath(result.outputFile)} for more details.");

            //Clean
            Directory.Delete(outputPath, true);
        }

        [Theory, MemberData("GetProjectTemplates"), Trait("Type", "ProjectGeneration")]
        public async void GenerateAllPagesAndFeatures(string projectType, string framework)
        {
            var targetProjectTemplate = GenerationFixture.Templates.Where(t => t.GetTemplateType() == TemplateType.Project && t.GetProjectTypeList().Contains(projectType) && t.GetFrameworkList().Contains(framework)).FirstOrDefault();

            var projectName = $"{projectType}{framework}All";

            ProjectName = projectName;
            ProjectPath = Path.Combine(_fixture.TestProjectsPath, projectName, projectName);
            OutputPath = ProjectPath;

            var userSelection = GenerationFixture.SetupProject(projectType, framework);

            GenerationFixture.AddItems(userSelection, GenerationFixture.GetTemplates(framework), GenerationFixture.GetDefaultName);

            await NewProjectGenController.Instance.UnsafeGenerateProjectAsync(userSelection);

            //Build solution
            var outputPath = Path.Combine(_fixture.TestProjectsPath, projectName);
            var result = GenerationFixture.BuildSolution(projectName, outputPath);

            //Assert
            Assert.True(result.exitCode.Equals(0), $"Solution {targetProjectTemplate.Name} was not built successfully. {Environment.NewLine}Errors found: {GenerationFixture.GetErrorLines(result.outputFile)}.{Environment.NewLine}Please see {Path.GetFullPath(result.outputFile)} for more details.");

            //Clean
            Directory.Delete(outputPath, true);
        }

        [Theory, MemberData("GetProjectTemplates"), Trait("Type", "ProjectGeneration")]
        public async void GenerateAllPagesAndFeaturesRandomNames(string projectType, string framework)
        {
            var targetProjectTemplate = GenerationFixture.Templates.FirstOrDefault(t => t.GetTemplateType() == TemplateType.Project && t.GetProjectTypeList().Contains(projectType) && t.GetFrameworkList().Contains(framework) && !t.GetIsHidden());
            var projectName = $"{projectType}{framework}AllRandom";

            ProjectName = projectName;
            ProjectPath = Path.Combine(_fixture.TestProjectsPath, projectName, projectName);
            OutputPath = ProjectPath;

            var userSelection = GenerationFixture.SetupProject(projectType, framework);

            GenerationFixture.AddItems(userSelection, GenerationFixture.GetTemplates(framework), GenerationFixture.GetRandomName);

            await NewProjectGenController.Instance.UnsafeGenerateProjectAsync(userSelection);

            //Build solution
            var outputPath = Path.Combine(_fixture.TestProjectsPath, projectName);
            var result = GenerationFixture.BuildSolution(projectName, outputPath);

            //Assert
            Assert.True(result.exitCode.Equals(0), $"Solution {targetProjectTemplate.Name} was not built successfully. {Environment.NewLine}Errors found: {GenerationFixture.GetErrorLines(result.outputFile)}.{Environment.NewLine}Please see {Path.GetFullPath(result.outputFile)} for more details.");

            //Clean
            Directory.Delete(outputPath, true);
        }


        public static IEnumerable<object[]> GetProjectTemplates()
        {
            return GenerationFixture.GetProjectTemplates();

        }

        public static IEnumerable<object[]> GetPageAndFeatureTemplates()
        {
            return GenerationFixture.GetPageAndFeatureTemplates();
        }
    }
}
