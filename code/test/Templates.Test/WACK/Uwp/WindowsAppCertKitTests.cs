// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.TemplateEngine.Abstractions;
using Microsoft.Templates.Core;
using Microsoft.Templates.Core.Extensions;
using Microsoft.Templates.Core.Gen;
using Microsoft.Templates.Core.Helpers;
using Xunit;

namespace Microsoft.Templates.Test.Uwp
{
    //// *** WARNING ***
    //// Running this requires:
    //// - Lots of time (as .Net Native compilation is slow)
    //// - Lots of disk space (as the artifacts of each test may use >2GB)
    //// - Running a Administrator (for the WACK tests or you'll get UAC prompts)
    //// - Control of the machine (as WACK tests will launch and try and control the generated app. If you're doing other things it may cause the test to fail incorrectly)
    [Collection("BuildCollection")]
    [Trait("ExecutionSet", "LongRunning")]
    [Trait("ExecutionSet", "_Wack")]
    public class WindowsAppCertKitTests : BaseGenAndBuildTests
    {
        public WindowsAppCertKitTests(BuildFixture fixture)
            : base(fixture)
        {
        }

        [Theory]
        [MemberData(nameof(GetProjectTemplatesForBuild), Frameworks.CodeBehind, ProgrammingLanguages.CSharp, Platforms.Uwp)]
        public async Task WackTests_CodeBehind_CSAsync(string projectType, string framework, string platform, string language)
        {
            await RunWackOnProjectWithAllPagesAndFeaturesAsync(projectType, framework, platform, language);
        }

        [Theory]
        [MemberData(nameof(GetProjectTemplatesForBuild), Frameworks.CodeBehind, ProgrammingLanguages.VisualBasic, Platforms.Uwp)]
        public async Task WackTests_CodeBehind_VBAsync(string projectType, string framework, string platform, string language)
        {
            await RunWackOnProjectWithAllPagesAndFeaturesAsync(projectType, framework, platform, language);
        }

        [Theory]
        [MemberData(nameof(GetProjectTemplatesForBuild), Frameworks.MVVMBasic, ProgrammingLanguages.CSharp, Platforms.Uwp)]
        public async Task WackTests_MvvmBasic_CSAsync(string projectType, string framework, string platform, string language)
        {
            await RunWackOnProjectWithAllPagesAndFeaturesAsync(projectType, framework, platform, language);
        }

        [Theory]
        [MemberData(nameof(GetProjectTemplatesForBuild), Frameworks.MVVMBasic, ProgrammingLanguages.VisualBasic, Platforms.Uwp)]
        public async Task WackTests_MvvmBasic_VBAsync(string projectType, string framework, string platform, string language)
        {
            await RunWackOnProjectWithAllPagesAndFeaturesAsync(projectType, framework, platform, language);
        }

        [Theory]
        [MemberData(nameof(GetProjectTemplatesForBuild), Frameworks.MVVMLight, ProgrammingLanguages.CSharp, Platforms.Uwp)]
        public async Task WackTests_MVVMLight_CSAsync(string projectType, string framework, string platform, string language)
        {
            await RunWackOnProjectWithAllPagesAndFeaturesAsync(projectType, framework, platform, language);
        }

        [Theory]
        [MemberData(nameof(GetProjectTemplatesForBuild), Frameworks.MVVMLight, ProgrammingLanguages.VisualBasic, Platforms.Uwp)]
        public async Task WackTests_MVVMLight_VBAsync(string projectType, string framework, string platform, string language)
        {
            await RunWackOnProjectWithAllPagesAndFeaturesAsync(projectType, framework, platform, language);
        }

        [Theory]
        [MemberData(nameof(GetProjectTemplatesForBuild), Frameworks.CaliburnMicro, ProgrammingLanguages.CSharp, Platforms.Uwp)]
        public async Task WackTests_CaliburnMicro_CSAsync(string projectType, string framework, string platform, string language)
        {
            await RunWackOnProjectWithAllPagesAndFeaturesAsync(projectType, framework, platform, language);
        }

        [Theory]
        [MemberData(nameof(GetProjectTemplatesForBuild), Frameworks.Prism, ProgrammingLanguages.CSharp, Platforms.Uwp)]
        public async Task WackTests_Prism_CSAsync(string projectType, string framework, string platform, string language)
        {
            await RunWackOnProjectWithAllPagesAndFeaturesAsync(projectType, framework, platform, language);
        }

        [Theory]
        [MemberData(nameof(GetProjectTemplatesForBuild), Frameworks.MVVMToolkit, ProgrammingLanguages.VisualBasic, Platforms.Uwp)]
        public async Task WackTests_MvvmToolkit_VBAsync(string projectType, string framework, string platform, string language)
        {
            await RunWackOnProjectWithAllPagesAndFeaturesAsync(projectType, framework, platform, language);
        }

        [Theory]
        [MemberData(nameof(GetProjectTemplatesForBuild), Frameworks.MVVMToolkit, ProgrammingLanguages.CSharp, Platforms.Uwp)]
        public async Task WackTests_MvvmToolkit_CSAsync(string projectType, string framework, string platform, string language)
        {
            await RunWackOnProjectWithAllPagesAndFeaturesAsync(projectType, framework, platform, language);
        }

        [Theory]
        [MemberData(nameof(GetProjectTemplatesForBuild), Frameworks.CodeBehind, ProgrammingLanguages.CSharp, Platforms.Uwp)]
        public async Task WackTests_CodeBehind_Other_CSAsync(string projectType, string framework, string platform, string language)
        {
            await RunWackOnProjectWithExcludedFeaturesAsync(projectType, framework, platform, language);
        }

        [Theory]
        [MemberData(nameof(GetProjectTemplatesForBuild), Frameworks.CodeBehind, ProgrammingLanguages.VisualBasic, Platforms.Uwp)]
        public async Task WackTests_CodeBehind_Other_VBAsync(string projectType, string framework, string platform, string language)
        {
            await RunWackOnProjectWithExcludedFeaturesAsync(projectType, framework, platform, language);
        }

        [Theory]
        [MemberData(nameof(GetProjectTemplatesForBuild), Frameworks.MVVMBasic, ProgrammingLanguages.CSharp, Platforms.Uwp)]
        public async Task WackTests_MvvmBasic_Other_CSAsync(string projectType, string framework, string platform, string language)
        {
            await RunWackOnProjectWithExcludedFeaturesAsync(projectType, framework, platform, language);
        }

        [Theory]
        [MemberData(nameof(GetProjectTemplatesForBuild), Frameworks.MVVMBasic, ProgrammingLanguages.VisualBasic, Platforms.Uwp)]
        public async Task WackTests_MvvmBasic_Other_VBAsync(string projectType, string framework, string platform, string language)
        {
            await RunWackOnProjectWithExcludedFeaturesAsync(projectType, framework, platform, language);
        }

        [Theory]
        [MemberData(nameof(GetProjectTemplatesForBuild), Frameworks.MVVMLight, ProgrammingLanguages.CSharp, Platforms.Uwp)]
        public async Task WackTests_MVVMLight_Other_CSAsync(string projectType, string framework, string platform, string language)
        {
            await RunWackOnProjectWithExcludedFeaturesAsync(projectType, framework, platform, language);
        }

        [Theory]
        [MemberData(nameof(GetProjectTemplatesForBuild), Frameworks.MVVMLight, ProgrammingLanguages.VisualBasic, Platforms.Uwp)]
        public async Task WackTests_MVVMLight_Other_VBAsync(string projectType, string framework, string platform, string language)
        {
            await RunWackOnProjectWithExcludedFeaturesAsync(projectType, framework, platform, language);
        }

        [Theory]
        [MemberData(nameof(GetProjectTemplatesForBuild), Frameworks.CaliburnMicro, ProgrammingLanguages.CSharp, Platforms.Uwp)]
        public async Task WackTests_CaliburnMicro_Other_CSAsync(string projectType, string framework, string platform, string language)
        {
            await RunWackOnProjectWithExcludedFeaturesAsync(projectType, framework, platform, language);
        }

        [Theory]
        [MemberData(nameof(GetProjectTemplatesForBuild), Frameworks.Prism, ProgrammingLanguages.CSharp, Platforms.Uwp)]
        public async Task WackTests_Prism_Other_CSAsync(string projectType, string framework, string platform, string language)
        {
            await RunWackOnProjectWithExcludedFeaturesAsync(projectType, framework, platform, language);
        }

        [Theory]
        [MemberData(nameof(GetProjectTemplatesForBuild), Frameworks.MVVMToolkit, ProgrammingLanguages.CSharp, Platforms.Uwp)]
        public async Task WackTests_MvvmToolkit_Other_CSAsync(string projectType, string framework, string platform, string language)
        {
            await RunWackOnProjectWithExcludedFeaturesAsync(projectType, framework, platform, language);
        }

        [Theory]
        [MemberData(nameof(GetProjectTemplatesForBuild), Frameworks.MVVMToolkit, ProgrammingLanguages.VisualBasic, Platforms.Uwp)]
        public async Task WackTests_MvvmToolkit_Other_VBAsync(string projectType, string framework, string platform, string language)
        {
            await RunWackOnProjectWithExcludedFeaturesAsync(projectType, framework, platform, language);
        }

        private async Task RunWackOnProjectWithAllPagesAndFeaturesAsync(string projectType, string framework, string platform, string language)
        {
            // Exclude options that cannot be combined with others
            bool templateSelector(ITemplateInfo t) => t.GetTemplateType().IsItemTemplate()
                && (t.GetProjectTypeList().Contains(projectType) || t.GetProjectTypeList().Contains(All))
                && t.GetFrontEndFrameworkList().Contains(framework)
                && !t.GroupIdentity.StartsWith("wts.Feat.BackgroundTask")
                && !t.GroupIdentity.StartsWith("wts.Service.Identity")
                & !t.GroupIdentity.Contains("Test")
                & !t.GroupIdentity.Contains("WinAppDriver")
                & !t.GroupIdentity.Contains("WebApi")
                && t.GetPlatform() == platform
                && !t.GetIsHidden();

            await BuildProjectAndRunWackAsync(projectType, framework, platform, language, templateSelector);
        }

        private async Task RunWackOnProjectWithExcludedFeaturesAsync(string projectType, string framework, string platform, string language)
        {
            // Skip these options as they don't present anything that isn't tested by other combinations
            if (projectType == ProjectTypes.Blank || projectType == ProjectTypes.TabbedNav)
            {
                return;
            }

            // This should pick up the minimum required items for a project plus those identified below (and excluded above).
            bool templateSelector(ITemplateInfo t) => t.GetTemplateType().IsItemTemplate()
                     && (t.GetProjectTypeList().Contains(projectType) || t.GetProjectTypeList().Contains(All))
                     && t.GetFrontEndFrameworkList().Contains(framework)
                     && (t.GroupIdentity.StartsWith("wts.Feat.BackgroundTask"))
                     && t.GetPlatform() == platform
                     && !t.GetIsHidden();

            await BuildProjectAndRunWackAsync(projectType, framework, platform, language, templateSelector);
        }

        private async Task BuildProjectAndRunWackAsync(string projectType, string framework, string platform, string language, Func<ITemplateInfo, bool> templateSelector)
        {
            var projectName = $"{projectType}{framework}Wack{ShortLanguageName(language)}Uwp";

            var projectPath = await AssertGenerateProjectAsync(projectName, projectType, framework, platform, language, templateSelector, GenerationFixture.GetDefaultName);

            // Replace the default assets in the generated project or they will cause WACK to fail
            foreach (var assetFile in new DirectoryInfo("../../TestData/NonDefaultAssets").GetFiles("*.png"))
            {
                File.Copy(assetFile.FullName, Path.Combine(GenContext.Current.DestinationPath, "Assets", assetFile.Name), overwrite: true);
            }

            // Create MSIXBundle
            // NOTE. This is very slow. (i.e. ~10+ mins) as it does a full release build including all .net native compilation
            var (exitCode, outputFile) = _fixture.BuildMsixBundle(projectName, projectPath, projectName, GetProjectExtension(language), "bat\\Uwp\\RestoreAndBuildAppx.bat");

            Assert.True(exitCode.Equals(0), $"Failed to create MsixBundle for {projectName}. {Environment.NewLine}Errors found: {_fixture.GetErrorLines(outputFile)}.{Environment.NewLine}Please see {Path.GetFullPath(outputFile)} for more details.");

            var bundleFile = new DirectoryInfo(Path.Combine(projectPath, projectName, "AppPackages")).GetFiles("*.msixbundle", SearchOption.AllDirectories).First().FullName;

            // Run WACK test
            // NOTE. This requires elevation. If not elevated you'll get a UAC prompt
            var wackResult = _fixture.RunWackTestOnMsixBundle(bundleFile, projectPath);

            Assert.True(wackResult.exitCode.Equals(0), $"Failed to create MsixBundle for {projectName}. {Environment.NewLine}Errors found: {_fixture.GetErrorLines(wackResult.outputFile)}.{Environment.NewLine}Please see {Path.GetFullPath(wackResult.outputFile)} for more details.");

            var overallSuccessXml = @"OVERALL_RESULT=""PASS""";

            var wackResultOutput = File.ReadAllText(wackResult.resultFile);

            Assert.True(wackResultOutput.Contains(overallSuccessXml), $"WACK test failed for {projectName}.{Environment.NewLine}Please see {Path.GetFullPath(wackResult.resultFile)} for more details.");

            Fs.SafeDeleteDirectory(projectPath);
        }
    }
}
