using System.Threading.Tasks;
using Param_ItemNamespace.Helpers;
using Param_ItemNamespace.Services.Ink;
using Caliburn.Micro;
using Windows.Storage;
using Windows.UI.Xaml.Media.Imaging;

namespace Param_ItemNamespace.ViewModels
{
    public class InkDrawPictureViewViewModel : System.ComponentModel.INotifyPropertyChanged
    {
        private InkStrokesService _strokesService;
        private InkPointerDeviceService _pointerDeviceService;
        private InkFileService _fileService;
        private InkZoomService _zoomService;

        private bool enableTouch = true;
        private bool enableMouse = true;

        private BitmapImage image;

        public InkDrawPictureViewViewModel()
        {
        }

        public void Initialize(
            InkStrokesService strokesService,
            InkPointerDeviceService pointerDeviceService,
            InkFileService fileService,
            InkZoomService zoomService)
        {
            _strokesService = strokesService;
            _pointerDeviceService = pointerDeviceService;
            _fileService = fileService;
            _zoomService = zoomService;

            pointerDeviceService.DetectPenEvent += (s, e) => EnableTouch = false;
        }

        public bool EnableTouch
        {
            get => enableTouch;
            set
            {
                Param_Setter(ref enableTouch, value);
                _pointerDeviceService.EnableTouch = value;
            }
        }

        public bool EnableMouse
        {
            get => enableMouse;
            set
            {
                Param_Setter(ref enableMouse, value);
                _pointerDeviceService.EnableMouse = value;
            }
        }

        public StorageFile ImageFile { get; set; }

        public BitmapImage Image
        {
            get => image;
            set => Param_Setter(ref image, value);
        }

        public async void OpenImage() => await OnLoadImageAsync();

        public async void SaveImage() => await _fileService?.ExportToImageAsync(ImageFile);

        public void ZoomIn() => _zoomService?.ZoomIn();

        public void ZoomOut() => _zoomService?.ZoomOut();

        public void ResetZoom() => _zoomService?.ResetZoom();

        public void FitToScreen() => _zoomService?.FitToScreen();

        public void ClearAll()
        {
            _strokesService?.ClearStrokes();
            ImageFile = null;
            Image = null;
        }

        private async Task OnLoadImageAsync()
        {
            var file = await ImageHelper.LoadImageFileAsync();
            var bitmapImage = await ImageHelper.GetBitmapFromImageAsync(file);

            if (file != null && bitmapImage != null)
            {
                ClearAll();
                ImageFile = file;
                Image = bitmapImage;
                _zoomService?.FitToSize(Image.PixelWidth, Image.PixelHeight);
            }
        }
    }
}
