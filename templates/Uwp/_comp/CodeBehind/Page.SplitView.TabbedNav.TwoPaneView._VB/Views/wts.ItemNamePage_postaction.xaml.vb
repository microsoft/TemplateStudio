'{[{
Imports Param_RootNamespace.Helpers
'}]}

Namespace Views
    Public NotInheritable Partial Class wts.ItemNamePage
        Inherits Page
'{[{
        Implements IBackNavigationHandler
'}]}

'^^
'{[{
        Public Event OnPageCanGoBackChanged As EventHandler(Of Boolean) Implements IBackNavigationHandler.OnPageCanGoBackChanged

'}]}
        Public Property Selected As SampleOrder

        Private Sub OnItemClick(sender As Object, e As ItemClickEventArgs)
            If twoPaneView.Mode = WinUI.TwoPaneViewMode.SinglePane Then
'^^
'{[{
                RaiseEvent OnPageCanGoBackChanged(Me, True)
'}]}
                TwoPanePriority = WinUI.TwoPaneViewPriority.Pane2
            End If
        End Sub

        Private Sub OnModeChanged(sender As WinUI.TwoPaneView, args As Object)
            If twoPaneView.Mode = WinUI.TwoPaneViewMode.SinglePane Then
'^^
'{[{
                RaiseEvent OnPageCanGoBackChanged(Me, True)
'}]}
                TwoPanePriority = WinUI.TwoPaneViewPriority.Pane2
            Else
'^^
'{[{
                RaiseEvent OnPageCanGoBackChanged(Me, False)
'}]}
                TwoPanePriority = WinUI.TwoPaneViewPriority.Pane1
            End If
        End Sub

'^^
'{[{
        Public Sub GoBack() Implements IBackNavigationHandler.GoBack
            If TwoPanePriority = WinUI.TwoPaneViewPriority.Pane2 Then
                TwoPanePriority = WinUI.TwoPaneViewPriority.Pane1
                RaiseEvent OnPageCanGoBackChanged(Me, False)
            End If
        End Sub
'}]}

        Public Event PropertyChanged As PropertyChangedEventHandler Implements INotifyPropertyChanged.PropertyChanged
    End Class
End Namespace
