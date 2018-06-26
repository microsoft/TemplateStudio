using Param_ItemNamespace.Services.Ink;
using Param_ItemNamespace.Helpers;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.Storage;
using Windows.UI.Xaml.Media.Imaging;
using Prism.Commands;
using Prism.Windows.Mvvm;

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
            LoadImageCommand = new DelegateCommand(async () => await OnLoadImageAsync());
            SaveImageCommand = new DelegateCommand(async () => await OnSaveImageAsync());
            ZoomInCommand = new DelegateCommand(() => zoomService?.ZoomIn());
            ZoomOutCommand = new DelegateCommand(() => zoomService?.ZoomOut());
            ResetZoomCommand = new DelegateCommand(() => zoomService?.ResetZoom());
            FitToScreenCommand = new DelegateCommand(() => zoomService?.FitToScreen());
            ClearAllCommand = new DelegateCommand(ClearAll);
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
                SetProperty(ref enableTouch, value);
                pointerDeviceService.EnableTouch = value;
            }
        }

        public bool EnableMouse
        {
            get => enableMouse;
            set
            {
                SetProperty(ref enableMouse, value);
                pointerDeviceService.EnableMouse = value;
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
