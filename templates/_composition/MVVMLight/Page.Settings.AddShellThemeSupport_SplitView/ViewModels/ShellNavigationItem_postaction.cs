//{**
// This code block add code to the ShellNavigationItem to apply the correct color based on the selected theme.
//**}
//{[{
using Param_RootNamespace.Services;
//}]}

        public ShellNavigationItem(string label, string viewModelName)
        {
            Label = label;
            ViewModelName = viewModelName;
//^^
//{[{
    
            ThemeSelectorService.OnThemeChanged += (s, e) =>
            {
                if (!IsSelected)
                {
                    SelectedForeground = GetStandardTextColorBrush();
                }
            };
//}]}
        }

        private SolidColorBrush GetStandardTextColorBrush()
        {
//{--{
            var brush = Application.Current.Resources["SystemControlForegroundBaseHighBrush"] as SolidColorBrush;

            return brush;//}--}
            //{[{
            return ThemeSelectorService.GetSystemControlForegroundForTheme();
            //}]}
        }
    }