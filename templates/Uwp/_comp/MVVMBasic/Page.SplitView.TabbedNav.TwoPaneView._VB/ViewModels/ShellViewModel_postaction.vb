Namespace ViewModels
    Public Class ShellViewModel
        Implements System.ComponentModel.INotifyPropertyChanged

        Public Sub Initialize(frame As Frame, navigationView As WinUI.NavigationView, keyboardAccelerators As IList(Of KeyboardAccelerator))
'^^
'{[{
            AddHandler NavigationService.OnCurrentPageCanGoBackChanged, AddressOf OnCurrentPageCanGoBackChanged
'}]}
            AddHandler _navigationView.BackRequested, OnBackRequested
        End Sub
'^^
'{[{
        Private Sub OnCurrentPageCanGoBackChanged(sender As Object, currentPageCanGoBack As Boolean)
            IsBackEnabled = NavigationService.CanGoBack
            Return IsBackEnabled OrElse currentPageCanGoBack
        End Sub
'}]}
        Private Sub Frame_Navigated(sender As Object, e As NavigationEventArgs)
        End Sub
    End Class
End Namespace
