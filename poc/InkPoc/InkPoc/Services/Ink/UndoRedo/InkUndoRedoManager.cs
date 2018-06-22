using System.Collections.Generic;
using System.Linq;
using Windows.UI.Xaml.Controls;

namespace InkPoc.Services.Ink
{
    public class InkUndoRedoService
    {
        private readonly InkStrokesService strokeService;

        private Stack<IUndoRedoOperation> undoStack = new Stack<IUndoRedoOperation>();
        private Stack<IUndoRedoOperation> redoStack = new Stack<IUndoRedoOperation>();

        public InkUndoRedoService(InkCanvas _inkCanvas, InkStrokesService _strokeService)
        {
            strokeService = _strokeService;
            strokeService.MoveStrokesEvent += StrokeService_MoveStrokesEvent;
            strokeService.CutStrokesEvent += StrokeService_CutStrokesEvent;
            strokeService.PasteStrokesEvent += StrokeService_PasteStrokesEvent;

            _inkCanvas.InkPresenter.StrokesCollected += (s, e) => AddOperation(new AddStrokeUndoRedoOperation(e.Strokes, strokeService));
            _inkCanvas.InkPresenter.StrokesErased += (s, e) => AddOperation(new RemoveStrokeUndoRedoOperation(e.Strokes, strokeService));
        }       

        public void Reset()
        {
            undoStack.Clear();
            redoStack.Clear();
        }

        public bool CanUndo => undoStack.Any();

        public bool CanRedo => redoStack.Any();

        public void Undo()
        {
            if (!CanUndo)
                return;

            strokeService.MoveStrokesEvent -= StrokeService_MoveStrokesEvent;

            var element = undoStack.Pop();
            element.ExecuteUndo();
            redoStack.Push(element);

            strokeService.MoveStrokesEvent += StrokeService_MoveStrokesEvent;
        }

        public void Redo()
        {
            if (!CanRedo)
                return;

            strokeService.MoveStrokesEvent -= StrokeService_MoveStrokesEvent;

            var element = redoStack.Pop();
            element.ExecuteRedo();           
            undoStack.Push(element);

            strokeService.MoveStrokesEvent += StrokeService_MoveStrokesEvent;
        }

        public void AddOperation(IUndoRedoOperation operation)
        {
            if (operation == null)
                return;

            undoStack.Push(operation);
            redoStack.Clear();
        }

        private void StrokeService_MoveStrokesEvent(object sender, MoveStrokesEventArgs e)
        {
            var operation = new MoveStrokesUndoRedoOperation(e.Strokes, e.StartPosition, e.EndPosition, strokeService);
            AddOperation(operation);
        }

        private void StrokeService_CutStrokesEvent(object sender, CopyPasteStrokesEventArgs e)
        {
            var operation = new RemoveStrokeUndoRedoOperation(e.Strokes, strokeService);
            AddOperation(operation);
        }

        private void StrokeService_PasteStrokesEvent(object sender, CopyPasteStrokesEventArgs e)
        {
            var operation = new AddStrokeUndoRedoOperation(e.Strokes, strokeService);
            AddOperation(operation);
        }        
    }
}
