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

        public List<GenerationWarning> GenerationWarnings { get; } = new List<GenerationWarning>();

        public Dictionary<string, List<MergeInfo>> MergeFilesFromProject { get; } = new Dictionary<string, List<MergeInfo>>();

        public NewItemGenerationTests(GenerationFixture fixture)
        {
            _fixture = fixture;
            GenContext.Bootstrap(new LocalTemplatesSource(), new FakeGenShell());
            GenContext.Current = this;

        }


        [Theory, MemberData("GetProjectTemplates"), Trait("Type", "NewItemGeneration")]
        public async void GenerateEmptyProject(string projectType, string framework)
        {
            var projectName = $"{projectType}{framework}";

            ProjectName = projectName;
            ProjectPath = Path.Combine(_fixture.TestProjectsPath, projectName, projectName);
            OutputPath = ProjectPath;

            var userSelection = GenerationFixture.SetupProject(projectType, framework);
            await NewProjectGenController.Instance.UnsafeGenerateProjectAsync(userSelection);

            //Add new item
            var rightClickTemplates = GenerationFixture.Templates.Where
                                            (t => (t.GetTemplateType() == TemplateType.Feature || t.GetTemplateType() == TemplateType.Page)
                                                && t.GetFrameworkList().Contains(framework)
                                                && t.GetRightClickEnabled());

            foreach (var item in rightClickTemplates)
            {
                OutputPath = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());

                var newUserSelection = new UserSelection()
                {
                    ProjectType = projectType, 
                    Framework = framework,
                    HomeName = ""
                };
                GenerationFixture.AddItem(newUserSelection, item, GenerationFixture.GetDefaultName);
                await NewItemGenController.Instance.UnsafeGenerateNewItemAsync(newUserSelection);
                NewItemGenController.Instance.UnsafeSyncNewItem();
            }

            //Build solution
            var outputPath = Path.Combine(_fixture.TestProjectsPath, projectName);
            var result = GenerationFixture.BuildSolution(projectName, outputPath);

            //Assert
            Assert.True(result.exitCode.Equals(0), $"Solution {projectName} was not built successfully. {Environment.NewLine}Errors found: {GenerationFixture.GetErrorLines(result.outputFile)}.{Environment.NewLine}Please see {Path.GetFullPath(result.outputFile)} for more details.");

            //Clean
            Directory.Delete(outputPath, true);
        }

        public static IEnumerable<object[]> GetProjectTemplates()
        {
            return GenerationFixture.GetProjectTemplates();
           
        }
    }
}
