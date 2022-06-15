﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.IO;
using System.Linq;
using Microsoft.Templates.Core;
using Microsoft.Templates.Core.Gen.Shell;
using Microsoft.Templates.Core.Helpers;
using Microsoft.Templates.Core.Services;
using Microsoft.Templates.SharedResources;

namespace Microsoft.Templates.UI.Services
{
    public class ProjectConfigInfoService
    {
        // Framework
        public const string CodeBehind = "CodeBehind";
        public const string None = "None";
        public const string Prism = "Prism";
        public const string MvvmToolkit = "MVVMToolkit";

        // ProjectType
        private const string Blank = "Blank";
        private const string NavView = "NavView";
        private const string SplitView = "SplitView";
        private const string TabbedNav = "TabbedNav";
        private const string MenuBar = "MenuBar";
        private const string Ribbon = "Ribbon";

        private readonly IGenShell _shell;

        public ProjectConfigInfoService(IGenShell shell)
        {
            _shell = shell;
        }

        public ProjectMetadata ReadProjectConfiguration()
        {
            var projectPath = _shell.Project.GetActiveProjectPath();
            var projectMetadata = ProjectMetadataService.GetProjectMetadata(projectPath);

            if (IsValid(projectMetadata))
            {
                return projectMetadata;
            }

            var inferredConfig = InferProjectConfiguration(projectMetadata);

            if (IsValid(inferredConfig))
            {
                ProjectMetadataService.SaveProjectMetadata(inferredConfig, projectPath);
            }

            return inferredConfig;
        }

        private bool IsValid(ProjectMetadata data)
        {
            return !string.IsNullOrEmpty(data.ProjectType) &&
                   !string.IsNullOrEmpty(data.Framework) &&
                   !string.IsNullOrEmpty(data.Platform);
        }

        private ProjectMetadata InferProjectConfiguration(ProjectMetadata data)
        {
            if (string.IsNullOrEmpty(data.Platform))
            {
                data.Platform = InferPlatform();
            }

            if (data.Platform == Platforms.WinUI)
            {
                data.AppModel = InferAppModel();
            }

            if (string.IsNullOrEmpty(data.ProjectType))
            {
                data.ProjectType = InferProjectType(data.Platform);
            }

            if (string.IsNullOrEmpty(data.Framework))
            {
                data.Framework = InferFramework(data.Platform);
            }

            return data;
        }

        public string InferAppModel()
        {
            if (IsCSharpProject())
            {
                return ContainsSDK("Microsoft.NET.Sdk") ? AppModels.Desktop : AppModels.Uwp;
            }

            return AppContainerApplication("AppContainerApplication", "false") ? AppModels.Desktop : AppModels.Uwp;
        }

        public string InferPlatform()
        {
            if (IsWinUI())
            {
                return Platforms.WinUI;
            }
            else if (IsUwp())
            {
                return Platforms.Uwp;
            }
            else if (IsWpf())
            {
                return Platforms.Wpf;
            }

            throw new Exception(Resources.ErrorUnableResolvePlatform);
        }

        private bool IsUwp()
        {
            var projectTypeGuids = _shell.Project.GetActiveProjectTypeGuids();

            if (projectTypeGuids != null && projectTypeGuids.ToUpperInvariant().Split(';').Contains("{A5A43C5B-DE2A-4C0C-9213-0A381AF9435A}"))
            {
                return true;
            }

            return false;
        }

        private bool IsWpf()
        {
            return ContainsSDK("Microsoft.NET.Sdk.WindowsDesktop");
        }

        private bool IsWinUI()
        {
            return ContainsNugetPackage("Microsoft.WindowsAppSDK");
        }

        private string InferProjectType(string platform)
        {
            if (IsTabbedNav(platform))
            {
                return TabbedNav;
            }
            else if (IsMenuBar(platform))
            {
                return MenuBar;
            }
            else if (IsNavView(platform))
            {
                return NavView;
            }
            else if (IsSplitView(platform))
            {
                return SplitView;
            }
            else if (IsBlank(platform))
            {
                return Blank;
            }
            else if (IsRibbon(platform))
            {
                return Ribbon;
            }

            return string.Empty;
        }

        private string InferFramework(string platform)
        {
            if (IsCodeBehind(platform))
            {
                return CodeBehind;
            }
            else if (IsNone(platform))
            {
                return None;
            }
            else if (IsPrism())
            {
                return Prism;
            }
            else if (IsMicrosoftMvvmToolkit(platform))
            {
                return MvvmToolkit;
            }

            return string.Empty;
        }

        private bool IsBlank(string platform)
        {
            switch (platform)
            {
                case Platforms.Uwp:
                    return !(ExistsFileInProjectPath("ShellPage.xaml", "Views")
                        || ExistsFileInProjectPath("PivotPage.xaml", "Views"));
                case Platforms.Wpf:
                    return ExistsFileInProjectPath("ShellWindow.xaml", "Views")
                        && !FileContainsLine("Views", "ShellWindow.xaml", "<Menu Grid.Row=\"0\" Focusable=\"False\">")
                        && !FileContainsLine("Views", "ShellWindow.xaml", "<controls:HamburgerMenu")
                        && !FileContainsLine("Views", "ShellWindow.xaml", "<Fluent:Ribbon x:Name=\"ribbonControl\" Grid.Row=\"0\">");
                case Platforms.WinUI:
                    return ExistsFileInProjectPath("IActivationHandler.cs", "Activation") || IsCppProject();
                default:
                    return false;
            }
        }

        private bool IsNavView(string platform)
        {
            switch (platform)
            {
                case Platforms.WinUI:
                    return ExistsFileInProjectPath("ShellPage.xaml", "Views")
                        && FileContainsLine("Views", "ShellPage.xaml", "<NavigationView");
                default:
                    return false;
            }
        }

        private bool IsSplitView(string platform)
        {
            switch (platform)
            {
                case Platforms.Uwp:
                    if (IsCSharpProject())
                    {
                        // Prism doesn't have an activation service, but will have PageToken constants
                        return ExistsFileInProjectPath("ShellPage.xaml", "Views")
                            && (ExistsFileInProjectPath("ActivationService.cs", "Services") || ExistsFileInProjectPath("PageTokens.cs", "Constants"));
                    }

                    return ExistsFileInProjectPath("ActivationService.vb", "Services")
                        && ExistsFileInProjectPath("ShellPage.xaml", "Views");
                case Platforms.Wpf:
                    return ExistsFileInProjectPath("ShellWindow.xaml", "Views")
                        && FileContainsLine("Views", "ShellWindow.xaml", "<controls:HamburgerMenu");
                default:
                    return false;
            }
        }

        private bool IsMenuBar(string platform)
        {
            switch (platform)
            {
                case Platforms.Uwp:
                    return ExistsFileInProjectPath("ShellPage.xaml", "Views")
                        && (ExistsFileInProjectPath("MenuNavigationHelper.cs", "Helpers")
                        || ExistsFileInProjectPath("MenuNavigationHelper.vb", "Helpers")
                        || ExistsFileInProjectPath("MenuNavigationService.cs", "Services"));
                case Platforms.Wpf:
                    return ExistsFileInProjectPath("ShellWindow.xaml", "Views")
                        && FileContainsLine("Views", "ShellWindow.xaml", "<Menu Grid.Row=\"0\" Focusable=\"False\">");
                case Platforms.WinUI:
                    return ExistsFileInProjectPath("ShellPage.xaml", "Views")
                        && FileContainsLine("Views", "ShellPage.xaml", "<MenuBar");
                default:
                    return false;
            }
        }

        private bool IsTabbedNav(string platform)
        {
            if (platform == Platforms.Uwp)
            {
                // TabbedNav implementation is equal to SplitView but winui:NavigationView contains a property PaneDisplayMode="Top"
                return IsSplitView(Platforms.Uwp)
                    && FileContainsLine("Views", "ShellPage.xaml", "PaneDisplayMode=\"Top\"");
            }

            return false;
        }

        private bool IsRibbon(string platform)
        {
            if (platform == Platforms.Wpf)
            {
                return ExistsFileInProjectPath("ShellWindow.xaml", "Views")
                        && FileContainsLine("Views", "ShellWindow.xaml", "<Fluent:Ribbon x:Name=\"ribbonControl\" Grid.Row=\"0\">");
            }

            return false;
        }

        private bool IsCodeBehind(string platform)
        {
            switch (platform)
            {
                case Platforms.Uwp:
                    if (IsCSharpProject())
                    {
                        if (ExistsFileInProjectPath("ActivationService.cs", "Services"))
                        {
                            var codebehindFile = Directory.GetFiles(Path.Combine(_shell.Project.GetActiveProjectPath(), "Views"), "*.xaml.cs", SearchOption.TopDirectoryOnly).FirstOrDefault();
                            if (!string.IsNullOrEmpty(codebehindFile))
                            {
                                var fileContent = File.ReadAllText(codebehindFile);
                                return fileContent.Contains("INotifyPropertyChanged")
                                    && fileContent.Contains("public event PropertyChangedEventHandler PropertyChanged;");
                            }
                        }
                    }

                    if (ExistsFileInProjectPath("ActivationService.vb", "Services"))
                    {
                        var codebehindFile = Directory.GetFiles(Path.Combine(_shell.Project.GetActiveProjectPath(), "Views"), "*.xaml.vb", SearchOption.TopDirectoryOnly).FirstOrDefault();
                        if (!string.IsNullOrEmpty(codebehindFile))
                        {
                            var fileContent = File.ReadAllText(codebehindFile);
                            return fileContent.Contains("INotifyPropertyChanged") &&
                                   fileContent.Contains("Public Event PropertyChanged As PropertyChangedEventHandler Implements INotifyPropertyChanged.PropertyChanged");
                        }
                    }

                    return false;
                case Platforms.Wpf:
                    if (ExistsFileInProjectPath("ApplicationHostService.cs", "Services"))
                    {
                        var codebehindFile = Directory.GetFiles(Path.Combine(_shell.Project.GetActiveProjectPath(), "Views"), "*.xaml.cs", SearchOption.TopDirectoryOnly).FirstOrDefault();
                        if (!string.IsNullOrEmpty(codebehindFile))
                        {
                            var fileContent = File.ReadAllText(codebehindFile);
                            return fileContent.Contains("INotifyPropertyChanged")
                                && fileContent.Contains("public event PropertyChangedEventHandler PropertyChanged;");
                        }
                    }

                    return false;
                default:
                    return false;
            }
        }

        private bool IsNone(string platform)
        {
            if (platform == Platforms.WinUI)
            {
                if (IsCSharpProject())
                {
                    var codebehindFile = Directory.GetFiles(Path.Combine(_shell.Project.GetActiveProjectPath()), "*.xaml.cs", SearchOption.AllDirectories).FirstOrDefault();
                    if (!string.IsNullOrEmpty(codebehindFile))
                    {
                        var fileContent = File.ReadAllText(codebehindFile);
                        return !fileContent.Contains("ViewModel");
                    }
                }

                if (IsCppProject())
                {
                    return true;
                }

                return false;
            }
            else
            {
                return false;
            }
        }

        private bool IsPrism()
        {
            return ContainsNugetPackage("Prism.Unity");
        }

        private bool IsMicrosoftMvvmToolkit(string platform)
        {
            if (platform == Platforms.WinUI)
            {
                return ContainsNugetPackage("CommunityToolkit.Mvvm");
            }
            else
            {
                return ContainsNugetPackage("Microsoft.Toolkit.Mvvm");
            }
        }

        private bool IsCSharpProject()
        {
            return Directory.GetFiles(_shell.Project.GetActiveProjectPath(), "*.csproj", SearchOption.TopDirectoryOnly).Any();
        }

        private bool IsVisualBasicProject()
        {
            return Directory.GetFiles(_shell.Project.GetActiveProjectPath(), "*.vbproj", SearchOption.TopDirectoryOnly).Any();
        }

        private bool IsCppProject()
        {
            return Directory.GetFiles(_shell.Project.GetActiveProjectPath(), "*.vcxproj", SearchOption.TopDirectoryOnly).Any();
        }

        public string GetProgrammingLanguage()
        {
            if (IsCSharpProject())
            {
                return ProgrammingLanguages.CSharp;
            }
            else if (IsCppProject())
            {
                return ProgrammingLanguages.Cpp;
            }
            else if (IsVisualBasicProject())
            {
                return ProgrammingLanguages.VisualBasic;
            }

            return string.Empty;
        }

        private bool ExistsFileInProjectPath(string fileName, string subPath = null)
        {
            try
            {
                var path = _shell.Project.GetActiveProjectPath();
                if (!string.IsNullOrEmpty(subPath))
                {
                    path = Path.Combine(path, subPath);
                }

                return Directory.Exists(path) && Directory.GetFiles(path, fileName, SearchOption.TopDirectoryOnly).Length > 0;
            }
            catch (DirectoryNotFoundException)
            {
                return false;
            }
            catch (FileNotFoundException)
            {
                return false;
            }
        }

        private bool FileContainsLine(string subPath, string fileName, string lineToFind)
        {
            try
            {
                var filePath = Path.Combine(_shell.Project.GetActiveProjectPath(), subPath, fileName);
                var fileContent = FileHelper.GetFileContent(filePath);
                return fileContent != null && fileContent.Contains(lineToFind);
            }
            catch (DirectoryNotFoundException)
            {
                return false;
            }
            catch (FileNotFoundException)
            {
                return false;
            }
        }

        private bool ContainsNugetPackage(string packageId)
        {
            var projfiles = Directory.GetFiles(_shell.Project.GetActiveProjectPath(), "*.*proj", SearchOption.TopDirectoryOnly);
            foreach (string file in projfiles)
            {
                if (File.ReadAllText(file).IndexOf($"<packagereference include=\"{packageId}", StringComparison.OrdinalIgnoreCase) != -1)
                {
                    return true;
                }
            }

            var configfiles = Directory.GetFiles(_shell.Project.GetActiveProjectPath(), "packages.config", SearchOption.TopDirectoryOnly);
            foreach (string file in configfiles)
            {
                if (File.ReadAllText(file).IndexOf($"<package id=\"{packageId}", StringComparison.OrdinalIgnoreCase) != -1)
                {
                    return true;
                }
            }

            return false;
        }

        private bool ContainsSDK(string sdkId)
        {
            var files = Directory.GetFiles(_shell.Project.GetActiveProjectPath(), "*.*proj", SearchOption.TopDirectoryOnly);
            foreach (string file in files)
            {
                if (File.ReadAllText(file).IndexOf($"sdk=\"{sdkId}\"", StringComparison.OrdinalIgnoreCase) != -1)
                {
                    return true;
                }
            }

            return false;
        }

        private bool AppContainerApplication(string property, string value)
        {
            var files = Directory.GetFiles(_shell.Project.GetActiveProjectPath(), "*.*proj", SearchOption.TopDirectoryOnly);
            foreach (string file in files)
            {
                if (File.ReadAllText(file).Contains($"<{property}>{value}</{property}>"))
                {
                    return true;
                }
            }

            return false;
        }
    }
}
