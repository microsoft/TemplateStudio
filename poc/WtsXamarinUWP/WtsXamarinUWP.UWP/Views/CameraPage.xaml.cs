using Windows.UI.Xaml.Controls;

using WtsXamarinUWP.UWP.ViewModels;

namespace WtsXamarinUWP.UWP.Views
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
