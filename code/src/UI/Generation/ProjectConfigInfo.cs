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

namespace Microsoft.Templates.UI.Generation
{
    internal class ProjectConfigInfo
    {
        const string FxMVVMBasic = "MVVMBasic";
        const string FxMVVMLight = "MVVMLight";
        const string FxCodeBehid = "Codebehind";

        const string ProjTypeBlank = "Blank";
        const string ProjTypeSplitView = "SplitView";
        const string ProjTypeTabbedPivot = "TabbedPivot";

        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.SpacingRules", "SA1008:Opening parenthesis must be spaced correctly", Justification = "Using tuples must allow to have preceding whitespace", Scope = "member")]
        public static (string ProjectType, string Framework) ReadProjectConfiguration()
        {
            var path = Path.Combine(GenContext.Current.ProjectPath, "Package.appxmanifest");
            if (File.Exists(path))
            {
                var manifest = XElement.Load(path);
                XNamespace ns = "http://schemas.microsoft.com/appx/developer/windowsTemplateStudio";

                var metadata = manifest.Descendants().FirstOrDefault(e => e.Name.LocalName == "Metadata" && e.Name.Namespace == ns);

                var projectType = metadata?.Descendants().FirstOrDefault(m => m.Attribute("Name").Value == "projectType")?.Attribute("Value")?.Value;
                var framework = metadata?.Descendants().FirstOrDefault(m => m.Attribute("Name").Value == "framework")?.Attribute("Value")?.Value;
                if (!string.IsNullOrEmpty(projectType) && !string.IsNullOrEmpty(framework))
                {
                    return (projectType, framework);
                }
                else
                {
                    var inferedConfig = InferProjectConfiguration();
                    if (!string.IsNullOrEmpty(inferedConfig.ProjectType) && !string.IsNullOrEmpty(inferedConfig.Framework))
                    {
                        SaveProjectConfiguration(inferedConfig.ProjectType, inferedConfig.Framework);
                    }
                    return inferedConfig;
                }
            }
            return (string.Empty, string.Empty);
        }

        private static void SaveProjectConfiguration(string projectType, string framework)
        {
            var path = Path.Combine(GenContext.Current.ProjectPath, "Package.appxmanifest");
            if (File.Exists(path))
            {
                var manifest = XElement.Load(path);
                XNamespace ns = "http://schemas.microsoft.com/appx/developer/windowsTemplateStudio";
                manifest.GetPrefixOfNamespace(ns);
                var metadata = manifest.Descendants().FirstOrDefault(e => e.Name.LocalName == "Metadata" && e.Name.Namespace == ns);
                metadata.Add(new XElement(ns + "Item", new XAttribute("projectType", projectType)));
                metadata.Add(new XElement(ns + "Item", new XAttribute("framework", framework)));

                manifest.Save(path);
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
            else
            {
                return FxCodeBehid;
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
            var files = Directory.GetFiles(GenContext.Current.ProjectPath, "*.*proj", SearchOption.TopDirectoryOnly);
            foreach (string file in files)
            {
                if (File.ReadAllText(file).Contains("<PackageReference Include=\"MvvmLight\"> csproj"))
                {
                    return true;
                }
            }
            return false;
        }

        private static bool IsMVVMBasic()
        {
            return ExistsFileInProjectPath("Helpers", "Observable.cs");
        }

        private static bool IsTabbedPivot()
        {
            return ExistsFileInProjectPath("Views", "PivotPage.xaml");
        }

        private static bool IsSplitView()
        {
            return ExistsFileInProjectPath("Views", "ShellPage.xaml") && (ExistsFileInProjectPath("Views", "ShellNavigationItem.cs") || ExistsFileInProjectPath("ViewModels", "ShellNavigationItem.cs"));
        }

        private static bool ExistsFileInProjectPath(string subPath, string fileName)
        {
            return Directory.GetFiles(Path.Combine(GenContext.Current.ProjectPath, subPath), fileName, SearchOption.TopDirectoryOnly).Count() > 0;
        }
    }
}
