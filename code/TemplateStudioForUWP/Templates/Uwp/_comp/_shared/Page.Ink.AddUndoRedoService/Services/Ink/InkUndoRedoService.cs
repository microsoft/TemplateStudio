using System;
using System.Collections.Generic;
using System.Linq;
using Windows.UI.Xaml.Controls;
using Windows.UI.Input.Inking;
using Param_RootNamespace.EventHandlers.Ink;
using Param_RootNamespace.Services.Ink.UndoRedo;

namespace Param_RootNamespace.Services.Ink
{
    public class InkUndoRedoService
    {
        private readonly InkStrokesService _strokeService;

        private Stack<IUndoRedoOperation> undoStack = new Stack<IUndoRedoOperation>();
        private Stack<IUndoRedoOperation> redoStack = new Stack<IUndoRedoOperation>();

        public InkUndoRedoService(InkCanvas inkCanvas, InkStrokesService strokeService)
        {
            _strokeService = strokeService;
            _strokeService.StrokesCollected += StrokeService_StrokesCollected;
            _strokeService.StrokesErased += StrokeService_StrokesErased;
            _strokeService.MoveStrokesEvent += StrokeService_MoveStrokesEvent;
            _strokeService.CutStrokesEvent += StrokeService_CutStrokesEvent;
            _strokeService.PasteStrokesEvent += StrokeService_PasteStrokesEvent;
        }

        public event EventHandler<EventArgs> UndoEvent;

        public event EventHandler<EventArgs> RedoEvent;

        public event EventHandler<EventArgs> AddUndoOperationEvent;

        public void Reset()
        {
            undoStack.Clear();
            redoStack.Clear();
            AddUndoOperationEvent?.Invoke(this, EventArgs.Empty);
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
            UndoEvent?.Invoke(this, EventArgs.Empty);
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
            RedoEvent?.Invoke(this, EventArgs.Empty);
        }

        public void AddOperation(IUndoRedoOperation operation)
        {
            if (operation == null)
            {
                return;
            }

            undoStack.Push(operation);
            redoStack.Clear();
            AddUndoOperationEvent?.Invoke(this, EventArgs.Empty);
        }

        private void StrokeService_StrokesCollected(object sender, InkStrokesCollectedEventArgs e) => AddStrokesOperation(e.Strokes);

        private void StrokeService_StrokesErased(object sender, InkStrokesErasedEventArgs e) => RemoveStrokesOperation(e.Strokes);

        private void StrokeService_CutStrokesEvent(object sender, CopyPasteStrokesEventArgs e) => RemoveStrokesOperation(e.Strokes);

        private void StrokeService_PasteStrokesEvent(object sender, CopyPasteStrokesEventArgs e) => AddStrokesOperation(e.Strokes);

        private void StrokeService_MoveStrokesEvent(object sender, MoveStrokesEventArgs e)
        {
            var operation = new MoveStrokesUndoRedoOperation(e.Strokes, e.StartPosition, e.EndPosition, _strokeService);
            AddOperation(operation);
        }

        private void AddStrokesOperation(IEnumerable<InkStroke> strokes)
        {
            var operation = new AddStrokeUndoRedoOperation(strokes, _strokeService);
            AddOperation(operation);
        }

        private void RemoveStrokesOperation(IEnumerable<InkStroke> strokes)
        {
            var operation = new RemoveStrokeUndoRedoOperation(strokes, _strokeService);
            AddOperation(operation);
        }
    }
}