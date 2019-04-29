'{**
'This code block adds the logic to handle SettingsItem in NavigationView control from ShellPage code behind.
'**}

Namespace Views
    Public NotInheritable Partial Class ShellPage
        Inherits Page
        Implements INotifyPropertyChanged

        Public Sub Frame_Navigated(sender As Object, e As NavigationEventArgs)
            IsBackEnabled = NavigationService.CanGoBack
'{[{
            If e.SourcePageType = GetType(wts.ItemNamePage) Then
                Selected = TryCast(navigationView.SettingsItem, WinUI.NavigationViewItem)
                Return
            End If

'}]}
        End Sub

        Private Sub OnItemInvoked(sender As WinUI.NavigationView, args As WinUI.NavigationViewItemInvokedEventArgs)
'{[{
            If args.IsSettingsInvoked Then
                NavigationService.Navigate(GetType(wts.ItemNamePage))
                Return
            End If

'}]}
        End Sub
    End Class
End Namespace
