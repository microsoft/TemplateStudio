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
            result.Add("wts.Page.TabView");
            result.Add("wts.Page.TreeView");
            result.Add("wts.Page.TwoPaneView");

            return result.ToArray();
        }

        public static string[] AllPagesThatSupportSimpleTestingOnAllFrameworks()
        {
            return new[]
            {
                "wts.Page.Blank",
                "wts.Page.Chart",
                "wts.Page.ContentGrid",
                "wts.Page.DataGrid",
                "wts.Page.Grid",
                "wts.Page.ImageGallery",
                "wts.Page.InkDraw",
                "wts.Page.InkDrawPicture",
                "wts.Page.InkSmartCanvas",
                "wts.Page.ListDetails",
                "wts.Page.Settings",
                "wts.Page.TabbedPivot",
            };
        }

        public static string[] AllPagesThatRequireExtraLogicForTesting()
        {
            return new[]
            {
                "wts.Page.Camera",
                "wts.Page.WebView",
                "wts.Page.MediaPlayer",
            };
        }

        public static string[] AllPagesNotVisuallyTestable()
        {
            return new[]
            {
                "wts.Page.Map", // Map page cannot be relied on to load the same details on the screen (buildings, road names, etc.) and so cannot use screenshots to compare displayed output
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
