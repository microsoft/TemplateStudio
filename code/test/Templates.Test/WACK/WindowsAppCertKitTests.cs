// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.TemplateEngine.Abstractions;
using Microsoft.Templates.Core;
using Microsoft.Templates.Core.Extensions;
using Microsoft.Templates.Core.Gen;
using Microsoft.Templates.Core.Helpers;
using Microsoft.Templates.Fakes;
using Xunit;

namespace Microsoft.Templates.Test
{
    [Collection("BuildCollection")]
    [Trait("ExecutionSet", "LongRunning")]
    public class WindowsAppCertKitTests : BaseGenAndBuildTests
    {
        public WindowsAppCertKitTests(BuildFixture fixture)
            : base(fixture)
        {
        }

        //// *** WARNING ***
        //// Running this requires:
        //// - Lots of time (as .Net Native compilation is slow)
        //// - Lots of disk space (as the artifacts of each test may use >2GB)
        //// - Running a Administrator (for the WACK tests or you'll get UAC prompts)
        //// - Control of the machine (as WACK tests will launch and try and control the generated app. If you're doing other things it may cause the test to fail incorrectly)
        [Theory]
        [MemberData(nameof(BaseGenAndBuildTests.GetProjectTemplatesForBuild), "", "", "Uwp")]
        public async Task RunWackOnProjectWithAllPagesAndFeaturesAsync(string projectType, string framework, string platform, string language)
        {
            // Exclude background task from WACK tests until WACK is fixed
            Func<ITemplateInfo, bool> templateSelector =
                t => t.GetTemplateType().IsItemTemplate()
                && (t.GetProjectTypeList().Contains(projectType) || t.GetProjectTypeList().Contains(All))
                && t.GetFrontEndFrameworkList().Contains(framework)
                && t.GroupIdentity != "wts.Feat.BackgroundTask"
                && t.GroupIdentity != "wts.Feat.BackgroundTask.VB"
                && t.GroupIdentity != "wts.Service.IdentityForcedLogin"
                && t.GroupIdentity != "wts.Service.IdentityForcedLogin.VB"
                && t.GetPlatform() == platform
                && !t.GetIsHidden();

            var projectName = $"{projectType}{framework}Wack{ShortLanguageName(language)}";

            var projectPath = await AssertGenerateProjectAsync(projectName, projectType, framework, platform, language, templateSelector, GenerationFixture.GetDefaultName);

            // Replace the default assets in the generated project or they will cause WACK to fail
            foreach (var assetFile in new DirectoryInfo("./TestData/NonDefaultAssets").GetFiles("*.png"))
            {
                File.Copy(assetFile.FullName, Path.Combine(GenContext.Current.DestinationPath, "Assets", assetFile.Name), overwrite: true);
            }

            // Create APPXBundle
            // NOTE. This is very slow. (i.e. ~10+ mins) as it does a full release build including all .net native compilation
            var bundleResult = _fixture.BuildAppxBundle(projectName, projectPath, GetProjectExtension(language));

            Assert.True(bundleResult.exitCode.Equals(0), $"Failed to create AppxBundle for {projectName}. {Environment.NewLine}Errors found: {_fixture.GetErrorLines(bundleResult.outputFile)}.{Environment.NewLine}Please see {Path.GetFullPath(bundleResult.outputFile)} for more details.");

            var bundleFile = new DirectoryInfo(Path.Combine(projectPath, projectName, "AppPackages")).GetFiles("*.appxbundle", SearchOption.AllDirectories).First().FullName;

            // Run WACK test
            // NOTE. This requires elevation. If not elevated you'll get a UAC prompt
            var wackResult = _fixture.RunWackTestOnAppxBundle(bundleFile, projectPath);

            Assert.True(wackResult.exitCode.Equals(0), $"Failed to create AppxBundle for {projectName}. {Environment.NewLine}Errors found: {_fixture.GetErrorLines(wackResult.outputFile)}.{Environment.NewLine}Please see {Path.GetFullPath(wackResult.outputFile)} for more details.");

            var overallSuccessXml = @"OVERALL_RESULT=""PASS""";

            var wackResultOutput = File.ReadAllText(wackResult.resultFile);

            Assert.True(wackResultOutput.Contains(overallSuccessXml), $"WACK test failed for {projectName}.{Environment.NewLine}Please see {Path.GetFullPath(wackResult.resultFile)} for more details.");

            Fs.SafeDeleteDirectory(projectPath);
        }
    }
}
