// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using VsTemplates.Test.Models;
using VsTemplates.Test.Validator;
using Xunit;

namespace VsTemplates.Test
{
    [Trait("Type", "VsTemplateValidation")]
    [Trait("ExecutionSet", "Minimum")]
    [Trait("ExecutionSet", "VsTemplateValidation")]
    [Trait("ExecutionSet", "_CIBuild")]
    [Trait("ExecutionSet", "_Full")]
    public class VsTemplatesContentValidationTests
    {
        private const string ItemTemplatesRoot = "../../../../../src/ItemTemplates";
        private const string ProjectTemplatesRoot = "../../../../../src/ProjectTemplates";
        private const string VsTemplateFileExtension = ".vstemplate";
        private const string WPF = "Wpf";
        private const string UWP = "Uwp";
        private const string WINUI = "WinUI";

        private const string DESKTOP = "Desktop";

        private const string CSHARP = "C#";
        private const string VB = "VisualBasic";
        private const string CPP = "C++";

        private static readonly List<VSTemplateData> WizardProjectTemplatesData = new List<VSTemplateData>
        {
            new VSTemplateData(WPF, string.Empty, CSHARP, "../../../../../src/ProjectTemplates/WPF"),
            new VSTemplateData(UWP, string.Empty, CSHARP, "../../../../../src/ProjectTemplates/UWP/CS"),
            new VSTemplateData(UWP, string.Empty, VB, "../../../../../src/ProjectTemplates/UWP/VB"),
            new VSTemplateData(WINUI, DESKTOP, CPP, "../../../../../src/ProjectTemplates/WinUI/Cpp/Cpp.WinUI.Desktop.Solution"),
            new VSTemplateData(WINUI, UWP, CPP, "../../../../../src/ProjectTemplates/WinUI/Cpp/Cpp.WinUI.UWP.Solution"),
            new VSTemplateData(WINUI, DESKTOP, CSHARP, "../../../../../src/ProjectTemplates/WinUI/CS/CSharp.WinUI.Desktop.Solution"),
            new VSTemplateData(WINUI, UWP, CSHARP, "../../../../../src/ProjectTemplates/WinUI/CS/CSharp.WinUI.Uwp.Solution"),
        };

        public static IEnumerable<object[]> GetWizardProjectTemplateFiles()
        {
            var vsTemplatesPaths = new List<object[]>();
            foreach (var templateData in WizardProjectTemplatesData)
            {
                var filePaths = Directory.GetFiles(templateData.RootPath, $"*{VsTemplateFileExtension}", SearchOption.AllDirectories);
                vsTemplatesPaths.AddRange(filePaths.Select(p => new object[] { p, templateData.Platform, templateData.AppModel, templateData.Language }));
            }

            return vsTemplatesPaths;
        }

        public static IEnumerable<object[]> GetAllVsTemplatePaths()
        {
            var vsTemplatesPaths = new List<object[]>();
            var vsItemTemplatesPaths = GetDirectoriesContainsVsTemplateFiles(ItemTemplatesRoot).Select(p => new object[] { p });
            var vsProjectTemplatesPaths = GetDirectoriesContainsVsTemplateFiles(ProjectTemplatesRoot).Select(p => new object[] { p });
            vsTemplatesPaths.AddRange(vsItemTemplatesPaths);
            vsTemplatesPaths.AddRange(vsProjectTemplatesPaths);
            return vsTemplatesPaths;
        }

        public static IEnumerable<object[]> GetAllVsTemplateFiles()
        {
            var vsTemplatesPaths = new List<object[]>();
            var vsItemTemplatesPaths = GetAllVsItemTemplateFiles();
            var vsProjectTemplatesPaths = GetAllVsProjectTemplateFiles();
            vsTemplatesPaths.AddRange(vsItemTemplatesPaths);
            vsTemplatesPaths.AddRange(vsProjectTemplatesPaths);
            return vsTemplatesPaths;
        }

        public static IEnumerable<object[]> GetAllVsProjectTemplateFiles()
        {
            var filePaths = Directory.GetFiles(ProjectTemplatesRoot, $"*{VsTemplateFileExtension}", SearchOption.AllDirectories);
            return filePaths.Select(p => new object[] { p });
        }

        private static IEnumerable<object[]> GetAllVsItemTemplateFiles()
        {
            var filePaths = Directory.GetFiles(ItemTemplatesRoot, $"*{VsTemplateFileExtension}", SearchOption.AllDirectories);
            return filePaths.Select(p => new object[] { p });
        }

        private static IEnumerable<string> GetDirectoriesContainsVsTemplateFiles(string directory)
        {
            foreach (var dir in Directory.GetDirectories(directory))
            {
                if (Directory.GetFiles(dir, $"*{VsTemplateFileExtension}", SearchOption.TopDirectoryOnly).Any())
                {
                    yield return dir;
                }
                else
                {
                    foreach (var subDir in GetDirectoriesContainsVsTemplateFiles(dir))
                    {
                        yield return subDir;
                    }
                }
            }
        }

        [Theory]
        [MemberData(nameof(GetAllVsTemplateFiles))]
        public void VerifyTemplateId(string filePath)
        {
            var result = VsTemplateValidator.VerifyTemplateId(filePath);
            Assert.True(result.Success, $"{filePath}: " + result.Message);
        }

        [Theory]
        [MemberData(nameof(GetAllVsTemplateFiles))]
        public void VerifyTemplateNames(string filePath)
        {
            var result = VsTemplateValidator.VerifyTemplateName(filePath);
            Assert.True(result.Success, $"{filePath}: " + result.Message);
        }

        [Theory]
        [MemberData(nameof(GetAllVsProjectTemplateFiles))]
        public void VerifyProjectTemplatesIncludeWtsTag(string filePath)
        {
            var result = VsTemplateValidator.VerifyProjectTemplatesIncludeWtsTag(filePath);
            Assert.True(result.Success, $"{filePath}: " + result.Message);
        }

        [Theory]
        [MemberData(nameof(GetWizardProjectTemplateFiles))]
        public void VerifyWizardProjectTemplatesIncludeCustomParameters(string filePath, string platform, string appModel, string language)
        {
            var hasPlatform = VsTemplateValidator.VerifyWizardProjectTemplatesIncludesCustomParameter(filePath, "$wts.platform$", platform);
            Assert.True(hasPlatform.Success, $"{filePath}: " + hasPlatform.Message);

            var hasLanguage = VsTemplateValidator.VerifyWizardProjectTemplatesIncludesCustomParameter(filePath, "$wts.language$", language);
            Assert.True(hasLanguage.Success, $"{filePath}: " + hasLanguage.Message);

            if (!string.IsNullOrEmpty(appModel))
            {
                var hasAppModel = VsTemplateValidator.VerifyWizardProjectTemplatesIncludesCustomParameter(filePath, "$wts.appmodel$", appModel);
                Assert.True(hasAppModel.Success, $"{filePath}: " + hasPlatform.Message);
            }
        }

        [Theory]
        [MemberData(nameof(GetAllVsTemplatePaths))]
        public void VerifyAllLocalizedTemplatesHaveSameDefinition(string filePath)
        {
            var pathFiles = Directory.GetFiles(filePath, $"*{VsTemplateFileExtension}", SearchOption.AllDirectories);
            var result = VsTemplateValidator.VerifyAllLocalizedTemplatesHaveSameDefinition(pathFiles);
            Assert.True(result.Success, $"{filePath}: " + result.Message);
        }
    }
}
