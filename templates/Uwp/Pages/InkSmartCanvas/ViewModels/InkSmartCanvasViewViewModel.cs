using Param_ItemNamespace.Services.Ink;
using Param_ItemNamespace.Services.Ink.UndoRedo;
using System.Linq;

namespace Param_ItemNamespace.ViewModels
{
    public class InkSmartCanvasViewViewModel : System.ComponentModel.INotifyPropertyChanged
    {
        private InkStrokesService _strokeService;
        private InkLassoSelectionService _lassoSelectionService;
        private InkNodeSelectionService _nodeSelectionService;
        private InkPointerDeviceService _pointerDeviceService;
        private InkUndoRedoService _undoRedoService;
        private InkTransformService _transformService;
        private InkFileService _fileService;

        private RelayCommand undoCommand;
        private RelayCommand redoCommand;
        private RelayCommand loadInkFileCommand;
        private RelayCommand saveInkFileCommand;
        private RelayCommand transformTextAndShapesCommand;
        private RelayCommand clearAllCommand;

        private bool enableTouch = true;
        private bool enableMouse = true;
        private bool enablePen = true;

        private bool enableLassoSelection;

        public InkSmartCanvasViewViewModel()
        {
        }

        public void Initialize(
            InkStrokesService strokeService,
            InkLassoSelectionService lassoSelectionService,
            InkNodeSelectionService nodeSelectionService,
            InkPointerDeviceService pointerDeviceService,
            InkUndoRedoService undoRedoService,
            InkTransformService transformService,
            InkFileService fileService)
        {
            _strokeService = strokeService;
            _lassoSelectionService = lassoSelectionService;
            _nodeSelectionService = nodeSelectionService;
            _pointerDeviceService = pointerDeviceService;
            _undoRedoService = undoRedoService;
            _transformService = transformService;
            _fileService = fileService;

            _pointerDeviceService.DetectPenEvent += (s, e) => EnableTouch = false;
        }

        public RelayCommand UndoCommand => undoCommand
           ?? (undoCommand = new RelayCommand(() =>
           {
               ClearSelection();
               _undoRedoService.Undo();
           }));

        public RelayCommand RedoCommand => redoCommand
           ?? (redoCommand = new RelayCommand(() =>
           {
               ClearSelection();
               _undoRedoService.Redo();
           }));

        public RelayCommand LoadInkFileCommand => loadInkFileCommand
           ?? (loadInkFileCommand = new RelayCommand(async () =>
           {
               ClearSelection();
               var fileLoaded = await _fileService.LoadInkAsync();

               if (fileLoaded)
               {
                   _transformService.ClearTextAndShapes();
                   _undoRedoService.Reset();
               }
           }));

        public RelayCommand SaveInkFileCommand => saveInkFileCommand
           ?? (saveInkFileCommand = new RelayCommand(async () =>
           {
               ClearSelection();
               await _fileService.SaveInkAsync();
           }));

        public RelayCommand TransformTextAndShapesCommand => transformTextAndShapesCommand
           ?? (transformTextAndShapesCommand = new RelayCommand(async () =>
           {
               var result = await _transformService.TransformTextAndShapesAsync();
               if (result.TextAndShapes.Any())
               {
                   ClearSelection();
                   _undoRedoService.AddOperation(new TransformUndoRedoOperation(result, _strokeService));
               }
           }));

        public RelayCommand ClearAllCommand => clearAllCommand
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

        public bool EnablePen
        {
            get => enablePen;
            set
            {
                Param_Setter(ref enablePen, value);
                _pointerDeviceService.EnablePen = value;
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
                _lassoSelectionService.StartLassoSelectionConfig();
            }
            else
            {
                _lassoSelectionService.EndLassoSelectionConfig();
            }
        }

        private void ClearSelection()
        {
            _nodeSelectionService.ClearSelection();
            _lassoSelectionService.ClearSelection();
        }

        private void ClearAll()
        {
            ClearSelection();
            _strokeService.ClearStrokes();
            _transformService.ClearTextAndShapes();
            _undoRedoService.Reset();
        }
    }
}
