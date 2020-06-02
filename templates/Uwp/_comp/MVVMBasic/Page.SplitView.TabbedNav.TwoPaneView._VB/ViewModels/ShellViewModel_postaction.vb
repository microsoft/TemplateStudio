Namespace ViewModels
    Public Class ShellViewModel
        Implements System.ComponentModel.INotifyPropertyChanged

        Public Sub Initialize(ByVal frame As Frame, ByVal navigationView As WinUI.NavigationView, ByVal keyboardAccelerators As IList(Of KeyboardAccelerator))
'^^
'{[{
            AddHandler NavigationService.OnCurrentPageCanGoBackChanged, AddressOf OnCurrentPageCanGoBackChanged
'}]}
            AddHandler _navigationView.BackRequested, OnBackRequested
        End Sub
'^^
'{[{
        Private Sub OnCurrentPageCanGoBackChanged(ByVal sender As Object, ByVal currentPageCanGoBack As Boolean)
            IsBackEnabled = NavigationService.CanGoBack
            Return IsBackEnabled OrElse currentPageCanGoBack
        End Sub
'}]}
        Private Sub Frame_Navigated(ByVal sender As Object, ByVal e As NavigationEventArgs)
        End Sub
    End Class
End Namespace
