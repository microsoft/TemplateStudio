Namespace ViewModels
    Public Class wts.ItemNameViewModel
        Inherits Observable
        Private Sub OnItemClick(args As ItemClickEventArgs)
            '{[{
            Dim item As SampleOrder = TryCast(args?.ClickedItem, SampleOrder)
            If item IsNot Nothing Then
                If _currentState.Name = NarrowStateName Then
                    NavigationService.Navigate(Of Views.wts.ItemNameDetailPage)(item)
                Else
                    Selected = item
                End If
            End If
            '}]}
        End Sub
    End Class
End Namespace
