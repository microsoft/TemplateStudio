Namespace Services
    Public Class OnBackgroundEnteringEventArgs
        Inherits EventArgs

        Public Property SuspensionState As SuspensionState
        
        Public Property Target As Type

        Public Sub New(suspensionState As SuspensionState, target As Type)
            MyBase.New()
            SuspensionState = suspensionState
            Target = target
        End Sub
      End Class
End Namespace
