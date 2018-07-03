using Param_ItemNamespace.Services.Ink;
using Param_ItemNamespace.Helpers;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.Storage;
using Windows.UI.Xaml.Media.Imaging;
using Prism.Commands;

namespace Param_ItemNamespace.ViewModels
{
    public class InkDrawPictureViewViewModel : System.ComponentModel.INotifyPropertyChanged
    {
        private InkStrokesService _strokeService;
        private InkPointerDeviceService _pointerDeviceService;
        private InkFileService _fileService;
        private InkZoomService _zoomService;

        private bool enableTouch = true;
        private bool enableMouse = true;

        private BitmapImage image;

        public InkDrawPictureViewViewModel()
        {
            LoadImageCommand = new DelegateCommand(async () => await OnLoadImageAsync());
            SaveImageCommand = new DelegateCommand(async () => await OnSaveImageAsync());
            ZoomInCommand = new DelegateCommand(() => _zoomService?.ZoomIn());
            ZoomOutCommand = new DelegateCommand(() => _zoomService?.ZoomOut());
            ResetZoomCommand = new DelegateCommand(() => _zoomService?.ResetZoom());
            FitToScreenCommand = new DelegateCommand(() => _zoomService?.FitToScreen());
            ClearAllCommand = new DelegateCommand(ClearAll);
        }

        public void Initialize(
            InkStrokesService strokeService,
            InkPointerDeviceService pointerDeviceService,
            InkFileService fileService,
            InkZoomService zoomService)
        {
            _strokeService = strokeService;
            _pointerDeviceService = pointerDeviceService;
            _fileService = fileService;
            _zoomService = zoomService;

            _pointerDeviceService.DetectPenEvent += (s, e) => EnableTouch = false;
        }

        public bool EnableTouch
        {
            get => enableTouch;
            set
            {
                SetProperty(ref enableTouch, value);
                _pointerDeviceService.EnableTouch = value;
            }
        }

        public bool EnableMouse
        {
            get => enableMouse;
            set
            {
                SetProperty(ref enableMouse, value);
                _pointerDeviceService.EnableMouse = value;
            }
        }

        public StorageFile ImageFile { get; set; }

        public BitmapImage Image
        {
            get => image;
            set => SetProperty(ref image, value);
        }

        public ICommand LoadImageCommand { get; }

        public ICommand SaveImageCommand { get; }

        public ICommand ClearAllCommand { get; }

        public ICommand ZoomInCommand { get; }

        public ICommand ZoomOutCommand { get; }

        public ICommand ResetZoomCommand { get; }

        public ICommand FitToScreenCommand { get; }

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

        private async Task OnSaveImageAsync()
        {
            await _fileService?.ExportToImageAsync(ImageFile);
        }

        private void ClearAll()
        {
            _strokeService?.ClearStrokes();
            ImageFile = null;
            Image = null;
        }
    }
}
