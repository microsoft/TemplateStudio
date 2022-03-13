// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using AutomatedUITests;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium.Appium.Windows;
using WindowsTestHelpers;

namespace AutomatedUITests.Tests
{
    [TestClass]
    public class LaunchBothAppsAndCompareAllNavViewPages
    {
        private string AppFileNameFormat { get; } = "CompareNavViewPages-{0}-{1}.png";

        [TestMethod]
        public void CompareScreenshotsOfAllPages()
        {
            WinAppDriverHelper.StartIfNotRunning();

            if (!Directory.Exists(TestAppInfo.ScreenshotsFolder))
            {
                Directory.CreateDirectory(TestAppInfo.ScreenshotsFolder);
            }

            // Hide other apps to all a consistent backdrop for acrylic textures
            VirtualKeyboard.MinimizeAllWindows();

            using (var appSession1 = WinAppDriverHelper.LaunchAppx(TestAppInfo.AppPfn1))
            {
                appSession1.Manage().Window.Maximize();

                Task.Delay(TimeSpan.FromSeconds(2)).Wait();

                var menuItems = appSession1.FindElementsByClassName("Microsoft.UI.Xaml.Controls.MenuBarItem");

                foreach (var menuItem in menuItems)
                {
                    menuItem.Click();
                    Task.Delay(TimeSpan.FromMilliseconds(1500)).Wait(); // Allow page to load and animations to complete

                    var screenshot = appSession1.GetScreenshot();
                    var fileName = string.Format(AppFileNameFormat, TestAppInfo.AppName1, menuItem.Text);
                    screenshot.SaveAsFile(Path.Combine(TestAppInfo.ScreenshotsFolder, fileName), ImageFormat.Png);
                }

                // Don't leave the app maximized in case we want to open the app again.
                // Some controls handle layout differently when the app is first opened maximized
                VirtualKeyboard.RestoreMaximizedWindow();
            }

            using (var appSession2 = WinAppDriverHelper.LaunchAppx(TestAppInfo.AppPfn2))
            {
                appSession2.Manage().Window.Maximize();

                Task.Delay(TimeSpan.FromSeconds(2)).Wait();

                var menuItems = appSession2.FindElementsByClassName("Microsoft.UI.Xaml.Controls.MenuBarItem");

                foreach (var menuItem in menuItems)
                {
                    menuItem.Click();
                    Task.Delay(TimeSpan.FromMilliseconds(1500)).Wait(); // Allow page to load and animations to complete

                    var screenshot = appSession2.GetScreenshot();
                    var fileName = string.Format(AppFileNameFormat, TestAppInfo.AppName2, menuItem.Text);
                    screenshot.SaveAsFile(Path.Combine(TestAppInfo.ScreenshotsFolder, fileName), ImageFormat.Png);
                }

                // Don't leave the app maximized in case we want to open the app again.
                // Some controls handle layout differently when the app is first opened maximized
                VirtualKeyboard.RestoreMaximizedWindow();
            }

            var app1Images = Directory.GetFiles(TestAppInfo.ScreenshotsFolder, $"*-{TestAppInfo.AppName1}*.png");

            var allImagesCount = Directory.GetFiles(TestAppInfo.ScreenshotsFolder, "*.png").Length;

            if (allImagesCount != app1Images.Length * 2)
            {
                Assert.Fail($"Did not produce equal number of page screenshots for each app. {TestAppInfo.ScreenshotsFolder}");
            }

            var nonMatchingImages = new List<string>();

            var toDelete = new List<string>();

            foreach (var file in app1Images)
            {
                var equivFile = file.Replace(TestAppInfo.AppName1, TestAppInfo.AppName2);

                var imagesAreTheSame = CheckImagesAreTheSame(TestAppInfo.ScreenshotsFolder, file, equivFile);

                if (imagesAreTheSame)
                {
                    toDelete.Add(file);
                    toDelete.Add(equivFile);
                }
                else
                {
                    nonMatchingImages.Add(equivFile);
                }
            }

            foreach (var fileToDelete in toDelete)
            {
                Task.Run(() =>
                {
                    try
                    {
                        File.Delete(fileToDelete);
                    }
                    catch (Exception)
                    {
                        Task.Delay(TimeSpan.FromSeconds(5)).Wait();
                        File.Delete(fileToDelete);
                    }
                });

            }

            Assert.IsFalse(nonMatchingImages.Any(), string.Join(Environment.NewLine, nonMatchingImages));
        }

        private bool CheckImagesAreTheSame(string folder, string fileName1, string fileName2)
        {
            var imagePath1 = Path.Combine(folder, fileName1);
            var imagePath2 = Path.Combine(folder, fileName2);

            var image1 = Image.FromFile(imagePath1);
            var image2 = Image.FromFile(imagePath2);

            var pageName = Path.GetFileNameWithoutExtension(imagePath1).Split('-').Last();
            var exclusionAreas = GetAllExclusionAreas(pageName);
            var percentageDifference = ImageComparer.PercentageDifferent(image1, image2, exclusionAreas);

            if (percentageDifference > 0f)
            {
                var diffImage = image1.GetDifferenceImage(image2, exclusionAreas);

                diffImage.Save(Path.Combine(folder, $"DIFF-{Path.GetFileName(fileName2)}"), ImageFormat.Png);

                return false;
            }
            else
            {
                return true;
            }
        }

        private ImageComparer.ExclusionArea[] GetAllExclusionAreas(string pageSpecific = null)
        {
            var hasPageSpecific = !string.IsNullOrWhiteSpace(pageSpecific) && TestAppInfo.PageSpecificExclusions.ContainsKey(pageSpecific);
            // Extra on all pages + app bar title + page specific
            var finalCount = TestAppInfo.ExclusionAreas.Length + 1 + (hasPageSpecific ? 1 : 0);

            var result = new ImageComparer.ExclusionArea[finalCount];

            // We always exclude the area the app name occupies in the title bar as these will always be different
            result[0] = new ImageComparer.ExclusionArea(new Rectangle(0, 0, 600, 40), scaleFactor: 1.25f);

            Array.Copy(TestAppInfo.ExclusionAreas, 0, result, 1, TestAppInfo.ExclusionAreas.Length);

            if (hasPageSpecific)
            {
                result[finalCount - 1] = TestAppInfo.PageSpecificExclusions[pageSpecific];
            }

            return result;
        }
    }
}
