using System;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Appium;
using OpenQA.Selenium.Appium.Windows;

namespace Param_RootNamespace.Tests.WinAppDriver
{
    [TestClass]
    public class BasicTests
    {
        // TODO WTS: install WinAppDriver and start it before running tests: https://github.com/Microsoft/WinAppDriver
        protected const string WindowsApplicationDriverUrl = "http://127.0.0.1:4723";

        // TODO WTS: set the app launch ID.
        // The part before "!App" will be in Package.Appxmanifest > Packaging > Package Family Name.
        // The app must also be installed (or launched for debugging) for WinAppDriver to be able to launch it.
        protected const string AppToLaunch = "XXXXXXXX-XXXX-XXXX-XXXX-XXXXXXXXXXXX_XXXXXXXXXXXXX!App";

        protected static WindowsDriver<WindowsElement> AppSession { get; set; }

        private static string _screenshotFolder;

        [ClassInitialize]
        public static void Setup(TestContext context)
        {
            // TODO WTS: change the location where screenshots are saved.
            // Create separate folders for saving the results of each test run.
            _screenshotFolder = $"{Path.GetPathRoot(Environment.CurrentDirectory)}\\Temp\\Screenshots\\{DateTime.Now.ToString("dd_HHmm")}\\";

            // Make sure the folder exists or saving screenshots will fail.
            if (!Directory.Exists(_screenshotFolder))
            {
                Directory.CreateDirectory(_screenshotFolder);
            }
        }

        [TestInitialize]
        public void LaunchApp()
        {
            if (AppSession == null)
            {
                var appiumOptions = new AppiumOptions();
                appiumOptions.AddAdditionalCapability("app", AppToLaunch);
                appiumOptions.AddAdditionalCapability("deviceName", "WindowsPC");
                AppSession = new WindowsDriver<WindowsElement>(new Uri(WindowsApplicationDriverUrl), appiumOptions);

                Assert.IsNotNull(AppSession, "Unable to launch app.");

                AppSession.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(4);

                // Maximize the window to have a consistent size and position.
                AppSession.Manage().Window.Maximize();
            }
        }

        // TODO WTS: Add other tests as appropriate.
        [TestMethod]
        public void TakeScreenshotOfLaunchPage()
        {
            var screenshotFileName = Path.Combine(_screenshotFolder, $"{Path.GetRandomFileName()}.png");

            var screenshot = AppSession.GetScreenshot();
            screenshot.SaveAsFile(screenshotFileName, ScreenshotImageFormat.Png);

            Assert.IsTrue(File.Exists(screenshotFileName));
        }

        [TestCleanup]
        public void TearDown()
        {
            if (AppSession != null)
            {
                AppSession.Dispose();
                AppSession = null;
            }
        }
    }
}
