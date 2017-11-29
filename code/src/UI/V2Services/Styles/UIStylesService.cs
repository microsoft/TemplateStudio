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
            // Colors
            SetThemedDialogColors();
            SetThemedCardColors();
            SetCommonDocumentColors();
            SetCommonControlColors();

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

        private void SetThemedDialogColors()
        {
            WindowPanel = _stylesProvider.GetColor("ThemedDialogColors", "WindowPanelColorKey");
            WindowBorder = _stylesProvider.GetColor("ThemedDialogColors", "WindowBorderColorKey");
            HeaderText = _stylesProvider.GetColor("ThemedDialogColors", "HeaderTextColorKey");
            HeaderTextSecondary = _stylesProvider.GetColor("ThemedDialogColors", "HeaderTextSecondaryColorKey");
            Hyperlink = _stylesProvider.GetColor("ThemedDialogColors", "HyperlinkColorKey");
            HyperlinkHover = _stylesProvider.GetColor("ThemedDialogColors", "HyperlinkHoverColorKey");
            HyperlinkPressed = _stylesProvider.GetColor("ThemedDialogColors", "HyperlinkPressedColorKey");
            HyperlinkDisabled = _stylesProvider.GetColor("ThemedDialogColors", "HyperlinkDisabledColorKey");
            SelectedItemActive = _stylesProvider.GetColor("ThemedDialogColors", "SelectedItemActiveColorKey");
            SelectedItemInactive = _stylesProvider.GetColor("ThemedDialogColors", "SelectedItemInactiveColorKey");
            ListItemMouseOver = _stylesProvider.GetColor("ThemedDialogColors", "ListItemMouseOverColorKey");
            ListItemDisabledText = _stylesProvider.GetColor("ThemedDialogColors", "ListItemDisabledTextColorKey");
            GridHeadingBackground = _stylesProvider.GetColor("ThemedDialogColors", "GridHeadingBackgroundColorKey");
            GridHeadingHoverBackground = _stylesProvider.GetColor("ThemedDialogColors", "GridHeadingHoverBackgroundColorKey");
            GridHeadingText = _stylesProvider.GetColor("ThemedDialogColors", "GridHeadingTextColorKey");
            GridHeadingHoverText = _stylesProvider.GetColor("ThemedDialogColors", "GridHeadingHoverTextColorKey");
            GridLine = _stylesProvider.GetColor("ThemedDialogColors", "GridLineColorKey");
            SectionDivider = _stylesProvider.GetColor("ThemedDialogColors", "SectionDividerColorKey");

            WindowButton = _stylesProvider.GetColor("ThemedDialogColors", "WindowButtonColorKey");
            WindowButtonHover = _stylesProvider.GetColor("ThemedDialogColors", "WindowButtonHoverColorKey");
            WindowButtonDown = _stylesProvider.GetColor("ThemedDialogColors", "WindowButtonDownColorKey");
            WindowButtonBorder = _stylesProvider.GetColor("ThemedDialogColors", "WindowButtonBorderColorKey");
            WindowButtonHoverBorder = _stylesProvider.GetColor("ThemedDialogColors", "WindowButtonHoverBorderColorKey");
            WindowButtonDownBorder = _stylesProvider.GetColor("ThemedDialogColors", "WindowButtonDownBorderColorKey");
            WindowButtonGlyph = _stylesProvider.GetColor("ThemedDialogColors", "WindowButtonGlyphColorKey");
            WindowButtonHoverGlyph = _stylesProvider.GetColor("ThemedDialogColors", "WindowButtonHoverGlyphColorKey");
            WindowButtonDownGlyph = _stylesProvider.GetColor("ThemedDialogColors", "WindowButtonDownGlyphColorKey");

            WizardFooter = _stylesProvider.GetColor("ThemedDialogColors", "WizardFooterColorKey");
            WizardFooterText = _stylesProvider.GetColor("ThemedDialogColors", "WizardFooterTextColorKey");
        }

        private void SetThemedCardColors()
        {
            CardTitleText = _stylesProvider.GetColor("ThemedCardColors", "CardTitleTextColorKey");
            CardDescriptionText = _stylesProvider.GetColor("ThemedCardColors", "CardDescriptionTextColorKey");
            CardBackgroundDefault = _stylesProvider.GetColor("ThemedCardColors", "CardBackgroundDefaultColorKey");
            CardBackgroundFocus = _stylesProvider.GetColor("ThemedCardColors", "CardBackgroundFocusColorKey");
            CardBackgroundHover = _stylesProvider.GetColor("ThemedCardColors", "CardBackgroundHoverColorKey");
            CardBackgroundPressed = _stylesProvider.GetColor("ThemedCardColors", "CardBackgroundPressedColorKey");
            CardBackgroundSelected = _stylesProvider.GetColor("ThemedCardColors", "CardBackgroundSelectedColorKey");
            CardBackgroundDisabled = _stylesProvider.GetColor("ThemedCardColors", "CardBackgroundDisabledColorKey");
            CardBorderDefault = _stylesProvider.GetColor("ThemedCardColors", "CardBorderDefaultColorKey");
            CardBorderFocus = _stylesProvider.GetColor("ThemedCardColors", "CardBorderFocusColorKey");
            CardBorderHover = _stylesProvider.GetColor("ThemedCardColors", "CardBorderHoverColorKey");
            CardBorderPressed = _stylesProvider.GetColor("ThemedCardColors", "CardBorderPressedColorKey");
            CardBorderSelected = _stylesProvider.GetColor("ThemedCardColors", "CardBorderSelectedColorKey");
            CardBorderDisabled = _stylesProvider.GetColor("ThemedCardColors", "CardBorderDisabledColorKey");
        }

        private void SetCommonDocumentColors()
        {
            ListItemText = _stylesProvider.GetColor("CommonDocumentColors", "ListItemTextColorKey");
            ListItemTextDisabled = _stylesProvider.GetColor("CommonDocumentColors", "ListItemTextDisabledColorKey");
        }

        private void SetCommonControlColors()
        {
            Button = _stylesProvider.GetColor("CommonControlColors", "ButtonColorKey");
            ButtonText = _stylesProvider.GetColor("CommonControlColors", "ButtonTextColorKey");
            ButtonBorder = _stylesProvider.GetColor("CommonControlColors", "ButtonBorderColorKey");

            ButtonDefault = _stylesProvider.GetColor("CommonControlColors", "ButtonDefaultColorKey");
            ButtonDefaultText = _stylesProvider.GetColor("CommonControlColors", "ButtonDefaultTextColorKey");
            ButtonBorderDefault = _stylesProvider.GetColor("CommonControlColors", "ButtonBorderDefaultColorKey");

            ButtonDisabled = _stylesProvider.GetColor("CommonControlColors", "ButtonDisabledColorKey");
            ButtonDisabledText = _stylesProvider.GetColor("CommonControlColors", "ButtonDisabledTextColorKey");
            ButtonBorderDisabled = _stylesProvider.GetColor("CommonControlColors", "ButtonBorderDisabledColorKey");

            ButtonFocused = _stylesProvider.GetColor("CommonControlColors", "ButtonFocusedColorKey");
            ButtonFocusedText = _stylesProvider.GetColor("CommonControlColors", "ButtonFocusedTextColorKey");
            ButtonBorderFocused = _stylesProvider.GetColor("CommonControlColors", "ButtonBorderFocusedColorKey");

            ButtonHover = _stylesProvider.GetColor("CommonControlColors", "ButtonHoverColorKey");
            ButtonHoverText = _stylesProvider.GetColor("CommonControlColors", "ButtonHoverTextColorKey");
            ButtonBorderHover = _stylesProvider.GetColor("CommonControlColors", "ButtonBorderHoverColorKey");

            ButtonPressed = _stylesProvider.GetColor("CommonControlColors", "ButtonPressedColorKey");
            ButtonPressedText = _stylesProvider.GetColor("CommonControlColors", "ButtonPressedTextColorKey");
            ButtonBorderPressed = _stylesProvider.GetColor("CommonControlColors", "ButtonBorderPressedColorKey");

            ////ComboBox Colors
            ComboBoxBackground = _stylesProvider.GetColor("CommonControlColors", "ComboBoxBackgroundColorKey");
            ComboBoxBackgroundDisabled = _stylesProvider.GetColor("CommonControlColors", "ComboBoxBackgroundDisabledColorKey");
            ComboBoxBackgroundFocused = _stylesProvider.GetColor("CommonControlColors", "ComboBoxBackgroundFocusedColorKey");
            ComboBoxBackgroundHover = _stylesProvider.GetColor("CommonControlColors", "ComboBoxBackgroundHoverColorKey");
            ComboBoxBackgroundPressed = _stylesProvider.GetColor("CommonControlColors", "ComboBoxBackgroundPressedColorKey");

            ComboBoxBorder = _stylesProvider.GetColor("CommonControlColors", "ComboBoxBorderColorKey");
            ComboBoxBorderDisabled = _stylesProvider.GetColor("CommonControlColors", "ComboBoxBorderDisabledColorKey");
            ComboBoxBorderFocused = _stylesProvider.GetColor("CommonControlColors", "ComboBoxBorderFocusedColorKey");
            ComboBoxBorderHover = _stylesProvider.GetColor("CommonControlColors", "ComboBoxBorderHoverColorKey");
            ComboBoxBorderPressed = _stylesProvider.GetColor("CommonControlColors", "ComboBoxBorderPressedColorKey");

            ComboBoxGlyph = _stylesProvider.GetColor("CommonControlColors", "ComboBoxGlyphColorKey");
            ComboBoxGlyphBackground = _stylesProvider.GetColor("CommonControlColors", "ComboBoxGlyphBackgroundColorKey");
            ComboBoxGlyphBackgroundDisabled = _stylesProvider.GetColor("CommonControlColors", "ComboBoxGlyphBackgroundDisabledColorKey");
            ComboBoxGlyphBackgroundFocused = _stylesProvider.GetColor("CommonControlColors", "ComboBoxGlyphBackgroundFocusedColorKey");
            ComboBoxGlyphBackgroundHover = _stylesProvider.GetColor("CommonControlColors", "ComboBoxGlyphBackgroundHoverColorKey");
            ComboBoxGlyphBackgroundPressed = _stylesProvider.GetColor("CommonControlColors", "ComboBoxGlyphBackgroundPressedColorKey");
            ComboBoxGlyphDisabled = _stylesProvider.GetColor("CommonControlColors", "ComboBoxGlyphDisabledColorKey");
            ComboBoxGlyphFocused = _stylesProvider.GetColor("CommonControlColors", "ComboBoxGlyphFocusedColorKey");
            ComboBoxGlyphHover = _stylesProvider.GetColor("CommonControlColors", "ComboBoxGlyphHoverColorKey");
            ComboBoxGlyphPressed = _stylesProvider.GetColor("CommonControlColors", "ComboBoxGlyphPressedColorKey");

            ComboBoxListBackground = _stylesProvider.GetColor("CommonControlColors", "ComboBoxListBackgroundColorKey");
            ComboBoxListBackgroundShadow = _stylesProvider.GetColor("CommonControlColors", "ComboBoxListBackgroundShadowColorKey");
            ComboBoxListBorder = _stylesProvider.GetColor("CommonControlColors", "ComboBoxListBorderColorKey");

            ComboBoxListItemBackgroundHover = _stylesProvider.GetColor("CommonControlColors", "ComboBoxListItemBackgroundHoverColorKey");
            ComboBoxListItemBorderHover = _stylesProvider.GetColor("CommonControlColors", "ComboBoxListItemBorderHoverColorKey");
            ComboBoxListItemText = _stylesProvider.GetColor("CommonControlColors", "ComboBoxListItemTextColorKey");
            ComboBoxListItemTextHover = _stylesProvider.GetColor("CommonControlColors", "ComboBoxListItemTextHoverColorKey");

            ComboBoxSelection = _stylesProvider.GetColor("CommonControlColors", "ComboBoxSelectionColorKey");

            ComboBoxSeparator = _stylesProvider.GetColor("CommonControlColors", "ComboBoxSeparatorColorKey");
            ComboBoxSeparatorDisabled = _stylesProvider.GetColor("CommonControlColors", "ComboBoxSeparatorDisabledColorKey");
            ComboBoxSeparatorFocused = _stylesProvider.GetColor("CommonControlColors", "ComboBoxSeparatorFocusedColorKey");
            ComboBoxSeparatorHover = _stylesProvider.GetColor("CommonControlColors", "ComboBoxSeparatorHoverColorKey");
            ComboBoxSeparatorPressed = _stylesProvider.GetColor("CommonControlColors", "ComboBoxSeparatorPressedColorKey");

            ComboBoxText = _stylesProvider.GetColor("CommonControlColors", "ComboBoxTextColorKey");
            ComboBoxTextDisabled = _stylesProvider.GetColor("CommonControlColors", "ComboBoxTextDisabledColorKey");
            ComboBoxTextFocused = _stylesProvider.GetColor("CommonControlColors", "ComboBoxTextFocusedColorKey");
            ComboBoxTextHover = _stylesProvider.GetColor("CommonControlColors", "ComboBoxTextHoverColorKey");
            ComboBoxTextInputSelection = _stylesProvider.GetColor("CommonControlColors", "ComboBoxTextInputSelectionColorKey");
            ComboBoxTextPressed = _stylesProvider.GetColor("CommonControlColors", "ComboBoxTextPressedColorKey");
        }
    }
}
