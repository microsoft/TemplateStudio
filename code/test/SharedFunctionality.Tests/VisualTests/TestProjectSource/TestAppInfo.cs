// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;
using System.Drawing;
using WindowsTestHelpers;

namespace AutomatedUITests
{
    internal class TestAppInfo
    {
        public const string AppPfn1 = "***APP-PFN-1-GOES-HERE***";
        public const string AppPfn2 = "***APP-PFN-2-GOES-HERE***";
        public const string AppName1 = "***APP-NAME-1-GOES-HERE***";
        public const string AppName2 = "***APP-NAME-2-GOES-HERE***";
        public const string ScreenshotsFolder = @"***FOLDER-GOES-HERE***";
        public const int NoClickCount = 0;
        public const bool LongPauseAfterLaunch = false;

        public static ImageComparer.ExclusionArea[] ExclusionAreas => new ImageComparer.ExclusionArea[0];

        public static Dictionary<string, ImageComparer.ExclusionArea> PageSpecificExclusions = new Dictionary<string, ImageComparer.ExclusionArea>();
    }
}
