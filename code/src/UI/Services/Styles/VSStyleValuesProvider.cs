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
                case "InfoBar":
                    return GetColorFromInfoBar(memberName);
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
                    return GetColor(ThemedDialogColors.WindowPanelBrushKey);
                case "WindowPanelText":
                    return GetColor(ThemedDialogColors.WindowPanelTextBrushKey);
                case "WindowBorder":
                    return GetColor(ThemedDialogColors.WindowBorderBrushKey);
                case "HeaderText":
                    return GetColor(ThemedDialogColors.HeaderTextBrushKey);
                case "Hyperlink":
                    return GetColor(ThemedDialogColors.HyperlinkBrushKey);
                case "HyperlinkHover":
                    return GetColor(ThemedDialogColors.HyperlinkHoverBrushKey);
                case "HyperlinkPressed":
                    return GetColor(ThemedDialogColors.HyperlinkPressedBrushKey);
                case "HyperlinkDisabled":
                    return GetColor(ThemedDialogColors.HyperlinkDisabledBrushKey);
                case "SelectedItemActive":
                    return GetColor(ThemedDialogColors.SelectedItemActiveBrushKey);
                case "SelectedItemInactive":
                    return GetColor(ThemedDialogColors.SelectedItemInactiveBrushKey);
                case "ListItemMouseOver":
                    return GetColor(ThemedDialogColors.ListItemMouseOverBrushKey);
                case "ListItemDisabledText":
                    return GetColor(ThemedDialogColors.ListItemDisabledTextBrushKey);
                case "GridHeadingBackground":
                    return GetColor(ThemedDialogColors.GridHeadingBackgroundBrushKey);
                case "GridHeadingHoverBackground":
                    return GetColor(ThemedDialogColors.GridHeadingHoverBackgroundBrushKey);
                case "GridHeadingText":
                    return GetColor(ThemedDialogColors.GridHeadingTextBrushKey);
                case "GridHeadingHoverText":
                    return GetColor(ThemedDialogColors.GridHeadingHoverTextBrushKey);
                case "GridLine":
                    return GetColor(ThemedDialogColors.GridLineBrushKey);
                case "SectionDivider":
                    return GetColor(ThemedDialogColors.SectionDividerBrushKey);
                case "WindowButton":
                    return GetColor(ThemedDialogColors.WindowButtonBrushKey);
                case "WindowButtonHover":
                    return GetColor(ThemedDialogColors.WindowButtonHoverBrushKey);
                case "WindowButtonDown":
                    return GetColor(ThemedDialogColors.WindowButtonDownBrushKey);
                case "WindowButtonBorder":
                    return GetColor(ThemedDialogColors.WindowButtonBorderBrushKey);
                case "WindowButtonHoverBorder":
                    return GetColor(ThemedDialogColors.WindowButtonHoverBorderBrushKey);
                case "WindowButtonDownBorder":
                    return GetColor(ThemedDialogColors.WindowButtonDownBorderBrushKey);
                case "WindowButtonGlyph":
                    return GetColor(ThemedDialogColors.WindowButtonGlyphBrushKey);
                case "WindowButtonHoverGlyph":
                    return GetColor(ThemedDialogColors.WindowButtonHoverGlyphBrushKey);
                case "WindowButtonDownGlyph":
                    return GetColor(ThemedDialogColors.WindowButtonDownGlyphBrushKey);
                case "WizardFooter":
                    return GetColor(ThemedDialogColors.WizardFooterBrushKey);
                default:
                    throw new Exception($"The color key value '{memberName}' is not found");
            }
        }

        private Brush GetColorFromCommonDocument(string memberName)
        {
            switch (memberName)
            {
                case "ListItemText":
                    return GetColor(CommonDocumentColors.ListItemTextBrushKey);
                case "ListItemTextDisabled":
                    return GetColor(CommonDocumentColors.ListItemTextDisabledBrushKey);
                default:
                    throw new Exception($"The color key value '{memberName}' is not found");
            }
        }

        private Brush GetColorFromCommonControls(string memberName)
        {
            switch (memberName)
            {
                case "Button":
                    return GetColor(CommonControlsColors.ButtonBrushKey);
                case "ButtonText":
                    return GetColor(CommonControlsColors.ButtonTextBrushKey);
                case "ButtonBorder":
                    return GetColor(CommonControlsColors.ButtonBorderBrushKey);

                case "ButtonDefault":
                    return GetColor(CommonControlsColors.ButtonDefaultBrushKey);
                case "ButtonDefaultText":
                    return GetColor(CommonControlsColors.ButtonDefaultTextBrushKey);
                case "ButtonBorderDefault":
                    return GetColor(CommonControlsColors.ButtonBorderDefaultBrushKey);

                case "ButtonDisabled":
                    return GetColor(CommonControlsColors.ButtonDisabledBrushKey);
                case "ButtonDisabledText":
                    return GetColor(CommonControlsColors.ButtonDisabledTextBrushKey);
                case "ButtonBorderDisabled":
                    return GetColor(CommonControlsColors.ButtonBorderDisabledBrushKey);

                case "ButtonFocused":
                    return GetColor(CommonControlsColors.ButtonFocusedBrushKey);
                case "ButtonFocusedText":
                    return GetColor(CommonControlsColors.ButtonFocusedTextBrushKey);
                case "ButtonBorderFocused":
                    return GetColor(CommonControlsColors.ButtonBorderFocusedBrushKey);

                case "ButtonHover":
                    return GetColor(CommonControlsColors.ButtonHoverBrushKey);
                case "ButtonHoverText":
                    return GetColor(CommonControlsColors.ButtonHoverTextBrushKey);
                case "ButtonBorderHover":
                    return GetColor(CommonControlsColors.ButtonBorderHoverBrushKey);

                case "ButtonPressed":
                    return GetColor(CommonControlsColors.ButtonPressedBrushKey);
                case "ButtonPressedText":
                    return GetColor(CommonControlsColors.ButtonPressedTextBrushKey);
                case "ButtonBorderPressed":
                    return GetColor(CommonControlsColors.ButtonBorderPressedBrushKey);

                case "ComboBoxBackground":
                    return GetColor(CommonControlsColors.ComboBoxBackgroundBrushKey);
                case "ComboBoxBackgroundDisabled":
                    return GetColor(CommonControlsColors.ComboBoxBackgroundDisabledBrushKey);
                case "ComboBoxBackgroundFocused":
                    return GetColor(CommonControlsColors.ComboBoxBackgroundFocusedBrushKey);
                case "ComboBoxBackgroundHover":
                    return GetColor(CommonControlsColors.ComboBoxBackgroundHoverBrushKey);
                case "ComboBoxBackgroundPressed":
                    return GetColor(CommonControlsColors.ComboBoxBackgroundPressedBrushKey);

                case "ComboBoxBorder":
                    return GetColor(CommonControlsColors.ComboBoxBorderBrushKey);
                case "ComboBoxBorderDisabled":
                    return GetColor(CommonControlsColors.ComboBoxBorderDisabledBrushKey);
                case "ComboBoxBorderFocused":
                    return GetColor(CommonControlsColors.ComboBoxBorderFocusedBrushKey);
                case "ComboBoxBorderHover":
                    return GetColor(CommonControlsColors.ComboBoxBorderHoverBrushKey);
                case "ComboBoxBorderPressed":
                    return GetColor(CommonControlsColors.ComboBoxBorderPressedBrushKey);

                case "ComboBoxGlyph":
                    return GetColor(CommonControlsColors.ComboBoxGlyphBrushKey);
                case "ComboBoxGlyphBackground":
                    return GetColor(CommonControlsColors.ComboBoxGlyphBackgroundBrushKey);
                case "ComboBoxGlyphBackgroundDisabled":
                    return GetColor(CommonControlsColors.ComboBoxGlyphBackgroundDisabledBrushKey);
                case "ComboBoxGlyphBackgroundFocused":
                    return GetColor(CommonControlsColors.ComboBoxGlyphBackgroundFocusedBrushKey);
                case "ComboBoxGlyphBackgroundHover":
                    return GetColor(CommonControlsColors.ComboBoxGlyphBackgroundHoverBrushKey);
                case "ComboBoxGlyphBackgroundPressed":
                    return GetColor(CommonControlsColors.ComboBoxGlyphBackgroundPressedBrushKey);
                case "ComboBoxGlyphDisabled":
                    return GetColor(CommonControlsColors.ComboBoxGlyphDisabledBrushKey);
                case "ComboBoxGlyphFocused":
                    return GetColor(CommonControlsColors.ComboBoxGlyphFocusedBrushKey);
                case "ComboBoxGlyphHover":
                    return GetColor(CommonControlsColors.ComboBoxGlyphHoverBrushKey);
                case "ComboBoxGlyphPressed":
                    return GetColor(CommonControlsColors.ComboBoxGlyphPressedBrushKey);

                case "ComboBoxListBackground":
                    return GetColor(CommonControlsColors.ComboBoxListBackgroundBrushKey);
                case "ComboBoxListBackgroundShadow":
                    return GetColor(CommonControlsColors.ComboBoxListBackgroundShadowBrushKey);
                case "ComboBoxListBorder":
                    return GetColor(CommonControlsColors.ComboBoxListBorderBrushKey);
                case "ComboBoxListItemBackgroundHover":
                    return GetColor(CommonControlsColors.ComboBoxListItemBackgroundHoverBrushKey);
                case "ComboBoxListItemBorderHover":
                    return GetColor(CommonControlsColors.ComboBoxListItemBorderHoverBrushKey);
                case "ComboBoxListItemText":
                    return GetColor(CommonControlsColors.ComboBoxListItemTextBrushKey);
                case "ComboBoxListItemTextHover":
                    return GetColor(CommonControlsColors.ComboBoxListItemTextHoverBrushKey);

                case "ComboBoxSelection":
                    return GetColor(CommonControlsColors.ComboBoxSelectionBrushKey);

                case "ComboBoxSeparator":
                    return GetColor(CommonControlsColors.ComboBoxSeparatorBrushKey);
                case "ComboBoxSeparatorDisabled":
                    return GetColor(CommonControlsColors.ComboBoxSeparatorDisabledBrushKey);
                case "ComboBoxSeparatorFocused":
                    return GetColor(CommonControlsColors.ComboBoxSeparatorFocusedBrushKey);
                case "ComboBoxSeparatorHover":
                    return GetColor(CommonControlsColors.ComboBoxSeparatorHoverBrushKey);
                case "ComboBoxSeparatorPressed":
                    return GetColor(CommonControlsColors.ComboBoxSeparatorPressedBrushKey);

                case "ComboBoxText":
                    return GetColor(CommonControlsColors.ComboBoxTextBrushKey);
                case "ComboBoxTextDisabled":
                    return GetColor(CommonControlsColors.ComboBoxTextDisabledBrushKey);
                case "ComboBoxTextFocused":
                    return GetColor(CommonControlsColors.ComboBoxTextFocusedBrushKey);
                case "ComboBoxTextHover":
                    return GetColor(CommonControlsColors.ComboBoxTextHoverBrushKey);
                case "ComboBoxTextInputSelection":
                    return GetColor(CommonControlsColors.ComboBoxTextInputSelectionBrushKey);
                case "ComboBoxTextPressed":
                    return GetColor(CommonControlsColors.ComboBoxTextPressedBrushKey);

                case "TextBoxBackground":
                    return GetColor(CommonControlsColors.TextBoxBackgroundBrushKey);
                case "TextBoxBorder":
                    return GetColor(CommonControlsColors.TextBoxBorderBrushKey);
                case "TextBoxText":
                    return GetColor(CommonControlsColors.TextBoxTextBrushKey);

                case "TextBoxBackgroundDisabled":
                    return GetColor(CommonControlsColors.TextBoxBackgroundDisabledBrushKey);
                case "TextBoxBorderDisabled":
                    return GetColor(CommonControlsColors.TextBoxBorderDisabledBrushKey);
                case "TextBoxTextDisabled":
                    return GetColor(CommonControlsColors.TextBoxTextDisabledBrushKey);

                case "TextBoxBackgroundFocused":
                    return GetColor(CommonControlsColors.TextBoxBackgroundFocusedBrushKey);
                case "TextBoxBorderFocused":
                    return GetColor(CommonControlsColors.TextBoxBorderFocusedBrushKey);
                case "TextBoxTextFocused":
                    return GetColor(CommonControlsColors.TextBoxTextFocusedBrushKey);

                case "CheckBoxBackground":
                    return GetColor(CommonControlsColors.CheckBoxBackgroundBrushKey);
                case "CheckBoxBackgroundDisabled":
                    return GetColor(CommonControlsColors.CheckBoxBackgroundDisabledBrushKey);
                case "CheckBoxBackgroundFocused":
                    return GetColor(CommonControlsColors.CheckBoxBackgroundFocusedBrushKey);
                case "CheckBoxBackgroundHover":
                    return GetColor(CommonControlsColors.CheckBoxBackgroundHoverBrushKey);
                case "CheckBoxBackgroundPressed":
                    return GetColor(CommonControlsColors.CheckBoxBackgroundPressedBrushKey);

                case "CheckBoxGlyph":
                    return GetColor(CommonControlsColors.CheckBoxGlyphBrushKey);
                case "CheckBoxGlyphDisabled":
                    return GetColor(CommonControlsColors.CheckBoxGlyphDisabledBrushKey);
                case "CheckBoxGlyphFocused":
                    return GetColor(CommonControlsColors.CheckBoxGlyphFocusedBrushKey);
                case "CheckBoxGlyphHover":
                    return GetColor(CommonControlsColors.CheckBoxGlyphHoverBrushKey);
                case "CheckBoxGlyphPressed":
                    return GetColor(CommonControlsColors.CheckBoxGlyphPressedBrushKey);

                case "CheckBoxBorder":
                    return GetColor(CommonControlsColors.CheckBoxBorderBrushKey);
                case "CheckBoxBorderDisabled":
                    return GetColor(CommonControlsColors.CheckBoxBorderDisabledBrushKey);
                case "CheckBoxBorderFocused":
                    return GetColor(CommonControlsColors.CheckBoxBorderFocusedBrushKey);
                case "CheckBoxBorderHover":
                    return GetColor(CommonControlsColors.CheckBoxBorderHoverBrushKey);
                case "CheckBoxBorderPressed":
                    return GetColor(CommonControlsColors.CheckBoxBorderPressedBrushKey);

                case "CheckBoxText":
                    return GetColor(CommonControlsColors.CheckBoxTextBrushKey);
                case "CheckBoxTextDisabled":
                    return GetColor(CommonControlsColors.CheckBoxTextDisabledBrushKey);
                case "CheckBoxTextFocused":
                    return GetColor(CommonControlsColors.CheckBoxTextFocusedBrushKey);
                case "CheckBoxTextHover":
                    return GetColor(CommonControlsColors.CheckBoxTextHoverBrushKey);
                case "CheckBoxTextPressed":
                    return GetColor(CommonControlsColors.CheckBoxTextPressedBrushKey);

                default:
                    throw new Exception($"The color key value '{memberName}' is not found");
            }
        }

        private Brush GetColorFromEnvironment(string memberName)
        {
            switch (memberName)
            {
                case "PageSideBarExpanderBody":
                    return GetColor(EnvironmentColors.PageSideBarExpanderBodyBrushKey);
                case "PageSideBarExpanderChevron":
                    return GetColor(EnvironmentColors.PageSideBarExpanderChevronBrushKey);
                case "PageSideBarExpanderHeader":
                    return GetColor(EnvironmentColors.PageSideBarExpanderHeaderBrushKey);
                case "PageSideBarExpanderHeaderHover":
                    return GetColor(EnvironmentColors.PageSideBarExpanderHeaderHoverBrushKey);
                case "PageSideBarExpanderHeaderPressed":
                    return GetColor(EnvironmentColors.PageSideBarExpanderHeaderPressedBrushKey);
                case "PageSideBarExpanderSeparator":
                    return GetColor(EnvironmentColors.PageSideBarExpanderSeparatorBrushKey);
                case "PageSideBarExpanderText":
                    return GetColor(EnvironmentColors.PageSideBarExpanderTextBrushKey);
                default:
                    throw new Exception($"The color key value '{memberName}' is not found");
            }
        }

        private Brush GetColorFromInfoBar(string memberName)
        {
            switch (memberName)
            {
                case "Button":
                    return GetColor(InfoBarColors.ButtonBrushKey);
                case "ButtonBorder":
                    return GetColor(InfoBarColors.ButtonBorderBrushKey);
                case "ButtonDisabled":
                    return GetColor(InfoBarColors.ButtonDisabledBrushKey);
                case "ButtonDisabledBorder":
                    return GetColor(InfoBarColors.ButtonDisabledBorderBrushKey);
                case "ButtonFocus":
                    return GetColor(InfoBarColors.ButtonFocusBrushKey);
                case "ButtonFocusBorder":
                    return GetColor(InfoBarColors.ButtonFocusBorderBrushKey);
                case "ButtonMouseDown":
                    return GetColor(InfoBarColors.ButtonMouseDownBrushKey);
                case "ButtonMouseDownBorder":
                    return GetColor(InfoBarColors.ButtonMouseDownBorderBrushKey);
                case "ButtonMouseOver":
                    return GetColor(InfoBarColors.ButtonMouseOverBrushKey);
                case "ButtonMouseOverBorder":
                    return GetColor(InfoBarColors.ButtonMouseOverBorderBrushKey);
                case "CloseButton":
                    return GetColor(InfoBarColors.CloseButtonBrushKey);
                case "CloseButtonBorder":
                    return GetColor(InfoBarColors.CloseButtonBorderBrushKey);
                case "CloseButtonDown":
                    return GetColor(InfoBarColors.CloseButtonDownBrushKey);
                case "CloseButtonDownBorder":
                    return GetColor(InfoBarColors.CloseButtonDownBorderBrushKey);
                case "CloseButtonDownGlyph":
                    return GetColor(InfoBarColors.CloseButtonDownGlyphBrushKey);
                case "CloseButtonGlyph":
                    return GetColor(InfoBarColors.CloseButtonGlyphBrushKey);
                case "CloseButtonHover":
                    return GetColor(InfoBarColors.CloseButtonHoverBrushKey);
                case "CloseButtonHoverBorder":
                    return GetColor(InfoBarColors.CloseButtonHoverBorderBrushKey);
                case "CloseButtonHoverGlyph":
                    return GetColor(InfoBarColors.CloseButtonHoverGlyphBrushKey);
                case "InfoBarBackground":
                    return GetColor(InfoBarColors.InfoBarBackgroundBrushKey);
                case "InfoBarBackgroundText":
                    return GetColor(InfoBarColors.InfoBarBackgroundTextBrushKey);
                case "InfoBarBorder":
                    return GetColor(InfoBarColors.InfoBarBorderBrushKey);
                default:
                    throw new Exception($"The color key value '{memberName}' is not found");
            }
        }

        private Brush GetColorFromWindowsTemplateStudio(string memberName)
        {
            switch (memberName)
            {
                case "CardTitleText":
                    return GetColor(WindowsTemplateStudioColors.CardTitleTextBrushKey);
                case "CardDescriptionText":
                    return GetColor(WindowsTemplateStudioColors.CardDescriptionTextBrushKey);
                case "CardBackgroundDefault":
                    return GetColor(WindowsTemplateStudioColors.CardBackgroundDefaultBrushKey);
                case "CardBackgroundFocus":
                    return GetColor(WindowsTemplateStudioColors.CardBackgroundFocusBrushKey);
                case "CardBackgroundHover":
                    return GetColor(WindowsTemplateStudioColors.CardBackgroundHoverBrushKey);
                case "CardBackgroundPressed":
                    return GetColor(WindowsTemplateStudioColors.CardBackgroundPressedBrushKey);
                case "CardBackgroundSelected":
                    return GetColor(WindowsTemplateStudioColors.CardBackgroundSelectedBrushKey);
                case "CardBackgroundDisabled":
                    return GetColor(WindowsTemplateStudioColors.CardBackgroundDisabledBrushKey);
                case "CardBorderDefault":
                    return GetColor(WindowsTemplateStudioColors.CardBorderDefaultBrushKey);
                case "CardBorderFocus":
                    return GetColor(WindowsTemplateStudioColors.CardBorderFocusBrushKey);
                case "CardBorderHover":
                    return GetColor(WindowsTemplateStudioColors.CardBorderHoverBrushKey);
                case "CardBorderPressed":
                    return GetColor(WindowsTemplateStudioColors.CardBorderPressedBrushKey);
                case "CardBorderSelected":
                    return GetColor(WindowsTemplateStudioColors.CardBorderSelectedBrushKey);
                case "CardBorderDisabled":
                    return GetColor(WindowsTemplateStudioColors.CardBorderDisabledBrushKey);
                case "CardIcon":
                    return GetColor(WindowsTemplateStudioColors.CardIconBrushKey);
                case "CardFooterText":
                    return GetColor(WindowsTemplateStudioColors.CardFooterTextBrushKey);

                case "DeleteTemplateIcon":
                    return GetColor(WindowsTemplateStudioColors.DeleteTemplateIconBrushKey);
                case "SavedTemplateBackgroundHover":
                    return GetColor(WindowsTemplateStudioColors.SavedTemplateBackgroundHoverBrushKey);

                case "NewItemFileStatusNewFile":
                    return GetColor(WindowsTemplateStudioColors.NewItemFileStatusNewFileBrushKey);
                case "NewItemFileStatusModifiedFile":
                    return GetColor(WindowsTemplateStudioColors.NewItemFileStatusModifiedFileBrushKey);
                case "NewItemFileStatusConflictingFile":
                    return GetColor(WindowsTemplateStudioColors.NewItemFileStatusConflictingFileBrushKey);
                case "NewItemFileStatusConflictingStylesFile":
                    return GetColor(WindowsTemplateStudioColors.NewItemFileStatusConflictingStylesFileBrushKey);
                case "NewItemFileStatusWarningFile":
                    return GetColor(WindowsTemplateStudioColors.NewItemFileStatusWarningFileBrushKey);
                case "NewItemFileStatusUnchangedFile":
                    return GetColor(WindowsTemplateStudioColors.NewItemFileStatusUnchangedFileBrushKey);

                case "ChangesSummaryDetailFileHeaderBackground":
                    return GetColor(WindowsTemplateStudioColors.ChangesSummaryDetailFileHeaderBackgroundBrushKey);

                case "DialogInfoIcon":
                    return GetColor(WindowsTemplateStudioColors.DialogInfoIconBrushKey);
                case "DialogErrorIcon":
                    return GetColor(WindowsTemplateStudioColors.DialogErrorIconBrushKey);
                case "DialogWarningIcon":
                    return GetColor(WindowsTemplateStudioColors.DialogWarningIconBrushKey);

                case "HeaderTextSecondary":
                    return GetColor(WindowsTemplateStudioColors.HeaderTextSecondaryBrushKey);
                case "WizardFooterText":
                    return GetColor(WindowsTemplateStudioColors.WizardFooterTextBrushKey);

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
