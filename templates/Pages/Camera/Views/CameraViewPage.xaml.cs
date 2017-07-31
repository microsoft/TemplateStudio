using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace Param_ItemNamespace.Views
{
    public sealed partial class CameraViewPage : Page
    {
        public CameraViewPage()
        {
            InitializeComponent();
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            await Camera.InitializeAsync();
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            base.OnNavigatedFrom(e);
            Camera.Cleanup();
        }

        private async void Photo_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.CapturedCommand.Execute(await Camera.TakePhotoAsync());
        }
    }
}