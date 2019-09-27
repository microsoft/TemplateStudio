Imports System.IO
Imports Microsoft.VisualStudio.TestTools.UnitTesting
Imports OpenQA.Selenium
Imports OpenQA.Selenium.Appium
Imports OpenQA.Selenium.Appium.Windows
Imports OpenQA.Selenium.Remote

<TestClass>
Public Class BasicTests

    ' TODO WTS: install WinAppDriver and start it before running tests: https://github.com/Microsoft/WinAppDriver
    Protected Const WindowsApplicationDriverUrl As String = "http://127.0.0.1:4723"

    ' TODO WTS: set the app launch ID.
    ' The part before "!App" will be in Package.Appxmanifest > Packaging > Package Family Name.
    ' The app must also be installed (or launched for debugging) for WinAppDriver to be able to launch it.
    Protected Const AppToLaunch As String = "XXXXXXXX-XXXX-XXXX-XXXX-XXXXXXXXXXXX_XXXXXXXXXXXXX!App"

    Protected Shared Property AppSession As WindowsDriver(Of WindowsElement)

    Private Shared _screenshotFolder As String

    <ClassInitialize>
    Public Shared Sub Setup(context As TestContext)
        ' TODO WTS: change the location where screenshots are saved.
        ' Create separate folders for saving the results of each test run.
        _screenshotFolder = $"{Path.GetPathRoot(Environment.CurrentDirectory)}\\Temp\\Screenshots\\{DateTime.Now.ToString("dd_HHmm")}\\"

        ' Make sure the folder exists or saving screenshots will fail
        If Not Directory.Exists(_screenshotFolder) Then
            Directory.CreateDirectory(_screenshotFolder)
        End If
    End Sub

    <TestInitialize>
    Public Sub LaunchApp()
        If AppSession Is Nothing Then
            Dim appiumOptions = New AppiumOptions()
            appiumOptions.AddAdditionalCapability("app", AppToLaunch)
            appiumOptions.AddAdditionalCapability("deviceName", "WindowsPC")
            AppSession = New WindowsDriver(Of WindowsElement)(New Uri(WindowsApplicationDriverUrl), appiumOptions)

            Assert.IsNotNull(AppSession, "Unable to launch app.")

            AppSession.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(4)

            ' Maximize the window to have a consistent size and position.
            AppSession.Manage().Window.Maximize()
        End If
    End Sub

    ' TODO WTS: Add other tests as appropriate.
    <TestMethod>
    Public Sub TakeScreenshotOfLaunchPage()
        Dim screenshotFileName = Path.Combine(_screenshotFolder, $"{Path.GetRandomFileName()}.png")
        Dim screenshot = AppSession.GetScreenshot()
        screenshot.SaveAsFile(screenshotFileName, ScreenshotImageFormat.Png)
        Assert.IsTrue(File.Exists(screenshotFileName))
    End Sub

    <TestCleanup>
    Public Sub TearDown()
        If AppSession IsNot Nothing Then
            AppSession.Dispose()
            AppSession = Nothing
        End If
    End Sub
End Class
