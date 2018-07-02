using Param_ItemNamespace.Services.Ink;
using Param_ItemNamespace.Helpers;
using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.Storage;

namespace Param_ItemNamespace.Views
{
    // For more information regarding Windows Ink documentation and samples see https://github.com/Microsoft/WindowsTemplateStudio/blob/master/docs/pages/ink.md
    public sealed partial class InkDrawPictureViewPage : Page, System.ComponentModel.INotifyPropertyChanged
    {
        private StorageFile imageFile;

        private InkStrokesService strokesService;
        private InkPointerDeviceService pointerDeviceService;
        private InkFileService fileService;
        private InkZoomService zoomService;

        public InkDrawPictureViewPage()
        {
            InitializeComponent();
            Loaded += (sender, eventArgs) =>
            {
                SetCanvasSize();
                image.SizeChanged += Image_SizeChanged;

                strokesService = new InkStrokesService(inkCanvas.InkPresenter.StrokeContainer);
                pointerDeviceService = new InkPointerDeviceService(inkCanvas);
                fileService = new InkFileService(inkCanvas, strokesService);
                zoomService = new InkZoomService(canvasScroll);

                touchInkingButton.IsChecked = true;
                mouseInkingButton.IsChecked = true;
                pointerDeviceService.DetectPenEvent += (s, e) => touchInkingButton.IsChecked = false;
            };
        }

        private void SetCanvasSize()
        {
            inkCanvas.Width = Math.Max(canvasScroll.ViewportWidth, 1000);
            inkCanvas.Height = Math.Max(canvasScroll.ViewportHeight, 1000);
        }

        private void Image_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (e.NewSize.Height == 0 || e.NewSize.Width == 0)
            {
                SetCanvasSize();
            }
            else
            {
                inkCanvas.Width = e.NewSize.Width;
                inkCanvas.Height = e.NewSize.Height;
            }
        }

        private void TouchInking_Checked(object sender, RoutedEventArgs e) => pointerDeviceService.EnableTouch = true;

        private void TouchInking_Unchecked(object sender, RoutedEventArgs e) => pointerDeviceService.EnableTouch = false;

        private void MouseInking_Checked(object sender, RoutedEventArgs e) => pointerDeviceService.EnableMouse = true;

        private void MouseInking_Unchecked(object sender, RoutedEventArgs e) => pointerDeviceService.EnableMouse = false;

        private void ZoomIn_Click(object sender, RoutedEventArgs e) => zoomService?.ZoomIn();

        private void ZoomOut_Click(object sender, RoutedEventArgs e) => zoomService?.ZoomOut();

        private void ResetZoom_Click(object sender, RoutedEventArgs e) => zoomService?.ResetZoom();

        private void FitToScreen_Click(object sender, RoutedEventArgs e) => zoomService?.FitToScreen();

        private async void LoadImage_Click(object sender, RoutedEventArgs e)
        {
            var file = await ImageHelper.LoadImageFileAsync();
            var bitmapImage = await ImageHelper.GetBitmapFromImageAsync(file);

            if (file != null && bitmapImage != null)
            {
                ClearAll();
                imageFile = file;
                image.Source = bitmapImage;
                zoomService?.FitToSize(bitmapImage.PixelWidth, bitmapImage.PixelHeight);
            }
        }

        private async void SaveImage_Click(object sender, RoutedEventArgs e) => await fileService?.ExportToImageAsync(imageFile);

        private void ClearAll_Click(object sender, RoutedEventArgs e) => ClearAll();

        private void ClearAll()
        {
            strokesService?.ClearStrokes();
            imageFile = null;
            image.Source = null;
        }
    }
}
