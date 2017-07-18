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
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;

using Microsoft.TemplateEngine.Abstractions;
using Microsoft.Templates.Core;
using Microsoft.Templates.Core.Gen;
using Microsoft.Templates.UI;
using Microsoft.Templates.Test.Artifacts;

using Xunit;

namespace Microsoft.Templates.Test
{
    [Collection("StyleCop collection")]
    public class StyleCopProjectGenerationTests : BaseTestContextProvider
    {
        private readonly StyleCopGenerationTestsFixture _fixture;

        public StyleCopProjectGenerationTests(StyleCopGenerationTestsFixture fixture)
        {
            _fixture = fixture;
            GenContext.Bootstrap(new StyleCopPlusLocalTemplatesSource(), new FakeGenShell());
            GenContext.Current = this;
        }

        [Theory, MemberData("GetProjectTemplatesForStyleCop"), Trait("Type", "ProjectGeneration")]
        public async void GenerateAllPagesAndFeaturesAndCheckWithStyleCop(string projectType, string framework)
        {
            var targetProjectTemplate = StyleCopGenerationTestsFixture.Templates
                .FirstOrDefault(t => t.GetTemplateType() == TemplateType.Project
                                  && t.GetProjectTypeList().Contains(projectType)
                                  && t.GetFrameworkList().Contains(framework));

            var projectName = $"{projectType}{framework}AllStyleCop";

            ProjectName = projectName;
            ProjectPath = Path.Combine(_fixture.TestProjectsPath, projectName, projectName);
            OutputPath = ProjectPath;

            var userSelection = new UserSelection
            {
                Framework = framework,
                ProjectType = projectType,
                HomeName = "Main"
            };

            AddLayoutItems(userSelection, targetProjectTemplate);
            _fixture.AddItems(userSelection, GetTemplates(framework, TemplateType.Page), _fixture.GetDefaultName);
            _fixture.AddItems(userSelection, GetTemplates(framework, TemplateType.Feature), _fixture.GetDefaultName);

            var x = StyleCopGenerationTestsFixture.Templates
                .First(t => t.Name == "Feature.Testing.StyleCop");

            _fixture.AddItem(userSelection, "StyleCopTesting", x);

            await NewProjectGenController.Instance.UnsafeGenerateProjectAsync(userSelection);

            //Build solution
            var outputPath = Path.Combine(_fixture.TestProjectsPath, projectName);
            var result = _fixture.BuildSolution(projectName, outputPath);

            //Assert
            Assert.True(result.exitCode.Equals(0), $"Solution {targetProjectTemplate.Name} was not built successfully. {Environment.NewLine}Errors found: {_fixture.GetErrorLines(result.outputFile)}.{Environment.NewLine}Please see {Path.GetFullPath(result.outputFile)} for more details.");

            //Clean
            Directory.Delete(outputPath, true);
        }

        public static IEnumerable<object[]> GetProjectTemplatesForStyleCop()
        {
            GenContext.Bootstrap(new StyleCopPlusLocalTemplatesSource(), new FakeGenShell());
            var projectTemplates = StyleCopGenerationTestsFixture.Templates.Where(t => t.GetTemplateType() == TemplateType.Project);

            foreach (var projectTemplate in projectTemplates)
            {
                var projectTypeList = projectTemplate.GetProjectTypeList();
                foreach (var projectType in projectTypeList)
                {
                    var frameworks = GenComposer.GetSupportedFx(projectType);
                    foreach (var framework in frameworks)
                    {
                        yield return new object[] { projectType, framework };
                    }
                }
            }
        }

        private IEnumerable<ITemplateInfo> GetTemplates(string framework, TemplateType templateType)
        {
            return StyleCopGenerationTestsFixture.Templates
                                         .Where(t => t.GetFrameworkList().Contains(framework)
                                                  && t.GetTemplateType() == templateType);
        }

        private void AddLayoutItems(UserSelection userSelection, ITemplateInfo projectTemplate)
        {
            var layouts = GenComposer.GetLayoutTemplates(userSelection.ProjectType, userSelection.Framework);

            foreach (var item in layouts)
            {
                _fixture.AddItem(userSelection, item.Layout.name, item.Template);
            }
        }
    }
}
