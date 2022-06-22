﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;

namespace Localization
{
    public static class Routes
    {
        // TODO: Add WinUI RuntimeComponent and ClassLibrary solutions when add multilanguage files to this projects
        internal static (string Path, string FileName, string FileNamePattern)[] VSProjectTemplatePaths { get; } = new[]
        {
            (@"code\src\ProjectTemplates\UWP\CS\CSharp.UWP.Solution", "CSharp.UWP.Solution.vstemplate", "CSharp.UWP.Solution.{0}.vstemplate"),
            (@"code\src\ProjectTemplates\UWP\VB\VBNet.UWP.Solution", "VBNet.UWP.Solution.vstemplate", "VBNet.UWP.Solution.{0}.vstemplate"),
            (@"code\src\ProjectTemplates\WinUI\Cpp\Cpp.WinUI.Desktop.Solution", "Cpp.WinUI.Desktop.Solution.vstemplate", "Cpp.WinUI.Desktop.Solution.{0}.vstemplate"),
            (@"code\src\ProjectTemplates\WinUI\Cpp\Cpp.WinUI.RuntimeComponent", "Cpp.WinUI.RuntimeComponent.vstemplate", "Cpp.WinUI.RuntimeComponent.{0}.vstemplate"),
            (@"code\src\ProjectTemplates\WinUI\Cpp\Cpp.WinUI.Uwp.Solution", "Cpp.WinUI.Uwp.Solution.vstemplate", "Cpp.WinUI.Uwp.Solution.{0}.vstemplate"),
            (@"code\src\ProjectTemplates\WinUI\CS\Csharp.WinUI.Desktop.ClassLibrary", "Csharp.WinUI.Desktop.ClassLibrary.vstemplate", "Csharp.WinUI.Desktop.ClassLibrary.{0}.vstemplate"),
            (@"code\src\ProjectTemplates\WinUI\CS\Csharp.WinUI.Desktop.Solution", "Csharp.WinUI.Desktop.Solution.vstemplate", "Csharp.WinUI.Desktop.Solution.{0}.vstemplate"),
            (@"code\src\ProjectTemplates\WinUI\CS\Csharp.WinUI.Uwp.ClassLibrary", "Csharp.WinUI.Uwp.ClassLibrary.vstemplate", "Csharp.WinUI.Uwp.ClassLibrary.{0}.vstemplate"),
            (@"code\src\ProjectTemplates\WinUI\CS\Csharp.WinUI.Uwp.RuntimeComponent", "Csharp.WinUI.Uwp.RuntimeComponent.vstemplate", "Csharp.WinUI.Uwp.RuntimeComponent.{0}.vstemplate"),
            (@"code\src\ProjectTemplates\WinUI\CS\Csharp.WinUI.Uwp.Solution", "Csharp.WinUI.Uwp.Solution.vstemplate", "Csharp.WinUI.Uwp.Solution.{0}.vstemplate"),
            (@"code\src\ProjectTemplates\WPF\CS\CSharp.WPF.Solution", "CSharp.WPF.Solution.vstemplate", "CSharp.WPF.Solution.{0}.vstemplate"),
        };

        internal static (string Path, string FileName, string FileNamePattern)[] VSItemTemplatePaths { get; } = new[]
        {
            (@"code\src\ItemTemplates\WinUI\Cpp\Cpp.WinUI.BlankPage", "Cpp.WinUI.BlankPage.vstemplate", "Cpp.WinUI.BlankPage.{0}.vstemplate"),
            (@"code\src\ItemTemplates\WinUI\Cpp\Cpp.WinUI.Desktop.BlankWindow", "Cpp.WinUI.Desktop.BlankWindow.vstemplate", "Cpp.WinUI.Desktop.BlankWindow.{0}.vstemplate"),
            (@"code\src\ItemTemplates\WinUI\Cpp\Cpp.WinUI.ResourceDictionary", "Cpp.WinUI.ResourceDictionary.vstemplate",  "Cpp.WinUI.ResourceDictionary.{0}.vstemplate"),
            (@"code\src\ItemTemplates\WinUI\Cpp\Cpp.WinUI.Resw", "Cpp.WinUI.Resw.vstemplate", "Cpp.WinUI.Resw.{0}.vstemplate"),
            (@"code\src\ItemTemplates\WinUI\Cpp\Cpp.WinUI.TemplatedControl", "Cpp.WinUI.TemplatedControl.vstemplate", "Cpp.WinUI.TemplatedControl.{0}.vstemplate"),
            (@"code\src\ItemTemplates\WinUI\Cpp\Cpp.WinUI.UserControl", "Cpp.WinUI.UserControl.vstemplate", "Cpp.WinUI.UserControl.{0}.vstemplate"),

            (@"code\src\ItemTemplates\WinUI\CS\Csharp.WinUI.BlankPage", "Csharp.WinUI.BlankPage.vstemplate", "Csharp.WinUI.BlankPage.{0}.vstemplate"),
            (@"code\src\ItemTemplates\WinUI\CS\Csharp.WinUI.Desktop.BlankWindow", "Csharp.Desktop.BlankWindow.vstemplate", "Csharp.Desktop.BlankWindow.{0}.vstemplate"),
            (@"code\src\ItemTemplates\WinUI\CS\Csharp.WinUI.ResourceDictionary", "Csharp.ResourceDictionary.vstemplate", "Csharp.ResourceDictionary.{0}.vstemplate"),
            (@"code\src\ItemTemplates\WinUI\CS\Csharp.WinUI.Resw", "Csharp.WinUI.Resw.vstemplate", "Csharp.WinUI.Resw.{0}.vstemplate"),
            (@"code\src\ItemTemplates\WinUI\CS\Csharp.WinUI.TemplatedControl", "Csharp.TemplatedControl.vstemplate", "Csharp.TemplatedControl.{0}.vstemplate"),
            (@"code\src\ItemTemplates\WinUI\CS\Csharp.WinUI.UserControl", "Csharp.WinUI.UserControl.vstemplate", "Csharp.WinUI.UserControl.{0}.vstemplate"),
        };

        internal const string CommandTemplateRootDirPath = "code\\src\\Installer\\Commands";
        internal const string RelayCommandFile = "RelayCommandPackage.en-US.vsct";
        internal const string VspackageFile = "VSPackage.en-US.resx";

        internal const string RelayCommandFileNamePattern = "RelayCommandPackage.{0}.vsct";
        internal const string VspackageFileNamePattern = "VSPackage.{0}.resx";

        internal const string RightClickFileSearchPattern = "*postaction.md";

        internal const string TemplatesRootDirPath = "templates";
        internal const string TemplatesPagesPatternPath = "Pages";
        internal const string TemplatesFeaturesPatternPath = "Features";
        internal const string TemplatesServicesPatternPath = "Services";
        internal const string TemplatesTestingPatternPath = "Testing";

        internal const string TemplateConfigDir = ".template.config";
        internal const string TemplateDescriptionFile = "description.md";
        internal const string TemplateJsonFile = "template.json";

        internal const string CatalogPath = "_catalog";
        internal const string WtsProjectTypes = "projectTypes";
        internal const string WtsFrameworks = "frontendframeworks";

        internal const string VsixRootDirPath = "code\\src\\Installer";
        internal const string VsixLangDirPattern = "{0}\\Extension.vsixlangpack";
        internal const string VsixManifestFile = "source.extension.vsixmanifest";
        internal const string VsixLangpackFile = "Extension.vsixlangpack";
        internal const string VsixLangpackContent = @"<?xml version=""1.0"" encoding=""utf-8"" ?>  
<VsixLanguagePack Version = ""1.0.0"" xmlns=""http://schemas.microsoft.com/developer/vsx-schema-lp/2010"" >  
  <LocalizedName>{0}</LocalizedName>
  <LocalizedDescription>{1}</LocalizedDescription>  
  <License>..\Content\EULA.{2}.rtf</License>
  <MoreInfoUrl>https://github.com/microsoft/TemplateStudio/</MoreInfoUrl>  
</VsixLanguagePack>";

        internal const string ResourcesFilePath = "Resources.resx";
        internal const string ResourcesFilePathPattern = "Resources.{0}.resx";

        internal static string[] ResoureceDirectories { get; } =
        {
            "code\\CoreTemplateStudio\\code\\src\\CoreTemplateStudio\\CoreTemplateStudio.Core\\Resources",
            "code\\src\\Installer\\Resources",
            "code\\src\\UI\\Resources",
        };

        internal static string[] TemplatesPlatforms { get; } =
        {
            "Uwp",
            "Wpf",
            "WinUI",
        };

        // Validate Routes
        internal const string RelayCommandFileNameValidate = "code\\src\\Installer\\Commands\\RelayCommandPackage.en-US.vsct";
        internal const string VspackageFileNameValidate = "code\\src\\Installer\\Commands\\VSPackage.en-US.resx";
        internal const string WtsProjectTypesValidate = "_catalog\\projectTypes.json";
        internal const string WtsFrameworksValidate = "_catalog\\frontendframeworks.json";

        // Extract files folders
        internal const string OriginalExtractDirectory = "original";
        internal const string ActualExtractDirectory = "actual";
        internal const string DiffExtractDirectory = "diff";
    }
}
