namespace Param_RootNamespace.Controls
{
    public partial class wts.ItemNameControl : UserControl
    {
        public wts.ItemNameControl(/*{[{*/IThemeSelectorService themeSelectorService/*}]}*/)
        {
//^^
//{[{
            _themeSelectorService = themeSelectorService;
//}]}
            _themeSelectorService.ThemeChanged += OnThemeChanged;
        }
    }
}