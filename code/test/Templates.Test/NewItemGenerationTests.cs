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
    public class NewItemGenerationTests : IContextProvider
    {
        private GenerationFixture _fixture;
        private List<string> _usedNames = new List<string>();

        public string ProjectName { get; set; }
        public string OutputPath { get; set; }

        public string ProjectPath { get; set; }

        public List<string> ProjectItems { get; } = new List<string>();

        public List<FailedMergePostAction> FailedMergePostActions { get; } = new List<FailedMergePostAction>();

        public Dictionary<string, List<MergeInfo>> MergeFilesFromProject { get; } = new Dictionary<string, List<MergeInfo>>();

        public List<string> FilesToOpen { get; } = new List<string>();

        public NewItemGenerationTests(GenerationFixture fixture)
        {
            _fixture = fixture;
            GenContext.Bootstrap(new LocalTemplatesSource(), new FakeGenShell(), "C#");
            GenContext.Current = this;
        }

        [Theory, MemberData("GetProjectTemplates"), Trait("Type", "NewItemGeneration")]
        public async void GenerateEmptyProject(string projectType, string framework, string language)
        {
            var projectName = $"{projectType}{framework}";

            ProjectName = projectName;
            ProjectPath = Path.Combine(_fixture.TestNewItemPath, projectName, projectName);
            OutputPath = ProjectPath;

            var userSelection = GenerationFixture.SetupProject(projectType, framework, language);
            await NewProjectGenController.Instance.UnsafeGenerateProjectAsync(userSelection);

            //Add new item
            var rightClickTemplates = GenerationFixture.Templates.Where
                                            (t => (t.GetTemplateType() == TemplateType.Feature || t.GetTemplateType() == TemplateType.Page)
                                                && t.GetFrameworkList().Contains(framework)
                                                && (!t.GetIsHidden())
                                                && t.GetRightClickEnabled());

            foreach (var item in rightClickTemplates)
            {
                OutputPath = GetTempGenerationPath(projectName);

                var newUserSelection = new UserSelection()
                {
                    ProjectType = projectType,
                    Framework = framework,
                    HomeName = "",
                    ItemGenerationType = ItemGenerationType.GenerateAndMerge
                };

                GenerationFixture.AddItem(newUserSelection, item, GenerationFixture.GetDefaultName);
                await NewItemGenController.Instance.UnsafeGenerateNewItemAsync(item.GetTemplateType(), newUserSelection);
                NewItemGenController.Instance.UnsafeFinishGeneration(newUserSelection);
            }

            //Build solution
            var outputPath = Path.Combine(_fixture.TestNewItemPath, projectName);
            var result = GenerationFixture.BuildSolution(projectName, outputPath);

            //Assert
            Assert.True(result.exitCode.Equals(0), $"Solution {projectName} was not built successfully. {Environment.NewLine}Errors found: {GenerationFixture.GetErrorLines(result.outputFile)}.{Environment.NewLine}Please see {Path.GetFullPath(result.outputFile)} for more details.");

            //Clean
            Directory.Delete(outputPath, true);
        }

        private static string GetTempGenerationPath(string projectName)
        {
            var tempGenerationName = $"{projectName}_{DateTime.Now.ToString("yyyyMMdd_HHmmss")}";
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
