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
using Microsoft.Templates.Test.Artifacts;
using Microsoft.Templates.UI;

namespace Microsoft.Templates.Test
{

    public sealed class GenerationFixture : IDisposable
    {
        private const string Platform = "x86";
        private const string Configuration = "Debug";

        internal string TestRunPath = $"{Path.GetPathRoot(Environment.CurrentDirectory)}\\UIT\\{DateTime.Now.ToString("dd_HHmm")}\\";
        internal string TestProjectsPath => Path.GetFullPath(Path.Combine(TestRunPath, "Proj"));
        internal string TestNewItemPath => Path.GetFullPath(Path.Combine(TestRunPath, "RightClick"));

        private static readonly Lazy<TemplatesRepository> _repos = new Lazy<TemplatesRepository>(() => CreateNewRepos(), true);
        public static IEnumerable<ITemplateInfo> Templates => _repos.Value.GetAll();

        private static TemplatesRepository CreateNewRepos()
        {
            var source = new LocalTemplatesSource();

            GenContext.Bootstrap(new LocalTemplatesSource(), new FakeGenShell());
            GenContext.ToolBox.Repo.SynchronizeAsync().Wait();

            return GenContext.ToolBox.Repo;
        }

        public GenerationFixture()
        {
        }

        public void Dispose()
        {
            if (Directory.Exists(TestRunPath))
            {
                if ((!Directory.Exists(TestProjectsPath) || Directory.EnumerateDirectories(TestProjectsPath).Count() == 0)
                    && (!Directory.Exists(TestNewItemPath) || Directory.EnumerateDirectories(TestNewItemPath).Count() == 0))
                {
                    Directory.Delete(TestRunPath, true);
                }
            }
        }

        public static IEnumerable<object[]> GetProjectTemplates()
        {
            var projectTemplates = Templates.Where(t => t.GetTemplateType() == TemplateType.Project);

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

        public static IEnumerable<object[]> GetPageAndFeatureTemplates()
        {
            var projectTemplates = Templates.Where(t => t.GetTemplateType() == TemplateType.Project);

            foreach (var projectTemplate in projectTemplates)
            {
                var projectTypeList = projectTemplate.GetProjectTypeList();
                foreach (var projectType in projectTypeList)
                {
                    var frameworks = GenComposer.GetSupportedFx(projectType);

                    foreach (var framework in frameworks)
                    {
                        var itemTemplates = GenerationFixture.Templates.Where(t => t.GetFrameworkList().Contains(framework) && t.GetTemplateType() == TemplateType.Page || t.GetTemplateType() == TemplateType.Feature && !t.GetIsHidden());

                        foreach (var itemTemplate in itemTemplates)
                        {
                            yield return new object[] { itemTemplate.Name, projectType, framework, itemTemplate.Identity };
                        }
                    }
                }
            }
        }

        public static IEnumerable<ITemplateInfo> GetTemplates(string framework)
        {
            return Templates
                    .Where(t => t.GetFrameworkList().Contains(framework));
        }

        public static UserSelection SetupProject(string projectType, string framework)
        {
            var projectTemplate = Templates.Where(t => t.GetTemplateType() == TemplateType.Project && t.GetProjectTypeList().Contains(projectType) && t.GetFrameworkList().Contains(framework)).FirstOrDefault();

            var userSelection = new UserSelection
            {
                Framework = framework,
                ProjectType = projectType,
                HomeName = "Main"
            };

            var layouts = GenComposer.GetLayoutTemplates(userSelection.ProjectType, userSelection.Framework);

            foreach (var item in layouts)
            {
                AddItem(userSelection, item.Layout.name, item.Template);
            }
            return userSelection;
        }

        public static void AddItems(UserSelection userSelection, IEnumerable<ITemplateInfo> templates, Func<ITemplateInfo, string> getName)
        {
            foreach (var template in templates)
            {
                AddItem(userSelection, template, getName);
            }
        }

        public static void AddItem(UserSelection userSelection, ITemplateInfo template, Func<ITemplateInfo, string> getName)
        {
            if (template.GetMultipleInstance() || !AlreadyAdded(userSelection, template))
            {
                var itemName = getName(template);
                var usedNames = userSelection.Pages.Select(p => p.name).Concat(userSelection.Features.Select(f => f.name));
                var validators = new List<Validator>()
                    {
                        new ExistingNamesValidator(usedNames),
                        new ReservedNamesValidator(),
                    };
                if (template.GetItemNameEditable())
                {
                    validators.Add(new DefaultNamesValidator());
                }
                itemName = Naming.Infer(itemName, validators);
                AddItem(userSelection, itemName, template);
            }
        }

        public static (int exitCode, string outputFile) BuildSolution(string solutionName, string outputPath)
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

        public static string GetErrorLines(string filePath)
        {
            Regex re = new Regex(@"^.*error .*$", RegexOptions.Multiline & RegexOptions.IgnoreCase);
            var outputLines = File.ReadAllLines(filePath);
            var errorLines = outputLines.Where(l => re.IsMatch(l));

            return errorLines.Count() > 0 ? errorLines.Aggregate((i, j) => i + Environment.NewLine + j) : String.Empty;
        }

        public static string GetDefaultName(ITemplateInfo template)
        {
            return template.GetDefaultName();
        }

        public static string GetRandomName(ITemplateInfo template)
        {
            return Path.GetRandomFileName().Replace(".", "");
        }

        private static void AddItem(UserSelection userSelection, string itemName, ITemplateInfo template)
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

        
    }
}
