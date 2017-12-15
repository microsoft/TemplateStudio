Namespace EventHandlers
    Public Class CameraControlEventArgs
        Inherits EventArgs

        Public Property Photo As String

        Public Sub New(photo_ As String)
            Photo = photo_
        End Sub
    End Class
End Namespace
