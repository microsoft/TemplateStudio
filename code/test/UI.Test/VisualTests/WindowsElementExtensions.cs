// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections.ObjectModel;
using OpenQA.Selenium;
using OpenQA.Selenium.Appium.Windows;

namespace Microsoft.UI.Test.VisualTests
{
    public static class WindowsElementExtensions
    {
        public static bool TryFindElementByName(this WindowsDriver<WindowsElement> session, string name, out WindowsElement element)
        {
            try
            {
                // FindElementByName will throw if it can't find something matching the name
                element = session.FindElementByName(name);

                return true;
            }
            catch
            {
                element = null;
                return false;
            }
        }

        public static bool TryFindElementByAutomationId(this WindowsDriver<WindowsElement> session, string className, string automationId, out WindowsElement element)
        {
            try
            {
                // Can't get FindElementByWindowsUIAutomation to work with WPF app so this is a workaround
                foreach (var windowsElement in session.FindElements(By.ClassName(className)))
                {
                    var possibleElement = session.FindElement(By.Id(windowsElement.Id));
                    if (possibleElement.GetAttribute("AutomationId") == automationId)
                    {
                        element = possibleElement;
                        return true;
                    }
                }

                element = null;
                return false;
            }
            catch
            {
                element = null;
                return false;
            }
        }

        public static bool TryFindElementsByClassName(this WindowsDriver<WindowsElement> session, string className, out ReadOnlyCollection<WindowsElement> elements)
        {
            try
            {
                elements = session.FindElements(By.ClassName(className));
                return true;
            }
            catch
            {
                elements = null;
                return false;
            }
        }

        public static bool TryFindElementByClassNameAndText(this WindowsDriver<WindowsElement> session, string className, string text, out WindowsElement element)
        {
            try
            {
                // Can't get FindElementByWindowsUIAutomation to work with WPF app so this is a workaround
                foreach (var windowsElement in session.FindElements(By.ClassName(className)))
                {
                    var possibleElement = session.FindElement(By.Id(windowsElement.Id));

                    if (possibleElement.Text == text)
                    {
                        element = possibleElement;
                        return true;
                    }
                    else
                    {
                        foreach (var textblock in possibleElement.FindElements(By.ClassName("TextBlock")))
                        {
                            if (textblock.GetAttribute("Name") == text)
                            {
                                element = possibleElement;
                                return true;
                            }
                        }
                    }
                }

                element = null;
                return false;
            }
            catch
            {
                element = null;
                return false;
            }
        }
    }
}
