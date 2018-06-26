using Param_ItemNamespace.Services.Ink;
using Param_ItemNamespace.Helpers;

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
                Param_Setter(ref enableTouch, value);
                pointerDeviceService.EnableTouch = value;
            }
        }

        public bool EnableMouse
        {
            get => enableMouse;
            set
            {
                Param_Setter(ref enableMouse, value);
                pointerDeviceService.EnableMouse = value;
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

        public void ZoomIn() => zoomService?.ZoomIn();

        public void ZoomOut () => zoomService?.ZoomOut();

        public void Copy() => copyPasteService?.Copy();

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
