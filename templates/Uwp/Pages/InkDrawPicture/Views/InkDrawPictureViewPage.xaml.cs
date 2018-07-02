using Param_ItemNamespace.Services.Ink;
using Param_ItemNamespace.ViewModels;
using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Param_ItemNamespace.Views
{
    // For more information regarding Windows Ink documentation and samples see https://github.com/Microsoft/WindowsTemplateStudio/blob/master/docs/pages/ink.md
    public sealed partial class InkDrawPictureViewPage : Page
    {
        public InkDrawPictureViewPage()
        {
            InitializeComponent();
            Loaded += (s, e) =>
            {
                SetCanvasSize();
                image.SizeChanged += Image_SizeChanged;

                var strokeService = new InkStrokesService(inkCanvas.InkPresenter.StrokeContainer);

                ViewModel.Initialize(
                    strokeService,
                    new InkPointerDeviceService(inkCanvas),
                    new InkFileService(inkCanvas, strokeService),
                    new InkZoomService(canvasScroll));
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
    }
}
