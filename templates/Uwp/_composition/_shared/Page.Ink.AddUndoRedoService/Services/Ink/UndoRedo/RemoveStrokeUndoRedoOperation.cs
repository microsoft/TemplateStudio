using System.Collections.Generic;
using System.Linq;
using Windows.UI.Input.Inking;
using Param_ItemNamespace.EventHandlers.Ink;

namespace Param_ItemNamespace.Services.Ink.UndoRedo
{
    public class RemoveStrokeUndoRedoOperation : IUndoRedoOperation
    {
        private readonly InkStrokesService _strokeService;
        private List<InkStroke> _strokes;

        public RemoveStrokeUndoRedoOperation(IEnumerable<InkStroke> strokes, InkStrokesService strokeService)
        {
            _strokes = new List<InkStroke>(strokes);
            _strokeService = strokeService;

            _strokeService.AddStrokeEvent += StrokeService_AddStrokeEvent;
        }

        public void ExecuteRedo() => _strokes.ForEach(s => _strokeService.RemoveStroke(s));

        public void ExecuteUndo() => _strokes.ToList().ForEach(s => _strokeService.AddStroke(s));

        private void StrokeService_AddStrokeEvent(object sender, AddStrokeEventArgs e)
        {
            if (e.NewStroke == null)
            {
                return;
            }

            var changes = _strokes.RemoveAll(s => s.Id == e.OldStroke?.Id);
            if (changes > 0)
            {
                _strokes.Add(e.NewStroke);
            }
        }
    }
}