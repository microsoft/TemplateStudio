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
                case "ThemedDialog":
                    return GetColorFromThemedDialog(memberName);
                case "CommonDocument":
                    return GetColorFromCommonDocument(memberName);
                case "CommonControls":
                    return GetColorFromCommonControls(memberName);
                case "Environment":
                    return GetColorFromEnvironment(memberName);
                case "ThemedCard":
                    return GetColorFromThemedCard(memberName);
                case "WindowsTemplateStudio":
                    return GetColorFromWindowsTemplateStudio(memberName);
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

        private Brush GetColorFromThemedDialog(string memberName)
        {
            switch (memberName)
            {
                case "WindowPanel":
                    return LightColorValues.Color_FFFFFFFF;
                case "WindowPanelText":
                    return LightColorValues.Color_FF1E1E1E;
                case "WindowBorder":
                    return LightColorValues.Color_FFCCCEDB;
                case "HeaderText":
                    return LightColorValues.Color_FF1E1E1E;
                case "Hyperlink":
                    return LightColorValues.Color_FF0E70C0;
                case "HyperlinkHover":
                    return LightColorValues.Color_FF007ACC;
                case "HyperlinkPressed":
                    return LightColorValues.Color_FF0E70C0;
                case "HyperlinkDisabled":
                    return LightColorValues.Color_FFA2A4A5;
                case "SelectedItemActive":
                    return LightColorValues.Color_FF007ACC;
                case "SelectedItemInactive":
                    return LightColorValues.Color_FFCCCEDB;
                case "ListItemMouseOver":
                    return LightColorValues.Color_FFC9DEF5;
                case "ListItemDisabledText":
                    return LightColorValues.Color_FFA2A4A5;
                case "GridHeadingBackground":
                    return LightColorValues.Color_00000000;
                case "GridHeadingHoverBackground":
                    return LightColorValues.Color_00000000;
                case "GridHeadingText":
                    return LightColorValues.Color_FF717171;
                case "GridHeadingHoverText":
                    return LightColorValues.Color_FF1E1E1E;
                case "GridLine":
                    return LightColorValues.Color_FFCCCEDB;
                case "SectionDivider":
                    return LightColorValues.Color_FFCCCEDB;
                case "WindowButton":
                    return LightColorValues.Color_00000000;
                case "WindowButtonHover":
                    return LightColorValues.Color_FFC9DEF5;
                case "WindowButtonDown":
                    return LightColorValues.Color_FF007ACC;
                case "WindowButtonBorder":
                    return LightColorValues.Color_00000000;
                case "WindowButtonHoverBorder":
                    return LightColorValues.Color_FFC9DEF5;
                case "WindowButtonDownBorder":
                    return LightColorValues.Color_FF007ACC;
                case "WindowButtonGlyph":
                    return LightColorValues.Color_FF1E1E1E;
                case "WindowButtonHoverGlyph":
                    return LightColorValues.Color_FF1E1E1E;
                case "WindowButtonDownGlyph":
                    return LightColorValues.Color_FFFFFFFF;
                case "WizardFooter":
                    return LightColorValues.Color_FFEFEFF2;
                case "HeaderTextSecondary":
                    return LightColorValues.Color_FF828282;
                case "WizardFooterText":
                    return LightColorValues.Color_FF828282;
                default:
                    throw new Exception($"The color key value '{memberName}' is not found");
            }
        }

        private Brush GetColorFromCommonDocument(string memberName)
        {
            switch (memberName)
            {
                case "ListItemText":
                    return LightColorValues.Color_FF1E1E1E;
                case "ListItemTextDisabled":
                    return LightColorValues.Color_FFA2A4A5;
                default:
                    throw new Exception($"The color key value '{memberName}' is not found");
            }
        }

        private Brush GetColorFromCommonControls(string memberName)
        {
            switch (memberName)
            {
                case "Button":
                    return LightColorValues.Color_FFECECF0;
                case "ButtonText":
                    return LightColorValues.Color_FF1E1E1E;
                case "ButtonBorder":
                    return LightColorValues.Color_FFACACAC;

                case "ButtonDefault":
                    return LightColorValues.Color_FFECECF0;
                case "ButtonDefaultText":
                    return LightColorValues.Color_FF1E1E1E;
                case "ButtonBorderDefault":
                    return LightColorValues.Color_FF3399FF;

                case "ButtonDisabled":
                    return LightColorValues.Color_FFF5F5F5;
                case "ButtonDisabledText":
                    return LightColorValues.Color_FFA2A4A5;
                case "ButtonBorderDisabled":
                    return LightColorValues.Color_FFCCCEDB;

                case "ButtonFocused":
                    return LightColorValues.Color_FFC9DEF5;
                case "ButtonFocusedText":
                    return LightColorValues.Color_FF1E1E1E;
                case "ButtonBorderFocused":
                    return LightColorValues.Color_FF3399FF;

                case "ButtonHover":
                    return LightColorValues.Color_FFC9DEF5;
                case "ButtonHoverText":
                    return LightColorValues.Color_FF1E1E1E;
                case "ButtonBorderHover":
                    return LightColorValues.Color_FF3399FF;

                case "ButtonPressed":
                    return LightColorValues.Color_FF007ACC;
                case "ButtonPressedText":
                    return LightColorValues.Color_FFFFFFFF;
                case "ButtonBorderPressed":
                    return LightColorValues.Color_FF007ACC;

                case "ComboBoxBackground":
                    return LightColorValues.Color_FFFFFFFF;
                case "ComboBoxBackgroundDisabled":
                    return LightColorValues.Color_FFEEEEF2;
                case "ComboBoxBackgroundFocused":
                    return LightColorValues.Color_FFFFFFFF;
                case "ComboBoxBackgroundHover":
                    return LightColorValues.Color_FFFFFFFF;
                case "ComboBoxBackgroundPressed":
                    return LightColorValues.Color_FFFFFFFF;

                case "ComboBoxBorder":
                    return LightColorValues.Color_FFCCCEDB;
                case "ComboBoxBorderDisabled":
                    return LightColorValues.Color_FFCCCEDB;
                case "ComboBoxBorderFocused":
                    return LightColorValues.Color_FF007ACC;
                case "ComboBoxBorderHover":
                    return LightColorValues.Color_FF007ACC;
                case "ComboBoxBorderPressed":
                    return LightColorValues.Color_FF007ACC;

                case "ComboBoxGlyph":
                    return LightColorValues.Color_FF717171;
                case "ComboBoxGlyphBackground":
                    return LightColorValues.Color_FFFFFFFF;
                case "ComboBoxGlyphBackgroundDisabled":
                    return LightColorValues.Color_FFEEEEF2;
                case "ComboBoxGlyphBackgroundFocused":
                    return LightColorValues.Color_FFC9DEF5;
                case "ComboBoxGlyphBackgroundHover":
                    return LightColorValues.Color_FFC9DEF5;
                case "ComboBoxGlyphBackgroundPressed":
                    return LightColorValues.Color_FF007ACC;
                case "ComboBoxGlyphDisabled":
                    return LightColorValues.Color_FFCCCEDB;
                case "ComboBoxGlyphFocused":
                    return LightColorValues.Color_FF1E1E1E;
                case "ComboBoxGlyphHover":
                    return LightColorValues.Color_FF1E1E1E;
                case "ComboBoxGlyphPressed":
                    return LightColorValues.Color_FFFFFFFF;

                case "ComboBoxListBackground":
                    return LightColorValues.Color_FFF6F6F6;
                case "ComboBoxListBackgroundShadow":
                    return LightColorValues.Color_19000000;
                case "ComboBoxListBorder":
                    return LightColorValues.Color_FFCCCEDB;
                case "ComboBoxListItemBackgroundHover":
                    return LightColorValues.Color_FFC9DEF5;
                case "ComboBoxListItemBorderHover":
                    return LightColorValues.Color_FFC9DEF5;
                case "ComboBoxListItemText":
                    return LightColorValues.Color_FF1E1E1E;
                case "ComboBoxListItemTextHover":
                    return LightColorValues.Color_FF1E1E1E;

                case "ComboBoxSelection":
                    return LightColorValues.Color_FF007ACC;

                case "ComboBoxSeparator":
                    return LightColorValues.Color_FFFFFFFF;
                case "ComboBoxSeparatorDisabled":
                    return LightColorValues.Color_FFEEEEF2;
                case "ComboBoxSeparatorFocused":
                    return LightColorValues.Color_FF007ACC;
                case "ComboBoxSeparatorHover":
                    return LightColorValues.Color_FF007ACC;
                case "ComboBoxSeparatorPressed":
                    return LightColorValues.Color_FF007ACC;

                case "ComboBoxText":
                    return LightColorValues.Color_FF1E1E1E;
                case "ComboBoxTextDisabled":
                    return LightColorValues.Color_FFA2A4A5;
                case "ComboBoxTextFocused":
                    return LightColorValues.Color_FF1E1E1E;
                case "ComboBoxTextHover":
                    return LightColorValues.Color_FF1E1E1E;
                case "ComboBoxTextInputSelection":
                    return LightColorValues.Color_66007ACC;
                case "ComboBoxTextPressed":
                    return LightColorValues.Color_FF1E1E1E;

                case "TextBoxBackground":
                    return LightColorValues.Color_FFFFFFFF;
                case "TextBoxBorder":
                    return LightColorValues.Color_FFCCCEDB;
                case "TextBoxText":
                    return LightColorValues.Color_FF1E1E1E;

                case "TextBoxBackgroundDisabled":
                    return LightColorValues.Color_FFEEEEF2;
                case "TextBoxBorderDisabled":
                    return LightColorValues.Color_FFCCCEDB;
                case "TextBoxTextDisabled":
                    return LightColorValues.Color_FFA2A4A5;

                case "TextBoxBackgroundFocused":
                    return LightColorValues.Color_FFFFFFFF;
                case "TextBoxBorderFocused":
                    return LightColorValues.Color_FF007ACC;
                case "TextBoxTextFocused":
                    return LightColorValues.Color_FF1E1E1E;
                default:
                    throw new Exception($"The color key value '{memberName}' is not found");
            }
        }

        private Brush GetColorFromEnvironment(string memberName)
        {
            switch (memberName)
            {
                case "PageSideBarExpanderBody":
                    return LightColorValues.Color_FFF5F5F5;
                case "PageSideBarExpanderChevron":
                    return LightColorValues.Color_FF1E1E1E;
                case "PageSideBarExpanderHeader":
                    return LightColorValues.Color_FFFEFEFE;
                case "PageSideBarExpanderHeaderHover":
                    return LightColorValues.Color_FFCCCEDB;
                case "PageSideBarExpanderHeaderPressed":
                    return LightColorValues.Color_FFCCCEDB;
                case "PageSideBarExpanderSeparator":
                    return LightColorValues.Color_FFCCCEDB;
                case "PageSideBarExpanderText":
                    return LightColorValues.Color_FF1E1E1E;
                default:
                    throw new Exception($"The color key value '{memberName}' is not found");
            }
        }

        private Brush GetColorFromThemedCard(string memberName)
        {
            switch (memberName)
            {
                case "CardTitleText":
                    return LightColorValues.Color_FF1E1E1E;
                case "CardDescriptionText":
                    return LightColorValues.Color_FF717171;
                case "CardBackgroundDefault":
                    return LightColorValues.Color_FFFFFFFF;
                case "CardBackgroundFocus":
                    return LightColorValues.Color_FFFFFFFF;
                case "CardBackgroundHover":
                    return LightColorValues.Color_FFEFEFF2;
                case "CardBackgroundPressed":
                    return LightColorValues.Color_FFEFEFF2;
                case "CardBackgroundSelected":
                    return LightColorValues.Color_FFFFFFFF;
                case "CardBackgroundDisabled":
                    return LightColorValues.Color_FFF5F5F5;
                case "CardBorderDefault":
                    return LightColorValues.Color_FFBFBFBF;
                case "CardBorderFocus":
                    return LightColorValues.Color_FF3399FF;
                case "CardBorderHover":
                    return LightColorValues.Color_FF9A9A9A;
                case "CardBorderPressed":
                    return LightColorValues.Color_FF007ACC;
                case "CardBorderSelected":
                    return LightColorValues.Color_FF007ACC;
                case "CardBorderDisabled":
                    return LightColorValues.Color_FFCCCEDB;
                case "CardIcon":
                    return LightColorValues.Color_FF888D8F;
                case "CardFooterText":
                    return LightColorValues.Color_FF575757;
                default:
                    throw new Exception($"The color key value '{memberName}' is not found");
            }
        }

        private Brush GetColorFromWindowsTemplateStudio(string memberName)
        {
            switch (memberName)
            {
                case "NotificationInformationText":
                    return LightColorValues.Color_FF1E1E1E;
                case "NotificationInformationBackground":
                    return LightColorValues.Color_FFE5F1FB;
                case "NotificationInformationIcon":
                    return LightColorValues.Color_FF18A2E7;

                case "NotificationWarningText":
                    return LightColorValues.Color_FF1E1E1E;
                case "NotificationWarningBackground":
                    return LightColorValues.Color_FFFDFBAC;
                case "NotificationWarningIcon":
                    return LightColorValues.Color_FF18A2E7;

                case "NotificationErrorText":
                    return LightColorValues.Color_FF1E1E1E;
                case "NotificationErrorBackground":
                    return LightColorValues.Color_FFFDFBAC;
                case "NotificationErrorIcon":
                    return LightColorValues.Color_FF18A2E7;

                case "DeleteTemplateIcon":
                    return LightColorValues.Color_FFE82C3C;
                case "SavedTemplateBackgroundHover":
                    return LightColorValues.Color_FFE9E9E9;

                case "NewItemFileStatusNewFile":
                    return LightColorValues.Color_FF00CC6A;
                case "NewItemFileStatusModifiedFile":
                    return LightColorValues.Color_FF0078D6;
                case "NewItemFileStatusConflictingFile":
                    return LightColorValues.Color_FFE81123;
                case "NewItemFileStatusConflictingStylesFile":
                    return LightColorValues.Color_FFFFB900;
                case "NewItemFileStatusWarningFile":
                    return LightColorValues.Color_FFFFB900;
                case "NewItemFileStatusUnchangedFile":
                    return LightColorValues.Color_FF004F9E;

                case "DialogInfoIcon":
                    return LightColorValues.Color_FF007ACC;
                case "DialogErrorIcon":
                    return LightColorValues.Color_FFFF0000;
                case "DialogWarningIcon":
                    return LightColorValues.Color_FFFFCC00;

                default:
                    throw new Exception($"The color key value '{memberName}' is not found");
            }
        }
    }
}
