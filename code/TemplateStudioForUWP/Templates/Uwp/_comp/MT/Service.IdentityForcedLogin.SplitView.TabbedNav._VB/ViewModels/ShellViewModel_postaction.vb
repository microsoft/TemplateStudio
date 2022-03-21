'{[{
Imports Param_RootNamespace.Core.Services
Imports Param_RootNamespace.Core.Helpers
Imports Microsoft.Toolkit.Mvvm.Input
'}]}
Namespace ViewModels
    Public Class ShellViewModel
        Inherits ObservableObject

        Private _itemInvokedCommand As ICommand
'{[{
        Private _userProfileCommand As ICommand
        Private _user As UserViewModel

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

        Public ReadOnly Property ItemInvokedCommand As ICommand
            Get
                If _itemInvokedCommand Is Nothing Then
                    _itemInvokedCommand = New RelayCommand(Of WinUI.NavigationViewItemInvokedEventArgs)(AddressOf OnItemInvoked)
                End If

                Return _itemInvokedCommand
            End Get
        End Property

'^^
'{[{
        Public ReadOnly Property UserProfileCommand As ICommand
            Get
                If _userProfileCommand Is Nothing Then
                    _userProfileCommand = New RelayCommand(AddressOf OnUserProfile)
                End If

                Return _userProfileCommand
            End Get
        End Property

        Public Property User As UserViewModel
            Get
                Return _user
            End Get
            Set(value As UserViewModel)
                SetProperty(_user, value)
            End Set
        End Property

'}]}
        Public Sub New()
        End Sub

        Public Sub Initialize(frame As Frame, navigationView As WinUI.NavigationView, keyboardAccelerators As IList(Of KeyboardAccelerator))
'^^
'{[{
            AddHandler IdentityService.LoggedOut, AddressOf OnLoggedOut
            AddHandler UserDataService.UserDataUpdated, AddressOf OnUserDataUpdated
'}]}
        End Sub

        Private Async Sub OnLoaded()
'^^
'{[{
            User = Await UserDataService.GetUserAsync()
'}]}
        End Sub

'{[{
        Private Sub OnUserDataUpdated(sender As Object, userData As UserViewModel)
            User = userData
        End Sub

        Private Sub OnLoggedOut(sender As Object, e As EventArgs)
            RemoveHandler NavigationService.NavigationFailed, AddressOf Frame_NavigationFailed
            RemoveHandler NavigationService.Navigated, AddressOf Frame_Navigated
            RemoveHandler _navigationView.BackRequested, AddressOf OnBackRequested
            RemoveHandler UserDataService.UserDataUpdated, AddressOf OnUserDataUpdated
            RemoveHandler IdentityService.LoggedOut, AddressOf OnLoggedOut
        End Sub

        Private Sub OnUserProfile()
            NavigationService.Navigate(Of SettingsPage)()
        End Sub
'}]}
    End Class
End Namespace
