using InkPoc.Services.Ink;
using InkPoc.ViewModels;
using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace InkPoc.Views
{
    public sealed partial class PaintImagePage : Page
    {
        public PaintImageViewModel ViewModel { get; } = new PaintImageViewModel();

        public PaintImagePage()
        {
            InitializeComponent();
            Loaded += (s, e) => SetCanvasSize();
            image.SizeChanged += Image_SizeChanged;

            var strokeService = new InkStrokesService(inkCanvas.InkPresenter.StrokeContainer);

            ViewModel = new PaintImageViewModel(
                strokeService,
                new InkPointerDeviceService(inkCanvas),
                new InkFileService(inkCanvas, strokeService),
                new InkZoomService(canvasScroll));
        }
        
        private void SetCanvasSize()
        {
            inkCanvas.Width = Math.Max(canvasScroll.ViewportWidth, 1000);
            inkCanvas.Height = Math.Max(canvasScroll.ViewportHeight, 1000);
        }

        private void Image_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if(e.NewSize.Height == 0 || e.NewSize.Width == 0)
            {
                SetCanvasSize();
            }
            else
            {
                inkCanvas.Width = e.NewSize.Width;
                inkCanvas.Height = e.NewSize.Height;
            }
        }
    }
}
