using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Param_RootNamespace.Services.Ink;
using Param_RootNamespace.Services.Ink.UndoRedo;

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

        private ICommand cutCommand;
        private ICommand copyCommand;
        private ICommand pasteCommand;
        private ICommand undoCommand;
        private ICommand redoCommand;
        private ICommand zoomInCommand;
        private ICommand zoomOutCommand;
        private ICommand loadInkFileCommand;
        private ICommand saveInkFileCommand;
        private ICommand exportAsImageCommand;
        private ICommand clearAllCommand;

        private bool enableTouch = true;
        private bool enableMouse = true;
        private bool enableLassoSelection;

        public InkDrawViewViewModel()
        {
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
                Param_Setter(ref enableTouch, value);
                _pointerDeviceService.EnableTouch = value;
            }
        }

        public bool EnableMouse
        {
            get => enableMouse;
            set
            {
                Param_Setter(ref enableMouse, value);
                _pointerDeviceService.EnableMouse = value;
            }
        }

        public bool EnableLassoSelection
        {
            get => enableLassoSelection;
            set
            {
                Param_Setter(ref enableLassoSelection, value);
                ConfigLassoSelection(value);
            }
        }

        public ICommand CutCommand => cutCommand
           ?? (cutCommand = new RelayCommand(Cut, CanCut));

        public ICommand CopyCommand => copyCommand
           ?? (copyCommand = new RelayCommand(Copy, CanCopy));

        public ICommand PasteCommand => pasteCommand
           ?? (pasteCommand = new RelayCommand(Paste, CanPaste));

        public ICommand UndoCommand => undoCommand
           ?? (undoCommand = new RelayCommand(Undo, CanUndo));

        public ICommand RedoCommand => redoCommand
           ?? (redoCommand = new RelayCommand(Redo, CanRedo));

        public ICommand ZoomInCommand => zoomInCommand
            ?? (zoomInCommand = new RelayCommand(() => _zoomService?.ZoomIn()));

        public ICommand ZoomOutCommand => zoomOutCommand
            ?? (zoomOutCommand = new RelayCommand(() => _zoomService?.ZoomOut()));

        public ICommand LoadInkFileCommand => loadInkFileCommand
           ?? (loadInkFileCommand = new RelayCommand(async () => await LoadInkFileAsync()));

        public ICommand SaveInkFileCommand => saveInkFileCommand
           ?? (saveInkFileCommand = new RelayCommand(async () => await SaveInkFileAsync(), CanSaveInkFile));

        public ICommand ExportAsImageCommand => exportAsImageCommand
           ?? (exportAsImageCommand = new RelayCommand(async () => await ExportAsImageAsync(), CanExportAsImage));

        public ICommand ClearAllCommand => clearAllCommand
           ?? (clearAllCommand = new RelayCommand(ClearAll, CanClearAll));

        private void Cut()
        {
            _copyPasteService?.Cut();
            ClearSelection();
        }

        private void Copy()
        {
            _copyPasteService?.Copy();
        }

        private void Paste()
        {
            _copyPasteService?.Paste();
            ClearSelection();
        }

        private void Undo()
        {
            ClearSelection();
            _undoRedoService?.Undo();
        }

        private void Redo()
        {
            ClearSelection();
            _undoRedoService?.Redo();
        }

        private async Task LoadInkFileAsync()
        {
            ClearSelection();
            var fileLoaded = await _fileService?.LoadInkAsync();

            if (fileLoaded)
            {
                _undoRedoService?.Reset();
            }
        }

        private async Task SaveInkFileAsync()
        {
            ClearSelection();
            await _fileService?.SaveInkAsync();
        }

        private async Task ExportAsImageAsync()
        {
            ClearSelection();
            await _fileService?.ExportToImageAsync();
        }

        private void ClearAll()
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
