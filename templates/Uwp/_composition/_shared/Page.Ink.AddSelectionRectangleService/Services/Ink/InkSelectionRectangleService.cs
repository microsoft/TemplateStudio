using System.Linq;
using Windows.Foundation;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Shapes;

namespace Param_ItemNamespace.Services.Ink
{
    public class InkSelectionRectangleService
    {
        private const string SelectionRectName = "selectionRectangle";

        private readonly Canvas _selectionCanvas;
        private readonly InkCanvas _inkCanvas;
        private readonly InkStrokesService _strokeService;

        private Point dragStartPosition;
        private Rect selectionStrokesRect = Rect.Empty;

        public InkSelectionRectangleService(InkCanvas inkCanvas, Canvas selectionCanvas, InkStrokesService strokeService)
        {
            _inkCanvas = inkCanvas;
            _selectionCanvas = selectionCanvas;
            _strokeService = strokeService;

            _inkCanvas.ManipulationMode = ManipulationModes.TranslateX | ManipulationModes.TranslateY;
            _inkCanvas.ManipulationStarted += InkCanvas_ManipulationStarted;
            _inkCanvas.ManipulationDelta += InkCanvas_ManipulationDelta;
            _inkCanvas.ManipulationCompleted += InkCanvas_ManipulationCompleted;
        }

        public void UpdateSelectionRect(Rect rect)
        {
            selectionStrokesRect = rect;

            var selectionRect = GetSelectionRectangle();
            selectionRect.Width = rect.Width;
            selectionRect.Height = rect.Height;

            Canvas.SetLeft(selectionRect, rect.Left);
            Canvas.SetTop(selectionRect, rect.Top);
        }

        public void Clear()
        {
            selectionStrokesRect = Rect.Empty;
            _selectionCanvas.Children.Clear();
        }

        public bool ContainsPosition(Point position)
        {
            return !selectionStrokesRect.IsEmpty && RectHelper.Contains(selectionStrokesRect, position);
        }

        private Rectangle GetSelectionRectangle()
        {
            var selectionRectange = _selectionCanvas
                .Children
                .FirstOrDefault(f => f is Rectangle r && r.Name == SelectionRectName)
                as Rectangle;

            if (selectionRectange == null)
            {
                selectionRectange = CreateNewSelectionRectangle();
                _selectionCanvas.Children.Add(selectionRectange);
            }

            return selectionRectange;
        }

        private Rectangle CreateNewSelectionRectangle()
        {
            return new Rectangle()
            {
                Name = SelectionRectName,
                Stroke = new SolidColorBrush(Colors.Gray),
                StrokeThickness = 2,
                StrokeDashArray = new DoubleCollection() { 2, 2 },
                StrokeDashCap = PenLineCap.Round
            };
        }

        private void InkCanvas_ManipulationStarted(object sender, ManipulationStartedRoutedEventArgs e)
        {
            if (!selectionStrokesRect.IsEmpty)
            {
                dragStartPosition = e.Position;
            }
        }

        private void InkCanvas_ManipulationDelta(object sender, ManipulationDeltaRoutedEventArgs e)
        {
            if (!selectionStrokesRect.IsEmpty)
            {
                Point offset;
                offset.X = e.Delta.Translation.X;
                offset.Y = e.Delta.Translation.Y;
                MoveSelection(offset);
            }
        }

        private void InkCanvas_ManipulationCompleted(object sender, ManipulationCompletedRoutedEventArgs e)
        {
            if (!selectionStrokesRect.IsEmpty)
            {
                _strokeService.MoveSelectedStrokes(dragStartPosition, e.Position);
            }
        }

        private void MoveSelection(Point offset)
        {
            var selectionRect = GetSelectionRectangle();

            var left = Canvas.GetLeft(selectionRect);
            var top = Canvas.GetTop(selectionRect);
            Canvas.SetLeft(selectionRect, left + offset.X);
            Canvas.SetTop(selectionRect, top + offset.Y);

            selectionStrokesRect.X = left + offset.X;
            selectionStrokesRect.Y = top + offset.Y;
        }
    }
}