//{[{
using Windows.UI.Xaml.Navigation;
using Param_ItemNamespace.Helpers;
//}]}

namespace Param_ItemNamespace.Views
{
    public sealed partial class wts.ItemNamePage : Page
    {
        public wts.ItemNamePage()
        {
        }

        //{[{
        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            await cameraControl.InitializeCameraAsync();
        }

        protected override async void OnNavigatedFrom(NavigationEventArgs e)
        {
            base.OnNavigatedFrom(e);
            await cameraControl.CleanupCameraAsync();
        }
        //}]}
    }
}
