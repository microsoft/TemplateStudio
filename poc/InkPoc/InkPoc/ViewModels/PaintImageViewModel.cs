using InkPoc.Helpers;
using InkPoc.Services.Ink;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Storage;
using Windows.UI.Xaml.Media.Imaging;

namespace InkPoc.ViewModels
{
    public class PaintImageViewModel : Observable
    {
        private bool enableTouch = true;
        private bool enableMouse = true;

        private BitmapImage image;

        private readonly InkStrokesService strokesService;
        private readonly InkPointerDeviceService pointerDeviceService;
        private readonly InkFileService fileService;
        private readonly InkZoomService zoomService;

        private RelayCommand loadImageCommand;
        private RelayCommand saveImageCommand;
        private RelayCommand clearAllCommand;
        private RelayCommand zoomInCommand;
        private RelayCommand zoomOutCommand;
        private RelayCommand resetZoomCommand;
        private RelayCommand fitToScreenCommand;

        public PaintImageViewModel()
        {
        }
        
        public PaintImageViewModel(
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
                Set(ref enableTouch, value);
                pointerDeviceService.EnableTouch = value;
            }
        }

        public bool EnableMouse
        {
            get => enableMouse;
            set
            {
                Set(ref enableMouse, value);
                pointerDeviceService.EnableMouse = value;
            }
        }

        public StorageFile ImageFile { get; set; }

        public BitmapImage Image
        {
            get => image;
            set => Set(ref image, value);
        }

        public RelayCommand LoadImageCommand => loadImageCommand
            ?? (loadImageCommand = new RelayCommand(async () => await OnLoadImageAsync()));

        public RelayCommand SaveImageCommand => saveImageCommand
            ?? (saveImageCommand = new RelayCommand(async () => await OnSaveImageAsync()));

        public RelayCommand ZoomInCommand => zoomInCommand
            ?? (zoomInCommand = new RelayCommand(() => zoomService.ZoomIn()));

        public RelayCommand ZoomOutCommand => zoomOutCommand
            ?? (zoomOutCommand = new RelayCommand(() => zoomService.ZoomOut()));

        public RelayCommand ResetZoomCommand => resetZoomCommand
            ?? (resetZoomCommand = new RelayCommand(() => zoomService.ResetZoom()));

        public RelayCommand FitToScreenCommand => fitToScreenCommand
            ?? (fitToScreenCommand = new RelayCommand(() => zoomService.FitToScreen()));

        private async Task OnLoadImageAsync()
        {
            var file = await ImageHelper.LoadImageFileAsync();
            var bitmapImage = await ImageHelper.GetBitmapFromImageAsync(file);

            if (file != null && bitmapImage != null)
            {
                ClearAll();
                ImageFile = file;
                Image = bitmapImage;
                zoomService.FitToSize(Image.PixelWidth, Image.PixelHeight);
            }
        }

        private async Task OnSaveImageAsync()
        {
            await fileService.ExportToImageAsync(ImageFile);
        }

        public RelayCommand ClearAllCommand => clearAllCommand
           ?? (clearAllCommand = new RelayCommand(ClearAll));

        private void ClearAll()
        {
            strokesService.ClearStrokes();
            ImageFile = null;
            Image = null;
        }
    }
}
