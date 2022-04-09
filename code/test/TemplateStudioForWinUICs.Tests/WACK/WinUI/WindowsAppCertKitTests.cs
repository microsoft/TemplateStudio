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
using TemplateStudioForWinUICs.Tests;
using Xunit;

namespace Microsoft.Templates.Test.WinUICs.Wack
{
    //// *** WARNING ***
    //// Running this requires:
    //// - Lots of time (as .Net Native compilation is slow)
    //// - Lots of disk space (as the artifacts of each test may use >2GB)
    //// - Running a Administrator (for the WACK tests or you'll get UAC prompts)
    //// - Control of the machine (as WACK tests will launch and try and control the generated app. If you're doing other things it may cause the test to fail incorrectly)
    [Collection(nameof(WinUICsBuildTemplatesTestCollection))]
    [Trait("Group", "ManualOnlyWinUICs")]
    public class WindowsAppCertKitTests : WinUICsBaseGenAndBuildTests
    {
        public WindowsAppCertKitTests(WinUICsBuildTemplatesTestFixture fixture)
            : base(fixture)
        {
        }

        [Theory]
        [MemberData(nameof(GetProjectTemplatesForBuild), Frameworks.None, ProgrammingLanguages.CSharp, Platforms.WinUI)]
        public async Task WackTests_None_All_WinUI_CSAsync(string projectType, string framework, string platform, string language, string appModel)
        {
            await RunWackOnProjectWithAllPagesAndFeaturesAsync(projectType, framework, platform, language, appModel);
        }

        [Theory]
        [MemberData(nameof(GetProjectTemplatesForBuild), Frameworks.MVVMToolkit, ProgrammingLanguages.CSharp, Platforms.WinUI)]
        public async Task WackTests_MvvmToolkit_All_WinUI_CSAsync(string projectType, string framework, string platform, string language, string appModel)
        {
            await RunWackOnProjectWithAllPagesAndFeaturesAsync(projectType, framework, platform, language, appModel);
        }

        private async Task RunWackOnProjectWithAllPagesAndFeaturesAsync(string projectType, string framework, string platform, string language, string appModel)
        {
            bool templateSelector(ITemplateInfo t) => t.GetTemplateType().IsItemTemplate()
                && (t.GetProjectTypeList().Contains(projectType) || t.GetProjectTypeList().Contains(All))
                && (t.GetFrontEndFrameworkList().Contains(framework) || t.GetFrontEndFrameworkList().Contains(All))
                && t.GetPropertyBagValuesList("appmodel").Contains(appModel)
                && t.GetPlatform() == platform
                && !t.GetIsHidden();

            await BuildProjectAndRunWackAsync(projectType, framework, platform, language, appModel, templateSelector);
        }

        private async Task BuildProjectAndRunWackAsync(string projectType, string framework, string platform, string language, string appModel, Func<ITemplateInfo, bool> templateSelector)
        {
            var projectName = $"{projectType}{framework}Wack{ShortLanguageName(language)}WinUI";

            var context = new UserSelectionContext(language, platform)
            {
                ProjectType = projectType,
                FrontEndFramework = framework,
            };
            context.AddAppModel(appModel);

            var projectPath = await AssertGenerateProjectAsync(projectName, context, templateSelector, GenerationFixture.GetDefaultName);

            // Replace the default assets in the generated project or they will cause WACK to fail
            foreach (var assetFile in new DirectoryInfo("./TestData/NonDefaultAssets").GetFiles("*.png"))
            {
                File.Copy(assetFile.FullName, Path.Combine(GenContext.Current.DestinationPath, "Assets", assetFile.Name), overwrite: true);
            }

            var batFile = language == ProgrammingLanguages.Cpp ? "bat\\WinUI\\RestoreAndBuildCppAppx.bat" : "bat\\WinUI\\RestoreAndBuildAppx.bat";

            // Create MSIXBundle
            // NOTE. This is very slow. (i.e. ~10+ mins) as it does a full release build including all .net native compilation
            var (exitCode, outputFile) = _fixture.BuildMsixBundle(projectName, projectPath, projectName, "csproj", batFile);

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
