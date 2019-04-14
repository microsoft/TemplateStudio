'{[{
Imports Param_RootNamespace.Core.Services
Imports Param_RootNamespace.Core.Helpers
Imports Param_RootNamespace.Models
'}]}
Namespace Views
    Public NotInheritable Partial Class ShellPage
        Inherits Page
        Implements INotifyPropertyChanged

        Private _selected As WinUI.NavigationViewItem
'{[{
        Private _user As UserData

        Private ReadOnly Property IdentityService As IdentityService
            Get
                Return Singleton(Of IdentityService).Instance
            End Get
        End Property

        Private ReadOnly Property UserDataService As UserDataService
            Get
                Return Singleton(Of UserDataService).Instance
            End Get
        End Property
'}]}
        Public Property Selected As WinUI.NavigationViewItem

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

        Private Sub Initialize()
'^^
'{[{
            AddHandler IdentityService.LoggedOut, AddressOf OnLoggedOut
            AddHandler UserDataService.UserDataUpdated, AddressOf OnUserDataUpdated
'}]}
        End Sub

        Private Async Sub OnLoaded(sender As Object, e As RoutedEventArgs)
'^^
'{[{
            User = Await UserDataService.GetUserAsync()
'}]}
        End Sub
'{[{

        Private Sub OnUserDataUpdated(sender As Object, user As UserData)
            User = user
        End Sub

        Private Sub OnLoggedOut(sender As Object, e As EventArgs)
            RemoveHandler NavigationService.NavigationFailed, Frame_NavigationFailed
            RemoveHandler NavigationService.Navigated, Frame_Navigated
            RemoveHandler navigationView.BackRequested, OnBackRequested
            RemoveHandler UserDataService.UserDataUpdated, AddressOf OnUserDataUpdated
            RemoveHandler IdentityService.LoggedOut, AddressOf OnLoggedOut
        End Sub

        Private Sub OnUserProfile(sender As Object, e As RoutedEventArgs)
            NavigationService.Navigate(Of SettingsPage)()
        End Sub
'}]}
    End Class
End Namespace
