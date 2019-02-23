using System.Linq;
using Param_RootNamespace.Services.Ink;
using Param_RootNamespace.Services.Ink.UndoRedo;
using Prism.Commands;
using System.Windows.Input;

namespace Param_RootNamespace.ViewModels
{
    public class InkDrawViewViewModel : System.ComponentModel.INotifyPropertyChanged
    {
        private InkStrokesService _strokeService;
        private InkLassoSelectionService _lassoSelectionService;
        private InkPointerDeviceService _pointerDeviceService;
        private InkCopyPasteService _copyPasteService;
        private InkUndoRedoService _undoRedoService;
        private InkFileService _fileService;
        private InkZoomService _zoomService;

        private bool enableTouch = true;
        private bool enableMouse = true;
        private bool enableLassoSelection;

        public InkDrawViewViewModel()
        {
            ZoomInCommand = new DelegateCommand(() => _zoomService?.ZoomIn());
            ZoomOutCommand = new DelegateCommand(() => _zoomService?.ZoomOut());
            UndoCommand = new DelegateCommand(Undo, CanUndo);
            RedoCommand = new DelegateCommand(Redo, CanRedo);
            CutCommand = new DelegateCommand(Cut, CanCut);
            CopyCommand = new DelegateCommand(() => _copyPasteService?.Copy(), CanCopy);
            PasteCommand = new DelegateCommand(Paste, CanPaste);
            LoadInkFileCommand = new DelegateCommand(LoadInkFile);
            SaveInkFileCommand = new DelegateCommand(SaveInkFile, CanSaveInkFile);
            ExportAsImageCommand = new DelegateCommand(ExportAsImage, CanExportAsImage);
            ClearAllCommand = new DelegateCommand(ClearAll, CanClearAll);
        }

        public void Initialize(
            InkStrokesService strokeService,
            InkLassoSelectionService lassoSelectionService,
            InkPointerDeviceService pointerDeviceService,
            InkCopyPasteService copyPasteService,
            InkUndoRedoService undoRedoService,
            InkFileService fileService,
            InkZoomService zoomService)
        {
            _strokeService = strokeService;
            _lassoSelectionService = lassoSelectionService;
            _pointerDeviceService = pointerDeviceService;
            _copyPasteService = copyPasteService;
            _undoRedoService = undoRedoService;
            _fileService = fileService;
            _zoomService = zoomService;

            _strokeService.CopyStrokesEvent += (s, e) => RefreshCommands();
            _strokeService.SelectStrokesEvent += (s, e) => RefreshCommands();
            _strokeService.ClearStrokesEvent += (s, e) => RefreshCommands();
            _undoRedoService.UndoEvent += (s, e) => RefreshCommands();
            _undoRedoService.RedoEvent += (s, e) => RefreshCommands();
            _undoRedoService.AddUndoOperationEvent += (s, e) => RefreshCommands();
            _pointerDeviceService.DetectPenEvent += (s, e) => EnableTouch = false;
        }

        public bool EnableTouch
        {
            get => enableTouch;
            set
            {
                SetProperty(ref enableTouch, value);
                _pointerDeviceService.EnableTouch = value;
            }
        }

        public bool EnableMouse
        {
            get => enableMouse;
            set
            {
                SetProperty(ref enableMouse, value);
                _pointerDeviceService.EnableMouse = value;
            }
        }

        public bool EnableLassoSelection
        {
            get => enableLassoSelection;
            set
            {
                SetProperty(ref enableLassoSelection, value);
                ConfigLassoSelection(value);
            }
        }

        public ICommand ZoomInCommand { get; }

        public ICommand ZoomOutCommand { get; }

        public ICommand UndoCommand { get; }

        public ICommand RedoCommand { get; }

        public ICommand CutCommand { get; }

        public ICommand CopyCommand { get; }

        public ICommand PasteCommand { get; }

        public ICommand LoadInkFileCommand { get; }

        public ICommand SaveInkFileCommand { get; }

        public ICommand ExportAsImageCommand { get; }

        public ICommand ClearAllCommand { get; }

        public void Undo()
        {
            ClearSelection();
            _undoRedoService?.Undo();
        }

        public void Redo()
        {
            ClearSelection();
            _undoRedoService?.Redo();
        }

        public void Cut()
        {
            _copyPasteService?.Cut();
            ClearSelection();
        }

        public void Paste()
        {
            _copyPasteService?.Paste();
            ClearSelection();
        }

        public async void LoadInkFile()
        {
            ClearSelection();
            var fileLoaded = await _fileService?.LoadInkAsync();

            if (fileLoaded)
            {
                _undoRedoService?.Reset();
            }
        }

        public async void SaveInkFile()
        {
            ClearSelection();
            await _fileService?.SaveInkAsync();
        }

        public async void ExportAsImage()
        {
            ClearSelection();
            await _fileService?.ExportToImageAsync();
        }

        public void ClearAll()
        {
            var strokes = _strokeService?.GetStrokes().ToList();
            ClearSelection();
            _strokeService?.ClearStrokes();
            _undoRedoService?.AddOperation(new RemoveStrokeUndoRedoOperation(strokes, _strokeService));
        }

        private bool CanCut() => _copyPasteService != null && _copyPasteService.CanCut;

        private bool CanCopy() => _copyPasteService != null && _copyPasteService.CanCopy;

        private bool CanPaste() => _copyPasteService != null && _copyPasteService.CanPaste;

        private bool CanUndo() => _undoRedoService != null && _undoRedoService.CanUndo;

        private bool CanRedo() => _undoRedoService != null && _undoRedoService.CanRedo;

        private bool CanSaveInkFile() => _strokeService != null && _strokeService.GetStrokes().Any();

        private bool CanExportAsImage() => _strokeService != null && _strokeService.GetStrokes().Any();

        private bool CanClearAll() => _strokeService != null && _strokeService.GetStrokes().Any();

        private void RefreshCommands()
        {
            (CutCommand as DelegateCommand)?.RaiseCanExecuteChanged();
            (CopyCommand as DelegateCommand)?.RaiseCanExecuteChanged();
            (PasteCommand as DelegateCommand)?.RaiseCanExecuteChanged();
            (UndoCommand as DelegateCommand)?.RaiseCanExecuteChanged();
            (RedoCommand as DelegateCommand)?.RaiseCanExecuteChanged();
            (SaveInkFileCommand as DelegateCommand)?.RaiseCanExecuteChanged();
            (ExportAsImageCommand as DelegateCommand)?.RaiseCanExecuteChanged();
            (ClearAllCommand as DelegateCommand)?.RaiseCanExecuteChanged();
        }

        private void ConfigLassoSelection(bool enableLasso)
        {
            if (enableLasso)
            {
                _lassoSelectionService?.StartLassoSelectionConfig();
            }
            else
            {
                _lassoSelectionService?.EndLassoSelectionConfig();
            }
        }

        private void ClearSelection() => _lassoSelectionService?.ClearSelection();
    }
}
