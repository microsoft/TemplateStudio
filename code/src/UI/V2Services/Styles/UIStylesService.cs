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
            //VSColorTheme.ThemeChanged += OnThemeChanged;
        }

        //private void OnThemeChanged(ThemeChangedEventArgs e)
        //{
        //    SetStyles();
        //}

        public void Initialize(IStyleValuesProvider stylesProvider)
        {
            _stylesProvider = stylesProvider;
            SetStyles();
        }

        private void SetStyles()
        {
            // Colors
            WindowPanel = _stylesProvider.GetColor("ThemedDialogColors", "WindowPanelColorKey");
            WindowBorder = _stylesProvider.GetColor("ThemedDialogColors", "WindowBorderColorKey");
            HeaderText = _stylesProvider.GetColor("ThemedDialogColors", "HeaderTextColorKey");
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
