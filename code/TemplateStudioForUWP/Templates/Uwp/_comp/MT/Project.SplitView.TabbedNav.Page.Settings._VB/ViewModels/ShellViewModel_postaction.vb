'{**
'This code block adds the logic to handle SettingsItem in NavigationView control from ViewModel.
'**}
'{[{
Imports Param_RootNamespace.Views
'}]}

Namespace ViewModels
    Public Class ShellViewModel
        Inherits ObservableObject

        Private Sub OnItemInvoked(args As WinUI.NavigationViewItemInvokedEventArgs)
            If args.IsSettingsInvoked Then
'{--{
                ' Navigate to the settings page - implement as appropriate if needed
'}--}
                '{[{
                NavigationService.Navigate(GetType(wts.ItemNamePage), Nothing, args.RecommendedNavigationTransitionInfo)
                '}]}
            End If
        End Sub

        Public Sub Frame_Navigated(sender As Object, e As NavigationEventArgs)
            IsBackEnabled = NavigationService.CanGoBack
            '{[{
            If e.SourcePageType = GetType(wts.ItemNamePage) Then
                Selected = TryCast(_navigationView.SettingsItem, WinUI.NavigationViewItem)
                Return
            End If

            '}]}
        End Sub
    End Class
End Namespace
