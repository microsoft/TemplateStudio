// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Windows;

namespace Microsoft.Templates.UI.V2Services
{
    public partial class UIStylesService : DependencyObject
    {
        private IStyleValuesProvider _stylesProvider;

        private static UIStylesService _instance;

        public static UIStylesService Instance => _instance ?? (_instance = new UIStylesService());

        public UIStylesService()
        {
        }

        public void Initialize(IStyleValuesProvider stylesProvider)
        {
            _stylesProvider = stylesProvider;
            _stylesProvider.ThemeChanged += StylesProvider_ThemeChanged;
            SetStyles();
        }

        private void StylesProvider_ThemeChanged(object sender, System.EventArgs e)
        {
            SetStyles();
        }

        private void SetStyles()
        {
            // VS Colors
            SetThemedDialogColors();
            SetCommonDocumentColors();
            SetCommonControlColors();
            SetEnvironmentColors();

            // New Color additions
            SetThemedCardColors();
            SetWindowsTemplateStudioColors();

            // Font Sizes and Font Family
            SetFontProperties();
        }

        // VS Colors
        private void SetThemedDialogColors()
        {
            WindowPanel = _stylesProvider.GetColor("ThemedDialog", "WindowPanel");
            WindowPanelText = _stylesProvider.GetColor("ThemedDialog", "WindowPanelText");
            WindowBorder = _stylesProvider.GetColor("ThemedDialog", "WindowBorder");
            HeaderText = _stylesProvider.GetColor("ThemedDialog", "HeaderText");
            HeaderTextSecondary = _stylesProvider.GetColor("ThemedDialog", "HeaderTextSecondary");
            Hyperlink = _stylesProvider.GetColor("ThemedDialog", "Hyperlink");
            HyperlinkHover = _stylesProvider.GetColor("ThemedDialog", "HyperlinkHover");
            HyperlinkPressed = _stylesProvider.GetColor("ThemedDialog", "HyperlinkPressed");
            HyperlinkDisabled = _stylesProvider.GetColor("ThemedDialog", "HyperlinkDisabled");
            SelectedItemActive = _stylesProvider.GetColor("ThemedDialog", "SelectedItemActive");
            SelectedItemInactive = _stylesProvider.GetColor("ThemedDialog", "SelectedItemInactive");
            ListItemMouseOver = _stylesProvider.GetColor("ThemedDialog", "ListItemMouseOver");
            ListItemDisabledText = _stylesProvider.GetColor("ThemedDialog", "ListItemDisabledText");
            GridHeadingBackground = _stylesProvider.GetColor("ThemedDialog", "GridHeadingBackground");
            GridHeadingHoverBackground = _stylesProvider.GetColor("ThemedDialog", "GridHeadingHoverBackground");
            GridHeadingText = _stylesProvider.GetColor("ThemedDialog", "GridHeadingText");
            GridHeadingHoverText = _stylesProvider.GetColor("ThemedDialog", "GridHeadingHoverText");
            GridLine = _stylesProvider.GetColor("ThemedDialog", "GridLine");
            SectionDivider = _stylesProvider.GetColor("ThemedDialog", "SectionDivider");

            WindowButton = _stylesProvider.GetColor("ThemedDialog", "WindowButton");
            WindowButtonHover = _stylesProvider.GetColor("ThemedDialog", "WindowButtonHover");
            WindowButtonDown = _stylesProvider.GetColor("ThemedDialog", "WindowButtonDown");
            WindowButtonBorder = _stylesProvider.GetColor("ThemedDialog", "WindowButtonBorder");
            WindowButtonHoverBorder = _stylesProvider.GetColor("ThemedDialog", "WindowButtonHoverBorder");
            WindowButtonDownBorder = _stylesProvider.GetColor("ThemedDialog", "WindowButtonDownBorder");
            WindowButtonGlyph = _stylesProvider.GetColor("ThemedDialog", "WindowButtonGlyph");
            WindowButtonHoverGlyph = _stylesProvider.GetColor("ThemedDialog", "WindowButtonHoverGlyph");
            WindowButtonDownGlyph = _stylesProvider.GetColor("ThemedDialog", "WindowButtonDownGlyph");

            WizardFooter = _stylesProvider.GetColor("ThemedDialog", "WizardFooter");
            WizardFooterText = _stylesProvider.GetColor("ThemedDialog", "WizardFooterText");
        }

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
        }

        // New Color additions
        private void SetThemedCardColors()
        {
            CardTitleText = _stylesProvider.GetColor("ThemedCard", "CardTitleText");
            CardDescriptionText = _stylesProvider.GetColor("ThemedCard", "CardDescriptionText");
            CardBackgroundDefault = _stylesProvider.GetColor("ThemedCard", "CardBackgroundDefault");
            CardBackgroundFocus = _stylesProvider.GetColor("ThemedCard", "CardBackgroundFocus");
            CardBackgroundHover = _stylesProvider.GetColor("ThemedCard", "CardBackgroundHover");
            CardBackgroundPressed = _stylesProvider.GetColor("ThemedCard", "CardBackgroundPressed");
            CardBackgroundSelected = _stylesProvider.GetColor("ThemedCard", "CardBackgroundSelected");
            CardBackgroundDisabled = _stylesProvider.GetColor("ThemedCard", "CardBackgroundDisabled");
            CardBorderDefault = _stylesProvider.GetColor("ThemedCard", "CardBorderDefault");
            CardBorderFocus = _stylesProvider.GetColor("ThemedCard", "CardBorderFocus");
            CardBorderHover = _stylesProvider.GetColor("ThemedCard", "CardBorderHover");
            CardBorderPressed = _stylesProvider.GetColor("ThemedCard", "CardBorderPressed");
            CardBorderSelected = _stylesProvider.GetColor("ThemedCard", "CardBorderSelected");
            CardBorderDisabled = _stylesProvider.GetColor("ThemedCard", "CardBorderDisabled");
            CardIcon = _stylesProvider.GetColor("ThemedCard", "CardIcon"); // Added by mvegaca
            CardFooterText = _stylesProvider.GetColor("ThemedCard", "CardFooterText"); // Added by mvegaca
        }

        private void SetWindowsTemplateStudioColors()
        {
            NotificationInformationText = _stylesProvider.GetColor("WindowsTemplateStudio", "NotificationInformationText");
            NotificationInformationBackground = _stylesProvider.GetColor("WindowsTemplateStudio", "NotificationInformationBackground");
            NotificationInformationIcon = _stylesProvider.GetColor("WindowsTemplateStudio", "NotificationInformationIcon");

            NotificationWarningText = _stylesProvider.GetColor("WindowsTemplateStudio", "NotificationWarningText");
            NotificationWarningBackground = _stylesProvider.GetColor("WindowsTemplateStudio", "NotificationWarningBackground");
            NotificationWarningIcon = _stylesProvider.GetColor("WindowsTemplateStudio", "NotificationWarningIcon");

            NotificationErrorText = _stylesProvider.GetColor("WindowsTemplateStudio", "NotificationErrorText");
            NotificationErrorBackground = _stylesProvider.GetColor("WindowsTemplateStudio", "NotificationErrorBackground");
            NotificationErrorIcon = _stylesProvider.GetColor("WindowsTemplateStudio", "NotificationErrorIcon");

            DeleteTemplateIcon = _stylesProvider.GetColor("WindowsTemplateStudio", "DeleteTemplateIcon");
            SavedTemplateBackgroundHover = _stylesProvider.GetColor("WindowsTemplateStudio", "SavedTemplateBackgroundHover");

            NewItemFileStatusNewFile = _stylesProvider.GetColor("WindowsTemplateStudio", "NewItemFileStatusNewFile");
            NewItemFileStatusModifiedFile = _stylesProvider.GetColor("WindowsTemplateStudio", "NewItemFileStatusModifiedFile");
            NewItemFileStatusConflictingFile = _stylesProvider.GetColor("WindowsTemplateStudio", "NewItemFileStatusConflictingFile");
            NewItemFileStatusConflictingStylesFile = _stylesProvider.GetColor("WindowsTemplateStudio", "NewItemFileStatusConflictingStylesFile");
            NewItemFileStatusWarningFile = _stylesProvider.GetColor("WindowsTemplateStudio", "NewItemFileStatusWarningFile");
            NewItemFileStatusUnchangedFile = _stylesProvider.GetColor("WindowsTemplateStudio", "NewItemFileStatusUnchangedFile");

            DialogInfoIcon = _stylesProvider.GetColor("WindowsTemplateStudio", "DialogInfoIcon");
            DialogErrorIcon = _stylesProvider.GetColor("WindowsTemplateStudio", "DialogErrorIcon");
        }

        // Font Sizes and Font Family
        private void SetFontProperties()
        {
            // FontSizes
            Environment90PercentFontSize = _stylesProvider.GetFontSize("Environment90PercentFontSize");
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
    }
}
