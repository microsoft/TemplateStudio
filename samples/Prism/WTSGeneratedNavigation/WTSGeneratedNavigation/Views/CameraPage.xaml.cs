using Windows.UI.Xaml.Controls;

using WTSGeneratedNavigation.ViewModels;

namespace WTSGeneratedNavigation.Views
{
    public sealed partial class CameraPage : Page
    {
        private CameraViewModel ViewModel => DataContext as CameraViewModel;

        public CameraPage()
        {
            InitializeComponent();
        }
    }
}
