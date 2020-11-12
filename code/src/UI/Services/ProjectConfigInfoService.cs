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
        public const string FxMVVMBasic = "MVVMBasic";
        public const string FxMVVMLight = "MVVMLight";
        public const string FxCodeBehid = "CodeBehind";
        public const string FxCaliburnMicro = "CaliburnMicro";
        public const string FxPrism = "Prism";
        public const string FxMvvmToolkit = "MVVMToolkit";

        private const string PlUwp = "Uwp";
        private const string PlWpf = "Wpf";

        private const string ProjTypeBlank = "Blank";
        private const string ProjTypeSplitView = "SplitView";
        private const string ProjTypeTabbedNav = "TabbedNav";
        private const string ProjTypeMenuBar = "MenuBar";
        private const string ProjTypeRibbon = "Ribbon";

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

            if (data.Platform == PlUwp)
            {
                if (string.IsNullOrEmpty(data.ProjectType))
                {
                    data.ProjectType = InferUwpProjectType();
                }

                if (string.IsNullOrEmpty(data.Framework))
                {
                    data.Framework = InferUwpFramework();
                }

                return data;
            }
            else if (data.Platform == PlWpf)
            {
                if (string.IsNullOrEmpty(data.ProjectType))
                {
                    data.ProjectType = InferWpfProjectType();
                }

                if (string.IsNullOrEmpty(data.Framework))
                {
                    data.Framework = InferWpfFramework();
                }

                return data;
            }

            return new ProjectMetadata();
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
            return ContainsSDK("Microsoft.NET.Sdk");
        }

        private static string InferUwpFramework()
        {
            if (IsUwpMVVMBasic())
            {
                return FxMVVMBasic;
            }
            else if (IsUwpMVVMLight())
            {
                return FxMVVMLight;
            }
            else if (IsUwpCodeBehind())
            {
                return FxCodeBehid;
            }
            else if (IsUwpCaliburnMicro())
            {
                return FxCaliburnMicro;
            }
            else if (IsUwpPrism())
            {
                return FxPrism;
            }
            else if (IsUwpMicrosoftToolkitMvvm())
            {
                return FxMvvmToolkit;
            }
            else
            {
                return string.Empty;
            }
        }

        private static string InferWpfFramework()
        {
            if (IsWpfMVVMBasic())
            {
                return FxMVVMBasic;
            }
            else if (IsWpfMVVMLight())
            {
                return FxMVVMLight;
            }
            else if (IsWpfPrism())
            {
                return FxPrism;
            }
            else
            {
                return string.Empty;
            }
        }

        private static string InferUwpProjectType()
        {
            if (IsUwpTabbedNav())
            {
                return ProjTypeTabbedNav;
            }
            else if (IsUwpMenuBar())
            {
                return ProjTypeMenuBar;
            }
            else if (IsUwpSplitView())
            {
                return ProjTypeSplitView;
            }
            else if (IsUwpBlank())
            {
                return ProjTypeBlank;
            }
            else
            {
                return string.Empty;
            }
        }

        private static string InferWpfProjectType()
        {
            if (IsWpfMenuBar())
            {
                return ProjTypeMenuBar;
            }
            else if (IsWpfSplitView())
            {
                return ProjTypeSplitView;
            }
            else if (IsWpfBlank())
            {
                return ProjTypeBlank;
            }
            else if (IsWpfRibbon())
            {
                return ProjTypeRibbon;
            }
            else
            {
                return string.Empty;
            }
        }

        private static bool IsUwpBlank()
        {
            return !(ExistsFileInProjectPath("Views", "ShellPage.xaml")
                || ExistsFileInProjectPath("Views", "PivotPage.xaml"));
        }

        private static bool IsWpfBlank()
        {
            return ExistsFileInProjectPath("Views", "ShellWindow.xaml")
                && !FileContainsLine("Views", "ShellWindow.xaml", "<Menu Grid.Row=\"0\" Focusable=\"False\">")
                && !FileContainsLine("Views", "ShellWindow.xaml", "<controls:HamburgerMenu")
                && !FileContainsLine("Views", "ShellWindow.xaml", "<Fluent:Ribbon x:Name=\"ribbonControl\" Grid.Row=\"0\">");
        }

        private static bool IsWpfRibbon()
        {
            return ExistsFileInProjectPath("Views", "ShellWindow.xaml")
                && FileContainsLine("Views", "ShellWindow.xaml", "<Fluent:Ribbon x:Name=\"ribbonControl\" Grid.Row=\"0\">");
        }

        private static bool IsUwpMenuBar()
        {
            return ExistsFileInProjectPath("Views", "ShellPage.xaml")
                && (ExistsFileInProjectPath("Helpers", "MenuNavigationHelper.cs") || ExistsFileInProjectPath("Helpers", "MenuNavigationHelper.vb") || ExistsFileInProjectPath("Services", "MenuNavigationService.cs"));
        }

        private static bool IsWpfMenuBar()
        {
            return ExistsFileInProjectPath("Views", "ShellWindow.xaml")
                && FileContainsLine("Views", "ShellWindow.xaml", "<Menu Grid.Row=\"0\" Focusable=\"False\">");
        }

        private static bool IsUwpMVVMLight()
        {
            return (ExistsFileInProjectPath("Services", "ActivationService.cs") || ExistsFileInProjectPath("Services", "ActivationService.vb"))
                && ContainsNugetPackage("MvvmLight");
        }

        private static bool IsWpfMVVMLight()
        {
            return ExistsFileInProjectPath("Services", "ApplicationHostService.cs")
                && ExistsFileInProjectPath("ViewModels", "ViewModelLocator.cs")
                && ContainsNugetPackage("MvvmLight");
        }

        private static bool IsUwpMVVMBasic()
        {
            if (IsCSharpProject())
            {
                return ExistsFileInProjectPath("Services", "ActivationService.cs")
                    && ExistsFileInProjectPath("Helpers", "Observable.cs");
            }
            else
            {
                return ExistsFileInProjectPath("Services", "ActivationService.vb")
                       && ExistsFileInProjectPath("Helpers", "Observable.vb");
            }
        }

        private static bool IsWpfMVVMBasic()
        {
            return ExistsFileInProjectPath("Services", "ApplicationHostService.cs")
                    && ExistsFileInProjectPath("Helpers", "Observable.cs")
                    && ExistsFileInProjectPath("Helpers", "RelayCommand.cs");
        }

        private static bool IsUwpTabbedNav()
        {
            // TabbedNav implementation is equal to SplitView but winui:NavigationView contains
            // a property PaneDisplayMode="Top"
            if (IsUwpSplitView())
            {
                return FileContainsLine("Views", "ShellPage.xaml", "PaneDisplayMode=\"Top\"");
            }

            return false;
        }

        private static bool IsUwpCodeBehind()
        {
            if (IsCSharpProject())
            {
                if (ExistsFileInProjectPath("Services", "ActivationService.cs"))
                {
                    var codebehindFile = Directory.GetFiles(Path.Combine(GenContext.ToolBox.Shell.GetActiveProjectPath(), "Views"), "*.xaml.cs", SearchOption.TopDirectoryOnly).FirstOrDefault();
                    if (!string.IsNullOrEmpty(codebehindFile))
                    {
                        var fileContent = File.ReadAllText(codebehindFile);
                        return fileContent.Contains("INotifyPropertyChanged") &&
                               fileContent.Contains("public event PropertyChangedEventHandler PropertyChanged;");
                    }
                }
            }
            else
            {
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
            }

            return false;
        }

        private static bool IsUwpCaliburnMicro()
        {
            return (ExistsFileInProjectPath("Services", "ActivationService.cs") || ExistsFileInProjectPath("Services", "ActivationService.vb"))
                && ContainsNugetPackage("Caliburn.Micro");
        }

        private static bool IsUwpMicrosoftToolkitMvvm()
        {
            return ContainsNugetPackage("Microsoft.Toolkit.MVVM");
        }

        private static bool IsUwpPrism()
        {
            return ExistsFileInProjectPath("Constants", "PageTokens.cs")
                && ContainsNugetPackage("Prism.Unity");
        }

        private static bool IsWpfPrism()
        {
            return ContainsNugetPackage("Prism.Unity");
        }

        private static bool IsUwpSplitView()
        {
            if (IsCSharpProject())
            {
                // Prism doesn't have an activation service, but will have PageToken constants
                return ExistsFileInProjectPath("Views", "ShellPage.xaml")
                    && (ExistsFileInProjectPath("Services", "ActivationService.cs") || ExistsFileInProjectPath("Constants", "PageTokens.cs"));
            }
            else
            {
                return ExistsFileInProjectPath("Services", "ActivationService.vb")
                    && ExistsFileInProjectPath("Views", "ShellPage.xaml");
            }
        }

        private static bool IsWpfSplitView()
        {
            return ExistsFileInProjectPath("Views", "ShellWindow.xaml")
                && FileContainsLine("Views", "ShellWindow.xaml", "<controls:HamburgerMenu");
        }

        private static bool IsCSharpProject()
        {
            return Directory.GetFiles(GenContext.ToolBox.Shell.GetActiveProjectPath(), "*.csproj", SearchOption.TopDirectoryOnly).Any();
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
                if (File.ReadAllText(file).Contains($"<PackageReference Include=\"{packageId}"))
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
    }
}
