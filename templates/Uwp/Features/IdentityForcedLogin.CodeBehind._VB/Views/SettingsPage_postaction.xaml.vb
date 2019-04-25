'{[{
Imports Param_RootNamespace.Core.Helpers
Imports Param_RootNamespace.Core.Services
Imports Param_RootNamespace.Models
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
        Private _user As UserData
'}]}

'^^
'{[{
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

        Public Async Function InitializeAsync() As Task
'^^
'{[{
            User = Await UserDataService.GetUserAsync()
'}]}
            Await Task.CompletedTask
        End Function

'^^
'{[{
        Protected Overrides Sub OnNavigatingFrom(e As NavigatingCancelEventArgs)
            MyBase.OnNavigatingFrom(e)
            UnregisterEvents()
        End Sub

        Public Sub UnregisterEvents()
            Removehandler IdentityService.LoggedOut, AddressOf OnLoggedOut
            Removehandler UserDataService.UserDataUpdated, AddressOf OnUserDataUpdated
        End Sub

        Private Sub OnUserDataUpdated(sender As Object, userData As UserData)
            User = userData
        End Sub

        Private Async Sub OnLogout(sender As Object, e As RoutedEventArgs)
            Await IdentityService.LogoutAsync()
        End Sub

        Private Sub OnLoggedOut(sender As Object, e As EventArgs)
            UnregisterEvents()
        End Sub
'}]}

        Public Event PropertyChanged As PropertyChangedEventHandler Implements INotifyPropertyChanged.PropertyChanged
    End Class
End Namespace
