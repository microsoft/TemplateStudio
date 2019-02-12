Imports Windows.UI.Input.Inking

Namespace EventHandlers.Ink
    Public Class MoveStrokesEventArgs

        Public Property StartPosition As Point

        Public Property EndPosition As Point

        Public Property Strokes As IEnumerable(Of InkStroke)

        Public Sub New(strokes As IEnumerable(Of InkStroke), startPosition As Point, endPosition As Point)
            Me.Strokes = strokes
            Me.StartPosition = startPosition
            Me.EndPosition = endPosition
        End Sub
    End Class
End Namespace
