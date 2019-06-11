'{[{
Imports Param_RootNamespace.Models
Imports Param_RootNamespace.Core.Services
Imports Param_RootNamespace.Core.Helpers
'}]}
Namespace Views
    Public NotInheritable Partial Class ShellPage
        Inherits Page
        Implements INotifyPropertyChanged
'^^
'{[{
        Private _isLoggedIn As Boolean
        Private _isAuthorized As Boolean

        Private ReadOnly Property IdentityService As IdentityService
            Get
                Return Singleton(Of IdentityService).Instance
            End Get
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
'^^
'{[{
            AddHandler IdentityService.LoggedIn, AddressOf OnLoggedIn
            AddHandler IdentityService.LoggedOut, AddressOf OnLoggedOut
'}]}
        End Sub

        Private Async Sub OnLoaded(sender As Object, e As RoutedEventArgs)
'^^
'{[{
            IsLoggedIn = IdentityService.IsLoggedIn()
            IsAuthorized = IsLoggedIn AndAlso IdentityService.IsAuthorized()
'}]}
        End Sub
'{[{

        Private Sub OnLoggedIn(sender As Object, e As EventArgs)
            IsLoggedIn = True
            IsAuthorized = IsLoggedIn AndAlso IdentityService.IsAuthorized()
        End Sub

        Private Sub OnLoggedOut(sender As Object, e As EventArgs)
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
                If NavigationService.CanGoBack Then
                    NavigationService.GoBack()
                Else
                    MenuNavigationHelper.UpdateView(GetType(Param_HomeNamePage))
                End If
            End If
        End Sub
'}]}
    End Class
End Namespace
