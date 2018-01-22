// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using OpenQA.Selenium.Appium.Windows;
using OpenQA.Selenium.Remote;

namespace AutomatedUITests.Tests
{
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
