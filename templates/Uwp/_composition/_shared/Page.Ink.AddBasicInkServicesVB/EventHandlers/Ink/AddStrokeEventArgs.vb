Imports Windows.UI.Input.Inking

Namespace EventHandlers.Ink
    Public Class AddStrokeEventArgs
        Inherits EventArgs

        Public Property OldStroke As InkStroke

        Public Property NewStroke As InkStroke

        Public Sub New(newStroke As InkStroke, Optional oldStroke As InkStroke = Nothing)
            Me.NewStroke = newStroke
            Me.OldStroke = oldStroke
        End Sub
    End Class
End Namespace
