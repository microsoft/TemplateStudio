Imports Param_RootNamespace.Core.Helpers
Imports Param_RootNamespace.Core.Services
Imports Param_RootNamespace.Helpers

Namespace Views
    Public NotInheritable Partial Class LogInPage
        Inherits Page
        Implements INotifyPropertyChanged

        Private _statusMessage As String
        Private _isBusy As Boolean

        Private ReadOnly Property IdentityService As IdentityService
            Get
                Return Singleton(Of IdentityService).Instance
            End Get
        End Property

        Public Property StatusMessage As String
            Get
                Return _statusMessage
            End Get
            Set(value As String)
                [Set](_statusMessage, value)
            End Set
        End Property

        Public Property IsBusy As Boolean
            Get
                Return _isBusy
            End Get
            Set(value As Boolean)
                [Set](_isBusy, value)
            End Set
        End Property

        Public Sub New()
            InitializeComponent()
        End Sub

        Private Async Sub OnLogIn(sender As Object, e As RoutedEventArgs)
            IsBusy = True
            StatusMessage = String.Empty
            Dim loginResult = Await IdentityService.LoginAsync()
            StatusMessage = GetStatusMessage(loginResult)
            IsBusy = False
        End Sub

        Private Function GetStatusMessage(loginResult As LoginResultType) As String
            Select Case loginResult
                Case LoginResultType.Unauthorized
                    Return "StatusUnauthorized".GetLocalized()
                Case LoginResultType.NoNetworkAvailable
                    Return "StatusNoNetworkAvailable".GetLocalized()
                Case LoginResultType.UnknownError
                    Return "StatusLoginFails".GetLocalized()
                Case Else
                    Return String.Empty
            End Select
        End Function

        Public Event PropertyChanged As PropertyChangedEventHandler Implements INotifyPropertyChanged.PropertyChanged

        Private Sub [Set](Of T)(ByRef storage As T, value As T, <CallerMemberName> Optional propertyName As String = Nothing)
            If Equals(storage, value) Then
                Return
            End If

            storage = value
            OnPropertyChanged(propertyName)
        End Sub

        Private Sub OnPropertyChanged(propertyName As String)
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs(propertyName))
        End Sub
    End Class
End Namespace
