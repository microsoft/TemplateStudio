Imports Windows.UI.Input.Inking
Imports Windows.UI.Input.Inking.Analysis

Namespace Services.Ink
    Public Class InkNodeSelectionService
        Private Const BusyWaitingTime As Double = 200
        Private Const TripleTapTime As Double = 400
        Private ReadOnly _inkCanvas As InkCanvas
        Private ReadOnly _inkPresenter As InkPresenter
        Private ReadOnly _analyzer As InkAsyncAnalyzer
        Private ReadOnly _strokeService As InkStrokesService
        Private ReadOnly _selectionRectangleService As InkSelectionRectangleService
        Private ReadOnly _selectionCanvas As Canvas
        Private selectedNode As IInkAnalysisNode
        Private lastDoubleTapTime As DateTime

        Public Sub New(inkCanvas As InkCanvas, selectionCanvas As Canvas, analyzer As InkAsyncAnalyzer, strokeService As InkStrokesService, selectionRectangleService As InkSelectionRectangleService)
            _inkCanvas = inkCanvas
            _selectionCanvas = selectionCanvas
            _inkPresenter = _inkCanvas.InkPresenter
            _analyzer = analyzer
            _strokeService = strokeService
            _selectionRectangleService = selectionRectangleService
            AddHandler _inkCanvas.Tapped, AddressOf InkCanvas_Tapped
            AddHandler _inkCanvas.DoubleTapped, AddressOf InkCanvas_DoubleTapped
            AddHandler _inkCanvas.PointerPressed, AddressOf InkCanvas_PointerPressed
            AddHandler _inkPresenter.StrokesErased, AddressOf InkPresenter_StrokesErased
        End Sub

        Public Sub ClearSelection()
            selectedNode = Nothing
            _strokeService.ClearStrokesSelection()
            _selectionRectangleService.Clear()
        End Sub

        Private Sub InkCanvas_Tapped(sender As Object, e As TappedRoutedEventArgs)
            Dim position = e.GetPosition(_inkCanvas)

            If selectedNode IsNot Nothing AndAlso RectHelper.Contains(selectedNode.BoundingRect, position) Then

                If DateTime.Now.Subtract(lastDoubleTapTime).TotalMilliseconds < TripleTapTime Then
                    ExpandSelection()
                End If
            Else
                selectedNode = _analyzer.FindHitNode(position)
                ShowOrHideSelection(selectedNode)
            End If
        End Sub

        Private Sub InkCanvas_DoubleTapped(sender As Object, e As DoubleTappedRoutedEventArgs)
            Dim position = e.GetPosition(_inkCanvas)

            If selectedNode IsNot Nothing AndAlso RectHelper.Contains(selectedNode.BoundingRect, position) Then
                ExpandSelection()
                lastDoubleTapTime = DateTime.Now
            End If
        End Sub

        Private Async Sub InkCanvas_PointerPressed(sender As Object, e As PointerRoutedEventArgs)
            Dim position = e.GetCurrentPoint(_inkCanvas).Position

            While _analyzer.IsAnalyzing
                Await Task.Delay(TimeSpan.FromMilliseconds(BusyWaitingTime))
            End While

            If _selectionRectangleService.ContainsPosition(position) Then
                ' Pressed on the selected rect, do nothing
                Return
            End If

            selectedNode = _analyzer.FindHitNode(position)
            ShowOrHideSelection(selectedNode)
        End Sub

        Private Sub InkPresenter_StrokesErased(sender As InkPresenter, e As InkStrokesErasedEventArgs)
            If e.Strokes.Any(Function(s) s.Selected) Then
                ClearSelection()
            End If
        End Sub

        Private Sub ExpandSelection()
            If selectedNode IsNot Nothing AndAlso selectedNode.Kind <> InkAnalysisNodeKind.UnclassifiedInk AndAlso selectedNode.Kind <> InkAnalysisNodeKind.InkDrawing AndAlso selectedNode.Kind <> InkAnalysisNodeKind.WritingRegion Then
                selectedNode = selectedNode.Parent

                If selectedNode.Kind = InkAnalysisNodeKind.ListItem AndAlso selectedNode.Children.Count = 1 Then
                    ' Hierarchy: WritingRegion->Paragraph->ListItem->Line->{Bullet, Word1, Word2...}
                    ' When a ListItem has only one Line, the bounding rect is same with its child Line,
                    ' in this case, we skip one level to avoid confusion.
                    selectedNode = selectedNode.Parent
                End If

                ShowOrHideSelection(selectedNode)
            End If
        End Sub

        Private Sub ShowOrHideSelection(node As IInkAnalysisNode)
            If node IsNot Nothing Then
                Dim rect = _strokeService.SelectStrokesByNode(node)
                _selectionRectangleService.UpdateSelectionRect(rect)
            Else
                ClearSelection()
            End If
        End Sub
    End Class
End Namespace
