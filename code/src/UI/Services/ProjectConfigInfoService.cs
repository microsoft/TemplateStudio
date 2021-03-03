// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.IO;
using System.Linq;
using Microsoft.Templates.Core;
using Microsoft.Templates.Core.Gen;
using Microsoft.Templates.Core.Helpers;
using Microsoft.Templates.Core.Resources;
using Microsoft.Templates.Core.Services;

namespace Microsoft.Templates.UI.Services
{
    public class ProjectConfigInfoService
    {
        // Framework
        public const string MVVMBasic = "MVVMBasic";
        public const string MVVMLight = "MVVMLight";
        public const string CodeBehid = "CodeBehind";
        public const string CaliburnMicro = "CaliburnMicro";
        public const string Prism = "Prism";
        public const string MvvmToolkit = "MVVMToolkit";

        // ProjectType
        private const string Blank = "Blank";
        private const string SplitView = "SplitView";
        private const string TabbedNav = "TabbedNav";
        private const string MenuBar = "MenuBar";
        private const string Ribbon = "Ribbon";

        public static ProjectMetadata ReadProjectConfiguration()
        {
            var projectPath = GenContext.ToolBox.Shell.GetActiveProjectPath();
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

        private static bool IsValid(ProjectMetadata data)
        {
            return !string.IsNullOrEmpty(data.ProjectType) &&
                   !string.IsNullOrEmpty(data.Framework) &&
                   !string.IsNullOrEmpty(data.Platform);
        }

        private static ProjectMetadata InferProjectConfiguration(ProjectMetadata data)
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

        public static string InferAppModel()
        {
            if (IsCSharpProject())
            {
                return ContainsSDK("Microsoft.NET.Sdk") ? AppModels.Desktop : AppModels.Uwp;
            }

            return AppContainerApplication("AppContainerApplication", "false") ? AppModels.Desktop : AppModels.Uwp;
        }

        public static string InferPlatform()
        {
            if (IsUwp())
            {
                return Platforms.Uwp;
            }
            else if (IsWpf())
            {
                return Platforms.Wpf;
            }
            else if (IsWinUI())
            {
                return Platforms.WinUI;
            }

            throw new Exception(StringRes.ErrorUnableResolvePlatform);
        }

        private static bool IsUwp()
        {
            var projectTypeGuids = GenContext.ToolBox.Shell.GetActiveProjectTypeGuids();

            if (projectTypeGuids != null && projectTypeGuids.ToUpperInvariant().Split(';').Contains("{A5A43C5B-DE2A-4C0C-9213-0A381AF9435A}"))
            {
                return true;
            }

            return false;
        }

        private static bool IsWpf()
        {
            return ContainsSDK("Microsoft.NET.Sdk.WindowsDesktop");
        }

        private static bool IsWinUI()
        {
            if (IsCSharpProject())
            {
                return ContainsNugetPackage("Microsoft.WinUI");
            }

            return ContainsNugetPackageCpp("Microsoft.WinUI");
        }

        private static string InferProjectType(string platform)
        {
            if (IsTabbedNav(platform))
            {
                return TabbedNav;
            }
            else if (IsMenuBar(platform))
            {
                return MenuBar;
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

        private static string InferFramework(string platform)
        {
            if (IsMVVMBasic(platform))
            {
                return MVVMBasic;
            }
            else if (IsMVVMLight(platform))
            {
                return MVVMLight;
            }
            else if (IsCodeBehind(platform))
            {
                return CodeBehid;
            }
            else if (IsCaliburnMicro(platform))
            {
                return CaliburnMicro;
            }
            else if (IsPrism())
            {
                return Prism;
            }
            else if (IsMicrosoftMvvmToolkit())
            {
                return MvvmToolkit;
            }

            return string.Empty;
        }

        private static bool IsBlank(string platform)
        {
            switch (platform)
            {
                case Platforms.Uwp:
                    return !(ExistsFileInProjectPath("Views", "ShellPage.xaml")
                        || ExistsFileInProjectPath("Views", "PivotPage.xaml"));
                case Platforms.Wpf:
                    return ExistsFileInProjectPath("Views", "ShellWindow.xaml")
                        && !FileContainsLine("Views", "ShellWindow.xaml", "<Menu Grid.Row=\"0\" Focusable=\"False\">")
                        && !FileContainsLine("Views", "ShellWindow.xaml", "<controls:HamburgerMenu")
                        && !FileContainsLine("Views", "ShellWindow.xaml", "<Fluent:Ribbon x:Name=\"ribbonControl\" Grid.Row=\"0\">");
                case Platforms.WinUI:
                    return IsCppProject();
                default:
                    return false;
            }
        }

        private static bool IsSplitView(string platform)
        {
            switch (platform)
            {
                case Platforms.Uwp:
                    if (IsCSharpProject())
                    {
                        // Prism doesn't have an activation service, but will have PageToken constants
                        return ExistsFileInProjectPath("Views", "ShellPage.xaml")
                            && (ExistsFileInProjectPath("Services", "ActivationService.cs") || ExistsFileInProjectPath("Constants", "PageTokens.cs"));
                    }

                    return ExistsFileInProjectPath("Services", "ActivationService.vb")
                        && ExistsFileInProjectPath("Views", "ShellPage.xaml");
                case Platforms.Wpf:
                    return ExistsFileInProjectPath("Views", "ShellWindow.xaml")
                        && FileContainsLine("Views", "ShellWindow.xaml", "<controls:HamburgerMenu");
                case Platforms.WinUI:
                    return ExistsFileInProjectPath("Views", "ShellWindow.xaml")
                        && FileContainsLine("Views", "ShellWindow.xaml", "<NavigationView");
                default:
                    return false;
            }
        }

        private static bool IsMenuBar(string platform)
        {
            switch (platform)
            {
                case Platforms.Uwp:
                    return ExistsFileInProjectPath("Views", "ShellPage.xaml")
                        && (ExistsFileInProjectPath("Helpers", "MenuNavigationHelper.cs")
                        || ExistsFileInProjectPath("Helpers", "MenuNavigationHelper.vb")
                        || ExistsFileInProjectPath("Services", "MenuNavigationService.cs"));
                case Platforms.Wpf:
                    return ExistsFileInProjectPath("Views", "ShellWindow.xaml")
                        && FileContainsLine("Views", "ShellWindow.xaml", "<Menu Grid.Row=\"0\" Focusable=\"False\">");
                default:
                    return false;
            }
        }

        private static bool IsTabbedNav(string platform)
        {
            if (platform == Platforms.Uwp)
            {
                // TabbedNav implementation is equal to SplitView but winui:NavigationView contains a property PaneDisplayMode="Top"
                return IsSplitView(Platforms.Uwp)
                    && FileContainsLine("Views", "ShellPage.xaml", "PaneDisplayMode=\"Top\"");
            }

            return false;
        }

        private static bool IsRibbon(string platform)
        {
            if (platform == Platforms.Wpf)
            {
                return ExistsFileInProjectPath("Views", "ShellWindow.xaml")
                        && FileContainsLine("Views", "ShellWindow.xaml", "<Fluent:Ribbon x:Name=\"ribbonControl\" Grid.Row=\"0\">");
            }

            return false;
        }

        private static bool IsMVVMBasic(string platform)
        {
            switch (platform)
            {
                case Platforms.Uwp:
                    if (IsCSharpProject())
                    {
                        return ExistsFileInProjectPath("Services", "ActivationService.cs")
                            && ExistsFileInProjectPath("Helpers", "Observable.cs");
                    }

                    return ExistsFileInProjectPath("Services", "ActivationService.vb")
                        && ExistsFileInProjectPath("Helpers", "Observable.vb");
                case Platforms.Wpf:
                    return ExistsFileInProjectPath("Services", "ApplicationHostService.cs")
                        && ExistsFileInProjectPath("Helpers", "Observable.cs")
                        && ExistsFileInProjectPath("Helpers", "RelayCommand.cs");
                default:
                    return false;
            }
        }

        private static bool IsMVVMLight(string platform)
        {
            switch (platform)
            {
                case Platforms.Uwp:
                    return (ExistsFileInProjectPath("Services", "ActivationService.cs")
                        || ExistsFileInProjectPath("Services", "ActivationService.vb"))
                        && ContainsNugetPackage("MvvmLight");
                case Platforms.Wpf:
                    return ExistsFileInProjectPath("Services", "ApplicationHostService.cs")
                        && ExistsFileInProjectPath("ViewModels", "ViewModelLocator.cs")
                        && ContainsNugetPackage("MvvmLight");
                default:
                    return false;
            }
        }

        private static bool IsCodeBehind(string platform)
        {
            switch (platform)
            {
                case Platforms.Uwp:
                    if (IsCSharpProject())
                    {
                        if (ExistsFileInProjectPath("Services", "ActivationService.cs"))
                        {
                            var codebehindFile = Directory.GetFiles(Path.Combine(GenContext.ToolBox.Shell.GetActiveProjectPath(), "Views"), "*.xaml.cs", SearchOption.TopDirectoryOnly).FirstOrDefault();
                            if (!string.IsNullOrEmpty(codebehindFile))
                            {
                                var fileContent = File.ReadAllText(codebehindFile);
                                return fileContent.Contains("INotifyPropertyChanged")
                                    && fileContent.Contains("public event PropertyChangedEventHandler PropertyChanged;");
                            }
                        }
                    }

                    if (ExistsFileInProjectPath("Services", "ActivationService.vb"))
                    {
                        var codebehindFile = Directory.GetFiles(Path.Combine(GenContext.ToolBox.Shell.GetActiveProjectPath(), "Views"), "*.xaml.vb", SearchOption.TopDirectoryOnly).FirstOrDefault();
                        if (!string.IsNullOrEmpty(codebehindFile))
                        {
                            var fileContent = File.ReadAllText(codebehindFile);
                            return fileContent.Contains("INotifyPropertyChanged") &&
                                   fileContent.Contains("Public Event PropertyChanged As PropertyChangedEventHandler Implements INotifyPropertyChanged.PropertyChanged");
                        }
                    }

                    return false;
                case Platforms.Wpf:
                    if (ExistsFileInProjectPath("Services", "ApplicationHostService.cs"))
                    {
                        var codebehindFile = Directory.GetFiles(Path.Combine(GenContext.ToolBox.Shell.GetActiveProjectPath(), "Views"), "*.xaml.cs", SearchOption.TopDirectoryOnly).FirstOrDefault();
                        if (!string.IsNullOrEmpty(codebehindFile))
                        {
                            var fileContent = File.ReadAllText(codebehindFile);
                            return fileContent.Contains("INotifyPropertyChanged")
                                && fileContent.Contains("public event PropertyChangedEventHandler PropertyChanged;");
                        }
                    }

                    return false;
                case Platforms.WinUI:
                    return IsCppProject();
                default:
                    return false;
            }
        }

        private static bool IsPrism()
        {
            return ContainsNugetPackage("Prism.Unity");
        }

        private static bool IsCaliburnMicro(string platform)
        {
            if (platform == Platforms.Uwp)
            {
                return (ExistsFileInProjectPath("Services", "ActivationService.cs")
                    || ExistsFileInProjectPath("Services", "ActivationService.vb"))
                    && ContainsNugetPackage("Caliburn.Micro");
            }

            return false;
        }

        private static bool IsMicrosoftMvvmToolkit()
        {
            return ContainsNugetPackage("Microsoft.Toolkit.Mvvm");
        }

        private static bool IsCSharpProject()
        {
            return Directory.GetFiles(GenContext.ToolBox.Shell.GetActiveProjectPath(), "*.csproj", SearchOption.TopDirectoryOnly).Any();
        }

        private static bool IsVisualBasicProject()
        {
            return Directory.GetFiles(GenContext.ToolBox.Shell.GetActiveProjectPath(), "*.vbproj", SearchOption.TopDirectoryOnly).Any();
        }

        private static bool IsCppProject()
        {
            return Directory.GetFiles(GenContext.ToolBox.Shell.GetActiveProjectPath(), "*.vcxproj", SearchOption.TopDirectoryOnly).Any();
        }

        public static string GetProgrammingLanguages()
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

        private static bool ExistsFileInProjectPath(string subPath, string fileName)
        {
            try
            {
                return Directory.GetFiles(Path.Combine(GenContext.ToolBox.Shell.GetActiveProjectPath(), subPath), fileName, SearchOption.TopDirectoryOnly).Count() > 0;
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

        private static bool FileContainsLine(string subPath, string fileName, string lineToFind)
        {
            try
            {
                var filePath = Path.Combine(GenContext.ToolBox.Shell.GetActiveProjectPath(), subPath, fileName);
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

        private static bool ContainsNugetPackage(string packageId)
        {
            var files = Directory.GetFiles(GenContext.ToolBox.Shell.GetActiveProjectPath(), "*.*proj", SearchOption.TopDirectoryOnly);
            foreach (string file in files)
            {
                if (File.ReadAllText(file).ToLowerInvariant().Contains($"<PackageReference Include=\"{packageId.ToLowerInvariant()}"))
                {
                    return true;
                }
            }

            return false;
        }

        private static bool ContainsNugetPackageCpp(string packageId)
        {
            var files = Directory.GetFiles(GenContext.ToolBox.Shell.GetActiveProjectPath(), "packages.config", SearchOption.TopDirectoryOnly);
            foreach (string file in files)
            {
                if (File.ReadAllText(file).ToLowerInvariant().Contains($"<package id=\"{packageId.ToLowerInvariant()}"))
                {
                    return true;
                }
            }

            return false;
        }

        private static bool ContainsSDK(string sdkId)
        {
            var files = Directory.GetFiles(GenContext.ToolBox.Shell.GetActiveProjectPath(), "*.*proj", SearchOption.TopDirectoryOnly);
            foreach (string file in files)
            {
                if (File.ReadAllText(file).Contains($"Sdk=\"{sdkId}\""))
                {
                    return true;
                }
            }

            return false;
        }

        private static bool AppContainerApplication(string property, string value)
        {
            var files = Directory.GetFiles(GenContext.ToolBox.Shell.GetActiveProjectPath(), "*.*proj", SearchOption.TopDirectoryOnly);
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
