namespace Param_RootNamespace.Controls
{
    public partial class ts.ItemNameControl : UserControl
    {
        public ts.ItemNameControl()
        {
//^^
//{[{
            _themeSelectorService = ((App) Application.Current).GetService<IThemeSelectorService>();
//}]}
            _themeSelectorService.ThemeChanged += OnThemeChanged;
        }
    }
}
