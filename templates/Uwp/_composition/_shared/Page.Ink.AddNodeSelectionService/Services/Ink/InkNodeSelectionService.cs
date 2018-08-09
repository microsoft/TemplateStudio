using System;
using System.Linq;
using System.Threading.Tasks;
using Windows.UI.Input.Inking;
using Windows.UI.Input.Inking.Analysis;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;

namespace Param_ItemNamespace.Services.Ink
{
    public class InkNodeSelectionService
    {
        private const double BusyWaitingTime = 200;
        private const double TripleTapTime = 400;

        private readonly InkCanvas _inkCanvas;
        private readonly InkPresenter _inkPresenter;
        private readonly InkAsyncAnalyzer _analyzer;
        private readonly InkStrokesService _strokeService;
        private readonly InkSelectionRectangleService _selectionRectangleService;
        private readonly Canvas _selectionCanvas;

        private IInkAnalysisNode selectedNode;
        private DateTime lastDoubleTapTime;

        public InkNodeSelectionService(
            InkCanvas inkCanvas,
            Canvas selectionCanvas,
            InkAsyncAnalyzer analyzer,
            InkStrokesService strokeService,
            InkSelectionRectangleService selectionRectangleService)
        {
            _inkCanvas = inkCanvas;
            _selectionCanvas = selectionCanvas;
            _inkPresenter = _inkCanvas.InkPresenter;
            _analyzer = analyzer;
            _strokeService = strokeService;
            _selectionRectangleService = selectionRectangleService;

            _inkCanvas.Tapped += InkCanvas_Tapped;
            _inkCanvas.DoubleTapped += InkCanvas_DoubleTapped;
            _inkCanvas.PointerPressed += InkCanvas_PointerPressed;
            _inkPresenter.StrokesErased += InkPresenter_StrokesErased;
        }

        public void ClearSelection()
        {
            selectedNode = null;
            _strokeService.ClearStrokesSelection();
            _selectionRectangleService.Clear();
        }

        private void InkCanvas_Tapped(object sender, TappedRoutedEventArgs e)
        {
            var position = e.GetPosition(_inkCanvas);

            if (selectedNode != null && RectHelper.Contains(selectedNode.BoundingRect, position))
            {
                if (DateTime.Now.Subtract(lastDoubleTapTime).TotalMilliseconds < TripleTapTime)
                {
                    ExpandSelection();
                }
            }
            else
            {
                selectedNode = _analyzer.FindHitNode(position);
                ShowOrHideSelection(selectedNode);
            }
        }

        private void InkCanvas_DoubleTapped(object sender, DoubleTappedRoutedEventArgs e)
        {
            var position = e.GetPosition(_inkCanvas);

            if (selectedNode != null && RectHelper.Contains(selectedNode.BoundingRect, position))
            {
                ExpandSelection();
                lastDoubleTapTime = DateTime.Now;
            }
        }

        private async void InkCanvas_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            var position = e.GetCurrentPoint(_inkCanvas).Position;

            while (_analyzer.IsAnalyzing)
            {
                await Task.Delay(TimeSpan.FromMilliseconds(BusyWaitingTime));
            }

                if (_selectionRectangleService.ContainsPosition(position))
                {
                    // Pressed on the selected rect, do nothing
                    return;
                }

            selectedNode = _analyzer.FindHitNode(position);
            ShowOrHideSelection(selectedNode);
        }

        private void InkPresenter_StrokesErased(InkPresenter sender, InkStrokesErasedEventArgs e)
        {
            if (e.Strokes.Any(s => s.Selected))
            {
                ClearSelection();
            }
        }

        private void ExpandSelection()
        {
            if (selectedNode != null &&
                selectedNode.Kind != InkAnalysisNodeKind.UnclassifiedInk &&
                selectedNode.Kind != InkAnalysisNodeKind.InkDrawing &&
                selectedNode.Kind != InkAnalysisNodeKind.WritingRegion)
            {
                selectedNode = selectedNode.Parent;
                if (selectedNode.Kind == InkAnalysisNodeKind.ListItem && selectedNode.Children.Count == 1)
                {
                    // Hierarchy: WritingRegion->Paragraph->ListItem->Line->{Bullet, Word1, Word2...}
                    // When a ListItem has only one Line, the bounding rect is same with its child Line,
                    // in this case, we skip one level to avoid confusion.
                    selectedNode = selectedNode.Parent;
                }

                ShowOrHideSelection(selectedNode);
            }
        }

        private void ShowOrHideSelection(IInkAnalysisNode node)
        {
            if (node != null)
            {
                var rect = _strokeService.SelectStrokesByNode(node);
                _selectionRectangleService.UpdateSelectionRect(rect);
            }
            else
            {
                ClearSelection();
            }
        }
    }
}