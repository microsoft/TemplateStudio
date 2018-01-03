Imports Param_ItemNamespace.Helpers
Imports Param_ItemNamespace.EventHandlers

Namespace ViewModels
    Public Class CameraViewViewModel
        Inherits System.ComponentModel.INotifyPropertyChanged

        Private _photoTakenCommand As ICommand

        Private _photo As BitmapImage

        Public Property Photo As BitmapImage
            Get
                Return _photo
            End Get
            Set
                  [Param_Setter](_photo, value)
            End Set
        End Property

        Public ReadOnly Property PhotoTakenCommand As ICommand
            Get
                If _photoTakenCommand Is Nothing Then
                    _photoTakenCommand = New RelayCommand(Of CameraControlEventArgs)(AddressOf OnPhotoTaken)
                End If

                Return _photoTakenCommand
            End Get
        End Property

        Private Sub OnPhotoTaken(args As CameraControlEventArgs)
            If Not String.IsNullOrEmpty(args.Photo) Then
                Photo = New BitmapImage(New Uri(args.Photo))
            End If
        End Sub
    End Class
End Namespace
