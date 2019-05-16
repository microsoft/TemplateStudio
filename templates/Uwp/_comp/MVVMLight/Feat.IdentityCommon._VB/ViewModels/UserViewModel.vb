Imports Param_RootNamespace.Helpers
Imports Windows.UI.Xaml.Media.Imaging
Imports GalaSoft.MvvmLight

Namespace ViewModels
    Public Class UserViewModel
        Inherits ViewModelBase

        Private _name As String
        Private _userPrincipalName As String
        Private _photo As BitmapImage

        Public Property Name As String
            Get
                Return _name
            End Get
            Set(value As String)
                [Set](_name, newValue := value)
            End Set
        End Property

        Public Property UserPrincipalName As String
            Get
                Return _userPrincipalName
            End Get
            Set(value As String)
                [Set](_userPrincipalName, newValue := value)
            End Set
        End Property

        Public Property Photo As BitmapImage
            Get
                Return _photo
            End Get
            Set(value As BitmapImage)
                [Set](_photo, value)
            End Set
        End Property

        Public Sub New()
        End Sub
    End Class
End Namespace
