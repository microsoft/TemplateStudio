//{[{
using Windows.UI.Xaml.Navigation;
//}]}
namespace Param_ItemNamespace.Views
{
    public sealed partial class wts.ItemNamePage : Page
    {
        //^^

        //{[{
        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            await ViewModel.InitializeAsync(Camera);
        }

        protected async override void OnNavigatedFrom(NavigationEventArgs e)
        {
            await ViewModel.CleanupAsync();
        }
        //}]}
    }
}
