using System;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Imaging;

namespace Param_ItemNamespace.Views
{
    public sealed partial class CameraViewPage : Page, System.ComponentModel.INotifyPropertyChanged
    {
        private bool _capturing;
        private string _errorMessage;

        public string ErrorMessage
        {
            get { return _errorMessage; }
            set { Set(ref _errorMessage, value); }
        }

        public CameraViewPage()
        {
            InitializeComponent();
        }

        private async Task InitializeAsync()
        {
            try
            {
                await Camera.InitializeAsync();
            }
            catch (Exception ex)
            {
                ErrorMessage = ex.Message;
            }
        }

        private Task CleanupAsync()
        {
            return Camera.CleanupAsync();
        }

        private async void Photo_Click(object sender, RoutedEventArgs e)
        {
            if (_capturing)
            {
                return;
            }

            _capturing = true;

            Photo.Source = new BitmapImage(new Uri(await Camera.TakePhotoAsync()));

            _capturing = false;
        }

        private void SwitchCamera_Click(object sender, RoutedEventArgs e)
        {
            Camera.Panel = (Camera.Panel == Windows.Devices.Enumeration.Panel.Front) ? Windows.Devices.Enumeration.Panel.Back : Windows.Devices.Enumeration.Panel.Front;
        }
    }
}
