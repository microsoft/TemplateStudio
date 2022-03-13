'{[{
Imports Param_RootNamespace.Core.Helpers
Imports Param_RootNamespace.Core.Services
'}]}
Namespace ViewModels
    Public Class SettingsViewModel
        Inherits System.ComponentModel.INotifyPropertyChanged

'{[{
        Private ReadOnly Property UserDataService As UserDataService
            Get
                Return Singleton(Of UserDataService).Instance
            End Get
        End Property

        Private ReadOnly Property IdentityService As IdentityService
            Get
                Return Singleton(Of IdentityService).Instance
            End Get
        End Property
'}]}

        Private _elementTheme As ElementTheme = ThemeSelectorService.Theme
'^^
'{[{
        Private _user As UserViewModel

        Private _logoutCommand As ICommand

        Public ReadOnly Property LogoutCommand As ICommand
            Get
                If _logoutCommand Is Nothing Then
                    _logoutCommand = New RelayCommand(AddressOf OnLogout)
                End If

                Return _logoutCommand
            End Get
        End Property

        Public Property User As UserViewModel
            Get
                Return _user
            End Get
            Set(value As UserViewModel)
                [Param_Setter](_user, value)
            End Set
        End Property
'}]}

        Public Sub New()
        End Sub

        Public Async Function InitializeAsync() As Task
            VersionDescription = GetVersionDescription()
'^^
'{[{
            AddHandler IdentityService.LoggedOut, AddressOf OnLoggedOut
            AddHandler UserDataService.UserDataUpdated, AddressOf OnUserDataUpdated
            User = Await UserDataService.GetUserAsync()
'}]}
        End Function
'^^
'{[{

        Public Sub UnregisterEvents()
            RemoveHandler IdentityService.LoggedOut, AddressOf OnLoggedOut
            RemoveHandler UserDataService.UserDataUpdated, AddressOf OnUserDataUpdated
        End Sub

        Private Sub OnUserDataUpdated(sender As Object, userData As UserViewModel)
            User = userData
        End Sub

        Private Async Sub OnLogout()
            Await IdentityService.LogoutAsync()
        End Sub

        Private Sub OnLoggedOut(sender As Object, e As EventArgs)
            UnregisterEvents()
        End Sub
'}]}
    End Class
End Namespace
