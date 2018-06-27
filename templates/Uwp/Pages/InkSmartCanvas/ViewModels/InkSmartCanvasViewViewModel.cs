using Param_ItemNamespace.Services.Ink;
using Param_ItemNamespace.Services.Ink.UndoRedo;
using System.Linq;

namespace Param_ItemNamespace.ViewModels
{
    public class InkSmartCanvasViewViewModel : System.ComponentModel.INotifyPropertyChanged
    {
        private InkStrokesService strokeService;
        private InkLassoSelectionService lassoSelectionService;
        private InkNodeSelectionService nodeSelectionService;
        private InkPointerDeviceService pointerDeviceService;
        private InkUndoRedoService undoRedoService;
        private InkTransformService transformService;
        private InkFileService fileService;

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
            InkStrokesService _strokeService,
            InkLassoSelectionService _lassoSelectionService,
            InkNodeSelectionService _nodeSelectionService,
            InkPointerDeviceService _pointerDeviceService,
            InkUndoRedoService _undoRedoService,
            InkTransformService _transformService,
            InkFileService _fileService)
        {
            strokeService = _strokeService;
            lassoSelectionService = _lassoSelectionService;
            nodeSelectionService = _nodeSelectionService;
            pointerDeviceService = _pointerDeviceService;
            undoRedoService = _undoRedoService;
            transformService = _transformService;
            fileService = _fileService;

            pointerDeviceService.DetectPenEvent += (s, e) => EnableTouch = false;
        }

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

        public RelayCommand LoadInkFileCommand => loadInkFileCommand
           ?? (loadInkFileCommand = new RelayCommand(async () =>
           {
               ClearSelection();
               var fileLoaded = await fileService.LoadInkAsync();

               if (fileLoaded)
               {
                   transformService.ClearTextAndShapes();
                   undoRedoService.Reset();
               }
           }));

        public RelayCommand SaveInkFileCommand => saveInkFileCommand
           ?? (saveInkFileCommand = new RelayCommand(async () =>
           {
               ClearSelection();
               await fileService.SaveInkAsync();
           }));

        public RelayCommand TransformTextAndShapesCommand => transformTextAndShapesCommand
           ?? (transformTextAndShapesCommand = new RelayCommand(async () =>
           {
               var result = await transformService.TransformTextAndShapesAsync();
               if (result.TextAndShapes.Any())
               {
                   ClearSelection();
                   undoRedoService.AddOperation(new TransformUndoRedoOperation(result, strokeService));
               }
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

        public bool EnablePen
        {
            get => enablePen;
            set
            {
                Set(ref enablePen, value);
                pointerDeviceService.EnablePen = value;
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
            if (enableLasso)
            {
                lassoSelectionService.StartLassoSelectionConfig();
            }
            else
            {
                lassoSelectionService.EndLassoSelectionConfig();
            }
        }

        private void ClearSelection()
        {
            nodeSelectionService.ClearSelection();
            lassoSelectionService.ClearSelection();
        }

        private void ClearAll()
        {
            ClearSelection();
            strokeService.ClearStrokes();
            transformService.ClearTextAndShapes();
            undoRedoService.Reset();
        }
    }
}
