Imports Windows.UI.Input.Inking

Namespace Services.Ink
    Public Class InkTransformResult

        Public Property Strokes As List(Of InkStroke) = New List(Of InkStroke)()

        Public Property TextAndShapes As List(Of UIElement) = New List(Of UIElement)()

        Public Property DrawingCanvas As Canvas

        Public Sub New(drawingCanvas As Canvas)
            Me.DrawingCanvas = drawingCanvas
        End Sub
    End Class
End Namespace
