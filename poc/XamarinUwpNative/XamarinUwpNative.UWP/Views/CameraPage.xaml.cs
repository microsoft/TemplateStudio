using Windows.UI.Xaml.Controls;

using XamarinUwpNative.UWP.ViewModels;

namespace XamarinUwpNative.UWP.Views
{
    public sealed partial class CameraPage : Page
    {
        public CameraViewModel ViewModel { get; } = new CameraViewModel();

        public CameraPage()
        {
            InitializeComponent();
        }
    }
}
