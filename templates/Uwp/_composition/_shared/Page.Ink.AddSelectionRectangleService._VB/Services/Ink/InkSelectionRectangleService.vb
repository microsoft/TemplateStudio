Imports System.Linq
Imports Windows.Foundation
Imports Windows.UI
Imports Windows.UI.Xaml
Imports Windows.UI.Xaml.Controls
Imports Windows.UI.Xaml.Input
Imports Windows.UI.Xaml.Media
Imports Windows.UI.Xaml.Shapes

Namespace Services.Ink
    Public Class InkSelectionRectangleService
        Private Const SelectionRectName As String = "selectionRectangle"
        Private ReadOnly _selectionCanvas As Canvas
        Private ReadOnly _inkCanvas As InkCanvas
        Private ReadOnly _strokeService As InkStrokesService
        Private dragStartPosition As Point
        Private selectionStrokesRect As Rect = Rect.Empty

        Public Sub New(inkCanvas As InkCanvas, selectionCanvas As Canvas, strokeService As InkStrokesService)
            _inkCanvas = inkCanvas
            _selectionCanvas = selectionCanvas
            _strokeService = strokeService
            _inkCanvas.ManipulationMode = ManipulationModes.TranslateX Or ManipulationModes.TranslateY
            AddHandler _inkCanvas.ManipulationStarted, AddressOf InkCanvas_ManipulationStarted
            AddHandler _inkCanvas.ManipulationDelta, AddressOf InkCanvas_ManipulationDelta
            AddHandler _inkCanvas.ManipulationCompleted, AddressOf InkCanvas_ManipulationCompleted
        End Sub

        Public Sub UpdateSelectionRect(rect As Rect)
            selectionStrokesRect = rect
            Dim selectionRect = GetSelectionRectangle()
            selectionRect.Width = rect.Width
            selectionRect.Height = rect.Height
            Canvas.SetLeft(selectionRect, rect.Left)
            Canvas.SetTop(selectionRect, rect.Top)
        End Sub

        Public Sub Clear()
            selectionStrokesRect = Rect.Empty
            _selectionCanvas.Children.Clear()
        End Sub

        Public Function ContainsPosition(position As Point) As Boolean
            Return Not selectionStrokesRect.IsEmpty AndAlso RectHelper.Contains(selectionStrokesRect, position)
        End Function

        Private Function GetSelectionRectangle() As Rectangle
            Dim selectionRectangle = TryCast(_selectionCanvas.Children.FirstOrDefault(Function(f)
                                                                                         Dim rect As Rectangle = TryCast(f, Rectangle)
                                                                                         return rect IsNot Nothing AndAlso rect.Name = SelectionRectName
                                                                                     End Function), Rectangle)

            If selectionRectangle Is Nothing Then
                selectionRectangle = CreateNewSelectionRectangle()
                _selectionCanvas.Children.Add(selectionRectangle)
            End If

            Return selectionRectangle
        End Function

        Private Function CreateNewSelectionRectangle() As Rectangle
            Return New Rectangle() With {
                .Name = SelectionRectName,
                .Stroke = New SolidColorBrush(Colors.Gray),
                .StrokeThickness = 2,
                .StrokeDashArray = New DoubleCollection() From {
                    2,
                    2
                },
                .StrokeDashCap = PenLineCap.Round
            }
        End Function

        Private Sub InkCanvas_ManipulationStarted(sender As Object, e As ManipulationStartedRoutedEventArgs)
            If Not selectionStrokesRect.IsEmpty Then
                dragStartPosition = e.Position
            End If
        End Sub

        Private Sub InkCanvas_ManipulationDelta(sender As Object, e As ManipulationDeltaRoutedEventArgs)
            If Not selectionStrokesRect.IsEmpty Then
                Dim offset As Point
                offset.X = e.Delta.Translation.X
                offset.Y = e.Delta.Translation.Y
                MoveSelection(offset)
            End If
        End Sub

        Private Sub InkCanvas_ManipulationCompleted(sender As Object, e As ManipulationCompletedRoutedEventArgs)
            If Not selectionStrokesRect.IsEmpty Then
                _strokeService.MoveSelectedStrokes(dragStartPosition, e.Position)
            End If
        End Sub

        Private Sub MoveSelection(offset As Point)
            Dim selectionRect = GetSelectionRectangle()
            Dim left = Canvas.GetLeft(selectionRect)
            Dim top = Canvas.GetTop(selectionRect)
            Canvas.SetLeft(selectionRect, left + offset.X)
            Canvas.SetTop(selectionRect, top + offset.Y)
            selectionStrokesRect.X = left + offset.X
            selectionStrokesRect.Y = top + offset.Y
        End Sub
    End Class
End Namespace
