// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Threading.Tasks;
using wts.rootNamespace;
using wts.rootNamespace.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium.Appium.Windows;
using OpenQA.Selenium.Remote;

namespace wts.rootNamespace.Tests
{
    [TestClass]
    public class LaunchBothAppsAndCompareInitialScreenshots : TestBase
    {
        [TestMethod]
        public void CompareInitialScreenshots()
        {
            var ScreenshotFolder = $"{Path.GetPathRoot(Environment.CurrentDirectory)}\\UIT\\{DateTime.Now.ToString("dd_HHmm")}\\Screenshots\\";

            if (!Directory.Exists(ScreenshotFolder))
            {
                Directory.CreateDirectory(ScreenshotFolder);
            }

            using (var appSession1 = GetAppSession(AppIds.AppPfn1))
            {
                appSession1.Manage().Window.Maximize();
                Task.Delay(TimeSpan.FromSeconds(2)).Wait();

                var screenshot = appSession1.GetScreenshot();
                screenshot.SaveAsFile(Path.Combine(ScreenshotFolder, "app1.png"), ImageFormat.Png);
            }

            using (var appSession2 = GetAppSession(AppIds.AppPfn2))
            {
                appSession2.Manage().Window.Maximize();
                Task.Delay(TimeSpan.FromSeconds(2)).Wait();

                var screenshot = appSession2.GetScreenshot();
                screenshot.SaveAsFile(Path.Combine(ScreenshotFolder, "app2.png"), ImageFormat.Png);
            }

            var imageCompareResult = CheckImagesAreTheSame(ScreenshotFolder, "app1.png", "app2.png");

            Assert.IsTrue(imageCompareResult);
        }

        private bool CheckImagesAreTheSame(string folder, string fileName1, string fileName2)
        {
            var imagePath1 = Path.Combine(folder, fileName1);
            var imagePath2 = Path.Combine(folder, fileName2);

            var image1 = Image.FromFile(imagePath1);
            var image2 = Image.FromFile(imagePath2);

            var percentageDifference = image1.Differences(image2);

            if (percentageDifference > 0f)
            {
                var diffImage = image1.GetDifferenceImage(image2);

                diffImage.Save(Path.Combine(folder, "diff.png"), ImageFormat.Png);

                return false;
            }
            else
            {
                return true;
            }
        }
    }

    public class TestBase
    {
        protected const string WindowsApplicationDriverUrl = "http://127.0.0.1:4723";

        internal WindowsDriver<WindowsElement> GetAppSession(string appPfn)
        {
            DesiredCapabilities appCapabilities = new DesiredCapabilities();
            appCapabilities.SetCapability("app", appPfn);
            appCapabilities.SetCapability("deviceName", "WindowsPC");

            var appSession = new WindowsDriver<WindowsElement>(new Uri(WindowsApplicationDriverUrl), appCapabilities);
            appSession.Manage().Timeouts().ImplicitlyWait(TimeSpan.FromSeconds(4));

            return appSession;
        }
    }
}
