'{[{
Imports Param_RootNamespace.Core.Services
Imports Param_RootNamespace.Core.Helpers
'}]}

Namespace ViewModels
    Public Class ShellViewModel
        Inherits ObservableObject

        Private _itemInvokedCommand As ICommand
'{[{
        Private _userProfileCommand As RelayCommand
        Private _user As UserViewModel
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
        Public ReadOnly Property UserProfileCommand As RelayCommand
            Get
                If _userProfileCommand Is Nothing Then
                    _userProfileCommand = New RelayCommand(AddressOf OnUserProfile, Function() Not IsBusy)
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

        Public Property IsBusy As Boolean
            Get
                Return _isBusy
            End Get
            Set(value As Boolean)
                SetProperty(_isBusy, value)
                UserProfileCommand.NotifyCanExecuteChanged()
            End Set
        End Property

        Public Property IsLoggedIn As Boolean
            Get
                Return _isLoggedIn
            End Get
            Set(value As Boolean)
                SetProperty(_isLoggedIn, value)
            End Set
        End Property

        Public Property IsAuthorized As Boolean
            Get
                Return _isAuthorized
            End Get
            Set(value As Boolean)
                SetProperty(_isAuthorized, value)
            End Set
        End Property
'}]}

        Public Sub New()
        End Sub

        Public Sub Initialize(frame As Frame, navigationView As WinUI.NavigationView, keyboardAccelerators As IList(Of KeyboardAccelerator))
'^^
'{[{
            AddHandler IdentityService.LoggedIn, AddressOf OnLoggedIn
            AddHandler IdentityService.LoggedOut, AddressOf OnLoggedOut
            AddHandler UserDataService.UserDataUpdated, AddressOf OnUserDataUpdated
'}]}
        End Sub

        Private Async Sub OnLoaded()
'^^
'{[{
            IsLoggedIn = IdentityService.IsLoggedIn()
            IsAuthorized = IsLoggedIn AndAlso IdentityService.IsAuthorized()
            User = Await UserDataService.GetUserAsync()
'}]}
        End Sub

'{[{
        Private Sub OnUserDataUpdated(sender As Object, userData As UserViewModel)
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

        Private Async Sub OnUserProfile()
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
