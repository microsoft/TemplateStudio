// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Windows;
using System.Windows.Media;

using Microsoft.VisualStudio.PlatformUI;
using Microsoft.VisualStudio.Shell;

namespace Microsoft.Templates.UI.Services
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
                case "ThemedDialog":
                    return GetColorFromThemedDialog(memberName);
                case "CommonDocument":
                    return GetColorFromCommonDocument(memberName);
                case "CommonControls":
                    return GetColorFromCommonControls(memberName);
                case "Environment":
                    return GetColorFromEnvironment(memberName);
                case "WindowsTemplateStudio":
                    return GetColorFromWindowsTemplateStudio(memberName);
                default:
                    throw new Exception($"Class name not recognized '{className}'");
            }
        }

        private Brush GetColorFromThemedDialog(string memberName)
        {
            switch (memberName)
            {
                case "WindowPanel":
                    return GetColor(ThemedDialogColors.WindowPanelColorKey);
                case "WindowPanelText":
                    return GetColor(ThemedDialogColors.WindowPanelTextColorKey);
                case "WindowBorder":
                    return GetColor(ThemedDialogColors.WindowBorderColorKey);
                case "HeaderText":
                    return GetColor(ThemedDialogColors.HeaderTextColorKey);
                case "Hyperlink":
                    return GetColor(ThemedDialogColors.HyperlinkColorKey);
                case "HyperlinkHover":
                    return GetColor(ThemedDialogColors.HyperlinkHoverColorKey);
                case "HyperlinkPressed":
                    return GetColor(ThemedDialogColors.HyperlinkPressedColorKey);
                case "HyperlinkDisabled":
                    return GetColor(ThemedDialogColors.HyperlinkDisabledColorKey);
                case "SelectedItemActive":
                    return GetColor(ThemedDialogColors.SelectedItemActiveColorKey);
                case "SelectedItemInactive":
                    return GetColor(ThemedDialogColors.SelectedItemInactiveColorKey);
                case "ListItemMouseOver":
                    return GetColor(ThemedDialogColors.ListItemMouseOverColorKey);
                case "ListItemDisabledText":
                    return GetColor(ThemedDialogColors.ListItemDisabledTextColorKey);
                case "GridHeadingBackground":
                    return GetColor(ThemedDialogColors.GridHeadingBackgroundColorKey);
                case "GridHeadingHoverBackground":
                    return GetColor(ThemedDialogColors.GridHeadingHoverBackgroundColorKey);
                case "GridHeadingText":
                    return GetColor(ThemedDialogColors.GridHeadingTextColorKey);
                case "GridHeadingHoverText":
                    return GetColor(ThemedDialogColors.GridHeadingHoverTextColorKey);
                case "GridLine":
                    return GetColor(ThemedDialogColors.GridLineColorKey);
                case "SectionDivider":
                    return GetColor(ThemedDialogColors.SectionDividerColorKey);
                case "WindowButton":
                    return GetColor(ThemedDialogColors.WindowButtonColorKey);
                case "WindowButtonHover":
                    return GetColor(ThemedDialogColors.WindowButtonHoverColorKey);
                case "WindowButtonDown":
                    return GetColor(ThemedDialogColors.WindowButtonDownColorKey);
                case "WindowButtonBorder":
                    return GetColor(ThemedDialogColors.WindowButtonBorderColorKey);
                case "WindowButtonHoverBorder":
                    return GetColor(ThemedDialogColors.WindowButtonHoverBorderColorKey);
                case "WindowButtonDownBorder":
                    return GetColor(ThemedDialogColors.WindowButtonDownBorderColorKey);
                case "WindowButtonGlyph":
                    return GetColor(ThemedDialogColors.WindowButtonGlyphColorKey);
                case "WindowButtonHoverGlyph":
                    return GetColor(ThemedDialogColors.WindowButtonHoverGlyphColorKey);
                case "WindowButtonDownGlyph":
                    return GetColor(ThemedDialogColors.WindowButtonDownGlyphColorKey);
                case "WizardFooter":
                    return GetColor(ThemedDialogColors.WizardFooterColorKey);
                case "HeaderTextSecondary":
                    return LightColorValues.Color_FF828282; // TODO: Replace this temporary value for a VS Color
                case "WizardFooterText":
                    return LightColorValues.Color_FF828282; // TODO: Replace this temporary value for a VS Color
                default:
                    throw new Exception($"The color key value '{memberName}' is not found");
            }
        }

        private Brush GetColorFromCommonDocument(string memberName)
        {
            switch (memberName)
            {
                case "ListItemText":
                    return GetColor(CommonDocumentColors.ListItemTextColorKey);
                case "ListItemTextDisabled":
                    return GetColor(CommonDocumentColors.ListItemTextDisabledColorKey);
                default:
                    throw new Exception($"The color key value '{memberName}' is not found");
            }
        }

        private Brush GetColorFromCommonControls(string memberName)
        {
            switch (memberName)
            {
                case "Button":
                    return GetColor(CommonControlsColors.ButtonColorKey);
                case "ButtonText":
                    return GetColor(CommonControlsColors.ButtonTextColorKey);
                case "ButtonBorder":
                    return GetColor(CommonControlsColors.ButtonBorderColorKey);

                case "ButtonDefault":
                    return GetColor(CommonControlsColors.ButtonDefaultColorKey);
                case "ButtonDefaultText":
                    return GetColor(CommonControlsColors.ButtonDefaultTextColorKey);
                case "ButtonBorderDefault":
                    return GetColor(CommonControlsColors.ButtonBorderDefaultColorKey);

                case "ButtonDisabled":
                    return GetColor(CommonControlsColors.ButtonDisabledColorKey);
                case "ButtonDisabledText":
                    return GetColor(CommonControlsColors.ButtonDisabledTextColorKey);
                case "ButtonBorderDisabled":
                    return GetColor(CommonControlsColors.ButtonBorderDisabledColorKey);

                case "ButtonFocused":
                    return GetColor(CommonControlsColors.ButtonFocusedColorKey);
                case "ButtonFocusedText":
                    return GetColor(CommonControlsColors.ButtonFocusedTextColorKey);
                case "ButtonBorderFocused":
                    return GetColor(CommonControlsColors.ButtonBorderFocusedColorKey);

                case "ButtonHover":
                    return GetColor(CommonControlsColors.ButtonHoverColorKey);
                case "ButtonHoverText":
                    return GetColor(CommonControlsColors.ButtonHoverTextColorKey);
                case "ButtonBorderHover":
                    return GetColor(CommonControlsColors.ButtonBorderHoverColorKey);

                case "ButtonPressed":
                    return GetColor(CommonControlsColors.ButtonPressedColorKey);
                case "ButtonPressedText":
                    return GetColor(CommonControlsColors.ButtonPressedTextColorKey);
                case "ButtonBorderPressed":
                    return GetColor(CommonControlsColors.ButtonBorderPressedColorKey);

                case "ComboBoxBackground":
                    return GetColor(CommonControlsColors.ComboBoxBackgroundColorKey);
                case "ComboBoxBackgroundDisabled":
                    return GetColor(CommonControlsColors.ComboBoxBackgroundDisabledColorKey);
                case "ComboBoxBackgroundFocused":
                    return GetColor(CommonControlsColors.ComboBoxBackgroundFocusedColorKey);
                case "ComboBoxBackgroundHover":
                    return GetColor(CommonControlsColors.ComboBoxBackgroundHoverColorKey);
                case "ComboBoxBackgroundPressed":
                    return GetColor(CommonControlsColors.ComboBoxBackgroundPressedColorKey);

                case "ComboBoxBorder":
                    return GetColor(CommonControlsColors.ComboBoxBorderColorKey);
                case "ComboBoxBorderDisabled":
                    return GetColor(CommonControlsColors.ComboBoxBorderDisabledColorKey);
                case "ComboBoxBorderFocused":
                    return GetColor(CommonControlsColors.ComboBoxBorderFocusedColorKey);
                case "ComboBoxBorderHover":
                    return GetColor(CommonControlsColors.ComboBoxBorderHoverColorKey);
                case "ComboBoxBorderPressed":
                    return GetColor(CommonControlsColors.ComboBoxBorderPressedColorKey);

                case "ComboBoxGlyph":
                    return GetColor(CommonControlsColors.ComboBoxGlyphColorKey);
                case "ComboBoxGlyphBackground":
                    return GetColor(CommonControlsColors.ComboBoxGlyphBackgroundColorKey);
                case "ComboBoxGlyphBackgroundDisabled":
                    return GetColor(CommonControlsColors.ComboBoxGlyphBackgroundDisabledColorKey);
                case "ComboBoxGlyphBackgroundFocused":
                    return GetColor(CommonControlsColors.ComboBoxGlyphBackgroundFocusedColorKey);
                case "ComboBoxGlyphBackgroundHover":
                    return GetColor(CommonControlsColors.ComboBoxGlyphBackgroundHoverColorKey);
                case "ComboBoxGlyphBackgroundPressed":
                    return GetColor(CommonControlsColors.ComboBoxGlyphBackgroundPressedColorKey);
                case "ComboBoxGlyphDisabled":
                    return GetColor(CommonControlsColors.ComboBoxGlyphDisabledColorKey);
                case "ComboBoxGlyphFocused":
                    return GetColor(CommonControlsColors.ComboBoxGlyphFocusedColorKey);
                case "ComboBoxGlyphHover":
                    return GetColor(CommonControlsColors.ComboBoxGlyphHoverColorKey);
                case "ComboBoxGlyphPressed":
                    return GetColor(CommonControlsColors.ComboBoxGlyphPressedColorKey);

                case "ComboBoxListBackground":
                    return GetColor(CommonControlsColors.ComboBoxListBackgroundColorKey);
                case "ComboBoxListBackgroundShadow":
                    return GetColor(CommonControlsColors.ComboBoxListBackgroundShadowColorKey);
                case "ComboBoxListBorder":
                    return GetColor(CommonControlsColors.ComboBoxListBorderColorKey);
                case "ComboBoxListItemBackgroundHover":
                    return GetColor(CommonControlsColors.ComboBoxListItemBackgroundHoverColorKey);
                case "ComboBoxListItemBorderHover":
                    return GetColor(CommonControlsColors.ComboBoxListItemBorderHoverColorKey);
                case "ComboBoxListItemText":
                    return GetColor(CommonControlsColors.ComboBoxListItemTextColorKey);
                case "ComboBoxListItemTextHover":
                    return GetColor(CommonControlsColors.ComboBoxListItemTextHoverColorKey);

                case "ComboBoxSelection":
                    return GetColor(CommonControlsColors.ComboBoxSelectionColorKey);

                case "ComboBoxSeparator":
                    return GetColor(CommonControlsColors.ComboBoxSeparatorColorKey);
                case "ComboBoxSeparatorDisabled":
                    return GetColor(CommonControlsColors.ComboBoxSeparatorDisabledColorKey);
                case "ComboBoxSeparatorFocused":
                    return GetColor(CommonControlsColors.ComboBoxSeparatorFocusedColorKey);
                case "ComboBoxSeparatorHover":
                    return GetColor(CommonControlsColors.ComboBoxSeparatorHoverColorKey);
                case "ComboBoxSeparatorPressed":
                    return GetColor(CommonControlsColors.ComboBoxSeparatorPressedColorKey);

                case "ComboBoxText":
                    return GetColor(CommonControlsColors.ComboBoxTextColorKey);
                case "ComboBoxTextDisabled":
                    return GetColor(CommonControlsColors.ComboBoxTextDisabledColorKey);
                case "ComboBoxTextFocused":
                    return GetColor(CommonControlsColors.ComboBoxTextFocusedColorKey);
                case "ComboBoxTextHover":
                    return GetColor(CommonControlsColors.ComboBoxTextHoverColorKey);
                case "ComboBoxTextInputSelection":
                    return GetColor(CommonControlsColors.ComboBoxTextInputSelectionColorKey);
                case "ComboBoxTextPressed":
                    return GetColor(CommonControlsColors.ComboBoxTextPressedColorKey);

                case "TextBoxBackground":
                    return GetColor(CommonControlsColors.TextBoxBackgroundColorKey);
                case "TextBoxBorder":
                    return GetColor(CommonControlsColors.TextBoxBorderColorKey);
                case "TextBoxText":
                    return GetColor(CommonControlsColors.TextBoxTextColorKey);

                case "TextBoxBackgroundDisabled":
                    return GetColor(CommonControlsColors.TextBoxBackgroundDisabledColorKey);
                case "TextBoxBorderDisabled":
                    return GetColor(CommonControlsColors.TextBoxBorderDisabledColorKey);
                case "TextBoxTextDisabled":
                    return GetColor(CommonControlsColors.TextBoxTextDisabledColorKey);

                case "TextBoxBackgroundFocused":
                    return GetColor(CommonControlsColors.TextBoxBackgroundFocusedColorKey);
                case "TextBoxBorderFocused":
                    return GetColor(CommonControlsColors.TextBoxBorderFocusedColorKey);
                case "TextBoxTextFocused":
                    return GetColor(CommonControlsColors.TextBoxTextFocusedColorKey);

                case "CheckBoxBackground":
                    return GetColor(CommonControlsColors.CheckBoxBackgroundColorKey);
                case "CheckBoxBackgroundDisabled":
                    return GetColor(CommonControlsColors.CheckBoxBackgroundDisabledColorKey);
                case "CheckBoxBackgroundFocused":
                    return GetColor(CommonControlsColors.CheckBoxBackgroundFocusedColorKey);
                case "CheckBoxBackgroundHover":
                    return GetColor(CommonControlsColors.CheckBoxBackgroundHoverColorKey);
                case "CheckBoxBackgroundPressed":
                    return GetColor(CommonControlsColors.CheckBoxBackgroundPressedColorKey);

                case "CheckBoxGlyph":
                    return GetColor(CommonControlsColors.CheckBoxGlyphColorKey);
                case "CheckBoxGlyphDisabled":
                    return GetColor(CommonControlsColors.CheckBoxGlyphDisabledColorKey);
                case "CheckBoxGlyphFocused":
                    return GetColor(CommonControlsColors.CheckBoxGlyphFocusedColorKey);
                case "CheckBoxGlyphHover":
                    return GetColor(CommonControlsColors.CheckBoxGlyphHoverColorKey);
                case "CheckBoxGlyphPressed":
                    return GetColor(CommonControlsColors.CheckBoxGlyphPressedColorKey);

                case "CheckBoxBorder":
                    return GetColor(CommonControlsColors.CheckBoxBorderColorKey);
                case "CheckBoxBorderDisabled":
                    return GetColor(CommonControlsColors.CheckBoxBorderDisabledColorKey);
                case "CheckBoxBorderFocused":
                    return GetColor(CommonControlsColors.CheckBoxBorderFocusedColorKey);
                case "CheckBoxBorderHover":
                    return GetColor(CommonControlsColors.CheckBoxBorderHoverColorKey);
                case "CheckBoxBorderPressed":
                    return GetColor(CommonControlsColors.CheckBoxBorderPressedColorKey);

                case "CheckBoxText":
                    return GetColor(CommonControlsColors.CheckBoxTextColorKey);
                case "CheckBoxTextDisabled":
                    return GetColor(CommonControlsColors.CheckBoxTextDisabledColorKey);
                case "CheckBoxTextFocused":
                    return GetColor(CommonControlsColors.CheckBoxTextFocusedColorKey);
                case "CheckBoxTextHover":
                    return GetColor(CommonControlsColors.CheckBoxTextHoverColorKey);
                case "CheckBoxTextPressed":
                    return GetColor(CommonControlsColors.CheckBoxTextPressedColorKey);

                default:
                    throw new Exception($"The color key value '{memberName}' is not found");
            }
        }

        private Brush GetColorFromEnvironment(string memberName)
        {
            switch (memberName)
            {
                case "PageSideBarExpanderBody":
                    return GetColor(EnvironmentColors.PageSideBarExpanderBodyColorKey);
                case "PageSideBarExpanderChevron":
                    return GetColor(EnvironmentColors.PageSideBarExpanderChevronColorKey);
                case "PageSideBarExpanderHeader":
                    return GetColor(EnvironmentColors.PageSideBarExpanderHeaderColorKey);
                case "PageSideBarExpanderHeaderHover":
                    return GetColor(EnvironmentColors.PageSideBarExpanderHeaderHoverColorKey);
                case "PageSideBarExpanderHeaderPressed":
                    return GetColor(EnvironmentColors.PageSideBarExpanderHeaderPressedColorKey);
                case "PageSideBarExpanderSeparator":
                    return GetColor(EnvironmentColors.PageSideBarExpanderSeparatorColorKey);
                case "PageSideBarExpanderText":
                    return GetColor(EnvironmentColors.PageSideBarExpanderTextColorKey);
                default:
                    throw new Exception($"The color key value '{memberName}' is not found");
            }
        }

        private Brush GetColorFromWindowsTemplateStudio(string memberName)
        {
            switch (memberName)
            {
                case "CardTitleText":
                    return GetColor(WindowsTemplateStudioColors.CardTitleTextColorKey);
                case "CardDescriptionText":
                    return GetColor(WindowsTemplateStudioColors.CardDescriptionTextColorKey);
                case "CardBackgroundDefault":
                    return GetColor(WindowsTemplateStudioColors.CardBackgroundDefaultColorKey);
                case "CardBackgroundFocus":
                    return GetColor(WindowsTemplateStudioColors.CardBackgroundFocusColorKey);
                case "CardBackgroundHover":
                    return GetColor(WindowsTemplateStudioColors.CardBackgroundHoverColorKey);
                case "CardBackgroundPressed":
                    return GetColor(WindowsTemplateStudioColors.CardBackgroundPressedColorKey);
                case "CardBackgroundSelected":
                    return GetColor(WindowsTemplateStudioColors.CardBackgroundSelectedColorKey);
                case "CardBackgroundDisabled":
                    return GetColor(WindowsTemplateStudioColors.CardBackgroundDisabledColorKey);
                case "CardBorderDefault":
                    return GetColor(WindowsTemplateStudioColors.CardBorderDefaultColorKey);
                case "CardBorderFocus":
                    return GetColor(WindowsTemplateStudioColors.CardBorderFocusColorKey);
                case "CardBorderHover":
                    return GetColor(WindowsTemplateStudioColors.CardBorderHoverColorKey);
                case "CardBorderPressed":
                    return GetColor(WindowsTemplateStudioColors.CardBorderPressedColorKey);
                case "CardBorderSelected":
                    return GetColor(WindowsTemplateStudioColors.CardBorderSelectedColorKey);
                case "CardBorderDisabled":
                    return GetColor(WindowsTemplateStudioColors.CardBorderDisabledColorKey);
                case "CardIcon":
                    return GetColor(WindowsTemplateStudioColors.CardIconColorKey);
                case "CardFooterText":
                    return GetColor(WindowsTemplateStudioColors.CardFooterTextColorKey);

                case "NotificationInformationText":
                    return LightColorValues.Color_FF1E1E1E; // TODO: Replace this temporary value for a VS Color
                case "NotificationInformationBackground":
                    return LightColorValues.Color_FFE5F1FB; // TODO: Replace this temporary value for a VS Color
                case "NotificationInformationIcon":
                    return LightColorValues.Color_FF18A2E7; // TODO: Replace this temporary value for a VS Color

                case "NotificationWarningText":
                    return LightColorValues.Color_FF1E1E1E; // TODO: Replace this temporary value for a VS Color
                case "NotificationWarningBackground":
                    return LightColorValues.Color_FFFDFBAC; // TODO: Replace this temporary value for a VS Color
                case "NotificationWarningIcon":
                    return LightColorValues.Color_FF18A2E7; // TODO: Replace this temporary value for a VS Color

                case "NotificationErrorText":
                    return LightColorValues.Color_FF1E1E1E; // TODO: Replace this temporary value for a VS Color
                case "NotificationErrorBackground":
                    return LightColorValues.Color_FFFDFBAC; // TODO: Replace this temporary value for a VS Color
                case "NotificationErrorIcon":
                    return LightColorValues.Color_FF18A2E7; // TODO: Replace this temporary value for a VS Color

                case "DeleteTemplateIcon":
                    return GetColor(WindowsTemplateStudioColors.DeleteTemplateIconColorKey);
                case "SavedTemplateBackgroundHover":
                    return GetColor(WindowsTemplateStudioColors.SavedTemplateBackgroundHoverColorKey);

                case "NewItemFileStatusNewFile":
                    return GetColor(WindowsTemplateStudioColors.NewItemFileStatusNewFileColorKey);
                case "NewItemFileStatusModifiedFile":
                    return GetColor(WindowsTemplateStudioColors.NewItemFileStatusModifiedFileColorKey);
                case "NewItemFileStatusConflictingFile":
                    return GetColor(WindowsTemplateStudioColors.NewItemFileStatusConflictingFileColorKey);
                case "NewItemFileStatusConflictingStylesFile":
                    return GetColor(WindowsTemplateStudioColors.NewItemFileStatusConflictingStylesFileColorKey);
                case "NewItemFileStatusWarningFile":
                    return GetColor(WindowsTemplateStudioColors.NewItemFileStatusWarningFileColorKey);
                case "NewItemFileStatusUnchangedFile":
                    return GetColor(WindowsTemplateStudioColors.NewItemFileStatusUnchangedFileColorKey);

                case "ChangesSummaryDetailFileHeaderBackground":
                    return GetColor(WindowsTemplateStudioColors.ChangesSummaryDetailFileHeaderBackgroundColorKey);

                case "DialogInfoIcon":
                    return GetColor(WindowsTemplateStudioColors.DialogInfoIconColorKey);
                case "DialogErrorIcon":
                    return GetColor(WindowsTemplateStudioColors.DialogErrorIconColorKey);
                case "DialogWarningIcon":
                    return GetColor(WindowsTemplateStudioColors.DialogWarningIconColorKey);

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
