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
            if (className == "ThemedDialogColors")
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
                        return LightColorValues.Color_FF_828282; // TODO: Replace this temporary value for a VS Color
                    case "WizardFooterTextColorKey":
                        return LightColorValues.Color_FF_828282; // TODO: Replace this temporary value for a VS Color
                    default:
                        throw new Exception($"The color key value '{memberName}' is not found");
                }
            }
            else if (className == "ThemedCardColors")
            {
                switch (memberName)
                {
                    case "CardTitleTextColorKey":
                        return LightColorValues.Color_FF_1E1E1E; // TODO: Replace this temporary value for a VS Color
                    case "CardDescriptionTextColorKey":
                        return LightColorValues.Color_FF_717171; // TODO: Replace this temporary value for a VS Color
                    case "CardBackgroundDefaultColorKey":
                        return LightColorValues.Color_FF_FFFFFF; // TODO: Replace this temporary value for a VS Color
                    case "CardBackgroundFocusColorKey":
                        return LightColorValues.Color_FF_FFFFFF; // TODO: Replace this temporary value for a VS Color
                    case "CardBackgroundHoverColorKey":
                        return LightColorValues.Color_FF_EFEFF2; // TODO: Replace this temporary value for a VS Color
                    case "CardBackgroundPressedColorKey":
                        return LightColorValues.Color_FF_EFEFF2; // TODO: Replace this temporary value for a VS Color
                    case "CardBackgroundSelectedColorKey":
                        return LightColorValues.Color_FF_FFFFFF; // TODO: Replace this temporary value for a VS Color
                    case "CardBackgroundDisabledColorKey":
                        return LightColorValues.Color_FF_F5F5F5; // TODO: Replace this temporary value for a VS Color
                    case "CardBorderDefaultColorKey":
                        return LightColorValues.Color_FF_BFBFBF; // TODO: Replace this temporary value for a VS Color
                    case "CardBorderFocusColorKey":
                        return LightColorValues.Color_FF_3399FF; // TODO: Replace this temporary value for a VS Color
                    case "CardBorderHoverColorKey":
                        return LightColorValues.Color_FF_9A9A9A; // TODO: Replace this temporary value for a VS Color
                    case "CardBorderPressedColorKey":
                        return LightColorValues.Color_FF_007ACC; // TODO: Replace this temporary value for a VS Color
                    case "CardBorderSelectedColorKey":
                        return LightColorValues.Color_FF_007ACC; // TODO: Replace this temporary value for a VS Color
                    case "CardBorderDisabledColorKey":
                        return LightColorValues.Color_FF_CCCEDB; // TODO: Replace this temporary value for a VS Color
                    default:
                        throw new Exception($"The color key value '{memberName}' is not found");
                }
            }
            else
            {
                throw new Exception($"Class name not recognized '{className}'");
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
                case "EnvironmentFontSize":
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
