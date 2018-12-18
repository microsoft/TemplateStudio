'{[{
Imports Param_ItemNamespace.Helpers
Imports Param_ItemNamespace.Views
'}]}

Namespace ViewModels
    Public Class wts.ItemNameViewModel

        Private Sub OnItemClick(clickedItem As SampleOrder)
            If clickedItem IsNot Nothing Then
'^^
'{[{
                NavigationService.Navigate(Of wts.ItemNameDetailPage)(clickedItem.OrderId)
'}]}
            End If
        End Sub
    End Class
End Namespace
