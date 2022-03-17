// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using Microsoft.TemplateEngine.Abstractions;
using Microsoft.Templates.Core;
using Microsoft.Templates.Core.Extensions;
using Microsoft.Templates.Core.Gen;
using Microsoft.Templates.Core.Locations;
using Microsoft.Templates.Core.Naming;
using Microsoft.Templates.Fakes.GenShell;
using Microsoft.Templates.UI;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Templates.Test
{
    public abstract class BaseGenAndBuildFixture
    {
        protected const string All = "all";

        private static Dictionary<string, bool> syncExecuted = new Dictionary<string, bool>();

        public virtual TemplatesSource Source => null;

        public abstract string GetTestRunPath();

        public abstract void InitializeFixture(IContextProvider contextProvider, string framework = "");

        public string TestProjectsPath => Path.GetFullPath(Path.Combine(GetTestRunPath(), "Proj"));

        public string TestNewItemPath => Path.GetFullPath(Path.Combine(GetTestRunPath(), "RightClick"));

        public IEnumerable<ITemplateInfo> Templates() => GenContext.ToolBox.Repo.GetAll();

        public UserSelection SetupProject(UserSelectionContext context, Func<TemplateInfo, string> getName = null)
        {
            var userSelection = new UserSelection(context);

            var layouts = GenContext.ToolBox.Repo.GetLayoutTemplates(context);

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

            userSelection.HomeName = userSelection.Pages.FirstOrDefault()?.Name ?? string.Empty;

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
            var nugetExecutable = GetPath("nuget\\nuget.exe");

            Console.Out.WriteLine();
            Console.Out.WriteLine($"### > Ready to start building");
            Console.Out.Write($"### > Running following command: {GetPath(batfile)} \"{solutionFile}\"");

            var startInfo = new ProcessStartInfo(GetPath(batfile))
            {
                Arguments = $"\"{solutionFile}\" \"{projectFile}\" \"{nugetExecutable}\"",
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

        public static string ShortFrameworkName(string framework)
        {
            switch (framework)
            {
                case Frameworks.Prism:
                    return "P";
                case Frameworks.CodeBehind:
                    return "CB";
                case Frameworks.MVVMToolkit:
                    return "MTM";
                case "":
                    return "_";
                default:
                    return framework;
            }
        }

        public (int exitCode, string outputFile) BuildSolutionUwp(string solutionName, string outputPath, string platform)
        {
            return BuildSolution(solutionName, outputPath, platform, "bat\\Uwp\\RestoreAndBuild.bat", "Debug", "x86");
        }

        public (int exitCode, string outputFile) BuildSolutionWinUI(string solutionName, string outputPath, string platform, string language)
        {
            if (language == ProgrammingLanguages.Cpp)
            {
                return BuildSolution(solutionName, outputPath, platform, "bat\\WinUI\\RestoreAndBuildCpp.bat", "Debug", "x86");
            }

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

            var nugetExecutable = GetPath("nuget\\nuget.exe");

            var vsRoot = GetVsInstallRoot();

            Console.Out.WriteLine();
            Console.Out.WriteLine($"### > Ready to start building");
            Console.Out.Write($"### > Running following command: {GetPath(batfile)} \"{vsRoot}\" \"{solutionFile}\" {buildPlatform} {config}");

            var startInfo = new ProcessStartInfo(GetPath(batfile))
            {
                Arguments = $"\"{vsRoot}\" \"{solutionFile}\" \"{buildPlatform}\" \"{config}\" \"{nugetExecutable}\"",
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

        public static string GetVsInstallRoot()
        {
            var VsEditions = new List<string> { "Enterprise", "Preview", "Professional", "Community" };

            // Try both of these to allow for wonderful inconsistencies in resolving 
            var progFileLocations = new List<string> { Environment.GetEnvironmentVariable("ProgramW6432"), Environment.GetEnvironmentVariable("ProgramFiles") };

            var basePath = "{0}\\Microsoft Visual Studio\\2022\\{1}\\";

            foreach (var progFiles in progFileLocations)
            {
                foreach (var edition in VsEditions)
                {
                    var possPath = string.Format(basePath, progFiles, edition);

                    if (Directory.Exists(possPath))
                    {
                        return possPath;
                    }
                }
            }

            throw new DirectoryNotFoundException("Visual Studio 2022 install location could not be found.");
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

        public static void InitializeTemplates(TemplatesSource source, string programmingLanguage)
        {
            if (syncExecuted.ContainsKey(source.Id) && syncExecuted[source.Id] == true)
            {
                return;
            }

            GenContext.Bootstrap(source, new FakeGenShell(source.Platform, programmingLanguage), source.Platform, programmingLanguage, TestConstants.TemplateVersionNumber);

            syncExecuted.Add(source.Id, true);
        }

        public static IEnumerable<object[]> GetProjectTemplates(TemplatesSource templateSource, string frameworkFilter, string programmingLanguage, string selectedPlatform)
        {
            //InitializeTemplates(new LocalTemplatesSource(null, ShortFrameworkName(frameworkFilter)));
            InitializeTemplates(templateSource, programmingLanguage);

            List<object[]> result = new List<object[]>();

            var languagesOfInterest = ProgrammingLanguages.GetAllLanguages().ToList();

            if (!string.IsNullOrWhiteSpace(programmingLanguage))
            {
                languagesOfInterest.Clear();
                languagesOfInterest.Add(programmingLanguage);
            }

            var platformsOfInterest = Platforms.GetAllPlatforms().ToList();

            if (!string.IsNullOrWhiteSpace(selectedPlatform))
            {
                platformsOfInterest.Clear();
                platformsOfInterest.Add(selectedPlatform);
            }

            foreach (var language in languagesOfInterest)
            {
                SetCurrentLanguage(language);
                foreach (var platform in platformsOfInterest)
                {
                    SetCurrentPlatform(platform);

                    if (platform == Platforms.WinUI)
                    {
                        ////var appModels = AppModels.GetAllAppModels().ToList();
                        ////foreach (var appModel in appModels)
                        ////{
                        ////    if (appModel == AppModels.Desktop)
                        ////    {
                        result.AddRange(GetContextOptions(frameworkFilter, language, platform, AppModels.Desktop));
                        ////    }
                        ////}
                    }
                    else
                    {
                        result.AddRange(GetContextOptions(frameworkFilter, language, platform, string.Empty));
                    }
                }
            }

            return result;
        }

        protected static List<object[]> GetContextOptions(string frameworkFilter, string language, string platform, string appModel)
        {
            List<object[]> result = new List<object[]>();

            var context = new UserSelectionContext(language, platform);
            if (!string.IsNullOrEmpty(appModel))
            {
                context.AddAppModel(appModel);
            }

            var projectTypes = GenContext.ToolBox.Repo.GetProjectTypes(context)
                    .Where(m => !string.IsNullOrEmpty(m.Description))
                    .Select(m => m.Name);

            foreach (var projectType in projectTypes)
            {
                context.ProjectType = projectType;
                var targetFrameworks = GenContext.ToolBox.Repo.GetFrontEndFrameworks(context)
                                            .Where(m => m.Name == frameworkFilter)
                                            .Select(m => m.Name)
                                            .ToList();

                foreach (var framework in targetFrameworks)
                {
                    if (!string.IsNullOrEmpty(appModel))
                    {
                        result.Add(new object[] { projectType, framework, platform, language, appModel });
                    }
                    else
                    {
                        result.Add(new object[] { projectType, framework, platform, language });
                    }
                }
            }

            return result;
        }

        private static bool IsMatchPropertyBag(ITemplateInfo info, Dictionary<string, string> propertyBag)
        {
            if (propertyBag == null || !propertyBag.Any())
            {
                return true;
            }

            return propertyBag.All(p =>
                info.GetPropertyBagValuesList(p.Key).Contains(p.Value, StringComparer.OrdinalIgnoreCase) ||
                info.GetPropertyBagValuesList(p.Key).Contains(All, StringComparer.OrdinalIgnoreCase));
        }

        protected static IEnumerable<object[]> GetPageAndFeatureTemplates(string frameworkFilter, string language = ProgrammingLanguages.CSharp, string platform = Platforms.Uwp, string excludedItem = "")
        {
            List<object[]> result = new List<object[]>();

            SetCurrentLanguage(language);

            SetCurrentPlatform(platform);

            if (platform == Platforms.WinUI)
            {
                ////var appModels = AppModels.GetAllAppModels().ToList();
                ////foreach (var appModel in appModels)
                ////{
                ////    if (appModel == AppModels.Desktop)
                ////    {
                // Any use of appmodels (for distinguishing between UWP & Desktop -- for WinUI) has been dropped
                result.AddRange(GetTemplateOptions(frameworkFilter, language, platform, excludedItem, AppModels.Desktop));
                ////    }
                ////}
            }
            else
            {
                result.AddRange(GetTemplateOptions(frameworkFilter, language, platform, excludedItem, string.Empty));
            }

            return result;
        }

        private static List<object[]> GetTemplateOptions(string frameworkFilter, string language, string platform, string excludedItem, string appModel)
        {
            List<object[]> result = new List<object[]>();

            var context = new UserSelectionContext(language, platform);
            //if (!string.IsNullOrEmpty(appModel))
            //{
            //    context.AddAppModel(appModel);
            //}

            var projectTypes = GenContext.ToolBox.Repo.GetProjectTypes(context)
                                                    .Where(m => !string.IsNullOrEmpty(m.Description))
                                                    .Select(m => m.Name);

            foreach (var projectType in projectTypes)
            {
                context.ProjectType = projectType;
                var targetFrameworks = GenContext.ToolBox.Repo.GetFrontEndFrameworks(context)
                                                                .Where(m => m.Name == frameworkFilter)
                                                                .Select(m => m.Name).ToList();

                foreach (var framework in targetFrameworks)
                {
                    var itemTemplates = GenContext.ToolBox.Repo.GetAll()
                        .Where(t =>
                        (t.GetFrontEndFrameworkList().Contains(framework) || t.GetFrontEndFrameworkList().Contains(All))
                        && (t.GetProjectTypeList().Contains(projectType) || t.GetProjectTypeList().Contains(All))
                        && t.GetTemplateType().IsItemTemplate()
                        && t.GetPlatform() == platform
                        && t.GetLanguage() == language
                        && IsMatchPropertyBag(t, context.PropertyBag)
                        && t.Identity != excludedItem
                        && !t.GetIsHidden());

                    foreach (var itemTemplate in itemTemplates)
                    {
                        if (!string.IsNullOrEmpty(appModel))
                        {
                            result.Add(new object[]
                            {
                                itemTemplate.Name,
                                projectType,
                                framework,
                                platform,
                                itemTemplate.Identity,
                                language,
                                appModel,
                            });
                        }
                        else
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
            }

            return result;
        }

        protected static IEnumerable<object[]> GetVBProjectTemplates()
        {
            List<object[]> result = new List<object[]>();

            var platform = Platforms.Uwp;
            var context = new UserSelectionContext(ProgrammingLanguages.VisualBasic, platform);

            var projectTemplates =
               GenContext.ToolBox.Repo.GetAll().Where(
                   t => t.GetTemplateType() == TemplateType.Project
                    && t.GetLanguage() == ProgrammingLanguages.VisualBasic);

            foreach (var projectTemplate in projectTemplates)
            {
                var projectTypeList = projectTemplate.GetProjectTypeList();

                foreach (var projectType in projectTypeList)
                {
                    context.ProjectType = projectType;
                    var frameworks = GenContext.ToolBox.Repo.GetFrontEndFrameworks(context)
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

                    var context = new UserSelectionContext(language, platform);

                    var projectTypes = GenContext.ToolBox.Repo.GetProjectTypes(context)
                                                              .Where(m => !string.IsNullOrEmpty(m.Description))
                                                              .Select(m => m.Name);

                    foreach (var projectType in projectTypes)
                    {
                        context.ProjectType = projectType;
                        var targetFrameworks = GenContext.ToolBox.Repo.GetFrontEndFrameworks(context)
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
