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
            switch (className)
            {
                case "ThemedDialogColors":
                    return GetThemedDialogColor(memberName);
                case "CommonDocumentColors":
                    return GetCommonDocumentColor(memberName);
                case "CommonControlColors":
                    return GetCommonControlColor(memberName);
                case "ThemedCardColors":
                    return GetThemedCardColor(memberName);
                default:
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

        private Brush GetThemedDialogColor(string memberName)
        {
            switch (memberName)
            {
                case "WindowPanelColorKey":
                    return LightColorValues.Color_FFFFFFFF;
                case "WindowBorderColorKey":
                    return LightColorValues.Color_FFCCCEDB;
                case "HeaderTextColorKey":
                    return LightColorValues.Color_FF1E1E1E;
                case "HyperlinkColorKey":
                    return LightColorValues.Color_FF0E70C0;
                case "HyperlinkHoverColorKey":
                    return LightColorValues.Color_FF007ACC;
                case "HyperlinkPressedColorKey":
                    return LightColorValues.Color_FF0E70C0;
                case "HyperlinkDisabledColorKey":
                    return LightColorValues.Color_FFA2A4A5;
                case "SelectedItemActiveColorKey":
                    return LightColorValues.Color_FF007ACC;
                case "SelectedItemInactiveColorKey":
                    return LightColorValues.Color_FFCCCEDB;
                case "ListItemMouseOverColorKey":
                    return LightColorValues.Color_FFC9DEF5;
                case "ListItemDisabledTextColorKey":
                    return LightColorValues.Color_FFA2A4A5;
                case "GridHeadingBackgroundColorKey":
                    return LightColorValues.Color_00000000;
                case "GridHeadingHoverBackgroundColorKey":
                    return LightColorValues.Color_00000000;
                case "GridHeadingTextColorKey":
                    return LightColorValues.Color_FF717171;
                case "GridHeadingHoverTextColorKey":
                    return LightColorValues.Color_FF1E1E1E;
                case "GridLineColorKey":
                    return LightColorValues.Color_FFCCCEDB;
                case "SectionDividerColorKey":
                    return LightColorValues.Color_FFCCCEDB;
                case "WindowButtonColorKey":
                    return LightColorValues.Color_00000000;
                case "WindowButtonHoverColorKey":
                    return LightColorValues.Color_FFC9DEF5;
                case "WindowButtonDownColorKey":
                    return LightColorValues.Color_FF007ACC;
                case "WindowButtonBorderColorKey":
                    return LightColorValues.Color_00000000;
                case "WindowButtonHoverBorderColorKey":
                    return LightColorValues.Color_FFC9DEF5;
                case "WindowButtonDownBorderColorKey":
                    return LightColorValues.Color_FF007ACC;
                case "WindowButtonGlyphColorKey":
                    return LightColorValues.Color_FF1E1E1E;
                case "WindowButtonHoverGlyphColorKey":
                    return LightColorValues.Color_FF1E1E1E;
                case "WindowButtonDownGlyphColorKey":
                    return LightColorValues.Color_FFFFFFFF;
                case "WizardFooterColorKey":
                    return LightColorValues.Color_FFEFEFF2;
                case "HeaderTextSecondaryColorKey":
                    return LightColorValues.Color_FF828282;
                case "WizardFooterTextColorKey":
                    return LightColorValues.Color_FF828282;
                default:
                    throw new Exception($"The color key value '{memberName}' is not found");
            }
        }

        private Brush GetCommonDocumentColor(string memberName)
        {
            switch (memberName)
            {
                case "ListItemTextColorKey":
                    return LightColorValues.Color_FF1E1E1E;
                case "ListItemTextDisabledColorKey":
                    return LightColorValues.Color_FFA2A4A5;
                default:
                    throw new Exception($"The color key value '{memberName}' is not found");
            }
        }

        private Brush GetCommonControlColor(string memberName)
        {
            switch (memberName)
            {
                case "ButtonColorKey":
                    return LightColorValues.Color_FFECECF0;
                case "ButtonTextColorKey":
                    return LightColorValues.Color_FF1E1E1E;
                case "ButtonBorderColorKey":
                    return LightColorValues.Color_FFACACAC;

                case "ButtonDefaultColorKey":
                    return LightColorValues.Color_FFECECF0;
                case "ButtonDefaultTextColorKey":
                    return LightColorValues.Color_FF1E1E1E;
                case "ButtonBorderDefaultColorKey":
                    return LightColorValues.Color_FF3399FF;

                case "ButtonDisabledColorKey":
                    return LightColorValues.Color_FFF5F5F5;
                case "ButtonDisabledTextColorKey":
                    return LightColorValues.Color_FFA2A4A5;
                case "ButtonBorderDisabledColorKey":
                    return LightColorValues.Color_FFCCCEDB;

                case "ButtonFocusedColorKey":
                    return LightColorValues.Color_FFC9DEF5;
                case "ButtonFocusedTextColorKey":
                    return LightColorValues.Color_FF1E1E1E;
                case "ButtonBorderFocusedColorKey":
                    return LightColorValues.Color_FF3399FF;

                case "ButtonHoverColorKey":
                    return LightColorValues.Color_FFC9DEF5;
                case "ButtonHoverTextColorKey":
                    return LightColorValues.Color_FF1E1E1E;
                case "ButtonBorderHoverColorKey":
                    return LightColorValues.Color_FF3399FF;

                case "ButtonPressedColorKey":
                    return LightColorValues.Color_FF007ACC;
                case "ButtonPressedTextColorKey":
                    return LightColorValues.Color_FFFFFFFF;
                case "ButtonBorderPressedColorKey":
                    return LightColorValues.Color_FF007ACC;

                case "ComboBoxBackgroundColorKey":
                    return LightColorValues.Color_FFFFFFFF;
                case "ComboBoxBackgroundDisabledColorKey":
                    return LightColorValues.Color_FFEEEEF2;
                case "ComboBoxBackgroundFocusedColorKey":
                    return LightColorValues.Color_FFFFFFFF;
                case "ComboBoxBackgroundHoverColorKey":
                    return LightColorValues.Color_FFFFFFFF;
                case "ComboBoxBackgroundPressedColorKey":
                    return LightColorValues.Color_FFFFFFFF;

                case "ComboBoxBorderColorKey":
                    return LightColorValues.Color_FFCCCEDB;
                case "ComboBoxBorderDisabledColorKey":
                    return LightColorValues.Color_FFCCCEDB;
                case "ComboBoxBorderFocusedColorKey":
                    return LightColorValues.Color_FF007ACC;
                case "ComboBoxBorderHoverColorKey":
                    return LightColorValues.Color_FF007ACC;
                case "ComboBoxBorderPressedColorKey":
                    return LightColorValues.Color_FF007ACC;

                case "ComboBoxGlyphColorKey":
                    return LightColorValues.Color_FF717171;
                case "ComboBoxGlyphBackgroundColorKey":
                    return LightColorValues.Color_FFFFFFFF;
                case "ComboBoxGlyphBackgroundDisabledColorKey":
                    return LightColorValues.Color_FFEEEEF2;
                case "ComboBoxGlyphBackgroundFocusedColorKey":
                    return LightColorValues.Color_FFC9DEF5;
                case "ComboBoxGlyphBackgroundHoverColorKey":
                    return LightColorValues.Color_FFC9DEF5;
                case "ComboBoxGlyphBackgroundPressedColorKey":
                    return LightColorValues.Color_FF007ACC;
                case "ComboBoxGlyphDisabledColorKey":
                    return LightColorValues.Color_FFCCCEDB;
                case "ComboBoxGlyphFocusedColorKey":
                    return LightColorValues.Color_FF1E1E1E;
                case "ComboBoxGlyphHoverColorKey":
                    return LightColorValues.Color_FF1E1E1E;
                case "ComboBoxGlyphPressedColorKey":
                    return LightColorValues.Color_FFFFFFFF;

                case "ComboBoxListBackgroundColorKey":
                    return LightColorValues.Color_FFF6F6F6;
                case "ComboBoxListBackgroundShadowColorKey":
                    return LightColorValues.Color_19000000;
                case "ComboBoxListBorderColorKey":
                    return LightColorValues.Color_FFCCCEDB;
                case "ComboBoxListItemBackgroundHoverColorKey":
                    return LightColorValues.Color_FFC9DEF5;
                case "ComboBoxListItemBorderHoverColorKey":
                    return LightColorValues.Color_FFC9DEF5;
                case "ComboBoxListItemTextColorKey":
                    return LightColorValues.Color_FF1E1E1E;
                case "ComboBoxListItemTextHoverColorKey":
                    return LightColorValues.Color_FF1E1E1E;

                case "ComboBoxSelectionColorKey":
                    return LightColorValues.Color_FF007ACC;

                case "ComboBoxSeparatorColorKey":
                    return LightColorValues.Color_FFFFFFFF;
                case "ComboBoxSeparatorDisabledColorKey":
                    return LightColorValues.Color_FFEEEEF2;
                case "ComboBoxSeparatorFocusedColorKey":
                    return LightColorValues.Color_FF007ACC;
                case "ComboBoxSeparatorHoverColorKey":
                    return LightColorValues.Color_FF007ACC;
                case "ComboBoxSeparatorPressedColorKey":
                    return LightColorValues.Color_FF007ACC;

                case "ComboBoxTextColorKey":
                    return LightColorValues.Color_FF1E1E1E;
                case "ComboBoxTextDisabledColorKey":
                    return LightColorValues.Color_FFA2A4A5;
                case "ComboBoxTextFocusedColorKey":
                    return LightColorValues.Color_FF1E1E1E;
                case "ComboBoxTextHoverColorKey":
                    return LightColorValues.Color_FF1E1E1E;
                case "ComboBoxTextInputSelectionColorKey":
                    return LightColorValues.Color_66007ACC;
                case "ComboBoxTextPressedColorKey":
                    return LightColorValues.Color_FF1E1E1E;
                default:
                    throw new Exception($"The color key value '{memberName}' is not found");
            }
        }

        private Brush GetThemedCardColor(string memberName)
        {
            switch (memberName)
            {
                case "CardTitleTextColorKey":
                    return LightColorValues.Color_FF1E1E1E;
                case "CardDescriptionTextColorKey":
                    return LightColorValues.Color_FF717171;
                case "CardBackgroundDefaultColorKey":
                    return LightColorValues.Color_FFFFFFFF;
                case "CardBackgroundFocusColorKey":
                    return LightColorValues.Color_FFFFFFFF;
                case "CardBackgroundHoverColorKey":
                    return LightColorValues.Color_FFEFEFF2;
                case "CardBackgroundPressedColorKey":
                    return LightColorValues.Color_FFEFEFF2;
                case "CardBackgroundSelectedColorKey":
                    return LightColorValues.Color_FFFFFFFF;
                case "CardBackgroundDisabledColorKey":
                    return LightColorValues.Color_FFF5F5F5;
                case "CardBorderDefaultColorKey":
                    return LightColorValues.Color_FFBFBFBF;
                case "CardBorderFocusColorKey":
                    return LightColorValues.Color_FF3399FF;
                case "CardBorderHoverColorKey":
                    return LightColorValues.Color_FF9A9A9A;
                case "CardBorderPressedColorKey":
                    return LightColorValues.Color_FF007ACC;
                case "CardBorderSelectedColorKey":
                    return LightColorValues.Color_FF007ACC;
                case "CardBorderDisabledColorKey":
                    return LightColorValues.Color_FFCCCEDB;
                default:
                    throw new Exception($"The color key value '{memberName}' is not found");
            }
        }
    }
}
