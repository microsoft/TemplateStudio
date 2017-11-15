// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Windows;

using Microsoft.VisualStudio.PlatformUI;

namespace Microsoft.Templates.UI.V2Services
{
    public partial class UIStylesService : DependencyObject
    {
        private IStyleValuesProvider _stylesProvider;

        private static UIStylesService _instance;

        public static UIStylesService Instance => _instance ?? (_instance = new UIStylesService());

        public UIStylesService()
        {
            VSColorTheme.ThemeChanged += OnThemeChanged;
        }

        private void OnThemeChanged(ThemeChangedEventArgs e)
        {
            SetStyles();
        }

        public void Initialize(IStyleValuesProvider stylesProvider)
        {
            _stylesProvider = stylesProvider;
            SetStyles();
        }

        private void SetStyles()
        {
            // Colors
            EnvironmentBackgroundColor = _stylesProvider.GetColor(EnvironmentColors.EnvironmentBackgroundColorKey);
            EnvironmentBackgroundTexture1Color = _stylesProvider.GetColor(EnvironmentColors.EnvironmentBackgroundTexture1ColorKey);
            EnvironmentBackgroundTexture2Color = _stylesProvider.GetColor(EnvironmentColors.EnvironmentBackgroundTexture2ColorKey);

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
