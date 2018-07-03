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

        public void ZoomIn() => _zoomService?.ZoomIn();

        public void ZoomOut() => _zoomService?.ZoomOut();

        public void Copy() => _copyPasteService?.Copy();

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
            ClearSelection();
            _strokeService?.ClearStrokes();
            _undoRedoService?.Reset();
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
