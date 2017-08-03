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

        public ShellNavigationItem(string label, Symbol symbol, string viewModelName)
        {
            Label = label;
            Symbol = symbol;
            ViewModelName = viewModelName;

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

        public ShellNavigationItem(string label, IconElement icon, string viewModelName)
        {
            Label = label;
            _iconElement = icon;
            ViewModelName = viewModelName;

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
