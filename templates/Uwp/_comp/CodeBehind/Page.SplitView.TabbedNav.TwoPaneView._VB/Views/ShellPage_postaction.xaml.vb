Namespace Views
    Public NotInheritable Partial Class ShellPage
        Inherits Page
        Implements INotifyPropertyChanged

        Private Sub Initialize()
'^^
'{[{
            AddHandler NavigationService.OnCurrentPageCanGoBackChanged, AddressOf OnCurrentPageCanGoBackChanged
'}]}
            AddHandler navigationView.BackRequested, AddressOf OnBackRequested
        End Sub

'^^
'{[{
        Private Sub OnCurrentPageCanGoBackChanged(sender As Object, currentPageCanGoBack As Boolean)
            IsBackEnabled = NavigationService.CanGoBack OrElse currentPageCanGoBack
        End Sub

'}]}
        Public Sub Frame_Navigated(sender As Object, e As NavigationEventArgs)
        End Sub
    End Class
End Namespace
