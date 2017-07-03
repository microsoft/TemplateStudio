//{**
// This code block add code to the ShellNavigationItem to apply the correct color based on the selected theme.
//**}    

public class ShellNavigationItem : INotifyPropertyChanged
{
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
        this.Label = name;
        this.Symbol = symbol;
        this.PageType = pageType;

        //^^
        //{[{
        Services.ThemeSelectorService.OnThemeChanged += (s, e) => { if (!IsSelected) SelectedForeground = GetStandardTextColorBrush(); };
        //}]}
    }
}
