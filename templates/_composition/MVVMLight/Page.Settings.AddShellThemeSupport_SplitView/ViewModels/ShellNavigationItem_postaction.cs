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
            this.Label = label;
            this.Symbol = symbol;
            this.ViewModelName = viewModelName;

            //^^
            //{[{
            Services.ThemeSelectorService.OnThemeChanged += (s, e) => { if (!IsSelected) SelectedForeground = GetStandardTextColorBrush(); };
            //}]}
        }

        public ShellNavigationItem(string label, IconElement icon, string viewModelName)
        {
            this.Label = label;
            this._iconElement = icon;
            this.ViewModelName = viewModelName;

            //^^
            //{[{
            Services.ThemeSelectorService.OnThemeChanged += (s, e) => { if (!IsSelected) SelectedForeground = GetStandardTextColorBrush(); };
            //}]}
        }
