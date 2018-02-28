// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using OpenQA.Selenium.Appium.Windows;

namespace AutomatedUITests.Utils
{
    public static class WindowsElementExtensions
    {
        public static bool TryFindElementByName(this WindowsDriver<WindowsElement> session, string name, out WindowsElement element)
        {
            try
            {
                // FindElementByName will throw if it can't find something matching the name
                element = session.FindElementByName("Yes");

                return true;
            }
            catch
            {
                element = null;
                return false;
            }
        }
    }
}
