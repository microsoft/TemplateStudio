Namespace ViewModels
    Public Class ShellViewModel
        Inherits System.ComponentModel.INotifyPropertyChanged

        Public Sub Initialize(frame As Frame, navigationView As WinUI.NavigationView, keyboardAccelerators As IList(Of KeyboardAccelerator))
'^^
'{[{
            AddHandler NavigationService.OnCurrentPageCanGoBackChanged, AddressOf OnCurrentPageCanGoBackChanged
'}]}
            AddHandler _navigationView.BackRequested, AddressOf OnBackRequested
        End Sub
'^^
'{[{
        Private Sub OnCurrentPageCanGoBackChanged(ByVal sender As Object, ByVal currentPageCanGoBack As Boolean)
            IsBackEnabled = NavigationService.CanGoBack OrElse currentPageCanGoBack
        End Sub

'}]}
    End Class
End Namespace
