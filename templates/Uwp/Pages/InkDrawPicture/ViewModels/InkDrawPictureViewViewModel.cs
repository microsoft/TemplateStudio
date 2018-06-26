using Param_ItemNamespace.Services.Ink;
using System.Threading.Tasks;
using System.Windows.Input;
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

        public ICommand LoadImageCommand => loadImageCommand
            ?? (loadImageCommand = new RelayCommand(async () => await OnLoadImageAsync()));

        public ICommand SaveImageCommand => saveImageCommand
            ?? (saveImageCommand = new RelayCommand(async () => await OnSaveImageAsync()));

        public ICommand ZoomInCommand => zoomInCommand
            ?? (zoomInCommand = new RelayCommand(() => zoomService?.ZoomIn()));

        public ICommand ZoomOutCommand => zoomOutCommand
            ?? (zoomOutCommand = new RelayCommand(() => zoomService?.ZoomOut()));

        public ICommand ResetZoomCommand => resetZoomCommand
            ?? (resetZoomCommand = new RelayCommand(() => zoomService?.ResetZoom()));

        public ICommand FitToScreenCommand => fitToScreenCommand
            ?? (fitToScreenCommand = new RelayCommand(() => zoomService?.FitToScreen()));

        public ICommand ClearAllCommand => clearAllCommand
           ?? (clearAllCommand = new RelayCommand(ClearAll));


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

        private async Task OnSaveImageAsync()
        {
            await fileService?.ExportToImageAsync(ImageFile);
        }

        private void ClearAll()
        {
            strokesService?.ClearStrokes();
            ImageFile = null;
            Image = null;
        }
    }
}
