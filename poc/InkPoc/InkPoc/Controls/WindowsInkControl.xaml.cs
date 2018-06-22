using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Threading.Tasks;
using InkPoc.Helpers;
using InkPoc.Services.Ink;
using Windows.Foundation;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Media.Imaging;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace InkPoc.Controls
{
    public sealed partial class WindowsInkControl : UserControl
    {
        private const bool DefaultEnableTouchValue = true;
        private const bool DefaultEnableMouseValue = true;

        private InkAsyncAnalyzer _analyzer;
        private InkCopyPasteService _copyPasteService;
        private InkFileService _fileService;
        private InkZoomService _zoomService;
        private StorageFile _imageFile;

        private readonly InkStrokesService StrokeService;
        private readonly InkLassoSelectionService LassoSelectionService;
        private readonly InkPointerDeviceService PointerDeviceService;
        private readonly InkSelectionRectangleService SelectionRectangleService;

        private InkZoomService ZoomService => _zoomService ?? (_zoomService = new InkZoomService(canvasScroll));
        private InkCopyPasteService CopyPasteService => _copyPasteService ?? (_copyPasteService = new InkCopyPasteService(StrokeService));
        private InkUndoRedoService UndoRedoService { get; set; }
        private InkFileService FileService => _fileService ?? (_fileService = new InkFileService(inkCanvas, StrokeService));
        private InkAsyncAnalyzer Analyzer => _analyzer ?? (_analyzer = new InkAsyncAnalyzer(inkCanvas, StrokeService));

        private InkTransformService TransformService { get; set; }

        private InkNodeSelectionService NodeSelectionService { get; set; }

        public event EventHandler OnCut;
        public event EventHandler OnCopy;
        public event EventHandler OnPaste;
        public event EventHandler OnStrokesOpened;
        public event EventHandler OnStrokesSaved;
        public event EventHandler OnImageExported;
        public event EventHandler OnTextAndShapesTransformed;
        public event EventHandler OnUndo;
        public event EventHandler OnRedo;
        public event EventHandler<float> OnZoomIn;
        public event EventHandler<float> OnZoomOut;
        public event EventHandler OnClearedAll;
        public event EventHandler<BitmapImage> OnImageOpened;
        public event EventHandler<BitmapImage> OnImageSaved;

        #region Properties
        public InkOptionCollection Options => (InkOptionCollection)GetValue(OptionsProperty);

        public static readonly DependencyProperty OptionsProperty = DependencyProperty.Register(nameof(Options), typeof(InkOptionCollection), typeof(WindowsInkControl), new PropertyMetadata(null, OnOptionsPropertyChanged));

        public bool EnableTouch
        {
            get => (bool)GetValue(EnableTouchProperty);
            set => SetValue(EnableTouchProperty, value);
        }

        public static readonly DependencyProperty EnableTouchProperty = DependencyProperty.Register(nameof(EnableTouch), typeof(bool), typeof(WindowsInkControl), new PropertyMetadata(DefaultEnableTouchValue, OnEnableTouchPropertyChanged));

        public bool EnableMouse
        {
            get => (bool)GetValue(EnableMouseProperty);
            set => SetValue(EnableMouseProperty, value);
        }

        public static readonly DependencyProperty EnableMouseProperty = DependencyProperty.Register(nameof(EnableMouse), typeof(bool), typeof(WindowsInkControl), new PropertyMetadata(DefaultEnableMouseValue, OnEnableMousePropertyChanged));



        public BitmapImage Image
        {
            get { return (BitmapImage)GetValue(ImageProperty); }
            set { SetValue(ImageProperty, value); }
        }

        public static readonly DependencyProperty ImageProperty = DependencyProperty.Register("Image", typeof(BitmapImage), typeof(WindowsInkControl), new PropertyMetadata(null));
        #endregion

        public WindowsInkControl()
        {
            this.InitializeComponent();
            SetValue(OptionsProperty, new InkOptionCollection());
            StrokeService = new InkStrokesService(inkCanvas.InkPresenter.StrokeContainer);
            SelectionRectangleService = new InkSelectionRectangleService(inkCanvas, selectionCanvas, StrokeService);
            LassoSelectionService = new InkLassoSelectionService(inkCanvas, selectionCanvas, StrokeService, SelectionRectangleService);
            PointerDeviceService = new InkPointerDeviceService(inkCanvas);
        }

        private static void OnOptionsPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
            => (d as WindowsInkControl)?.UpdateOptionsProperty();
        private static void OnEnableTouchPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
            => (d as WindowsInkControl)?.UpdateEnableTouchProperty();
        private static void OnEnableMousePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
            => (d as WindowsInkControl)?.UpdateEnableMouseProperty();

        private void UpdateOptionsProperty() => Options.CollectionChanged += OnOptionsCollectionChanged;
        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            inkCanvas.Width = Math.Max(inkCanvas.ActualWidth, 1000);
            inkCanvas.Height = Math.Max(inkCanvas.ActualHeight, 1000);
        }
        private void UpdateEnableTouchProperty() => PointerDeviceService.EnableTouch = EnableTouch;
        private void UpdateEnableMouseProperty() => PointerDeviceService.EnableMouse = EnableMouse;
        private void OnOptionsCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            foreach (var option in e.NewItems)
            {
                switch (option)
                {
                    case LassoSelectionInkOption lassoSelectionOption:
                        EnableLassoSelectionOption(lassoSelectionOption);
                        break;
                    case TouchInkingInkOption touchInkingOption:
                        EnableTouchInking(touchInkingOption);
                        break;
                    case MouseInkingInkOption mouseInkingOption:
                        EnableMouseInking(mouseInkingOption);
                        break;
                    case ZoomInkOption zoom:
                        EnableZoom(zoom);
                        break;
                    case CutCopyPasteInkOption cutCopyPaste:
                        EnableCutCopyPaste(cutCopyPaste);
                        break;
                    case UndoRedoInkOption undoRedo:
                        EnableUndoRedo(undoRedo);
                        break;
                    case OpenSaveStrokesInkOption openSaveStrokes:
                        EnableOpenSaveStrokes(openSaveStrokes);
                        break;
                    case SmartInkOptions smartInkOptions:
                        EnableSmartInkOptions(smartInkOptions);
                        break;
                    case ClearAllInkOption clearAllOption:
                        EnableClearAll(clearAllOption);
                        break;
                    case OpenImageInkOption openImageOption:
                        EnableOpenImage(openImageOption);
                        break;
                    case SaveImageInkOption saveImageInkOption:
                        EnableSaveImage(saveImageInkOption);
                        break;
                    default:
                        break;
                }
            }
        }

        private void EnableLassoSelectionOption(LassoSelectionInkOption lassoSelectionOption)
        {
            var lassoSelectionButton = lassoSelectionOption.LassoSelectionButton;
            lassoSelectionButton.Tag = LassoSelectionInkOption.LassoSelectionButtonTag;
            toolbar.Children.Insert(0, lassoSelectionButton);
        }

        private void EnableTouchInking(TouchInkingInkOption touchInkingOption)
        {
            var touchInkingButton = touchInkingOption.TouchInkingButton;
            touchInkingButton.DataContext = this;
            var binding = new Binding() { Path = new PropertyPath("EnableTouch"), Mode = BindingMode.TwoWay };
            touchInkingButton.SetBinding(InkToolbarCustomToggleButton.IsCheckedProperty, binding);
            toolbar.Children.Insert(0, touchInkingButton);
        }

        private void EnableMouseInking(MouseInkingInkOption mouseInkingOption)
        {
            var mouseInkingButton = mouseInkingOption.MouseInkingButton;
            mouseInkingButton.DataContext = this;
            var binding = new Binding() { Path = new PropertyPath("EnableMouse"), Mode = BindingMode.TwoWay };
            mouseInkingButton.SetBinding(InkToolbarCustomToggleButton.IsCheckedProperty, binding);
            toolbar.Children.Insert(0, mouseInkingButton);
        }

        private void EnableZoom(ZoomInkOption zoom)
        {
            commandBar.PrimaryCommands.Add(new AppBarSeparator());

            var zoomInButton = zoom.ZoomInButton;
            zoomInButton.Click += (sender, e) => ZoomIn();
            commandBar.PrimaryCommands.Add(zoomInButton);

            var zoomOutButton = zoom.ZoomOutButton;
            zoomOutButton.Click += (sender, e) => ZoomOut();
            commandBar.PrimaryCommands.Add(zoomOutButton);
        }

        private void EnableCutCopyPaste(CutCopyPasteInkOption cutOption)
        {
            commandBar.PrimaryCommands.Add(new AppBarSeparator());

            var cutButton = cutOption.CutButton;
            cutButton.Click += (sender, e) => Cut();
            commandBar.PrimaryCommands.Add(cutButton);

            var copyButton = cutOption.CopyButton;
            copyButton.Click += (sender, e) => Copy();
            commandBar.PrimaryCommands.Add(copyButton);

            var pasteButton = cutOption.PasteButton;
            pasteButton.Click += (sender, e) => Paste();
            commandBar.PrimaryCommands.Add(pasteButton);
        }

        private void EnableUndoRedo(UndoRedoInkOption undoRedo)
        {
            UndoRedoService = new InkUndoRedoService(inkCanvas, StrokeService);
            commandBar.PrimaryCommands.Add(new AppBarSeparator());
            var undoButton = undoRedo.UndoButton;
            undoButton.Click += (sender, e) => Undo();
            commandBar.PrimaryCommands.Add(undoButton);

            var redoButton = undoRedo.RedoButton;            
            redoButton.Click += (sender, e) => Redo();
            commandBar.PrimaryCommands.Add(redoButton);
        }

        private void EnableOpenSaveStrokes(OpenSaveStrokesInkOption fileImportExport)
        {
            commandBar.PrimaryCommands.Add(new AppBarSeparator());

            var openStrokesButton = fileImportExport.OpenStrokesButton;
            openStrokesButton.Click += async (sender, e) => await OpenStrokesAsync();
            commandBar.PrimaryCommands.Add(openStrokesButton);

            var saveStrokesButton = fileImportExport.SaveStrokesButton;
            saveStrokesButton.Click += async (sender, e) => await SaveStrokesAsync();
            commandBar.PrimaryCommands.Add(saveStrokesButton);
        }

        private void EnableSmartInkOptions(SmartInkOptions transformTextAndShapes)
        {
            TransformService = new InkTransformService(drawingCanvas, StrokeService);
            NodeSelectionService = new InkNodeSelectionService(inkCanvas, selectionCanvas, Analyzer, StrokeService, SelectionRectangleService);
            commandBar.PrimaryCommands.Add(new AppBarSeparator());

            var transformTextAndShapesButton = transformTextAndShapes.TransformTextAndShapesButton;
            transformTextAndShapesButton.Click += async (sender, e) => await TransformTextAndShapesAsync();
            commandBar.PrimaryCommands.Add(transformTextAndShapesButton);
        }

        private void EnableClearAll(ClearAllInkOption clearAllOption)
        {
            commandBar.PrimaryCommands.Add(new AppBarSeparator());
            var clearAllButton = clearAllOption.ClearAllButton;
            clearAllButton.Click += (sender, e) => ClearAll();
            commandBar.PrimaryCommands.Add(clearAllButton);
        }

        private void EnableOpenImage(OpenImageInkOption openImageInkOption)
        {
            commandBar.PrimaryCommands.Add(new AppBarSeparator());
            var openImageButton = openImageInkOption.OpenImageButton;
            openImageButton.Click += async (sender, e) => await OpenImageAsync();
            commandBar.PrimaryCommands.Add(openImageButton);
        }

        private void EnableSaveImage(SaveImageInkOption saveImageInkOption)
        {
            commandBar.PrimaryCommands.Add(new AppBarSeparator());
            var saveImageButton = saveImageInkOption.SaveImageButton;
            saveImageButton.Click += async (sender, e) => await SaveImageAsync();
            commandBar.PrimaryCommands.Add(saveImageButton);
        }

        public float ZoomIn()
        {
            var zoomFactor = ZoomService.ZoomIn();
            OnZoomIn?.Invoke(this, zoomFactor);
            return zoomFactor;
        }

        public float ZoomOut()
        {
            var zoomFactor = ZoomService.ZoomOut();
            OnZoomOut?.Invoke(this, zoomFactor);
            return zoomFactor;
        }

        public void Cut()
        {
            CopyPasteService.Cut();
            LassoSelectionService.ClearSelection();
            OnCut?.Invoke(this, EventArgs.Empty);
        }

        public void Copy()
        {
            CopyPasteService.Copy();
            LassoSelectionService.ClearSelection();
            OnCopy?.Invoke(this, EventArgs.Empty);
        }

        public void Paste()
        {
            CopyPasteService.Paste();
            LassoSelectionService.ClearSelection();
            OnPaste?.Invoke(this, EventArgs.Empty);
        }

        public bool Undo()
        {
            if (!IsOptionAvailable(typeof(UndoRedoInkOption)))
            {
                return false;
            }
            LassoSelectionService.ClearSelection();
            UndoRedoService.Undo();
            OnUndo?.Invoke(this, EventArgs.Empty);
            return true;
        }

        public bool Redo()
        {
            if (!IsOptionAvailable(typeof(UndoRedoInkOption)))
            {
                return false;
            }
            LassoSelectionService.ClearSelection();
            UndoRedoService.Redo();
            OnRedo?.Invoke(this, EventArgs.Empty);
            return true;
        }

        public async Task OpenStrokesAsync()
        {
            LassoSelectionService.ClearSelection();
            var fileLoaded = await FileService.LoadInkAsync();

            if (fileLoaded)
            {
                UndoRedoService?.Reset();
                OnStrokesOpened?.Invoke(this, EventArgs.Empty);
            }
        }

        public async Task SaveStrokesAsync()
        {
            LassoSelectionService.ClearSelection();
            await FileService.SaveInkAsync();
            OnStrokesSaved?.Invoke(this, EventArgs.Empty);
        }

        public async Task ExportAsImageAsync()
        {
            LassoSelectionService.ClearSelection();
            await FileService.ExportToImageAsync();
            OnImageExported?.Invoke(this, EventArgs.Empty);
        }

        public async Task<bool> TransformTextAndShapesAsync()
        {
            if (!IsOptionAvailable(typeof(SmartInkOptions)))
            {
                return false;
            }
            var result = await TransformService.TransformTextAndShapesAsync();
            if (result.TextAndShapes.Any())
            {
                NodeSelectionService?.ClearSelection();
                LassoSelectionService.ClearSelection();
                UndoRedoService?.AddOperation(new TransformUndoRedoOperation(result, StrokeService));
                OnTextAndShapesTransformed?.Invoke(this, EventArgs.Empty);
                return true;
            }
            return false;
        }

        public void ClearAll()
        {
            LassoSelectionService.ClearSelection();
            StrokeService.ClearStrokes();
            NodeSelectionService?.ClearSelection();
            UndoRedoService?.Reset();
            TransformService?.ClearTextAndShapes();
            OnClearedAll?.Invoke(this, EventArgs.Empty);
            _imageFile = null;
            Image = null;
        }

        public async Task OpenImageAsync()
        {
            var file = await ImageHelper.LoadImageFileAsync();
            var bitmapImage = await ImageHelper.GetBitmapFromImageAsync(file);

            if (file != null && bitmapImage != null)
            {
                _imageFile = file;
                Image = bitmapImage;
                ZoomService?.FitToSize(Image.PixelWidth, Image.PixelHeight);
                OnImageOpened?.Invoke(this, Image);
            }
        }

        public async Task SaveImageAsync()
        {
            var storageFile = await FileService.ExportToImageAsync(_imageFile);
            var bitmapImage = await ImageHelper.GetBitmapFromImageAsync(storageFile);
            OnImageSaved?.Invoke(this, bitmapImage);
        }

        private bool IsOptionAvailable(Type optionType) => Options.Any(o => o.GetType() == optionType);

        private void OnImageSizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (e.NewSize.Width > 0)
            {
                inkCanvas.Width = e.NewSize.Width;
            }

            if (e.NewSize.Height > 0)
            {
                inkCanvas.Height = e.NewSize.Height;
            }
        }

        private void OnInkToolbarActiveToolChanged(InkToolbar sender, object args)
        {
            switch (sender.ActiveTool)
            {
                case InkToolbarPencilButton pencilButton:
                case InkToolbarHighlighterButton highlighterButton:
                case InkToolbarPenButton pen:
                case InkToolbarEraserButton eraser:
                    LassoSelectionService.EndLassoSelectionConfig();
                    LassoSelectionService.ClearSelection();
                    break;
                case InkToolbarCustomToolButton customButton:
                    if (customButton.Tag as string == LassoSelectionInkOption.LassoSelectionButtonTag)
                    {
                        LassoSelectionService.StartLassoSelectionConfig();
                    }
                    break;
                default:
                    break;
            }
        }
    }
}
