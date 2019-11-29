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

        private const string PlUwp = "Uwp";

        private const string ProjTypeBlank = "Blank";
        private const string ProjTypeSplitView = "SplitView";
        private const string ProjTypeTabbedNav = "TabbedNav";
        private const string ProjTypeMenuBar = "MenuBar";

        public static ProjectMetadata ReadProjectConfiguration()
        {
            var projectMetadata = ProjectMetadataService.GetProjectMetadata();

            if (IsValid(projectMetadata))
            {
                return projectMetadata;
            }

            var inferredConfig = InferProjectConfiguration(projectMetadata);

            if (IsValid(inferredConfig))
            {
                ProjectMetadataService.SaveProjectMetadata(inferredConfig);
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
                    data.ProjectType = InferProjectType();
                }

                if (string.IsNullOrEmpty(data.Framework))
                {
                    data.Framework = InferFramework();
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

            throw new Exception(StringRes.ErrorUnableResolvePlatform);
        }

        private static bool IsUwp()
        {
            var projectTypeGuids = GenContext.ToolBox.Shell.GetActiveProjectTypeGuids();

            if (projectTypeGuids.ToUpperInvariant().Split(';').Contains("{A5A43C5B-DE2A-4C0C-9213-0A381AF9435A}"))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private static string InferFramework()
        {
            if (IsMVVMBasic())
            {
                return FxMVVMBasic;
            }
            else if (IsMVVMLight())
            {
                return FxMVVMLight;
            }
            else if (IsCodeBehind())
            {
                return FxCodeBehid;
            }
            else if (IsCaliburnMicro())
            {
                return FxCaliburnMicro;
            }
            else if (IsPrism())
            {
                return FxPrism;
            }
            else
            {
                return string.Empty;
            }
        }

        private static string InferProjectType()
        {
            if (IsTabbedNav())
            {
                return ProjTypeTabbedNav;
            }
            else if (IsMenuBar())
            {
                return ProjTypeMenuBar;
            }
            else if (IsSplitView())
            {
                return ProjTypeSplitView;
            }
            else if (IsBlank())
            {
                return ProjTypeBlank;
            }
            else
            {
                return string.Empty;
            }
        }

        private static bool IsBlank()
        {
            return !(ExistsFileInProjectPath("Views", "ShellPage.xaml")
                || ExistsFileInProjectPath("Views", "PivotPage.xaml"));
        }

        private static bool IsMenuBar()
        {
            return ExistsFileInProjectPath("Views", "ShellPage.xaml")
                && (ExistsFileInProjectPath("Helpers", "MenuNavigationHelper.cs") || ExistsFileInProjectPath("Helpers", "MenuNavigationHelper.vb") || ExistsFileInProjectPath("Services", "MenuNavigationService.cs"));
        }

        private static bool IsMVVMLight()
        {
            if (ExistsFileInProjectPath("Services", "ActivationService.cs") || ExistsFileInProjectPath("Services", "ActivationService.vb"))
            {
                var files = Directory.GetFiles(GenContext.ToolBox.Shell.GetActiveProjectPath(), "*.*proj", SearchOption.TopDirectoryOnly);
                foreach (string file in files)
                {
                    // Allow for reference names "MvvmLight" & "MvvmLightStd10"
                    if (File.ReadAllText(file).Contains("<PackageReference Include=\"MvvmLight"))
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        private static bool IsMVVMBasic()
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

        private static bool IsTabbedNav()
        {
            // TabbedNav implementation is equal to SplitView but winui:NavigationView contains
            // a property PaneDisplayMode="Top"
            if (IsSplitView())
            {
                return FileContainsLine("Views", "ShellPage.xaml", "PaneDisplayMode=\"Top\"");
            }

            return false;
        }

        private static bool IsCodeBehind()
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

        private static bool IsCaliburnMicro()
        {
            if (ExistsFileInProjectPath("Services", "ActivationService.cs") || ExistsFileInProjectPath("Services", "ActivationService.vb"))
            {
                var files = Directory.GetFiles(GenContext.ToolBox.Shell.GetActiveProjectPath(), "*.*proj", SearchOption.TopDirectoryOnly);
                foreach (string file in files)
                {
                    if (File.ReadAllText(file).Contains("<PackageReference Include=\"Caliburn.Micro\">"))
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        private static bool IsPrism()
        {
            if (ExistsFileInProjectPath("Constants", "PageTokens.cs"))
            {
                var files = Directory.GetFiles(GenContext.ToolBox.Shell.GetActiveProjectPath(), "*.*proj", SearchOption.TopDirectoryOnly);
                foreach (string file in files)
                {
                    if (File.ReadAllText(file).Contains("<PackageReference Include=\"Prism.Unity\">"))
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        private static bool IsSplitView()
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
    }
}
