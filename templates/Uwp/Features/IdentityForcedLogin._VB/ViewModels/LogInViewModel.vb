Imports Param_RootNamespace.Core.Helpers
Imports Param_RootNamespace.Core.Services
Imports Param_RootNamespace.Helpers

Namespace ViewModels
    Public Class LogInViewModel
        Inherits System.ComponentModel.INotifyPropertyChanged

        Private _statusMessage As String
        Private _isBusy As Boolean
        Private _loginCommand As RelayCommand

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
                [Param_Setter](_statusMessage, newValue := value)
            End Set
        End Property

        Public Property IsBusy As Boolean
            Get
                Return _isBusy
            End Get
            Set(value As Boolean)
                [Param_Setter](_isBusy, value)
            End Set
        End Property

        Public ReadOnly Property LoginCommand As RelayCommand
            Get
                If _loginCommand Is Nothing Then
                    _loginCommand = New RelayCommand(AddressOf OnLogin, Function() Not IsBusy)
                End If

                Return _loginCommand
            End Get
        End Property

        Public Sub New()
        End Sub

        Private Async Sub OnLogin()
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
    End Class
End Namespace
