// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using AutomatedUITests;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium.Appium.Windows;
using WindowsTestHelpers;

namespace AutomatedUITests.Tests
{
    [TestClass]
    public class LaunchBothAppsAndCompareInitialScreenshots
    {
        private string App1Filename { get; }
        private string App2Filename { get; }
        private string DiffFilename { get; }

        public LaunchBothAppsAndCompareInitialScreenshots()
        {
            App1Filename = $"CompareInitialScreenshots-{TestAppInfo.AppName1}.png";
            App2Filename = $"CompareInitialScreenshots-{TestAppInfo.AppName2}.png";
            DiffFilename = $"DIFF-CompareInitialScreenshots-{TestAppInfo.AppName1}-{TestAppInfo.AppName2}.png";
        }

        [TestMethod]
        public async Task CompareInitialScreenshots()
        {
            WinAppDriverHelper.StartIfNotRunning();

            if (!Directory.Exists(TestAppInfo.ScreenshotsFolder))
            {
                Directory.CreateDirectory(TestAppInfo.ScreenshotsFolder);
            }

            // Hide other apps to give a consistent backdrop for acrylic textures
            VirtualKeyboard.MinimizeAllWindows();

            async Task GetScreenshot(string pfn, string fileName)
            {
                using (var session = WinAppDriverHelper.LaunchAppx(pfn))
                {
                    if (TestAppInfo.NoClickCount > 0)
                    {
                        await Task.Delay(TimeSpan.FromSeconds(2));

                        for (var i = 0; i < TestAppInfo.NoClickCount; i++)
                        {
                            await ClickNoOnPopUpAsync(session);
                        }
                    }

                    session.Manage().Window.Maximize();

                    if (TestAppInfo.LongPauseAfterLaunch)
                    {
                        await Task.Delay(TimeSpan.FromSeconds(30)); // Because loading the first frame of the video can sometimes take this long
                    }
                    else
                    {
                        await Task.Delay(TimeSpan.FromSeconds(2));
                    }

                    var screenshot = session.GetScreenshot();
                    screenshot.SaveAsFile(Path.Combine(TestAppInfo.ScreenshotsFolder, fileName), ImageFormat.Png);

                    // Don't leave the app maximized in case we want to open the app again.
                    // Some controls handle layout differently when the app is first opened maximized
                    VirtualKeyboard.RestoreMaximizedWindow();
                }
            }

            await GetScreenshot(TestAppInfo.AppPfn1, App1Filename);
            await GetScreenshot(TestAppInfo.AppPfn2, App2Filename);

            var imageCompareResult = CheckImagesAreTheSame(TestAppInfo.ScreenshotsFolder, App1Filename, App2Filename);

            Assert.IsTrue(imageCompareResult, $"Images do not match.{Environment.NewLine}App1: {App1Filename}{Environment.NewLine}App2: {App2Filename}{Environment.NewLine}See results in '{TestAppInfo.ScreenshotsFolder}'");
        }

        private async Task<bool> ClickNoOnPopUpAsync(WindowsDriver<WindowsElement> session)
        {
            await Task.Delay(TimeSpan.FromSeconds(1)); // Allow extra time for popup to be displayed
            var popups = session.FindElementsByAccessibilityId("Popup Window");
            if (popups.Count == 1)
            {
                var no = popups[0].FindElementsByName("No");
                if (no.Count == 1)
                {
                    no[0].Click();
                    return true;
                }
            }
            return false;
        }

        private bool CheckImagesAreTheSame(string folder, string fileName1, string fileName2)
        {
            var imagePath1 = Path.Combine(folder, fileName1);
            var imagePath2 = Path.Combine(folder, fileName2);

            var image1 = Image.FromFile(imagePath1);
            var image2 = Image.FromFile(imagePath2);

            var percentageDifference = ImageComparer.PercentageDifferent(image1, image2, GetAllExclusionAreas());

            if (percentageDifference > 0f)
            {
                var diffImage = image1.GetDifferenceImage(image2, GetAllExclusionAreas());

                diffImage.Save(Path.Combine(folder, DiffFilename), ImageFormat.Png);
                return false;
            }
            else
            {
                return true;
            }
        }

        private ImageComparer.ExclusionArea[] GetAllExclusionAreas()
        {
            var result = new ImageComparer.ExclusionArea[TestAppInfo.ExclusionAreas.Length + 1];

            // We always exclude the area the app name occupies in the title bar as these will always be different
            result[0] = new ImageComparer.ExclusionArea(new Rectangle(0, 0, 600, 40), scaleFactor: 1.25f);

            Array.Copy(TestAppInfo.ExclusionAreas, 0, result, 1, TestAppInfo.ExclusionAreas.Length);

            return result;
        }
    }
}
