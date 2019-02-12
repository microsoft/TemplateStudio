Namespace Services
    Public Class OnBackgroundEnteringEventArgs
        Inherits EventArgs

        Public Property SuspensionState As SuspensionState
        
        Public Property Target As Type

        Public Sub New(_suspensionState As SuspensionState, _target As Type)
            SuspensionState = _suspensionState
            Target = _target
        End Sub
      End Class
End Namespace
