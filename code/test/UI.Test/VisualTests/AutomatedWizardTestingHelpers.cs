// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Management.Automation;
using Microsoft.Templates.Core;
using OpenQA.Selenium.Appium.Windows;
using OpenQA.Selenium.Remote;
using Xunit;
using Task = System.Threading.Tasks.Task;

namespace Microsoft.UI.Test.VisualTests
{
    [Collection("UI-Visuals")]
    [Trait("ExecutionSet", "ManualOnly")]
    [Trait("Type", "WinAppDriver")]
    public class AutomatedWizardTestingHelpers : IDisposable
    {
        private readonly List<string> nonDefaultVsCultures = new List<string>
        {
            "cs-CZ",
            "de-DE",
            "es-ES",
            "fr-FR",
            "it-IT",
            "ja-JP",
            "ko-KR",
            "pl-PL",
            "pt-BR",
            "ru-RU",
            "tr-TR",
            "zh-CN",
            "zh-TW"
        };

        private readonly List<string> allVsCultures = new List<string>();

        private WindowsDriver<WindowsElement> wizardSession;

        public AutomatedWizardTestingHelpers()
        {
            allVsCultures.Add("en-US");
            allVsCultures.AddRange(nonDefaultVsCultures);

            CheckWinAppDriverInstalled();
            StartWinAppDriverIfNotRunning();
        }

        public void Dispose()
        {
            wizardSession?.Dispose();

            StopWinAppDriverIfRunning();
        }

        [Fact]
        public async Task EnsureLaunchPageVisualsAreEquivalentAsync()
        {
            var defaultText = GetDefaultText();

            foreach (var culture in nonDefaultVsCultures)
            {
                var localizedText = GetAllUiText(culture);

                // compare localizedText with defaultText
            }
        }

        // This isn't a real test, it just captures screenshots for manual review
        [Fact]
        public void GetScreenshotsOfEveryPage()
        {
            var testRoot = @"C:\UIT";

            var wizardScreenshotsRoot = Path.Combine(testRoot, "WizardScreenshots");

            var theseTestsRoot = Path.Combine(wizardScreenshotsRoot, DateTime.Now.FormatAsDateHoursMinutes());

            Directory.CreateDirectory(theseTestsRoot);
            foreach (var culture in allVsCultures)
            {
                ForEachPageInApp(culture, true, pageName =>
                {
                    var screenshot = wizardSession.GetScreenshot();
                    screenshot.SaveAsFile(Path.Combine(theseTestsRoot, $"{culture}_{Uri.EscapeUriString(pageName)}.png"), ImageFormat.Png);
                });
            }
        }

        private Dictionary<string, string> GetDefaultText()
        {
            return GetAllUiText();
        }

        private Dictionary<string, string> GetAllUiText(string culture = "")
        {
            var result = new Dictionary<string, string>();

            ForEachPageInApp(culture, false, pageName => { result.Add(pageName, "XXXX"); });

            return result;
        }

        private void ForEachPageInApp(string culture, bool includeDetails, Action<string> action)
        {
            LaunchApp(culture);

            action.Invoke("ProjectType");

            if (includeDetails)
            {
                ViewAllDetails("ProjectType", action);
            }

            SelectStep(2);
            action.Invoke("Framework");

            if (includeDetails)
            {
                ViewAllDetails("Framework", action);
            }

            SelectStep(3);
            action.Invoke("Pages");

            if (includeDetails)
            {
                ViewAllDetails("Pages", action);
            }

            SelectStep(4);
            action.Invoke("Features");

            if (includeDetails)
            {
                ViewAllDetails("Features", action);
            }

            CloseApp();
        }

        private void ViewAllDetails(string stepName, Action<string> action)
        {
            var scrollCount = 0;

            if (wizardSession.TryFindElementsByClassName("ListBoxItem", out ReadOnlyCollection<WindowsElement> items))
            {
                var itemCount = 0;

                foreach (var item in items)
                {
                    var elementName = item.GetAttribute("Name");

                    if (elementName == "Microsoft.Templates.UI.ViewModels.Common.MetadataInfoViewModel"
                     || elementName == "Microsoft.Templates.UI.ViewModels.Common.TemplateInfoViewModel")
                    {
                        itemCount++;

                        if ((stepName == "Pages" && itemCount == 7)
                         || (stepName == "Features" && new[] { 6, 9 }.Contains(itemCount)))
                        {
                            if (wizardSession.TryFindElementByAutomationId("RepeatButton", "PageDown", out WindowsElement pageDownButton))
                            {
                                pageDownButton.Click();

                                Pause();
                                action.Invoke($"{stepName}_scroll{++scrollCount}");
                            }
                        }

                        var itemName = item.FindElement(OpenQA.Selenium.By.ClassName("TextBlock")).Text.Replace("/", string.Empty);

                        if (itemName == ":")
                        {
                            itemName = "Uri Scheme"; // Hack for icon actually being text ("://")
                        }

                        var detailsLink = item.FindElement(OpenQA.Selenium.By.ClassName("Hyperlink"));
                        detailsLink.Click();

                        Pause();
                        action.Invoke($"{stepName}_{itemName}");

                        if (wizardSession.TryFindElementByClassNameAndText("Hyperlink", "Back", out WindowsElement backlink))
                        {
                            backlink.Click();
                        }
                    }
                }
            }
        }

        private void SelectStep(int stepNumber)
        {
            if (wizardSession.TryFindElementByAutomationId("ListBoxItem", $"Step{stepNumber}.", out var listItem))
            {
                listItem.Click();
                Pause();
            }
        }

        private void Pause()
        {
#pragma warning disable VSTHRD002 // Avoid problematic synchronous waits - VS specific check but this doesn't run in VS context
            Task.Delay(TimeSpan.FromSeconds(1)).Wait();
#pragma warning restore VSTHRD002 // Avoid problematic synchronous waits
        }

        private void CloseApp()
        {
            if (wizardSession.TryFindElementByAutomationId("Button", "CancelButton", out var cancelButton))
            {
                cancelButton.Click();
            }
        }

        private void LaunchApp(string culture, string progLang = ProgrammingLanguages.CSharp, string appName = "")
        {
            var appCapabilities = new DesiredCapabilities();

            // TODO [ML] need to get relative path working
              appCapabilities.SetCapability("app", @"..\..\..\TestWizardLauncher\bin\Debug\TestWizardLauncher.exe");
            //appCapabilities.SetCapability("app", @"C:\Users\matt\Documents\GitHub\WTSMyFork\WindowsTemplateStudio\code\test\TestWizardLauncher\bin\Debug\TestWizardLauncher.exe");
            appCapabilities.SetCapability("appArguments", $"{culture} {progLang} {appName}");

            // TODO [ML] use current directory?
            appCapabilities.SetCapability("appWorkingDir", Environment.CurrentDirectory);

            wizardSession = new WindowsDriver<WindowsElement>(new Uri("http://127.0.0.1:4723"), appCapabilities);
        }

        private void CheckWinAppDriverInstalled()
        {
            Assert.True(
                File.Exists(@"C:\Program Files (x86)\Windows Application Driver\WinAppDriver.exe"),
                "WinAppDriver is not installed. Download from https://github.com/Microsoft/WinAppDriver/releases");
        }

        private void StartWinAppDriverIfNotRunning()
        {
            var script = @"
$wad = Get-Process WinAppDriver -ErrorAction SilentlyContinue

if ($wad -eq $Null)
{
    Start-Process ""C:\Program Files (x86)\Windows Application Driver\WinAppDriver.exe""
}";

            ExecutePowerShellScript(script);
        }

        private void StopWinAppDriverIfRunning()
        {
            var script = @"
$wad = Get-Process WinAppDriver -ErrorAction SilentlyContinue

if ($wad -ne $null)
{
    $wad.CloseMainWindow()
}";

            ExecutePowerShellScript(script);
        }

        private Collection<PSObject> ExecutePowerShellScript(string script, bool outputOnError = false)
        {
            using (var ps = System.Management.Automation.PowerShell.Create())
            {
                ps.AddScript(script);

                Collection<PSObject> psOutput = ps.Invoke();

                if (ps.Streams.Error.Count > 0)
                {
                    foreach (var errorRecord in ps.Streams.Error)
                    {
                        Debug.WriteLine(errorRecord.ToString());
                    }

                    // Some things (such as failing test execution) report an error but we still want the full output
                    if (!outputOnError)
                    {
                        throw new PSInvalidOperationException(ps.Streams.Error.First().ToString());
                    }
                }

                return psOutput;
            }
        }
    }
}
