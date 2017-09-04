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
        private ICommand _captureCommand;
        private ICommand _switchCommand;
        private CameraControl _camera;
        private string _errorMessage;
        private BitmapImage _photo;
        private Panel _panel;
        private bool _capturing;

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

        public ICommand CaptureCommand
        {
            get
            {
                if (_captureCommand == null)
                {
                    _captureCommand = new RelayCommand(async () => await OnCaptureAsync());
                }

                return _captureCommand;
            }
        }

        public ICommand SwitchCommand
        {
            get
            {
                if (_switchCommand == null)
                {
                    _switchCommand = new RelayCommand(OnSwitch);
                }

                return _switchCommand;
            }
        }

        public Task CleanupAsync()
        {
            return _camera.CleanupAsync();
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

        private async Task OnCaptureAsync()
        {
            if (_capturing)
            {
                return;
            }

            _capturing = true;

            Photo = new BitmapImage(new Uri(await _camera.TakePhotoAsync()));

            _capturing = false;
        }

        private void OnSwitch()
        {
            Panel = (Panel == Panel.Front) ? Panel.Back : Panel.Front;
        }
    }
}
