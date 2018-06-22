using System.Collections.Generic;
using Windows.Foundation;
using Windows.UI.Input.Inking;

namespace InkPoc.Services.Ink
{
    public class MoveStrokesUndoRedoOperation : IUndoRedoOperation
    {
        private readonly Point endPosition;
        private readonly Point startPosition;
        private readonly List<InkStroke> strokes;
        private readonly InkStrokesService strokeService;

        public MoveStrokesUndoRedoOperation(
            IEnumerable<InkStroke> _strokes,
            Point _startPosition,
            Point _endPosition,
            InkStrokesService _strokeService)
        {
            strokes = new List<InkStroke>(_strokes);
            startPosition = _startPosition;
            endPosition = _endPosition;
            strokeService = _strokeService;

            strokeService.AddStrokeEvent += StrokeService_AddStrokeEvent;
        }

        public void ExecuteRedo()
        {
            strokeService.SelectStrokes(strokes);
            strokeService.MoveSelectedStrokes(startPosition, endPosition);
        }

        public void ExecuteUndo()
        {
            strokeService.SelectStrokes(strokes);
            strokeService.MoveSelectedStrokes(endPosition, startPosition);
        }

        private void StrokeService_AddStrokeEvent(object sender, AddStrokeEventArgs e)
        {
            if (e.NewStroke == null)
            {
                return;
            }

            var removedStrokes = strokes.RemoveAll(s => s.Id == e.OldStroke?.Id);
            if (removedStrokes > 0)
            {
                strokes.Add(e.NewStroke);
            }
        }
    }
}
