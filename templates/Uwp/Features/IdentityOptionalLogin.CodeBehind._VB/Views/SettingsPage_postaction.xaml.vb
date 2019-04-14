'{[{
Imports Param_RootNamespace.Models
Imports Param_RootNamespace.Core.Helpers
Imports Param_RootNamespace.Core.Services
'}]}
Namespace Views
    Public NotInheritable Partial Class SettingsPage
        Inherits Page
        Implements INotifyPropertyChanged
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
'{[{
        Private _isLoggedIn As Boolean
        Private _isBusy As Boolean
        Private _user As UserData
'}]}
        Public Property ElementTheme As ElementTheme

'^^
'{[{
        Public Property IsLoggedIn As Boolean
            Get
                Return _isLoggedIn
            End Get
            Set(value As Boolean)
                [Set](_isLoggedIn, value)
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

        Public Property User As UserData
            Get
                Return _user
            End Get
            Set(value As UserData)
                [Set](_user, value)
            End Set
        End Property
'}]}

        Public Sub New()
        End Sub

        Private Async Function InitializeAsync() As Task
            VersionDescription = GetVersionDescription()
'{[{
            AddHandler IdentityService.LoggedIn, AddressOf OnLoggedIn
            AddHandler IdentityService.LoggedOut, AddressOf OnLoggeOut
            AddHandler UserDataService.UserDataUpdated, AddressOf OnUserDataUpdated
            IsLoggedIn = IdentityService.IsLoggedIn()
            User = Await UserDataService.GetUserAsync()
'}]}
        End Function

'^^
'{[{
        Private Sub OnUserDataUpdated(sender As Object, user As UserData)
            User = user
        End Sub

        Private Async Sub OnLogIn(sender As Object, e As RoutedEventArgs)
            IsBusy = True
            Dim loginResult = Await IdentityService.LoginAsync()

            If loginResult <> LoginResultType.Success Then
                Await AuthenticationHelper.ShowLoginErrorAsync(loginResult)
                IsBusy = False
            End If
        End Sub

        Private Async Sub OnLogOut(sender As Object, e As RoutedEventArgs)
            IsBusy = True
            Await IdentityService.LogoutAsync()
        End Sub

        Private Sub OnLoggedIn(sender As Object, e As EventArgs)
            IsLoggedIn = True
            IsBusy = False
        End Sub

        Private Sub OnLoggeOut(sender As Object, e As EventArgs)
            User = Nothing
            IsLoggedIn = False
            IsBusy = False
        End Sub

        Protected Overrides Sub OnNavigatingFrom(e As NavigatingCancelEventArgs)
            MyBase.OnNavigatingFrom(e)
            RemoveHandler IdentityService.LoggedIn, AddressOf OnLoggedIn
            RemoveHandler IdentityService.LoggedOut, AddressOf OnLoggeOut
            RemoveHandler UserDataService.UserDataUpdated, AddressOf OnUserDataUpdated
        End Sub

'}]}
        Public Event PropertyChanged As PropertyChangedEventHandler
    End Class
End Namespace
