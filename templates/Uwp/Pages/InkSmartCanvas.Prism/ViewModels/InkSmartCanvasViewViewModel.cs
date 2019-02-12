using Param_RootNamespace.Services.Ink;
using Param_RootNamespace.Services.Ink.UndoRedo;
using System.Windows.Input;
using System.Linq;
using Prism.Commands;

namespace Param_RootNamespace.ViewModels
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

        private bool enableTouch = true;
        private bool enableMouse = true;
        private bool enablePen = true;

        private bool enableLassoSelection;

        public InkSmartCanvasViewViewModel()
        {
            UndoCommand = new DelegateCommand(Undo, CanUndo);
            RedoCommand = new DelegateCommand(Redo, CanRedo);
            LoadInkFileCommand = new DelegateCommand(LoadInkFile);
            SaveInkFileCommand = new DelegateCommand(SaveInkFile, CanSaveInkFile);
            TransformTextAndShapesCommand = new DelegateCommand(TransformTextAndShapes, CanTransformTextAndShapes);
            ClearAllCommand = new DelegateCommand(ClearAll, CanClearAll);
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
                SetProperty(ref enableTouch, value);
                _pointerDeviceService.EnableTouch = value;
            }
        }

        public bool EnableMouse
        {
            get => enableMouse;
            set
            {
                SetProperty(ref enableMouse, value);
                _pointerDeviceService.EnableMouse = value;
            }
        }

        public bool EnablePen
        {
            get => enablePen;
            set
            {
                SetProperty(ref enablePen, value);
                _pointerDeviceService.EnablePen = value;
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
            _undoRedoService.Undo();
        }

        private void Redo()
        {
            ClearSelection();
            _undoRedoService.Redo();
        }

        private async void LoadInkFile()
        {
            ClearSelection();
            var fileLoaded = await _fileService.LoadInkAsync();

            if (fileLoaded)
            {
                _transformService.ClearTextAndShapes();
                _undoRedoService.Reset();
            }
        }

        private async void SaveInkFile()
        {
            ClearSelection();
            await _fileService.SaveInkAsync();
        }

        private async void TransformTextAndShapes()
        {
            var result = await _transformService.TransformTextAndShapesAsync();
            if (result.TextAndShapes.Any())
            {
                ClearSelection();
                _undoRedoService.AddOperation(new TransformUndoRedoOperation(result, _strokeService));
            }
        }

        private void ClearAll()
        {
            var strokes = _strokeService?.GetStrokes().ToList();
            var textAndShapes = _transformService?.GetTextAndShapes().ToList();
            ClearSelection();
            _strokeService.ClearStrokes();
            _transformService.ClearTextAndShapes();
            _undoRedoService.AddOperation(new ClearStrokesAndShapesUndoRedoOperation(strokes, textAndShapes, _strokeService, _transformService));
        }

        private bool CanUndo() => _undoRedoService != null && _undoRedoService.CanUndo;

        private bool CanRedo() => _undoRedoService != null && _undoRedoService.CanRedo;

        private bool CanSaveInkFile() => _strokeService != null && _strokeService.GetStrokes().Any();

        private bool CanTransformTextAndShapes() => _strokeService != null && _strokeService.GetStrokes().Any();

        private bool CanClearAll() => (_strokeService != null && _strokeService.GetStrokes().Any()) ||
                                      (_transformService != null && _transformService.HasTextAndShapes());

        private void RefreshCommands()
        {
            (UndoCommand as DelegateCommand)?.RaiseCanExecuteChanged();
            (RedoCommand as DelegateCommand)?.RaiseCanExecuteChanged();
            (SaveInkFileCommand as DelegateCommand)?.RaiseCanExecuteChanged();
            (TransformTextAndShapesCommand as DelegateCommand)?.RaiseCanExecuteChanged();
            (ClearAllCommand as DelegateCommand)?.RaiseCanExecuteChanged();
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

        private void ClearSelection()
        {
            _nodeSelectionService.ClearSelection();
            _lassoSelectionService.ClearSelection();
        }
    }
}
