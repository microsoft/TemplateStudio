using Windows.UI.Xaml.Controls;

using WTSGeneratedPivot.ViewModels;

namespace WTSGeneratedPivot.Views
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
