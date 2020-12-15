// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using Microsoft.TemplateEngine.Abstractions;
using Microsoft.Templates.Core;
using Microsoft.Templates.Core.Extensions;
using Microsoft.Templates.Core.Gen;
using Microsoft.Templates.Core.Naming;
using Microsoft.Templates.Fakes;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Templates.Test
{
    public abstract class BaseGenAndBuildFixture
    {
        protected const string All = "all";

        private readonly string _emptyBackendFramework = string.Empty;

        public abstract string GetTestRunPath();

        public abstract void InitializeFixture(IContextProvider contextProvider, string framework = "");

        public string TestProjectsPath => Path.GetFullPath(Path.Combine(GetTestRunPath(), "Proj"));

        public string TestNewItemPath => Path.GetFullPath(Path.Combine(GetTestRunPath(), "RightClick"));

        public IEnumerable<ITemplateInfo> Templates() => GenContext.ToolBox.Repo.GetAll();


        public UserSelection SetupProject(string projectType, string framework, string platform, string language, Func<TemplateInfo, string> getName = null)
        {
            var userSelection = new UserSelection(projectType, framework, _emptyBackendFramework, platform, language);

            var layouts = GenContext.ToolBox.Repo.GetLayoutTemplates(userSelection.Platform, userSelection.ProjectType, userSelection.FrontEndFramework, userSelection.BackEndFramework);

            foreach (var item in layouts)
            {
                if (getName == BaseGenAndBuildFixture.GetDefaultName || getName == null)
                {
                    AddItem(userSelection, item.Layout.Name, item.Template);
                }
                else
                {
                    AddItem(userSelection, item.Template, getName);
                }
            }

            userSelection.HomeName = userSelection.Pages.FirstOrDefault().Name;

            return userSelection;
        }

        public void AddItems(UserSelection userSelection, IEnumerable<TemplateInfo> templates, Func<TemplateInfo, string> getName, bool includeMultipleInstances = false)
        {
            foreach (var template in templates)
            {
                AddItem(userSelection, template, getName);
                // Add multiple pages if supported to check these are handled the same
                if (includeMultipleInstances && template.MultipleInstance)
                {
                    AddItem(userSelection, template, getName);
                }
            }
        }

        public void AddItem(UserSelection userSelection, TemplateInfo template, Func<TemplateInfo, string> getName)
        {
            if (template.MultipleInstance || !AlreadyAdded(userSelection, template))
            {
                var itemName = getName(template);
                var usedNames = userSelection.Pages.Select(p => p.Name)
                    .Concat(userSelection.Features.Select(f => f.Name))
                    .Concat(userSelection.Services.Select(f => f.Name))
                    .Concat(userSelection.Testing.Select(f => f.Name));

                if (template.ItemNameEditable)
                {
                    var itemBameValidationService = new ItemNameService(GenContext.ToolBox.Repo.ItemNameValidationConfig, () => usedNames);
                    itemName = itemBameValidationService.Infer(itemName);
                }
                else
                {
                    itemName = template.DefaultName;
                }
                
                AddItem(userSelection, itemName, template);
            }
        }

        public void AddItem(UserSelection userSelection, string itemName, TemplateInfo template)
        {
            var selectedTemplate = new UserSelectionItem { Name = itemName, TemplateId = template.TemplateId };

            foreach (var item in template.Dependencies)
            {
                if (!AlreadyAdded(userSelection, item))
                {
                    AddItem(userSelection, item.DefaultName, item);
                }
            }

            if (template.Requirements.Count() > 0 && !userSelection.Items.Any(u => template.Requirements.Select(r => r.TemplateId).Contains(u.TemplateId)))
            {
                AddItem(userSelection, template.Requirements.FirstOrDefault().DefaultName, template.Requirements.FirstOrDefault());
            }

            userSelection.Add(selectedTemplate, template.TemplateType);
        }

        public (int exitCode, string outputFile) BuildMsixBundle(string projectName, string outputPath, string packagingProjectName, string packagingProjectExtension, string batfile)
        {
            var outputFile = Path.Combine(outputPath, $"_buildOutput_{projectName}.txt");

            var solutionFile = Path.GetFullPath(outputPath + @"\" + projectName + ".sln");
            var projectFile = Path.GetFullPath(outputPath + @"\" + packagingProjectName + @"\" + packagingProjectName + $".{packagingProjectExtension}");

            Console.Out.WriteLine();
            Console.Out.WriteLine($"### > Ready to start building");
            Console.Out.Write($"### > Running following command: {GetPath(batfile)} \"{solutionFile}\"");

            var startInfo = new ProcessStartInfo(GetPath(batfile))
            {
                Arguments = $"\"{solutionFile}\" \"{projectFile}\" ",
                UseShellExecute = false,
                RedirectStandardOutput = true,
                CreateNoWindow = false,
                WorkingDirectory = outputPath,
            };

            var process = Process.Start(startInfo);

            File.WriteAllText(outputFile, process.StandardOutput.ReadToEnd(), Encoding.UTF8);

            process.WaitForExit();

            return (process.ExitCode, outputFile);
        }

        public (int exitCode, string outputFile, string resultFile) RunWackTestOnMsixBundle(string bundleFilePath, string outputPath)
        {
            var outputFile = Path.Combine(outputPath, $"_wackOutput_{Path.GetFileName(bundleFilePath)}.txt");
            var resultFile = Path.Combine(outputPath, "_wackresults.xml");

            Console.Out.WriteLine();
            Console.Out.WriteLine("### > Ready to run WACK test");
            Console.Out.Write($"### > Running following command: {GetPath("bat\\RunWackTest.bat")} \"{bundleFilePath}\" \"{resultFile}\"");

            var startInfo = new ProcessStartInfo(GetPath("bat\\RunWackTest.bat"))
            {
                Arguments = $"\"{bundleFilePath}\" \"{resultFile}\" ",
                UseShellExecute = false,
                RedirectStandardOutput = true,
                CreateNoWindow = false,
                WorkingDirectory = outputPath,
            };

            var process = Process.Start(startInfo);

            File.WriteAllText(outputFile, process.StandardOutput.ReadToEnd(), Encoding.UTF8);

            process.WaitForExit();

            return (process.ExitCode, outputFile, resultFile);
        }

        private bool AlreadyAdded(UserSelection userSelection, TemplateInfo item)
        {
            return userSelection.Pages.Any(p => p.TemplateId == item.TemplateId)
                || userSelection.Features.Any(f => f.TemplateId == item.TemplateId)
                || userSelection.Services.Any(f => f.TemplateId == item.TemplateId)
                || userSelection.Testing.Any(f => f.TemplateId == item.TemplateId);
        }

        public static string GetDefaultName(TemplateInfo template)
        {
            return template.DefaultName;
        }

#pragma warning disable RECS0154 // Parameter is never used - but used by method which takes an action which is passed a template
        public static string GetRandomName(TemplateInfo template)
#pragma warning restore RECS0154 // Parameter is never used
        {
            for (int i = 0; i < 10; i++)
            {
                var itemNameValidationService = new ItemNameService(GenContext.ToolBox.Repo.ItemNameValidationConfig, () => new string[] { });
                var randomName = Path.GetRandomFileName().Replace(".", string.Empty);
                if (itemNameValidationService.Validate(randomName).IsValid)
                {
                    return randomName;
                }
            }

            throw new ApplicationException("No valid randomName could be generated");
        }

        public (int exitCode, string outputFile) BuildSolutionUwp(string solutionName, string outputPath, string platform)
        {
            return BuildSolution(solutionName, outputPath, platform, "bat\\Uwp\\RestoreAndBuild.bat", "Debug", "x86");
        }

        public (int exitCode, string outputFile) BuildSolutionWinUI(string solutionName, string outputPath, string platform)
        {
            return BuildSolution(solutionName, outputPath, platform, "bat\\WinUI\\RestoreAndBuild.bat", "Debug", "x86");
        }

        public (int exitCode, string outputFile) BuildSolutionWpf(string solutionName, string outputPath, string platform)
        {
            var isXamlIslandProj = Directory.EnumerateDirectories(outputPath, "*XamlIsland").Count() > 0;

            if (isXamlIslandProj)
            {
                return BuildSolution(solutionName, outputPath, platform, "bat\\Wpf\\RestoreAndBuild.bat", "Debug", "x86");
            }
            else
            {
                return BuildSolution(solutionName, outputPath, platform, "bat\\Wpf\\RestoreAndBuild.bat", "Debug", "Any CPU");
            }
        }

        public (int exitCode, string outputFile) BuildSolutionWpfWithMsix(string solutionName, string outputPath, string platform)
        {
            return BuildSolution(solutionName, outputPath, platform, "bat\\Wpf\\RestoreAndBuildWithMsix.bat", "Debug", "x86");
        }

        private (int exitCode, string outputFile) BuildSolution(string solutionName, string outputPath, string platform, string batfile, string config, string buildPlatform)
        {
            var outputFile = Path.Combine(outputPath, $"_buildOutput_{solutionName}.txt");

            // Build
            var solutionFile = Path.GetFullPath(outputPath + @"\" + solutionName + ".sln");

            var batPath = Path.GetDirectoryName(GetPath(batfile));

            Console.Out.WriteLine();
            Console.Out.WriteLine($"### > Ready to start building");
            Console.Out.Write($"### > Running following command: {GetPath(batfile)} \"{solutionFile}\" {buildPlatform} {config} {batPath}");

            var startInfo = new ProcessStartInfo(GetPath(batfile))
            {
                Arguments = $"\"{solutionFile}\" \"{buildPlatform}\" \"{config}\" \"{batPath}\"",
                UseShellExecute = false,
                RedirectStandardOutput = true,
                CreateNoWindow = false,
                WorkingDirectory = outputPath,
            };

            var process = Process.Start(startInfo);

            File.WriteAllText(outputFile, process.StandardOutput.ReadToEnd(), Encoding.UTF8);

            process.WaitForExit();

            return (process.ExitCode, outputFile);
        }

        public (int exitCode, string outputFile) RunTests(string projectName, string outputPath)
        {
            var outputFile = Path.Combine(outputPath, $"_testOutput_{projectName}.txt");

            var solutionFile = Path.GetFullPath(outputPath + @"\" + projectName + ".sln");

            const string batFile = "bat\\Uwp\\RunTests.bat";

            // Just run the tests against code in the core library. Can't run UI related/dependent code from the cmd line / on the server
            var mstestPath = $"\"{outputPath}\\{projectName}.Core.Tests.MSTest\\bin\\Debug\\netcoreapp3.1\\{projectName}.Core.Tests.MSTest.dll\" ";
            var nunitPath = $"\"{outputPath}\\{projectName}.Core.Tests.NUnit\\bin\\Debug\\netcoreapp3.1\\{projectName}.Core.Tests.NUnit.dll\" ";
            var xunitPath = $"\"{outputPath}\\{projectName}.Core.Tests.xUnit\\bin\\Debug\\netcoreapp3.1\\{projectName}.Core.Tests.xUnit.dll\" ";

            var batPath = Path.GetDirectoryName(GetPath(batFile));

            var startInfo = new ProcessStartInfo(GetPath(batFile))
            {
                Arguments = $"{mstestPath} {nunitPath} {xunitPath}",
                UseShellExecute = false,
                RedirectStandardOutput = true,
                CreateNoWindow = false,
                WorkingDirectory = outputPath,
            };

            var process = Process.Start(startInfo);

            File.WriteAllText(outputFile, process.StandardOutput.ReadToEnd(), Encoding.UTF8);

            process.WaitForExit();

            return (process.ExitCode, outputFile);
        }

        public string GetErrorLines(string filePath)
        {
            Regex re = new Regex(@"^.*error .*$", RegexOptions.Multiline & RegexOptions.IgnoreCase);
            var outputLines = File.ReadAllLines(filePath);
            var errorLines = outputLines.Where(l => re.IsMatch(l));

            return errorLines.Any() ? errorLines.Aggregate((i, j) => i + Environment.NewLine + j) : string.Empty;
        }

        public string GetTestSummary(string filePath)
        {
            var outputLines = File.ReadAllLines(filePath);
            var summaryLines = outputLines.Where(l => l.StartsWith("Total tests", StringComparison.OrdinalIgnoreCase) || l.StartsWith("Test ", StringComparison.OrdinalIgnoreCase));

            return summaryLines.Any() ? summaryLines.Aggregate((i, j) => i + Environment.NewLine + j) : string.Empty;
        }

        private static string GetPath(string fileName)
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

        public void Dispose()
        {
            if (Directory.Exists(GetTestRunPath()))
            {
                CleanUpOldTests();

                if ((!Directory.Exists(TestProjectsPath) || !Directory.EnumerateDirectories(TestProjectsPath).Any())
                 && (!Directory.Exists(TestNewItemPath) || !Directory.EnumerateDirectories(TestNewItemPath).Any()))
                {
                    Directory.Delete(GetTestRunPath(), true);
                }
            }
        }

        private void CleanUpOldTests()
        {
            var rootDir = new DirectoryInfo(GetTestRunPath()).Parent;

            var oldDirectories = rootDir.EnumerateDirectories().Where(d => d.CreationTime < DateTime.Now.AddDays(-7));
            foreach (var dir in oldDirectories)
            {
                try
                {
                    dir.Delete(true);
                }
                catch
                {
                    // This can happen when a test run as admin (such as some WinAppDriver tests) failed
                    // but now running a test when not admin and can't tidy up the files previously left behind.
                    Assert.Fail($"There was an exception while tidying up old test files. Manually delete the contents of '{dir.FullName}'.");
                }
            }
        }

        public static void SetCurrentLanguage(string language)
        {
            GenContext.SetCurrentLanguage(language);
            var fakeShell = GenContext.ToolBox.Shell as FakeGenShell;
            fakeShell.SetCurrentLanguage(language);
        }

        public static void SetCurrentPlatform(string platform)
        {
            GenContext.SetCurrentPlatform(platform);
            var fakeShell = GenContext.ToolBox.Shell as FakeGenShell;
            fakeShell.SetCurrentPlatform(platform);
        }

        protected static IEnumerable<object[]> GetPageAndFeatureTemplates(string frameworkFilter, string language = ProgrammingLanguages.CSharp, string platform = Platforms.Uwp, string excludedItem = "")
        {
            List<object[]> result = new List<object[]>();

            SetCurrentLanguage(language);

            SetCurrentPlatform(platform);

            var projectTypes = GenContext.ToolBox.Repo.GetProjectTypes(platform)
                                                        .Where(m => !string.IsNullOrEmpty(m.Description))
                                                        .Select(m => m.Name);

            foreach (var projectType in projectTypes)
            {
                var targetFrameworks = GenContext.ToolBox.Repo.GetFrontEndFrameworks(platform, projectType)
                                                                .Where(m => m.Name == frameworkFilter)
                                                                .Select(m => m.Name).ToList();

                foreach (var framework in targetFrameworks)
                {
                    var itemTemplates = GenContext.ToolBox.Repo.GetAll()
                        .Where(t =>
                        (t.GetFrontEndFrameworkList().Contains(framework) || t.GetFrontEndFrameworkList().Contains(All))
                        && t.GetTemplateType().IsItemTemplate()
                        && t.GetPlatform() == platform
                        && t.GetLanguage() == language
                        && t.Identity != excludedItem
                        && !t.GetIsHidden());

                    foreach (var itemTemplate in itemTemplates)
                    {
                        result.Add(new object[]
                        {
                            itemTemplate.Name,
                            projectType,
                            framework,
                            platform,
                            itemTemplate.Identity,
                            language,
                        });
                    }
                }    
            }

            return result;
        }

        protected static IEnumerable<object[]> GetVBProjectTemplates()
        {
            List<object[]> result = new List<object[]>();

            var platform = Platforms.Uwp;

            var projectTemplates =
               GenContext.ToolBox.Repo.GetAll().Where(
                   t => t.GetTemplateType() == TemplateType.Project
                    && t.GetLanguage() == ProgrammingLanguages.VisualBasic);

            foreach (var projectTemplate in projectTemplates)
            {
                var projectTypeList = projectTemplate.GetProjectTypeList();

                foreach (var projectType in projectTypeList)
                {
                    var frameworks = GenContext.ToolBox.Repo.GetFrontEndFrameworks(platform, projectType)
                                            .Select(m => m.Name).ToList();

                    foreach (var framework in frameworks)
                    {
                        result.Add(new object[] { projectType, framework, platform });
                    }
                }
            }

            return result;
        }

        protected static IEnumerable<object[]> GetAllProjectTemplates()
        {
            List<object[]> result = new List<object[]>();
            foreach (var language in ProgrammingLanguages.GetAllLanguages())
            {
                SetCurrentLanguage(language);

                foreach (var platform in Platforms.GetAllPlatforms())
                {
                    SetCurrentPlatform(platform);

                    var projectTypes = GenContext.ToolBox.Repo.GetProjectTypes(platform)
                                                              .Where(m => !string.IsNullOrEmpty(m.Description))
                                                              .Select(m => m.Name);

                    foreach (var projectType in projectTypes)
                    {

                        var targetFrameworks = GenContext.ToolBox.Repo.GetFrontEndFrameworks(platform, projectType)
                                                                      .Select(m => m.Name).ToList();

                        foreach (var framework in targetFrameworks)
                        {
                            result.Add(new object[] { projectType, framework, platform, language });
                        }
                    }
                }
            }

            return result;
        }
    }
}
