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
using System.Text.RegularExpressions;

using Microsoft.TemplateEngine.Abstractions;
using Microsoft.Templates.Core;
using Microsoft.Templates.Core.Gen;
using Microsoft.Templates.Core.Locations;
using Microsoft.Templates.Fakes;
using Microsoft.Templates.UI;

namespace Microsoft.Templates.Test
{
    public sealed class StyleCopGenerationTestsFixture : IDisposable
    {
        private const string Platform = "x86";
        private const string Configuration = "Debug";
        private List<string> _usedNames = new List<string>();

        internal string TestRunPath = $"{Path.GetPathRoot(Environment.CurrentDirectory)}\\UIT\\SC{DateTime.Now.ToString("dd_HHmm")}\\";

        internal string TestProjectsPath => Path.GetFullPath(Path.Combine(TestRunPath, "Proj"));

        private static Lazy<TemplatesRepository> _repos = new Lazy<TemplatesRepository>(CreateNewRepos, true);

        public static IEnumerable<ITemplateInfo> Templates => _repos.Value.GetAll();

        private static TemplatesRepository CreateNewRepos()
        {
            return GenContext.ToolBox.Repo;
        }

        private static void InitializeTemplatesForLanguage(TemplatesSource source)
        {
            GenContext.Bootstrap(source, new FakeGenShell(Language.CSharp), Language.CSharp);
            GenContext.ToolBox.Repo.SynchronizeAsync().Wait();

            _repos = new Lazy<TemplatesRepository>(CreateNewRepos, true);
        }

        public void InitializeFixture(IContextProvider contextProvider)
        {
            var source = new StyleCopPlusLocalTemplatesSource();
            GenContext.Current = contextProvider;

            InitializeTemplatesForLanguage(source);
        }

        public void Dispose()
        {
            if (Directory.Exists(TestRunPath))
            {
                if (!Directory.Exists(TestProjectsPath)
                 || !Directory.EnumerateDirectories(TestProjectsPath).Any())
                {
                    Directory.Delete(TestRunPath, true);
                }
            }
        }

        public static IEnumerable<object[]> GetProjectTemplatesForStyleCop()
        {
            InitializeTemplatesForLanguage(new LocalTemplatesSource());

            var projectTemplates =
                StyleCopGenerationTestsFixture.Templates.Where(t => t.GetTemplateType() == TemplateType.Project);

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

        public void AddItems(UserSelection userSelection, IEnumerable<ITemplateInfo> templates, Func<ITemplateInfo, string> getName)
        {
            foreach (var template in templates)
            {
                if (template.GetMultipleInstance() || !AlreadyAdded(userSelection, template))
                {
                    var itemName = getName(template);

                    var validators = new List<Validator>()
                    {
                        new ExistingNamesValidator(_usedNames),
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

        [SuppressMessage("StyleCop", "SA1008", Justification = "StyleCop doesn't understand C#7 tuple return types yet.")]
        public (int exitCode, string outputFile) BuildSolution(string solutionName, string outputPath)
        {
            var outputFile = Path.Combine(outputPath, $"_buildOutput_{solutionName}.txt");

            // Build
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

        public string GetErrorLines(string filePath)
        {
            var re = new Regex(@"^.*error .*$", RegexOptions.Multiline & RegexOptions.IgnoreCase);
            var outputLines = File.ReadAllLines(filePath);
            var errorLines = outputLines.Where(l => re.IsMatch(l));

            return errorLines.Any() ? errorLines.Aggregate((i, j) => i + Environment.NewLine + j) : string.Empty;
        }

        public string GetDefaultName(ITemplateInfo template)
        {
            return template.GetDefaultName();
        }

        private static string GetPath(string fileName)
        {
            var path = Path.Combine(new FileInfo(Assembly.GetExecutingAssembly().Location).DirectoryName, fileName);

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

        private static bool AlreadyAdded(UserSelection userSelection, ITemplateInfo item)
        {
            return userSelection.Pages.Any(p => p.template.Identity == item.Identity)
                || userSelection.Features.Any(f => f.template.Identity == item.Identity);
        }
    }
}
