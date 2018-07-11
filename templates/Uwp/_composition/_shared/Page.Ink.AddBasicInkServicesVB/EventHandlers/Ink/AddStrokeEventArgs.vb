Imports Windows.UI.Input.Inking

Namespace EventHandlers.Ink
    Public Class AddStrokeEventArgs
        Inherits EventArgs

        Public Property OldStroke As InkStroke

        Public Property NewStroke As InkStroke

        Public Sub New(newStroke As InkStroke, Optional oldStroke As InkStroke = Nothing)
            NewStroke = newStroke
            OldStroke = oldStroke
        End Sub
    End Class
End Namespace
