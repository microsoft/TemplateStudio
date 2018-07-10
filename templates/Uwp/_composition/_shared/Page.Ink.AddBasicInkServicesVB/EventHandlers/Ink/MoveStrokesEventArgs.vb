Imports System.Collections.Generic
Imports Windows.Foundation
Imports Windows.UI.Input.Inking

Namespace EventHandlers.Ink
    Public Class MoveStrokesEventArgs
        Public Property StartPosition As Point
        Public Property EndPosition As Point
        Public Property Strokes As IEnumerable(Of InkStroke)

        Public Sub New(strokes As IEnumerable(Of InkStroke), startPosition As Point, endPosition As Point)
            Strokes = strokes
            StartPosition = startPosition
            EndPosition = endPosition
        End Sub
    End Class
End Namespace
