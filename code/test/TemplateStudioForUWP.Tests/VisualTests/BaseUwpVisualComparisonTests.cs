// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Templates.Test.Build;
using OpenQA.Selenium.Appium.Windows;

namespace Microsoft.Templates.Test.UWP.Build
{
    public class BaseUwpVisualComparisonTests : BaseVisualComparisonTests
    {
        public BaseUwpVisualComparisonTests(UwpGenTemplatesTestFixture fixture)
        : base(fixture)
        {
        }

        public static string[] AllTestablePages(string framework)
        {
            var result = new List<string>();

            if (framework == Frameworks.Prism)
            {
                result.AddRange(AllPagesThatSupportSimpleTestingOnAllFrameworks());
            }
            else
            {
                result.AddRange(AllPagesThatSupportSimpleTesting());
            }

            result.AddRange(AllPagesThatRequireExtraLogicForTesting());
            result.AddRange(AllPagesNotVisuallyTestable());

            return result.ToArray();
        }

        public static string[] AllVisuallyTestablePages()
        {
            var result = new List<string>();

            result.AddRange(AllPagesThatSupportSimpleTesting());
            result.AddRange(AllPagesThatRequireExtraLogicForTesting());

            return result.ToArray();
        }

        public static string[] AllPagesThatSupportSimpleTesting()
        {
            var result = new List<string>();

            result.AddRange(AllPagesThatSupportSimpleTestingOnAllFrameworks());
            result.Add("ts.Page.TabView");
            result.Add("ts.Page.TreeView");
            result.Add("ts.Page.TwoPaneView");

            return result.ToArray();
        }

        public static string[] AllPagesThatSupportSimpleTestingOnAllFrameworks()
        {
            return new[]
            {
                "ts.Page.Blank",
                "ts.Page.Chart",
                "ts.Page.ContentGrid",
                "ts.Page.DataGrid",
                "ts.Page.Grid",
                "ts.Page.ImageGallery",
                "ts.Page.InkDraw",
                "ts.Page.InkDrawPicture",
                "ts.Page.InkSmartCanvas",
                "ts.Page.ListDetails",
                "ts.Page.Settings",
                "ts.Page.TabbedPivot",
            };
        }

        public static string[] AllPagesThatRequireExtraLogicForTesting()
        {
            return new[]
            {
                "ts.Page.Camera",
                "ts.Page.WebView",
                "ts.Page.MediaPlayer",
            };
        }

        public static string[] AllPagesNotVisuallyTestable()
        {
            return new[]
            {
                "ts.Page.Map", // Map page cannot be relied on to load the same details on the screen (buildings, road names, etc.) and so cannot use screenshots to compare displayed output
            };
        }

        protected async Task<bool> ClickYesOnPopUpAsync(WindowsDriver<WindowsElement> session)
        {
            await Task.Delay(TimeSpan.FromSeconds(1)); // Allow extra time for popup to be displayed

            try
            {
                var popups = session.FindElementsByAccessibilityId("Popup Window");

                if (popups.Count == 1)
                {
                    var yes = popups[0].FindElementsByName("Yes");

                    if (yes.Count == 1)
                    {
                        yes[0].Click();
                    }
                }
                else
                {
                    // No pop-up was shown so assume this is ok.
                }

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
