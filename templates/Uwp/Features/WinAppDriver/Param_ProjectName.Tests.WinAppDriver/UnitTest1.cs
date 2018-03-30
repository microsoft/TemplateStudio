using System;
using System.Drawing.Imaging;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium.Appium.Windows;
using OpenQA.Selenium.Remote;

namespace Param_ProjectName.Tests.WinAppDriver
{
    [TestClass]
    public class UnitTest1
    {
        // TODO WTS: install WinAppDriver and start it before running tests: https://github.com/Microsoft/WinAppDriver
        protected const string WindowsApplicationDriverUrl = "http://127.0.0.1:4723";

        // TODO WTS: set the app launch ID
        // The part before "!App" will be in Package.Appxmanifest > Packaging > Package Family Name
        protected const string AppToLaunch = "XXXXXXXX-XXXX-XXXX-XXXX-XXXXXXXXXXXX_XXXXXXXXXXXXX!App";

        protected static WindowsDriver<WindowsElement> AppSession;

        private static string ScreenshotFolder;

        [ClassInitialize]
        public static void Setup(TestContext context)
        {
            // TODO WTS: change the location where screenshots are saved
            // Create separate folders for saving the results of each test run
            ScreenshotFolder = $"{Path.GetPathRoot(Environment.CurrentDirectory)}\\Temp\\Screenshots\\{DateTime.Now.ToString("dd_HHmm")}\\";

            // Make sure the folder exists or saving screenshots will fail
            if (!Directory.Exists(ScreenshotFolder))
            {
                Directory.CreateDirectory(ScreenshotFolder);
            }
        }

        [TestInitialize]
        public void LaunchApp()
        {
            if (AppSession == null)
            {
                DesiredCapabilities appCapabilities = new DesiredCapabilities();
                appCapabilities.SetCapability("app", AppToLaunch);
                appCapabilities.SetCapability("deviceName", "WindowsPC");
                AppSession = new WindowsDriver<WindowsElement>(new Uri(WindowsApplicationDriverUrl), appCapabilities);

                Assert.IsNotNull(AppSession);

                AppSession.Manage().Timeouts().ImplicitlyWait(TimeSpan.FromSeconds(4));

                // Maximize the window to have a consistent size and position
                AppSession.Manage().Window.Maximize();
            }
        }

        // TODO WTS: Add other tests as appropriate
        [TestMethod]
        public void TakeScreenshotOfLaunchPage()
        {
            var screenshotFileName = Path.Combine(ScreenshotFolder, $"{Path.GetRandomFileName()}.png");

            var screenshot = AppSession.GetScreenshot();
            screenshot.SaveAsFile(screenshotFileName, ImageFormat.Png);

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
