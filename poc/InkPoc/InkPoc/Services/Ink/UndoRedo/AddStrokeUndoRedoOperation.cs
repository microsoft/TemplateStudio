using System.Collections.Generic;
using System.Linq;
using Windows.UI.Input.Inking;

namespace InkPoc.Services.Ink
{
    public class AddStrokeUndoRedoOperation : IUndoRedoOperation
    {
        private List<InkStroke> strokes;
        private readonly InkStrokesService strokeService;

        public AddStrokeUndoRedoOperation(IEnumerable<InkStroke> _strokes, InkStrokesService _strokeService)
        {
            strokes = new List<InkStroke>(_strokes);
            strokeService = _strokeService;

            strokeService.AddStrokeEvent += StrokeService_AddStrokeEvent;
        }

        public void ExecuteUndo() => strokes.ForEach(s => strokeService.RemoveStroke(s));

        public void ExecuteRedo() => strokes.ToList().ForEach(s => strokeService.AddStroke(s));

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
