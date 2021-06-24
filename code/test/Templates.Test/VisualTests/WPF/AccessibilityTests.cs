// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using WindowsTestHelpers;
using Microsoft.Templates.Core;
using Xunit;
using System.Collections.Generic;

namespace Microsoft.Templates.Test.WPf
{
    [Collection("GenerationCollection")]
    public class AccessibilityTests : VisualComparisonTests
    {
        public AccessibilityTests(GenerationFixture fixture)
            : base(fixture)
        {
        }

        /// <summary>
        /// Warning - Expect this to fail!
        /// ******************************
        ///
        /// The following external issues prevent this test from passing:
        ///
        /// Symbols in the MetroWindow (& WebView buttons) - https://github.com/dotnet/wpf/issues/3296
        /// MetroWindow - https://github.com/MahApps/MahApps.Metro/issues/3894
        /// WebView - The content loaded in the page may not be fully accessible and so cause the test to fail. 
        /// 
        /// There may still be value in running this test and reviewing the actual results with known external issues.
        /// </summary>
        [Fact]
        [Trait("ExecutionSet", "ManualOnly")]
        [Trait("Type", "WinAppDriver")]
        public async Task RunBasicAccessibilityChecksAgainstEachPageWpfAsync()
        {
            // This test does not run against all combinations but relies on other tests to ensure output is the same for each combination.
            // In theory this means that there should be no need to repeat tests as they would find the same things.
            // As this is a slow test to run we do not want to increase overall execution time unnecessarily.

            ExecutionEnvironment.CheckRunningAsAdmin();
            WinAppDriverHelper.CheckIsInstalled();
            WinAppDriverHelper.StartIfNotRunning();

            var pagesFoundWithIssues = 0;

            var rootFolder = $"{Path.GetPathRoot(Environment.CurrentDirectory)}UIT\\ALLY\\{DateTime.Now:dd_HHmmss}\\";
            var reportFolder = Path.Combine(rootFolder, "Reports");

            void ForOpenedPage(string projectName)
            {
                var processes = Process.GetProcessesByName(projectName);

                var config = Axe.Windows.Automation.Config.Builder.ForProcessId(processes[0].Id);

                config.WithOutputFileFormat(Axe.Windows.Automation.OutputFileFormat.A11yTest);
                config.WithOutputDirectory(reportFolder);

                var scanner = Axe.Windows.Automation.ScannerFactory.CreateScanner(config.Build());

                try
                {
                    var scanResults = scanner.Scan();

                    if (scanResults.ErrorCount > 0)
                    {
                        pagesFoundWithIssues++;
                    }
                }
                catch (Axe.Windows.Automation.AxeWindowsAutomationException exc)
                {
                    Assert.True(exc == null, exc.ToString());
                }
            }

            try
            {
                var wpfTestPages = new List<string>
                {
                    "wts.Wpf.Page.Blank",
                    "wts.Wpf.Page.ListDetails",
                    "wts.Wpf.Page.Settings",
                    "wts.Wpf.Page.WebView",
                };

                var appDetails = await SetUpWpfProjectForUiTestComparisonAsync(ProgrammingLanguages.CSharp, ProjectTypes.SplitView, Frameworks.MVVMLight, wpfTestPages);

                var exePath = Path.Combine(appDetails.ProjectPath, appDetails.ProjectName, "bin", "Release", "netcoreapp3.1", $"{appDetails.ProjectName}.exe");

                using (var appSession = WinAppDriverHelper.LaunchExe(exePath))
                {
                    appSession.Manage().Window.Maximize();

                    await Task.Delay(TimeSpan.FromSeconds(2));

                    var menuItems = appSession.FindElementsByClassName("ListBoxItem");

                    foreach (var menuItem in menuItems)
                    {
                        menuItem.Click();

                        await Task.Delay(TimeSpan.FromMilliseconds(1500)); // Allow page to load and animations to complete

                        ForOpenedPage(appDetails.ProjectName);
                    }
                }

                // Don't leave the app maximized in case we want to open the app again.
                // Some controls handle layout differently when the app is first opened maximized
                VirtualKeyboard.RestoreMaximizedWindow();
            }
            finally
            {
                WinAppDriverHelper.StopIfRunning();
            }

            Assert.True(0 == pagesFoundWithIssues, $"Accessibility issues found. For details, see reports in '{reportFolder}'.");
        }
    }
}
