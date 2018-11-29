'{[{
Imports Param_ItemNamespace.Helpers
Imports Param_ItemNamespace.Views
'}]}

Namespace ViewModels
    Public Class wts.ItemNameViewModel

        Private Sub OnsItemSelected(args As ItemClickEventArgs)
            If item IsNot Nothing Then
'^^
'{[{
                NavigationService.Navigate(Of ContentGridDetailPage)(item.OrderId)
'}]}
            End If
        End Sub
    End Class
End Namespace
