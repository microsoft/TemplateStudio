using Param_ItemNamespace.Services.Ink;
using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Param_ItemNamespace.Views
{
    // For more information regarding Windows Ink documentation and samples see https://github.com/Microsoft/WindowsTemplateStudio/blob/master/docs/pages/ink.md
    public sealed partial class InkDrawViewPage : Page, System.ComponentModel.INotifyPropertyChanged
    {
        private InkStrokesService strokeService;
        private InkLassoSelectionService lassoSelectionService;
        private InkPointerDeviceService pointerDeviceService;
        private InkCopyPasteService copyPasteService;
        private InkUndoRedoService undoRedoService;
        private InkFileService fileService;
        private InkZoomService zoomService;

        public InkDrawViewPage()
        {
            InitializeComponent();
            Loaded += (sender, eventArgs) =>
            {
                SetCanvasSize();

                strokeService = new InkStrokesService(inkCanvas.InkPresenter.StrokeContainer);
                var selectionRectangleService = new InkSelectionRectangleService(inkCanvas, selectionCanvas, strokeService);
                lassoSelectionService = new InkLassoSelectionService(inkCanvas, selectionCanvas, strokeService, selectionRectangleService);
                pointerDeviceService = new InkPointerDeviceService(inkCanvas);
                copyPasteService = new InkCopyPasteService(strokeService);
                undoRedoService = new InkUndoRedoService(inkCanvas, strokeService);
                fileService = new InkFileService(inkCanvas, strokeService);
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

        private void LassoSelection_Checked(object sender, RoutedEventArgs e) => lassoSelectionService?.StartLassoSelectionConfig();

        private void LassoSelection_Unchecked(object sender, RoutedEventArgs e) => lassoSelectionService?.EndLassoSelectionConfig();

        private void TouchInking_Checked(object sender, RoutedEventArgs e) => pointerDeviceService.EnableTouch = true;

        private void TouchInking_Unchecked(object sender, RoutedEventArgs e) => pointerDeviceService.EnableTouch = false;

        private void MouseInking_Checked(object sender, RoutedEventArgs e) => pointerDeviceService.EnableMouse = true;

        private void MouseInking_Unchecked(object sender, RoutedEventArgs e) => pointerDeviceService.EnableMouse = false;

        private void ZoomIn_Click(object sender, RoutedEventArgs e) => zoomService?.ZoomIn();

        private void ZoomOut_Click(object sender, RoutedEventArgs e) => zoomService?.ZoomOut();

        private void Copy_Click(object sender, RoutedEventArgs e) => copyPasteService?.Copy();

        private void Cut_Click(object sender, RoutedEventArgs e)
        {
            copyPasteService?.Cut();
            ClearSelection();
        }

        private void Paste_Click(object sender, RoutedEventArgs e)
        {
            copyPasteService?.Paste();
            ClearSelection();
        }

        private void Undo_Click(object sender, RoutedEventArgs e)
        {
            ClearSelection();
            undoRedoService?.Undo();
        }

        private void Redo_Click(object sender, RoutedEventArgs e)
        {
            ClearSelection();
            undoRedoService?.Redo();
        }

        private async void LoadInkFile_Click(object sender, RoutedEventArgs e)
        {
            ClearSelection();
            var fileLoaded = await fileService?.LoadInkAsync();

            if (fileLoaded)
            {
                undoRedoService?.Reset();
            }
        }

        private async void SaveInkFile_Click(object sender, RoutedEventArgs e)
        {
            ClearSelection();
            await fileService?.SaveInkAsync();
        }

        private async void ExportAsImage_Click(object sender, RoutedEventArgs e)
        {
            ClearSelection();
            await fileService?.ExportToImageAsync();
        }

        private void ClearAll_Click(object sender, RoutedEventArgs e)
        {
            ClearSelection();
            strokeService?.ClearStrokes();
            undoRedoService?.Reset();
        }

        private void ClearSelection() => lassoSelectionService?.ClearSelection();
    }
}
