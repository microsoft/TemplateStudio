Namespace Services.Ink
    Public Class InkZoomService
        Private Const DefaultZoomFactor As Single = 0.1F
        Private ReadOnly _scrollViewer As ScrollViewer

        Public Sub New(scrollViewer As ScrollViewer)
            _scrollViewer = scrollViewer
        End Sub

        Public Function ZoomIn(Optional zoomFactor As Single = DefaultZoomFactor) As Single
            Return ExecuteZoom(_scrollViewer.ZoomFactor + zoomFactor)
        End Function

        Public Function ZoomOut(Optional zoomFactor As Single = DefaultZoomFactor) As Single
            Return ExecuteZoom(_scrollViewer.ZoomFactor - zoomFactor)
        End Function

        Public Function ResetZoom() As Single
            Return ExecuteZoom(1F)
        End Function

        Public Sub FitToScreen()
            Dim element As FrameworkElement = TryCast(_scrollViewer.Content, FrameworkElement)

            If element IsNot Nothing Then
                FitToSize(element.Width, element.Height)
            End If
        End Sub

        Public Sub FitToSize(width As Double, height As Double)
            If width = 0 OrElse height = 0 Then
                Return
            End If

            Dim ratioWidth = _scrollViewer.ViewportWidth / width
            Dim ratioHeight = _scrollViewer.ViewportHeight / height
            Dim zoomFactor = CSng(Math.Min(ratioWidth, ratioHeight))
            ExecuteZoom(zoomFactor)
        End Sub

        Private Function ExecuteZoom(zoomFactor As Single) As Single
            If _scrollViewer.ChangeView(_scrollViewer.HorizontalOffset, _scrollViewer.VerticalOffset, zoomFactor) Then
                Return zoomFactor
            End If

            Return _scrollViewer.ZoomFactor
        End Function
    End Class
End Namespace
