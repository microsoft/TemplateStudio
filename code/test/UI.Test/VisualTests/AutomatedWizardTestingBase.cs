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
using System.Threading.Tasks;
using Microsoft.Templates.Core;
using OpenQA.Selenium.Appium.Windows;
using OpenQA.Selenium.Remote;
using Xunit;

namespace Microsoft.UI.Test.VisualTests
{
    public class AutomatedWizardTestingBase : IDisposable
    {
        protected List<string> NonDefaultVsCultures => new List<string>
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

        protected string DefaultCulture => "en-US";

        protected List<string> AllVsCultures => new List<string>();

        protected WindowsDriver<WindowsElement> WizardSession { get; private set; }

        protected AutomatedWizardTestingBase()
        {
            AllVsCultures.Add(DefaultCulture);
            AllVsCultures.AddRange(NonDefaultVsCultures);

            CheckWinAppDriverInstalled();
            StartWinAppDriverIfNotRunning();
        }

        public void Dispose()
        {
            WizardSession?.Dispose();

            StopWinAppDriverIfRunning();
        }

        protected static string GetRootFolderForTestOutput()
        {
            var testRoot = @"C:\UIT";

            var wizardScreenshotsRoot = Path.Combine(testRoot, "WizardScreenshots");

            var theseTestsRoot = Path.Combine(wizardScreenshotsRoot, DateTime.Now.FormatAsDateHoursMinutes());

            Directory.CreateDirectory(theseTestsRoot);
            return theseTestsRoot;
        }

        protected void TakeScreenshot(string fileName)
        {
            var screenshot = WizardSession.GetScreenshot();
            screenshot.SaveAsFile(fileName, ImageFormat.Png);
        }

        protected void ForEachPageInProjectWizard(string culture, string progLanguage, bool includeDetails, Action<string> action)
        {
            LaunchApp(culture, progLanguage);

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

        protected void ForEachStepInAddPageWizard(string culture, string progLanguage, bool includeDetails, Action<string> action)
        {
            LaunchApp(culture, progLanguage, ui: "PAGE");

            action.Invoke("AddPage_Select");

            SelectStep(2);
            action.Invoke("AddPage_Summary");

            CloseApp();
        }

        protected void ForEachStepInAddFeatureWizard(string culture, string progLanguage, bool includeDetails, Action<string> action)
        {
            LaunchApp(culture, progLanguage, ui: "FEATURE");

            action.Invoke("AddFeature_Select");

            SelectStep(2);
            action.Invoke("AddFeature_Summary");

            CloseApp();
        }

        private void ViewAllDetails(string stepName, Action<string> action)
        {
            var scrollCount = 0;

            if (WizardSession.TryFindElementsByClassName("ListBoxItem", out ReadOnlyCollection<WindowsElement> items))
            {
                var itemCount = 0;

                foreach (var item in items)
                {
                    var elementName = item.GetAttribute("Name");

                    //if (elementName == "Microsoft.Templates.UI.ViewModels.Common.MetadataInfoViewModel"
                    //    || elementName == "Microsoft.Templates.UI.ViewModels.Common.TemplateInfoViewModel")
                    if (elementName != "Microsoft.Templates.UI.Controls.Step")
                    {
                        itemCount++;

                        // TODO [ML]: these hardcoded values are based on a presumed screen size - need to add proper detection of items not clickable as off screen
                        if ((stepName == "Pages" && itemCount == 7)
                            || (stepName == "Features" && new[] { 6, 9 }.Contains(itemCount)))
                        {
                            if (WizardSession.TryFindElementByAutomationId("RepeatButton", "PageDown", out WindowsElement pageDownButton))
                            {
                                pageDownButton.Click();

                                Pause();
                                action.Invoke($"{stepName}_scroll{++scrollCount}");
                            }
                        }

                        var itemName = item.FindElement(OpenQA.Selenium.By.ClassName("TextBlock")).Text.Replace("/", string.Empty);

                        if (itemName == ":")
                        {
                            itemName = "Uri Scheme"; // Work around for icon actually being text ("://")
                        }

                        var detailsLink = item.FindElement(OpenQA.Selenium.By.ClassName("Hyperlink"));
                        detailsLink.Click();

                        Pause();
                        action.Invoke($"{stepName}_{itemName}");

                        if (WizardSession.TryFindElementByClassNameAndText("Hyperlink", "Back", out WindowsElement backlink))
                        {
                            backlink.Click();
                        }
                    }
                }
            }
        }

        private void SelectStep(int stepNumber)
        {
            if (WizardSession.TryFindElementByAutomationId("ListBoxItem", $"Step{stepNumber - 1}", out var listItem))
            {
                listItem.Click();
                Pause();
            }
        }

        // Default value was arbitrarily chosen to allow UI to update. Extend if needed but will have a big impact on tests as this may be called hundreds of times.
        protected void Pause(double seconds = 1)
        {
#pragma warning disable VSTHRD002 // Avoid problematic synchronous waits - VS specific check but this doesn't run in VS context
            Task.Delay(TimeSpan.FromSeconds(seconds)).Wait();
#pragma warning restore VSTHRD002 // Avoid problematic synchronous waits
        }

        private void CloseApp()
        {
            if (WizardSession.TryFindElementByAutomationId("Button", "CancelButton", out var cancelButton))
            {
                cancelButton.Click();
            }
        }

        private void LaunchApp(string culture, string progLang = ProgrammingLanguages.CSharp, string appName = "", string ui = "Project")
        {
            var appCapabilities = new DesiredCapabilities();

            appCapabilities.SetCapability("app", @"..\..\..\VsEmulator\bin\Debug\VsEmulator.exe");

            var cmdLineArgs = $"-c {culture} -l {progLang} -u {ui}";

            if (!string.IsNullOrWhiteSpace(appName))
            {
                cmdLineArgs += $" -n {appName}";
            }

            appCapabilities.SetCapability("appArguments", cmdLineArgs);  // e.g. -c en-US -l C# -n testapp -u Project
            appCapabilities.SetCapability("appWorkingDir", Environment.CurrentDirectory);

            WizardSession = new WindowsDriver<WindowsElement>(new Uri("http://127.0.0.1:4723"), appCapabilities);
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
