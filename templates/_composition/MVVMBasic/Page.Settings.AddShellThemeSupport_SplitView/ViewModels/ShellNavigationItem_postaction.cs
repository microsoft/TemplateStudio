//{**
//This code block add code to the ShellNavigationItem to apply the correct color based on the selected theme.
//**}
        private SolidColorBrush GetStandardTextColorBrush()
        {
            var brush = Application.Current.Resources["SystemControlForegroundBaseHighBrush"] as SolidColorBrush;

            //{[{
            if (!Services.ThemeSelectorService.IsLightThemeEnabled)
            {
                brush = Application.Current.Resources["SystemControlForegroundAltHighBrush"] as SolidColorBrush;
            }
            //}]}
            return brush;
        }

        private ShellNavigationItem(string name, Symbol symbol, Type pageType)
        {
            Label = name;
            Symbol = symbol;
            PageType = pageType;

            //^^
            //{[{
            Services.ThemeSelectorService.OnThemeChanged += (s, e) =>
            {
                if (!IsSelected)
                {
                    SelectedForeground = GetStandardTextColorBrush();
                }
            };
            //}]}
        }

        private ShellNavigationItem(string name, IconElement icon, Type pageType)
        {
            Label = name;
            _iconElement = icon;
            PageType = pageType;

            //^^
            //{[{
            Services.ThemeSelectorService.OnThemeChanged += (s, e) =>
            {
                if (!IsSelected)
                {
                    SelectedForeground = GetStandardTextColorBrush();
                }
            };
            //}]}
        }
