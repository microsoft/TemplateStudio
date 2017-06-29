// ******************************************************************
// Copyright (c) Microsoft. All rights reserved.
// This code is licensed under the MIT License (MIT).
// THE CODE IS PROVIDED “AS IS”, WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED,
// INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.
// IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
// DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT,
// TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH
// THE CODE OR THE USE OR OTHER DEALINGS IN THE CODE.
// ******************************************************************

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

using Microsoft.Templates.Core.Gen;
using Microsoft.Templates.Core.Diagnostics;
using Microsoft.Templates.Core;

namespace Microsoft.Templates.UI.Generation
{
    internal class ProjectConfigInfo
    {
        const string FxMVVMBasic = "MVVMBasic";
        const string FxMVVMLight = "MVVMLight";
        const string FxCodeBehid = "CodeBehind";

        const string ProjTypeBlank = "Blank";
        const string ProjTypeSplitView = "SplitView";
        const string ProjTypeTabbedPivot = "TabbedPivot";

        const string ProjectTypeLiteral = "projectType";
        const string FrameworkLiteral = "framework";
        const string MetadataLiteral = "Metadata";
        const string NameAttribLiteral = "Name";
        const string ValueAttribLiteral = "Value";
        const string ItemLiteral = "Item";

        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.SpacingRules", "SA1008:Opening parenthesis must be spaced correctly", Justification = "Using tuples must allow to have preceding whitespace", Scope = "member")]
        public static (string ProjectType, string Framework) ReadProjectConfiguration()
        {
            try
            {
                var path = Path.Combine(GenContext.Current.ProjectPath, "Package.appxmanifest");
                if (File.Exists(path))
                {
                    var manifest = XElement.Load(path);
                    XNamespace ns = "http://schemas.microsoft.com/appx/developer/windowsTemplateStudio";

                    var metadata = manifest.Descendants().FirstOrDefault(e => e.Name.LocalName == MetadataLiteral && e.Name.Namespace == ns);

                    var projectType = metadata?.Descendants().FirstOrDefault(m => m.Attribute(NameAttribLiteral)?.Value == ProjectTypeLiteral)?.Attribute(ValueAttribLiteral)?.Value;
                    var framework = metadata?.Descendants().FirstOrDefault(m => m.Attribute(NameAttribLiteral)?.Value == FrameworkLiteral)?.Attribute(ValueAttribLiteral)?.Value;
                    if (!string.IsNullOrEmpty(projectType) && !string.IsNullOrEmpty(framework))
                    {
                        return (projectType, framework);
                    }
                    else
                    {
                        var inferredConfig = InferProjectConfiguration();
                        if (!string.IsNullOrEmpty(inferredConfig.ProjectType) && !string.IsNullOrEmpty(inferredConfig.Framework))
                        {
                            SaveProjectConfiguration(inferredConfig.ProjectType, inferredConfig.Framework);
                        }
                        return inferredConfig;
                    }
                }
                return (string.Empty, string.Empty);
            }
            catch (Exception ex)
            {
                AppHealth.Current.Warning.TrackAsync("Exception reading projectType and framework from Package.appxmanifest", ex).FireAndForget();
                return (string.Empty, string.Empty);
            }
        }

        public static void SaveProjectConfiguration(string projectType, string framework)
        {
            try
            {
                var path = Path.Combine(GenContext.Current.ProjectPath, "Package.appxmanifest");
                if (File.Exists(path))
                {
                    var manifest = XElement.Load(path);
                    XNamespace ns = "http://schemas.microsoft.com/appx/developer/windowsTemplateStudio";

                    var metadata = manifest.Descendants().FirstOrDefault(e => e.Name.LocalName == MetadataLiteral && e.Name.Namespace == ns);
                    metadata.Add(new XElement(ns + ItemLiteral, new XAttribute(NameAttribLiteral, ProjectTypeLiteral), new XAttribute(ValueAttribLiteral, projectType)));
                    metadata.Add(new XElement(ns + ItemLiteral, new XAttribute(NameAttribLiteral, FrameworkLiteral), new XAttribute(ValueAttribLiteral, framework)));

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
        private static (string ProjectType, string Framework) InferProjectConfiguration()
        {
            var projectType = InferProjectType();
            var framework = InferFramework();
            return (projectType, framework);
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
            else
            {
                return string.Empty;
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
            if (ExistsFileInProjectPath("Services", "ActivationService.cs"))
            {
                var files = Directory.GetFiles(GenContext.Current.ProjectPath, "*.*proj", SearchOption.TopDirectoryOnly);
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
            return ExistsFileInProjectPath("Services", "ActivationServiHace.cs")
                && ExistsFileInProjectPath("Helpers", "Observable.cs");
        }

        private static bool IsTabbedPivot()
        {
            return ExistsFileInProjectPath("Services", "ActivationService.cs")
                && ExistsFileInProjectPath("Views", "PivotPage.xaml");
        }

        private static bool IsCodeBehind()
        {
            if (ExistsFileInProjectPath("Services", "ActivationService.cs"))
            {
                var codebehindFile = Directory.GetFiles(Path.Combine(GenContext.Current.ProjectPath, "Views"), "*.xaml.cs", SearchOption.TopDirectoryOnly).FirstOrDefault();
                if (!string.IsNullOrEmpty(codebehindFile))
                {
                    var fileContent = File.ReadAllText(codebehindFile);
                    return fileContent.Contains($"INotifyPropertyChanged") &&
                        fileContent.Contains("public event PropertyChangedEventHandler PropertyChanged;");
                }
            }
            return false;
        }

        private static bool IsSplitView()
        {
            return ExistsFileInProjectPath("Services", "ActivationService.cs")
                && ExistsFileInProjectPath("Views", "ShellPage.xaml")
                && (ExistsFileInProjectPath("Views", "ShellNavigationItem.cs") || ExistsFileInProjectPath("ViewModels", "ShellNavigationItem.cs"));
        }

        private static bool ExistsFileInProjectPath(string subPath, string fileName)
        {
            return Directory.GetFiles(Path.Combine(GenContext.Current.ProjectPath, subPath), fileName, SearchOption.TopDirectoryOnly).Count() > 0;
        }
    }
}
