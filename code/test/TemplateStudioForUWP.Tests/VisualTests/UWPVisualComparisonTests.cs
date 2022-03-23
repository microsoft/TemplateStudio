// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Templates.Core;
using Microsoft.Templates.Core.Helpers;
using OpenQA.Selenium.Appium.Windows;
using WindowsTestHelpers;
using Xunit;

namespace Microsoft.Templates.Test.UWP.Build
{
    [Trait("Group", "TS4UWP")]
    [Trait("Group", "ManualOnly")]
    [Trait("Type", "WinAppDriver")]
    [Collection(nameof(UwpGenTemplatesTestCollection))]
    public class UWPVisualComparisonTests : BaseUwpVisualComparisonTests
    {
        public UWPVisualComparisonTests(UwpGenTemplatesTestFixture fixture)
        : base(fixture)
        {
        }

        // Gets all the pages that are available (and testable) in both VB & C#
        public static IEnumerable<object[]> GetAllSinglePageAppsVbAndCsSimple()
        {
            var pagesThatSupportUiTesting = AllPagesThatSupportSimpleTesting();

            foreach (var page in pagesThatSupportUiTesting)
            {
                yield return new object[] { page };
            }
        }

        // Gets all the pages that are available (and testable) in both VB & C#
        public static IEnumerable<object[]> GetAllSinglePageAppsVbAndCsExtraLogic()
        {
            var pagesThatSupportUiTesting = AllPagesThatRequireExtraLogicForTesting();

            foreach (var page in pagesThatSupportUiTesting)
            {
                yield return new object[] { page };
            }
        }

        // Get all the pages in C# templates that are to be compared with the MVVMToolkit version
        public static IEnumerable<object[]> GetAllSinglePageAppsCSharp()
        {
            foreach (var projectType in GetAllProjectTypes())
            {
                var otherFrameworks = GetAdditionalCsFrameworks(projectType).Select(f => f[0].ToString()).ToArray();

                var pagesThatSupportUiTesting = AllPagesThatSupportSimpleTestingOnAllFrameworks();

                foreach (var page in pagesThatSupportUiTesting)
                {
                    yield return new object[] { projectType, page, otherFrameworks };
                }
            }
        }

        public static IEnumerable<object[]> GetAdditionalCsFrameworks(string projectType)
        {
            foreach (var framework in new[] { Frameworks.CodeBehind, Frameworks.Prism })
            {
                yield return new object[] { framework };
            }
        }

        public static IEnumerable<object[]> GetAllFrameworksForBothVbAndCs()
        {
            foreach (var framework in new[] { Frameworks.CodeBehind, Frameworks.MVVMToolkit })
            {
                yield return new object[] { framework };
            }
        }

        public static IEnumerable<object[]> GetAllFrameworksAndLanguageCombinations()
        {
            yield return new object[] { Frameworks.CodeBehind, ProgrammingLanguages.CSharp };
            yield return new object[] { Frameworks.Prism, ProgrammingLanguages.CSharp };
            yield return new object[] { Frameworks.MVVMToolkit, ProgrammingLanguages.CSharp };
            yield return new object[] { Frameworks.CodeBehind, ProgrammingLanguages.VisualBasic };
            yield return new object[] { Frameworks.MVVMToolkit, ProgrammingLanguages.VisualBasic };
        }

        public static IEnumerable<string> GetAllProjectTypes()
        {
            foreach (var projectType in new[] { ProjectTypes.Blank, ProjectTypes.SplitView, ProjectTypes.TabbedNav, ProjectTypes.MenuBar })
            {
                yield return projectType;
            }
        }

        // Returned rectangles are measured in pixels from the top left of the image/window
        private string GetExclusionAreasForVisualEquivalencyTest(string projectType, string pageName)
        {
            switch (pageName)
            {
                case "wts.Page.Settings":
                    // Exclude the area at the end of the app name and also covering the version number
                    switch (projectType)
                    {
                        case ProjectTypes.SplitView:
                            return "new[] { new ImageComparer.ExclusionArea(new Rectangle(480, 290, 450, 50), 1.25f) }";
                        case ProjectTypes.TabbedNav:
                            return "new[] { new ImageComparer.ExclusionArea(new Rectangle(60, 340, 450, 50), 1.25f) }";
                        case ProjectTypes.MenuBar:
                            return "new[] { new ImageComparer.ExclusionArea(new Rectangle(60, 405, 450, 40), 1.25f) }";
                        case ProjectTypes.Blank:
                        default:
                            return "new[] { new ImageComparer.ExclusionArea(new Rectangle(60, 340, 450, 50), 1.25f) }";
                    }

                default:
                    return string.Empty;
            }
        }

        // Note. There are multiple theories defined here but could be combined in a single one.
        // However, it would be a lot tests being generated and some of the longer running and
        // more complicated options are susceptible to false negatives.
        // Splitting them up like this makes it easier to rerun and debug failed tests.
        [Theory]
        [MemberData(nameof(GetAllSinglePageAppsVbAndCsSimple))]
        public async Task EnsureLaunchPageVisualAreTheSameInVbAndCs_Blank_CodeBehind_Simple_Async(string page)
        {
            await EnsureLanguageLaunchPageVisualsAreEquivalentAsync(ProjectTypes.Blank, Frameworks.CodeBehind, page);
        }

        [Theory]
        [MemberData(nameof(GetAllSinglePageAppsVbAndCsExtraLogic))]
        public async Task EnsureLaunchPageVisualAreTheSameInVbAndCs_Blank_CodeBehind_ExtraLogic_Async(string page)
        {
            await EnsureLanguageLaunchPageVisualsAreEquivalentAsync(ProjectTypes.Blank, Frameworks.CodeBehind, page);
        }

        [Theory]
        [MemberData(nameof(GetAllSinglePageAppsVbAndCsSimple))]
        public async Task EnsureLaunchPageVisualAreTheSameInVbAndCs_SplitView_CodeBehind_Simple_Async(string page)
        {
            await EnsureLanguageLaunchPageVisualsAreEquivalentAsync(ProjectTypes.SplitView, Frameworks.CodeBehind, page);
        }

        [Theory]
        [MemberData(nameof(GetAllSinglePageAppsVbAndCsExtraLogic))]
        public async Task EnsureLaunchPageVisualAreTheSameInVbAndCs_SplitView_CodeBehind_ExtraLogic_Async(string page)
        {
            await EnsureLanguageLaunchPageVisualsAreEquivalentAsync(ProjectTypes.SplitView, Frameworks.CodeBehind, page);
        }

        [Theory]
        [MemberData(nameof(GetAllSinglePageAppsVbAndCsSimple))]
        public async Task EnsureLaunchPageVisualAreTheSameInVbAndCs_TabbedNav_CodeBehind_Simple_Async(string page)
        {
            await EnsureLanguageLaunchPageVisualsAreEquivalentAsync(ProjectTypes.TabbedNav, Frameworks.CodeBehind, page);
        }

        [Theory]
        [MemberData(nameof(GetAllSinglePageAppsVbAndCsExtraLogic))]
        public async Task EnsureLaunchPageVisualAreTheSameInVbAndCs_TabbedNav_CodeBehind_ExtraLogic_Async(string page)
        {
            await EnsureLanguageLaunchPageVisualsAreEquivalentAsync(ProjectTypes.TabbedNav, Frameworks.CodeBehind, page);
        }

        [Theory]
        [MemberData(nameof(GetAllSinglePageAppsVbAndCsSimple))]
        public async Task EnsureLaunchPageVisualAreTheSameInVbAndCs_SplitView_MvvmToolkit_Simple_Async(string page)
        {
            await EnsureLanguageLaunchPageVisualsAreEquivalentAsync(ProjectTypes.SplitView, Frameworks.MVVMToolkit, page);
        }

        [Theory]
        [MemberData(nameof(GetAllSinglePageAppsVbAndCsExtraLogic))]
        public async Task EnsureLaunchPageVisualAreTheSameInVbAndCs_SplitView_MvvmToolkit_ExtraLogic_Async(string page)
        {
            await EnsureLanguageLaunchPageVisualsAreEquivalentAsync(ProjectTypes.SplitView, Frameworks.MVVMToolkit, page);
        }

        [Theory]
        [MemberData(nameof(GetAllSinglePageAppsVbAndCsSimple))]
        public async Task EnsureLaunchPageVisualAreTheSameInVbAndCs_TabbedNav_MvvmToolkit_Simple_Async(string page)
        {
            await EnsureLanguageLaunchPageVisualsAreEquivalentAsync(ProjectTypes.TabbedNav, Frameworks.MVVMToolkit, page);
        }

        [Theory]
        [MemberData(nameof(GetAllSinglePageAppsVbAndCsExtraLogic))]
        public async Task EnsureLaunchPageVisualAreTheSameInVbAndCs_TabbedNav_MvvmToolkit_ExtraLogic_Async(string page)
        {
            await EnsureLanguageLaunchPageVisualsAreEquivalentAsync(ProjectTypes.TabbedNav, Frameworks.MVVMToolkit, page);
        }

        // There are tests with hardcoded projectType and framework values to make rerunning/debugging only some of the tests easier
        private async Task EnsureLanguageLaunchPageVisualsAreEquivalentAsync(string projectType, string framework, string page)
        {
            var genIdentities = new[] { page };

            ExecutionEnvironment.CheckRunningAsAdmin();
            WinAppDriverHelper.CheckIsInstalled();

            var app1Details = await SetUpProjectForUiTestComparisonAsync(ProgrammingLanguages.CSharp, projectType, framework, genIdentities, lastPageIsHome: true, createForScreenshots: true);
            var app2Details = await SetUpProjectForUiTestComparisonAsync(ProgrammingLanguages.VisualBasic, projectType, framework, genIdentities, lastPageIsHome: true, createForScreenshots: true);

            var noClickCount = 0;

            if (page == "wts.Page.Map")
            {
                noClickCount = 1;
            }
            else if (page == "wts.Page.Camera")
            {
                noClickCount = 2;
            }

            var longPause = page.EndsWith(".WebView", StringComparison.Ordinal) || page.EndsWith(".MediaPlayer", StringComparison.Ordinal);

            var testProjectDetails = SetUpTestProjectForInitialScreenshotComparison(app1Details, app2Details, GetExclusionAreasForVisualEquivalencyTest(projectType, page), noClickCount, longPause);

            (bool wereSuccessful, List<string> testOutput) testResults = (false, new List<string>());

            ExceptionHelper.RetryOn<OpenQA.Selenium.WebDriverException>(() =>
            {
                testResults = RunWinAppDriverTests(testProjectDetails);
            });

            // Note that failing tests will leave the projects behind, plus the apps and test certificates installed
            if (testResults.wereSuccessful)
            {
                UninstallAppx(app1Details.PackageFullName);
                UninstallAppx(app2Details.PackageFullName);

                RemoveCertificate(app1Details.CertificatePath);
                RemoveCertificate(app2Details.CertificatePath);

                // Parent of images folder also contains the test project
                Fs.SafeDeleteDirectory(Path.Combine(testProjectDetails.imagesFolder, ".."));
            }

            var outputMessages = string.Join(Environment.NewLine, testResults.testOutput);

            // A diff image is automatically created if the outputs differ
            if (Directory.Exists(testProjectDetails.imagesFolder)
             && Directory.GetFiles(testProjectDetails.imagesFolder, "*.*-Diff.png").Any())
            {
                Assert.True(
                    testResults.wereSuccessful,
                    $"Failing test images in {testProjectDetails.imagesFolder}{Environment.NewLine}{Environment.NewLine}{outputMessages}");
            }
            else
            {
                Assert.True(testResults.wereSuccessful, outputMessages);
            }

            WindowHelpers.BringVisualStudioToFront("Templates.Test");
            WindowHelpers.BringVisualStudioToFront("Big");
        }

        // Note. Visual Studio MUST be running as Admin to run this test.
        // Note that failing tests will leave the projects behind, plus the apps and test certificates installed
        [Theory]
        [MemberData(nameof(GetAllSinglePageAppsCSharp))]
        public async Task EnsureFrameworkLaunchPageVisualsAreEquivalentAsync(string projectType, string page, string[] frameworks)
        {
            var genIdentities = new[] { page };

            var noClickCount = 0;

            if (page == "wts.Page.Map")
            {
                noClickCount = 1;
            }
            else if (page == "wts.Page.Camera")
            {
                noClickCount = 2;
            }

            ExecutionEnvironment.CheckRunningAsAdmin();
            WinAppDriverHelper.CheckIsInstalled();

            // MVVMToolkit is considered the reference version. Compare generated apps with equivalent in other frameworks
            var refAppDetails = await SetUpProjectForUiTestComparisonAsync(ProgrammingLanguages.CSharp, projectType, Frameworks.MVVMToolkit, genIdentities, lastPageIsHome: true, createForScreenshots: true);

            var otherProjDetails = new VisualComparisonTestDetails[frameworks.Length];

            bool allTestsPassed = true;

            var outputMessages = string.Empty;

            for (int i = 0; i < frameworks.Length; i++)
            {
                string framework = frameworks[i];
                otherProjDetails[i] = await SetUpProjectForUiTestComparisonAsync(ProgrammingLanguages.CSharp, projectType, framework, genIdentities, lastPageIsHome: true, createForScreenshots: true);

                var testProjectDetails = SetUpTestProjectForInitialScreenshotComparison(refAppDetails, otherProjDetails[i], GetExclusionAreasForVisualEquivalencyTest(projectType, page), noClickCount);

                var (testSuccess, testOutput) = RunWinAppDriverTests(testProjectDetails);

                if (testSuccess)
                {
                    UninstallAppx(otherProjDetails[i].PackageFullName);

                    RemoveCertificate(otherProjDetails[i].CertificatePath);

                    // Parent of images folder also contains the test project
                    Fs.SafeDeleteDirectory(Path.Combine(testProjectDetails.imagesFolder, ".."));
                }
                else
                {
                    allTestsPassed = false;

                    if (Directory.Exists(testProjectDetails.imagesFolder)
                     && Directory.GetFiles(testProjectDetails.imagesFolder, "*.*-Diff.png").Any())
                    {
                        outputMessages += $"Failing test images in {testProjectDetails.imagesFolder}{Environment.NewLine}{Environment.NewLine}{outputMessages}";
                    }
                    else
                    {
                        outputMessages += $"{Environment.NewLine}{string.Join(Environment.NewLine, testOutput)}";
                    }
                }
            }

            if (allTestsPassed)
            {
                UninstallAppx(refAppDetails.PackageFullName);

                RemoveCertificate(refAppDetails.CertificatePath);
            }

            Assert.True(allTestsPassed, outputMessages.TrimStart());
        }

        // Note. Visual Studio MUST be running as Admin to run this test.
        // Note that failing tests will leave the projects behind, plus the apps and test certificates installed
        [Theory]
        [MemberData(nameof(GetAllFrameworksForBothVbAndCs))]
        public async Task EnsureLanguagesProduceIdenticalOutputForEachPageInNavViewAsync(string framework)
        {
            await EnsureLanguagesProduceIdenticalOutputForEachPageAsync(framework, ProjectTypes.SplitView);
        }

        // Note. Visual Studio MUST be running as Admin to run this test.
        // Note that failing tests will leave the projects behind, plus the apps and test certificates installed
        [Theory]
        [MemberData(nameof(GetAllFrameworksForBothVbAndCs))]
        public async Task EnsureLanguagesProduceIdenticalOutputForEachPageInMenuBarAsync(string framework)
        {
            await EnsureLanguagesProduceIdenticalOutputForEachPageAsync(framework, ProjectTypes.MenuBar);
        }

        private async Task EnsureLanguagesProduceIdenticalOutputForEachPageAsync(string framework, string projectType)
        {
            var genIdentities = AllPagesThatSupportSimpleTesting();

            ExecutionEnvironment.CheckRunningAsAdmin();
            WinAppDriverHelper.CheckIsInstalled();

            var app1Details = await SetUpProjectForUiTestComparisonAsync(ProgrammingLanguages.CSharp, projectType, framework, genIdentities);
            var app2Details = await SetUpProjectForUiTestComparisonAsync(ProgrammingLanguages.VisualBasic, projectType, framework, genIdentities);

            var pageExclusions = new Dictionary<string, string>();
            pageExclusions.Add("Settings", "new ImageComparer.ExclusionArea(new Rectangle(480, 360, 450, 40), 1.25f)");

            (string projectFolder, string imagesFolder) testProjectDetails;

            switch (projectType)
            {
                case ProjectTypes.MenuBar:
                    testProjectDetails = SetUpTestProjectForAllMenuBarPagesComparison(app1Details, app2Details, pageExclusions);
                    break;
                default:
                    testProjectDetails = SetUpTestProjectForAllNavViewPagesComparison(app1Details, app2Details, pageExclusions);
                    break;
            }

            var (testSuccess, testOutput) = RunWinAppDriverTests(testProjectDetails);

            // Note that failing tests will leave the projects behind, plus the apps and test certificates installed
            if (testSuccess)
            {
                UninstallAppx(app1Details.PackageFullName);
                UninstallAppx(app2Details.PackageFullName);

                RemoveCertificate(app1Details.CertificatePath);
                RemoveCertificate(app2Details.CertificatePath);

                // Parent of images folder also contains the test project
                Fs.SafeDeleteDirectory(Path.Combine(testProjectDetails.imagesFolder, ".."));
            }

            var outputMessages = string.Join(Environment.NewLine, testOutput);

            // A diff image is automatically created if the outputs differ
            if (Directory.Exists(testProjectDetails.imagesFolder)
                && Directory.GetFiles(testProjectDetails.imagesFolder, "DIFF-*.png").Any())
            {
                Assert.True(
                    testSuccess,
                    $"Failing test images in {testProjectDetails.imagesFolder}{Environment.NewLine}{Environment.NewLine}{outputMessages}");
            }
            else
            {
                Assert.True(testSuccess, outputMessages);
            }
        }

        // Note. Visual Studio MUST be running as Admin to run this test.
        // Note that failing tests will leave the projects behind, plus the apps and test certificates installed
        [Fact]
        public async Task EnsureCsFrameworksProduceIdenticalOutputForEachPageInNavViewAsync()
        {
            var genIdentities = AllPagesThatSupportSimpleTestingOnAllFrameworks();

            ExecutionEnvironment.CheckRunningAsAdmin();
            WinAppDriverHelper.CheckIsInstalled();

            var app1Details = await SetUpProjectForUiTestComparisonAsync(ProgrammingLanguages.CSharp, ProjectTypes.SplitView, Frameworks.MVVMToolkit, genIdentities);

            var errors = new List<string>();

            foreach (var framework in GetAdditionalCsFrameworks(ProjectTypes.SplitView))
            {
                var app2Details = await SetUpProjectForUiTestComparisonAsync(ProgrammingLanguages.CSharp, ProjectTypes.SplitView, framework[0].ToString(), genIdentities);

                var pageExclusions = new Dictionary<string, string>();
                pageExclusions.Add("Settings", "new ImageComparer.ExclusionArea(new Rectangle(480, 360, 450, 40), 1.25f)");

                var testProjectDetails = SetUpTestProjectForAllNavViewPagesComparison(app1Details, app2Details, pageExclusions);

                var (testSuccess, testOutput) = RunWinAppDriverTests(testProjectDetails);

                // Note that failing tests will leave the projects behind, plus the apps and test certificates installed
                if (testSuccess)
                {
                    UninstallAppx(app2Details.PackageFullName);

                    RemoveCertificate(app2Details.CertificatePath);

                    // Parent of images folder also contains the test project
                    Fs.SafeDeleteDirectory(Path.Combine(testProjectDetails.imagesFolder, ".."));
                }
                else
                {
                    errors.AddRange(testOutput);

                    // A diff image is automatically created if the outputs differ
                    if (Directory.GetFiles(testProjectDetails.imagesFolder, "DIFF-*.png").Any())
                    {
                        errors.Add($"Failing test images in {testProjectDetails.imagesFolder}");
                    }
                }
            }

            var outputMessages = string.Join(Environment.NewLine, errors);

            if (outputMessages.Any())
            {
                Assert.True(false, $"{Environment.NewLine}{Environment.NewLine}{outputMessages}");
            }
            else
            {
                UninstallAppx(app1Details.PackageFullName);
                RemoveCertificate(app1Details.CertificatePath);
            }

            WindowHelpers.BringVisualStudioToFront("Big");
            WindowHelpers.TryFlashVisualStudio("Big");
        }

        // Note. Visual Studio MUST be running as Admin to run this test.
        // Note that failing tests will leave the projects behind, plus the apps and test certificates installed
        [Fact]
        public async Task EnsureCsFrameworksProduceIdenticalOutputForEachPageInMenuBarAsync()
        {
            var genIdentities = AllPagesThatSupportSimpleTestingOnAllFrameworks();

            ExecutionEnvironment.CheckRunningAsAdmin();
            WinAppDriverHelper.CheckIsInstalled();

            var app1Details = await SetUpProjectForUiTestComparisonAsync(ProgrammingLanguages.CSharp, ProjectTypes.MenuBar, Frameworks.MVVMToolkit, genIdentities);

            var errors = new List<string>();

            foreach (var framework in GetAdditionalCsFrameworks(ProjectTypes.MenuBar))
            {
                var app2Details = await SetUpProjectForUiTestComparisonAsync(ProgrammingLanguages.CSharp, ProjectTypes.MenuBar, framework[0].ToString(), genIdentities);

                var pageExclusions = new Dictionary<string, string>();
                pageExclusions.Add("Settings", "new ImageComparer.ExclusionArea(new Rectangle(480, 360, 450, 40), 1.25f)");

                var testProjectDetails = SetUpTestProjectForAllMenuBarPagesComparison(app1Details, app2Details, pageExclusions);

                var (testSuccess, testOutput) = RunWinAppDriverTests(testProjectDetails);

                // Note that failing tests will leave the projects behind, plus the apps and test certificates installed
                if (testSuccess)
                {
                    UninstallAppx(app2Details.PackageFullName);

                    RemoveCertificate(app2Details.CertificatePath);

                    // Parent of images folder also contains the test project
                    Fs.SafeDeleteDirectory(Path.Combine(testProjectDetails.imagesFolder, ".."));
                }
                else
                {
                    errors.AddRange(testOutput);

                    // A diff image is automatically created if the outputs differ
                    if (Directory.GetFiles(testProjectDetails.imagesFolder, "DIFF-*.png").Any())
                    {
                        errors.Add($"Failing test images in {testProjectDetails.imagesFolder}");
                    }
                }
            }

            var outputMessages = string.Join(Environment.NewLine, errors);

            if (outputMessages.Any())
            {
                Assert.True(false, $"{Environment.NewLine}{Environment.NewLine}{outputMessages}");
            }
            else
            {
                UninstallAppx(app1Details.PackageFullName);
                RemoveCertificate(app1Details.CertificatePath);
            }

            WindowHelpers.BringVisualStudioToFront("Big");
            WindowHelpers.TryFlashVisualStudio("Big");
        }

        [Theory]
        [MemberData(nameof(GetAllFrameworksAndLanguageCombinations))]
        public async Task EnsureCanNavigateToEveryPageInNavViewWithoutErrorAsync(string framework, string language)
        {
            await EnsureCanNavigateToEveryPageWithoutErrorAsync(framework, language, ProjectTypes.SplitView);
        }

        [Theory]
        [MemberData(nameof(GetAllFrameworksAndLanguageCombinations))]
        public async Task EnsureCanNavigateToEveryPageInTabbedNavWithoutErrorAsync(string framework, string language)
        {
            await EnsureCanNavigateToEveryPageWithoutErrorAsync(framework, language, ProjectTypes.TabbedNav);
        }

        [Theory]
        [MemberData(nameof(GetAllFrameworksAndLanguageCombinations))]
        public async Task EnsureCanNavigateToEveryPageWithMenuBarWithoutErrorAsync(string framework, string language)
        {
            await EnsureCanNavigateToEveryPageWithoutErrorAsync(framework, language, ProjectTypes.MenuBar);
        }

        private async Task EnsureCanNavigateToEveryPageWithoutErrorAsync(string framework, string language, string projectType)
        {
            int pagesOpenedSuccessfully = 0;
            string[] pageIdentities = new string[0];

            ExecutionEnvironment.CheckRunningAsAdmin();
            WinAppDriverHelper.CheckIsInstalled();

            // InvalidOperationException occurs when WinAppDriver can't launch the app. Retrying normally solves
            await ExceptionHelper.RetryOnAsync<InvalidOperationException>(async () =>
            {
                pageIdentities = AllTestablePages(framework);

                pagesOpenedSuccessfully = 0;

                ExecutionEnvironment.CheckRunningAsAdmin();
                WinAppDriverHelper.CheckIsInstalled();
                WinAppDriverHelper.StartIfNotRunning();

                VisualComparisonTestDetails appDetails = null;

                async Task ForOpenedPage(string pageName, WindowsDriver<WindowsElement> session)
                {
                    if (pageName == "Map")
                    {
                        // For location permission
                        if (await ClickYesOnPopUpAsync(session))
                        {
                            await Task.Delay(TimeSpan.FromSeconds(2)); // Allow page to load after accepting prompt
                            pagesOpenedSuccessfully++;
                        }
                        else
                        {
                            Assert.True(false, "Failed to click \"Yes\" on popup for Map permission.");
                        }
                    }
                    else if (pageName == "Camera")
                    {
                        var cameraPermission = await ClickYesOnPopUpAsync(session); // For camera permission
                        var microphonePermission = await ClickYesOnPopUpAsync(session); // For microphone permission

                        if (cameraPermission && microphonePermission)
                        {
                            await Task.Delay(TimeSpan.FromSeconds(2)); // Allow page to load after accepting prompts
                            pagesOpenedSuccessfully++;
                        }
                        else
                        {
                            Assert.True(false, "Failed to click \"Yes\" on popups for Camera page permissions.");
                        }
                    }
                    else
                    {
                        pagesOpenedSuccessfully++;
                    }
                }

                try
                {
                    appDetails = await SetUpProjectForUiTestComparisonAsync(language, projectType, framework, pageIdentities);

                    // Have found the need to wait longer between installing an app and trying to launch it
                    // This is far from ideal but enough to unblock running tests.
                    // The delay is quiet long but small in terms of overall test execution time.
                    await Task.Delay(TimeSpan.FromSeconds(5));

                    using (var appSession = WinAppDriverHelper.LaunchAppx(appDetails.PackageFamilyName))
                    {
                        appSession.Manage().Window.Maximize();

                        // Allow app to resize and fully load
                        await Task.Delay(TimeSpan.FromSeconds(2));

                        if (projectType == ProjectTypes.MenuBar)
                        {
                            var menuItems = appSession.FindElementsByClassName("Microsoft.UI.Xaml.Controls.MenuBarItem");

                            for (int i = menuItems.Count - 1; i >= 0; i--)
                            {
                                menuItems[i].Click(); // Open menu to count sub-items
                                var subItemCount = appSession.FindElementsByClassName("MenuFlyoutItem").Count;
                                menuItems[i].Click(); // Close menu so get in a consistent state

                                // Stepping through the MenuFlyoutItems is unreliable
                                for (int j = 0; j < subItemCount; j++)
                                {
                                    menuItems[i].Click(); // Open the menu again

                                    var subItems = appSession.FindElementsByClassName("MenuFlyoutItem");

                                    var option = subItems[j].Text;

                                    // Don't close the app or the WinAppDriver will throw an error and the test will fail
                                    if (option != "Exit")
                                    {
                                        subItems[j].Click();
                                    }

                                    await Task.Delay(TimeSpan.FromMilliseconds(1500)); // Allow page to load and animations to complete

                                    if (option == "Settings")
                                    {
                                        // In a MenuBar, Settings is shown in a flyout - we must dismiss it to avoid confusing other logic that is looking at flyouts
                                        const byte Escape = 27; // From System.Windows.Forms.Keys

                                        VirtualKeyboard.KeyDown(Escape);
                                        VirtualKeyboard.KeyUp(Escape);

                                        // Clicking escape (above) doesn't always dismiss the flyout during automation
                                        // Clicking on the open page will dismiss it though
                                        appSession.Mouse.MouseMove(null, 200, 200);
                                        appSession.Mouse.MouseDown(null);
                                        appSession.Mouse.MouseUp(null);
                                    }
                                    else
                                    {
                                        await ForOpenedPage(subItems[j].Text, appSession);
                                    }
                                }
                            }
                        }
                        else
                        {
                            var menuItems = appSession.FindElementsByClassName("Microsoft.UI.Xaml.Controls.NavigationViewItem");

                            foreach (var menuItem in menuItems)
                            {
                                menuItem.Click();
                                Debug.WriteLine("Opened: " + menuItem.Text);

                                await Task.Delay(TimeSpan.FromMilliseconds(1500)); // Allow page to load and animations to complete

                                await ForOpenedPage(menuItem.Text, appSession);
                            }

                            if (appSession.TryFindElementByName("More", out WindowsElement moreBtn))
                            {
                                moreBtn.Click();

                                // Issues in automation prevent testing this so assume all ok as not had any other errors
                                // see: https://github.com/microsoft/WinAppDriver/issues/1204
                                // and: https://github.com/microsoft/microsoft-ui-xaml/issues/2736
                                pagesOpenedSuccessfully = pageIdentities.Length + 1; // work-around to get the test to pass

                                // This should only be an issue if running these tests on a small monitor.
                                // Avoid this issue by using a monitor large enough to display all the options.
                                ////// The following should work once the above underlying issues are addressed
                                ////await Task.Delay(TimeSpan.FromMilliseconds(1500));

                                ////var popupMenu = appSession.FindElementByName("Pop-up");

                                ////var moreMenuItemsCount = popupMenu.FindElementsByClassName("Microsoft.UI.Xaml.Controls.NavigationViewItem").Count;
                                ////moreBtn.Click(); // Close menu so get in a consistent state

                                ////for (int i = 0; i < moreMenuItemsCount; i++)
                                ////{
                                ////    moreBtn.Click();
                                ////    popupMenu = appSession.FindElementByName("Pop-up");

                                ////    var moreMenuItems = popupMenu.FindElementsByClassName("Microsoft.UI.Xaml.Controls.NavigationViewItem");

                                ////    await Task.Delay(TimeSpan.FromSeconds(5));

                                ////    var x = popupMenu.FindElementByClassName("TextBlock");

                                ////    moreMenuItems[i].Click();

                                ////    await Task.Delay(TimeSpan.FromMilliseconds(1500)); // Allow page to load and animations to complete

                                ////    await ForOpenedPage(moreMenuItems[i].Text, appSession);
                                ////}
                            }
                        }
                    }

                    // Don't leave the app maximized in case we want to open the app again.
                    // Some controls handle layout differently when the app is first opened maximized
                    VirtualKeyboard.RestoreMaximizedWindow();
                }
                catch (Exception exc)
                {
                    System.Diagnostics.Debug.WriteLine(exc.ToString());
                    throw;
                }
                finally
                {
                    if (appDetails != null)
                    {
                        UninstallAppx(appDetails.PackageFullName);
                        RemoveCertificate(appDetails.CertificatePath);
                    }

                    WinAppDriverHelper.StopIfRunning();
                }

            });
            var expectedPageCount = pageIdentities.Length + 1; // Add 1 for"Main page" added as well by default

            Assert.True(pagesOpenedSuccessfully > 0, "No pages were navigated to");
            Assert.True(expectedPageCount == pagesOpenedSuccessfully, $"Not all pages were opened successfully. Expected {expectedPageCount} but got {pagesOpenedSuccessfully}.");

            await Task.CompletedTask;
        }
    }
}
