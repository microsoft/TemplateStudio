using System.Collections.Generic;
using Windows.Foundation;
using Windows.UI.Input.Inking;

namespace Param_ItemNamespace.EventHandlers.Ink
{
    public class MoveStrokesEventArgs
    {
        public Point StartPosition { get; set; }

        public Point EndPosition { get; set; }

        public IEnumerable<InkStroke> Strokes { get; set; }

        public MoveStrokesEventArgs(IEnumerable<InkStroke> _strokes, Point _startPosition, Point _endPosition)
        {
            Strokes = _strokes;
            StartPosition = _startPosition;
            EndPosition = _endPosition;
        }
    }
}
