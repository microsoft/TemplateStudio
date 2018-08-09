using System.Linq;
using Param_ItemNamespace.EventHandlers.Ink;

namespace Param_ItemNamespace.Services.Ink.UndoRedo
{
    public class TransformUndoRedoOperation : IUndoRedoOperation
    {
        private readonly InkStrokesService _strokeService;
        private InkTransformResult _transformResult;

        public TransformUndoRedoOperation(InkTransformResult transformResult, InkStrokesService strokeService)
        {
            _transformResult = transformResult;
            _strokeService = strokeService;

            _strokeService.AddStrokeEvent += StrokeService_AddStrokeEvent;
        }

        public void ExecuteRedo()
        {
            RemoveStrokes();
            AddTextAndShapes();
        }

        public void ExecuteUndo()
        {
            RemoveTextAndShapes();
            AddStrokes();
        }

        private void StrokeService_AddStrokeEvent(object sender, AddStrokeEventArgs e)
        {
            if (e.NewStroke == null)
            {
                return;
            }

            var removedStrokes = _transformResult.Strokes.RemoveAll(s => s.Id == e.OldStroke?.Id);
            if (removedStrokes > 0)
            {
                _transformResult.Strokes.Add(e.NewStroke);
            }
        }

        private void AddTextAndShapes()
        {
            foreach (var uielement in _transformResult.TextAndShapes.ToList())
            {
                _transformResult.DrawingCanvas.Children.Add(uielement);
            }
        }

        private void RemoveTextAndShapes()
        {
            foreach (var uielement in _transformResult.TextAndShapes)
            {
                if (_transformResult.DrawingCanvas.Children.Contains(uielement))
                {
                    _transformResult.DrawingCanvas.Children.Remove(uielement);
                }
            }
        }

        private void AddStrokes()
        {
            foreach (var stroke in _transformResult.Strokes.ToList())
            {
                _strokeService.AddStroke(stroke);
            }
        }

        private void RemoveStrokes()
        {
            foreach (var stroke in _transformResult.Strokes)
            {
                _strokeService.RemoveStroke(stroke);
            }
        }
    }
}
