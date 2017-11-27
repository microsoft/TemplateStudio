// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Windows.Media;

namespace Microsoft.Templates.UI.V2Services
{
    public class FakeStyleValuesProvider : IStyleValuesProvider
    {
        private double _baseFontSize = 12;

        public event EventHandler ThemeChanged;

        public Brush GetColor(string className, string memberName)
        {
            if (className == "ThemedDialogColors")
            {
                switch (memberName)
                {
                    case "WindowPanelColorKey":
                        return LightColorValues.Color_FF_FFFFFF;
                    case "WindowBorderColorKey":
                        return LightColorValues.Color_FF_CCCEDB;
                    case "HeaderTextColorKey":
                        return LightColorValues.Color_FF_1E1E1E;
                    case "HyperlinkColorKey":
                        return LightColorValues.Color_FF_0E70C0;
                    case "HyperlinkHoverColorKey":
                        return LightColorValues.Color_FF_007ACC;
                    case "HyperlinkPressedColorKey":
                        return LightColorValues.Color_FF_0E70C0;
                    case "HyperlinkDisabledColorKey":
                        return LightColorValues.Color_FF_A2A4A5;
                    case "SelectedItemActiveColorKey":
                        return LightColorValues.Color_FF_007ACC;
                    case "SelectedItemInactiveColorKey":
                        return LightColorValues.Color_FF_CCCEDB;
                    case "ListItemMouseOverColorKey":
                        return LightColorValues.Color_FF_C9DEF5;
                    case "ListItemDisabledTextColorKey":
                        return LightColorValues.Color_FF_A2A4A5;
                    case "GridHeadingBackgroundColorKey":
                        return LightColorValues.Color_00_000000;
                    case "GridHeadingHoverBackgroundColorKey":
                        return LightColorValues.Color_00_000000;
                    case "GridHeadingTextColorKey":
                        return LightColorValues.Color_FF_717171;
                    case "GridHeadingHoverTextColorKey":
                        return LightColorValues.Color_FF_1E1E1E;
                    case "GridLineColorKey":
                        return LightColorValues.Color_FF_CCCEDB;
                    case "SectionDividerColorKey":
                        return LightColorValues.Color_FF_CCCEDB;
                    case "WindowButtonColorKey":
                        return LightColorValues.Color_00_000000;
                    case "WindowButtonHoverColorKey":
                        return LightColorValues.Color_FF_C9DEF5;
                    case "WindowButtonDownColorKey":
                        return LightColorValues.Color_FF_007ACC;
                    case "WindowButtonBorderColorKey":
                        return LightColorValues.Color_00_000000;
                    case "WindowButtonHoverBorderColorKey":
                        return LightColorValues.Color_FF_C9DEF5;
                    case "WindowButtonDownBorderColorKey":
                        return LightColorValues.Color_FF_007ACC;
                    case "WindowButtonGlyphColorKey":
                        return LightColorValues.Color_FF_1E1E1E;
                    case "WindowButtonHoverGlyphColorKey":
                        return LightColorValues.Color_FF_1E1E1E;
                    case "WindowButtonDownGlyphColorKey":
                        return LightColorValues.Color_FF_FFFFFF;
                    case "WizardFooterColorKey":
                        return LightColorValues.Color_FF_EFEFF2;
                    case "HeaderTextSecondaryColorKey":
                        return LightColorValues.Color_FF_828282;
                    case "WizardFooterTextColorKey":
                        return LightColorValues.Color_FF_828282;
                    default:
                        throw new Exception($"The color key value '{memberName}' is not found");
                }
            }
            else if (className == "CommonDocumentColors")
            {
                switch (memberName)
                {
                    case "ListItemTextColorKey":
                        return LightColorValues.Color_FF_1E1E1E;
                    case "ListItemTextDisabledColorKey":
                        return LightColorValues.Color_FF_A2A4A5;
                    default:
                        throw new Exception($"The color key value '{memberName}' is not found");
                }
            }
            else if (className == "CommonControlColors")
            {
                switch (memberName)
                {
                    case "ButtonColorKey":
                        return LightColorValues.Color_FF_ECECF0;
                    case "ButtonTextColorKey":
                        return LightColorValues.Color_FF_1E1E1E;
                    case "ButtonBorderColorKey":
                        return LightColorValues.Color_FF_ACACAC;

                    case "ButtonDefaultColorKey":
                        return LightColorValues.Color_FF_ECECF0;
                    case "ButtonDefaultTextColorKey":
                        return LightColorValues.Color_FF_1E1E1E;
                    case "ButtonBorderDefaultColorKey":
                        return LightColorValues.Color_FF_3399FF;

                    case "ButtonDisabledColorKey":
                        return LightColorValues.Color_FF_F5F5F5;
                    case "ButtonDisabledTextColorKey":
                        return LightColorValues.Color_FF_A2A4A5;
                    case "ButtonBorderDisabledColorKey":
                        return LightColorValues.Color_FF_CCCEDB;

                    case "ButtonFocusedColorKey":
                        return LightColorValues.Color_FF_C9DEF5;
                    case "ButtonFocusedTextColorKey":
                        return LightColorValues.Color_FF_1E1E1E;
                    case "ButtonBorderFocusedColorKey":
                        return LightColorValues.Color_FF_3399FF;

                    case "ButtonHoverColorKey":
                        return LightColorValues.Color_FF_C9DEF5;
                    case "ButtonHoverTextColorKey":
                        return LightColorValues.Color_FF_1E1E1E;
                    case "ButtonBorderHoverColorKey":
                        return LightColorValues.Color_FF_3399FF;

                    case "ButtonPressedColorKey":
                        return LightColorValues.Color_FF_007ACC;
                    case "ButtonPressedTextColorKey":
                        return LightColorValues.Color_FF_FFFFFF;
                    case "ButtonBorderPressedColorKey":
                        return LightColorValues.Color_FF_007ACC;

                    default:
                        throw new Exception($"The color key value '{memberName}' is not found");
                }
            }
            else if (className == "ThemedCardColors")
            {
                switch (memberName)
                {
                    case "CardTitleTextColorKey":
                        return LightColorValues.Color_FF_1E1E1E;
                    case "CardDescriptionTextColorKey":
                        return LightColorValues.Color_FF_717171;
                    case "CardBackgroundDefaultColorKey":
                        return LightColorValues.Color_FF_FFFFFF;
                    case "CardBackgroundFocusColorKey":
                        return LightColorValues.Color_FF_FFFFFF;
                    case "CardBackgroundHoverColorKey":
                        return LightColorValues.Color_FF_EFEFF2;
                    case "CardBackgroundPressedColorKey":
                        return LightColorValues.Color_FF_EFEFF2;
                    case "CardBackgroundSelectedColorKey":
                        return LightColorValues.Color_FF_FFFFFF;
                    case "CardBackgroundDisabledColorKey":
                        return LightColorValues.Color_FF_F5F5F5;
                    case "CardBorderDefaultColorKey":
                        return LightColorValues.Color_FF_BFBFBF;
                    case "CardBorderFocusColorKey":
                        return LightColorValues.Color_FF_3399FF;
                    case "CardBorderHoverColorKey":
                        return LightColorValues.Color_FF_9A9A9A;
                    case "CardBorderPressedColorKey":
                        return LightColorValues.Color_FF_007ACC;
                    case "CardBorderSelectedColorKey":
                        return LightColorValues.Color_FF_007ACC;
                    case "CardBorderDisabledColorKey":
                        return LightColorValues.Color_FF_CCCEDB;
                    default:
                        throw new Exception($"The color key value '{memberName}' is not found");
                }
            }
            else
            {
                throw new Exception($"Class name not recognized '{className}'");
            }
        }

        public double GetFontSize(string fontSizeResourceKey)
        {
            switch (fontSizeResourceKey)
            {
                case "Environment100PercentFontSize":
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

        public FontFamily GetFontFamily() => new FontFamily("Segoe UI");

        private void OnThemeChanged()
        {
            ThemeChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}
