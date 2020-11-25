// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Windows;

namespace Microsoft.Templates.UI.Services
{
    public partial class UIStylesService : DependencyObject
    {
        private readonly BaseStyleValuesProvider _stylesProvider;

        public static UIStylesService Instance { get; private set; }

        public UIStylesService(BaseStyleValuesProvider stylesProvider)
        {
            Instance = this;
            _stylesProvider = stylesProvider;
            _stylesProvider.ThemeChanged += StylesProvider_ThemeChanged;
            SetStyles();
        }

        public void UnsubscribeEventHandlers()
        {
            _stylesProvider.ThemeChanged -= StylesProvider_ThemeChanged;
            _stylesProvider.UnsubscribeEventHandlers();
        }

        private void StylesProvider_ThemeChanged(object sender, System.EventArgs e)
        {
            SetStyles();
        }

        private void SetStyles()
        {
            // VS Colors
            SetCommonDocumentColors();
            SetCommonControlColors();
            SetEnvironmentColors();
            SetInfoBarColors();

            // New Color additions
            SetWindowsTemplateStudioColors();

            // Font Sizes and Font Family
            SetFontProperties();

            // Styles
            SetStyleProperties();
        }

        // VS Colors
        private void SetCommonDocumentColors()
        {
            ListItemText = _stylesProvider.GetColor("CommonDocument", "ListItemText");
            ListItemTextDisabled = _stylesProvider.GetColor("CommonDocument", "ListItemTextDisabled");
        }

        private void SetCommonControlColors()
        {
            Button = _stylesProvider.GetColor("CommonControls", "Button");
            ButtonText = _stylesProvider.GetColor("CommonControls", "ButtonText");
            ButtonBorder = _stylesProvider.GetColor("CommonControls", "ButtonBorder");

            ButtonDefault = _stylesProvider.GetColor("CommonControls", "ButtonDefault");
            ButtonDefaultText = _stylesProvider.GetColor("CommonControls", "ButtonDefaultText");
            ButtonBorderDefault = _stylesProvider.GetColor("CommonControls", "ButtonBorderDefault");

            ButtonDisabled = _stylesProvider.GetColor("CommonControls", "ButtonDisabled");
            ButtonDisabledText = _stylesProvider.GetColor("CommonControls", "ButtonDisabledText");
            ButtonBorderDisabled = _stylesProvider.GetColor("CommonControls", "ButtonBorderDisabled");

            ButtonFocused = _stylesProvider.GetColor("CommonControls", "ButtonFocused");
            ButtonFocusedText = _stylesProvider.GetColor("CommonControls", "ButtonFocusedText");
            ButtonBorderFocused = _stylesProvider.GetColor("CommonControls", "ButtonBorderFocused");

            ButtonHover = _stylesProvider.GetColor("CommonControls", "ButtonHover");
            ButtonHoverText = _stylesProvider.GetColor("CommonControls", "ButtonHoverText");
            ButtonBorderHover = _stylesProvider.GetColor("CommonControls", "ButtonBorderHover");

            ButtonPressed = _stylesProvider.GetColor("CommonControls", "ButtonPressed");
            ButtonPressedText = _stylesProvider.GetColor("CommonControls", "ButtonPressedText");
            ButtonBorderPressed = _stylesProvider.GetColor("CommonControls", "ButtonBorderPressed");

            ////ComboBox Colors
            ComboBoxBackground = _stylesProvider.GetColor("CommonControls", "ComboBoxBackground");
            ComboBoxBackgroundDisabled = _stylesProvider.GetColor("CommonControls", "ComboBoxBackgroundDisabled");
            ComboBoxBackgroundFocused = _stylesProvider.GetColor("CommonControls", "ComboBoxBackgroundFocused");
            ComboBoxBackgroundHover = _stylesProvider.GetColor("CommonControls", "ComboBoxBackgroundHover");
            ComboBoxBackgroundPressed = _stylesProvider.GetColor("CommonControls", "ComboBoxBackgroundPressed");

            ComboBoxBorder = _stylesProvider.GetColor("CommonControls", "ComboBoxBorder");
            ComboBoxBorderDisabled = _stylesProvider.GetColor("CommonControls", "ComboBoxBorderDisabled");
            ComboBoxBorderFocused = _stylesProvider.GetColor("CommonControls", "ComboBoxBorderFocused");
            ComboBoxBorderHover = _stylesProvider.GetColor("CommonControls", "ComboBoxBorderHover");
            ComboBoxBorderPressed = _stylesProvider.GetColor("CommonControls", "ComboBoxBorderPressed");

            ComboBoxGlyph = _stylesProvider.GetColor("CommonControls", "ComboBoxGlyph");
            ComboBoxGlyphBackground = _stylesProvider.GetColor("CommonControls", "ComboBoxGlyphBackground");
            ComboBoxGlyphBackgroundDisabled = _stylesProvider.GetColor("CommonControls", "ComboBoxGlyphBackgroundDisabled");
            ComboBoxGlyphBackgroundFocused = _stylesProvider.GetColor("CommonControls", "ComboBoxGlyphBackgroundFocused");
            ComboBoxGlyphBackgroundHover = _stylesProvider.GetColor("CommonControls", "ComboBoxGlyphBackgroundHover");
            ComboBoxGlyphBackgroundPressed = _stylesProvider.GetColor("CommonControls", "ComboBoxGlyphBackgroundPressed");
            ComboBoxGlyphDisabled = _stylesProvider.GetColor("CommonControls", "ComboBoxGlyphDisabled");
            ComboBoxGlyphFocused = _stylesProvider.GetColor("CommonControls", "ComboBoxGlyphFocused");
            ComboBoxGlyphHover = _stylesProvider.GetColor("CommonControls", "ComboBoxGlyphHover");
            ComboBoxGlyphPressed = _stylesProvider.GetColor("CommonControls", "ComboBoxGlyphPressed");

            ComboBoxListBackground = _stylesProvider.GetColor("CommonControls", "ComboBoxListBackground");
            ComboBoxListBackgroundShadow = _stylesProvider.GetColor("CommonControls", "ComboBoxListBackgroundShadow");
            ComboBoxListBorder = _stylesProvider.GetColor("CommonControls", "ComboBoxListBorder");

            ComboBoxListItemBackgroundHover = _stylesProvider.GetColor("CommonControls", "ComboBoxListItemBackgroundHover");
            ComboBoxListItemBorderHover = _stylesProvider.GetColor("CommonControls", "ComboBoxListItemBorderHover");
            ComboBoxListItemText = _stylesProvider.GetColor("CommonControls", "ComboBoxListItemText");
            ComboBoxListItemTextHover = _stylesProvider.GetColor("CommonControls", "ComboBoxListItemTextHover");

            ComboBoxSelection = _stylesProvider.GetColor("CommonControls", "ComboBoxSelection");

            ComboBoxSeparator = _stylesProvider.GetColor("CommonControls", "ComboBoxSeparator");
            ComboBoxSeparatorDisabled = _stylesProvider.GetColor("CommonControls", "ComboBoxSeparatorDisabled");
            ComboBoxSeparatorFocused = _stylesProvider.GetColor("CommonControls", "ComboBoxSeparatorFocused");
            ComboBoxSeparatorHover = _stylesProvider.GetColor("CommonControls", "ComboBoxSeparatorHover");
            ComboBoxSeparatorPressed = _stylesProvider.GetColor("CommonControls", "ComboBoxSeparatorPressed");

            ComboBoxText = _stylesProvider.GetColor("CommonControls", "ComboBoxText");
            ComboBoxTextDisabled = _stylesProvider.GetColor("CommonControls", "ComboBoxTextDisabled");
            ComboBoxTextFocused = _stylesProvider.GetColor("CommonControls", "ComboBoxTextFocused");
            ComboBoxTextHover = _stylesProvider.GetColor("CommonControls", "ComboBoxTextHover");
            ComboBoxTextInputSelection = _stylesProvider.GetColor("CommonControls", "ComboBoxTextInputSelection");
            ComboBoxTextPressed = _stylesProvider.GetColor("CommonControls", "ComboBoxTextPressed");

            TextBoxBackground = _stylesProvider.GetColor("CommonControls", "TextBoxBackground");
            TextBoxText = _stylesProvider.GetColor("CommonControls", "TextBoxText");
            TextBoxBorder = _stylesProvider.GetColor("CommonControls", "TextBoxBorder");

            TextBoxBackgroundDisabled = _stylesProvider.GetColor("CommonControls", "TextBoxBackgroundDisabled");
            TextBoxBorderDisabled = _stylesProvider.GetColor("CommonControls", "TextBoxBorderDisabled");
            TextBoxTextDisabled = _stylesProvider.GetColor("CommonControls", "TextBoxTextDisabled");

            TextBoxBackgroundFocused = _stylesProvider.GetColor("CommonControls", "TextBoxBackgroundFocused");
            TextBoxBorderFocused = _stylesProvider.GetColor("CommonControls", "TextBoxBorderFocused");
            TextBoxTextFocused = _stylesProvider.GetColor("CommonControls", "TextBoxTextFocused");

            CheckBoxBackground = _stylesProvider.GetColor("CommonControls", "CheckBoxBackground");
            CheckBoxBackgroundDisabled = _stylesProvider.GetColor("CommonControls", "CheckBoxBackgroundDisabled");
            CheckBoxBackgroundFocused = _stylesProvider.GetColor("CommonControls", "CheckBoxBackgroundFocused");
            CheckBoxBackgroundHover = _stylesProvider.GetColor("CommonControls", "CheckBoxBackgroundHover");
            CheckBoxBackgroundPressed = _stylesProvider.GetColor("CommonControls", "CheckBoxBackgroundPressed");
            CheckBoxGlyph = _stylesProvider.GetColor("CommonControls", "CheckBoxGlyph");
            CheckBoxGlyphDisabled = _stylesProvider.GetColor("CommonControls", "CheckBoxGlyphDisabled");
            CheckBoxGlyphFocused = _stylesProvider.GetColor("CommonControls", "CheckBoxGlyphFocused");
            CheckBoxGlyphHover = _stylesProvider.GetColor("CommonControls", "CheckBoxGlyphHover");
            CheckBoxGlyphPressed = _stylesProvider.GetColor("CommonControls", "CheckBoxGlyphPressed");
            CheckBoxBorder = _stylesProvider.GetColor("CommonControls", "CheckBoxBorder");
            CheckBoxBorderDisabled = _stylesProvider.GetColor("CommonControls", "CheckBoxBorderDisabled");
            CheckBoxBorderFocused = _stylesProvider.GetColor("CommonControls", "CheckBoxBorderFocused");
            CheckBoxBorderHover = _stylesProvider.GetColor("CommonControls", "CheckBoxBorderHover");
            CheckBoxBorderPressed = _stylesProvider.GetColor("CommonControls", "CheckBoxBorderPressed");
            CheckBoxText = _stylesProvider.GetColor("CommonControls", "CheckBoxText");
            CheckBoxTextDisabled = _stylesProvider.GetColor("CommonControls", "CheckBoxTextDisabled");
            CheckBoxTextFocused = _stylesProvider.GetColor("CommonControls", "CheckBoxTextFocused");
            CheckBoxTextHover = _stylesProvider.GetColor("CommonControls", "CheckBoxTextHover");
            CheckBoxTextPressed = _stylesProvider.GetColor("CommonControls", "CheckBoxTextPressed");
        }

        private void SetEnvironmentColors()
        {
            PageSideBarExpanderBody = _stylesProvider.GetColor("Environment", "PageSideBarExpanderBody");
            PageSideBarExpanderChevron = _stylesProvider.GetColor("Environment", "PageSideBarExpanderChevron");
            PageSideBarExpanderHeader = _stylesProvider.GetColor("Environment", "PageSideBarExpanderHeader");
            PageSideBarExpanderHeaderHover = _stylesProvider.GetColor("Environment", "PageSideBarExpanderHeaderHover");
            PageSideBarExpanderHeaderPressed = _stylesProvider.GetColor("Environment", "PageSideBarExpanderHeaderPressed");
            PageSideBarExpanderSeparator = _stylesProvider.GetColor("Environment", "PageSideBarExpanderSeparator");
            PageSideBarExpanderText = _stylesProvider.GetColor("Environment", "PageSideBarExpanderText");

            ScrollBarArrowBackground = _stylesProvider.GetColor("Environment", "ScrollBarArrowBackground");
            ScrollBarArrowDisabledBackground = _stylesProvider.GetColor("Environment", "ScrollBarArrowDisabledBackground");
            ScrollBarArrowGlyph = _stylesProvider.GetColor("Environment", "ScrollBarArrowGlyph"); // Used
            ScrollBarArrowGlyphDisabled = _stylesProvider.GetColor("Environment", "ScrollBarArrowGlyphDisabled"); // Used
            ScrollBarArrowGlyphMouseOver = _stylesProvider.GetColor("Environment", "ScrollBarArrowGlyphMouseOver"); // Used
            ScrollBarArrowGlyphPressed = _stylesProvider.GetColor("Environment", "ScrollBarArrowGlyphPressed"); // Used
            ScrollBarArrowMouseOverBackground = _stylesProvider.GetColor("Environment", "ScrollBarArrowMouseOverBackground");
            ScrollBarArrowPressedBackground = _stylesProvider.GetColor("Environment", "ScrollBarArrowPressedBackground");
            ScrollBarBackground = _stylesProvider.GetColor("Environment", "ScrollBarBackground"); // Used
            ScrollBarBorder = _stylesProvider.GetColor("Environment", "ScrollBarBorder"); // Used
            ScrollBarDisabledBackground = _stylesProvider.GetColor("Environment", "ScrollBarDisabledBackground");

            ScrollBarThumbBackground = _stylesProvider.GetColor("Environment", "ScrollBarThumbBackground"); // Used
            ScrollBarThumbBorder = _stylesProvider.GetColor("Environment", "ScrollBarThumbBorder");
            ScrollBarThumbDisabled = _stylesProvider.GetColor("Environment", "ScrollBarThumbDisabled");
            ScrollBarThumbGlyph = _stylesProvider.GetColor("Environment", "ScrollBarThumbGlyph");
            ScrollBarThumbGlyphMouseOverBorder = _stylesProvider.GetColor("Environment", "ScrollBarThumbGlyphMouseOverBorder");
            ScrollBarThumbGlyphPressedBorder = _stylesProvider.GetColor("Environment", "ScrollBarThumbGlyphPressedBorder");
            ScrollBarThumbMouseOverBackground = _stylesProvider.GetColor("Environment", "ScrollBarThumbMouseOverBackground"); // Used
            ScrollBarThumbMouseOverBorder = _stylesProvider.GetColor("Environment", "ScrollBarThumbMouseOverBorder");
            ScrollBarThumbPressedBackground = _stylesProvider.GetColor("Environment", "ScrollBarThumbPressedBackground"); // Used
            ScrollBarThumbPressedBorder = _stylesProvider.GetColor("Environment", "ScrollBarThumbPressedBorder");
        }

        private void SetInfoBarColors()
        {
            IBButton = _stylesProvider.GetColor("InfoBar", "Button");
            IBButtonBorder = _stylesProvider.GetColor("InfoBar", "ButtonBorder");
            IBButtonDisabled = _stylesProvider.GetColor("InfoBar", "ButtonDisabled");
            IBButtonDisabledBorder = _stylesProvider.GetColor("InfoBar", "ButtonDisabledBorder");
            IBButtonFocus = _stylesProvider.GetColor("InfoBar", "ButtonFocus");
            IBButtonFocusBorder = _stylesProvider.GetColor("InfoBar", "ButtonFocusBorder");
            IBButtonMouseDown = _stylesProvider.GetColor("InfoBar", "ButtonMouseDown");
            IBButtonMouseDownBorder = _stylesProvider.GetColor("InfoBar", "ButtonMouseDownBorder");
            IBButtonMouseOver = _stylesProvider.GetColor("InfoBar", "ButtonMouseOver");
            IBButtonMouseOverBorder = _stylesProvider.GetColor("InfoBar", "ButtonMouseOverBorder");
            IBCloseButton = _stylesProvider.GetColor("InfoBar", "CloseButton");
            IBCloseButtonBorder = _stylesProvider.GetColor("InfoBar", "CloseButtonBorder");
            IBCloseButtonDown = _stylesProvider.GetColor("InfoBar", "CloseButtonDown");
            IBCloseButtonDownBorder = _stylesProvider.GetColor("InfoBar", "CloseButtonDownBorder");
            IBCloseButtonDownGlyph = _stylesProvider.GetColor("InfoBar", "CloseButtonDownGlyph");
            IBCloseButtonGlyph = _stylesProvider.GetColor("InfoBar", "CloseButtonGlyph");
            IBCloseButtonHover = _stylesProvider.GetColor("InfoBar", "CloseButtonHover");
            IBCloseButtonHoverBorder = _stylesProvider.GetColor("InfoBar", "CloseButtonHoverBorder");
            IBCloseButtonHoverGlyph = _stylesProvider.GetColor("InfoBar", "CloseButtonHoverGlyph");
            IBInfoBarBackground = _stylesProvider.GetColor("InfoBar", "InfoBarBackground");
            IBInfoBarBackgroundText = _stylesProvider.GetColor("InfoBar", "InfoBarBackgroundText");
            IBInfoBarBorder = _stylesProvider.GetColor("InfoBar", "InfoBarBorder");
        }

        // New Color additions
        private void SetWindowsTemplateStudioColors()
        {
            CardBackgroundDefault = _stylesProvider.GetColor("WindowsTemplateStudio", "CardBackgroundDefault");
            CardBackgroundDisabled = _stylesProvider.GetColor("WindowsTemplateStudio", "CardBackgroundDisabled");
            CardBackgroundHover = _stylesProvider.GetColor("WindowsTemplateStudio", "CardBackgroundHover");
            CardBackgroundSelected = _stylesProvider.GetColor("WindowsTemplateStudio", "CardBackgroundSelected");
            CardBorderDefault = _stylesProvider.GetColor("WindowsTemplateStudio", "CardBorderDefault");
            CardBorderDisabled = _stylesProvider.GetColor("WindowsTemplateStudio", "CardBorderDisabled");
            CardBorderHover = _stylesProvider.GetColor("WindowsTemplateStudio", "CardBorderHover");
            CardBorderSelected = _stylesProvider.GetColor("WindowsTemplateStudio", "CardBorderSelected");
            CardDescriptionText = _stylesProvider.GetColor("WindowsTemplateStudio", "CardDescriptionText");
            CardFooterText = _stylesProvider.GetColor("WindowsTemplateStudio", "CardFooterText");
            CardIcon = _stylesProvider.GetColor("WindowsTemplateStudio", "CardIcon");
            CardTitleText = _stylesProvider.GetColor("WindowsTemplateStudio", "CardTitleText");
            ChangesSummaryDetailFileHeader = _stylesProvider.GetColor("WindowsTemplateStudio", "ChangesSummaryDetailFileHeader");
            ChangesSummaryDetailFileHeaderText = _stylesProvider.GetColor("WindowsTemplateStudio", "ChangesSummaryDetailFileHeaderText");
            DeleteTemplateIcon = _stylesProvider.GetColor("WindowsTemplateStudio", "DeleteTemplateIcon");
            GridHeadingBackground = _stylesProvider.GetColor("WindowsTemplateStudio", "GridHeadingBackground");
            GridHeadingHoverBackground = _stylesProvider.GetColor("WindowsTemplateStudio", "GridHeadingHoverBackground");
            GridHeadingHoverText = _stylesProvider.GetColor("WindowsTemplateStudio", "GridHeadingHoverText");
            GridHeadingText = _stylesProvider.GetColor("WindowsTemplateStudio", "GridHeadingText");
            GridLine = _stylesProvider.GetColor("WindowsTemplateStudio", "GridLine");
            HeaderText = _stylesProvider.GetColor("WindowsTemplateStudio", "HeaderText");
            HeaderTextSecondary = _stylesProvider.GetColor("WindowsTemplateStudio", "HeaderTextSecondary");
            Hyperlink = _stylesProvider.GetColor("WindowsTemplateStudio", "Hyperlink");
            HyperlinkDisabled = _stylesProvider.GetColor("WindowsTemplateStudio", "HyperlinkDisabled");
            HyperlinkHover = _stylesProvider.GetColor("WindowsTemplateStudio", "HyperlinkHover");
            ListItemDisabledText = _stylesProvider.GetColor("WindowsTemplateStudio", "ListItemDisabledText");
            ListItemMouseOver = _stylesProvider.GetColor("WindowsTemplateStudio", "ListItemMouseOver");
            ListItemMouseOverText = _stylesProvider.GetColor("WindowsTemplateStudio", "ListItemMouseOverText");
            NewItemFileStatusConflictingFile = _stylesProvider.GetColor("WindowsTemplateStudio", "NewItemFileStatusConflictingFile");
            NewItemFileStatusConflictingStylesFile = _stylesProvider.GetColor("WindowsTemplateStudio", "NewItemFileStatusConflictingStylesFile");
            NewItemFileStatusModifiedFile = _stylesProvider.GetColor("WindowsTemplateStudio", "NewItemFileStatusModifiedFile");
            NewItemFileStatusNewFile = _stylesProvider.GetColor("WindowsTemplateStudio", "NewItemFileStatusNewFile");
            NewItemFileStatusUnchangedFile = _stylesProvider.GetColor("WindowsTemplateStudio", "NewItemFileStatusUnchangedFile");
            NewItemFileStatusWarningFile = _stylesProvider.GetColor("WindowsTemplateStudio", "NewItemFileStatusWarningFile");
            SavedTemplateBackgroundHover = _stylesProvider.GetColor("WindowsTemplateStudio", "SavedTemplateBackgroundHover");
            SectionDivider = _stylesProvider.GetColor("WindowsTemplateStudio", "SectionDivider");
            SelectedItemActive = _stylesProvider.GetColor("WindowsTemplateStudio", "SelectedItemActive");
            SelectedItemActiveText = _stylesProvider.GetColor("WindowsTemplateStudio", "SelectedItemActiveText");
            SelectedItemInactive = _stylesProvider.GetColor("WindowsTemplateStudio", "SelectedItemInactive");
            SelectedItemInactiveText = _stylesProvider.GetColor("WindowsTemplateStudio", "SelectedItemInactiveText");
            TemplateInfoPageDescription = _stylesProvider.GetColor("WindowsTemplateStudio", "TemplateInfoPageDescription");
            WindowBorder = _stylesProvider.GetColor("WindowsTemplateStudio", "WindowBorder");
            WindowPanel = _stylesProvider.GetColor("WindowsTemplateStudio", "WindowPanel");
            WindowPanelColor = _stylesProvider.GetThemedColor("WindowsTemplateStudio", "WindowPanel");
            WindowPanelText = _stylesProvider.GetColor("WindowsTemplateStudio", "WindowPanelText");
            WindowPanelTextColor = _stylesProvider.GetThemedColor("WindowsTemplateStudio", "WindowPanelText");
            WizardFooter = _stylesProvider.GetColor("WindowsTemplateStudio", "WizardFooter");
            WizardFooterText = _stylesProvider.GetColor("WindowsTemplateStudio", "WizardFooterText");
        }

        // Font Sizes and Font Family
        private void SetFontProperties()
        {
            // FontSizes
            Environment100PercentFontSize = _stylesProvider.GetFontSize("Environment100PercentFontSize");
            Environment111PercentFontSize = _stylesProvider.GetFontSize("Environment111PercentFontSize");
            Environment122PercentFontSize = _stylesProvider.GetFontSize("Environment122PercentFontSize");
            Environment133PercentFontSize = _stylesProvider.GetFontSize("Environment133PercentFontSize");
            Environment155PercentFontSize = _stylesProvider.GetFontSize("Environment155PercentFontSize");
            Environment200PercentFontSize = _stylesProvider.GetFontSize("Environment200PercentFontSize");
            Environment310PercentFontSize = _stylesProvider.GetFontSize("Environment310PercentFontSize");
            Environment330PercentFontSize = _stylesProvider.GetFontSize("Environment330PercentFontSize");
            Environment375PercentFontSize = _stylesProvider.GetFontSize("Environment375PercentFontSize");

            // Font Family
            EnvironmentFontFamily = _stylesProvider.GetFontFamily();
        }

        private void SetStyleProperties()
        {
            FocusVisualStyle = _stylesProvider.GetStyle("FocusVisualStyleKey");
        }
    }
}
