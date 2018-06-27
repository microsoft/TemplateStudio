using Param_ItemNamespace.Services.Ink;
using Param_ItemNamespace.Services.Ink.UndoRedo;
using System.Windows.Input;
using System.Linq;
using Prism.Commands;

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
            UndoCommand = new DelegateCommand(Undo);
            RedoCommand = new DelegateCommand(Redo);
            LoadInkFileCommand = new DelegateCommand(LoadInkFile);
            SaveInkFileCommand = new DelegateCommand(SaveInkFile);
            TransformTextAndShapesCommand = new DelegateCommand(TransformTextAndShapes);
            ClearAllCommand = new DelegateCommand(ClearAll);
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

        public bool EnablePen
        {
            get => enablePen;
            set
            {
                SetProperty(ref enablePen, value);
                pointerDeviceService.EnablePen = value;
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

        public ICommand UndoCommand { get; }

        public ICommand RedoCommand { get; }

        public ICommand LoadInkFileCommand { get; }

        public ICommand SaveInkFileCommand { get; }

        public ICommand TransformTextAndShapesCommand { get; }
        
        public ICommand ClearAllCommand { get; }
      
        private void Undo()
        {
            ClearSelection();
            undoRedoService.Undo();
        }

        private void Redo()
        {
            ClearSelection();
            undoRedoService.Redo();
        }

        private async void LoadInkFile()
        {
            ClearSelection();
            var fileLoaded = await fileService.LoadInkAsync();

            if (fileLoaded)
            {
                transformService.ClearTextAndShapes();
                undoRedoService.Reset();
            }
        }

        private async void SaveInkFile()
        {
            ClearSelection();
            await fileService.SaveInkAsync();
        }

        private async void TransformTextAndShapes()
        {
            var result = await transformService.TransformTextAndShapesAsync();
            if (result.TextAndShapes.Any())
            {
                ClearSelection();
                undoRedoService.AddOperation(new TransformUndoRedoOperation(result, strokeService));
            }
        }

        private void ClearAll()
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
