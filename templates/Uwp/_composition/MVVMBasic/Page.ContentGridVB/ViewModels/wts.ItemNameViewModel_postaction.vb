'{[{
Imports Param_RootNamespace.Helpers
Imports Param_RootNamespace.Views
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
