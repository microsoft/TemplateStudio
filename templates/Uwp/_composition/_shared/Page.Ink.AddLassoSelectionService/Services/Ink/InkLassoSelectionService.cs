using System.Linq;
using Windows.UI;
using Windows.UI.Core;
using Windows.UI.Input.Inking;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Shapes;

namespace Param_ItemNamespace.Services.Ink
{
    public class InkLassoSelectionService
    {
        private readonly InkPresenter inkPresenter;
        private readonly Canvas _selectionCanvas;
        private readonly InkStrokesService _strokeService;
        private readonly InkSelectionRectangleService _selectionRectangleService;

        private bool enableLasso;
        private Polyline lasso;

        public InkLassoSelectionService(
            InkCanvas inkCanvas,
            Canvas selectionCanvas,
            InkStrokesService strokeService,
            InkSelectionRectangleService selectionRectangleService)
        {
            // Initialize properties
            inkPresenter = inkCanvas.InkPresenter;
            _selectionCanvas = selectionCanvas;
            _strokeService = strokeService;
            _selectionRectangleService = selectionRectangleService;

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
            if (_selectionRectangleService.ContainsPosition(position))
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
            _selectionCanvas.Children.Add(lasso);
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

            var rect = _strokeService.SelectStrokesByPoints(lasso.Points);
            enableLasso = false;

            _selectionCanvas.Children.Remove(lasso);
            _selectionRectangleService.UpdateSelectionRect(rect);
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
            _strokeService.ClearStrokesSelection();
            _selectionRectangleService.Clear();
        }
    }
}
