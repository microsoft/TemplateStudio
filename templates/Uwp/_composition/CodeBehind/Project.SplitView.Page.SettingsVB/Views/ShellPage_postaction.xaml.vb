'{**
'This code block adds the logic to handle SettingsItem in NavigationView control from ShellPage code behind.
'**}

Namespace Views
    Partial Public NotInheritable Class ShellPage
        Inherits Page
        Implements INotifyPropertyChanged

        Private Sub Frame_Navigated(sender As Object, e As NavigationEventArgs)
            '{[{
            If e.SourcePageType = GetType(wts.ItemNamePage) Then
                Selected = TryCast(navigationView.SettingsItem, NavigationViewItem)
                Return
            End If

            '}]}
        End Sub

        Private Sub OnItemInvoked(sender As NavigationView, args As NavigationViewItemInvokedEventArgs)
            '{[{
            If args.IsSettingsInvoked Then
                NavigationService.Navigate(GetType(wts.ItemNamePage))
                Return
            End If

            '}]}
        End Sub
    End Class
End Namespace
