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

        private const string UwpProjectConfigurationTemplate = @"		{0}.Debug|Any CPU.ActiveCfg = Debug|x86
		{0}.Debug|Any CPU.Build.0 = Debug|x86
		{0}.Debug|Any CPU.Deploy.0 = Debug|x86
        {0}.Debug|ARM.ActiveCfg = Debug|ARM
		{0}.Debug|ARM.Build.0 = Debug|ARM
		{0}.Debug|ARM.Deploy.0 = Debug|ARM
		{0}.Debug|ARM64.ActiveCfg = Debug|ARM64
		{0}.Debug|ARM64.Build.0 = Debug|ARM64
		{0}.Debug|ARM64.Deploy.0 = Debug|ARM64
		{0}.Debug|x64.ActiveCfg = Debug|x64
		{0}.Debug|x64.Build.0 = Debug|x64
		{0}.Debug|x64.Deploy.0 = Debug|x64
		{0}.Debug|x86.ActiveCfg = Debug|x86
		{0}.Debug|x86.Build.0 = Debug|x86
		{0}.Debug|x86.Deploy.0 = Debug|x86
        {0}.Release|Any CPU.ActiveCfg = Release|x86
		{0}.Release|Any CPU.Build.0 = Release|x86
		{0}.Release|Any CPU.Deploy.0 = Release|x86
		{0}.Release|ARM.ActiveCfg = Release|ARM
		{0}.Release|ARM.Build.0 = Release|ARM
		{0}.Release|ARM.Deploy.0 = Release|ARM
        {0}.Release|ARM64.ActiveCfg = Release|ARM64
		{0}.Release|ARM64.Build.0 = Release|ARM64
		{0}.Release|ARM64.Deploy.0 = Release|ARM64
		{0}.Release|x64.ActiveCfg = Release|x64
		{0}.Release|x64.Build.0 = Release|x64
		{0}.Release|x64.Deploy.0 = Release|x64
		{0}.Release|x86.ActiveCfg = Release|x86
		{0}.Release|x86.Build.0 = Release|x86
		{0}.Release|x86.Deploy.0 = Release|x86
";

        private const string UwpProjectConfigurationTemplateForAnyCpu = @"		{0}.Debug|Any CPU.ActiveCfg = Debug|Any CPU
		{0}.Debug|Any CPU.Build.0 = Debug|Any CPU
		{0}.Debug|ARM.ActiveCfg = Debug|Any CPU
		{0}.Debug|ARM.Build.0 = Debug|Any CPU
        {0}.Debug|ARM64.ActiveCfg = Debug|Any CPU
		{0}.Debug|ARM64.Build.0 = Debug|Any CPU
		{0}.Debug|x64.ActiveCfg = Debug|Any CPU
		{0}.Debug|x64.Build.0 = Debug|Any CPU
		{0}.Debug|x86.ActiveCfg = Debug|Any CPU
		{0}.Debug|x86.Build.0 = Debug|Any CPU
		{0}.Release|Any CPU.ActiveCfg = Release|Any CPU
		{0}.Release|Any CPU.Build.0 = Release|Any CPU
		{0}.Release|ARM.ActiveCfg = Release|Any CPU
		{0}.Release|ARM.Build.0 = Release|Any CPU
        {0}.Release|ARM64.ActiveCfg = Release|Any CPU
		{0}.Release|ARM64.Build.0 = Release|Any CPU
		{0}.Release|x64.ActiveCfg = Release|Any CPU
		{0}.Release|x64.Build.0 = Release|Any CPU
		{0}.Release|x86.ActiveCfg = Release|Any CPU
		{0}.Release|x86.Build.0 = Release|Any CPU
";

        private const string WpfProjectConfigurationTemplate = @"		{0}.Debug|Any CPU.ActiveCfg = Debug|Any CPU
		{0}.Debug|Any CPU.Build.0 = Debug|Any CPU
		{0}.Release|Any CPU.ActiveCfg = Release|Any CPU
		{0}.Release|Any CPU.Build.0 = Release|Any CPU
";

        private const string ProjectTemplate = @"Project(""{{guid}}"") = ""{name}"", ""{path}"", ""{id}""
EndProject
";

        private const string ProjectFilter = @"Project\(\""(?<typeGuid>.*?)\""\)\s+=\s+\""(?<name>.*?)\"",\s+\""(?<path>.*?)\"",\s+\""(?<guid>.*?)\""(?<content>.*?)\bEndProject\b";

        private readonly string _path;

        private FakeSolution(string path)
        {
            _path = path;
        }

        public static FakeSolution LoadOrCreate(string platform, string path)
        {
            if (!File.Exists(path))
            {
                var solutionTemplate = ReadTemplate(platform);

                File.WriteAllText(path, solutionTemplate, Encoding.UTF8);
            }

            return new FakeSolution(path);
        }

        public void AddProjectToSolution(string platform, string projectName, string projectGuid, string projectRelativeToSolutionPath, bool isCPSProject)
        {
            var slnContent = File.ReadAllText(_path);

            if (slnContent.IndexOf(projectName, StringComparison.Ordinal) == -1)
            {
                var globalIndex = slnContent.IndexOf("Global", StringComparison.Ordinal);
                var projectTypeGuid = GetProjectGuid(Path.GetExtension(projectRelativeToSolutionPath), isCPSProject);
                var projectContent = ProjectTemplate
                                            .Replace("{guid}", projectTypeGuid)
                                            .Replace("{name}", projectName)
                                            .Replace("{path}", projectRelativeToSolutionPath)
                                            .Replace("{id}", projectGuid);

                slnContent = slnContent.Insert(globalIndex, projectContent);

                var projectConfigurationTemplate = GetProjectConfigurationTemplate(platform, projectName, isCPSProject);
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

        private static string GetProjectGuid(string projectExtension, bool isCPSProject)
        {
            // See https://github.com/dotnet/project-system/blob/master/docs/opening-with-new-project-system.md
            switch (projectExtension)
            {
                case ".csproj":
                    return isCPSProject ? "9A19103F-16F7-4668-BE54-9A1E7A4F7556" : "FAE04EC0-301F-11D3-BF4B-00C04F79EFBC";
                case ".vbproj":
                    return isCPSProject ? "778DAE3C-4631-46EA-AA77-85C1314464D9" : "F184B08F-C81C-45F6-A57F-5ABD9991F28F";
            }

            return string.Empty;
        }

        private static string GetProjectConfigurationTemplate(string platform, string projectName, bool isCPSProject)
        {
            switch (platform)
            {
                case Platforms.Uwp:
                    if (isCPSProject)
                    {
                        return UwpProjectConfigurationTemplateForAnyCpu;
                    }
                    else
                    {
                        return UwpProjectConfigurationTemplate;
                    }

                case Platforms.Wpf:
                    return WpfProjectConfigurationTemplate;
                default:
                    return string.Empty;
            }
        }

        private static string ReadTemplate(string platform)
        {
            switch (platform)
            {
                case Platforms.Uwp:
                    return File.ReadAllText(@"Solution\UwpSolutionTemplate.txt");
                case Platforms.Wpf:
                    return File.ReadAllText(@"Solution\WpfSolutionTemplate.txt");
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
