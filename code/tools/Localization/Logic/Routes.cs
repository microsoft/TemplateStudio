// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace Localization
{
    public static class Routes
    {
        internal static string ProjectTemplatePathCS = "code\\src\\ProjectTemplates\\CSharp.UWP.2017.Solution";
        internal static string ProjectTemplatePathVB = "code\\src\\ProjectTemplates\\VBNet.UWP.VS2017.Solution";
        internal static string ProjectTemplateFileCS = "CSharp.UWP.VS2017.Solution.vstemplate";
        internal static string ProjectTemplateFileVB = "VBNet.UWP.VS2017.Solution.vstemplate";
        internal static string ProjectTemplateFileNamePatternCS = "CSharp.UWP.VS2017.Solution.{0}.vstemplate";
        internal static string ProjectTemplateFileNamePatternVB = "VBNet.UWP.VS2017.Solution.{0}.vstemplate";

        internal static string CommandTemplateRootDirPath = "code\\src\\Installer.2017\\Commands";
        internal static string RelayCommandFileNamePattern = "RelayCommandPackage.{0}.vsct";
        internal static string VspackageFileNamePattern = "VSPackage.{0}.resx";

        internal static string[] RightClickMdsFolders = new string[] { "_composition", "Features", "Pages" };
        internal static string RightClickFileSearchPattern = "*postaction.md";

        internal static string TemplatesRootDirPath = "templates";
        internal static string TemplatesPagesPath = "templates\\Pages";
        internal static string TemplatesFeaturesPath = "templates\\Features";

        internal static string TemplateConfigDir = ".template.config";
        internal static string TemplateDescriptionFile = "description.md";
        internal static string TemplateJsonFile = "template.json";

        internal static string WtsTemplatesRootDirPath = "templates\\_catalog";
        internal static string WtsProjectTypes = "projectTypes";
        internal static string WtsFrameworks = "frameworks";

        internal static string VsixRootDirPath = "code\\src\\Installer.2017";
        internal static string VsixManifestFile = "source.extension.vsixmanifest";
        internal static string VsixLangpackFile = "Extension.vsixlangpack";
        internal static string VsixLangpackContent = @"<?xml version=""1.0"" encoding=""utf-8"" ?>  
            <VsixLanguagePack Version = ""1.0.0"" xmlns=""http://schemas.microsoft.com/developer/vsx-schema-lp/2010"" >  
              <LocalizedName>{0}</LocalizedName>  
              <LocalizedDescription>{1}</LocalizedDescription>  
              <License>..\Content\EULA.{2}.rtf</License>
              <MoreInfoUrl>https://github.com/Microsoft/WindowsTemplateStudio/</MoreInfoUrl>  
            </VsixLanguagePack> ";

        internal static string ResourcesFilePath = "StringRes.resx";
        internal static string ResourcesFilePathPattern = "StringRes.{0}.resx";
        internal static string[] ResoureceDirectories = new string[]
        {
            "code\\src\\Core\\Resources",
            "code\\src\\Installer.2017\\Resources",
            "code\\src\\UI\\Resources"
        };

        // Validate Routes
        internal static string VsixValidatePath = "code\\src\\Installer.2017\\source.extension.vsixmanifest";
        internal static string ProjectTemplateFileNameValidateCS = "code\\src\\ProjectTemplates\\CSharp.UWP.2017.Solution\\CSharp.UWP.VS2017.Solution.vstemplate";
        internal static string ProjectTemplateFileNameValidateVB = "code\\src\\ProjectTemplates\\VBNet.UWP.2017.Solution\\VBNet.UWP.VS2017.Solution.vstemplate";
        internal static string RelayCommandFileNameValidate = "code\\src\\Installer.2017\\Commands\\RelayCommandPackage.en-US.vsct";
        internal static string VspackageFileNameValidate = "code\\src\\Installer.2017\\Commands\\VSPackage.en-US.resx";
        internal static string WtsProjectTypesValidate = "templates\\_catalog\\projectTypes.json";
        internal static string WtsFrameworksValidate = "templates\\_catalog\\frameworks.json";
    }
}
