// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Xml.Linq;

using Microsoft.Templates.Core;
using Microsoft.Templates.Core.Diagnostics;
using Microsoft.Templates.Core.Gen;
using Microsoft.Templates.UI.Resources;

namespace Microsoft.Templates.UI.Generation
{
    public class ProjectConfigInfo
    {
        public const string FxMVVMBasic = "MVVMBasic";
        public const string FxMVVMLight = "MVVMLight";
        public const string FxCodeBehid = "CodeBehind";
        public const string FxCaliburnMicro = "CaliburnMicro";
        public const string FxPrism = "Prism";

        private const string ProjTypeBlank = "Blank";
        private const string ProjTypeSplitView = "SplitView";
        private const string ProjTypeTabbedPivot = "TabbedPivot";

        private const string PlUwp = "Uwp";
        private const string PlXamarin = "Xamarin";

        private const string ProjectTypeLiteral = "projectType";
        private const string FrameworkLiteral = "framework";
        private const string PlatformLiteral = "platform";
        private const string MetadataLiteral = "Metadata";
        private const string NameAttribLiteral = "Name";
        private const string ValueAttribLiteral = "Value";
        private const string ItemLiteral = "Item";

        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.SpacingRules", "SA1008:Opening parenthesis must be spaced correctly", Justification = "Using tuples must allow to have preceding whitespace", Scope = "member")]
        public static (string ProjectType, string Framework, string Platform) ReadProjectConfiguration()
        {
            try
            {
                // TODO: Adapt to read project configuration for xamarin forms (X-ref: https://github.com/Microsoft/WindowsTemplateStudio/issues/1350)
                var path = Path.Combine(GenContext.ToolBox.Shell.GetActiveProjectPath(), "Package.appxmanifest");
                if (File.Exists(path))
                {
                    var manifest = XElement.Load(path);
                    XNamespace ns = "http://schemas.microsoft.com/appx/developer/windowsTemplateStudio";

                    var metadata = manifest.Descendants().FirstOrDefault(e => e.Name.LocalName == MetadataLiteral && e.Name.Namespace == ns);

                    var projectType = metadata?.Descendants().FirstOrDefault(m => m.Attribute(NameAttribLiteral)?.Value == ProjectTypeLiteral)?.Attribute(ValueAttribLiteral)?.Value;
                    var framework = metadata?.Descendants().FirstOrDefault(m => m.Attribute(NameAttribLiteral)?.Value == FrameworkLiteral)?.Attribute(ValueAttribLiteral)?.Value;
                    var platform = metadata?.Descendants().FirstOrDefault(m => m.Attribute(NameAttribLiteral)?.Value == PlatformLiteral)?.Attribute(ValueAttribLiteral)?.Value;

                    if (!string.IsNullOrEmpty(projectType) && !string.IsNullOrEmpty(framework) && !string.IsNullOrEmpty(platform))
                    {
                        return (projectType, framework, platform);
                    }
                    else
                    {
                        var inferredConfig = InferProjectConfiguration(projectType, framework, platform);
                        if (!string.IsNullOrEmpty(inferredConfig.ProjectType) && !string.IsNullOrEmpty(inferredConfig.Framework) && !string.IsNullOrEmpty(inferredConfig.Platform))
                        {
                            SaveProjectConfiguration(inferredConfig.ProjectType, inferredConfig.Framework, inferredConfig.Platform);
                        }

                        return inferredConfig;
                    }
                }

                return (string.Empty, string.Empty, string.Empty);
            }
            catch (Exception ex)
            {
                AppHealth.Current.Warning.TrackAsync("Exception reading projectType and framework from Package.appxmanifest", ex).FireAndForget();
                return (string.Empty, string.Empty, string.Empty);
            }
        }

        public static void SaveProjectConfiguration(string projectType, string framework, string platform)
        {
            try
            {
                var path = Path.Combine(GenContext.ToolBox.Shell.GetActiveProjectPath(), "Package.appxmanifest");
                if (File.Exists(path))
                {
                    var manifest = XElement.Load(path);
                    XNamespace ns = "http://schemas.microsoft.com/appx/developer/windowsTemplateStudio";

                    var metadata = manifest.Descendants().FirstOrDefault(e => e.Name.LocalName == MetadataLiteral && e.Name.Namespace == ns);
                    if (metadata?.Descendants().FirstOrDefault(m => m.Attribute(NameAttribLiteral)?.Value == ProjectTypeLiteral) == null)
                    {
                        metadata.Add(new XElement(ns + ItemLiteral, new XAttribute(NameAttribLiteral, ProjectTypeLiteral), new XAttribute(ValueAttribLiteral, projectType)));
                    }

                    if (metadata?.Descendants().FirstOrDefault(m => m.Attribute(NameAttribLiteral)?.Value == FrameworkLiteral) == null)
                    {
                        metadata.Add(new XElement(ns + ItemLiteral, new XAttribute(NameAttribLiteral, FrameworkLiteral), new XAttribute(ValueAttribLiteral, framework)));
                    }

                    if (metadata?.Descendants().FirstOrDefault(m => m.Attribute(NameAttribLiteral)?.Value == PlatformLiteral) == null)
                    {
                        metadata.Add(new XElement(ns + ItemLiteral, new XAttribute(NameAttribLiteral, PlatformLiteral), new XAttribute(ValueAttribLiteral, platform)));
                    }

                    manifest.Save(path);
                }
            }
            catch (Exception ex)
            {
                AppHealth.Current.Warning.TrackAsync("Exception saving inferred projectType and framework to Package.appxmanifest", ex).FireAndForget();
                throw;
            }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.SpacingRules", "SA1008:Opening parenthesis must be spaced correctly", Justification = "Using tuples must allow to have preceding whitespace", Scope = "member")]
        private static (string ProjectType, string Framework, string Platform) InferProjectConfiguration(string projectType, string framework, string platform)
        {
            if (string.IsNullOrEmpty(platform))
            {
                platform = InferPlatform();
            }

            if (platform == PlUwp)
            {
                if (string.IsNullOrEmpty(projectType))
                {
                    projectType = InferProjectType();
                }

                if (string.IsNullOrEmpty(framework))
                {
                    framework = InferFramework();
                }

                return (projectType, framework, platform);
            }

            // TODO: Infer projectType y framework for Xamarin forms
            return (string.Empty, string.Empty, string.Empty);
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

        public static string InferPlatform()
        {
            if (IsXamarin())
            {
                return Platforms.Xamarin;
            }

            if (IsUwp())
            {
                return Platforms.Uwp;
            }

            throw new Exception(StringRes.ExceptionUnableResolvePlatform);
        }

        private static bool IsXamarin()
        {
            var searchPath = new DirectoryInfo(GenContext.ToolBox.Shell.GetActiveProjectPath()).Parent.FullName;
            string[] fileExtensions = { ".json", ".config", ".csproj" };

            var files = Directory.GetFiles(searchPath, "*.*", SearchOption.AllDirectories)
                    .Where(f => fileExtensions.Contains(Path.GetExtension(f)));

            foreach (string file in files)
            {
                if (File.ReadAllText(file).Contains("Xamarin.Forms"))
                {
                    return true;
                }
            }

            return false;
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

        private static string InferProjectType()
        {
            if (IsSplitView())
            {
                return ProjTypeSplitView;
            }
            else if (IsTabbedPivot())
            {
                return ProjTypeTabbedPivot;
            }
            else
            {
                return ProjTypeBlank;
            }
        }

        private static bool IsMVVMLight()
        {
            if (ExistsFileInProjectPath("Services", "ActivationService.cs") || ExistsFileInProjectPath("Services", "ActivationService.vb"))
            {
                var files = Directory.GetFiles(GenContext.ToolBox.Shell.GetActiveProjectPath(), "*.*proj", SearchOption.TopDirectoryOnly);
                foreach (string file in files)
                {
                    if (File.ReadAllText(file).Contains("<PackageReference Include=\"MvvmLight\">"))
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

        private static bool IsTabbedPivot()
        {
            return ((ExistsFileInProjectPath("Services", "ActivationService.cs") || ExistsFileInProjectPath("Services", "ActivationService.vb"))
                && ExistsFileInProjectPath("Views", "PivotPage.xaml")) || (ExistsFileInProjectPath("Constants", "PageTokens.cs")
                && ExistsFileInProjectPath("Views", "PivotPage.xaml"));
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
                return (ExistsFileInProjectPath("Services", "ActivationService.cs") && ExistsFileInProjectPath("Views", "ShellPage.xaml")
                    && (ExistsFileInProjectPath("Views", "ShellNavigationItem.cs") || ExistsFileInProjectPath("ViewModels", "ShellNavigationItem.cs")))
                    || (ExistsFileInProjectPath("ViewModels", "ShellNavigationItem.cs") && ExistsFileInProjectPath("Constants", "PageTokens.cs"));
            }
            else
            {
                return ExistsFileInProjectPath("Services", "ActivationService.vb")
                    && ExistsFileInProjectPath("Views", "ShellPage.xaml")
                    && (ExistsFileInProjectPath("Views", "ShellNavigationItem.vb") || ExistsFileInProjectPath("ViewModels", "ShellNavigationItem.vb"));
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
                return Directory.GetFiles(Path.Combine(GenContext.ToolBox.Shell.GetActiveProjectPath(), subPath), fileName, SearchOption.TopDirectoryOnly).Any();
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
