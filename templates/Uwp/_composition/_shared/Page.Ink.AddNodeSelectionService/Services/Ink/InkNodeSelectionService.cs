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
        private const double BUSYWAITINGTIME = 200;
        private const double TRIPLETAPTIME = 400;

        private readonly InkCanvas _inkCanvas;
        private readonly InkPresenter _inkPresenter;
        private readonly InkAsyncAnalyzer _analyzer;

        private readonly InkStrokesService _strokeService;
        private readonly InkSelectionRectangleService _selectionRectangleService;
        private readonly Canvas _selectionCanvas;

        private IInkAnalysisNode _selectedNode;
        private DateTime _lastDoubleTapTime;

        public InkNodeSelectionService(
            InkCanvas inkCanvas,
            Canvas selectionCanvas,
            InkAsyncAnalyzer analyzer,
            InkStrokesService strokeService,
            InkSelectionRectangleService selectionRectangleService)
        {
            // Initialize properties
            _inkCanvas = inkCanvas;
            _selectionCanvas = selectionCanvas;
            _inkPresenter = _inkCanvas.InkPresenter;
            _analyzer = analyzer;
            _strokeService = strokeService;
            _selectionRectangleService = selectionRectangleService;

            // selection on tap
            _inkCanvas.Tapped += InkCanvas_Tapped;
            _inkCanvas.DoubleTapped += InkCanvas_DoubleTapped;

            // drag and drop
            _inkCanvas.PointerPressed += InkCanvas_PointerPressed;
            _inkPresenter.StrokesErased += InkPresenter_StrokesErased;
        }

        private void InkCanvas_Tapped(object sender, TappedRoutedEventArgs e)
        {
            var position = e.GetPosition(_inkCanvas);

            if (_selectedNode != null && RectHelper.Contains(_selectedNode.BoundingRect, position))
            {
                if (DateTime.Now.Subtract(_lastDoubleTapTime).TotalMilliseconds < TRIPLETAPTIME)
                {
                    ExpandSelection();
                }
            }
            else
            {
                _selectedNode = _analyzer.FindHitNode(position);
                ShowOrHideSelection(_selectedNode);
            }
        }

        private void InkCanvas_DoubleTapped(object sender, DoubleTappedRoutedEventArgs e)
        {
            var position = e.GetPosition(_inkCanvas);

            if (_selectedNode != null && RectHelper.Contains(_selectedNode.BoundingRect, position))
            {
                ExpandSelection();
                _lastDoubleTapTime = DateTime.Now;
            }
        }

        private async void InkCanvas_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            var position = e.GetCurrentPoint(_inkCanvas).Position;

            while (_analyzer.IsAnalyzing)
            {
                await Task.Delay(TimeSpan.FromMilliseconds(BUSYWAITINGTIME));
            }

                if (_selectionRectangleService.ContainsPosition(position))
                {
                    // Pressed on the selected rect, do nothing
                    return;
                }

            _selectedNode = _analyzer.FindHitNode(position);
            ShowOrHideSelection(_selectedNode);
        }

        private void InkPresenter_StrokesErased(InkPresenter sender, InkStrokesErasedEventArgs e)
        {
            if (e.Strokes.Any(s => s.Selected))
            {
                ClearSelection();
            }
        }

        public void ClearSelection()
        {
            _selectedNode = null;
            _strokeService.ClearStrokesSelection();
            _selectionRectangleService.Clear();
        }

        private void ExpandSelection()
        {
            if (_selectedNode != null &&
                _selectedNode.Kind != InkAnalysisNodeKind.UnclassifiedInk &&
                _selectedNode.Kind != InkAnalysisNodeKind.InkDrawing &&
                _selectedNode.Kind != InkAnalysisNodeKind.WritingRegion)
            {
                _selectedNode = _selectedNode.Parent;
                if (_selectedNode.Kind == InkAnalysisNodeKind.ListItem && _selectedNode.Children.Count == 1)
                {
                    // Hierarchy: WritingRegion->Paragraph->ListItem->Line->{Bullet, Word1, Word2...}
                    // When a ListItem has only one Line, the bounding rect is same with its child Line,
                    // in this case, we skip one level to avoid confusion.
                    _selectedNode = _selectedNode.Parent;
                }

                ShowOrHideSelection(_selectedNode);
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