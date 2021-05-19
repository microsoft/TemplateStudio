// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;
using System.IO;
using System.Linq;
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
        private static readonly List<string> WizardProjectTemplatesRoot = new List<string>
        {
            "../../../../../src/ProjectTemplates/UWP",
            "../../../../../src/ProjectTemplates/WPF",
            "../../../../../src/ProjectTemplates/WinUI/Cpp/Cpp.WinUI.Desktop.Solution",
            "../../../../../src/ProjectTemplates/WinUI/Cpp/Cpp.WinUI.UWP.Solution",
            "../../../../../src/ProjectTemplates/WinUI/CS/CSharp.WinUI.Desktop.Solution",
            "../../../../../src/ProjectTemplates/WinUI/CS/CSharp.WinUI.Uwp.Solution",
        };

        public static IEnumerable<object[]> GetWizardProjectTemplateFiles()
        {
            var vsTemplatesPaths = new List<object[]>();
            foreach (var path in WizardProjectTemplatesRoot)
            {
                var filePaths = Directory.GetFiles(path, $"*{VsTemplateFileExtension}", SearchOption.AllDirectories);
                vsTemplatesPaths.AddRange(filePaths.Select(p => new object[] { p }));
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
        public void VerifyWizardProjectTemplatesIncludePlatformTag(string filePath)
        {
            var result = VsTemplateValidator.VerifyWizardProjectTemplatesIncludePlatformTag(filePath);
            Assert.True(result.Success, $"{filePath}: " + result.Message);
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
