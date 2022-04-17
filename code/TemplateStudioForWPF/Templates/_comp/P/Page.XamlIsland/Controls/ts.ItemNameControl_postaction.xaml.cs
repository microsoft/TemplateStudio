namespace Param_RootNamespace.Controls
{
    public partial class ts.ItemNameControl : UserControl
    {
        public ts.ItemNameControl()
        {
//^^
//{[{
            _themeSelectorService = ((App) Application.Current).Container.Resolve(typeof(IThemeSelectorService)) as IThemeSelectorService;
//}]}
            _themeSelectorService.ThemeChanged += OnThemeChanged;
        }
    }
}
