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
    public sealed class GenerationFixture : IDisposable
    {
        private const string Platform = "x86";
        private const string Configuration = "Debug";

        internal string TestRunPath = $"{Path.GetPathRoot(Environment.CurrentDirectory)}\\UIT\\{DateTime.Now.FormatAsDateHoursMinutes()}\\";

        internal string TestProjectsPath => Path.GetFullPath(Path.Combine(TestRunPath, "Proj"));

        internal string TestNewItemPath => Path.GetFullPath(Path.Combine(TestRunPath, "RightClick"));

        private static bool syncExecuted = false;
        public static IEnumerable<ITemplateInfo> Templates { get; private set; }

        private static async Task InitializeTemplatesForLanguageAsync(TemplatesSource source, string language)
        {
            GenContext.Bootstrap(source, new FakeGenShell(language), language);

            if (!syncExecuted)
            {
                await GenContext.ToolBox.Repo.SynchronizeAsync();
                syncExecuted = true;
            }
            else
            {
                await GenContext.ToolBox.Repo.RefreshAsync();
            }

            Templates = GenContext.ToolBox.Repo.GetAll();
        }

        public GenerationFixture()
        {
        }

        public async Task InitializeFixtureAsync(string language, IContextProvider contextProvider)
        {
            var source = new LocalTemplatesSource();
            GenContext.Current = contextProvider;

            await InitializeTemplatesForLanguageAsync(source, language);
        }

        public void Dispose()
        {
            if (Directory.Exists(TestRunPath))
            {
                if ((!Directory.Exists(TestProjectsPath) || !Directory.EnumerateDirectories(TestProjectsPath).Any())
                 && (!Directory.Exists(TestNewItemPath) || !Directory.EnumerateDirectories(TestNewItemPath).Any()))
                {
                    Directory.Delete(TestRunPath, true);
                }
            }
        }

        public static async Task<IEnumerable<object[]>> GetProjectTemplatesAsync()
        {
            List<object[]> result = new List<object[]>();
            foreach (var language in ProgrammingLanguages.GetAllLanguages())
            {
                await InitializeTemplatesForLanguageAsync(new LocalTemplatesSource(), language);

                var projectTemplates = Templates.Where(t => t.GetTemplateType() == TemplateType.Project
                                                         && t.GetLanguage() == language);

                foreach (var projectTemplate in projectTemplates)
                {
                    var projectTypeList = projectTemplate.GetProjectTypeList();

                    foreach (var projectType in projectTypeList)
                    {
                        var frameworks = GenComposer.GetSupportedFx(projectType);

                        foreach (var framework in frameworks)
                        {
                            result.Add(new object[] { projectType, framework, language });
                        }
                    }
                }
            }
            return result;
        }

        public static async Task<IEnumerable<object[]>> GetPageAndFeatureTemplatesAsync(string frameworkFilter)
        {
            List<object[]> result = new List<object[]>();
            foreach (var language in ProgrammingLanguages.GetAllLanguages())
            {
                await InitializeTemplatesForLanguageAsync(new LocalTemplatesSource(), language);

                var projectTemplates = Templates.Where(t => t.GetTemplateType() == TemplateType.Project
                                                         && t.GetLanguage() == language);

                foreach (var projectTemplate in projectTemplates)
                {
                    var projectTypeList = projectTemplate.GetProjectTypeList();

                    foreach (var projectType in projectTypeList)
                    {
                        var frameworks = GenComposer.GetSupportedFx(projectType).Where(f => f == frameworkFilter);

                        foreach (var framework in frameworks)
                        {
                            var itemTemplates = Templates.Where(t => t.GetFrameworkList().Contains(framework)
                                                                  && (t.GetTemplateType() == TemplateType.Page || t.GetTemplateType() == TemplateType.Feature)
                                                                  && t.GetLanguage() == language
                                                                  && !t.GetIsHidden());

                            foreach (var itemTemplate in itemTemplates)
                            {
                                result.Add(new object[]
                                    { itemTemplate.Name, projectType, framework, itemTemplate.Identity, language });
                            }
                        }
                    }
                }
            }
            return result;
        }

        public static IEnumerable<ITemplateInfo> GetTemplates(string framework)
        {
            return Templates.Where(t => t.GetFrameworkList().Contains(framework));
        }

        public static async Task<UserSelection> SetupProjectAsync(string projectType, string framework, string language)
        {
            await InitializeTemplatesForLanguageAsync(new LocalTemplatesSource(), language);

            var userSelection = new UserSelection
            {
                Framework = framework,
                ProjectType = projectType,
                Language = language,
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

        [SuppressMessage("StyleCop", "SA1008", Justification = "StyleCop doesn't understand C#7 tuple return types yet.")]
        public static (int exitCode, string outputFile) BuildSolution(string solutionName, string outputPath)
        {
            var outputFile = Path.Combine(outputPath, $"_buildOutput_{solutionName}.txt");

            // Build
            var solutionFile = Path.GetFullPath(outputPath + @"\" + solutionName + ".sln");

            Console.Out.WriteLine();
            Console.Out.WriteLine($"### > Ready to start building");
            Console.Out.Write($"### > Running following command: {GetPath("RestoreAndBuild.bat")} \"{solutionFile}\" {Platform} {Configuration}");

            var startInfo = new ProcessStartInfo(GetPath("RestoreAndBuild.bat"))
            {
                Arguments = $"\"{solutionFile}\" {Platform} {Configuration}",
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

        public static string GetErrorLines(string filePath)
        {
            Regex re = new Regex(@"^.*error .*$", RegexOptions.Multiline & RegexOptions.IgnoreCase);
            var outputLines = File.ReadAllLines(filePath);
            var errorLines = outputLines.Where(l => re.IsMatch(l));

            return errorLines.Any() ? errorLines.Aggregate((i, j) => i + Environment.NewLine + j) : string.Empty;
        }

        public static string GetDefaultName(ITemplateInfo template)
        {
            return template.GetDefaultName();
        }

#pragma warning disable RECS0154 // Parameter is never used - but used by method which takes an action which is passed a template
        public static string GetRandomName(ITemplateInfo template)
#pragma warning restore RECS0154 // Parameter is never used
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
