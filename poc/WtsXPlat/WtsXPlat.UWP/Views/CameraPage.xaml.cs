using Windows.UI.Xaml.Controls;

using WtsXPlat.UWP.ViewModels;

namespace WtsXPlat.UWP.Views
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
