// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Windows;
using System.Windows.Media;
using Microsoft.Templates.UI.Services;

namespace Microsoft.Templates.VsEmulator.Services
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
                case "InfoBar":
                    return GetColorFromInfoBar(memberName);
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
                    return GetColor(ThemedDialog.WindowPanelBrushKey);
                case "WindowPanelText":
                    return GetColor(ThemedDialog.WindowPanelTextBrushKey);
                case "WindowBorder":
                    return GetColor(ThemedDialog.WindowBorderBrushKey);
                case "HeaderText":
                    return GetColor(ThemedDialog.HeaderTextBrushKey);
                case "Hyperlink":
                    return GetColor(ThemedDialog.HyperlinkBrushKey);
                case "HyperlinkHover":
                    return GetColor(ThemedDialog.HyperlinkHoverBrushKey);
                case "HyperlinkPressed":
                    return GetColor(ThemedDialog.HyperlinkPressedBrushKey);
                case "HyperlinkDisabled":
                    return GetColor(ThemedDialog.HyperlinkDisabledBrushKey);
                case "SelectedItemActive":
                    return GetColor(ThemedDialog.SelectedItemActiveBrushKey);
                case "SelectedItemInactive":
                    return GetColor(ThemedDialog.SelectedItemInactiveBrushKey);
                case "ListItemMouseOver":
                    return GetColor(ThemedDialog.ListItemMouseOverBrushKey);
                case "ListItemDisabledText":
                    return GetColor(ThemedDialog.ListItemDisabledTextBrushKey);
                case "GridHeadingBackground":
                    return GetColor(ThemedDialog.GridHeadingBackgroundBrushKey);
                case "GridHeadingHoverBackground":
                    return GetColor(ThemedDialog.GridHeadingHoverBackgroundBrushKey);
                case "GridHeadingText":
                    return GetColor(ThemedDialog.GridHeadingTextBrushKey);
                case "GridHeadingHoverText":
                    return GetColor(ThemedDialog.GridHeadingHoverTextBrushKey);
                case "GridLine":
                    return GetColor(ThemedDialog.GridLineBrushKey);
                case "SectionDivider":
                    return GetColor(ThemedDialog.SectionDividerBrushKey);
                case "WindowButton":
                    return GetColor(ThemedDialog.WindowButtonBrushKey);
                case "WindowButtonHover":
                    return GetColor(ThemedDialog.WindowButtonHoverBrushKey);
                case "WindowButtonDown":
                    return GetColor(ThemedDialog.WindowButtonDownBrushKey);
                case "WindowButtonBorder":
                    return GetColor(ThemedDialog.WindowButtonBorderBrushKey);
                case "WindowButtonHoverBorder":
                    return GetColor(ThemedDialog.WindowButtonHoverBorderBrushKey);
                case "WindowButtonDownBorder":
                    return GetColor(ThemedDialog.WindowButtonDownBorderBrushKey);
                case "WindowButtonGlyph":
                    return GetColor(ThemedDialog.WindowButtonGlyphBrushKey);
                case "WindowButtonHoverGlyph":
                    return GetColor(ThemedDialog.WindowButtonHoverGlyphBrushKey);
                case "WindowButtonDownGlyph":
                    return GetColor(ThemedDialog.WindowButtonDownGlyphBrushKey);
                case "WizardFooter":
                    return GetColor(ThemedDialog.WizardFooterBrushKey);

                default:
                    throw new Exception($"The color key value '{memberName}' is not found");
            }
        }

        private Brush GetColorFromCommonDocument(string memberName)
        {
            switch (memberName)
            {
                case "ListItemText":
                    return GetColor(CommonDocument.ListItemTextBrushKey);
                case "ListItemTextDisabled":
                    return GetColor(CommonDocument.ListItemTextDisabledBrushKey);
                default:
                    throw new Exception($"The color key value '{memberName}' is not found");
            }
        }

        private Brush GetColorFromCommonControls(string memberName)
        {
            switch (memberName)
            {
                case "Button":
                    return GetColor(CommonControls.ButtonBrushKey);
                case "ButtonText":
                    return GetColor(CommonControls.ButtonTextBrushKey);
                case "ButtonBorder":
                    return GetColor(CommonControls.ButtonBorderBrushKey);

                case "ButtonDefault":
                    return GetColor(CommonControls.ButtonDefaultBrushKey);
                case "ButtonDefaultText":
                    return GetColor(CommonControls.ButtonDefaultTextBrushKey);
                case "ButtonBorderDefault":
                    return GetColor(CommonControls.ButtonBorderDefaultBrushKey);

                case "ButtonDisabled":
                    return GetColor(CommonControls.ButtonDisabledBrushKey);
                case "ButtonDisabledText":
                    return GetColor(CommonControls.ButtonDisabledTextBrushKey);
                case "ButtonBorderDisabled":
                    return GetColor(CommonControls.ButtonBorderDisabledBrushKey);

                case "ButtonFocused":
                    return GetColor(CommonControls.ButtonFocusedBrushKey);
                case "ButtonFocusedText":
                    return GetColor(CommonControls.ButtonFocusedTextBrushKey);
                case "ButtonBorderFocused":
                    return GetColor(CommonControls.ButtonBorderFocusedBrushKey);

                case "ButtonHover":
                    return GetColor(CommonControls.ButtonHoverBrushKey);
                case "ButtonHoverText":
                    return GetColor(CommonControls.ButtonHoverTextBrushKey);
                case "ButtonBorderHover":
                    return GetColor(CommonControls.ButtonBorderHoverBrushKey);

                case "ButtonPressed":
                    return GetColor(CommonControls.ButtonPressedBrushKey);
                case "ButtonPressedText":
                    return GetColor(CommonControls.ButtonPressedTextBrushKey);
                case "ButtonBorderPressed":
                    return GetColor(CommonControls.ButtonBorderPressedBrushKey);

                case "ComboBoxBackground":
                    return GetColor(CommonControls.ComboBoxBackgroundBrushKey);
                case "ComboBoxBackgroundDisabled":
                    return GetColor(CommonControls.ComboBoxBackgroundDisabledBrushKey);
                case "ComboBoxBackgroundFocused":
                    return GetColor(CommonControls.ComboBoxBackgroundFocusedBrushKey);
                case "ComboBoxBackgroundHover":
                    return GetColor(CommonControls.ComboBoxBackgroundHoverBrushKey);
                case "ComboBoxBackgroundPressed":
                    return GetColor(CommonControls.ComboBoxBackgroundPressedBrushKey);

                case "ComboBoxBorder":
                    return GetColor(CommonControls.ComboBoxBorderBrushKey);
                case "ComboBoxBorderDisabled":
                    return GetColor(CommonControls.ComboBoxBorderDisabledBrushKey);
                case "ComboBoxBorderFocused":
                    return GetColor(CommonControls.ComboBoxBorderFocusedBrushKey);
                case "ComboBoxBorderHover":
                    return GetColor(CommonControls.ComboBoxBorderHoverBrushKey);
                case "ComboBoxBorderPressed":
                    return GetColor(CommonControls.ComboBoxBorderPressedBrushKey);

                case "ComboBoxGlyph":
                    return GetColor(CommonControls.ComboBoxGlyphBrushKey);
                case "ComboBoxGlyphBackground":
                    return GetColor(CommonControls.ComboBoxGlyphBackgroundBrushKey);
                case "ComboBoxGlyphBackgroundDisabled":
                    return GetColor(CommonControls.ComboBoxGlyphBackgroundDisabledBrushKey);
                case "ComboBoxGlyphBackgroundFocused":
                    return GetColor(CommonControls.ComboBoxGlyphBackgroundFocusedBrushKey);
                case "ComboBoxGlyphBackgroundHover":
                    return GetColor(CommonControls.ComboBoxGlyphBackgroundHoverBrushKey);
                case "ComboBoxGlyphBackgroundPressed":
                    return GetColor(CommonControls.ComboBoxGlyphBackgroundPressedBrushKey);
                case "ComboBoxGlyphDisabled":
                    return GetColor(CommonControls.ComboBoxGlyphDisabledBrushKey);
                case "ComboBoxGlyphFocused":
                    return GetColor(CommonControls.ComboBoxGlyphFocusedBrushKey);
                case "ComboBoxGlyphHover":
                    return GetColor(CommonControls.ComboBoxGlyphHoverBrushKey);
                case "ComboBoxGlyphPressed":
                    return GetColor(CommonControls.ComboBoxGlyphPressedBrushKey);

                case "ComboBoxListBackground":
                    return GetColor(CommonControls.ComboBoxListBackgroundBrushKey);
                case "ComboBoxListBackgroundShadow":
                    return GetColor(CommonControls.ComboBoxListBackgroundShadowBrushKey);
                case "ComboBoxListBorder":
                    return GetColor(CommonControls.ComboBoxListBorderBrushKey);
                case "ComboBoxListItemBackgroundHover":
                    return GetColor(CommonControls.ComboBoxListItemBackgroundHoverBrushKey);
                case "ComboBoxListItemBorderHover":
                    return GetColor(CommonControls.ComboBoxListItemBorderHoverBrushKey);
                case "ComboBoxListItemText":
                    return GetColor(CommonControls.ComboBoxListItemTextBrushKey);
                case "ComboBoxListItemTextHover":
                    return GetColor(CommonControls.ComboBoxListItemTextHoverBrushKey);

                case "ComboBoxSelection":
                    return GetColor(CommonControls.ComboBoxSelectionBrushKey);

                case "ComboBoxSeparator":
                    return GetColor(CommonControls.ComboBoxSeparatorBrushKey);
                case "ComboBoxSeparatorDisabled":
                    return GetColor(CommonControls.ComboBoxSeparatorDisabledBrushKey);
                case "ComboBoxSeparatorFocused":
                    return GetColor(CommonControls.ComboBoxSeparatorFocusedBrushKey);
                case "ComboBoxSeparatorHover":
                    return GetColor(CommonControls.ComboBoxSeparatorHoverBrushKey);
                case "ComboBoxSeparatorPressed":
                    return GetColor(CommonControls.ComboBoxSeparatorPressedBrushKey);

                case "ComboBoxText":
                    return GetColor(CommonControls.ComboBoxTextBrushKey);
                case "ComboBoxTextDisabled":
                    return GetColor(CommonControls.ComboBoxTextDisabledBrushKey);
                case "ComboBoxTextFocused":
                    return GetColor(CommonControls.ComboBoxTextFocusedBrushKey);
                case "ComboBoxTextHover":
                    return GetColor(CommonControls.ComboBoxTextHoverBrushKey);
                case "ComboBoxTextInputSelection":
                    return GetColor(CommonControls.ComboBoxTextInputSelectionBrushKey);
                case "ComboBoxTextPressed":
                    return GetColor(CommonControls.ComboBoxTextPressedBrushKey);

                case "TextBoxBackground":
                    return GetColor(CommonControls.TextBoxBackgroundBrushKey);
                case "TextBoxBorder":
                    return GetColor(CommonControls.TextBoxBorderBrushKey);
                case "TextBoxText":
                    return GetColor(CommonControls.TextBoxTextBrushKey);

                case "TextBoxBackgroundDisabled":
                    return GetColor(CommonControls.TextBoxBackgroundDisabledBrushKey);
                case "TextBoxBorderDisabled":
                    return GetColor(CommonControls.TextBoxBorderDisabledBrushKey);
                case "TextBoxTextDisabled":
                    return GetColor(CommonControls.TextBoxTextDisabledBrushKey);

                case "TextBoxBackgroundFocused":
                    return GetColor(CommonControls.TextBoxBackgroundFocusedBrushKey);
                case "TextBoxBorderFocused":
                    return GetColor(CommonControls.TextBoxBorderFocusedBrushKey);
                case "TextBoxTextFocused":
                    return GetColor(CommonControls.TextBoxTextFocusedBrushKey);

                case "CheckBoxBackground":
                    return GetColor(CommonControls.CheckBoxBackgroundBrushKey);
                case "CheckBoxBackgroundDisabled":
                    return GetColor(CommonControls.CheckBoxBackgroundDisabledBrushKey);
                case "CheckBoxBackgroundFocused":
                    return GetColor(CommonControls.CheckBoxBackgroundFocusedBrushKey);
                case "CheckBoxBackgroundHover":
                    return GetColor(CommonControls.CheckBoxBackgroundHoverBrushKey);
                case "CheckBoxBackgroundPressed":
                    return GetColor(CommonControls.CheckBoxBackgroundPressedBrushKey);

                case "CheckBoxGlyph":
                    return GetColor(CommonControls.CheckBoxGlyphBrushKey);
                case "CheckBoxGlyphDisabled":
                    return GetColor(CommonControls.CheckBoxGlyphDisabledBrushKey);
                case "CheckBoxGlyphFocused":
                    return GetColor(CommonControls.CheckBoxGlyphFocusedBrushKey);
                case "CheckBoxGlyphHover":
                    return GetColor(CommonControls.CheckBoxGlyphHoverBrushKey);
                case "CheckBoxGlyphPressed":
                    return GetColor(CommonControls.CheckBoxGlyphPressedBrushKey);

                case "CheckBoxBorder":
                    return GetColor(CommonControls.CheckBoxBorderBrushKey);
                case "CheckBoxBorderDisabled":
                    return GetColor(CommonControls.CheckBoxBorderDisabledBrushKey);
                case "CheckBoxBorderFocused":
                    return GetColor(CommonControls.CheckBoxBorderFocusedBrushKey);
                case "CheckBoxBorderHover":
                    return GetColor(CommonControls.CheckBoxBorderHoverBrushKey);
                case "CheckBoxBorderPressed":
                    return GetColor(CommonControls.CheckBoxBorderPressedBrushKey);

                case "CheckBoxText":
                    return GetColor(CommonControls.CheckBoxTextBrushKey);
                case "CheckBoxTextDisabled":
                    return GetColor(CommonControls.CheckBoxTextDisabledBrushKey);
                case "CheckBoxTextFocused":
                    return GetColor(CommonControls.CheckBoxTextFocusedBrushKey);
                case "CheckBoxTextHover":
                    return GetColor(CommonControls.CheckBoxTextHoverBrushKey);
                case "CheckBoxTextPressed":
                    return GetColor(CommonControls.CheckBoxTextPressedBrushKey);

                default:
                    throw new Exception($"The color key value '{memberName}' is not found");
            }
        }

        private Brush GetColorFromEnvironment(string memberName)
        {
            switch (memberName)
            {
                case "PageSideBarExpanderBody":
                    return GetColor(Environment.PageSideBarExpanderBodyBrushKey);
                case "PageSideBarExpanderChevron":
                    return GetColor(Environment.PageSideBarExpanderChevronBrushKey);
                case "PageSideBarExpanderHeader":
                    return GetColor(Environment.PageSideBarExpanderHeaderBrushKey);
                case "PageSideBarExpanderHeaderHover":
                    return GetColor(Environment.PageSideBarExpanderHeaderHoverBrushKey);
                case "PageSideBarExpanderHeaderPressed":
                    return GetColor(Environment.PageSideBarExpanderHeaderPressedBrushKey);
                case "PageSideBarExpanderSeparator":
                    return GetColor(Environment.PageSideBarExpanderSeparatorBrushKey);
                case "PageSideBarExpanderText":
                    return GetColor(Environment.PageSideBarExpanderTextBrushKey);

                case "SystemScrollBar":
                    return GetColor(Environment.ScrollBarBrushKey);
                case "ScrollBarArrowBackground":
                    return GetColor(Environment.ScrollBarArrowBackgroundBrushKey);
                case "ScrollBarArrowDisabledBackground":
                    return GetColor(Environment.ScrollBarArrowDisabledBackgroundBrushKey);
                case "ScrollBarArrowGlyph":
                    return GetColor(Environment.ScrollBarArrowGlyphBrushKey);
                case "ScrollBarArrowGlyphDisabled":
                    return GetColor(Environment.ScrollBarArrowGlyphDisabledBrushKey);
                case "ScrollBarArrowGlyphMouseOver":
                    return GetColor(Environment.ScrollBarArrowGlyphMouseOverBrushKey);
                case "ScrollBarArrowGlyphPressed":
                    return GetColor(Environment.ScrollBarArrowGlyphPressedBrushKey);
                case "ScrollBarArrowMouseOverBackground":
                    return GetColor(Environment.ScrollBarArrowMouseOverBackgroundBrushKey);
                case "ScrollBarArrowPressedBackground":
                    return GetColor(Environment.ScrollBarArrowPressedBackgroundBrushKey);
                case "ScrollBarBackground":
                    return GetColor(Environment.ScrollBarBackgroundBrushKey);
                case "ScrollBarBorder":
                    return GetColor(Environment.ScrollBarBorderBrushKey);
                case "ScrollBarDisabledBackground":
                    return GetColor(Environment.ScrollBarDisabledBackgroundBrushKey);
                case "ScrollBarThumbBackground":
                    return GetColor(Environment.ScrollBarThumbBackgroundBrushKey);
                case "ScrollBarThumbBorder":
                    return GetColor(Environment.ScrollBarThumbBorderBrushKey);
                case "ScrollBarThumbDisabled":
                    return GetColor(Environment.ScrollBarThumbDisabledBrushKey);
                case "ScrollBarThumbGlyph":
                    return GetColor(Environment.ScrollBarThumbGlyphBrushKey);
                case "ScrollBarThumbGlyphMouseOverBorder":
                    return GetColor(Environment.ScrollBarThumbGlyphMouseOverBorderBrushKey);
                case "ScrollBarThumbGlyphPressedBorder":
                    return GetColor(Environment.ScrollBarThumbGlyphPressedBorderBrushKey);
                case "ScrollBarThumbMouseOverBackground":
                    return GetColor(Environment.ScrollBarThumbMouseOverBackgroundBrushKey);
                case "ScrollBarThumbMouseOverBorder":
                    return GetColor(Environment.ScrollBarThumbMouseOverBorderBrushKey);
                case "ScrollBarThumbPressedBackground":
                    return GetColor(Environment.ScrollBarThumbPressedBackgroundBrushKey);
                case "ScrollBarThumbPressedBorder":
                    return GetColor(Environment.ScrollBarThumbPressedBorderBrushKey);
                default:
                    throw new Exception($"The color key value '{memberName}' is not found");
            }
        }

        private Brush GetColorFromInfoBar(string memberName)
        {
            switch (memberName)
            {
                case "Button":
                    return GetColor(InfoBar.ButtonBrushKey);
                case "ButtonBorder":
                    return GetColor(InfoBar.ButtonBorderBrushKey);
                case "ButtonDisabled":
                    return GetColor(InfoBar.ButtonDisabledBrushKey);
                case "ButtonDisabledBorder":
                    return GetColor(InfoBar.ButtonDisabledBorderBrushKey);
                case "ButtonFocus":
                    return GetColor(InfoBar.ButtonFocusBrushKey);
                case "ButtonFocusBorder":
                    return GetColor(InfoBar.ButtonFocusBorderBrushKey);
                case "ButtonMouseDown":
                    return GetColor(InfoBar.ButtonMouseDownBrushKey);
                case "ButtonMouseDownBorder":
                    return GetColor(InfoBar.ButtonMouseDownBorderBrushKey);
                case "ButtonMouseOver":
                    return GetColor(InfoBar.ButtonMouseOverBrushKey);
                case "ButtonMouseOverBorder":
                    return GetColor(InfoBar.ButtonMouseOverBorderBrushKey);
                case "CloseButton":
                    return GetColor(InfoBar.CloseButtonBrushKey);
                case "CloseButtonBorder":
                    return GetColor(InfoBar.CloseButtonBorderBrushKey);
                case "CloseButtonDown":
                    return GetColor(InfoBar.CloseButtonDownBrushKey);
                case "CloseButtonDownBorder":
                    return GetColor(InfoBar.CloseButtonDownBorderBrushKey);
                case "CloseButtonDownGlyph":
                    return GetColor(InfoBar.CloseButtonDownGlyphBrushKey);
                case "CloseButtonGlyph":
                    return GetColor(InfoBar.CloseButtonGlyphBrushKey);
                case "CloseButtonHover":
                    return GetColor(InfoBar.CloseButtonHoverBrushKey);
                case "CloseButtonHoverBorder":
                    return GetColor(InfoBar.CloseButtonHoverBorderBrushKey);
                case "CloseButtonHoverGlyph":
                    return GetColor(InfoBar.CloseButtonHoverGlyphBrushKey);
                case "InfoBarBackground":
                    return GetColor(InfoBar.InfoBarBackgroundBrushKey);
                case "InfoBarBackgroundText":
                    return GetColor(InfoBar.InfoBarBackgroundTextBrushKey);
                case "InfoBarBorder":
                    return GetColor(InfoBar.InfoBarBorderBrushKey);
                default:
                    throw new Exception($"The color key value '{memberName}' is not found");
            }
        }

        private Brush GetColorFromWindowsTemplateStudio(string memberName)
        {
            switch (memberName)
            {
                case "CardTitleText":
                    return GetColor(WindowsTemplateStudio.CardTitleTextBrushKey);
                case "CardDescriptionText":
                    return GetColor(WindowsTemplateStudio.CardDescriptionTextBrushKey);
                case "CardBackgroundDefault":
                    return GetColor(WindowsTemplateStudio.CardBackgroundDefaultBrushKey);
                case "CardBackgroundFocus":
                    return GetColor(WindowsTemplateStudio.CardBackgroundFocusBrushKey);
                case "CardBackgroundHover":
                    return GetColor(WindowsTemplateStudio.CardBackgroundHoverBrushKey);
                case "CardBackgroundPressed":
                    return GetColor(WindowsTemplateStudio.CardBackgroundPressedBrushKey);
                case "CardBackgroundSelected":
                    return GetColor(WindowsTemplateStudio.CardBackgroundSelectedBrushKey);
                case "CardBackgroundDisabled":
                    return GetColor(WindowsTemplateStudio.CardBackgroundDisabledBrushKey);
                case "CardBorderDefault":
                    return GetColor(WindowsTemplateStudio.CardBorderDefaultBrushKey);
                case "CardBorderFocus":
                    return GetColor(WindowsTemplateStudio.CardBorderFocusBrushKey);
                case "CardBorderHover":
                    return GetColor(WindowsTemplateStudio.CardBorderHoverBrushKey);
                case "CardBorderPressed":
                    return GetColor(WindowsTemplateStudio.CardBorderPressedBrushKey);
                case "CardBorderSelected":
                    return GetColor(WindowsTemplateStudio.CardBorderSelectedBrushKey);
                case "CardBorderDisabled":
                    return GetColor(WindowsTemplateStudio.CardBorderDisabledBrushKey);
                case "CardIcon":
                    return GetColor(WindowsTemplateStudio.CardIconBrushKey);
                case "CardFooterText":
                    return GetColor(WindowsTemplateStudio.CardFooterTextBrushKey);

                case "DeleteTemplateIcon":
                    return GetColor(WindowsTemplateStudio.DeleteTemplateIconBrushKey);
                case "SavedTemplateBackgroundHover":
                    return GetColor(WindowsTemplateStudio.SavedTemplateBackgroundHoverBrushKey);

                case "NewItemFileStatusNewFile":
                    return GetColor(WindowsTemplateStudio.NewItemFileStatusNewFileBrushKey);
                case "NewItemFileStatusModifiedFile":
                    return GetColor(WindowsTemplateStudio.NewItemFileStatusModifiedFileBrushKey);
                case "NewItemFileStatusConflictingFile":
                    return GetColor(WindowsTemplateStudio.NewItemFileStatusConflictingFileBrushKey);
                case "NewItemFileStatusConflictingStylesFile":
                    return GetColor(WindowsTemplateStudio.NewItemFileStatusConflictingStylesFileBrushKey);
                case "NewItemFileStatusWarningFile":
                    return GetColor(WindowsTemplateStudio.NewItemFileStatusWarningFileBrushKey);
                case "NewItemFileStatusUnchangedFile":
                    return GetColor(WindowsTemplateStudio.NewItemFileStatusUnchangedFileBrushKey);

                case "ChangesSummaryDetailFileHeaderBackground":
                    return GetColor(WindowsTemplateStudio.ChangesSummaryDetailFileHeaderBackgroundBrushKey);

                case "DialogInfoIcon":
                    return GetColor(WindowsTemplateStudio.DialogInfoIconBrushKey);
                case "DialogInfoCharIcon":
                    return GetColor(WindowsTemplateStudio.DialogInfoCharIconBrushKey);
                case "DialogErrorIcon":
                    return GetColor(WindowsTemplateStudio.DialogErrorIconBrushKey);
                case "DialogWarningIcon":
                    return GetColor(WindowsTemplateStudio.DialogWarningIconBrushKey);

                case "HeaderTextSecondary":
                    return GetColor(WindowsTemplateStudio.HeaderTextSecondaryBrushKey);
                case "WizardFooterText":
                    return GetColor(WindowsTemplateStudio.WizardFooterTextBrushKey);

                default:
                    throw new Exception($"The color key value '{memberName}' is not found");
            }
        }

        public SolidColorBrush GetColor(string resourceKey) => Application.Current.FindResource(resourceKey) as SolidColorBrush;
    }
}
