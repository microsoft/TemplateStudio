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

        private const string XamarinSharedMSBuildProjectFilesText = "GlobalSection(SharedMSBuildProjectFiles) = preSolution";

        private const string XamarinSharedMSBuildProjectFilesTemplate = @"		{name}\{name}\{name}.projitems*{{id}}*SharedItemsImports = 13
";

        private const string XamarinMSBuildProjectFilesTemplate = @"		{name}\{name}\{name}.projitems*{id}*SharedItemsImports = 4
";

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

        private const string XamarinAndroidProjectConfigurationTemplate = @"		{0}.Ad-Hoc|Any CPU.ActiveCfg = Release|Any CPU
		{0}.Ad-Hoc|Any CPU.Build.0 = Release|Any CPU
		{0}.Ad-Hoc|Any CPU.Deploy.0 = Release|Any CPU
		{0}.Ad-Hoc|ARM.ActiveCfg = Release|Any CPU
		{0}.Ad-Hoc|ARM.Build.0 = Release|Any CPU
		{0}.Ad-Hoc|ARM.Deploy.0 = Release|Any CPU
		{0}.Ad-Hoc|iPhone.ActiveCfg = Release|Any CPU
		{0}.Ad-Hoc|iPhone.Build.0 = Release|Any CPU
		{0}.Ad-Hoc|iPhone.Deploy.0 = Release|Any CPU
		{0}.Ad-Hoc|iPhoneSimulator.ActiveCfg = Release|Any CPU
		{0}.Ad-Hoc|iPhoneSimulator.Build.0 = Release|Any CPU
		{0}.Ad-Hoc|iPhoneSimulator.Deploy.0 = Release|Any CPU
		{0}.Ad-Hoc|x64.ActiveCfg = Release|Any CPU
		{0}.Ad-Hoc|x64.Build.0 = Release|Any CPU
		{0}.Ad-Hoc|x64.Deploy.0 = Release|Any CPU
		{0}.Ad-Hoc|x86.ActiveCfg = Release|Any CPU
		{0}.Ad-Hoc|x86.Build.0 = Release|Any CPU
		{0}.Ad-Hoc|x86.Deploy.0 = Release|Any CPU
		{0}.AppStore|Any CPU.ActiveCfg = Release|Any CPU
		{0}.AppStore|Any CPU.Build.0 = Release|Any CPU
		{0}.AppStore|Any CPU.Deploy.0 = Release|Any CPU
		{0}.AppStore|ARM.ActiveCfg = Release|Any CPU
		{0}.AppStore|ARM.Build.0 = Release|Any CPU
		{0}.AppStore|ARM.Deploy.0 = Release|Any CPU
		{0}.AppStore|iPhone.ActiveCfg = Release|Any CPU
		{0}.AppStore|iPhone.Build.0 = Release|Any CPU
		{0}.AppStore|iPhone.Deploy.0 = Release|Any CPU
		{0}.AppStore|iPhoneSimulator.ActiveCfg = Release|Any CPU
		{0}.AppStore|iPhoneSimulator.Build.0 = Release|Any CPU
		{0}.AppStore|iPhoneSimulator.Deploy.0 = Release|Any CPU
		{0}.AppStore|x64.ActiveCfg = Release|Any CPU
		{0}.AppStore|x64.Build.0 = Release|Any CPU
		{0}.AppStore|x64.Deploy.0 = Release|Any CPU
		{0}.AppStore|x86.ActiveCfg = Release|Any CPU
		{0}.AppStore|x86.Build.0 = Release|Any CPU
		{0}.AppStore|x86.Deploy.0 = Release|Any CPU
		{0}.Debug|Any CPU.ActiveCfg = Debug|Any CPU
		{0}.Debug|Any CPU.Build.0 = Debug|Any CPU
		{0}.Debug|Any CPU.Deploy.0 = Debug|Any CPU
		{0}.Debug|ARM.ActiveCfg = Debug|Any CPU
		{0}.Debug|ARM.Build.0 = Debug|Any CPU
		{0}.Debug|ARM.Deploy.0 = Debug|Any CPU
		{0}.Debug|iPhone.ActiveCfg = Debug|Any CPU
		{0}.Debug|iPhone.Build.0 = Debug|Any CPU
		{0}.Debug|iPhone.Deploy.0 = Debug|Any CPU
		{0}.Debug|iPhoneSimulator.ActiveCfg = Debug|Any CPU
		{0}.Debug|iPhoneSimulator.Build.0 = Debug|Any CPU
		{0}.Debug|iPhoneSimulator.Deploy.0 = Debug|Any CPU
		{0}.Debug|x64.ActiveCfg = Debug|Any CPU
		{0}.Debug|x64.Build.0 = Debug|Any CPU
		{0}.Debug|x64.Deploy.0 = Debug|Any CPU
		{0}.Debug|x86.ActiveCfg = Debug|Any CPU
		{0}.Debug|x86.Build.0 = Debug|Any CPU
		{0}.Debug|x86.Deploy.0 = Debug|Any CPU
		{0}.Release|Any CPU.ActiveCfg = Release|Any CPU
		{0}.Release|Any CPU.Build.0 = Release|Any CPU
		{0}.Release|Any CPU.Deploy.0 = Release|Any CPU
		{0}.Release|ARM.ActiveCfg = Release|Any CPU
		{0}.Release|ARM.Build.0 = Release|Any CPU
		{0}.Release|ARM.Deploy.0 = Release|Any CPU
		{0}.Release|iPhone.ActiveCfg = Release|Any CPU
		{0}.Release|iPhone.Build.0 = Release|Any CPU
		{0}.Release|iPhone.Deploy.0 = Release|Any CPU
		{0}.Release|iPhoneSimulator.ActiveCfg = Release|Any CPU
		{0}.Release|iPhoneSimulator.Build.0 = Release|Any CPU
		{0}.Release|iPhoneSimulator.Deploy.0 = Release|Any CPU
		{0}.Release|x64.ActiveCfg = Release|Any CPU
		{0}.Release|x64.Build.0 = Release|Any CPU
		{0}.Release|x64.Deploy.0 = Release|Any CPU
		{0}.Release|x86.ActiveCfg = Release|Any CPU
		{0}.Release|x86.Build.0 = Release|Any CPU
		{0}.Release|x86.Deploy.0 = Release|Any CPU
";

        private const string XamarinIOSProjectConfigurationTemplate = @"		{0}.Ad-Hoc|Any CPU.ActiveCfg = Ad-Hoc|iPhone
		{0}.Ad-Hoc|ARM.ActiveCfg = Ad-Hoc|iPhone
		{0}.Ad-Hoc|iPhone.ActiveCfg = Ad-Hoc|iPhone
		{0}.Ad-Hoc|iPhone.Build.0 = Ad-Hoc|iPhone
		{0}.Ad-Hoc|iPhoneSimulator.ActiveCfg = Ad-Hoc|iPhoneSimulator
		{0}.Ad-Hoc|iPhoneSimulator.Build.0 = Ad-Hoc|iPhoneSimulator
		{0}.Ad-Hoc|x64.ActiveCfg = Ad-Hoc|iPhone
		{0}.Ad-Hoc|x86.ActiveCfg = Ad-Hoc|iPhone
		{0}.AppStore|Any CPU.ActiveCfg = AppStore|iPhone
		{0}.AppStore|ARM.ActiveCfg = AppStore|iPhone
		{0}.AppStore|iPhone.ActiveCfg = AppStore|iPhone
		{0}.AppStore|iPhone.Build.0 = AppStore|iPhone
		{0}.AppStore|iPhoneSimulator.ActiveCfg = AppStore|iPhoneSimulator
		{0}.AppStore|iPhoneSimulator.Build.0 = AppStore|iPhoneSimulator
		{0}.AppStore|x64.ActiveCfg = AppStore|iPhone
		{0}.AppStore|x86.ActiveCfg = AppStore|iPhone
		{0}.Debug|Any CPU.ActiveCfg = Debug|iPhone
		{0}.Debug|ARM.ActiveCfg = Debug|iPhone
		{0}.Debug|iPhone.ActiveCfg = Debug|iPhone
		{0}.Debug|iPhone.Build.0 = Debug|iPhone
		{0}.Debug|iPhoneSimulator.ActiveCfg = Debug|iPhoneSimulator
		{0}.Debug|iPhoneSimulator.Build.0 = Debug|iPhoneSimulator
		{0}.Debug|x64.ActiveCfg = Debug|iPhone
		{0}.Debug|x86.ActiveCfg = Debug|iPhone
		{0}.Release|Any CPU.ActiveCfg = Release|iPhone
		{0}.Release|ARM.ActiveCfg = Release|iPhone
		{0}.Release|iPhone.ActiveCfg = Release|iPhone
		{0}.Release|iPhone.Build.0 = Release|iPhone
		{0}.Release|iPhoneSimulator.ActiveCfg = Release|iPhoneSimulator
		{0}.Release|iPhoneSimulator.Build.0 = Release|iPhoneSimulator
		{0}.Release|x64.ActiveCfg = Release|iPhone
		{0}.Release|x86.ActiveCfg = Release|iPhone
";

        private const string XamarinUwpProjectConfigurationTemplate = @"		{0}.Ad-Hoc|Any CPU.ActiveCfg = Release|x86
		{0}.Ad-Hoc|Any CPU.Build.0 = Release|x86
		{0}.Ad-Hoc|Any CPU.Deploy.0 = Release|x86
		{0}.Ad-Hoc|ARM.ActiveCfg = Release|ARM
		{0}.Ad-Hoc|ARM.Build.0 = Release|ARM
		{0}.Ad-Hoc|ARM.Deploy.0 = Release|ARM
		{0}.Ad-Hoc|iPhone.ActiveCfg = Release|x86
		{0}.Ad-Hoc|iPhone.Build.0 = Release|x86
		{0}.Ad-Hoc|iPhone.Deploy.0 = Release|x86
		{0}.Ad-Hoc|iPhoneSimulator.ActiveCfg = Release|x86
		{0}.Ad-Hoc|iPhoneSimulator.Build.0 = Release|x86
		{0}.Ad-Hoc|iPhoneSimulator.Deploy.0 = Release|x86
		{0}.Ad-Hoc|x64.ActiveCfg = Release|x64
		{0}.Ad-Hoc|x64.Build.0 = Release|x64
		{0}.Ad-Hoc|x64.Deploy.0 = Release|x64
		{0}.Ad-Hoc|x86.ActiveCfg = Release|x86
		{0}.Ad-Hoc|x86.Build.0 = Release|x86
		{0}.Ad-Hoc|x86.Deploy.0 = Release|x86
		{0}.AppStore|Any CPU.ActiveCfg = Release|x86
		{0}.AppStore|Any CPU.Build.0 = Release|x86
		{0}.AppStore|Any CPU.Deploy.0 = Release|x86
		{0}.AppStore|ARM.ActiveCfg = Release|ARM
		{0}.AppStore|ARM.Build.0 = Release|ARM
		{0}.AppStore|ARM.Deploy.0 = Release|ARM
		{0}.AppStore|iPhone.ActiveCfg = Release|x86
		{0}.AppStore|iPhone.Build.0 = Release|x86
		{0}.AppStore|iPhone.Deploy.0 = Release|x86
		{0}.AppStore|iPhoneSimulator.ActiveCfg = Release|x86
		{0}.AppStore|iPhoneSimulator.Build.0 = Release|x86
		{0}.AppStore|iPhoneSimulator.Deploy.0 = Release|x86
		{0}.AppStore|x64.ActiveCfg = Release|x64
		{0}.AppStore|x64.Build.0 = Release|x64
		{0}.AppStore|x64.Deploy.0 = Release|x64
		{0}.AppStore|x86.ActiveCfg = Release|x86
		{0}.AppStore|x86.Build.0 = Release|x86
		{0}.AppStore|x86.Deploy.0 = Release|x86
		{0}.Debug|Any CPU.ActiveCfg = Debug|x86
		{0}.Debug|ARM.ActiveCfg = Debug|ARM
		{0}.Debug|ARM.Build.0 = Debug|ARM
		{0}.Debug|ARM.Deploy.0 = Debug|ARM
		{0}.Debug|iPhone.ActiveCfg = Debug|x86
		{0}.Debug|iPhoneSimulator.ActiveCfg = Debug|x86
		{0}.Debug|x64.ActiveCfg = Debug|x64
		{0}.Debug|x64.Build.0 = Debug|x64
		{0}.Debug|x64.Deploy.0 = Debug|x64
		{0}.Debug|x86.ActiveCfg = Debug|x86
		{0}.Debug|x86.Build.0 = Debug|x86
		{0}.Debug|x86.Deploy.0 = Debug|x86
		{0}.Release|Any CPU.ActiveCfg = Release|x86
		{0}.Release|ARM.ActiveCfg = Release|ARM
		{0}.Release|ARM.Build.0 = Release|ARM
		{0}.Release|ARM.Deploy.0 = Release|ARM
		{0}.Release|iPhone.ActiveCfg = Release|x86
		{0}.Release|iPhoneSimulator.ActiveCfg = Release|x86
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

                if (platform == Platforms.Xamarin)
                {
                    var globalSectionIndex = slnContent.IndexOf(XamarinSharedMSBuildProjectFilesText, StringComparison.Ordinal);
                    var endGobalSectionIndex = slnContent.IndexOf("EndGlobalSection", globalSectionIndex, StringComparison.Ordinal);

                    var content = string.Empty;
                    if (Path.GetExtension(projectRelativeToSolutionPath) == ".shproj")
                    {
                        content = XamarinSharedMSBuildProjectFilesTemplate
                                .Replace("{name}", GenContext.Current.ProjectName)
                                .Replace("{id}", projectGuid.ToLowerInvariant());
                    }
                    else
                    {
                        content = XamarinMSBuildProjectFilesTemplate
                                .Replace("{name}", GenContext.Current.ProjectName)
                                .Replace("{id}", projectGuid.ToLowerInvariant());
                    }

                    slnContent = slnContent.Insert(endGobalSectionIndex - 1, content);
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
            else if (platform == Platforms.Xamarin)
            {
                if (projectName.Contains("Android"))
                {
                    return XamarinAndroidProjectConfigurationTemplate;
                }
                else if (projectName.Contains("iOS"))
                {
                    return XamarinIOSProjectConfigurationTemplate;
                }
                else if (projectName.Contains("UWP"))
                {
                    return XamarinUwpProjectConfigurationTemplate;
                }
            }

            return string.Empty;
        }

        private static string ReadTemplate(string platform)
        {
            switch (platform)
            {
                case Platforms.Uwp:
                    return File.ReadAllText(@"Solution\UwpSolutionTemplate.txt");
                case Platforms.Xamarin:
                    return File.ReadAllText(@"Solution\XamarinSolutionTemplate.txt");
            }

            throw new InvalidDataException(nameof(platform));
        }
    }
}
