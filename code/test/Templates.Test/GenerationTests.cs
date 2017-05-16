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
using Microsoft.Templates.Core.Locations;
using Microsoft.Templates.UI;
using Microsoft.Templates.Test.Artifacts;

using Xunit;

namespace Microsoft.Templates.Test
{
    public class GenerationTests : IClassFixture<GenerationTestsFixture>, IContextProvider
    {
        private const string Platform = "x86";
        private const string Configuration = "Debug";

        private GenerationTestsFixture _fixture;
        private List<string> _usedNames = new List<string>();

        public string ProjectName { get; set; }
        public string OutputPath { get; set; }

        public GenerationTests(GenerationTestsFixture fixture)
        {
            _fixture = fixture;
            GenContext.Bootstrap(new LocalTemplatesSource(), new FakeGenShell());
            GenContext.Current = this;
        }

        [Theory, MemberData("GetProjectTemplates"), Trait("Type", "ProjectGeneration")]
        public async void GenerateEmptyProject(string name, string framework, string projId)
        {
            var projectTemplate = GenerationTestsFixture.Templates.Where(t => t.Identity == projId).FirstOrDefault();
            var projectName = $"{name}{framework}";

            ProjectName = projectName;
            OutputPath = Path.Combine(_fixture.TestProjectsPath, projectName, projectName);

            var userSelection = new UserSelection
            {
                Framework = framework,
                ProjectType = projectTemplate.GetProjectType(),
            };

            AddLayoutItems(userSelection, projectTemplate);

            await GenController.UnsafeGenerateAsync(userSelection);

            //Build solution
            var outputPath = Path.Combine(_fixture.TestProjectsPath, projectName);
            var result = BuildSolution(projectName, outputPath);

            //Assert
            Assert.True(result.exitCode.Equals(0), $"Solution {projectTemplate.Name} was not built successfully. {Environment.NewLine}Errors found: {GetErrorLines(result.outputFile)}.{Environment.NewLine}Please see {Path.GetFullPath(result.outputFile)} for more details.");

            //Clean
            Directory.Delete(outputPath, true);
        }


        [Theory, MemberData("GetPageAndFeatureTemplates"), Trait("Type", "OneByOneItemGeneration")]
        public async void GenerateProjectWithIsolatedItems(string itemName, string name, string framework, string projId, string itemId)
        {
            var projectTemplate = GenerationTestsFixture.Templates.Where(t => t.Identity == projId).FirstOrDefault();
            var itemTemplate = GenerationTestsFixture.Templates.FirstOrDefault(t => t.Identity == itemId);
            var finalName = itemTemplate.GetDefaultName();

            if (itemTemplate.GetItemNameEditable())
            {
                finalName = Naming.Infer(_usedNames, itemTemplate.GetDefaultName());
            }

            var projectName = $"{name}{framework}{finalName}";

            ProjectName = projectName;
            OutputPath = Path.Combine(_fixture.TestProjectsPath, projectName, projectName);

            var userSelection = new UserSelection
            {
                Framework = framework,
                ProjectType = projectTemplate.GetProjectType(),
            };

            AddLayoutItems(userSelection, projectTemplate);
            AddItem(userSelection, finalName, itemTemplate);

            await GenController.UnsafeGenerateAsync(userSelection);

            //Build solution
            var outputPath = Path.Combine(_fixture.TestProjectsPath, projectName);
            var result = BuildSolution(projectName, outputPath);

            //Assert
            Assert.True(result.exitCode.Equals(0), $"Solution {projectTemplate.Name} was not built successfully. {Environment.NewLine}Errors found: {GetErrorLines(result.outputFile)}.{Environment.NewLine}Please see {Path.GetFullPath(result.outputFile)} for more details.");

            //Clean
            Directory.Delete(outputPath, true);
        }

        [Theory, MemberData("GetProjectTemplates"), Trait("Type", "ProjectGeneration")]
        public async void GenerateAllPagesAndFeatures(string name, string framework, string projId)
        {
            var targetProjectTemplate = GenerationTestsFixture.Templates.Where(t => t.Identity == projId).FirstOrDefault();

            var projectName = $"{name}{framework}All";

            ProjectName = projectName;
            OutputPath = Path.Combine(_fixture.TestProjectsPath, projectName, projectName);

            var userSelection = new UserSelection
            {
                Framework = framework,
                ProjectType = targetProjectTemplate.GetProjectType(),
            };

            AddLayoutItems(userSelection, targetProjectTemplate);
            AddItems(userSelection, GetTemplates(framework, TemplateType.Page), GetDefaultName);
            AddItems(userSelection, GetTemplates(framework, TemplateType.Feature), GetDefaultName);

            await GenController.UnsafeGenerateAsync(userSelection);

            //Build solution
            var outputPath = Path.Combine(_fixture.TestProjectsPath, projectName);
            var result = BuildSolution(projectName, outputPath);

            //Assert
            Assert.True(result.exitCode.Equals(0), $"Solution {targetProjectTemplate.Name} was not built successfully. {Environment.NewLine}Errors found: {GetErrorLines(result.outputFile)}.{Environment.NewLine}Please see {Path.GetFullPath(result.outputFile)} for more details.");

            //Clean
            Directory.Delete(outputPath, true);
        }

        [Theory, MemberData("GetProjectTemplates"), Trait("Type", "ProjectGeneration")]
        public async void GenerateAllPagesAndFeaturesRandomNames(string name, string framework, string projId)
        {
            var targetProjectTemplate = GenerationTestsFixture.Templates.Where(t => t.Identity == projId).FirstOrDefault();
            var projectName = $"{name}{framework}AllRandom";

            ProjectName = projectName;
            OutputPath = Path.Combine(_fixture.TestProjectsPath, projectName, projectName);

            var userSelection = new UserSelection
            {
                Framework = framework,
                ProjectType = targetProjectTemplate.GetProjectType(),
            };

            AddLayoutItems(userSelection, targetProjectTemplate);
            AddItems(userSelection, GetTemplates(framework, TemplateType.Page), GetRandomName);
            AddItems(userSelection, GetTemplates(framework, TemplateType.Feature), GetRandomName);

            await GenController.UnsafeGenerateAsync(userSelection);

            //Build solution
            var outputPath = Path.Combine(_fixture.TestProjectsPath, projectName);
            var result = BuildSolution(projectName, outputPath);

            //Assert
            Assert.True(result.exitCode.Equals(0), $"Solution {targetProjectTemplate.Name} was not built successfully. {Environment.NewLine}Errors found: {GetErrorLines(result.outputFile)}.{Environment.NewLine}Please see {Path.GetFullPath(result.outputFile)} for more details.");

            //Clean
            Directory.Delete(outputPath, true);
        }

        private IEnumerable<ITemplateInfo> GetTemplates(string framework, TemplateType templateType)
        {
            return GenerationTestsFixture.Templates
                                              .Where(t => t.GetFrameworkList().Contains(framework) &&
                                              t.GetTemplateType() == templateType);
        }

        private void AddLayoutItems(UserSelection userSelection, ITemplateInfo projectTemplate)
        {
            var layouts = GenComposer.GetLayoutTemplates(userSelection.ProjectType, userSelection.Framework);

            foreach (var item in layouts)
            {
                AddItem(userSelection, item.Layout.name, item.Template);
            }
        }

        private void AddItems(UserSelection userSelection, IEnumerable<ITemplateInfo> templates, Func<ITemplateInfo, string> getName)
        {
            foreach (var template in templates)
            {
                if (template.GetMultipleInstance() || !AlreadyAdded(userSelection, template))
                {
                    var itemName = getName(template);

                    if (template.GetItemNameEditable())
                    {
                        itemName = Naming.Infer(_usedNames, itemName);
                    }

                    AddItem(userSelection, itemName, template);
                }
            }
        }

        private void AddItem(UserSelection userSelection, string itemName, ITemplateInfo template)
        {
            switch (template.GetTemplateType())
            {
                case TemplateType.Page:
                    userSelection.Pages.Add((itemName, template));
                    break;
                case TemplateType.Feature:
                    userSelection.Features.Add((itemName, template));
                    break;
            }

            _usedNames.Add(itemName);

            var dependencies = GenComposer.GetAllDependencies(template, userSelection.Framework);

            foreach (var item in dependencies)
            {
                if (!AlreadyAdded(userSelection, item))
                {
                    AddItem(userSelection, item.GetDefaultName(), item);
                }
            }
        }

        private static bool AlreadyAdded(UserSelection userSelection, ITemplateInfo item)
        {
            return (userSelection.Pages.Any(p => p.template.Identity == item.Identity) || userSelection.Features.Any(f => f.template.Identity == item.Identity));
        }

        public static IEnumerable<object[]> GetProjectTemplates()
        {
            GenContext.Bootstrap(new LocalTemplatesSource(), new FakeGenShell());

            var projectTemplates = GenerationTestsFixture.Templates.Where(t => t.GetTemplateType() == TemplateType.Project);

            foreach (var template in projectTemplates)
            {
                var frameworks = GenComposer.GetSupportedFx(template.Name);
                foreach (var framework in frameworks)
                {
                    yield return new object[] { template.Name, framework, template.Identity };
                }
            }
        }

        public static IEnumerable<object[]> GetPageAndFeatureTemplates()
        {
            GenContext.Bootstrap(new LocalTemplatesSource(), new FakeGenShell());
            var projectTemplates = GenerationTestsFixture.Templates.Where(t => t.GetTemplateType() == TemplateType.Project);

            foreach (var template in projectTemplates)
            {
                var frameworks = GenComposer.GetSupportedFx(template.Name);

                foreach (var framework in frameworks)
                {
                    var itemTemplates = GenerationTestsFixture.Templates.Where(t => t.GetFrameworkList().Contains(framework) && t.GetTemplateType() == TemplateType.Page || t.GetTemplateType() == TemplateType.Feature);

                    foreach (var itemTemplate in itemTemplates)
                    {
                        yield return new object[] { itemTemplate.Name, template.Name, framework, template.Identity, itemTemplate.Identity };
                    }
                }
            }
        }

        private static (int exitCode, string outputFile) BuildSolution(string solutionName, string outputPath)
        {
            var outputFile = Path.Combine(outputPath, $"_buildOutput_{solutionName}.txt");

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

        private string GetDefaultName(ITemplateInfo template)
        {
            return template.GetDefaultName();
        }

        private string GetRandomName(ITemplateInfo template)
        {
            return Path.GetRandomFileName().Replace(".", "");
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
