// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Text;
using System.Xml;
using Microsoft.Templates.Core;
using Microsoft.Templates.Core.Gen;
using Microsoft.Templates.Core.Locations;
using Microsoft.Templates.Fakes;
using Microsoft.Templates.UI;
using Microsoft.VisualStudio.Threading;
using Microsoft.TemplateEngine.Abstractions;

using Xunit;

namespace Microsoft.Templates.Test
{
    [Collection("GenerationCollection")]
    [Trait("ExecutionSet", "Generation")]
    public class GenProjectTests : BaseGenAndBuildTests
    {
        public GenProjectTests(GenerationFixture fixture)
        {
            _fixture = fixture;
        }

        [Theory]
        [MemberData("GetProjectTemplatesForGenerationAsync")]
        [Trait("Type", "GenerationProjects")]
        public async Task GenEmptyProjectAsync(string projectType, string framework, string language)
        {
            Func<ITemplateInfo, bool> selector =
                t => t.GetTemplateType() == TemplateType.Project
                    && t.GetProjectTypeList().Contains(projectType)
                    && t.GetFrameworkList().Contains(framework)
                    && !t.GetIsHidden()
                    && t.GetLanguage() == language;

            var projectName = $"{projectType}{framework}";

            await AssertGenerateProjectAsync(selector, projectName, projectType, framework, language);
        }

        [Theory]
        [MemberData("GetProjectTemplatesForGenerationAsync")]
        [Trait("Type", "GenerationProjects")]
        public async Task GenEmptyProjectCorrectInferProjectConfigInfoAsync(string projectType, string framework, string language)
        {
            Func<ITemplateInfo, bool> selector =
                t => t.GetTemplateType() == TemplateType.Project
                    && t.GetProjectTypeList().Contains(projectType)
                    && t.GetFrameworkList().Contains(framework)
                    && !t.GetIsHidden()
                    && t.GetLanguage() == language;

            var projectName = $"{projectType}{framework}";

            string projectPath = await AssertGenerateProjectAsync(selector, projectName, projectType, framework, language, null, false);

            // Remove configuration from the manifest
            RemoveProjectConfigInfoFromProject();

            // Now the configuration must be inferred and should be as expected
            AssertCorrectProjectConfigInfo(projectType, framework);

            AssertProjectConfigInfoRecreated(projectType, framework);
        }

        private static void AssertProjectConfigInfoRecreated(string projectType, string framework)
        {
            string content = File.ReadAllText(Path.Combine(GenContext.Current.ProjectPath, "Package.appxmanifest"));
            string expectedFxText = $"Name=\"framework\" Value=\"{framework}\"";
            string expectedPtText = $"Name=\"projectType\" Value=\"{projectType}\"";

            Assert.Contains(expectedFxText, content, StringComparison.InvariantCulture);
            Assert.Contains(expectedPtText, content, StringComparison.InvariantCulture);
        }

        private void RemoveProjectConfigInfoFromProject()
        {
            string manifest = Path.Combine(GenContext.Current.ProjectPath, "Package.appxmanifest");
            var lines = File.ReadAllLines(manifest);
            StringBuilder sb = new StringBuilder();
            string fx = $"genTemplate:Item Name=\"framework\"";
            string pt = $"genTemplate:Item Name=\"projectType\"";
            foreach (var line in lines)
            {
                if (!line.Contains(fx) && !line.Contains(pt))
                {
                    sb.AppendLine(line);
                }
            }
            File.Delete(manifest);
            File.WriteAllText(manifest, sb.ToString());
        }

        [Theory]
        [MemberData("GetProjectTemplatesForGenerationAsync")]
        [Trait("Type", "GenerationAllPagesAndFeatures")]
        public async Task GenAllPagesAndFeaturesAsync(string projectType, string framework, string language)
        {
            Func<ITemplateInfo, bool> selector =
                t => t.GetTemplateType() == TemplateType.Project
                    && t.GetProjectTypeList().Contains(projectType)
                    && t.GetFrameworkList().Contains(framework)
                    && !t.GetIsHidden()
                    && t.GetLanguage() == language;

            var projectName = $"{projectType}{framework}All";

            await AssertGenerateProjectAsync(selector, projectName, projectType, framework, language, BaseGenAndBuildFixture.GetDefaultName);
        }

        [Theory]
        [MemberData("GetProjectTemplatesForGenerationAsync")]
        [Trait("Type", "GenerationRandomNames")]
        [Trait("ExecutionSet", "VBQuickCheck")]
        public async Task GenAllPagesAndFeaturesRandomNamesAsync(string projectType, string framework, string language)
        {
            Func<ITemplateInfo, bool> selector =
                t => t.GetTemplateType() == TemplateType.Project
                    && t.GetProjectTypeList().Contains(projectType)
                    && t.GetFrameworkList().Contains(framework)
                    && !t.GetIsHidden()
                    && t.GetLanguage() == language;

            var projectName = $"{projectType}{framework}AllRandom";

            await AssertGenerateProjectAsync(selector, projectName, projectType, framework, language, BaseGenAndBuildFixture.GetRandomName);
        }

        [Theory]
        [MemberData("GetProjectTemplatesForGenerationAsync")]
        [Trait("Type", "GenerationRightClick")]
        public async Task GenEmptyProjectWithAllRightClickItemsAsync(string projectType, string framework, string language)
        {
            var projectName = $"{projectType}{framework}AllRightClick";
            var projectPath = Path.Combine(_fixture.TestProjectsPath, projectName, projectName);

            await AssertGenerateRightClickAsync(projectName, projectType, framework, language, true);
        }

        [Theory]
        [MemberData("GetProjectTemplatesForGenerationAsync")]
        [Trait("Type", "GenerationRightClick")]
        public async Task GenCompleteProjectWithAllRightClickItemsAsync(string projectType, string framework, string language)
        {
            var projectName = $"{projectType}{framework}AllRightClick2";
            var projectPath = Path.Combine(_fixture.TestProjectsPath, projectName, projectName);

            await AssertGenerateRightClickAsync(projectName, projectType, framework, language, false);
        }

        [Theory]
        [MemberData("GetPageAndFeatureTemplatesForGenerationAsync", "MVVMLight")]
        [Trait("Type", "GenerationOneByOneMVVMLight")]
        public async Task GenMVVMLightOneByOneItemsAsync(string itemName, string projectType, string framework, string itemId, string language)
        {
            await AssertGenerationOneByOneAsync(itemName, projectType, framework, itemId, language);
        }

        [Theory]
        [MemberData("GetPageAndFeatureTemplatesForGenerationAsync", "CaliburnMicro")]
        [Trait("Type", "GenerationOneByOneCaliburnMicro")]
        public async Task GenCaliburnMicroOneByOneItemsAsync(string itemName, string projectType, string framework, string itemId, string language)
        {
            await AssertGenerationOneByOneAsync(itemName, projectType, framework, itemId, language);
        }

        [Theory]
        [MemberData("GetPageAndFeatureTemplatesForGenerationAsync", "MVVMBasic")]
        [Trait("Type", "GenerationOneByOneMVVMBasic")]
        public async Task GenMVVMBasicOneByOneItemsAsync(string itemName, string projectType, string framework, string itemId, string language)
        {
            await AssertGenerationOneByOneAsync(itemName, projectType, framework, itemId, language);
        }

        [Theory]
        [MemberData("GetPageAndFeatureTemplatesForGenerationAsync", "CodeBehind")]
        [Trait("Type", "GenerationOneByOneCodeBehind")]
        public async Task GenCodeBehindOneByOneItemsAsync(string itemName, string projectType, string framework, string itemId, string language)
        {
            await AssertGenerationOneByOneAsync(itemName, projectType, framework, itemId, language);
        }

        [Theory]
        [MemberData("GetMultiLanguageProjectsAndFrameworks")]
        [Trait("Type", "GenerationLanguageComparison")]
        [Trait("ExecutionSet", "VBQuickCheck")]
        public async Task EnsureProjectsGeneratedWithDifferentLanguagesAreEquivalentAsync(string projectType, string framework)
        {
            var genIdentities = GetPagesAndFeaturesForMultiLanguageProjectsAndFrameworks(projectType, framework).ToList();

#pragma warning disable SA1008 // Opening parenthesis must be spaced correctly - C#7.0 feature StyleCop can't handle
            var (csResultPath, csProjectName) = await SetUpComparisonProjectAsync(ProgrammingLanguages.CSharp, projectType, framework, genIdentities);
            var (vbResultPath, vbProjectName) = await SetUpComparisonProjectAsync(ProgrammingLanguages.VisualBasic, projectType, framework, genIdentities);
#pragma warning restore SA1008 // Opening parenthesis must be spaced correctly

            // Check file names equivalence
            var allCsFiles = new DirectoryInfo(csResultPath).GetFiles("*.*").ToList();
            var allVbFiles = new DirectoryInfo(vbResultPath).GetFiles("*.*").ToList();
            var equalNumberOfFiles = allCsFiles.Count == allVbFiles.Count;
            Assert.True(equalNumberOfFiles, $"Differing number of files in the generated projects ({csResultPath.Replace("CS", " * ")}).");

            for (var i = 0; i < allCsFiles.Count; i++)
            {
                var fileNameMatches = allCsFiles[i].FullName == VbFileToCsEquivalent(allVbFiles[i].FullName);
                Assert.True(fileNameMatches, $"File '{allCsFiles[i].FullName}' does not have a VB equivalent.");
            }

            // Resource file contents should be identical regardless of programming language
            var csReswFilePath = Path.Combine(csResultPath, csProjectName, "Strings", "en-us", "Resources.resw");
            var vbReswFilePath = Path.Combine(vbResultPath, vbProjectName, "Strings", "en-us", "Resources.resw");

            var csResourcesString = File.ReadAllText(csReswFilePath);
            var vbResourcesString = File.ReadAllText(vbReswFilePath);

            var reswFilesMatch = csResourcesString == vbResourcesString;
            Assert.True(reswFilesMatch, $"Resource files do not match ({csResultPath.Replace("CS", "*")}).");

            // Ensure all resources are used
            var resourceKeys = new List<string>();
            var resourceXml = new XmlDocument();
            resourceXml.LoadXml(csResourcesString);

            var nodes = resourceXml.GetElementsByTagName("data");
            for (int i = 0; i < nodes.Count; i++)
            {
                // Assume resources containing dots are Uids
                var resourceName = nodes[i].Attributes.GetNamedItem("name").Value;
                if (!resourceName.Contains("."))
                {
                    resourceKeys.Add(resourceName);
                }
            }

            foreach (var vbFile in new DirectoryInfo(Path.Combine(vbResultPath, vbProjectName)).GetFiles("*.vb", SearchOption.AllDirectories))
            {
                var vbFileContents = File.ReadAllText(vbFile.FullName);

                for (var i = resourceKeys.Count - 1; i >= 0; i--)
                {
                    if (vbFileContents.Contains(resourceKeys[i]))
                    {
                        resourceKeys.RemoveAt(i);
                    }
                }

                if (!resourceKeys.Any())
                {
                    break;
                }
            }

            Assert.True(!resourceKeys.Any(), $"Resource strings are defined but not used in VB files: {string.Join("; ", resourceKeys)}");

            // Check contents of the Assets folder are identical
            var csAssets = new DirectoryInfo(Path.Combine(csResultPath, csProjectName, "Assets")).GetFiles().OrderBy(f => f.FullName).ToList();
            var vbAssets = new DirectoryInfo(Path.Combine(vbResultPath, vbProjectName, "Assets")).GetFiles().OrderBy(f => f.FullName).ToList();

            for (var i = 0; i < csAssets.Count; i++)
            {
                var styleFileMatches = BinaryFileEquals(csAssets[i].FullName, vbAssets[i].FullName);
                Assert.True(styleFileMatches, $"Asset file '{csAssets[i].Name}' does not match ({csResultPath.Replace("CS", "*")}).");
            }

            // Check contents of the Styles folder is identical
            var csStyles = new DirectoryInfo(Path.Combine(csResultPath, csProjectName, "Styles")).GetFiles().OrderBy(f => f.FullName).ToList();
            var vbStyles = new DirectoryInfo(Path.Combine(vbResultPath, vbProjectName, "Styles")).GetFiles().OrderBy(f => f.FullName).ToList();

            for (var i = 0; i < csStyles.Count; i++)
            {
                var styleFileMatches = File.ReadAllText(csStyles[i].FullName) == File.ReadAllText(vbStyles[i].FullName);
                Assert.True(styleFileMatches, $"Style file '{csStyles[i].Name}' does not match ({csResultPath.Replace("CS", "*")}).");
            }

            Fs.SafeDeleteDirectory(csResultPath);
            Fs.SafeDeleteDirectory(vbResultPath);
        }

        private static string VbFileToCsEquivalent(string vbFilePath)
        {
            return vbFilePath.Replace("VisualBasic", "CS")
                             .Replace(".vb", ".cs")
                             .Replace("My Project", "Properties");
        }

        private static bool BinaryFileEquals(string fileName1, string fileName2)
        {
            using (var file1 = new FileStream(fileName1, FileMode.Open))
            using (var file2 = new FileStream(fileName2, FileMode.Open))
            {
                return FileStreamEquals(file1, file2);
            }
        }

        private static bool FileStreamEquals(Stream stream1, Stream stream2)
        {
            const int bufferSize = 2048;
            var buffer1 = new byte[bufferSize];
            var buffer2 = new byte[bufferSize];

            while (true)
            {
                var count1 = stream1.Read(buffer1, 0, bufferSize);
                var count2 = stream2.Read(buffer2, 0, bufferSize);

                if (count1 != count2)
                {
                    return false;
                }

                if (count1 == 0)
                {
                    return true;
                }

                if (!buffer1.Take(count1).SequenceEqual(buffer2.Take(count2)))
                {
                    return false;
                }
            }
        }
    }
}
