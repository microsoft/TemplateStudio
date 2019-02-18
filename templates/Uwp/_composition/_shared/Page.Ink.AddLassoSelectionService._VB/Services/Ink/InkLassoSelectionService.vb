Imports System.Linq
Imports Windows.UI
Imports Windows.UI.Core
Imports Windows.UI.Input.Inking
Imports Windows.UI.Xaml.Controls
Imports Windows.UI.Xaml.Media
Imports Windows.UI.Xaml.Shapes

Namespace Services.Ink
    Public Class InkLassoSelectionService
        Private ReadOnly _inkPresenter As InkPresenter
        Private ReadOnly _selectionCanvas As Canvas
        Private ReadOnly _strokeService As InkStrokesService
        Private ReadOnly _selectionRectangleService As InkSelectionRectangleService
        Private enableLasso As Boolean
        Private lasso As Polyline

        Public Sub New(inkCanvas As InkCanvas, selectionCanvas As Canvas, strokeService As InkStrokesService, selectionRectangleService As InkSelectionRectangleService)
            _inkPresenter = inkCanvas.InkPresenter
            _selectionCanvas = selectionCanvas
            _strokeService = strokeService
            _selectionRectangleService = selectionRectangleService
            AddHandler _inkPresenter.StrokeInput.StrokeStarted, AddressOf StrokeInput_StrokeStarted
            AddHandler _inkPresenter.StrokesErased, AddressOf InkPresenter_StrokesErased
        End Sub

        Public Sub StartLassoSelectionConfig()
            _inkPresenter.InputProcessingConfiguration.RightDragAction = InkInputRightDragAction.LeaveUnprocessed
            AddHandler _inkPresenter.UnprocessedInput.PointerPressed, AddressOf UnprocessedInput_PointerPressed
            AddHandler _inkPresenter.UnprocessedInput.PointerMoved, AddressOf UnprocessedInput_PointerMoved
            AddHandler _inkPresenter.UnprocessedInput.PointerReleased, AddressOf UnprocessedInput_PointerReleased
        End Sub

        Public Sub EndLassoSelectionConfig()
            RemoveHandler _inkPresenter.UnprocessedInput.PointerPressed, AddressOf UnprocessedInput_PointerPressed
            RemoveHandler _inkPresenter.UnprocessedInput.PointerMoved, AddressOf UnprocessedInput_PointerMoved
            RemoveHandler _inkPresenter.UnprocessedInput.PointerReleased, AddressOf UnprocessedInput_PointerReleased
        End Sub

        Public Sub ClearSelection()
            _strokeService.ClearStrokesSelection()
            _selectionRectangleService.Clear()
        End Sub

        Private Sub StrokeInput_StrokeStarted(sender As InkStrokeInput, args As PointerEventArgs)
            EndLassoSelectionConfig()
        End Sub

        Private Sub InkPresenter_StrokesErased(sender As InkPresenter, args As InkStrokesErasedEventArgs)
            EndLassoSelectionConfig()

            If args.Strokes.Any(Function(s) s.Selected) Then
                ClearSelection()
            End If
        End Sub

        Private Sub UnprocessedInput_PointerPressed(sender As InkUnprocessedInput, args As PointerEventArgs)
            Dim position = args.CurrentPoint.Position

            If _selectionRectangleService.ContainsPosition(position) Then
                ' Pressed on the selected rect, do nothing
                Return
            End If

            lasso = New Polyline() With {
                .Stroke = New SolidColorBrush(Colors.Blue),
                .StrokeThickness = 1,
                .StrokeDashArray = New DoubleCollection() From {
                    5,
                    2
                }
            }
            lasso.Points.Add(args.CurrentPoint.RawPosition)
            _selectionCanvas.Children.Add(lasso)
            enableLasso = True
        End Sub

        Private Sub UnprocessedInput_PointerMoved(sender As InkUnprocessedInput, args As PointerEventArgs)
            If enableLasso Then
                lasso.Points.Add(args.CurrentPoint.RawPosition)
            End If
        End Sub

        Private Sub UnprocessedInput_PointerReleased(sender As InkUnprocessedInput, args As PointerEventArgs)
            lasso.Points.Add(args.CurrentPoint.RawPosition)
            Dim rect = _strokeService.SelectStrokesByPoints(lasso.Points)
            enableLasso = False
            _selectionCanvas.Children.Remove(lasso)
            _selectionRectangleService.UpdateSelectionRect(rect)
        End Sub
    End Class
End Namespace
