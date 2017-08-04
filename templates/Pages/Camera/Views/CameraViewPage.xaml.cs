using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Param_ItemNamespace.Views
{
    public sealed partial class CameraViewPage : Page
    {
        public CameraViewPage()
        {
            InitializeComponent();
        }

        private async void Photo_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.CapturedCommand.Execute(await Camera.TakePhotoAsync());
        }
    }
}