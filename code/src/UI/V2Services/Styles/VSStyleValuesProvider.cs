// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Windows;
using System.Windows.Media;

using Microsoft.VisualStudio.PlatformUI;
using Microsoft.VisualStudio.Shell;

namespace Microsoft.Templates.UI.V2Services
{
    public class VSStyleValuesProvider : IStyleValuesProvider
    {
        public VSStyleValuesProvider()
        {
            VSColorTheme.ThemeChanged += OnThemeChanged;
        }

        private void OnThemeChanged(ThemeChangedEventArgs e)
        {
            ThemeChanged?.Invoke(this, EventArgs.Empty);
        }

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

        private Brush GetThemedDialogColor(string memberName)
        {
            switch (memberName)
            {
                case "WindowPanelColorKey":
                    return GetColor(ThemedDialogColors.WindowPanelColorKey);
                case "WindowBorderColorKey":
                    return GetColor(ThemedDialogColors.WindowBorderColorKey);
                case "HeaderTextColorKey":
                    return GetColor(ThemedDialogColors.HeaderTextColorKey);
                case "HyperlinkColorKey":
                    return GetColor(ThemedDialogColors.HyperlinkColorKey);
                case "HyperlinkHoverColorKey":
                    return GetColor(ThemedDialogColors.HyperlinkHoverColorKey);
                case "HyperlinkPressedColorKey":
                    return GetColor(ThemedDialogColors.HyperlinkPressedColorKey);
                case "HyperlinkDisabledColorKey":
                    return GetColor(ThemedDialogColors.HyperlinkDisabledColorKey);
                case "SelectedItemActiveColorKey":
                    return GetColor(ThemedDialogColors.SelectedItemActiveColorKey);
                case "SelectedItemInactiveColorKey":
                    return GetColor(ThemedDialogColors.SelectedItemInactiveColorKey);
                case "ListItemMouseOverColorKey":
                    return GetColor(ThemedDialogColors.ListItemMouseOverColorKey);
                case "ListItemDisabledTextColorKey":
                    return GetColor(ThemedDialogColors.ListItemDisabledTextColorKey);
                case "GridHeadingBackgroundColorKey":
                    return GetColor(ThemedDialogColors.GridHeadingBackgroundColorKey);
                case "GridHeadingHoverBackgroundColorKey":
                    return GetColor(ThemedDialogColors.GridHeadingHoverBackgroundColorKey);
                case "GridHeadingTextColorKey":
                    return GetColor(ThemedDialogColors.GridHeadingTextColorKey);
                case "GridHeadingHoverTextColorKey":
                    return GetColor(ThemedDialogColors.GridHeadingHoverTextColorKey);
                case "GridLineColorKey":
                    return GetColor(ThemedDialogColors.GridLineColorKey);
                case "SectionDividerColorKey":
                    return GetColor(ThemedDialogColors.SectionDividerColorKey);
                case "WindowButtonColorKey":
                    return GetColor(ThemedDialogColors.WindowButtonColorKey);
                case "WindowButtonHoverColorKey":
                    return GetColor(ThemedDialogColors.WindowButtonHoverColorKey);
                case "WindowButtonDownColorKey":
                    return GetColor(ThemedDialogColors.WindowButtonDownColorKey);
                case "WindowButtonBorderColorKey":
                    return GetColor(ThemedDialogColors.WindowButtonBorderColorKey);
                case "WindowButtonHoverBorderColorKey":
                    return GetColor(ThemedDialogColors.WindowButtonHoverBorderColorKey);
                case "WindowButtonDownBorderColorKey":
                    return GetColor(ThemedDialogColors.WindowButtonDownBorderColorKey);
                case "WindowButtonGlyphColorKey":
                    return GetColor(ThemedDialogColors.WindowButtonGlyphColorKey);
                case "WindowButtonHoverGlyphColorKey":
                    return GetColor(ThemedDialogColors.WindowButtonHoverGlyphColorKey);
                case "WindowButtonDownGlyphColorKey":
                    return GetColor(ThemedDialogColors.WindowButtonDownGlyphColorKey);
                case "WizardFooterColorKey":
                    return GetColor(ThemedDialogColors.WizardFooterColorKey);
                case "HeaderTextSecondaryColorKey":
                    return LightColorValues.Color_FF828282; // TODO: Replace this temporary value for a VS Color
                case "WizardFooterTextColorKey":
                    return LightColorValues.Color_FF828282; // TODO: Replace this temporary value for a VS Color
                default:
                    throw new Exception($"The color key value '{memberName}' is not found");
            }
        }

        private Brush GetCommonDocumentColor(string memberName)
        {
            switch (memberName)
            {
                case "ListItemTextColorKey":
                    return GetColor(CommonDocumentColors.ListItemTextColorKey);
                case "ListItemTextDisabledColorKey":
                    return GetColor(CommonDocumentColors.ListItemTextDisabledColorKey);
                default:
                    throw new Exception($"The color key value '{memberName}' is not found");
            }
        }

        private Brush GetCommonControlColor(string memberName)
        {
            switch (memberName)
            {
                case "ButtonColorKey":
                    return GetColor(CommonControlsColors.ButtonColorKey);
                case "ButtonTextColorKey":
                    return GetColor(CommonControlsColors.ButtonTextColorKey);
                case "ButtonBorderColorKey":
                    return GetColor(CommonControlsColors.ButtonBorderColorKey);

                case "ButtonDefaultColorKey":
                    return GetColor(CommonControlsColors.ButtonDefaultColorKey);
                case "ButtonDefaultTextColorKey":
                    return GetColor(CommonControlsColors.ButtonDefaultTextColorKey);
                case "ButtonBorderDefaultColorKey":
                    return GetColor(CommonControlsColors.ButtonBorderDefaultColorKey);

                case "ButtonDisabledColorKey":
                    return GetColor(CommonControlsColors.ButtonDisabledColorKey);
                case "ButtonDisabledTextColorKey":
                    return GetColor(CommonControlsColors.ButtonDisabledTextColorKey);
                case "ButtonBorderDisabledColorKey":
                    return GetColor(CommonControlsColors.ButtonBorderDisabledColorKey);

                case "ButtonFocusedColorKey":
                    return GetColor(CommonControlsColors.ButtonFocusedColorKey);
                case "ButtonFocusedTextColorKey":
                    return GetColor(CommonControlsColors.ButtonFocusedTextColorKey);
                case "ButtonBorderFocusedColorKey":
                    return GetColor(CommonControlsColors.ButtonBorderFocusedColorKey);

                case "ButtonHoverColorKey":
                    return GetColor(CommonControlsColors.ButtonHoverColorKey);
                case "ButtonHoverTextColorKey":
                    return GetColor(CommonControlsColors.ButtonHoverTextColorKey);
                case "ButtonBorderHoverColorKey":
                    return GetColor(CommonControlsColors.ButtonBorderHoverColorKey);

                case "ButtonPressedColorKey":
                    return GetColor(CommonControlsColors.ButtonPressedColorKey);
                case "ButtonPressedTextColorKey":
                    return GetColor(CommonControlsColors.ButtonPressedTextColorKey);
                case "ButtonBorderPressedColorKey":
                    return GetColor(CommonControlsColors.ButtonBorderPressedColorKey);

                case "ComboBoxBackgroundColorKey":
                    return GetColor(CommonControlsColors.ComboBoxBackgroundColorKey);
                case "ComboBoxBackgroundDisabledColorKey":
                    return GetColor(CommonControlsColors.ComboBoxBackgroundDisabledColorKey);
                case "ComboBoxBackgroundFocusedColorKey":
                    return GetColor(CommonControlsColors.ComboBoxBackgroundFocusedColorKey);
                case "ComboBoxBackgroundHoverColorKey":
                    return GetColor(CommonControlsColors.ComboBoxBackgroundHoverColorKey);
                case "ComboBoxBackgroundPressedColorKey":
                    return GetColor(CommonControlsColors.ComboBoxBackgroundPressedColorKey);

                case "ComboBoxBorderColorKey":
                    return GetColor(CommonControlsColors.ComboBoxBorderColorKey);
                case "ComboBoxBorderDisabledColorKey":
                    return GetColor(CommonControlsColors.ComboBoxBorderDisabledColorKey);
                case "ComboBoxBorderFocusedColorKey":
                    return GetColor(CommonControlsColors.ComboBoxBorderFocusedColorKey);
                case "ComboBoxBorderHoverColorKey":
                    return GetColor(CommonControlsColors.ComboBoxBorderHoverColorKey);
                case "ComboBoxBorderPressedColorKey":
                    return GetColor(CommonControlsColors.ComboBoxBorderPressedColorKey);

                case "ComboBoxGlyphColorKey":
                    return GetColor(CommonControlsColors.ComboBoxGlyphColorKey);
                case "ComboBoxGlyphBackgroundColorKey":
                    return GetColor(CommonControlsColors.ComboBoxGlyphBackgroundColorKey);
                case "ComboBoxGlyphBackgroundDisabledColorKey":
                    return GetColor(CommonControlsColors.ComboBoxGlyphBackgroundDisabledColorKey);
                case "ComboBoxGlyphBackgroundFocusedColorKey":
                    return GetColor(CommonControlsColors.ComboBoxGlyphBackgroundFocusedColorKey);
                case "ComboBoxGlyphBackgroundHoverColorKey":
                    return GetColor(CommonControlsColors.ComboBoxGlyphBackgroundHoverColorKey);
                case "ComboBoxGlyphBackgroundPressedColorKey":
                    return GetColor(CommonControlsColors.ComboBoxGlyphBackgroundPressedColorKey);
                case "ComboBoxGlyphDisabledColorKey":
                    return GetColor(CommonControlsColors.ComboBoxGlyphDisabledColorKey);
                case "ComboBoxGlyphFocusedColorKey":
                    return GetColor(CommonControlsColors.ComboBoxGlyphFocusedColorKey);
                case "ComboBoxGlyphHoverColorKey":
                    return GetColor(CommonControlsColors.ComboBoxGlyphHoverColorKey);
                case "ComboBoxGlyphPressedColorKey":
                    return GetColor(CommonControlsColors.ComboBoxGlyphPressedColorKey);

                case "ComboBoxListBackgroundColorKey":
                    return GetColor(CommonControlsColors.ComboBoxListBackgroundColorKey);
                case "ComboBoxListBackgroundShadowColorKey":
                    return GetColor(CommonControlsColors.ComboBoxListBackgroundShadowColorKey);
                case "ComboBoxListBorderColorKey":
                    return GetColor(CommonControlsColors.ComboBoxListBorderColorKey);
                case "ComboBoxListItemBackgroundHoverColorKey":
                    return GetColor(CommonControlsColors.ComboBoxListItemBackgroundHoverColorKey);
                case "ComboBoxListItemBorderHoverColorKey":
                    return GetColor(CommonControlsColors.ComboBoxListItemBorderHoverColorKey);
                case "ComboBoxListItemTextColorKey":
                    return GetColor(CommonControlsColors.ComboBoxListItemTextColorKey);
                case "ComboBoxListItemTextHoverColorKey":
                    return GetColor(CommonControlsColors.ComboBoxListItemTextHoverColorKey);

                case "ComboBoxSelectionColorKey":
                    return GetColor(CommonControlsColors.ComboBoxSelectionColorKey);

                case "ComboBoxSeparatorColorKey":
                    return GetColor(CommonControlsColors.ComboBoxSeparatorColorKey);
                case "ComboBoxSeparatorDisabledColorKey":
                    return GetColor(CommonControlsColors.ComboBoxSeparatorDisabledColorKey);
                case "ComboBoxSeparatorFocusedColorKey":
                    return GetColor(CommonControlsColors.ComboBoxSeparatorFocusedColorKey);
                case "ComboBoxSeparatorHoverColorKey":
                    return GetColor(CommonControlsColors.ComboBoxSeparatorHoverColorKey);
                case "ComboBoxSeparatorPressedColorKey":
                    return GetColor(CommonControlsColors.ComboBoxSeparatorPressedColorKey);

                case "ComboBoxTextColorKey":
                    return GetColor(CommonControlsColors.ComboBoxTextColorKey);
                case "ComboBoxTextDisabledColorKey":
                    return GetColor(CommonControlsColors.ComboBoxTextDisabledColorKey);
                case "ComboBoxTextFocusedColorKey":
                    return GetColor(CommonControlsColors.ComboBoxTextFocusedColorKey);
                case "ComboBoxTextHoverColorKey":
                    return GetColor(CommonControlsColors.ComboBoxTextHoverColorKey);
                case "ComboBoxTextInputSelectionColorKey":
                    return GetColor(CommonControlsColors.ComboBoxTextInputSelectionColorKey);
                case "ComboBoxTextPressedColorKey":
                    return GetColor(CommonControlsColors.ComboBoxTextPressedColorKey);
                default:
                    throw new Exception($"The color key value '{memberName}' is not found");
            }
        }

        private Brush GetThemedCardColor(string memberName)
        {
            switch (memberName)
            {
                case "CardTitleTextColorKey":
                    return LightColorValues.Color_FF1E1E1E; // TODO: Replace this temporary value for a VS Color
                case "CardDescriptionTextColorKey":
                    return LightColorValues.Color_FF717171; // TODO: Replace this temporary value for a VS Color
                case "CardBackgroundDefaultColorKey":
                    return LightColorValues.Color_FFFFFFFF; // TODO: Replace this temporary value for a VS Color
                case "CardBackgroundFocusColorKey":
                    return LightColorValues.Color_FFFFFFFF; // TODO: Replace this temporary value for a VS Color
                case "CardBackgroundHoverColorKey":
                    return LightColorValues.Color_FFEFEFF2; // TODO: Replace this temporary value for a VS Color
                case "CardBackgroundPressedColorKey":
                    return LightColorValues.Color_FFEFEFF2; // TODO: Replace this temporary value for a VS Color
                case "CardBackgroundSelectedColorKey":
                    return LightColorValues.Color_FFFFFFFF; // TODO: Replace this temporary value for a VS Color
                case "CardBackgroundDisabledColorKey":
                    return LightColorValues.Color_FFF5F5F5; // TODO: Replace this temporary value for a VS Color
                case "CardBorderDefaultColorKey":
                    return LightColorValues.Color_FFBFBFBF; // TODO: Replace this temporary value for a VS Color
                case "CardBorderFocusColorKey":
                    return LightColorValues.Color_FF3399FF; // TODO: Replace this temporary value for a VS Color
                case "CardBorderHoverColorKey":
                    return LightColorValues.Color_FF9A9A9A; // TODO: Replace this temporary value for a VS Color
                case "CardBorderPressedColorKey":
                    return LightColorValues.Color_FF007ACC; // TODO: Replace this temporary value for a VS Color
                case "CardBorderSelectedColorKey":
                    return LightColorValues.Color_FF007ACC; // TODO: Replace this temporary value for a VS Color
                case "CardBorderDisabledColorKey":
                    return LightColorValues.Color_FFCCCEDB; // TODO: Replace this temporary value for a VS Color
                default:
                    throw new Exception($"The color key value '{memberName}' is not found");
            }
        }

        public Brush GetColor(ThemeResourceKey themeResourceKey)
        {
            var themeColor = VSColorTheme.GetThemedColor(themeResourceKey);
            return new SolidColorBrush(new Color()
            {
                A = themeColor.A,
                R = themeColor.R,
                G = themeColor.G,
                B = themeColor.B
            });
        }

        public double GetFontSize(string fontSizeResourceKey)
        {
            switch (fontSizeResourceKey)
            {
                case "Environment100PercentFontSize":
                    return GetVSFontSize(VsFonts.EnvironmentFontSizeKey);
                case "Environment111PercentFontSize":
                    return GetVSFontSize(VsFonts.Environment111PercentFontSizeKey);
                case "Environment122PercentFontSize":
                    return GetVSFontSize(VsFonts.Environment122PercentFontSizeKey);
                case "Environment133PercentFontSize":
                    return GetVSFontSize(VsFonts.Environment133PercentFontSizeKey);
                case "Environment155PercentFontSize":
                    return GetVSFontSize(VsFonts.Environment155PercentFontSizeKey);
                case "Environment200PercentFontSize":
                    return GetVSFontSize(VsFonts.Environment200PercentFontSizeKey);
                case "Environment310PercentFontSize":
                    return GetVSFontSize(VsFonts.Environment310PercentFontSizeKey);
                case "Environment330PercentFontSize":
                    return GetVSFontSize(VsFonts.Environment330PercentFontSizeKey);
                case "Environment375PercentFontSize":
                    return GetVSFontSize(VsFonts.Environment375PercentFontSizeKey);
                case "Environment90PercentFontSize":
                    return GetVSFontSize(VsFonts.Environment90PercentFontSizeKey);
                default:
                    throw new Exception($"The font size value '{fontSizeResourceKey}' is not found");
            }
        }

        public FontFamily GetFontFamily()
        {
            var fontFamily = Application.Current.FindResource(VsFonts.EnvironmentFontFamilyKey);
            if (fontFamily is FontFamily)
            {
                return fontFamily as FontFamily;
            }

            throw new Exception($"The font family is not valid.");
        }

        private double GetVSFontSize(object value)
        {
            var font = Application.Current.FindResource(value);
            if (font is double)
            {
                return (double)font;
            }

            throw new Exception($"The font size is not valid.");
        }
    }
}
