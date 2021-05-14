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
        private const string ItemTemplatesRoot = "../../../../src/ItemTemplates";
        private const string ProjectTemplatesRoot = "../../../../src/ProjectTemplates";
        private const string VsTemplateFileExtension = ".vstemplate";

        public static IEnumerable<object[]> GetAllTemplateFiles()
        {
            var vsTemplatesPaths = new List<string>();
            var vsItemTemplatesPaths = GetAllItemTemplateFiles();
            var vsProjectTemplatesPaths = GetAllProjectTemplateFiles();
            vsTemplatesPaths.AddRange(vsItemTemplatesPaths);
            vsTemplatesPaths.AddRange(vsProjectTemplatesPaths);
            return vsTemplatesPaths.Select(p => new object[] { p });
        }

        private static IEnumerable<string> GetAllItemTemplateFiles()
        {
            var filePaths = GetFilesFromPath(ItemTemplatesRoot);
            var vsItemTemplatesPaths = filePaths.Where(p => Path.GetExtension(p) == VsTemplateFileExtension);
            return vsItemTemplatesPaths;
        }

        private static IEnumerable<string> GetAllProjectTemplateFiles()
        {
            var filePaths = GetFilesFromPath(ProjectTemplatesRoot);
            var vsProjectTemplatesPaths = filePaths.Where(p => Path.GetExtension(p) == VsTemplateFileExtension);
            return vsProjectTemplatesPaths;
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
        [MemberData(nameof(GetAllTemplateFiles))]
        public void VerifyTemplateId(string filePath)
        {
            var result = VsTemplateValidator.VerifyTemplateId(filePath);
            Assert.True(result.Success, $"{filePath}: " + result.Message);
        }
    }
}
