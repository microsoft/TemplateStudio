// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Windows.Media;

using Microsoft.Templates.UI.Extensions;

namespace Microsoft.Templates.UI.V2Services
{
    public class FakeStyleValuesProvider : IStyleValuesProvider
    {
        private static Brush _color_FFFFFF = "FFFFFF".AsBrush();
        private static Brush _color_CCCEDB = "CCCEDB".AsBrush();
        private static Brush _color_1E1E1E = "1E1E1E".AsBrush();
        private static Brush _color_0E70C0 = "0E70C0".AsBrush();
        private static Brush _color_007ACC = "007ACC".AsBrush();
        private static Brush _color_A2A4A5 = "A2A4A5".AsBrush();
        private static Brush _color_C9DEF5 = "C9DEF5".AsBrush();
        private static Brush _color_000000 = "000000".AsBrush();
        private static Brush _color_EFEFF2 = "EFEFF2".AsBrush();
        private static Brush _color_717171 = "717171".AsBrush();

        private double _baseFontSize = 12;

        public Brush GetColor(string className, string memberName)
        {
            if (memberName == "WindowPanelColorKey")
            {
                return _color_FFFFFF;
            }
            else if (memberName == "WindowBorderColorKey")
            {
                return _color_CCCEDB;
            }
            else if (memberName == "HeaderTextColorKey")
            {
                return _color_1E1E1E;
            }
            else if (memberName == "HyperlinkColorKey")
            {
                return _color_0E70C0;
            }
            else if (memberName == "HyperlinkHoverColorKey")
            {
                return _color_007ACC;
            }
            else if (memberName == "HyperlinkPressedColorKey")
            {
                return _color_0E70C0;
            }
            else if (memberName == "HyperlinkDisabledColorKey")
            {
                return _color_A2A4A5;
            }
            else if (memberName == "SelectedItemActiveColorKey")
            {
                return _color_007ACC;
            }
            else if (memberName == "SelectedItemInactiveColorKey")
            {
                return _color_CCCEDB;
            }
            else if (memberName == "ListItemMouseOverColorKey")
            {
                return _color_C9DEF5;
            }
            else if (memberName == "ListItemDisabledTextColorKey")
            {
                return _color_A2A4A5;
            }
            else if (memberName == "GridHeadingBackgroundColorKey")
            {
                return _color_000000;
            }
            else if (memberName == "GridHeadingHoverBackgroundColorKey")
            {
                return _color_000000;
            }
            else if (memberName == "GridHeadingTextColorKey")
            {
                return _color_717171;
            }
            else if (memberName == "GridHeadingHoverTextColorKey")
            {
                return _color_1E1E1E;
            }
            else if (memberName == "GridLineColorKey")
            {
                return _color_CCCEDB;
            }
            else if (memberName == "SectionDividerColorKey")
            {
                return _color_CCCEDB;
            }
            else if (memberName == "WindowButtonColorKey")
            {
                return _color_000000;
            }
            else if (memberName == "WindowButtonHoverColorKey")
            {
                return _color_C9DEF5;
            }
            else if (memberName == "WindowButtonDownColorKey")
            {
                return _color_007ACC;
            }
            else if (memberName == "WindowButtonBorderColorKey")
            {
                return _color_000000;
            }
            else if (memberName == "WindowButtonHoverBorderColorKey")
            {
                return _color_C9DEF5;
            }
            else if (memberName == "WindowButtonDownBorderColorKey")
            {
                return _color_007ACC;
            }
            else if (memberName == "WindowButtonGlyphColorKey")
            {
                return _color_1E1E1E;
            }
            else if (memberName == "WindowButtonHoverGlyphColorKey")
            {
                return _color_1E1E1E;
            }
            else if (memberName == "WindowButtonDownGlyphColorKey")
            {
                return _color_FFFFFF;
            }
            else if (memberName == "WizardFooterColorKey")
            {
                return _color_EFEFF2;
            }
            else
            {
                throw new Exception($"The Ccolor key value '{memberName}' is not found");
            }
        }

        public double GetFontSize(string fontSizeResourceKey)
        {
            switch (fontSizeResourceKey)
            {
                case "EnvironmentFontSize":
                    return _baseFontSize * 1.0;
                case "Environment111PercentFontSize":
                    return _baseFontSize * 1.11;
                case "Environment122PercentFontSize":
                    return _baseFontSize * 1.22;
                case "Environment133PercentFontSize":
                    return _baseFontSize * 1.33;
                case "Environment155PercentFontSize":
                    return _baseFontSize * 1.55;
                case "Environment200PercentFontSize":
                    return _baseFontSize * 2.0;
                case "Environment310PercentFontSize":
                    return _baseFontSize * 3.1;
                case "Environment330PercentFontSize":
                    return _baseFontSize * 3.3;
                case "Environment375PercentFontSize":
                    return _baseFontSize * 3.75;
                case "Environment90PercentFontSize":
                    return _baseFontSize * 0.9;
                default:
                    throw new Exception($"The font size value '{fontSizeResourceKey}' is not found");
            }
        }
    }
}
