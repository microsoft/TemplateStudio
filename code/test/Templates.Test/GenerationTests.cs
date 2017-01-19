using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using Microsoft.TemplateEngine.Abstractions;
using Microsoft.TemplateEngine.Edge.Settings;
using Microsoft.TemplateEngine.Edge.Template;
using Microsoft.Templates.Core;
using Microsoft.Templates.Core.Locations;
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
        public void GenerateProject(string projectTemplateName)
        {
            var projectTemplate = GenerationTestsFixture.Templates.Where(t => t.Name == projectTemplateName).FirstOrDefault();

            //Generate app
            string outputPath = GenerateProject(fixture.TestProjectsPath, projectTemplateName, projectTemplate);

            //Build solution
            var outputFile = Path.Combine(outputPath, "_buildOutput.txt");
            int exitCode = BuildSolution(projectTemplateName, outputPath, outputFile);

            //Assert
            Assert.True(exitCode.Equals(0), string.Format("Solution {0} was not built successfully. Please see {1} for more details.", projectTemplate.Name, Path.GetFullPath(outputFile)));

            //Clean
            Directory.Delete(outputPath, true);
        }

       

        [Theory, MemberData("GetPageTemplates"), Trait("Type", "PageGeneration")]
        public void GeneratePage(string pageTemplateName, string targetProjectTemplateName)
        {
            //Set up test repos
            var targetProjectTemplate = GenerationTestsFixture.Templates.Where(t => t.Name == targetProjectTemplateName).FirstOrDefault();
            var pageTemplate = GenerationTestsFixture.Templates.Where(t => t.Name == pageTemplateName).FirstOrDefault();

            //Generate app
            var projectOutputPath = GenerateProject(fixture.TestPagesPath, targetProjectTemplateName, targetProjectTemplate);

            //Generate page
            var pageOutputPath = Path.Combine(projectOutputPath, targetProjectTemplateName);
            var page = TemplateCreator.InstantiateAsync(pageTemplate, pageTemplateName, null, pageOutputPath, new Dictionary<string, string>(), true).Result;

            //Add file to proj
            AddToProject(targetProjectTemplateName, pageOutputPath, page);

            //Build solution
            var outputFile = Path.Combine(projectOutputPath, "_buildOutput.txt");
            int exitCode = BuildSolution(targetProjectTemplateName, projectOutputPath, outputFile);

            //Assert
            Assert.True(exitCode.Equals(0), string.Format("Solution {0} with page {1} was not built successfully. Please see {2} for more details.", targetProjectTemplateName, pageTemplateName, Path.GetFullPath(outputFile)));

            //Clean
            Directory.Delete(projectOutputPath, true);

        }

        public static IEnumerable<object[]> GetPageTemplates()
        {
            var pageTemplates = GenerationTestsFixture.Templates.Where(t => t.GetTemplateType() == TemplateType.Page);

            foreach (var template in pageTemplates)
            {
                var projectTemplate = GenerationTestsFixture.Templates.Where(t => t.GetFramework() == template.GetFramework() && t.GetTemplateType() == TemplateType.Project).FirstOrDefault();
                if (projectTemplate != null)
                {
                    yield return new object[] { template.Name, projectTemplate.Name };
                }
            }
        }

        public static IEnumerable<object[]> GetProjectTemplates()
        {
            var projectTemplates = GenerationTestsFixture.Templates.Where(t => t.GetTemplateType() == TemplateType.Project);
            foreach (var template in projectTemplates)
            {
                yield return new object[] { template.Name };
            }
        }

        private string GenerateProject(string testPath, string projectName, ITemplateInfo projectTemplate)
        {
            var outputPath = Path.Combine(testPath, projectName);
            var result = TemplateCreator.InstantiateAsync(projectTemplate, projectName, null, outputPath, new Dictionary<string, string>(), true).Result;
            return outputPath;
        }

        private static void AddToProject(string projectName, string projectPath, TemplateCreationResult page)
        {
            var projectFile = Path.GetFullPath(projectPath + @"\\" + projectName + ".csproj");
            var msbuildProj = MsBuildProject.Load(projectFile);
            if (msbuildProj != null)
            {
                foreach (var output in page.PrimaryOutputs)
                {
                    if (!string.IsNullOrWhiteSpace(output.Path))
                    {
                        var itemPath = Path.GetFullPath(Path.Combine(projectPath, output.Path));
                        msbuildProj.AddItem(itemPath);
                    }
                }

                msbuildProj.Save();
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
