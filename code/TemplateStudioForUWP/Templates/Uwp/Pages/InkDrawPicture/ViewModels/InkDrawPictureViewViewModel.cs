using Param_RootNamespace.Services.Ink;
using Param_RootNamespace.Helpers;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Linq;
using Windows.Storage;
using Windows.UI.Xaml.Media.Imaging;

namespace Param_RootNamespace.ViewModels
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

        private ICommand loadImageCommand;
        private ICommand saveImageCommand;
        private ICommand clearAllCommand;
        private ICommand zoomInCommand;
        private ICommand zoomOutCommand;
        private ICommand resetZoomCommand;
        private ICommand fitToScreenCommand;

        public InkDrawPictureViewViewModel()
        {
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

            _strokeService.StrokesCollected += (s, e) => RefreshCommands();
            _strokeService.StrokesErased += (s, e) => RefreshCommands();
            _strokeService.ClearStrokesEvent += (s, e) => RefreshCommands();

            _pointerDeviceService.DetectPenEvent += (s, e) => EnableTouch = false;
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
            set
            {
                Param_Setter(ref image, value);
                RefreshCommands();
            }
        }

        public ICommand LoadImageCommand => loadImageCommand
            ?? (loadImageCommand = new RelayCommand(async () => await OnLoadImageAsync()));

        public ICommand SaveImageCommand => saveImageCommand
            ?? (saveImageCommand = new RelayCommand(async () => await OnSaveImageAsync(), CanSaveImage));

        public ICommand ZoomInCommand => zoomInCommand
            ?? (zoomInCommand = new RelayCommand(() => _zoomService?.ZoomIn()));

        public ICommand ZoomOutCommand => zoomOutCommand
            ?? (zoomOutCommand = new RelayCommand(() => _zoomService?.ZoomOut()));

        public ICommand ResetZoomCommand => resetZoomCommand
            ?? (resetZoomCommand = new RelayCommand(() => _zoomService?.ResetZoom()));

        public ICommand FitToScreenCommand => fitToScreenCommand
            ?? (fitToScreenCommand = new RelayCommand(() => _zoomService?.FitToScreen()));

        public ICommand ClearAllCommand => clearAllCommand
           ?? (clearAllCommand = new RelayCommand(ClearAll, CanClearAll));

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

        private bool CanSaveImage()
        {
            return Image != null
                && _strokeService != null
                && _strokeService.GetStrokes().Any();
        }

        private bool CanClearAll()
        {
            return Image != null
                || (_strokeService != null
                    && _strokeService.GetStrokes().Any());
        }

        private void ClearAll()
        {
            _strokeService?.ClearStrokes();
            ImageFile = null;
            Image = null;
        }

        private void RefreshCommands()
        {
        }
    }
}
