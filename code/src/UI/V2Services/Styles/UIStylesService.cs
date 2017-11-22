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

            // FontSizes
            Environment90PercentFontSize = _stylesProvider.GetFontSize("Environment90PercentFontSize");
            EnvironmentFontSize = _stylesProvider.GetFontSize("EnvironmentFontSize");
            Environment111PercentFontSize = _stylesProvider.GetFontSize("Environment111PercentFontSize");
            Environment122PercentFontSize = _stylesProvider.GetFontSize("Environment122PercentFontSize");
            Environment133PercentFontSize = _stylesProvider.GetFontSize("Environment133PercentFontSize");
            Environment155PercentFontSize = _stylesProvider.GetFontSize("Environment155PercentFontSize");
            Environment200PercentFontSize = _stylesProvider.GetFontSize("Environment200PercentFontSize");
            Environment310PercentFontSize = _stylesProvider.GetFontSize("Environment310PercentFontSize");
            Environment330PercentFontSize = _stylesProvider.GetFontSize("Environment330PercentFontSize");
            Environment375PercentFontSize = _stylesProvider.GetFontSize("Environment375PercentFontSize");
        }
    }
}
