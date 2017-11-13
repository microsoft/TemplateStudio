using Plugin.Media;
using Plugin.Media.Abstractions;
using System.Threading.Tasks;
using System.Windows.Input;
using WtsXamarin.Helpers;
using Xamarin.Forms;

namespace WtsXamarin.ViewModels
{
    public class CameraViewModel : Observable
    {
        private bool _isCameraAvailable;
        private IMedia _media = CrossMedia.Current;
        private string _status;
        private ImageSource _imageSource;
        public ICommand _takePhotoCommand;

        public CameraViewModel()
        {
            InitializeCamera();
        }
        
        public string Status
        {
            get => _status;
            set => Set(ref _status, value); 
        }

        public ImageSource ImageSource
        {
            get => _imageSource;
            set => Set(ref _imageSource, value);
        }

        public ICommand TakePhotoCommand
        {
            get => _takePhotoCommand ?? (_takePhotoCommand = new Command(async () => await OnTakePhoto(), () => _isCameraAvailable));
        }

        
        private async Task OnTakePhoto()
        {
            StoreCameraMediaOptions options = new StoreCameraMediaOptions
            {
                Directory = "CameraPageFolder",
                PhotoSize = PhotoSize.Medium,
                Name = "capture.jpg"
            };

            using (var mediaFile = await _media.TakePhotoAsync(options))
            {
                if (mediaFile == null)
                {
                    Status = "Photo could not be saved";
                    ImageSource = null;
                }
                else
                {
                    Status = "";
                    ImageSource = ImageSource.FromFile(mediaFile.Path);
                }
            }
        }


        private async void InitializeCamera()
        {
            _isCameraAvailable = await _media.Initialize() && _media.IsCameraAvailable && _media.IsTakePhotoSupported;
            (TakePhotoCommand as Command).ChangeCanExecute();
            Status = _isCameraAvailable ? "Press button to take photo" : "Camera is not available";
        }
    }
}