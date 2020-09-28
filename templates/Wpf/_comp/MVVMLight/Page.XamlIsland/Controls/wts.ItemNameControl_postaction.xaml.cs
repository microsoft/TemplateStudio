//{[{
using GalaSoft.MvvmLight.Ioc;
//}]}
namespace Param_RootNamespace.Controls
{
    public partial class wts.ItemNameControl : UserControl
    {
        public wts.ItemNameControl()
        {
//^^
//{[{
            _themeSelectorService = SimpleIoc.Default.GetService(typeof(IThemeSelectorService)) as IThemeSelectorService;
//}]}
            _themeSelectorService.ThemeChanged += OnThemeChanged;
        }
    }
}