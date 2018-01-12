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
using System.Threading.Tasks;
using Microsoft.TemplateEngine.Abstractions;
using Microsoft.Templates.Core;
using Microsoft.Templates.Core.Gen;
using Microsoft.Templates.Core.Locations;
using Microsoft.Templates.Fakes;
using Microsoft.Templates.UI;

namespace Microsoft.Templates.Test
{
    public abstract class BaseGenAndBuildFixture
    {
        private const string Platform = "x86";
        private const string Config = "Debug";

        public abstract string GetTestRunPath();

        public abstract Task InitializeFixtureAsync(IContextProvider contextProvider, string framework = "");

        public string TestProjectsPath => Path.GetFullPath(Path.Combine(GetTestRunPath(), "Proj"));

        public string TestNewItemPath => Path.GetFullPath(Path.Combine(GetTestRunPath(), "RightClick"));

        public IEnumerable<ITemplateInfo> Templates() => GenContext.ToolBox.Repo.GetAll();

        public IEnumerable<ITemplateInfo> GetTemplates(string framework)
        {
            return GenContext.ToolBox.Repo.GetAll().Where(t => t.GetFrameworkList().Contains(framework));
        }

        public UserSelection SetupProject(string projectType, string framework, string language, Func<ITemplateInfo, string> getName = null)
        {
            var userSelection = new UserSelection(projectType, framework, language);

            var layouts = GenComposer.GetLayoutTemplates(userSelection.ProjectType, userSelection.Framework);

            foreach (var item in layouts)
            {
                if (getName != null)
                {
                    AddItem(userSelection, item.Template, getName);
                }
                else
                {
                    AddItem(userSelection, item.Layout.Name, item.Template);
                }
            }

            userSelection.HomeName = userSelection.Pages.FirstOrDefault().name;

            return userSelection;
        }

        public void AddItems(UserSelection userSelection, IEnumerable<ITemplateInfo> templates, Func<ITemplateInfo, string> getName)
        {
            foreach (var template in templates)
            {
                AddItem(userSelection, template, getName);
            }
        }

        public void AddItem(UserSelection userSelection, ITemplateInfo template, Func<ITemplateInfo, string> getName)
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

        public void AddItem(UserSelection userSelection, string itemName, ITemplateInfo template)
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

        [SuppressMessage("StyleCop", "SA1008", Justification = "StyleCop doesn't understand C#7 tuple return types yet.")]
        public (int exitCode, string outputFile) BuildAppxBundle(string projectName, string outputPath, string projectExtension)
        {
            var outputFile = Path.Combine(outputPath, $"_buildOutput_{projectName}.txt");

            var solutionFile = Path.GetFullPath(outputPath + @"\" + projectName + ".sln");
            var projectFile = Path.GetFullPath(outputPath + @"\" + projectName + @"\" + projectName + $".{projectExtension}");

            Console.Out.WriteLine();
            Console.Out.WriteLine($"### > Ready to start building");
            Console.Out.Write($"### > Running following command: {GetPath("RestoreAndBuildAppx.bat")} \"{projectFile}\"");

            var startInfo = new ProcessStartInfo(GetPath("RestoreAndBuildAppx.bat"))
            {
                Arguments = $"\"{solutionFile}\" \"{projectFile}\" ",
                UseShellExecute = false,
                RedirectStandardOutput = true,
                CreateNoWindow = false,
                WorkingDirectory = outputPath
            };

            var process = Process.Start(startInfo);

            File.WriteAllText(outputFile, process.StandardOutput.ReadToEnd(), Encoding.UTF8);

            process.WaitForExit();

            return (process.ExitCode, outputFile);
        }

        [SuppressMessage("StyleCop", "SA1008", Justification = "StyleCop doesn't understand C#7 tuple return types yet.")]
        public (int exitCode, string outputFile, string resultFile) RunWackTestOnAppxBundle(string bundleFilePath, string outputPath)
        {
            var outputFile = Path.Combine(outputPath, $"_wackOutput_{Path.GetFileName(bundleFilePath)}.txt");
            var resultFile = Path.Combine(outputPath, "_wackresults.xml");

            Console.Out.WriteLine();
            Console.Out.WriteLine("### > Ready to run WACK test");
            Console.Out.Write($"### > Running following command: {GetPath("RunWackTest.bat")} \"{bundleFilePath}\" \"{resultFile}\"");

            var startInfo = new ProcessStartInfo(GetPath("RunWackTest.bat"))
            {
                Arguments = $"\"{bundleFilePath}\" \"{resultFile}\" ",
                UseShellExecute = false,
                RedirectStandardOutput = true,
                CreateNoWindow = false,
                WorkingDirectory = outputPath
            };

            var process = Process.Start(startInfo);

            File.WriteAllText(outputFile, process.StandardOutput.ReadToEnd(), Encoding.UTF8);

            process.WaitForExit();

            return (process.ExitCode, outputFile, resultFile);
        }

        private bool AlreadyAdded(UserSelection userSelection, ITemplateInfo item)
        {
            return userSelection.Pages.Any(p => p.template.Identity == item.Identity) || userSelection.Features.Any(f => f.template.Identity == item.Identity);
        }

        public static string GetDefaultName(ITemplateInfo template)
        {
            return template.GetDefaultName();
        }

#pragma warning disable RECS0154 // Parameter is never used - but used by method which takes an action which is passed a template
        public static string GetRandomName(ITemplateInfo template)
#pragma warning restore RECS0154 // Parameter is never used
        {
            for (int i = 0; i < 10; i++)
            {
                var randomName = Path.GetRandomFileName().Replace(".", string.Empty);
                if (Naming.Validate(randomName, new List<Validator>()).IsValid)
                {
                    return randomName;
                }
            }

            throw new ApplicationException("No valid randomName could be generated");
        }

        [SuppressMessage("StyleCop", "SA1008", Justification = "StyleCop doesn't understand C#7 tuple return types yet.")]
        public (int exitCode, string outputFile) BuildSolution(string solutionName, string outputPath)
        {
            var outputFile = Path.Combine(outputPath, $"_buildOutput_{solutionName}.txt");

            // Build
            var solutionFile = Path.GetFullPath(outputPath + @"\" + solutionName + ".sln");

            Console.Out.WriteLine();
            Console.Out.WriteLine($"### > Ready to start building");
            Console.Out.Write($"### > Running following command: {GetPath("RestoreAndBuild.bat")} \"{solutionFile}\" {Platform} {Config}");

            var startInfo = new ProcessStartInfo(GetPath("RestoreAndBuild.bat"))
            {
                Arguments = $"\"{solutionFile}\" {Platform} {Config}",
                UseShellExecute = false,
                RedirectStandardOutput = true,
                CreateNoWindow = false,
                WorkingDirectory = outputPath
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
                if ((!Directory.Exists(TestProjectsPath) || !Directory.EnumerateDirectories(TestProjectsPath).Any())
                 && (!Directory.Exists(TestNewItemPath) || !Directory.EnumerateDirectories(TestNewItemPath).Any()))
                {
                    Directory.Delete(GetTestRunPath(), true);
                }
            }
        }

        public static void SetCurrentLanguage(string language)
        {
            GenContext.SetCurrentLanguage(language);
            var fakeShell = GenContext.ToolBox.Shell as FakeGenShell;
            fakeShell.SetCurrentLanguage(language);
        }
    }
}
