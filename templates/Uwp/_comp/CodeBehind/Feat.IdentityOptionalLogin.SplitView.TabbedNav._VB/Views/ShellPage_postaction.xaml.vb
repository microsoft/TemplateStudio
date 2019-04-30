'{[{
Imports Param_RootNamespace.Models
Imports Param_RootNamespace.Core.Services
Imports Param_RootNamespace.Core.Helpers
'}]}
Namespace Views
    Public NotInheritable Partial Class ShellPage
        Inherits Page
        Implements INotifyPropertyChanged

        Private _selected As WinUI.NavigationViewItem
'{[{
        Private _user As UserData
        Private _isBusy As Boolean
        Private _isLoggedIn As Boolean
        Private _isAuthorized As Boolean

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

        Public Property IsBusy As Boolean
            Get
                Return _isBusy
            End Get
            Set(value As Boolean)
                [Set](_isBusy, value)
            End Set
        End Property

        Public Property IsLoggedIn As Boolean
            Get
                Return _isLoggedIn
            End Get
            Set(value As Boolean)
                [Set](_isLoggedIn, value)
            End Set
        End Property

        Public Property IsAuthorized As Boolean
            Get
                Return _isAuthorized
            End Get
            Set(value As Boolean)
                [Set](_isAuthorized, value)
            End Set
        End Property
'}]}

        Public Sub New()
        End Sub

        Private Sub Initialize()
'^^
'{[{
            AddHandler IdentityService.LoggedIn, AddressOf OnLoggedIn
            AddHandler IdentityService.LoggedOut, AddressOf OnLoggedOut
            AddHandler UserDataService.UserDataUpdated, AddressOf OnUserDataUpdated
'}]}
        End Sub

        Private Async Sub OnLoaded(sender As Object, e As RoutedEventArgs)
'^^
'{[{
            IsLoggedIn = IdentityService.IsLoggedIn()
            IsAuthorized = IsLoggedIn AndAlso IdentityService.IsAuthorized()
            User = Await UserDataService.GetUserAsync()
'}]}
        End Sub

'{[{
        Private Sub OnUserDataUpdated(sender As Object, userData As UserData)
            User = userData
        End Sub

        Private Sub OnLoggedIn(sender As Object, e As EventArgs)
            IsLoggedIn = True
            IsAuthorized = IsLoggedIn AndAlso IdentityService.IsAuthorized()
            IsBusy = False
        End Sub

        Private Sub OnLoggedOut(sender As Object, e As EventArgs)
            User = Nothing
            IsLoggedIn = False
            IsAuthorized = False
            CleanRestrictedPagesFromNavigationHistory()
            GoBackToLastUnrestrictedPage()
        End Sub

        Private Sub CleanRestrictedPagesFromNavigationHistory()
            NavigationService.Frame.BackStack.
                Where(Function(backStackEntry) Attribute.IsDefined(backStackEntry.SourcePageType, GetType(Restricted))).
                ToList().
                ForEach(Sub(restricted) NavigationService.Frame.BackStack.Remove(restricted))
        End Sub

        Private Sub GoBackToLastUnrestrictedPage()
            Dim currentPage = TryCast(NavigationService.Frame.Content, Page)
            Dim isCurrentPageRestricted = Attribute.IsDefined(currentPage.[GetType](), GetType(Restricted))

            If isCurrentPageRestricted Then
                NavigationService.GoBack()
            End If
        End Sub

        Private Async Sub OnUserProfile(sender As Object, e As RoutedEventArgs)
            If IsLoggedIn Then
                NavigationService.Navigate(Of SettingsPage)()
            Else
                IsBusy = True
                Dim loginResult = Await IdentityService.LoginAsync()

                If loginResult <> LoginResultType.Success Then
                    Await AuthenticationHelper.ShowLoginErrorAsync(loginResult)
                    IsBusy = False
                End If
            End If
        End Sub
'}]}
    End Class
End Namespace
