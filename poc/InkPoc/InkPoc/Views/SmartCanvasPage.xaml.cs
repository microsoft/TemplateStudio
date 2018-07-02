using InkPoc.Services.Ink;
using InkPoc.ViewModels;
using System;
using Windows.UI.Xaml.Controls;

namespace InkPoc.Views
{
    public sealed partial class SmartCanvasPage : Page
    {
        public SmartCanvasViewModel ViewModel { get; } = new SmartCanvasViewModel();

        public SmartCanvasPage()
        {
            InitializeComponent();
            Loaded += (s, e) => SetCanvasSize();

            var strokeService = new InkStrokesService(inkCanvas.InkPresenter.StrokeContainer);
            var analyzer = new InkAsyncAnalyzer(inkCanvas, strokeService);
            var selectionRectangleService = new InkSelectionRectangleService(inkCanvas, selectionCanvas, strokeService);

            ViewModel = new SmartCanvasViewModel(
                strokeService,
                new InkLassoSelectionService(inkCanvas, selectionCanvas, strokeService, selectionRectangleService),
                new InkNodeSelectionService(inkCanvas, selectionCanvas, analyzer, strokeService, selectionRectangleService),
                new InkPointerDeviceService(inkCanvas),
                new InkUndoRedoService(inkCanvas, strokeService),
                new InkTransformService(drawingCanvas,strokeService),
                new InkFileService(inkCanvas, strokeService));
        }

        private void SetCanvasSize()
        {
            inkCanvas.Width = Math.Max(inkCanvas.ActualWidth, 1000);
            inkCanvas.Height = Math.Max(inkCanvas.ActualHeight, 1000);
        }
    }
}
