using System.Collections.Generic;
using Windows.UI.Input.Inking;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Param_ItemNamespace.Services.Ink
{
    public class InkTransformResult
    {
        public List<InkStroke> Strokes { get; set; } = new List<InkStroke>();

        public List<UIElement> TextAndShapes { get; set; } = new List<UIElement>();

        public Canvas DrawingCanvas { get; set; }

        public InkTransformResult(Canvas drawingCanvas)
        {
            DrawingCanvas = drawingCanvas;
        }
    }
}
