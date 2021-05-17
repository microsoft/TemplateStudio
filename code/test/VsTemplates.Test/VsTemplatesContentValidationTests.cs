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
            var filePaths = GetFilesFromPath(ProjectTemplatesRoot);
            var vsProjectTemplatesPaths = filePaths.Where(p => Path.GetExtension(p) == VsTemplateFileExtension);
            return vsProjectTemplatesPaths.Select(p => new object[] { p });
        }

        private static IEnumerable<object[]> GetAllVsItemTemplateFiles()
        {
            var filePaths = GetFilesFromPath(ItemTemplatesRoot);
            var vsItemTemplatesPaths = filePaths.Where(p => Path.GetExtension(p) == VsTemplateFileExtension);
            return vsItemTemplatesPaths.Select(p => new object[] { p });
        }

        private static IEnumerable<string> GetFilesFromPath(string directory)
        {
            foreach (var dir in Directory.GetDirectories(directory))
            {
                foreach (var file in Directory.GetFiles(dir))
                {
                    yield return file;
                }

                foreach (var file in GetFilesFromPath(dir))
                {
                    yield return file;
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
    }
}
