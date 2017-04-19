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

using Microsoft.TemplateEngine.Abstractions;
using Microsoft.Templates.Core;
using Microsoft.Templates.Core.Gen;
using Microsoft.Templates.Core.Locations;
using Microsoft.Templates.Test.Artifacts;
using Microsoft.Templates.Wizard;
using Microsoft.Templates.Wizard.Host;

using Xunit;
using System.Text.RegularExpressions;

namespace Microsoft.Templates.Test
{
    public class GenerationTests : IClassFixture<GenerationTestsFixture>
    {
        private GenerationTestsFixture fixture;
        private const string Platform = "x86";
        private const string Configuration = "Debug";
        private List<string> UsedNames = new List<string>();

        public GenerationTests(GenerationTestsFixture fixture)
        {
            this.fixture = fixture;
            GenContext.Bootstrap(new LocalTemplatesSource(), new FakeGenShell());
        }


        [Theory, MemberData("GetProjectTemplates"), Trait("Type", "ProjectGeneration")]
        public async void GenerateEmptyProject(string name, string framework, string projId)
        {
            var projectTemplate = GenerationTestsFixture.Templates.Where(t => t.Identity == projId).FirstOrDefault();

            var projectName = $"{name}{framework}";

            using (var context = GenContext.CreateNew(projectName, Path.Combine(fixture.TestProjectsPath, projectName, projectName)))
            {
                var wizardState = new WizardState
                {
                    Framework = framework,
                    ProjectType = projectTemplate.GetProjectType(),
                };

                AddLayoutItems(wizardState, projectTemplate);

                await GenController.UnsafeGenerateAsync(wizardState);

                //Build solution
                var outputPath = Path.Combine(fixture.TestProjectsPath, projectName);
                var result = BuildSolution(projectName, outputPath);

                //Assert
                Assert.True(result.exitCode.Equals(0), $"Solution {projectTemplate.Name} was not built successfully. {Environment.NewLine}Errors found: {GetErrorLines(result.outputFile)}.{Environment.NewLine}Please see {Path.GetFullPath(result.outputFile)} for more details.");

                //Clean
                Directory.Delete(outputPath, true);
            }
        }

        
        [Theory (Skip ="Long running"), MemberData("GetPageAndFeatureTemplates"), Trait("Type", "ProjectWithIsolatedItemGeneration")]
        public async void GenerateProjectWithIsolatedItems(string itemName, string name, string framework, string projId, string itemId)
        {
            var projectTemplate = GenerationTestsFixture.Templates.Where(t => t.Identity == projId).FirstOrDefault();
            var itemTemplate = GenerationTestsFixture.Templates.FirstOrDefault(t => t.Identity == itemId);
            var itemInferredName = Naming.Infer(UsedNames, itemTemplate.GetDefaultName());

            var projectName = $"{name}{framework}{itemInferredName}";

            using (var context = GenContext.CreateNew(projectName, Path.Combine(fixture.TestProjectsPath, projectName, projectName)))
            {
                var wizardState = new WizardState
                {
                    Framework = framework,
                    ProjectType = projectTemplate.GetProjectType(),
                };

                AddLayoutItems(wizardState, projectTemplate);
                AddItem(wizardState, itemInferredName, itemTemplate);

                await GenController.UnsafeGenerateAsync(wizardState);

                //Build solution
                var outputPath = Path.Combine(fixture.TestProjectsPath, projectName);
                var result = BuildSolution(projectName, outputPath);

                //Assert
                Assert.True(result.exitCode.Equals(0), $"Solution {projectTemplate.Name} was not built successfully. {Environment.NewLine}Errors found: {GetErrorLines(result.outputFile)}.{Environment.NewLine}Please see {Path.GetFullPath(result.outputFile)} for more details.");

                //Clean
                Directory.Delete(outputPath, true);
            }
        }

        [Theory, MemberData("GetProjectTemplates"), Trait("Type", "ProjectGeneration")]
        public async void GenerateAllPagesAndFeatures(string name, string framework, string projId)
        {
            var targetProjectTemplate = GenerationTestsFixture.Templates.Where(t => t.Identity == projId).FirstOrDefault();

            var projectName = $"{name}{framework}All";

            using (var context = GenContext.CreateNew(projectName, Path.Combine(fixture.TestProjectsPath, projectName, projectName)))
            {
                var wizardState = new WizardState
                {
                    Framework = framework,
                    ProjectType = targetProjectTemplate.GetProjectType(),
                };

                AddLayoutItems(wizardState, targetProjectTemplate);
                AddItems(wizardState, GetTemplates(framework, TemplateType.Page));
                AddItems(wizardState, GetTemplates(framework, TemplateType.Feature));

                await GenController.UnsafeGenerateAsync(wizardState);

                //Build solution
                var outputPath = Path.Combine(fixture.TestProjectsPath, projectName);
                var result = BuildSolution(projectName, outputPath);

                //Assert
                Assert.True(result.exitCode.Equals(0), $"Solution {targetProjectTemplate.Name} was not built successfully. {Environment.NewLine}Errors found: {GetErrorLines(result.outputFile)}.{Environment.NewLine}Please see {Path.GetFullPath(result.outputFile)} for more details.");

                //Clean
                Directory.Delete(outputPath, true);
            }
        }


        private IEnumerable<ITemplateInfo> GetTemplates(string framework, TemplateType templateType)
        {
            return GenerationTestsFixture.Templates
                                              .Where(t => t.GetFrameworkList().Contains(framework) &&
                                              t.GetTemplateType() == templateType);
        }

        private void AddLayoutItems(WizardState wizardState, ITemplateInfo projectTemplate)
        {
            var pages = new List<(string name, string templateName)>();
            var layouts = projectTemplate.GetLayout();

            foreach (var layoutItem in layouts)
            {
                var template = GenerationTestsFixture.Templates.FirstOrDefault(t => t.GroupIdentity == layoutItem.templateGroupIdentity && t.GetFrameworkList().Any(f => f.Equals(wizardState.Framework, StringComparison.OrdinalIgnoreCase)));
                if (template == null)
                {
                    throw new Exception($"Template {layoutItem.templateGroupIdentity} could not be found");
                }
                AddItem(wizardState, layoutItem.name, template);
            }
        }

        private void AddItems(WizardState wizardState, IEnumerable<ITemplateInfo> templates)
        {
            foreach (var template in templates)
            {
                var itemName = Naming.Infer(UsedNames, template.GetDefaultName());
                AddItem(wizardState, itemName, template);
            }
        }

        private void AddItem(WizardState wizardState, string itemName, ITemplateInfo template)
        {
            switch (template.GetTemplateType())
            {
                case TemplateType.Page:
                    wizardState.Pages.Add((itemName, template));
                    break;
                case TemplateType.Feature:
                    wizardState.DevFeatures.Add((itemName, template));
                    break;
            }
            UsedNames.Add(itemName);
        }

        public static IEnumerable<object[]> GetProjectTemplates()
        {
            var projectTemplates = GenerationTestsFixture.Templates.Where(t => t.GetTemplateType() == TemplateType.Project);
            foreach (var template in projectTemplates)
            {
                foreach (var framework in template.GetFrameworkList())
                {
                    yield return new object[] { template.Name, framework, template.Identity };
                }
            }
        }

        public static IEnumerable<object[]> GetPageAndFeatureTemplates()
        {
            var projectTemplates = GenerationTestsFixture.Templates.Where(t => t.GetTemplateType() == TemplateType.Project);
            foreach (var template in projectTemplates)
            {
                foreach (var framework in template.GetFrameworkList())
                {
                    var itemTemplates = GenerationTestsFixture.Templates.Where(t => t.GetFrameworkList().Contains(framework) && t.GetTemplateType() == TemplateType.Page || t.GetTemplateType() == TemplateType.Feature);

                    foreach (var itemTemplate in itemTemplates)
                    {
                        yield return new object[] { itemTemplate.Name, template.Name, framework, template.Identity,  itemTemplate.Identity };
                    }
                }
            }
        }

        private static (int exitCode, string outputFile) BuildSolution(string solutionName, string outputPath)
        {
            var outputFile = Path.Combine(outputPath, "_buildOutput.txt");

            //Build
            var solutionFile = Path.GetFullPath(outputPath + @"\" + solutionName + ".sln");
            var startInfo = new ProcessStartInfo(GetPath("RestoreAndBuild.bat"))
            {
                Arguments = $"\"{solutionFile}\" {Platform} {Configuration}",
                UseShellExecute = false,
                RedirectStandardOutput = true,
                CreateNoWindow = false,
                WorkingDirectory = outputPath
            };

            var process = Process.Start(startInfo);
            File.WriteAllText(outputFile, process.StandardOutput.ReadToEnd());
            process.WaitForExit();

            return (process.ExitCode, outputFile);
        }

        internal static string GetPath(string fileName)
        {
            string path = Path.Combine(new FileInfo(Assembly.GetExecutingAssembly().Location).DirectoryName, fileName);
            if (!File.Exists(path))
            {
                path = Path.GetFullPath($@".\{fileName}");
                if (!File.Exists(path))
                {
                    throw new ApplicationException($"Can not find {fileName}");
                }
            }
            return path;
        }

        private string GetErrorLines(string filePath)
        {
            Regex re = new Regex(@"^.*error .*$", RegexOptions.Multiline & RegexOptions.IgnoreCase);

            var outputLines = File.ReadAllLines(filePath);

            var errorLines = outputLines.Where(l => re.IsMatch(l));

            return errorLines.Count() > 0 ? errorLines.Aggregate((i, j) => i + Environment.NewLine + j) : String.Empty;
        }
    }
}
