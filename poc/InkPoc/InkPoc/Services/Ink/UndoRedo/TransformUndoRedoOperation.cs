using System.Linq;

namespace InkPoc.Services.Ink
{
    public class TransformUndoRedoOperation : IUndoRedoOperation
    {
        private InkTransformResult transformResult;
        private readonly InkStrokesService strokeService;

        public TransformUndoRedoOperation(InkTransformResult _transformResult, InkStrokesService _strokeService)
        {
            transformResult = _transformResult;
            strokeService = _strokeService;

            strokeService.AddStrokeEvent += StrokeService_AddStrokeEvent;
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

            var removedStrokes = transformResult.Strokes.RemoveAll(s => s.Id == e.OldStroke?.Id);
            if (removedStrokes > 0)
            {
                transformResult.Strokes.Add(e.NewStroke);
            }
        }

        private void AddTextAndShapes()
        {
            foreach (var uielement in transformResult.TextAndShapes.ToList())
            {
                transformResult.DrawingCanvas.Children.Add(uielement);
            }
        }

        private void RemoveTextAndShapes()
        {
            foreach (var uielement in transformResult.TextAndShapes)
            {
                if (transformResult.DrawingCanvas.Children.Contains(uielement))
                {
                    transformResult.DrawingCanvas.Children.Remove(uielement);
                }
            }
        }

        private void AddStrokes()
        {
            foreach (var stroke in transformResult.Strokes.ToList())
            {
                strokeService.AddStroke(stroke);
            }
        }

        private void RemoveStrokes()
        {
            foreach (var stroke in transformResult.Strokes)
            {
                strokeService.RemoveStroke(stroke);
            }
        }
    }
}
