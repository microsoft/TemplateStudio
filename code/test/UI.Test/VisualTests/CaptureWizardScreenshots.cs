// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Diagnostics;
using System.Drawing.Imaging;
using System.IO;
using System.Windows.Media.Animation;
using Microsoft.Templates.Core;
using OpenQA.Selenium;
using OpenQA.Selenium.Appium.Windows;
using OpenQA.Selenium.Remote;
using Xunit;

namespace Microsoft.UI.Test.VisualTests
{
    // The methods in this class aren't real tests, they just use the test infrastructure to captures screenshots for manual review
    [Collection("UI-Visuals")]
    [Trait("ExecutionSet", "ManualOnly")]
    [Trait("Type", "WinAppDriver")]
    public class CaptureWizardScreenshots : AutomatedWizardTestingBase
    {
        [Fact]
        public void GetScreenshots_NewProject_CS_AllCultures()
        {
            var testOutputRoot = GetRootFolderForTestOutput();

            foreach (var culture in AllVsCultures)
            {
                ForEachPageInProjectWizard(culture, ProgrammingLanguages.CSharp, true, pageName =>
                {
                    TakeScreenshot(Path.Combine(testOutputRoot, $"{culture}_{Uri.EscapeUriString(pageName)}.png"));
                });
            }
        }

        [Fact]
        public void GetScreenshots_NewProject_CSandVB_DefaultCulture()
        {
            var testOutputRoot = GetRootFolderForTestOutput();

            foreach (var progLang in ProgrammingLanguages.GetAllLanguages())
            {
                ForEachPageInProjectWizard(DefaultCulture, progLang, true, pageName =>
                {
                    TakeScreenshot(Path.Combine(testOutputRoot, $"{DefaultCulture}_{progLang}_{Uri.EscapeUriString(pageName)}.png"));
                });
            }
        }

        [Fact]
        public void GetScreenshots_AddPage_CSandVB_DefaultCulture()
        {
            var testOutputRoot = GetRootFolderForTestOutput();

            foreach (var progLang in ProgrammingLanguages.GetAllLanguages())
            {
                ForEachStepInAddPageWizard(DefaultCulture, progLang, true, pageName =>
                {
                    TakeScreenshot(Path.Combine(testOutputRoot, $"{DefaultCulture}_{progLang}_{Uri.EscapeUriString(pageName)}.png"));
                });
            }
        }

        // TODO [ML]: need to launch through VS (not VSEMulator) to see "proper" theme support
        [Fact]
        public void GetScreenshots_Wizard_CS_DefaultCulture_HighContrast()
        {
            try
            {
                EnableHighContrast();

                var testOutputRoot = GetRootFolderForTestOutput();

                foreach (var progLang in ProgrammingLanguages.GetAllLanguages())
                {
                    ForEachPageInProjectWizard(DefaultCulture, progLang, false, pageName =>
                    {
                        TakeScreenshot(Path.Combine(testOutputRoot, $"{DefaultCulture}_{progLang}_{Uri.EscapeUriString(pageName)}.png"));
                    });
                }
            }
            finally
            {
                DisableHighContrast();
            }
        }

        private void EnableHighContrast()
        {
            SetHighContrast("High Contrast #1");
        }

        private void SetHighContrast(string option)
        {
            Process.Start("ms-settings:easeofaccess-highcontrast");

            var desktop = GetDesktopSession();

            var settingsSession = desktop.FindElementByName("Settings");

            // TODO [ML]: The Insider Preview does this slightly differently - support both versions
            var highContrastOption = settingsSession.FindElementByName("High contrast theme");
            highContrastOption.Click();

            highContrastOption.FindElements(By.ClassName("ComboBoxItem"));
            var option1 = highContrastOption.FindElementByName(option);
            option1.Click();

            // TODO [ML]: add a TabToButtonLabelled("Xxxx") helper
            if (option == "None")
            {
                desktop.Keyboard.SendKeys(Keys.Tab);
            }
            else
            {
                desktop.Keyboard.SendKeys(Keys.Tab);
                desktop.Keyboard.SendKeys(Keys.Tab);
                desktop.Keyboard.SendKeys(Keys.Tab);
                desktop.Keyboard.SendKeys(Keys.Tab);
                desktop.Keyboard.SendKeys(Keys.Tab);
                desktop.Keyboard.SendKeys(Keys.Tab);
                desktop.Keyboard.SendKeys(Keys.Tab);
                desktop.Keyboard.SendKeys(Keys.Tab);
                desktop.Keyboard.SendKeys(Keys.Tab);
            }

            desktop.Keyboard.SendKeys(Keys.Enter); // Apply;
            Pause(20);  // To allow changes to be made
        }

        private WindowsDriver<WindowsElement> desktopSession;

        private WindowsDriver<WindowsElement> GetDesktopSession()
        {
            if (desktopSession == null)
            {
                DesiredCapabilities appCapabilities = new DesiredCapabilities();
                appCapabilities.SetCapability("app", "Root");
                desktopSession = new WindowsDriver<WindowsElement>(new Uri("http://127.0.0.1:4723"), appCapabilities);
            }

            return desktopSession;
        }

        private void DisableHighContrast()
        {
            SetHighContrast("None");
        }
    }
}
