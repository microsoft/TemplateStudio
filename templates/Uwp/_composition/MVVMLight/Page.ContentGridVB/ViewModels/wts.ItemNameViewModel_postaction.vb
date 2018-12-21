'{[{
Imports GalaSoft.MvvmLight.Command
'}]}

Namespace ViewModels
    Public Class wts.ItemNameViewModel
        Inherits ViewModelBase
'{[{

        Public ReadOnly Property NavigationService As NavigationServiceEx
            Get
                Return ViewModelLocator.Current.NavigationService
            End Get
        End Property
'}]}

        Private Sub OnItemClick(clickedItem As SampleOrder)
            If clickedItem IsNot Nothing Then
'^^
'{[{
                NavigationService.Navigate(GetType(wts.ItemNameDetailViewModel).FullName, clickedItem.OrderId)
'}]}
            End If
        End Sub
    End Class
End Namespace
