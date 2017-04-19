    public class ShellNavigationItem : INotifyPropertyChanged
    {
        private SolidColorBrush GetStandardTextColorBrush()
        {
            var result = Application.Current.Resources["SystemControlForegroundBaseHighBrush"] as SolidColorBrush;

            //{[{
            if (!Services.ThemeSelectorService.IsLightThemeEnabled)
            {
                result = Application.Current.Resources["SystemControlForegroundAltHighBrush"] as SolidColorBrush;
            }
            //}]}
            return result;
        }

        private ShellNavigationItem(string name, Symbol symbol, Type pageType)
        {
            this.Label = name;
            this.Symbol = symbol;
            this.PageType = pageType;

            //^^
            //{[{
            Services.ThemeSelectorService.OnThemeChanged += (s, e) => { if (!IsSelected) SelectedForeground = GetStandardTextColorBrush(); };
            //}]}
        }
    }
