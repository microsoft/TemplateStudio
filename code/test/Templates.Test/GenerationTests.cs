using Microsoft.TemplateEngine.Abstractions;
using Microsoft.Templates.Core;
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

        public GenerationTests(GenerationTestsFixture fixture)
        {
            this.fixture = fixture;
        }


        [Theory, MemberData("GetProjectTemplates"), Trait("Type", "ProjectGeneration")]
        public void GenerateEmptyProject(string projId, string framework)
        {
            var projectTemplate = GenerationTestsFixture.Templates.Where(t => t.Identity == projId).FirstOrDefault();

            var projectName = $"{framework}{projectTemplate.GetProjectType()}";
            var usedPageNames = new List<string>();

            var generator = new GenController(new FakeGenShell(projectName, fixture.TestProjectsPath, new TextBlock()));
            var wizardState = new WizardState
            {
                Framework = framework,
                ProjectType = projectTemplate.GetProjectType(),
            };

            AddLayoutItems(wizardState, projectTemplate, usedPageNames);

            generator.Generate(wizardState);

            //Build solution
            var outputPath = Path.Combine(fixture.TestProjectsPath, projectName);
            var outputFile = Path.Combine(outputPath, "_buildOutput.txt");
            int exitCode = BuildSolution(projectName, outputPath, outputFile);

            //Assert
            Assert.True(exitCode.Equals(0), string.Format("Solution {0} was not built successfully. Please see {1} for more details.", projectTemplate.Name, Path.GetFullPath(outputFile)));

            //Clean
            Directory.Delete(outputPath, true);

        }

        
        [Theory, MemberData("GetPageTemplates"), Trait("Type", "PageGeneration")]
        public void GeneratePage(string pageId, string projId, string framework)
        {
            var targetProjectTemplate = GenerationTestsFixture.Templates.Where(t => t.Identity == projId).FirstOrDefault();
            var pageTemplate = GenerationTestsFixture.Templates.Where(t => t.Identity == pageId).FirstOrDefault();

            var projectName = $"{framework}{Naming.Infer(new List<string>(), pageTemplate.Name)}";
            var usedPageNames = new List<string>();

            var generator = new GenController(new FakeGenShell(projectName, fixture.TestPagesPath, new TextBlock()));

            var wizardState = new WizardState
            {
                Framework = framework,
                ProjectType = targetProjectTemplate.GetProjectType(),
            };

            AddLayoutItems(wizardState, targetProjectTemplate, usedPageNames);

            var pageName = Naming.Infer(usedPageNames, pageTemplate.Name);
            wizardState.Pages.Add((pageName, pageTemplate.Name));

            generator.Generate(wizardState);

            //Build solution
            var outputPath = Path.Combine(fixture.TestPagesPath, projectName);
            var outputFile = Path.Combine(outputPath, "_buildOutput.txt");
            int exitCode = BuildSolution(projectName, outputPath, outputFile);

            //Assert
            Assert.True(exitCode.Equals(0), string.Format("Solution {0} was not built successfully. Please see {1} for more details.", targetProjectTemplate.Name, Path.GetFullPath(outputFile)));

            //Clean
            Directory.Delete(outputPath, true);

        }

        [Theory, MemberData("GetProjectTemplates"), Trait("Type", "ProjectGeneration")]
        public void GenerateProjectWithAllPages(string projId, string framework)
        {
            var targetProjectTemplate = GenerationTestsFixture.Templates.Where(t => t.Identity == projId).FirstOrDefault();

            var projectName = $"{framework}{targetProjectTemplate.GetProjectType()}All";
            var usedPageNames = new List<string>();
            var generator = new GenController(new FakeGenShell(projectName, fixture.TestProjectsPath, new TextBlock()));

            var wizardState = new WizardState
            {
                Framework = framework,
                ProjectType = targetProjectTemplate.GetProjectType(),
            };

            AddLayoutItems(wizardState, targetProjectTemplate, usedPageNames);

            var pageTemplates = GenerationTestsFixture.Templates
              .Where(t => t.GetFrameworkList().Contains(framework) &&
              t.GetType() == t.GetType() &&
              t.GetTemplateType() == TemplateType.Page);

            wizardState.Pages.AddRange(GetPages(pageTemplates, usedPageNames));

            generator.Generate(wizardState);

            //Build solution
            var outputPath = Path.Combine(fixture.TestProjectsPath, projectName);
            var outputFile = Path.Combine(outputPath, "_buildOutput.txt");
            int exitCode = BuildSolution(projectName, outputPath, outputFile);

            //Assert
            Assert.True(exitCode.Equals(0), string.Format("Solution {0} was not built successfully. Please see {1} for more details.", targetProjectTemplate.Name, Path.GetFullPath(outputFile)));

            //Clean
            Directory.Delete(outputPath, true);
        }



        private static void AddLayoutItems(WizardState wizardState, ITemplateInfo projectTemplate,  IEnumerable<string> usedNames)
        {
            var pages = new List<(string name, string templateName)>();
            var layouts = projectTemplate.GetLayout();

            foreach (var layoutItem in layouts)
            {
                var template = GenerationTestsFixture.Templates.Where(t => t.Identity == layoutItem.templateIdentity).FirstOrDefault();
                if (template != null)
                {
                    if (template.GetTemplateType() == TemplateType.Page)
                    {
                        var itemName = Naming.Infer(usedNames, layoutItem.name);
                        wizardState.Pages.Add((itemName, template.Name));
                    }
                }

            }
        }

        private static IEnumerable<(string name, string templateName)> GetPages(IEnumerable<ITemplateInfo> pageTemplates, IEnumerable<string> usedNames)
        {
            var pages = new List<(string name, string templateName)>();
            foreach (var pageTemplate in pageTemplates)
            {
                var pageName = Naming.Infer(usedNames, pageTemplate.Name);
                pages.Add((pageName, pageTemplate.Name));              
            }
            return pages;
        }

    
        public static IEnumerable<object[]> GetPageTemplates()
        {
            var pageTemplates = GenerationTestsFixture.Templates.Where(t => t.GetTemplateType() == TemplateType.Page);

            foreach (var template in pageTemplates)
            {
                foreach (var framework in template.GetFrameworkList())
                {
                    var projectTemplate = GenerationTestsFixture.Templates.Where(t => t.GetFrameworkList().Contains(framework) && t.GetTemplateType() == TemplateType.Project).FirstOrDefault();
                    if (projectTemplate != null)
                    {
                        yield return new object[] { template.Identity, projectTemplate.Identity, framework };
                    }
                }
            }
        }

        public static IEnumerable<object[]> GetProjectTemplates()
        {
            var projectTemplates = GenerationTestsFixture.Templates.Where(t => t.GetTemplateType() == TemplateType.Project);
            foreach (var template in projectTemplates)
            {
                foreach (var framework in template.GetFrameworkList())
                {
                    yield return new object[] { template.Identity, framework };
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
