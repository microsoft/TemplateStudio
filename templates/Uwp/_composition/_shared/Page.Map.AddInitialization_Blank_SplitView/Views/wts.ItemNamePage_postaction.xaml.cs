//{[{
using Windows.UI.Xaml.Navigation;
//}]}
namespace Param_ItemNamespace.Views
{
    public sealed partial class wts.ItemNamePage : Page
    {
        //^^

        //{[{
        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            await ViewModel.InitializeAsync(mapControl);
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            ViewModel.Cleanup();
        }
        //}]}
    }
}
