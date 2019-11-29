using Param_RootNamespace.Services.Ink;
using Param_RootNamespace.Services.Ink.UndoRedo;
using System;
using System.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Param_RootNamespace.Views
{
    // For more information regarding Windows Ink documentation and samples see https://github.com/Microsoft/WindowsTemplateStudio/blob/master/docs/UWP/pages/ink.md
    public sealed partial class InkDrawViewPage : Page, System.ComponentModel.INotifyPropertyChanged
    {
        private bool lassoSelectionButtonIsChecked;
        private bool touchInkingButtonIsChecked = true;
        private bool mouseInkingButtonIsChecked = true;
        private bool cutButtonIsEnabled;
        private bool copyButtonIsEnabled;
        private bool pasteButtonIsEnabled;
        private bool undoButtonIsEnabled;
        private bool redoButtonIsEnabled;
        private bool saveInkFileButtonIsEnabled;
        private bool exportAsImageButtonIsEnabled;
        private bool clearAllButtonIsEnabled;
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

                strokeService = new InkStrokesService(inkCanvas.InkPresenter);
                var selectionRectangleService = new InkSelectionRectangleService(inkCanvas, selectionCanvas, strokeService);
                lassoSelectionService = new InkLassoSelectionService(inkCanvas, selectionCanvas, strokeService, selectionRectangleService);
                pointerDeviceService = new InkPointerDeviceService(inkCanvas);
                copyPasteService = new InkCopyPasteService(strokeService);
                undoRedoService = new InkUndoRedoService(inkCanvas, strokeService);
                fileService = new InkFileService(inkCanvas, strokeService);
                zoomService = new InkZoomService(canvasScroll);

                strokeService.CopyStrokesEvent += (s, e) => RefreshEnabledButtons();
                strokeService.SelectStrokesEvent += (s, e) => RefreshEnabledButtons();
                strokeService.ClearStrokesEvent += (s, e) => RefreshEnabledButtons();
                undoRedoService.UndoEvent += (s, e) => RefreshEnabledButtons();
                undoRedoService.RedoEvent += (s, e) => RefreshEnabledButtons();
                undoRedoService.AddUndoOperationEvent += (s, e) => RefreshEnabledButtons();
                pointerDeviceService.DetectPenEvent += (s, e) => TouchInkingButtonIsChecked = false;
            };
        }

        public bool LassoSelectionButtonIsChecked
        {
            get => lassoSelectionButtonIsChecked;
            set
            {
                Param_Setter(ref lassoSelectionButtonIsChecked, value);
                ConfigLassoSelection(value);
            }
        }

        public bool TouchInkingButtonIsChecked
        {
            get => touchInkingButtonIsChecked;
            set
            {
                Param_Setter(ref touchInkingButtonIsChecked, value);
                pointerDeviceService.EnableTouch = value;
            }
        }

        public bool MouseInkingButtonIsChecked
        {
            get => mouseInkingButtonIsChecked;
            set
            {
                Param_Setter(ref mouseInkingButtonIsChecked, value);
                pointerDeviceService.EnableMouse = value;
            }
        }

        public bool CutButtonIsEnabled
        {
            get => cutButtonIsEnabled;
            set => Set(ref cutButtonIsEnabled, value);
        }

        public bool CopyButtonIsEnabled
        {
            get => copyButtonIsEnabled;
            set => Set(ref copyButtonIsEnabled, value);
        }

        public bool PasteButtonIsEnabled
        {
            get => pasteButtonIsEnabled;
            set => Set(ref pasteButtonIsEnabled, value);
        }

        public bool UndoButtonIsEnabled
        {
            get => undoButtonIsEnabled;
            set => Set(ref undoButtonIsEnabled, value);
        }

        public bool RedoButtonIsEnabled
        {
            get => redoButtonIsEnabled;
            set => Set(ref redoButtonIsEnabled, value);
        }

        public bool SaveInkFileButtonIsEnabled
        {
            get => saveInkFileButtonIsEnabled;
            set => Set(ref saveInkFileButtonIsEnabled, value);
        }

        public bool ExportAsImageButtonIsEnabled
        {
            get => exportAsImageButtonIsEnabled;
            set => Set(ref exportAsImageButtonIsEnabled, value);
        }

        public bool ClearAllButtonIsEnabled
        {
            get => clearAllButtonIsEnabled;
            set => Set(ref clearAllButtonIsEnabled, value);
        }

        private void SetCanvasSize()
        {
            inkCanvas.Width = Math.Max(canvasScroll.ViewportWidth, 1000);
            inkCanvas.Height = Math.Max(canvasScroll.ViewportHeight, 1000);
        }

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
            var strokes = strokeService?.GetStrokes().ToList();
            ClearSelection();
            strokeService?.ClearStrokes();
            undoRedoService?.AddOperation(new RemoveStrokeUndoRedoOperation(strokes, strokeService));
        }

        private void RefreshEnabledButtons()
        {
            CutButtonIsEnabled = copyPasteService.CanCut;
            CopyButtonIsEnabled = copyPasteService.CanCopy;
            PasteButtonIsEnabled = copyPasteService.CanPaste;
            UndoButtonIsEnabled = undoRedoService.CanUndo;
            RedoButtonIsEnabled = undoRedoService.CanRedo;
            SaveInkFileButtonIsEnabled = strokeService.GetStrokes().Any();
            ExportAsImageButtonIsEnabled = strokeService.GetStrokes().Any();
            ClearAllButtonIsEnabled = strokeService.GetStrokes().Any();
        }

        private void ConfigLassoSelection(bool enableLasso)
        {
            if (enableLasso)
            {
                lassoSelectionService?.StartLassoSelectionConfig();
            }
            else
            {
                lassoSelectionService?.EndLassoSelectionConfig();
            }
        }

        private void ClearSelection() => lassoSelectionService?.ClearSelection();
    }
}
