using Param_RootNamespace.Services.Ink;
using Param_RootNamespace.Services.Ink.UndoRedo;
using System;
using System.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Param_RootNamespace.Views
{
    // For more information regarding Windows Ink documentation and samples see https://github.com/Microsoft/WindowsTemplateStudio/blob/master/docs/UWP/pages/ink.md
    public sealed partial class InkSmartCanvasViewPage : Page, System.ComponentModel.INotifyPropertyChanged
    {
        private bool lassoSelectionButtonIsChecked;
        private bool touchInkingButtonIsChecked = true;
        private bool mouseInkingButtonIsChecked = true;
        private bool penInkingButtonIsChecked = true;
        private bool transformTextAndShapesButtonIsEnabled;
        private bool undoButtonIsEnabled;
        private bool redoButtonIsEnabled;
        private bool saveInkFileButtonIsEnabled;
        private bool clearAllButtonIsEnabled;
        private InkStrokesService strokeService;
        private InkLassoSelectionService lassoSelectionService;
        private InkNodeSelectionService nodeSelectionService;
        private InkPointerDeviceService pointerDeviceService;
        private InkUndoRedoService undoRedoService;
        private InkTransformService transformService;
        private InkFileService fileService;

        public InkSmartCanvasViewPage()
        {
            InitializeComponent();

            Loaded += (sender, eventArgs) =>
            {
                SetCanvasSize();

                strokeService = new InkStrokesService(inkCanvas.InkPresenter);
                var analyzer = new InkAsyncAnalyzer(inkCanvas, strokeService);
                var selectionRectangleService = new InkSelectionRectangleService(inkCanvas, selectionCanvas, strokeService);

                lassoSelectionService = new InkLassoSelectionService(inkCanvas, selectionCanvas, strokeService, selectionRectangleService);
                nodeSelectionService = new InkNodeSelectionService(inkCanvas, selectionCanvas, analyzer, strokeService, selectionRectangleService);
                pointerDeviceService = new InkPointerDeviceService(inkCanvas);
                undoRedoService = new InkUndoRedoService(inkCanvas, strokeService);
                transformService = new InkTransformService(drawingCanvas, strokeService);
                fileService = new InkFileService(inkCanvas, strokeService);

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

        public bool PenInkingButtonIsChecked
        {
            get => penInkingButtonIsChecked;
            set
            {
                Param_Setter(ref penInkingButtonIsChecked, value);
                pointerDeviceService.EnablePen = value;
            }
        }

        public bool TransformTextAndShapesButtonIsEnabled
        {
            get => transformTextAndShapesButtonIsEnabled;
            set => Set(ref transformTextAndShapesButtonIsEnabled, value);
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
            var fileLoaded = await fileService.LoadInkAsync();

            if (fileLoaded)
            {
                transformService.ClearTextAndShapes();
                undoRedoService.Reset();
            }
        }

        private async void SaveInkFile_Click(object sender, RoutedEventArgs e)
        {
            ClearSelection();
            await fileService.SaveInkAsync();
        }

        private async void TransformTextAndShapes_Click(object sender, RoutedEventArgs e)
        {
            var result = await transformService.TransformTextAndShapesAsync();
            if (result.TextAndShapes.Any())
            {
                ClearSelection();
                undoRedoService.AddOperation(new TransformUndoRedoOperation(result, strokeService));
            }
        }

        private void ClearAll_Click(object sender, RoutedEventArgs e)
        {
            var strokes = strokeService?.GetStrokes().ToList();
            var textAndShapes = transformService?.GetTextAndShapes().ToList();
            ClearSelection();
            strokeService.ClearStrokes();
            transformService.ClearTextAndShapes();
            undoRedoService.AddOperation(new ClearStrokesAndShapesUndoRedoOperation(strokes, textAndShapes, strokeService, transformService));
        }

        private void RefreshEnabledButtons()
        {
            UndoButtonIsEnabled = undoRedoService.CanUndo;
            RedoButtonIsEnabled = undoRedoService.CanRedo;
            SaveInkFileButtonIsEnabled = strokeService.GetStrokes().Any();
            TransformTextAndShapesButtonIsEnabled = strokeService.GetStrokes().Any();
            ClearAllButtonIsEnabled = strokeService.GetStrokes().Any() || transformService.HasTextAndShapes();
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

        private void ClearSelection()
        {
            nodeSelectionService.ClearSelection();
            lassoSelectionService.ClearSelection();
        }
    }
}
