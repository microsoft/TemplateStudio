// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Threading.Tasks;
using AutomatedUITests;
using AutomatedUITests.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium.Appium.Windows;

namespace AutomatedUITests.Tests
{
    [TestClass]
    public class LaunchBothAppsAndCompareInitialScreenshots : TestBase
    {
        private string App1Filename { get; }
        private string App2Filename { get; }
        private string DiffFilename { get; }

        public LaunchBothAppsAndCompareInitialScreenshots()
        {
            App1Filename = $"CompareInitialScreenshots-{TestAppInfo.AppName1}.png";
            App2Filename = $"CompareInitialScreenshots-{TestAppInfo.AppName2}.png";
            DiffFilename = $"CompareInitialScreenshots-{TestAppInfo.AppName1}-{TestAppInfo.AppName2}-Diff.png";
        }

        [TestMethod]
        public void CompareInitialScreenshots()
        {
            if (!Directory.Exists(TestAppInfo.ScreenshotsFolder))
            {
                Directory.CreateDirectory(TestAppInfo.ScreenshotsFolder);
            }

            // Hide other apps to all a consistent backdrop for acrylic textures
            VirtualKeyboard.MinimizeAllWindows();

            using (var appSession1 = base.GetAppSession(TestAppInfo.AppPfn1))
            {
                //// See https://github.com/Microsoft/WindowsTemplateStudio/issues/1717
                //// ClickYesIfPermissionDialogShown(appSession1);

                appSession1.Manage().Window.Maximize();
                Task.Delay(TimeSpan.FromSeconds(2)).Wait();

                var screenshot = appSession1.GetScreenshot();
                screenshot.SaveAsFile(Path.Combine(TestAppInfo.ScreenshotsFolder, App1Filename), ImageFormat.Png);
            }

            using (var appSession2 = base.GetAppSession(TestAppInfo.AppPfn2))
            {
                //// See https://github.com/Microsoft/WindowsTemplateStudio/issues/1717
                //// ClickYesIfPermissionDialogShown(appSession2);

                appSession2.Manage().Window.Maximize();
                Task.Delay(TimeSpan.FromSeconds(2)).Wait();

                var screenshot = appSession2.GetScreenshot();
                screenshot.SaveAsFile(Path.Combine(TestAppInfo.ScreenshotsFolder, App2Filename), ImageFormat.Png);
            }

            var imageCompareResult = CheckImagesAreTheSame(TestAppInfo.ScreenshotsFolder, App1Filename, App2Filename);

            Assert.IsTrue(imageCompareResult);
        }

        private void ClickYesIfPermissionDialogShown(WindowsDriver<WindowsElement> session)
        {
            if (session.TryFindElementByName("Yes", out var yesButton))
            {
                yesButton.Click();
                Task.Delay(TimeSpan.FromSeconds(2)).Wait(); // Allow time for dialog to be dismissed
            }
        }

        private bool CheckImagesAreTheSame(string folder, string fileName1, string fileName2)
        {
            var imagePath1 = Path.Combine(folder, fileName1);
            var imagePath2 = Path.Combine(folder, fileName2);

            var image1 = Image.FromFile(imagePath1);
            var image2 = Image.FromFile(imagePath2);

            var percentageDifference = ImageComparer.PercentageDifferent(image1, image2);

            if (percentageDifference > 0f)
            {
                var diffImage = image1.GetDifferenceImage(image2);

                diffImage.Save(Path.Combine(folder, DiffFilename), ImageFormat.Png);

                return false;
            }
            else
            {
                return true;
            }
        }
    }
}
