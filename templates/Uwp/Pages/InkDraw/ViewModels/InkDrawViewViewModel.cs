using System.Windows.Input;
using Param_ItemNamespace.Services.Ink;
using Param_ItemNamespace.Helpers;

namespace Param_ItemNamespace.ViewModels
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

            _pointerDeviceService.DetectPenEvent += (s, e) => EnableTouch = false;
        }

        public ICommand CutCommand => cutCommand
           ?? (cutCommand = new RelayCommand(() =>
           {
               _copyPasteService?.Cut();
               ClearSelection();
           }));

        public ICommand CopyCommand => copyCommand
           ?? (copyCommand = new RelayCommand(() => _copyPasteService?.Copy()));

        public ICommand PasteCommand => pasteCommand
           ?? (pasteCommand = new RelayCommand(() =>
           {
               _copyPasteService?.Paste();
               ClearSelection();
           }));

        public ICommand UndoCommand => undoCommand
           ?? (undoCommand = new RelayCommand(() =>
           {
               ClearSelection();
               _undoRedoService?.Undo();
           }));

        public ICommand RedoCommand => redoCommand
           ?? (redoCommand = new RelayCommand(() =>
           {
               ClearSelection();
               _undoRedoService?.Redo();
           }));

        public ICommand ZoomInCommand => zoomInCommand
            ?? (zoomInCommand = new RelayCommand(() => _zoomService?.ZoomIn()));

        public ICommand ZoomOutCommand => zoomOutCommand
            ?? (zoomOutCommand = new RelayCommand(() => _zoomService?.ZoomOut()));

        public ICommand LoadInkFileCommand => loadInkFileCommand
           ?? (loadInkFileCommand = new RelayCommand(async () =>
           {
               ClearSelection();
               var fileLoaded = await _fileService?.LoadInkAsync();

               if (fileLoaded)
               {
                   _undoRedoService?.Reset();
               }
           }));

        public ICommand SaveInkFileCommand => saveInkFileCommand
           ?? (saveInkFileCommand = new RelayCommand(async () =>
           {
               ClearSelection();
               await _fileService?.SaveInkAsync();
           }));

        public ICommand ExportAsImageCommand => exportAsImageCommand
           ?? (exportAsImageCommand = new RelayCommand(async () =>
           {
               ClearSelection();
               await _fileService?.ExportToImageAsync();
           }));

        public ICommand ClearAllCommand => clearAllCommand
           ?? (clearAllCommand = new RelayCommand(ClearAll));

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

        private void ClearAll()
        {
            ClearSelection();
            _strokeService?.ClearStrokes();
            _undoRedoService?.Reset();
        }

        private void ClearSelection() => _lassoSelectionService?.ClearSelection();
    }
}
