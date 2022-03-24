Imports Windows.UI.Input.Inking

Namespace EventHandlers.Ink
    Public Class CopyPasteStrokesEventArgs
        Public Property Strokes As IEnumerable(Of InkStroke)

        Public Sub New(strokes As IEnumerable(Of InkStroke))
            Me.Strokes = strokes
        End Sub
    End Class
End Namespace
