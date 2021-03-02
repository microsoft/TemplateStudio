// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using Microsoft.Templates.Core;

namespace Microsoft.Templates.Fakes
{
    public class FakeSolution
    {
        private const string ProjectConfigurationPlatformsText = "GlobalSection(ProjectConfigurationPlatforms) = postSolution";

        private const string ProjectTemplate = @"Project(""{{guid}}"") = ""{name}"", ""{path}"", ""{id}""
EndProject
";

        private const string ProjectFilter = @"Project\(\""(?<typeGuid>.*?)\""\)\s+=\s+\""(?<name>.*?)\"",\s+\""(?<path>.*?)\"",\s+\""(?<guid>.*?)\""(?<content>.*?)\bEndProject\b";

        private readonly string _path;

        private FakeSolution(string path)
        {
            _path = path;
        }

        public static FakeSolution LoadOrCreate(string platform, string language, string path)
        {
            if (!File.Exists(path))
            {
                var solutionTemplate = ReadTemplate(platform, language);

                File.WriteAllText(path, solutionTemplate, Encoding.UTF8);
            }

            return new FakeSolution(path);
        }

        public void AddProjectToSolution(string platform, string appmodel, string language, string projectName, string projectGuid, string projectRelativeToSolutionPath, bool isCPSProject, bool hasPlatforms)
        {
            var slnContent = File.ReadAllText(_path);

            if (slnContent.IndexOf($"\"{projectName}\"", StringComparison.Ordinal) == -1)
            {
                var globalIndex = slnContent.IndexOf("Global", StringComparison.Ordinal);
                var projectTypeGuid = GetProjectTypeGuid(Path.GetExtension(projectRelativeToSolutionPath), isCPSProject);
                projectGuid = projectGuid.Contains("{") ? projectGuid : "{" + projectGuid + "}";
                var projectContent = ProjectTemplate
                                            .Replace("{guid}", projectTypeGuid)
                                            .Replace("{name}", projectName)
                                            .Replace("{path}", projectRelativeToSolutionPath)
                                            .Replace("{id}", projectGuid);

                slnContent = slnContent.Insert(globalIndex, projectContent);

                var projectConfigurationTemplate = GetProjectConfigurationTemplate(platform, appmodel, language, projectRelativeToSolutionPath, isCPSProject, hasPlatforms);
                if (!string.IsNullOrEmpty(projectConfigurationTemplate))
                {
                    var globalSectionIndex = slnContent.IndexOf(ProjectConfigurationPlatformsText, StringComparison.Ordinal);

                    var endGobalSectionIndex = slnContent.IndexOf("EndGlobalSection", globalSectionIndex, StringComparison.Ordinal);

                    var projectConfigContent = string.Format(projectConfigurationTemplate, projectGuid);

                    slnContent = slnContent.Insert(endGobalSectionIndex - 1, projectConfigContent);
                }

                if (isCPSProject)
                {
                    slnContent = AddAnyCpuSolutionConfigurations(slnContent);
                    slnContent = AddAnyCpuProjectConfigutations(slnContent);
                }
            }

            File.WriteAllText(_path, slnContent, Encoding.UTF8);
        }

        public Dictionary<string, string> GetProjectGuids()
        {
            var result = new Dictionary<string, string>();

            var projectPattern = new Regex(ProjectFilter, RegexOptions.ExplicitCapture | RegexOptions.Singleline);
            var solutionContent = GetContent();
            var match = projectPattern.Match(solutionContent);

            while (match.Success)
            {
                result.Add(match.Groups["name"].Value, match.Groups["guid"].Value);
                match = match.NextMatch();
            }

            return result;
        }

        private string AddAnyCpuProjectConfigutations(string slnContent)
        {
            if (slnContent.Contains("|Any CPU = "))
            {
                // Ensure that all projects have 'Any CPU' platform configurations
                var slnLines = slnContent.Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);

                var projectGuids = new List<string>();

                foreach (var line in slnLines)
                {
                    if (line.StartsWith("Project(\"{", StringComparison.OrdinalIgnoreCase))
                    {
                        projectGuids.Add(line.Substring(line.LastIndexOf("{", StringComparison.OrdinalIgnoreCase)).Trim(new[] { '{', '}', '"' }));
                    }

                    if (line.StartsWith("Global", StringComparison.OrdinalIgnoreCase))
                    {
                        break;
                    }
                }

                // see if already have an entry
                // if not, add them above the ARM entries
                foreach (var projGuid in projectGuids)
                {
                    if (!slnContent.Contains($"{{{projGuid}}}.Debug|Any CPU.ActiveCfg"))
                    {
                        slnContent = slnContent.Replace($"{{{projGuid}}}.Debug|ARM.ActiveCfg", $"{{{projGuid}}}.Debug|Any CPU.ActiveCfg = Debug|x86\r\n\t\t{{{projGuid}}}.Debug|ARM.ActiveCfg");
                    }

                    if (!slnContent.Contains($"{{{projGuid}}}.Release|Any CPU.ActiveCfg"))
                    {
                        slnContent = slnContent.Replace($"{{{projGuid}}}.Release|ARM.ActiveCfg", $"{{{projGuid}}}.Release|Any CPU.ActiveCfg = Release|x86\r\n\t\t{{{projGuid}}}.Release|ARM.ActiveCfg");
                    }
                }
            }

            return slnContent;
        }

        private string AddAnyCpuSolutionConfigurations(string slnContent)
        {
            if (!slnContent.Contains("Debug|Any CPU = Debug|Any CPU"))
            {
                slnContent = slnContent.Replace("Debug|ARM = Debug|ARM", "Debug|Any CPU = Debug|Any CPU\r\n\t\tDebug|ARM = Debug|ARM");
            }

            if (!slnContent.Contains("Release|Any CPU = Release|Any CPU"))
            {
                slnContent = slnContent.Replace("Release|ARM = Release|ARM", "Release|Any CPU = Release|Any CPU\r\n\t\tRelease|ARM = Release|ARM");
            }

            return slnContent;
        }

        private static string GetProjectTypeGuid(string projectExtension, bool isCPSProject)
        {
            // See https://github.com/dotnet/project-system/blob/master/docs/opening-with-new-project-system.md
            switch (projectExtension)
            {
                case ".csproj":
                    return isCPSProject ? "9A19103F-16F7-4668-BE54-9A1E7A4F7556" : "FAE04EC0-301F-11D3-BF4B-00C04F79EFBC";
                case ".vbproj":
                    return isCPSProject ? "778DAE3C-4631-46EA-AA77-85C1314464D9" : "F184B08F-C81C-45F6-A57F-5ABD9991F28F";
                case ".vcxproj":
                    return "8BC9CEB8-8B4A-11D0-8D11-00A0C91BC942";
                case ".wapproj":
                    return "C7167F0D-BC9F-4E6E-AFE1-012C56B48DB5";
            }

            return string.Empty;
        }

        private static string GetProjectConfigurationTemplate(string platform, string appmodel, string language, string projectRelativeToSolutionPath, bool isCPSProject, bool hasPlatforms)
        {
            switch (platform)
            {
                case Platforms.Uwp:
                    if (isCPSProject)
                    {
                         return File.ReadAllText(@"Solution\ProjectConfigurationTemplates\Uwp\UwpProjectAnyCPUTemplate.txt");
                    }
                    else
                    {
                        return File.ReadAllText(@"Solution\ProjectConfigurationTemplates\Uwp\UwpProjectTemplate.txt");
                    }

                case Platforms.Wpf:
                    if (projectRelativeToSolutionPath.Contains("wapproj"))
                    {
                        return File.ReadAllText(@"Solution\ProjectConfigurationTemplates\Wpf\MSIXProjectTemplate.txt");
                    }
                    else if (projectRelativeToSolutionPath.Contains(".Core."))
                    {
                        return File.ReadAllText(@"Solution\ProjectConfigurationTemplates\Wpf\WpfCoreProjectTemplate.txt");
                    }
                    else if (projectRelativeToSolutionPath.Contains("XamlIsland"))
                    {
                        return File.ReadAllText(@"Solution\ProjectConfigurationTemplates\Wpf\XamlIslandProjectTemplate.txt");
                    }
                    else
                    {
                        if (hasPlatforms)
                        {
                            return File.ReadAllText(@"Solution\ProjectConfigurationTemplates\Wpf\WpfProjectTemplate.txt");
                        }
                        else
                        {
                            return File.ReadAllText(@"Solution\ProjectConfigurationTemplates\Wpf\WpfProjectAnyCPUTemplate.txt");
                        }
                    }

                case Platforms.WinUI:
                    if (projectRelativeToSolutionPath.Contains("wapproj"))
                    {
                        if (language == ProgrammingLanguages.Cpp)
                        {
                            return File.ReadAllText(@"Solution\ProjectConfigurationTemplates\WinUI\MSIXCppProjectTemplate.txt");
                        }
                        else
                        {
                            return File.ReadAllText(@"Solution\ProjectConfigurationTemplates\WinUI\MSIXProjectTemplate.txt");
                        }
                    }
                    else if (projectRelativeToSolutionPath.Contains(".Core."))
                    {
                        return File.ReadAllText(@"Solution\ProjectConfigurationTemplates\WinUI\WinUICoreProjectTemplate.txt");
                    }
                    else if (language == ProgrammingLanguages.Cpp)
                    {
                        if (appmodel == "Desktop")
                        {
                            return File.ReadAllText(@"Solution\ProjectConfigurationTemplates\WinUI\WinUICppDesktopProjectTemplate.txt");
                        }
                        else
                        {
                            return File.ReadAllText(@"Solution\ProjectConfigurationTemplates\WinUI\WinUICppUwpProjectTemplate.txt");
                        }
                    }
                    else
                    {
                        return File.ReadAllText(@"Solution\ProjectConfigurationTemplates\WinUI\WinUIProjectTemplate.txt");
                    }

                default:
                    return string.Empty;
            }
        }

        private static string ReadTemplate(string platform, string language)
        {
            switch (platform)
            {
                case Platforms.Uwp:
                    return File.ReadAllText(@"Solution\SolutionTemplates\UwpSolutionTemplate.txt");
                case Platforms.Wpf:
                    return File.ReadAllText(@"Solution\SolutionTemplates\WpfSolutionTemplate.txt");
                case Platforms.WinUI:
                    if (language == ProgrammingLanguages.Cpp)
                    {
                        return File.ReadAllText(@"Solution\SolutionTemplates\WinUICppSolutionTemplate.txt");
                    }
                    else
                    {
                        return File.ReadAllText(@"Solution\SolutionTemplates\WinUISolutionTemplate.txt");
                    }
            }

            throw new InvalidDataException(nameof(platform));
        }

        private string GetContent()
        {
            if (!File.Exists(_path))
            {
                throw new FileNotFoundException(string.Format("Solution file {0} does not exist", _path));
            }

            string solutionContent;
            using (var reader = new StreamReader(_path))
            {
                solutionContent = reader.ReadToEnd();
            }

            return solutionContent;
        }
    }
}
