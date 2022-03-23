'{[{
Imports Param_RootNamespace.Helpers
Imports Microsoft.Toolkit.Mvvm.Input
'}]}

Namespace ViewModels
    Public Class wts.ItemNameViewModel
        Inherits ObservableObject
'{[{
        Implements IBackNavigationHandler
'}]}

        Private _twoPaneView As WinUI.TwoPaneView
'^^
'{[{
        Public Event OnPageCanGoBackChanged As EventHandler(Of Boolean) Implements IBackNavigationHandler.OnPageCanGoBackChanged

'}]}
        Public Property Selected As SampleOrder

        Private Sub OnItemClick()
            If _twoPaneView.Mode = WinUI.TwoPaneViewMode.SinglePane Then
'^^
'{[{
                RaiseEvent OnPageCanGoBackChanged(Me, True)
'}]}
                TwoPanePriority = WinUI.TwoPaneViewPriority.Pane2
            End If
        End Sub

        Private Sub OnModeChanged(twoPaneView As WinUI.TwoPaneView)
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
    End Class
End Namespace
