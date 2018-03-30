// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.IO;
using System.Linq;
using System.Text;
using Microsoft.Templates.Core;
using Microsoft.Templates.Core.Gen;

namespace Microsoft.Templates.Fakes
{
    public class FakeSolution
    {
        private const string ProjectConfigurationPlatformsText = "GlobalSection(ProjectConfigurationPlatforms) = postSolution";

        private const string UwpProjectConfigurationTemplate = @"		{0}.Debug|ARM.ActiveCfg = Debug|ARM
		{0}.Debug|ARM.Build.0 = Debug|ARM
		{0}.Debug|ARM.Deploy.0 = Debug|ARM
		{0}.Debug|x64.ActiveCfg = Debug|x64
		{0}.Debug|x64.Build.0 = Debug|x64
		{0}.Debug|x64.Deploy.0 = Debug|x64
		{0}.Debug|x86.ActiveCfg = Debug|x86
		{0}.Debug|x86.Build.0 = Debug|x86
		{0}.Debug|x86.Deploy.0 = Debug|x86
		{0}.Release|ARM.ActiveCfg = Release|ARM
		{0}.Release|ARM.Build.0 = Release|ARM
		{0}.Release|ARM.Deploy.0 = Release|ARM
		{0}.Release|x64.ActiveCfg = Release|x64
		{0}.Release|x64.Build.0 = Release|x64
		{0}.Release|x64.Deploy.0 = Release|x64
		{0}.Release|x86.ActiveCfg = Release|x86
		{0}.Release|x86.Build.0 = Release|x86
		{0}.Release|x86.Deploy.0 = Release|x86
";

        private const string ProjectTemplateCS = @"Project(""{FAE04EC0-301F-11D3-BF4B-00C04F79EFBC}"") = ""{name}"", ""{path}"", ""{id}""
EndProject
";

        private const string ProjectTemplateVB = @"Project(""{F184B08F-C81C-45F6-A57F-5ABD9991F28F}"") = ""{name}"", ""{path}"", ""{id}""
EndProject
";

        private const string ProjectTemplateShared = @"Project(""{D954291E-2A0B-460D-934E-DC6B0785DB48}"") = ""{name}"", ""{path}"", ""{{id}}""
EndProject
";

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

        public void AddProjectToSolution(string platform, string projectName, string projectGuid, string projectRelativeToSolutionPath)
        {
            var slnContent = File.ReadAllText(_path);

            if (slnContent.IndexOf(projectName, StringComparison.Ordinal) == -1)
            {
                var globalIndex = slnContent.IndexOf("Global", StringComparison.Ordinal);
                var projectTemplate = GetProjectTemplate(Path.GetExtension(projectRelativeToSolutionPath));
                var projectContent = projectTemplate
                                            .Replace("{name}", projectName)
                                            .Replace("{path}", projectRelativeToSolutionPath)
                                            .Replace("{id}", projectGuid);

                slnContent = slnContent.Insert(globalIndex, projectContent);

                var projectConfigurationTemplate = GetProjectConfigurationTemplate(platform, projectName);
                if (!string.IsNullOrEmpty(projectConfigurationTemplate))
                {
                    var globalSectionIndex = slnContent.IndexOf(ProjectConfigurationPlatformsText, StringComparison.Ordinal);

                    var endGobalSectionIndex = slnContent.IndexOf("EndGlobalSection", globalSectionIndex, StringComparison.Ordinal);

                    var projectConfigContent = string.Format(projectConfigurationTemplate, projectGuid);

                    slnContent = slnContent.Insert(endGobalSectionIndex - 1, projectConfigContent);
                }
            }

            File.WriteAllText(_path, slnContent, Encoding.UTF8);
        }

        private static string GetProjectTemplate(string projectExtension)
        {
            switch (projectExtension)
            {
                case ".csproj":
                    return ProjectTemplateCS;
                case ".vbproj":
                    return ProjectTemplateVB;
                case ".shproj":
                    return ProjectTemplateShared;
            }

            return string.Empty;
        }

        private static string GetProjectConfigurationTemplate(string platform, string projectName)
        {
            if (platform == Platforms.Uwp)
            {
                return UwpProjectConfigurationTemplate;
            }

            return string.Empty;
        }

        private static string ReadTemplate(string platform)
        {
            switch (platform)
            {
                case Platforms.Uwp:
                    return File.ReadAllText(@"Solution\UwpSolutionTemplate.txt");
            }

            throw new InvalidDataException(nameof(platform));
        }
    }
}
