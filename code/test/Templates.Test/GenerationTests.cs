using Microsoft.TemplateEngine.Abstractions;
using Microsoft.Templates.Core;
using Microsoft.Templates.Core.Gen;
using Microsoft.Templates.Core.Locations;
using Microsoft.Templates.Test.Artifacts;
using Microsoft.Templates.Wizard;
using Microsoft.Templates.Wizard.Host;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using Xunit;


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
            GenContext.Bootstrap(new TemplatesRepository(new LocalTemplatesLocation()), new FakeGenShell());
        }

        [STAThread]
        [Theory, MemberData("GetProjectTemplates"), Trait("Type", "ProjectGeneration")]
        public void GenerateEmptyProject(string name, string framework, string projId)
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

                GenController.Generate(wizardState);

                //Build solution
                var outputPath = Path.Combine(fixture.TestProjectsPath, projectName);
                var outputFile = Path.Combine(outputPath, "_buildOutput.txt");
                int exitCode = BuildSolution(projectName, outputPath, outputFile);

                //Assert
                Assert.True(exitCode.Equals(0), string.Format("Solution {0} was not built successfully. Please see {1} for more details.", projectTemplate.Name, Path.GetFullPath(outputFile)));

                //Clean
                Directory.Delete(outputPath, true);
            }
        }

        [Theory, MemberData("GetProjectTemplates"), Trait("Type", "ProjectGeneration")]
        public void GenerateAllPages(string name, string framework, string projId)
        {
            var targetProjectTemplate = GenerationTestsFixture.Templates.Where(t => t.Identity == projId).FirstOrDefault();

            var projectName = $"{name}{framework}All";

            using (var context = GenContext.CreateNew(projectName, Path.Combine(fixture.TestPagesPath, projectName, projectName)))
            {
                var wizardState = new WizardState
                {
                    Framework = framework,
                    ProjectType = targetProjectTemplate.GetProjectType(),
                };

                AddLayoutItems(wizardState, targetProjectTemplate);
                AddItems(wizardState, GetTemplates(framework, TemplateType.Page));

                GenController.Generate(wizardState);

                //Build solution
                var outputPath = Path.Combine(fixture.TestPagesPath, projectName);
                var outputFile = Path.Combine(outputPath, "_buildOutput.txt");
                int exitCode = BuildSolution(projectName, outputPath, outputFile);

                //Assert
                Assert.True(exitCode.Equals(0), string.Format("Solution {0} was not built successfully. Please see {1} for more details.", targetProjectTemplate.Name, Path.GetFullPath(outputFile)));

                //Clean
                Directory.Delete(outputPath, true);
            }
        }

        [Theory, MemberData("GetProjectTemplates"), Trait("Type", "ProjectGeneration")]
        public void GenerateAllDevFeatures(string name,  string framework, string projId)
        {
            var targetProjectTemplate = GenerationTestsFixture.Templates.Where(t => t.Identity == projId).FirstOrDefault();

            var projectName = $"{name}{framework}All";

            using (var context = GenContext.CreateNew(projectName, Path.Combine(fixture.TestDevFeaturesPath, projectName, projectName)))
            {

                var wizardState = new WizardState
                {
                    Framework = framework,
                    ProjectType = targetProjectTemplate.GetProjectType(),
                };

                AddLayoutItems(wizardState, targetProjectTemplate);
                AddItems(wizardState, GetTemplates(framework, TemplateType.DevFeature));

                GenController.Generate(wizardState);

                //Build solution
                var outputPath = Path.Combine(fixture.TestDevFeaturesPath, projectName);
                var outputFile = Path.Combine(outputPath, "_buildOutput.txt");
                int exitCode = BuildSolution(projectName, outputPath, outputFile);

                //Assert
                Assert.True(exitCode.Equals(0), string.Format("Solution {0} was not built successfully. Please see {1} for more details.", targetProjectTemplate.Name, Path.GetFullPath(outputFile)));

                //Clean
                Directory.Delete(outputPath, true);
            }
        }

        [Theory, MemberData("GetProjectTemplates"), Trait("Type", "ProjectGeneration")]
        public void GenerateAllConsumerFeatures(string name, string framework, string projId)
        {
            var targetProjectTemplate = GenerationTestsFixture.Templates.Where(t => t.Identity == projId).FirstOrDefault();

            var projectName = $"{name}{framework}All";

            using (var context = GenContext.CreateNew(projectName, Path.Combine(fixture.TestConsumerFeaturesPath, projectName, projectName)))
            {
                var wizardState = new WizardState
                {
                    Framework = framework,
                    ProjectType = targetProjectTemplate.GetProjectType(),
                };

                AddLayoutItems(wizardState, targetProjectTemplate);
                AddItems(wizardState, GetTemplates(framework, TemplateType.ConsumerFeature));

                GenController.Generate(wizardState);

                //Build solution
                var outputPath = Path.Combine(fixture.TestConsumerFeaturesPath, projectName);
                var outputFile = Path.Combine(outputPath, "_buildOutput.txt");
                int exitCode = BuildSolution(projectName, outputPath, outputFile);

                //Assert
                Assert.True(exitCode.Equals(0), string.Format("Solution {0} was not built successfully. Please see {1} for more details.", targetProjectTemplate.Name, Path.GetFullPath(outputFile)));

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
                var template = GenerationTestsFixture.Templates.Where(t => t.Identity == layoutItem.templateIdentity).FirstOrDefault();
                if (template == null)
                {
                    throw new Exception($"Template {layoutItem.templateIdentity} could not be found");
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
                    wizardState.Pages.Add((itemName, template.Name));
                    break;
                case TemplateType.DevFeature:
                    wizardState.DevFeatures.Add((itemName, template.Name));
                    break;
                case TemplateType.ConsumerFeature:
                    wizardState.ConsumerFeatures.Add((itemName, template.Name));
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
                    yield return new object[] { template.Name,  framework, template.Identity };
                }
            }
        }

        private static int BuildSolution(string solutionName, string outputPath, string outputFile)
        {
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

            return process.ExitCode;
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
    }
}
