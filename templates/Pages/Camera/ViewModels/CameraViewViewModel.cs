using System;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.Devices.Enumeration;
using Windows.UI.Xaml.Media.Imaging;
using Param_ItemNamespace.Views;

namespace Param_ItemNamespace.ViewModels
{
    public class CameraViewViewModel : System.ComponentModel.INotifyPropertyChanged
    {
        private CameraControl _camera;
        private string _errorMessage;
        private BitmapImage _photo;
        private Panel _panel;

        public string ErrorMessage
        {
            get { return _errorMessage; }
            set { Set(ref _errorMessage, value); }
        }

        public BitmapImage Photo
        {
            get { return _photo; }
            set { Set(ref _photo, value); }
        }

        public Panel Panel
        {
            get { return _panel; }
            set { Set(ref _panel, value); }
        }

        public ICommand CaptureCommand { get; private set; }

        public ICommand SwitchCommand { get; private set; }

        public override void Cleanup()
        {
            base.Cleanup();
            _camera.Cleanup();
        }

        public async Task InitializeAsync(CameraControl camera)
        {
            _camera = camera;

            try
            {
                await _camera.InitializeAsync();
            }
            catch (UnauthorizedAccessException)
            {
                ErrorMessage = "The app was denied access to the camera.";
            }
            catch (NotSupportedException)
            {
                ErrorMessage = "No video capture devices found.";
            }
        }

        public CameraViewViewModel()
        {
            CaptureCommand = new RelayCommand(async () => await OnCaptureAsync());
            SwitchCommand = new RelayCommand(OnSwitch);
        }

        private async Task OnCaptureAsync()
        {
            Photo = new BitmapImage(new Uri(await _camera.TakePhotoAsync()));
        }

        private void OnSwitch()
        {
            Panel = Panel == Panel.Front ? Panel.Back : Panel.Front;
        }
    }
}
