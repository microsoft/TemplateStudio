using InkPoc.Helpers;
using InkPoc.Services.Ink;

namespace InkPoc.ViewModels
{
    public class DrawViewModel : Observable
    {
        private readonly InkStrokesService strokeService;
        private readonly InkLassoSelectionService lassoSelectionService;
        private readonly InkPointerDeviceService pointerDeviceService;
        private readonly InkCopyPasteService copyPasteService;
        private readonly InkUndoRedoService undoRedoService;
        private readonly InkFileService fileService;
        private readonly InkZoomService zoomService;

        private RelayCommand cutCommand;
        private RelayCommand copyCommand;
        private RelayCommand pasteCommand;
        private RelayCommand undoCommand;
        private RelayCommand redoCommand;
        private RelayCommand zoomInCommand;
        private RelayCommand zoomOutCommand;
        private RelayCommand loadInkFileCommand;
        private RelayCommand saveInkFileCommand;
        private RelayCommand exportInkFileCommand;
        private RelayCommand clearAllCommand;

        private bool enableTouch = true;
        private bool enableMouse = true;
        private bool enableLassoSelection;

        public DrawViewModel()
        {
        }

        public DrawViewModel(
            InkStrokesService _strokeService,
            InkLassoSelectionService _lassoSelectionService,
            InkPointerDeviceService _pointerDeviceService,
            InkCopyPasteService _copyPasteService,
            InkUndoRedoService _undoRedoService,
            InkFileService _fileService,
            InkZoomService _zoomService)
        {
            strokeService = _strokeService;
            lassoSelectionService = _lassoSelectionService;
            pointerDeviceService = _pointerDeviceService;
            copyPasteService = _copyPasteService;
            undoRedoService = _undoRedoService;
            fileService = _fileService;
            zoomService = _zoomService;

            pointerDeviceService.DetectPenEvent += (s, e) => EnableTouch = false;
        }

        public RelayCommand CutCommand => cutCommand
           ?? (cutCommand = new RelayCommand(() =>
           {
               copyPasteService.Cut();
               ClearSelection();
           }));

        public RelayCommand CopyCommand => copyCommand
           ?? (copyCommand = new RelayCommand(() => copyPasteService.Copy()));

        public RelayCommand PasteCommand => pasteCommand
           ?? (pasteCommand = new RelayCommand(() =>
           {
               copyPasteService.Paste();
               ClearSelection();
           }));

        public RelayCommand UndoCommand => undoCommand
           ?? (undoCommand = new RelayCommand(() =>
           {
               ClearSelection();
               undoRedoService.Undo();
           }));

        public RelayCommand RedoCommand => redoCommand
           ?? (redoCommand = new RelayCommand(() =>
           {
               ClearSelection();
               undoRedoService.Redo();
           }));

        public RelayCommand ZoomInCommand => zoomInCommand
            ?? (zoomInCommand = new RelayCommand(() => zoomService.ZoomIn()));

        public RelayCommand ZoomOutCommand => zoomOutCommand
            ?? (zoomOutCommand = new RelayCommand(() => zoomService.ZoomOut()));

        public RelayCommand LoadInkFileCommand => loadInkFileCommand
           ?? (loadInkFileCommand = new RelayCommand(async () =>
           {
               ClearSelection();
               var fileLoaded = await fileService.LoadInkAsync();

               if(fileLoaded)
               {
                   undoRedoService.Reset();
               }
           }));

        public RelayCommand SaveInkFileCommand => saveInkFileCommand
           ?? (saveInkFileCommand = new RelayCommand(async () =>
           {
               ClearSelection();
               await fileService.SaveInkAsync();
           }));

        public RelayCommand ExportInkFileCommand => exportInkFileCommand
           ?? (exportInkFileCommand = new RelayCommand(async () =>
           {
               ClearSelection();
               await fileService.ExportToImageAsync();
           }));

        public RelayCommand ClearAllCommand => clearAllCommand
           ?? (clearAllCommand = new RelayCommand(ClearAll));

        public bool EnableTouch
        {
            get => enableTouch;
            set
            {
                Set(ref enableTouch, value);
                pointerDeviceService.EnableTouch = value;
            }
        }

        public bool EnableMouse
        {
            get => enableMouse;
            set
            {
                Set(ref enableMouse, value);
                pointerDeviceService.EnableMouse = value;
            }
        }
        
        public bool EnableLassoSelection
        {
            get => enableLassoSelection;
            set
            {
                Set(ref enableLassoSelection, value);
                ConfigLassoSelection(value);
            }
        }

        private void ConfigLassoSelection(bool enableLasso)
        {
            if(enableLasso)
            {
                lassoSelectionService.StartLassoSelectionConfig();
            }
            else
            {
                lassoSelectionService.EndLassoSelectionConfig();
            }
        }

        private void ClearAll()
        {
            ClearSelection();
            strokeService.ClearStrokes();
            undoRedoService.Reset();
        }

        private void ClearSelection() => lassoSelectionService.ClearSelection();
    }
}
