using System.Linq;
using Windows.UI;
using Windows.UI.Core;
using Windows.UI.Input.Inking;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Shapes;

namespace InkPoc.Services.Ink
{
    public class InkLassoSelectionService
    {
        private readonly InkPresenter inkPresenter;
        private readonly Canvas selectionCanvas;
        private readonly InkStrokesService strokeService;
        private readonly InkSelectionRectangleService selectionRectangleService;

        private bool enableLasso;
        private Polyline lasso;        

        public InkLassoSelectionService(InkCanvas _inkCanvas, Canvas _selectionCanvas, InkStrokesService _strokeService, InkSelectionRectangleService _selectionRectangleService)
        {
            // Initialize properties
            inkPresenter = _inkCanvas.InkPresenter;
            selectionCanvas = _selectionCanvas;
            strokeService = _strokeService;
            selectionRectangleService = _selectionRectangleService;

            // lasso selection
            inkPresenter.StrokeInput.StrokeStarted += StrokeInput_StrokeStarted;
            inkPresenter.StrokesErased += InkPresenter_StrokesErased;
        }

        private void StrokeInput_StrokeStarted(InkStrokeInput sender, PointerEventArgs args)
        {
            EndLassoSelectionConfig();
        }

        private void InkPresenter_StrokesErased(InkPresenter sender, InkStrokesErasedEventArgs args)
        {
            EndLassoSelectionConfig();

            if (args.Strokes.Any(s => s.Selected))
            {
                ClearSelection();
            }
        }

        private void UnprocessedInput_PointerPressed(InkUnprocessedInput sender, PointerEventArgs args)
        {
            var position = args.CurrentPoint.Position;
            if (selectionRectangleService.ContainsPosition(position))
            {
                // Pressed on the selected rect, do nothing
                return;
            }

            lasso = new Polyline()
            {
                Stroke = new SolidColorBrush(Colors.Blue),
                StrokeThickness = 1,
                StrokeDashArray = new DoubleCollection() { 5, 2 },
            };

            lasso.Points.Add(args.CurrentPoint.RawPosition);
            selectionCanvas.Children.Add(lasso);
            enableLasso = true;
        }

        private void UnprocessedInput_PointerMoved(InkUnprocessedInput sender, PointerEventArgs args)
        {
            if (enableLasso)
            {
                lasso.Points.Add(args.CurrentPoint.RawPosition);
            }
        }

        private void UnprocessedInput_PointerReleased(InkUnprocessedInput sender, PointerEventArgs args)
        {
            lasso.Points.Add(args.CurrentPoint.RawPosition);

            var rect = strokeService.SelectStrokesByPoints(lasso.Points);
            enableLasso = false;

            selectionCanvas.Children.Remove(lasso);
            selectionRectangleService.UpdateSelectionRect(rect);
        }
        
        public void StartLassoSelectionConfig()
        {
            inkPresenter.InputProcessingConfiguration.RightDragAction = InkInputRightDragAction.LeaveUnprocessed;

            inkPresenter.UnprocessedInput.PointerPressed += UnprocessedInput_PointerPressed;
            inkPresenter.UnprocessedInput.PointerMoved += UnprocessedInput_PointerMoved;
            inkPresenter.UnprocessedInput.PointerReleased += UnprocessedInput_PointerReleased;
        }

        public void EndLassoSelectionConfig()
        {
            inkPresenter.UnprocessedInput.PointerPressed -= UnprocessedInput_PointerPressed;
            inkPresenter.UnprocessedInput.PointerMoved -= UnprocessedInput_PointerMoved;
            inkPresenter.UnprocessedInput.PointerReleased -= UnprocessedInput_PointerReleased;
        }

        public void ClearSelection()
        {
            strokeService.ClearStrokesSelection();
            selectionRectangleService.Clear();
        }
    }
}
