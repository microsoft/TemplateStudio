using Param_RootNamespace.EventHandlers.Ink;
using System.Collections.Generic;
using System.Linq;
using Windows.UI.Input.Inking;
using Windows.UI.Xaml;

namespace Param_RootNamespace.Services.Ink.UndoRedo
{
    public class ClearStrokesAndShapesUndoRedoOperation : IUndoRedoOperation
    {
        private readonly InkStrokesService _strokeService;
        private readonly InkTransformService _transformService;
        private List<InkStroke> _strokes;
        private List<UIElement> _textAndShapes;

        public ClearStrokesAndShapesUndoRedoOperation(
            IEnumerable<InkStroke> strokes,
            IEnumerable<UIElement> textAndShapes,
            InkStrokesService strokeService,
            InkTransformService transformService)
        {
            _strokes = new List<InkStroke>(strokes);
            _textAndShapes = new List<UIElement>(textAndShapes);
            _strokeService = strokeService;
            _transformService = transformService;

            _strokeService.AddStrokeEvent += StrokeService_AddStrokeEvent;
        }

        public void ExecuteRedo()
        {
            _strokes.ForEach(s => _strokeService.RemoveStroke(s));
            _textAndShapes.ForEach(s => _transformService.RemoveUIElement(s));
        }

        public void ExecuteUndo()
        {
            _strokes.ToList().ForEach(s => _strokeService.AddStroke(s));
            _textAndShapes.ForEach(s => _transformService.AddUIElement(s));
        }

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