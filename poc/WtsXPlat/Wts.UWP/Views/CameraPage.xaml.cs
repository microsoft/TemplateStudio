using Windows.UI.Xaml.Controls;

using Wts.UWP.ViewModels;

namespace Wts.UWP.Views
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
