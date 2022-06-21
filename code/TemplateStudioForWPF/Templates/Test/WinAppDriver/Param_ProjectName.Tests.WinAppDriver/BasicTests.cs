﻿using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Appium;
using OpenQA.Selenium.Appium.Windows;

namespace Param_RootNamespace.Tests.WinAppDriver;

[TestClass]
public class BasicTests
{
    // TODO: install WinAppDriver and start it before running tests: https://github.com/Microsoft/WinAppDriver
    protected const string WindowsApplicationDriverUrl = "http://127.0.0.1:4723";

    // TODO: set the app launch ID.
    // The part before "!App" will be in Package.Appxmanifest > Packaging > Package Family Name.
    // The app must also be installed (or launched for debugging) for WinAppDriver to be able to launch it.
    // If your project doesn't contain MSIX Packaging set AppToLaunch .exe path.
    private const string AppToLaunch = "XXXXXXXX-XXXX-XXXX-XXXX-XXXXXXXXXXXX_XXXXXXXXXXXXX!App";

    private static WindowsDriver<WindowsElement> AppSession { get; set; }

    private static string _screenshotFolder;
    private static string _screenshotFilePath;

    [ClassInitialize]
    public static void Setup(TestContext context)
    {
        // TODO: change the location where screenshots are saved.
        // Create separate folders for saving the results of each test run.
        _screenshotFolder = $"{Path.GetPathRoot(Environment.CurrentDirectory)}\\Temp\\Screenshots\\{DateTime.Now:dd_HHmm}\\";
        _screenshotFilePath = Path.Combine(_screenshotFolder, $"{Path.GetRandomFileName()}.png");

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
            try
            {
                AppSession = new WindowsDriver<WindowsElement>(new Uri(WindowsApplicationDriverUrl), appiumOptions);
            }
            catch
            {
                Console.WriteLine("Failed to attach to app session (expected).");
            }

            // If your project doesn't contain MSIX Packaging you can remove the following code block
            // More info about testing a packaged application: https://techcommunity.microsoft.com/t5/windows-dev-appconsult/ui-testing-for-windows-apps-with-winappdriver-and-appium/ba-p/825352
            if (AppSession == null)
            {
                // Try get session using NativeWindowHandle
                appiumOptions.AddAdditionalCapability("app", "Root");
                AppSession = new WindowsDriver<WindowsElement>(new Uri(WindowsApplicationDriverUrl), appiumOptions);

                // Get main window by Accessibility Id
                var mainWindow = AppSession.FindElementByAccessibilityId("Param_RootNamespaceMainWindow");
                var mainWindowHandle = mainWindow.GetAttribute("NativeWindowHandle");
                mainWindowHandle = int.Parse(mainWindowHandle).ToString("x"); // Convert to Hex
                appiumOptions = new AppiumOptions();
                appiumOptions.AddAdditionalCapability("appTopLevelWindow", mainWindowHandle);
                AppSession = new WindowsDriver<WindowsElement>(new Uri(WindowsApplicationDriverUrl), appiumOptions);
            }

            Assert.IsNotNull(AppSession, "Unable to launch app.");

            AppSession.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(4);

            // Maximize the window to have a consistent size and position.
            AppSession.Manage().Window.Maximize();
        }
    }

    // TODO: Add other tests as appropriate.
    [TestMethod]
    public void TakeScreenshotOfLaunchPage()
    {
        var screenshot = AppSession.GetScreenshot();
        screenshot.SaveAsFile(_screenshotFilePath, ScreenshotImageFormat.Png);

        Assert.IsTrue(File.Exists(_screenshotFilePath));

        File.Delete(_screenshotFilePath);
    }

    [TestCleanup]
    public void TearDown()
    {
        if (AppSession != null)
        {
            AppSession.CloseApp();
            AppSession.Dispose();
            AppSession = null;
        }
    }
}
