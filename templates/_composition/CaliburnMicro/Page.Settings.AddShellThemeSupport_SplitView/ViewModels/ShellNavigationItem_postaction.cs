//{**
// This code block add code to the ShellNavigationItem to apply the correct color based on the selected theme.
//**}
//{[{
using Param_RootNamespace.Services;
//}]}
        private SolidColorBrush GetStandardTextColorBrush()
        {
//{--{
            var brush = Application.Current.Resources["ThemeControlForegroundBaseHighBrush"] as SolidColorBrush;

            return brush;//}--}
            //{[{
            return ThemeSelectorService.GetSystemControlForegroundForTheme();
            //}]}
        }

        protected ShellNavigationItem()
        {
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
    }