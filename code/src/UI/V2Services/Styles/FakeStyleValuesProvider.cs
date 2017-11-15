// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Windows.Media;

using Microsoft.Templates.UI.Extensions;
using Microsoft.VisualStudio.PlatformUI;
using Microsoft.VisualStudio.Shell;

namespace Microsoft.Templates.UI.V2Services
{
    public class FakeStyleValuesProvider : IStyleValuesProvider
    {
        public Brush GetColor(ThemeResourceKey themeResourceKey)
        {
            if (themeResourceKey == EnvironmentColors.EnvironmentBackgroundColorKey)
            {
                return "#FABADA".AsBrush();
            }
            else
            {
                return new SolidColorBrush(Colors.Red);
            }
        }

        public double GetFontSize(string fontSizeResourceKey)
        {
            switch (fontSizeResourceKey)
            {
                case "EnvironmentFontSize":
                    return GetScaledFontSize(1.0);
                case "Environment111PercentFontSize":
                    return GetScaledFontSize(1.11);
                case "Environment122PercentFontSize":
                    return GetScaledFontSize(1.22);
                case "Environment133PercentFontSize":
                    return GetScaledFontSize(1.33);
                case "Environment155PercentFontSize":
                    return GetScaledFontSize(1.55);
                case "Environment200PercentFontSize":
                    return GetScaledFontSize(2.0);
                case "Environment310PercentFontSize":
                    return GetScaledFontSize(3.1);
                case "Environment330PercentFontSize":
                    return GetScaledFontSize(3.3);
                case "Environment375PercentFontSize":
                    return GetScaledFontSize(3.75);
                case "Environment90PercentFontSize":
                    return GetScaledFontSize(0.9);
                default:
                    return 12;
            }
        }

        private double GetScaledFontSize(double percent)
        {
            return 12 * percent;
        }
    }
}
