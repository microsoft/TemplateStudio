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
        private bool enableTouch = true;
        private bool enableMouse = true;

        private BitmapImage image;

        private InkStrokesService strokesService;
        private InkPointerDeviceService pointerDeviceService;
        private InkFileService fileService;
        private InkZoomService zoomService;

        public InkDrawPictureViewViewModel()
        {
        }
        
        public void Initialize(
            InkStrokesService _strokesService,
            InkPointerDeviceService _pointerDeviceService,
            InkFileService _fileService,
            InkZoomService _zoomService)
        {
            strokesService = _strokesService;
            pointerDeviceService = _pointerDeviceService;
            fileService = _fileService;
            zoomService = _zoomService;

            pointerDeviceService.DetectPenEvent += (s, e) => EnableTouch = false;
        }

        public bool EnableTouch
        {
            get => enableTouch;
            set
            {
                Param_Setter(ref enableTouch, value);
                pointerDeviceService.EnableTouch = value;
            }
        }

        public bool EnableMouse
        {
            get => enableMouse;
            set
            {
                Param_Setter(ref enableMouse, value);
                pointerDeviceService.EnableMouse = value;
            }
        }

        public StorageFile ImageFile { get; set; }

        public BitmapImage Image
        {
            get => image;
            set => Param_Setter(ref image, value);
        }

        public async void OpenImage() => await OnLoadImageAsync();

        public async void SaveImage () => await fileService?.ExportToImageAsync(ImageFile);

        public void ZoomIn() => zoomService?.ZoomIn();

        public void ZoomOut () => zoomService?.ZoomOut();

        public void ResetZoom() => zoomService?.ResetZoom();

        public void FitToScreen() => zoomService?.FitToScreen();

        public void ClearAll()
        {
            strokesService?.ClearStrokes();
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
                zoomService?.FitToSize(Image.PixelWidth, Image.PixelHeight);
            }
        }
    }
}