Namespace EventHandlers
    Public Class CameraControlEventArgs
        Inherits EventArgs

        Private _Photo As String

        Public Property Photo As String
            Get
                Return _Photo
            End Get
            Set
                _Photo = Value
            End Set
        End Property

        Public Sub New(photo__1 As String)
            Photo = photo__1
        End Sub
    End Class
End Namespace
