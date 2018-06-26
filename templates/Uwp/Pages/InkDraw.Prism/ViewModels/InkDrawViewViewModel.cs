using Param_ItemNamespace.Services.Ink;
using Prism.Commands;
using System.Windows.Input;

namespace Param_ItemNamespace.ViewModels
{
    public class InkDrawViewViewModel : System.ComponentModel.INotifyPropertyChanged
    {
        private InkStrokesService strokeService;
        private InkLassoSelectionService lassoSelectionService;
        private InkPointerDeviceService pointerDeviceService;
        private InkCopyPasteService copyPasteService;
        private InkUndoRedoService undoRedoService;
        private InkFileService fileService;
        private InkZoomService zoomService;

        private bool enableTouch = true;
        private bool enableMouse = true;
        private bool enableLassoSelection;

        public InkDrawViewViewModel()
        {
            ZoomInCommand = new DelegateCommand(() => zoomService?.ZoomIn());
            ZoomOutCommand = new DelegateCommand(() => zoomService?.ZoomOut());
            UndoCommand = new DelegateCommand(Undo);
            RedoCommand = new DelegateCommand(Redo);            
            CutCommand = new DelegateCommand(Cut);
            CopyCommand = new DelegateCommand(() => copyPasteService?.Copy());
            PasteCommand = new DelegateCommand(Paste);
            LoadInkFileCommand = new DelegateCommand(LoadInkFile);
            SaveInkFileCommand = new DelegateCommand(SaveInkFile);
            ExportAsImageCommand = new DelegateCommand(ExportAsImage);
            ClearAllCommand = new DelegateCommand(ClearAll);
        }
        
        public void Initialize(
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

        public bool EnableTouch
        {
            get => enableTouch;
            set
            {
                SetProperty(ref enableTouch, value);
                pointerDeviceService.EnableTouch = value;
            }
        }

        public bool EnableMouse
        {
            get => enableMouse;
            set
            {
                SetProperty(ref enableMouse, value);
                pointerDeviceService.EnableMouse = value;
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
            undoRedoService?.Undo();
        }

        public void Redo()
        {
            ClearSelection();
            undoRedoService?.Redo();
        }

        public void Cut()
        {
            copyPasteService?.Cut();
            ClearSelection();
        }

        public void Paste()
        {
            copyPasteService?.Paste();
            ClearSelection();
        }

        public async void LoadInkFile()
        {
            ClearSelection();
            var fileLoaded = await fileService?.LoadInkAsync();

            if(fileLoaded)
            {
                undoRedoService?.Reset();
            }
        }

        public async void SaveInkFile()
        {
            ClearSelection();
            await fileService?.SaveInkAsync();
        }

        public async void ExportAsImage()
        {
            ClearSelection();
            await fileService?.ExportToImageAsync();
        }

        public void ClearAll()
        {
            ClearSelection();
            strokeService?.ClearStrokes();
            undoRedoService?.Reset();
        }

        private void ConfigLassoSelection(bool enableLasso)
        {
            if(enableLasso)
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
