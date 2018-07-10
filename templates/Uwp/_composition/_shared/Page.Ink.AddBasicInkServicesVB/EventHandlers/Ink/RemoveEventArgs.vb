Imports Windows.UI.Input.Inking

Namespace EventHandlers.Ink
    Public Class RemoveEventArgs
        Public Property RemovedStroke As InkStroke

        Public Sub New(removedStroke As InkStroke)
            RemovedStroke = removedStroke
        End Sub
    End Class
End Namespace
