using Windows.UI.Xaml.Controls;

namespace Param_RootNamespace.Views
{
    public sealed partial class CameraViewPage : Page
    {
        public CameraViewPage()
        {
            InitializeComponent();
            ViewModel.Initialize(cameraControl);
        }
    }
}
