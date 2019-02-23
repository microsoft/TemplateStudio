//{[{
using Windows.UI.Xaml.Navigation;
//}]}

namespace Param_RootNamespace.Views
{

    public sealed partial class wts.ItemNamePage : Page
    {
        //^^
        //{[{

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            base.OnNavigatedFrom(e);

            // Workaround for issue on MasterDetail Control. Find More info at https://github.com/Microsoft/WindowsTemplateStudio/issues/2738
            ViewModel.Selected = null;
        }
        //}]}
    }
}