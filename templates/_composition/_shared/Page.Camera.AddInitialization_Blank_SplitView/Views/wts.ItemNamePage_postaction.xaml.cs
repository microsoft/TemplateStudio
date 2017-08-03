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
            await Camera.InitializeAsync();
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            Camera.Cleanup();
        }
        //}]}
    }
}
