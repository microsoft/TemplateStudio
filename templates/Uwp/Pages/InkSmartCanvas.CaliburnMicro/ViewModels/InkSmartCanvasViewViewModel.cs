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

        public bool EnablePen
        {
            get => enablePen;
            set
            {
                Param_Setter(ref enablePen, value);
                pointerDeviceService.EnablePen = value;
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

        public void Undo()
        {
            ClearSelection();
            undoRedoService.Undo();
        }

        public void Redo()
        {
            ClearSelection();
            undoRedoService.Redo();
        }

        public async void LoadInkFile()
        {
            ClearSelection();
            var fileLoaded = await fileService.LoadInkAsync();

            if (fileLoaded)
            {
                transformService.ClearTextAndShapes();
                undoRedoService.Reset();
            }
        }

        public async void SaveInkFile()
        {
            ClearSelection();
            await fileService.SaveInkAsync();
        }

        public async void TransformTextAndShapes()
        {
            var result = await transformService.TransformTextAndShapesAsync();
            if (result.TextAndShapes.Any())
            {
                ClearSelection();
                undoRedoService.AddOperation(new TransformUndoRedoOperation(result, strokeService));
            }
        }

        public void ClearAll()
        {
            ClearSelection();
            strokeService.ClearStrokes();
            transformService.ClearTextAndShapes();
            undoRedoService.Reset();
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
    }
}
