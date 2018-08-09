using System.Collections.Generic;
using Windows.Foundation;
using Windows.UI.Input.Inking;
using Param_ItemNamespace.EventHandlers.Ink;

namespace Param_ItemNamespace.Services.Ink.UndoRedo
{
    public class MoveStrokesUndoRedoOperation : IUndoRedoOperation
    {
        private readonly Point _endPosition;
        private readonly Point _startPosition;
        private readonly List<InkStroke> _strokes;
        private readonly InkStrokesService _strokeService;

        public MoveStrokesUndoRedoOperation(
            IEnumerable<InkStroke> strokes,
            Point startPosition,
            Point endPosition,
            InkStrokesService strokeService)
        {
            _strokes = new List<InkStroke>(strokes);
            _startPosition = startPosition;
            _endPosition = endPosition;
            _strokeService = strokeService;

            _strokeService.AddStrokeEvent += StrokeService_AddStrokeEvent;
        }

        public void ExecuteRedo()
        {
            _strokeService.SelectStrokes(_strokes);
            _strokeService.MoveSelectedStrokes(_startPosition, _endPosition);
        }

        public void ExecuteUndo()
        {
            _strokeService.SelectStrokes(_strokes);
            _strokeService.MoveSelectedStrokes(_endPosition, _startPosition);
        }

        private void StrokeService_AddStrokeEvent(object sender, AddStrokeEventArgs e)
        {
            if (e.NewStroke == null)
            {
                return;
            }

            var removedStrokes = _strokes.RemoveAll(s => s.Id == e.OldStroke?.Id);
            if (removedStrokes > 0)
            {
                _strokes.Add(e.NewStroke);
            }
        }
    }
}
