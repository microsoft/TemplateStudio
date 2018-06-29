using System.Collections.Generic;
using System.Linq;
using Windows.UI.Xaml.Controls;
using Param_ItemNamespace.EventHandlers.Ink;
using Param_ItemNamespace.Services.Ink.UndoRedo;

namespace Param_ItemNamespace.Services.Ink
{
    public class InkUndoRedoService
    {
        private readonly InkStrokesService _strokeService;

        private Stack<IUndoRedoOperation> undoStack = new Stack<IUndoRedoOperation>();
        private Stack<IUndoRedoOperation> redoStack = new Stack<IUndoRedoOperation>();

        public InkUndoRedoService(InkCanvas inkCanvas, InkStrokesService strokeService)
        {
            _strokeService = strokeService;
            _strokeService.MoveStrokesEvent += StrokeService_MoveStrokesEvent;
            _strokeService.CutStrokesEvent += StrokeService_CutStrokesEvent;
            _strokeService.PasteStrokesEvent += StrokeService_PasteStrokesEvent;

            inkCanvas.InkPresenter.StrokesCollected += (s, e) => AddOperation(new AddStrokeUndoRedoOperation(e.Strokes, _strokeService));
            inkCanvas.InkPresenter.StrokesErased += (s, e) => AddOperation(new RemoveStrokeUndoRedoOperation(e.Strokes, _strokeService));
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
            {
                return;
            }

            _strokeService.MoveStrokesEvent -= StrokeService_MoveStrokesEvent;

            var element = undoStack.Pop();
            element.ExecuteUndo();
            redoStack.Push(element);

            _strokeService.MoveStrokesEvent += StrokeService_MoveStrokesEvent;
        }

        public void Redo()
        {
            if (!CanRedo)
            {
                return;
            }

            _strokeService.MoveStrokesEvent -= StrokeService_MoveStrokesEvent;

            var element = redoStack.Pop();
            element.ExecuteRedo();
            undoStack.Push(element);

            _strokeService.MoveStrokesEvent += StrokeService_MoveStrokesEvent;
        }

        public void AddOperation(IUndoRedoOperation operation)
        {
            if (operation == null)
            {
                return;
            }

            undoStack.Push(operation);
            redoStack.Clear();
        }

        private void StrokeService_MoveStrokesEvent(object sender, MoveStrokesEventArgs e)
        {
            var operation = new MoveStrokesUndoRedoOperation(e.Strokes, e.StartPosition, e.EndPosition, _strokeService);
            AddOperation(operation);
        }

        private void StrokeService_CutStrokesEvent(object sender, CopyPasteStrokesEventArgs e)
        {
            var operation = new RemoveStrokeUndoRedoOperation(e.Strokes, _strokeService);
            AddOperation(operation);
        }

        private void StrokeService_PasteStrokesEvent(object sender, CopyPasteStrokesEventArgs e)
        {
            var operation = new AddStrokeUndoRedoOperation(e.Strokes, _strokeService);
            AddOperation(operation);
        }
    }
}